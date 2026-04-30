using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Domain.Entities.Payroll;

namespace Transmart.Services.UnitTests.Services.Payroll.IntigrationTest
{
	public class PaySheetDataModel
	{
		public Guid EmployeeId { get; set; }
		public string Code { get; set; }
		public DateTime DOJ { get; set; }
		public DateTime? DOR { get; set; }
		public int Salary { get; set; }
		public int ActualBasic { get; set; }
		public int ActualHRA { get; set; }
		public int ActualMedicalTransport { get; set; }
		public int ActualFoodCoupons { get; set; }
		public int ActualSpecialAllowance { get; set; }
		public DateTime Month { get; set; }
		public int MonthDays { get; set; }
		public int EmployeeWorkingDays { get; set; }
		public int DaysPresent { get; set; }
		public int LOPDays { get; set; }
		public int LateComingDays { get; set; }
		public int UnauthorizedLeaves { get; set; }
		public int EarnedBasic { get; set; }
		public int EarnedHRA { get; set; }
		public int EarnedMedicalTransport { get; set; }
		public int EarnedFoodCoupons { get; set; }
		public int EarnedSpecialAllowance { get; set; }
		public int Arrears { get; set; }
		public int Incentives { get; set; }
		public int GrossEarningtotal { get; set; }
		public int LOPAmount { get; set; }
		public int UnauthorizedAmount { get; set; }
		public int LateComingAmount { get; set; }
		public int StaffSalaryAdvances { get; set; }
		public int Paycut { get; set; }
		public int NetSalaryEarned { get; set; }
		public int PT { get; set; }
		public int IncomeTax { get; set; }
		public int PFContribution { get; set; }
		public int ESIContribution { get; set; }
		public int ESIFinal { get; set; }
		public int GrossEarnings { get; set; }
		public int GrossDeduction { get; set; }
		public int NetTakeHome { get; set; }
		public string Comments { get; set; }
		public int IncomeTaxCalc { get; set; } 

	}
	public class EmployeeLoanModel
	{
		public Guid EmployeeId { get; set; }
		public string Code { get; set; }
		public int LoanAmount { get; set; }
		public DateTime LoanReleasedOn { get; set; }
		public DateTime LoanDeductFrom { get; set; }
		public int MonthlyAmount { get; set; }
		public bool Status { get; set; }
	}

}
