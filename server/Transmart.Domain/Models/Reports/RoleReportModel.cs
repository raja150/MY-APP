using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Reports
{
	public class RoleReportModel : BaseModel
	{
		public string RoleName { get; set; }
		public string View { get; set; }
		public string Add { get; set; }
		public string Update { get; set; }
		public string Delete { get; set; }

	}
}
