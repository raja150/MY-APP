using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Domain.Models.Payroll.List
{
	public class DeclarationList : BaseModel
	{
		public Guid EmployeeId { get; set; }
		public string EmployeeName { get; set; }
		public string EmployeeNo { get; set; }
		public string Designation { get; set; }
		public string Department { get; set; }
		public string Year { get; set; }
		public int Taxable { get; set; }
		public int Tax { get; set; }
		public int Due { get; set; }
		public bool IsNewRegime { get; set; }
		public DateTime DateOfJoining { get; set; }

	}
}
