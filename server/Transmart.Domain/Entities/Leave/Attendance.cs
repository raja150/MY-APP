using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TranSmart.Domain.Entities.Leave
{
    [Table("LM_Attendance")]
    public class Attendance : DataGroupEntity
    {
        public Guid EmployeeId { get; set; }
        public Organization.Employee Employee { get; set; }
        public DateTime AttendanceDate { get; set; }
        public int? SchIntime { get; set; }
        public int? SchOutTime { get; set; }
        public int? SchWorkTime { get; set; }
        public int? SchBreakTime { get; set; }
        public int? SchInTimeGrace { get; set; }
        public int? SchBreaks { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public int? WorkTime { get; set; }
        public int? BreakTime { get; set; }
        public int? Breaks { get; set; }
        public int? AttendanceStatus { get; set; }
        public decimal? UADays { get; set; }
        public Guid? LeaveTypeID { get; set; }
		public LeaveType LeaveType { get; set; }
        public bool? IsHalfDay { get; set; }
        public int? HalfDayType { get; set; }
        public bool? IsFirstHalf { get; set; }
        [Column(TypeName = "decimal(2, 1)")]
        public decimal? Present { get; set; }
        [Column(TypeName = "decimal(2, 1)")]
        public decimal? Absent { get; set; }
        [Column(TypeName = "decimal(2, 1)")]
        public decimal? Leave { get; set; }
        [Column(TypeName = "decimal(2, 1)")]
        public decimal? WFH { get; set; }
		public byte LoginType { get; set; }
		//public Guid ApplyLeaveID { get;set; }
		//public ApplyLeave ApplyLeave { get; set; }
	}
}
