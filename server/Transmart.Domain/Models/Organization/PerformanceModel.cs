using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Domain.Entities.Organization;

namespace TranSmart.Domain.Models.Organization
{
	public class PerformanceModel : BaseModel
	{
		public Guid EmployeeId { get; set; }
		public string Name { get; set; }
		public string EmployeeNo { get; set; }
		public byte PerformanceType { get; set; }
		public int? WeekNumber { get; set; }
		public DateTime PerformedDate { get; set; }
	}
}
