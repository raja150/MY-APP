using Microsoft.EntityFrameworkCore;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.SelfService.Reports;
using TranSmart.Domain.Models.Leave.Model;

namespace TranSmart.Data
{
    public partial class TranSmartContext
    {
        public DbSet<LeaveBalancesQry> Leaves { get; set; }
        public DbSet<LeaveBalance> Leave_LeaveBalance { get; set; }
        public DbSet<EmployeeLeaveBalanceQry> EmployeeLeaves { get; set; }
        public DbSet<EmployeeProfileQry> EmployeeProfiles { get; set; }
        public DbSet<ApplyLeave> Leave_ApplyLeave { get; set; }
		public DbSet<ApplyLeaveType> Leave_ApplyLeaveType { get; set; }
        public DbSet<Attendance> HR_Attendance { get; set; }
        public DbSet<AttendanceModifyLogs> LM_AttendanceModifyLogs { get; set; }
        public DbSet<AttendanceSum> HR_AttendanceSum { get; set; }
        public DbSet<BiometricAttLogs> HR_BiometricAttLogs { get; set; }
        public DbSet<ManualAttLogs> HR_ManualAttLogs { get; set; }
        public DbSet<ApplyCompo> LM_ApplyCompensatoryWorkingDay { get; set; }
        public DbSet<ApplyWfh> LM_ApplyWFH { get; set; }
        public DbSet<ApplyLeaveDetails> Leave_ApplyLeaveDetails { get; set; }
		public DbSet<UnAuthorizedLeaves> LM_UnAuthorizedLeaves { get; set; }
		public DbSet<WebAttendance> Web_Attendance { get; set; }

	}
}
