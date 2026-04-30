using System;
using System.Collections.Generic;
using System.Text;

namespace TranSmart.Data
{
	public enum LeaveTypesScreens
	{
		LeaveType = 1,
		CompensatoryWorkingDay,
		AssignPreLeaves,
		CustomizeLeaveBalance,
		ApplyLeaves,
		DeductionFromPreConsumed,
		LeaveLapsedSchedule,
		LeaveRevisedFromAttendance,
		LeavesModifiedFromManualUpdate,
		Attendance
	}

	public enum AttendanceStatus
	{
		Present = 1,
		Absent = 2,
		Leave = 3,
		WeekOff = 4,
		WFH = 5,
		Holiday = 6,
		HalfDayPresent = 7,
		HalfDayLeave = 8,
		HalfDayWFH = 9,
		HalfDayAbsent = 10,
		MaternityLeave = 11,
		LongLeave = 12,
		Unautherized = 20,
		Late = 21
	}
	public enum PayTypeStatus
	{
		UnPaid = 0,
		Paid = 1
	}
	public enum UserSts
	{
		Locked,
		Active,
		InActive
	}
	public enum LoginType
	{
		Manual = 0,
		Biometric=1,
		WebPunch=2,
		ClientVisit=3
	}
	public enum AttMovementType
	{
		In = 0,
		Out = 1
	}
	public enum ComputerType
	{
		OfficeDesktop = 1,
		OfficeLaptop,
		BYOD,
		BYODRDPSystem,
		OfficeRDPSystem,
		WorkingFromOffice
	}
	public enum WebAttendanceSts
	{
		Applied = 1,
		Approved,
		Rejected
	}
	public enum AttendanceType
	{
		Biometric = 0,
		Web
	}
}

