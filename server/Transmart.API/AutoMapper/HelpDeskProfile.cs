using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Domain.Entities.HelpDesk;
using TranSmart.Domain.Entities.SelfService;
using TranSmart.Domain.Models.Helpdesk.Request;
using TranSmart.Domain.Models.HelpDesk.Responce;
using TranSmart.Domain.Models.SelfService.List;
using TranSmart.Domain.Models.SelfService.Request;
using TranSmart.Domain.Models.SelfService.Response;

namespace TranSmart.API.AutoMapper
{
	public class HelpDeskProfile : Profile
	{
		public HelpDeskProfile()
		{
			CreateMap<TicketRequest, Ticket>();
			CreateMap<Ticket, TicketModel>()
				 .ForMember(dest => dest.User, opt => opt.MapFrom(x => x.RaiseBy.Name))
				 .ForMember(dest => dest.Priority, opt => opt.MapFrom(x => x.HelpTopic.Priority))
				 .ForMember(dest => dest.Email, opt => opt.MapFrom(x => x.RaiseBy.WorkEmail))
				 .ForMember(dest => dest.Department, opt => opt.MapFrom(x => x.RaiseBy.Department.Name))
				 .ForMember(dest => dest.Phone, opt => opt.MapFrom(x => x.RaiseBy.MobileNumber))
				 .ForMember(dest => dest.HelpTopic, opt => opt.MapFrom(x => x.HelpTopic.Name))
				 .ForMember(dest => dest.SubTopic, opt => opt.MapFrom(x => x.SubTopic.SubTopic))
				 .ForMember(dest => dest.SLAPlan, opt => opt.MapFrom(x => x.HelpTopic.DueHours))
				 .ForMember(dest => dest.DueDate, opt => opt.MapFrom(x => x.AddedAt.AddHours(x.HelpTopic.DueHours)))
				 .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(x => x.AssignedTo.Name))
				 .ForMember(dest => dest.LastResponse, opt => opt.MapFrom(x => x.ModifiedBy))
				 .ForMember(dest => dest.Status, opt => opt.MapFrom(x => x.TicketStatus.IsClosed))
				 .ForMember(dest => dest.TicketSts, opt => opt.MapFrom(x =>  x.TicketStatus.Name))
				 .ForMember(dest => dest.Gender, opt => opt.MapFrom(x => x.RaiseBy.Gender));

			CreateMap<Ticket, TicketList>()
				.ForMember(dest => dest.Department, opt => opt.MapFrom(x => x.Department.Department))
				.ForMember(dest => dest.AssignTo, opt => opt.MapFrom(x => x.AssignedTo.Name))
				.ForMember(dest => dest.EmployeeNo, opt => opt.MapFrom(x => x.RaiseBy.No))
				.ForMember(dest => dest.Designation, opt => opt.MapFrom(x => x.RaiseBy.Designation.Name))
				.ForMember(dest => dest.EmpDept, opt => opt.MapFrom(x => x.RaiseBy.Department.Name))
				.ForMember(dest => dest.LastUpdatedBy, opt => opt.MapFrom(x => x.ModifiedBy))
				.ForMember(dest => dest.LastModifiedAt, opt => opt.MapFrom(x => x.ModifiedAt))
				.ForMember(dest => dest.HelpTopic, opt => opt.MapFrom(x => x.HelpTopic.Name))
				.ForMember(dest => dest.RaisedBy, opt => opt.MapFrom(x => x.RaiseBy.Name))
				.ForMember(dest => dest.SubTopic, opt => opt.MapFrom(x => x.SubTopic.SubTopic))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(x => x.TicketStatus.Name))
				.ForMember(dest => dest.Priority, opt => opt.MapFrom(x => x.HelpTopic.Priority));

			CreateMap<TicketLogModel, TicketLog>();
			CreateMap<TicketLog, TicketLogModel>()
				.ForMember(d => d.RepliedBy, opt => opt.MapFrom(x => x.RepliedBy.Name));

			CreateMap<TicketLogRecipientsModel, TicketLogRecipients>();
			CreateMap<TicketLogRecipients, TicketLogRecipientsModel>();
			CreateMap<DeskDepartment, DeskDepartmentModel>()
			.ForMember(dest => dest.Department, opt => opt.MapFrom(x => x.Department));

			CreateMap<DeptTransferModel, Ticket>();

			CreateMap<TicketLog, TicketResponseModel>()
				.ForMember(dest => dest.Status, opt => opt.MapFrom(x => x.Ticket.TicketStatus.Name))
				.ForMember(dest => dest.No, opt => opt.MapFrom(x => x.Ticket.No))
				.ForMember(dest => dest.RaisedOn, opt => opt.MapFrom(x => x.Ticket.RaisedOn))
				.ForMember(dest => dest.RepliedBy, opt => opt.MapFrom(x => x.RepliedBy.Name))
				.ForMember(d => d.Raisedby, opt => opt.MapFrom(x => x.Ticket.RaiseBy.Name));
		}
	}
}
