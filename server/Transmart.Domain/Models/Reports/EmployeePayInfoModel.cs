using System;

namespace TranSmart.Domain.Models.Reports
{
	public class EmployeePayInfoRptModel : BaseModel
	{
		public Guid EmployeeId { get; set; }
		public string EmployeeCode { get; set; }
		public string EmployeeName { get; set; }
		public string Department { get; set; }
		public string Designation { get; set; }
		public DateTime DateOfJoining { get; set; }
		public int PayMode { get; set; }
		public string Bank { get; set; }
		public string BankName { get; set; }
		public string IFSCCode { get; set; }
		public string AccountNo { get; set; }
	}
}
