using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Entities.Leave
{
	[Table("Web_Attendance")]
	public class WebAttendance : DataGroupEntity
	{
		public Guid EmployeeId { get; set; }
		public Organization.Employee Employee { get; set; }
		public Guid? ApprovedById { get; set; }
		public Organization.Employee ApprovedBy { get; set; }
		public DateTime AttendanceDate { get; set; }
		public DateTime InTime { get; set; }
		public DateTime? OutTime { get; set; }
		public byte Status { get; set; }
		public string RejectReason { get; set; }
	}
}
