using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Extension;
using TranSmart.Data;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Enums;

namespace TranSmart.Service.PayRoll
{
	public class IncomeTaxOldRegmine : IncomeTax
	{
		private readonly DeclarationSetting _settings;
		private readonly Salary _salary;
		private readonly int _dueMonths;
		private readonly List<PaySheet> _paySheet;
		private readonly PrevEmployment _prevEmployment;

		public IncomeTaxOldRegmine(IUnitOfWork uow, DeclarationSetting settings, Salary salary,
			int dueMonths, List<PaySheet> paySheet, PrevEmployment prevEmployment,
			IEnumerable<OldRegimeSlab> slabs) : base(uow)
		{
			_settings = settings;
			_salary = salary;
			_dueMonths = dueMonths;
			_paySheet = paySheet;
			_prevEmployment = prevEmployment;
			Slabs = slabs.Select(x => new TaxBracket { Low = x.IncomeFrom, High = x.IncomeTo, Rate = x.TaxRate }).ToList();
		}

		public override async Task Calculate(Declaration declaration)
		{
			declaration.Allowance = await AllowancesCalc(_salary.Earnings, declaration.ID, _dueMonths, _paySheet);

			declaration.StandardDeduction = 50000;

			//int pTax = await GetPTaxPredict(_salary.Monthly, EmployeeWorkStateId);
			//declaration.TaxOnEmployment = _prevEmployment.ProfessionalTax + _paySheet.Sum(x => x.PTax) + (pTax * _dueMonths);

			var incomeFromHouse = await HomeLoanInterest(declaration.ID);
			var incomeFromHouseLetOut = await IncomeFromLetOutProperty(declaration.ID);
			declaration.HouseIncome = IncomeFromHome(incomeFromHouse.Item2, incomeFromHouseLetOut.Item2);
			declaration.HomeLoanPrincipal = incomeFromHouseLetOut.Item1 + incomeFromHouse.Item1;
			declaration.OtherIncome = await OtherIncome(declaration.ID);

			int epfPaid = _paySheet.Sum(x => x.EPF);
			//Calculate average EPF for due months 

			var epfOrg = await UOW.GetRepositoryAsync<EPF>().SingleAsync(x => x.PaySettingsId == _settings.PaySettingsId);
			var earningComponent = await UOW.GetRepositoryAsync<EarningComponent>().GetAsync();
			var empStatutory = await UOW.GetRepositoryAsync<EmpStatutory>().SingleAsync(x => x.EmpId == declaration.EmployeeId);

			//Add Previous employment, paid till now and future amount   
			declaration.EPF = _prevEmployment.ProvisionalFund + epfPaid +
				(ProvidentFundCalc.ForFormSixteen(epfOrg, earningComponent, _salary.Earnings, empStatutory).Item1) * _dueMonths;

			int eightyC = await VIEightyCQualifiedFromDeductions(_salary.Deductions, declaration.ID, _dueMonths, _paySheet);
			declaration.EightyC = await VIEightyCQualified(_settings.MaxLimitEightyC, eightyC, declaration.EPF, declaration.HomeLoanPrincipal, declaration.ID);
			declaration.EightyD = await VIEightyDQualified(_settings.MaxLimitEightyD, declaration.ID);
			declaration.OtherSections = await VIOtherSections(declaration.ID);

			declaration.TotalSalary = declaration.Salary + declaration.Perquisites + declaration.PreviousEmployment;
			declaration.Balance = Math.Max(0, declaration.TotalSalary - declaration.Allowance);
			declaration.Deductions = declaration.StandardDeduction + declaration.TaxOnEmployment;
			declaration.IncomeChargeable = Math.Max(0, declaration.Balance - declaration.Deductions);
			declaration.GrossTotal = declaration.IncomeChargeable + declaration.HouseIncome + declaration.OtherIncome;
			declaration.Taxable = Math.Max(0, declaration.GrossTotal - (declaration.EightyC + declaration.EightyD + declaration.OtherSections));

			declaration.Tax = TaxCalculator(declaration.Taxable, Slabs.ToArray());
		}

		public int IncomeFromHome(int incomeFromHouse, int incomeFromHouseLetOut)
		{
			return Math.Max(_settings.HouseLoanInt * -1, incomeFromHouse + incomeFromHouseLetOut);
		}
		#region Internal calculations


		public async Task<int> VIOtherSections(Guid declarationId)
		{
			var sectionOthers = await UOW.GetRepositoryAsync<Section6AOther>().GetAsync(x => x.DeclarationId == declarationId);
			return sectionOthers.Sum(x => x.Qualified);
		}

		public async Task<int> VIEightyDQualified(int MaxLimit, Guid declarationId)
		{
			var eightyd = await UOW.GetRepositoryAsync<Section6A80D>().GetAsync(x => x.DeclarationId == declarationId, include: x => x.Include(o => o.Section80D));
			return Math.Min(MaxLimit, eightyd.Sum(x => Math.Min(x.Qualified, x.Section80D.Limit)));
		}

		public async Task<int> VIEightyCQualified(int MaxLimit, int FromWages, int EPF, int homeLoanPrincipal, Guid declarationId)
		{
			var eightyc = await UOW.GetRepositoryAsync<Section6A80C>().SumOfIntAsync(x => x.DeclarationId == declarationId, x => x.Amount);
			return Math.Min(MaxLimit, FromWages + eightyc + EPF + homeLoanPrincipal);
		}


		public async Task<Tuple<int, int>> IncomeFromLetOutProperty(Guid declarationId)
		{
			int incomeFromHouseLetOut = 0;
			int principle = 0;
			var letOutHouses = await UOW.GetRepositoryAsync<LetOutProperty>().GetAsync(x => x.DeclarationId == declarationId);
			foreach (LetOutProperty letoutItem in letOutHouses)
			{
				int netValue = letoutItem.AnnualRentReceived - letoutItem.MunicipalTaxPaid;
				incomeFromHouseLetOut += (netValue - (int)Math.Round(netValue * 0.30, 0)) - letoutItem.InterestPaid;
				principle += letoutItem.Principle;
			}

			return new Tuple<int, int>(principle, incomeFromHouseLetOut);
		}

		public async Task<Tuple<int, int>> HomeLoanInterest(Guid declarationId)
		{
			HomeLoanPay homeLoanPay = await UOW.GetRepositoryAsync<HomeLoanPay>().SingleAsync(x => x.DeclarationId == declarationId);
			if (homeLoanPay != null)
			{
				return new Tuple<int, int>(homeLoanPay.Principle, Math.Min(homeLoanPay.InterestPaid, _settings.HouseLoanInt) * -1);
			}

			return new Tuple<int, int>(0, 0);
		}
		//Issue in getting Data
		public async Task<int> VIEightyCQualifiedFromDeductions(ICollection<SalaryDeduction> SalaryDeductions,
			Guid declarationId, int dueMonths, List<PaySheet> paySheet)
		{
			var list = new List<Section6A80CWages>();
			foreach (SalaryDeduction item in SalaryDeductions)
			{
				var paid = paySheet.SelectMany(x => x.Deductions.Where(x => x.DeductType == (int)DeductionType.Pre)).Sum(x => x.Deduction);
				int due = SalaryDeductions.Where(x => x.Deduction.Deduct == (int)DeductionType.Pre).Sum(x => x.Monthly * dueMonths);
				list.Add(new Section6A80CWages { ComponentId = item.DeductionId, DeclarationId = declarationId, Amount = paid + due });
			}
			var entity = await UOW.GetRepositoryAsync<Section6A80CWages>().GetAsync(x => x.DeclarationId == declarationId);

			var result = entity.Compare(list, (x, y) => x.ComponentId == y.ComponentId);
			foreach (Section6A80CWages dItem in result.Same)
			{
				Section6A80CWages edit = entity.FirstOrDefault(x => x.ComponentId == dItem.ComponentId);
				if (edit != null && !edit.Equals(dItem))
				{
					edit.Update(dItem);
					UOW.GetRepositoryAsync<Section6A80CWages>().UpdateAsync(edit);
				}
			}

			foreach (Section6A80CWages dItem in result.Added)
			{
				dItem.DeclarationId = declarationId;
				await UOW.GetRepositoryAsync<Section6A80CWages>().AddAsync(dItem);
			}

			foreach (Section6A80CWages dItem in result.Deleted)
			{
				UOW.GetRepositoryAsync<Section6A80CWages>().DeleteAsync(dItem);
			}
			return list.Sum(x => x.Amount);
		}
		public async Task<int> AllowancesCalc(ICollection<SalaryEarning> SalaryEarnings,
		Guid declarationId, int dueMonths, IEnumerable<PaySheet> paySheet)
		{
			//Allowance to the extent exempt
			var hraLines = await UOW.GetRepositoryAsync<HraDeclaration>().GetAsync(x => x.DeclarationId == declarationId);
			var allowances = new List<DeclarationAllowance>();
			if (hraLines.Any())
			{
				//Basic Paid
				var basicDaPaid = paySheet.SelectMany(y => y.Earnings.Where(x => x.EarningType == (int)EarningType.Basic)).Sum(x => x.Earning);
				//HRA Paid
				var hraPaid = paySheet.SelectMany(y => y.Earnings.Where(x => x.EarningType == (int)EarningType.HRA)).Sum(x => x.Earning);

				int basicDaDue = SalaryEarnings.Where(x => x.Component.EarningType == (int)EarningType.Basic)
					.Sum(x => x.Monthly * dueMonths);

				int hraDue = SalaryEarnings.Where(x => x.Component.EarningType == (int)EarningType.HRA)
					.Sum(x => x.Monthly * dueMonths);

				//1 Actual HRA receive
				int hraReceived = (hraPaid + hraDue);
				//2 50% of Basic
				decimal hraByBasic = Math.Round((basicDaDue + basicDaPaid) * (decimal)(0.5), 2);
				//3 Actual Rent paid by employee
				decimal actualRentPaid = Math.Round(hraLines.Sum(x => x.Total) - ((basicDaDue + basicDaPaid) * (decimal)(0.1)), 0);
				// Min of 1, 2, 3
				int hraConsidered = (int)Math.Min(hraReceived, Math.Min(hraByBasic, actualRentPaid));

				SalaryEarning hraComponent = SalaryEarnings.FirstOrDefault(x => x.Component.EarningType == (int)EarningType.HRA);
				if (hraComponent != null)
				{
					allowances.Add(new DeclarationAllowance
					{
						DeclarationId = declarationId,
						ComponentId = hraComponent.ComponentId,
						Amount = hraConsidered > 0 ? hraConsidered : 0,
						Name = hraComponent.Component.Name
					});
				}
			}

			List<SalaryEarning> allowanceComponents = SalaryEarnings.Where(x => x.Component.EarningType == (int)EarningType.FoodCoupon || x.Component.EarningType == (int)EarningType.ConveyanceAllowance || x.Component.EarningType == (int)EarningType.SodexoMultiBenefit).ToList();
			foreach (SalaryEarning allowItem in allowanceComponents)
			{
				int allowDue = SalaryEarnings.Where(x => x.ComponentId == allowItem.ComponentId).Sum(x => x.Monthly * dueMonths);

				int allowPaid = paySheet.SelectMany(x => x.Earnings.Where(x => x.ComponentId == allowItem.ComponentId)).Sum(x => x.Earning);
				allowances.Add(new DeclarationAllowance
				{
					DeclarationId = declarationId,
					ComponentId = allowItem.ComponentId,
					Amount = allowDue + allowPaid,
					Name = allowItem.Component.Name
				});
			}

			var allowancesEntity = await UOW.GetRepositoryAsync<DeclarationAllowance>().GetAsync(x => x.DeclarationId == declarationId);

			#region Allowance
			//Update
			IEnumerable<DeclarationAllowance> intersectLet = allowances.Intersection(allowancesEntity, (x, y) => x.ComponentId == y.ComponentId);
			foreach (DeclarationAllowance dItem in intersectLet)
			{
				DeclarationAllowance edit = allowancesEntity.FirstOrDefault(x => x.ComponentId == dItem.ComponentId);
				if (edit != null && !edit.Equals(dItem))
				{
					edit.Update(dItem);
					UOW.GetRepositoryAsync<DeclarationAllowance>().UpdateAsync(edit);
				}
			}

			//add
			var newLet = allowances.Exclude(allowancesEntity, (x, y) => x.ComponentId == y.ComponentId);
			foreach (DeclarationAllowance dItem in newLet)
			{
				dItem.DeclarationId = declarationId;
				await UOW.GetRepositoryAsync<DeclarationAllowance>().AddAsync(dItem);
			}

			//Delete
			var removedLet = allowancesEntity.Exclude(allowances, (x, y) => x.ComponentId == y.ComponentId);
			foreach (DeclarationAllowance dItem in removedLet)
			{
				UOW.GetRepositoryAsync<DeclarationAllowance>().DeleteAsync(dItem);
			}
			#endregion

			return allowances.Sum(x => x.Amount);
		}

		#endregion
	}
}
