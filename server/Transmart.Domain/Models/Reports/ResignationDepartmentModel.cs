using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Reports
{
	public class ResignationDepartmentModel
	{
		public string EmployeeName { get; set; }
		public string EmployeeNo { get; set; }
		public string LOB { get;set; }
		public string FunctionalArea { get; set; }
		public string Designation { get; set; }
		public DateTime DateOfJoining { get; set; }
		public DateTime? LastWorkingDate { get; set; }
	}
}
