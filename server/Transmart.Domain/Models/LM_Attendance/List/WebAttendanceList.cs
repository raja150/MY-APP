using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.LM_Attendance.List
{
	public class WebAttendanceList : BaseModel
	{
		public string EmployeeName { get; set; }
		public string EmployeeNo { get; set; }
		public string Designation { get; set; }
		public string Department { get;set; }
		public DateTime AttendanceDate { get; set; }
		public byte Status { get; set; }
	}
}
