using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Organization
{
	public class EmployeeImageModel : BaseModel
	{
		public string Name { get; set; }
		public string No { get; set; }
		public string MobileNumber { get; set; }
		public string Gender { get; set; }
		public DateTime? DateOfJoining { get; set; }
		public string WorkEmail { get; set; }
		public string DepartmentName { get; set; }
		public string WorkLocation { get; set; }
		public string EmployeeTeam { get; set; }

	}
}
