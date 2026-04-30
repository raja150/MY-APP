using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.LM_Attendance.Request
{
	public class WebAttendanceRequest : BaseModel
	{
		public Guid EmployeeId { get; set; }
		public Guid? ApprovedById { get; set; }
		public DateTime AttendanceDate { get; set; }
		public DateTime InTime { get; set; }
		public DateTime? OutTime { get; set; }
		public int WorkTime { get; set; }
		public int AttendanceStatus { get; set; }
		public byte Status { get; set; }
		public bool IsApproved { get; set; }
		public string RejectReason { get; set; }
	}
}
