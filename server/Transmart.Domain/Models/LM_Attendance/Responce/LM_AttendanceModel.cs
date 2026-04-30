using System;

namespace TranSmart.Domain.Models.LM_Attendance.Response
{
   public class AttendanceModel:BaseModel
    {
        public Guid EmployeeId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public int SchIntime { get; set; }
        public int SchOutTime { get; set; }
        public int SchWorkTime { get; set; }
        public int SchBreakTime { get; set; }
        public int SchInTimeGrace { get; set; }
        public int SchBreaks { get; set; }
        public DateTime InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public int WorkTime { get; set; }
        public int BreakTime { get; set; }
        public int Breaks { get; set; }
        public int? AttendanceStatus { get; set; }
        public Guid? LeaveTypeID { get; set; }
        public TimeSpan Time => InTime.TimeOfDay;
        public bool AllowWebPunch { get; set; }

    }
}
