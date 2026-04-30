using Transmart.TS4API.Models;
using TranSmart.API.Converter;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models.Leave;
using TranSmart.Domain.Models.Leave.List;
using TranSmart.Domain.Models.Leave.Model;
using TranSmart.Domain.Models.Leave.Request;
using TranSmart.Domain.Models.LM_Attendance;

namespace TranSmart.API.AutoMapper
{
	public class LeaveProfile : Profile
	{
		public decimal WFH_NumberOfDays(ApplyWfh x)
		{
			decimal HalfDayValue = (x.FromHalf ? 0.5m : 0) + (x.ToHalf ? 0.5m : 0);
			decimal TotalDays = (decimal)(x.ToDateC.Date - x.FromDateC.Date).TotalDays + 1;
			return TotalDays - HalfDayValue;
		}
		public LeaveProfile()
		{
			CreateMap<ApplyLeaveRequest, ApplyLeave>();
			CreateMap<ApplyLeave, ApplyLeaveRequest>();

			CreateMap<ApplyLeave, ApplyLeaveList>()
				.ForMember(d => d.Status, opt => opt.MapFrom(x => Enum.GetName(typeof(ApplyLeaveSts), x.Status)))
				.ForMember(d => d.No, o => o.MapFrom(x => x.Employee.No))
				.ForMember(d => d.Name, o => o.MapFrom(x => x.Employee.Name))
				.ForMember(d => d.Department, o => o.MapFrom(x => x.Employee.Department.Name))
				.ForMember(d => d.ApprovedBy, opt => opt.MapFrom(x => x.ApprovedBy == null ? "" : x.ApprovedBy.Name));

			CreateMap<ApplyLeaveType, ApplyLeaveList>()
				.ForMember(d => d.LeaveType, o => o.MapFrom(x => x.LeaveType.Code))
				.ForMember(d => d.Status, opt => opt.MapFrom(x => Enum.GetName(typeof(ApplyLeaveSts), x.ApplyLeave.Status)))
				.ForMember(d => d.No, o => o.MapFrom(x => x.ApplyLeave.Employee.No))
				.ForMember(d => d.Name, o => o.MapFrom(x => x.ApplyLeave.Employee.Name))
				.ForMember(d => d.FromDate, o => o.MapFrom(x => x.ApplyLeave.FromDate.Date))
				.ForMember(d => d.ToDate, o => o.MapFrom(x => x.ApplyLeave.ToDate.Date))
				.ForMember(d => d.Department, o => o.MapFrom(x => x.ApplyLeave.Employee.Department.Name))
				.ForMember(d => d.ApprovedBy, opt => opt.MapFrom(x => x.ApplyLeave.ApprovedBy == null ? "" : x.ApplyLeave.ApprovedBy.Name));

			CreateMap<ApplyLeaveTypeRequest, ApplyLeaveType>();
			CreateMap<ApplyLeaveType, ApplyLeaveTypeRequest>()
				.ForMember(x => x.LeaveTypeName, opt => opt.MapFrom(x => x.LeaveType.Name));

			CreateMap<ApplyLeaveTypeModel, ApplyLeaveType>();
			CreateMap<ApplyLeaveType, ApplyLeaveTypeModel>();

			CreateMap<ApplyClientVisits, ApplyClientVisitList>()
				.ForMember(d => d.Status, opt => opt.MapFrom(x => Enum.GetName(typeof(ClientVisitStatus), x.Status)))
				.ForMember(d => d.Employee, o => o.MapFrom(x => x.Employee.No));

			CreateMap<ApplyLeave, ApplyLeaveModel>()
			  .ForMember(d => d.EmployeeNo, o => o.MapFrom(x => x.Employee.No))
			  .ForMember(d => d.EmployeeName, o => o.MapFrom(x => x.Employee.Name))
			  .ForMember(d => d.Designation, o => o.MapFrom(x => x.Employee.Designation.Name));

			CreateMap<ApplyLeaveType, ApplyLeaveTypeModel>()
				.ForMember(x => x.LeaveTypeName, opt => opt.MapFrom(x => x.LeaveType.Name))
				.ForMember(x => x.IsDefaultPayOff, opt => opt.MapFrom(x => x.LeaveType.DefaultPayoff));

			CreateMap<ApplyClientVisits, ApplyClientVisitModel>()
				.ForMember(d => d.EmployeeNo, opt => opt.MapFrom(x => x.Employee.No))
			   .ForMember(d => d.ApprovedById, opt => opt.MapFrom(x => x.ApprovedById))
			   .ForMember(d => d.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
			   .ForMember(d => d.Reason, opt => opt.MapFrom(x => x.PurposeOfVisit));



			CreateMap<ManualAttLogs, AttendanceImportModel>();
			CreateMap<AttendanceImportModel, ManualAttLogs>();

			CreateMap<BiometricAttLogs, BiometricAttLogsRequest>();
			CreateMap<BiometricAttLogsRequest, BiometricAttLogs>();

			CreateMap<TranSmart.Domain.Models.Leave.ApplyCompensatoryWorkingDayList, ApplyCompo>();
			CreateMap<ApplyCompo, TranSmart.Domain.Models.Leave.ApplyCompensatoryWorkingDayModel>();

			CreateMap<ApplyCompo, TranSmart.Domain.Models.Leave.ApplyCompensatoryWorkingDayList>()
				.ForMember(d => d.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
				.ForMember(d => d.Department, opt => opt.MapFrom(x => x.Employee.Department.Name))
				.ForMember(d => d.Status, opt => opt.MapFrom(x => Enum.GetName(typeof(ApplyCompoSts), x.Status)))
				.ForMember(d => d.ApprovedBy, opt => opt.MapFrom(x => x.ApprovedById != null ? x.ApprovedBy.Name : ""));

			CreateMap<ApplyWfh, ApplyWfhList>()
				.ForMember(d => d.EmployeeName, opt => opt.MapFrom(x => x.Employee.Name))
				.ForMember(d => d.Status, opt => opt.MapFrom(x => Enum.GetName(typeof(WfhStatus), x.Status)))
				.ForMember(d => d.FromDate, opt => opt.MapFrom(x => x.FromDateC))
				.ForMember(d => d.ToDate, opt => opt.MapFrom(x => x.ToDateC))
				.ForMember(d => d.Reason, opt => opt.MapFrom(x => x.ReasonForWFH))
				.ForMember(d => d.Department, opt => opt.MapFrom(x => x.Employee.Department.Name))
				.ForMember(d => d.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
				.ForMember(d => d.EmployeeNo, opt => opt.MapFrom(x => x.Employee.No))
				.ForMember(d => d.ApprovedBy, opt => opt.MapFrom(x => x.ApprovedBy == null ? "" : x.ApprovedBy.Name))
				.ForMember(d => d.ReportingTo, opt => opt.MapFrom(x => x.Employee.ReportingTo.Name));

			CreateMap<ApplyWfhModel, ApplyWfh>();
			CreateMap<ApplyWfh, ApplyWfhModel>()
				.ForMember(d => d.EmployeeName, opt => opt.MapFrom(x => x.Employee.Name))
				.ForMember(d => d.FromDate, opt => opt.MapFrom(x => x.FromDateC))
				.ForMember(d => d.ToDate, opt => opt.MapFrom(x => x.ToDateC))
				.ForMember(d => d.Reason, opt => opt.MapFrom(x => x.ReasonForWFH))
				.ForMember(d => d.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
				.ForMember(d => d.RejectReason, opt => opt.MapFrom(x => x.AdminReason))
				.ForMember(d => d.NoOfDays, opt => opt.MapFrom(x => WFH_NumberOfDays(x)));

			CreateMap<TranSmart.Domain.Entities.Leave.WeekOffDays, TranSmart.Domain.Models.Leave.List.WeekOffDaysList>()
				.ForMember(d => d.Type, opt => opt.MapFrom<WeekOffDayTypeResolver>())
				.ForMember(d => d.WeekNoInMonth, opt => opt.MapFrom(x => x.Type == 1 ? x.WeekNoInMonth : ""))
				.ForMember(d => d.WeekInYear, opt => opt.MapFrom<WeekOffDayWeekInYearResolver>())
				.ForMember(d => d.Status, opt => opt.MapFrom<WeekOffDayStatusResolver>());

			CreateMap<UnAuthorizedLeavesModel, UnAuthorizedLeaves>();
			CreateMap<UnAuthorizedLeaves, UnAuthorizedLeavesModel>()
				.ForMember(d => d.Name, opt => opt.MapFrom(x => x.Employee.Name))
				.ForMember(d => d.Department, opt => opt.MapFrom(x => x.Employee.Department.Name))
				.ForMember(d => d.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
				.ForMember(d => d.EmployeeStatus, opt => opt.MapFrom(x => x.Employee.Status));

		}
	}
}
