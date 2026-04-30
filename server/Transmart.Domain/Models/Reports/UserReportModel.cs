using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Reports
{
	public class UserReportModel : BaseModel
	{
		public string RoleName { get; set; }
		public string EmployeeName { get; set; }
		public string EmployeeCode { get; set; }
		public string Department { get; set; }
		public string Designation { get; set; }

	}
}
