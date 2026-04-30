using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave.Approval;
using TranSmart.Domain.Models.LM_Attendance.List;
using TranSmart.Domain.Models.LM_Attendance.Responce;
using TranSmart.Domain.Models.Organization;
using TranSmart.Domain.Models.Reports;
using TranSmart.Domain.Models.SelfService;

namespace TranSmart.API.AutoMapper
{
	public class ApprovalProfile : Profile
	{
		public ApprovalProfile()
		{
			CreateMap<ApplyLeave, ApplyLeavesList>()
				.ForMember(d => d.EmployeeNo, opt => opt.MapFrom(x => x.Employee.No))
				.ForMember(d => d.EmployeeName, opt => opt.MapFrom(x => x.Employee.Name))
				.ForMember(d => d.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
				.ForMember(d => d.Department, opt => opt.MapFrom(x => x.Employee.Department.Name))
				.ForMember(d => d.Status, opt => opt.MapFrom(x => Enum.GetName(typeof(ApplyLeaveSts), x.Status)))
				.ForMember(d => d.ApprovedBy, opt => opt.MapFrom(x => x.ApprovedBy == null ? "" : x.ApprovedBy.Name))
				.ForMember(d => d.ReportingTo, opt => opt.MapFrom(x => x.Employee.ReportingTo.Name));
			CreateMap<ApplyLeaveDetails, LeaveRequestDetailsModel>()
				.ForMember(d => d.LeaveName, opt => opt.MapFrom(x => x.LeaveType.Name));

			CreateMap<ApplyLeavesModel, ApplyLeave>();
			CreateMap<ApplyLeave, ApplyLeavesModel>()
				.ForMember(d => d.EmployeeName, opt => opt.MapFrom(x => x.Employee.Name))
				.ForMember(d => d.EmployeeNo, opt => opt.MapFrom(x => x.Employee.No))
				.ForMember(d => d.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
				.ForMember(d => d.Department, opt => opt.MapFrom(x => x.Employee.Department.Name));
			CreateMap<ApplyLeave, LeaveInfoModel>()
				.ForMember(d => d.EmployeeNo, opt => opt.MapFrom(x => x.Employee.No))
				.ForMember(d => d.EmployeeId, opt => opt.MapFrom(x => x.EmployeeId))
				.ForMember(d => d.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
				.ForMember(d => d.EmployeeName, opt => opt.MapFrom(x => x.Employee.Name));

			CreateMap<ApplyClientVisits, ApplyClientVisitsList>()
			 .ForMember(d => d.Status, opt => opt.MapFrom(x => Enum.GetName(typeof(ClientVisitStatus), x.Status)))
			 .ForMember(d => d.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
			 .ForMember(d => d.ApprovedBy, opt => opt.MapFrom(x => x.ApprovedBy.Name));

			CreateMap<WebAttendance, WebAttendanceList>()
				.ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(x => x.Employee.Name))
				.ForMember(dest => dest.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
				.ForMember(dest => dest.Department, opt => opt.MapFrom(x => x.Employee.Department.Name));
			CreateMap<WebAttendance, WebAttendanceModel>()
				.ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(x => x.Employee.Name))
				.ForMember(dest => dest.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name));

			CreateMap<TranSmart.Domain.Models.Leave.ApplyCompensatoryWorkingDayModel, ApplyCompo>();
			CreateMap<ApplyCompo, TranSmart.Domain.Models.Leave.ApplyCompensatoryWorkingDayModel>();

			CreateMap<ApplyCompo, TranSmart.Domain.Models.Leave.ApplyCompensatoryWorkingDayList>();

			CreateMap<ApplyClientVisitsModel, ApplyClientVisits>();
			CreateMap<ApplyClientVisits, ApplyClientVisitsModel>();
			CreateMap<ApplyClientVisits, ClientVisitInfoModel>()
			   .ForMember(d => d.EmployeeNo, opt => opt.MapFrom(x => x.Employee.No))
			   .ForMember(d => d.Designation, opt => opt.MapFrom(x => x.Employee.Designation.Name))
			   .ForMember(d => d.Status, opt => opt.MapFrom(x => x.Status))
			   .ForMember(d => d.EmployeeName, opt => opt.MapFrom(x => x.Employee.Name));
			CreateMap<ApplyLeave, LeavesModel>();
			CreateMap<ApplyLeave, EmployeeModel>();
			CreateMap<ApplyWfh, EmployeeModel>()
				.ForMember(d => d.Name, opt => opt.MapFrom(x => x.Employee.Name));

			CreateMap<ImageModel, EmpImage>();
			CreateMap<EmpImage, ImageModel>()
				.ForMember(d => d.ImagePicture, x => x.MapFrom(s => s.ImageData))
				.ForMember(d => d.ResizeImagePicture, x => x.MapFrom(s => s.ResizeImageData));
		}
	}
}
