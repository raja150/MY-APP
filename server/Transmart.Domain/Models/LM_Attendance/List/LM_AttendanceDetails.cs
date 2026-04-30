using FluentValidation;
using System;
using System.Collections.Generic;

namespace TranSmart.Domain.Models.LM_Attendance.List
{
	public class EmployeeAttendance : BaseModel
	{
		public string EmployeeNo { get; set; }
		public string Name { get; set; }
		public string Department { get; set; }
		public string Designation { get; set; }
		public DateTime DateOfJoining { get; set; }
		public IEnumerable<AttendanceDetails> Attendance { get; set; }
	}
	public class AttendanceDetails : BaseModel
	{
		public DateTime AttendanceDate { get; set; }
		public Guid AttendanceID { get; set; }
		public Guid EmployeeId { get; set; }
		public string AttendanceStatus { get; set; }
		public int AttendanceStatusID { get; set; }
		public Guid? LeaveTypeId { get; set; }
		public string LeaveTypeName { get; set; }
		public int? Unauthorized { get; set; }
		//True means Full Day
		//False means Half Day
		public bool IsHalfDay { get; set; }
		//True means First Half
		//False means second Half
		public bool IsFirstOff { get; set; }
		public int HalfDayType { get; set; }
		public DateTime InTime { get; set; }
		public DateTime OutTime { get; set; }
		public int? WorkTime { get; set; }
		public bool IsRemainingHalf { get; set; }
	}
	public class AttandanceDetailsValidator : AbstractValidator<AttendanceDetails>
	{
		public AttandanceDetailsValidator()
		{
			RuleFor(x => x.AttendanceStatus).NotEmpty().NotNull().WithMessage("Leave Type is required!");
			RuleFor(x => x.LeaveTypeId).NotNull().When(x => x.AttendanceStatus == "3" || x.AttendanceStatus == "8").WithMessage("Leave Type is required !");
			RuleFor(x => x.Unauthorized).NotNull().GreaterThan(0).When(x => x.AttendanceStatus == "20");
			RuleFor(x => x.HalfDayType).NotEmpty().When(x => x.IsRemainingHalf).WithMessage("Please select remaining half");
		}
	}
}
