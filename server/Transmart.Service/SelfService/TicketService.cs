using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Core.Util;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Domain.Entities.HelpDesk;
using TranSmart.Domain.Entities.SelfService;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;

namespace TranSmart.Service.SelfService
{
	public partial interface ITicketService : IBaseService<Ticket>
	{
		Task<Result<TicketLog>> UserResponse(Guid ticketId, string resp, Guid loginEmpId);
		Task<Result<TicketLog>> UpdateResponse(Guid id, string response, Guid loginEmpId);

	}
	public partial class TicketService : BaseService<Ticket>, ITicketService
	{
		private readonly ISequenceNoService _sequenceNoService;
		public TicketService(IUnitOfWork uow, ISequenceNoService sequenceNo) : base(uow)
		{
			_sequenceNoService = sequenceNo;

		}
		public override async Task<Ticket> GetById(Guid id)
		{
			return await UOW.GetRepositoryAsync<Ticket>().SingleAsync(x => x.ID == id,
				include: i => i.Include(x => x.RaiseBy).ThenInclude(x => x.Department)
				.Include(x => x.AssignedTo).Include(x => x.HelpTopic)
				.Include(x => x.TicketStatus).Include(x => x.SubTopic));
		}
		public override async Task<Result<Ticket>> AddAsync(Ticket item)
		{
			Tuple<int, string> tuple = await _sequenceNoService.NextSequenceNo("SelfService_Ticket", "No");
			var tkSts = await UOW.GetRepositoryAsync<HelpTopic>().SingleAsync(x => x.ID == item.HelpTopicId && x.Status);

			item.No = tuple.Item2;
			item.RaisedOn = DateTime.Now;
			item.TicketStatusId = tkSts.TicketStatusId;

			return await base.AddAsync(item);
		}
		public override async Task<Result<Ticket>> UpdateAsync(Ticket item)
		{
			Ticket entity = await UOW.GetRepositoryAsync<Ticket>().SingleAsync(x => x.ID == item.ID);
			Result<Ticket> result = new();
			if (entity == null)
			{
				result.AddMessageItem(new MessageItem(Resource.Invalid_Ticket));
				return result;
			}
			entity.Update(item);
			return await base.UpdateAsync(entity);
		}

		public override async Task<IPaginate<Ticket>> GetPaginate(BaseSearch baseSearch)
		{
			var search = (TicketSearch)baseSearch;
			return await UOW.GetRepositoryAsync<Ticket>().GetPageListAsync(
			   predicate: x => x.RaiseById == baseSearch.RefId
			   && (!search.TicketStsId.HasValue || x.TicketStatusId == search.TicketStsId),
				include: i => i.Include(x => x.Department)
				.Include(x => x.HelpTopic).Include(x => x.SubTopic).Include(x => x.TicketStatus),
				 index: baseSearch.Page, size: baseSearch.Size, sortBy: baseSearch.SortBy ?? "AddedAt",
				 ascending: !baseSearch.IsDescend);
		}
		public async Task<Result<TicketLog>> UserResponse(Guid ticketId, string resp, Guid loginEmpId)
		{
			var result = new Result<TicketLog>();
			try
			{
				await UOW.GetRepositoryAsync<TicketLog>().AddAsync(new TicketLog
				{
					TicketId = ticketId,
					Response = resp,
					RepliedOn = TimeStamp(),
					RepliedById = loginEmpId,
					TypeOfLog = (byte)TicketLogSts.UserReply
				});
				await UOW.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}
			return result;
		}

		public async Task<Result<TicketLog>> UpdateResponse(Guid id, string response, Guid loginEmpId)
		{
			var result = new Result<TicketLog>();
			var logResp = await UOW.GetRepositoryAsync<TicketLog>().SingleAsync(x => x.ID == id);

			if (logResp == null)
			{
				result.AddMessageItem(new MessageItem(Resource.Invalid_Log_Response));
				return result;
			}
			var ticket = await UOW.GetRepositoryAsync<Ticket>().SingleAsync(x => x.ID == logResp.TicketId && !x.TicketStatus.IsClosed);
			if (ticket == null)
			{
				result.AddMessageItem(new MessageItem(Resource.TICKET_IS_CLOSED));
				return result;
			}

			if (logResp.RepliedById != loginEmpId)
			{
				result.AddMessageItem(new MessageItem(Resource.Cannot_Be_Editable));
				return result;
			}
			
			logResp.Response = response;
			try
			{
				UOW.GetRepositoryAsync<TicketLog>().UpdateAsync(logResp);
				await UOW.SaveChangesAsync();
				result.ReturnValue = logResp;
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}
			return result;
		}
	}
}
