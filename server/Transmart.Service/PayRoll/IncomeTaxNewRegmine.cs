using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Domain.Entities.Payroll;

namespace TranSmart.Service.PayRoll
{
	public class IncomeTaxNewRegmine : IncomeTax
	{
		public IncomeTaxNewRegmine(IUnitOfWork uow, IEnumerable<NewRegimeSlab> slabs) : base(uow)
		{
			Slabs = slabs.Select(x => new TaxBracket { Low = x.IncomeFrom, High = x.IncomeTo, Rate = x.TaxRate }).ToList();
		}
		public override async Task Calculate(Declaration declaration)
		{
			declaration.Allowance = 0;
			declaration.StandardDeduction = 50000;
			declaration.HouseIncome = 0;
			declaration.OtherIncome = await OtherIncome(declaration.ID);


			declaration.EPF = 0;

			declaration.EightyC = 0;
			declaration.EightyD = 0;
			declaration.OtherSections = 0;

			declaration.TotalSalary = Math.Max(declaration.Salary + declaration.PreviousEmployment - declaration.StandardDeduction, 0);
			declaration.Balance = declaration.TotalSalary;
			declaration.Deductions = 0;
			declaration.IncomeChargeable = declaration.TotalSalary;
			declaration.GrossTotal = declaration.IncomeChargeable + declaration.OtherIncome;
			declaration.Taxable = declaration.GrossTotal;

			declaration.Tax = TaxCalculator(declaration.Taxable, Slabs.ToArray());
		}
	}
}
