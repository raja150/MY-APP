using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Domain.Entities.Payroll;

namespace TranSmart.Service.PayRoll
{
	public interface IIncomeTaxCalculator
	{
		Task Calculate(Salary salaryItem, List<PaySheet> paySheets,
			Declaration declaration, DeclarationSetting settings,
			Guid EmployeeWorkStateId, int paidMonths, bool doCalculate);

		Task<int> GetPTaxPredict(decimal grossTaxable, Guid stateId);
	}
	public class IncomeTaxCalculator: IIncomeTaxCalculator
	{
		private readonly IUnitOfWork _uow;
		private readonly FinancialYear _financialYear;
		//public IUnitOfWork UOW { get { return _uow; } }
		public IncomeTaxCalculator(IUnitOfWork uow, FinancialYear financialYear)
		{
			_uow = uow;
			_financialYear = financialYear;
		}
		public virtual async Task Calculate(Salary salaryItem, List<PaySheet> paySheets,
			Declaration declaration, DeclarationSetting settings,
			Guid EmployeeWorkStateId, int paidMonths, bool doCalculate)
		{
			if (declaration == null) return;

			Guid declarationId = declaration.ID;

			var dueMonths = Math.Max(12 - paidMonths, 0);

			PrevEmployment prevEmployment = await _uow.GetRepositoryAsync<PrevEmployment>().SingleAsync(x => x.DeclarationId == declarationId) ??
											new PrevEmployment { IncomeAfterException = 0, };

			var bonus = await _uow.GetRepositoryAsync<EmpBonus>().SumOfIntAsync(x => x.EmployeeId == declaration.EmployeeId
						&& x.ReleasedOn > _financialYear.FromDate && x.ReleasedOn < _financialYear.ToDate,
						sumBy: x => x.Amount);
			//Salary as per provision
			var employeeSalary = paySheets.Sum(x => x.GrossTaxable) + (salaryItem.Monthly * dueMonths) + bonus;

			//When force calculation not a mandatory
			//This is to check when there is a difference in salary. So, to avoiding declaration update for every salary process.
			if (declaration.Salary == employeeSalary && !doCalculate) return;

			declaration.Salary = employeeSalary;
			//Perquisites 
			declaration.Perquisites = 0;
			declaration.PreviousEmployment = prevEmployment.IncomeAfterException;

			int pTax = await GetPTaxPredict(salaryItem.Monthly, EmployeeWorkStateId);
			declaration.TaxOnEmployment = prevEmployment.ProfessionalTax + paySheets.Sum(x => x.PTax) + (pTax * dueMonths);
			IncomeTax incomeTax;
			if (declaration.IsNewRegime)
			{
				var _slabs = await _uow.GetRepositoryAsync<NewRegimeSlab>().GetAsync(x => x.PaySettingsId == settings.PaySettingsId);
				incomeTax = new IncomeTaxNewRegmine(_uow, _slabs);
			}
			else
			{
				var _slabs = await _uow.GetRepositoryAsync<OldRegimeSlab>().GetAsync(x => x.PaySettingsId == settings.PaySettingsId);
				incomeTax = new IncomeTaxOldRegmine(_uow, settings, salaryItem, dueMonths, paySheets, prevEmployment, _slabs);
			}
			//Calculate based on the regime

			await incomeTax.Calculate(declaration);

			declaration.Relief = 0;
			if(declaration.IsNewRegime && declaration.Tax <= 25000)
			{
				declaration.Relief = declaration.Tax;
			}
			else if (!declaration.IsNewRegime && declaration.Tax <= 12500)
			{
				declaration.Relief = declaration.Tax;
			}
			declaration.Cess = (int)Math.Round((declaration.Tax - declaration.Relief) * (settings.EducationCess * 0.01m), 0);
			declaration.TaxPayable = declaration.Tax - declaration.Relief + declaration.Cess;
			declaration.TaxPaid = paySheets.Sum(x => x.Tax) + prevEmployment.IncomeTax;
			declaration.Due = declaration.TaxPayable - declaration.TaxPaid;
		}

		public async Task<int> GetPTaxPredict(decimal grossTaxable, Guid stateId)
		{
			var pTaxSlab = await _uow.GetRepositoryAsync<ProfessionalTaxSlab>().SingleAsync(x => x.ProfessionalTax.StateId == stateId
						&& (x.From <= grossTaxable && x.To >= grossTaxable));
			return pTaxSlab?.Amount ?? 0;
		}
	}
}
