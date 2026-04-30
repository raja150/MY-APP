namespace TranSmart.Domain.Entities.Payroll
{
	public partial class Loan : DataGroupEntity
	{
		public void Update(Loan loan)
		{
			EmployeeId = loan.EmployeeId;
			Employee = loan.Employee;
			LoanReleasedOn = loan.LoanReleasedOn;
			LoanAmount = loan.LoanAmount;
			DeductFrom = loan.DeductFrom;
			MonthlyAmount = loan.MonthlyAmount;
			Notes = loan.Notes;
			Status = loan.Status;
		}
	}
}
