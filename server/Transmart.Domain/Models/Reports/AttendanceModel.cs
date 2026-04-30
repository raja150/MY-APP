using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranSmart.Domain.Models.Reports
{
    public class AttendanceModel
    {
        public string Name { get; set; }
		public string Designation { get;set; }
        public DateTime Date { get; set; }
        public DateTime? PunchIn { get; set; }
        public DateTime? PunchOut { get; set; }
        public int? AttendanceStatus { get; set; }
        public int? Breaks { get; set; }
        public Guid EmployeeId { get; set; }
		public string Status { get; set; }
	}
	public class AttendanceReportModel : AttendanceModel
	{
		public int? BreakTime { get; set; }
		public int? WorkTime { get; set; }
		public byte LoginType { get; set; }
		public string LOB { get; set; }
		public string FA { get; set; }
		public string Department { get; set; }
		public bool? IsHalfDay { get; set; }
		public int? HalfDayType { get; set; }
	}
}
