using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Data.Repository.HelpDesk;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Domain.Entities.HelpDesk;
using TranSmart.Domain.Entities.SelfService;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;

namespace TranSmart.Service.Helpdesk
{
	public partial interface ITicketLogService : IBaseService<TicketLog>
	{
		Task<IPaginate<Ticket>> GetTicekts(TicketSearch search);
		Task<Result<Ticket>> DeptTransfer(Ticket item, Guid loginEmpId);
		Task<Result<TicketLog>> ReAssign(TicketLog item);
		Task<IEnumerable<TicketLog>> GetTicketLog(Guid id);
		Task<TicketLog> TicketResponse(Guid id);
		Task<Result<TicketLog>> UpdateResponse(Guid id, string response, Guid loginEmpId);
	}
	public partial class TicketLogService : BaseService<TicketLog>, ITicketLogService

	{
		private readonly ITicketRepository _repository;
		public TicketLogService(IUnitOfWork uow, ITicketRepository repository) : base(uow)
		{
			_repository = repository;
		}

		public override async Task<Result<TicketLog>> AddAsync(TicketLog item)
		{
			var result = new Result<TicketLog>();

			Ticket ticket = await UOW.GetRepositoryAsync<Ticket>().SingleAsync(x => x.ID == item.TicketId && !x.TicketStatus.IsClosed);

			if (ticket == null)
			{
				result.AddMessageItem(new MessageItem(Resource.TICKET_IS_CLOSED));
				return result;
			}

			DeskGroup grp = await GetGroup(item.RepliedById);



			if (grp == null)
			{
				if (!await IsManager(ticket.DepartmentId, item.RepliedById))
				{
					result.AddMessageItem(new MessageItem(Resource.Sorry_You_Does_Not_Have_Access));
					return result;
				}
			}
			else
			{
				//Checking login employee has access to post a reply to user or close the ticket
				if (grp.CanPostReply != 1)
				{
					result.AddMessageItem(new MessageItem(Resource.Does_Not_Have_Permission_To_Replay));
					return result;
				}
				var tkSts = await UOW.GetRepositoryAsync<TicketStatus>().SingleAsync(x => x.ID == item.TicketStatusId && x.IsClosed);

				if (grp.CanCloseTicket != 1 && tkSts != null)
				{
					result.AddMessageItem(new MessageItem(Resource.Does_Not_Have_Permission_To_Close));
					return result;
				}
			}
			ticket.TicketStatusId = item.TicketStatusId.Value;
			ticket.ModifiedAt = TimeStamp();
			ticket.ModifiedBy = item.ModifiedBy;

			UOW.GetRepositoryAsync<Ticket>().UpdateAsync(ticket);
			item.TypeOfLog = (byte)TicketLogSts.PostReply;
			return await base.AddAsync(item);
		}
		private async Task<DeskGroup> GetGroup(Guid empId)
		{
			var grpEmp = await UOW.GetRepositoryAsync<DepartmentEmployee>().SingleAsync(x => x.EmployeeId == empId);
			if (grpEmp == null)
			{
				return null;
			}
			return await UOW.GetRepositoryAsync<DeskGroup>().SingleAsync(x => x.ID == grpEmp.GroupId && x.Status);
		}
	
		private async Task<bool> IsManager(Guid deptId, Guid empId)
		{
			return await UOW.GetRepositoryAsync<DeskDepartment>().HasRecordsAsync(
						x => x.ManagerId == empId && x.ID == deptId);
		}
		public async Task<Result<Ticket>> DeptTransfer(Ticket item, Guid loginEmpId)
		{
			Result<Ticket> result = new();
			Ticket entity = await UOW.GetRepositoryAsync<Ticket>().SingleAsync(x => x.ID == item.ID && !x.TicketStatus.IsClosed);

			if (entity == null)
			{
				result.AddMessageItem(new MessageItem(Resource.Invalid_Ticket));
				return result;
			}

			DeskGroup grp = await GetGroup(loginEmpId);

			//Checking the employee has access to transfer the ticket or not
			if (grp == null)
			{
				if (!await IsManager(entity.DepartmentId, loginEmpId))
				{
					result.AddMessageItem(new MessageItem(Resource.Sorry_You_Does_Not_Have_Access));
					return result;
				}
			}
			else
			{
				if (grp.CanTransferTicket != 1)
				{
					result.AddMessageItem(new MessageItem(Resource.Does_Not_Have_Permission_To_Transfer));
					return result;
				}
			}

			await UOW.GetRepositoryAsync<TicketLog>().AddAsync(new TicketLog
			{
				TicketId = entity.ID,
				RepliedOn = TimeStamp(),
				TypeOfLog = (byte)TicketLogSts.Transfer,
				RepliedById = loginEmpId,
				Response = item.Message,
			});

			entity.DepartmentId = item.DepartmentId;
			entity.ModifiedAt = TimeStamp();
			entity.ModifiedBy = item.ModifiedBy;
			try
			{
				UOW.GetRepositoryAsync<Ticket>().UpdateAsync(entity);
				await UOW.SaveChangesAsync();
				result.ReturnValue = entity;
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}
			return result;
		}

		public async Task<Result<TicketLog>> ReAssign(TicketLog item)
		{
			Result<TicketLog> result = new();
			Ticket entity = await UOW.GetRepositoryAsync<Ticket>().SingleAsync(x => x.ID == item.TicketId && !x.TicketStatus.IsClosed);

			if (entity == null)
			{
				result.AddMessageItem(new MessageItem(Resource.TICKET_IS_CLOSED));
				return result;
			}

			DeskGroup grp = await GetGroup(item.RepliedById);

			if (grp == null)
			{
				if (!await IsManager(entity.DepartmentId, item.RepliedById))
				{
					result.AddMessageItem(new MessageItem(Resource.Sorry_You_Does_Not_Have_Access));
					return result;
				}

			}
			else
			{
				if (grp.CanAssignTicket != 1)
				{
					result.AddMessageItem(new MessageItem(Resource.Does_Not_Have_Permission_To_Re_Assign));
					return result;
				}
			}
			try
			{
				entity.AssignedToId = item.AssignedToId;
				entity.ModifiedAt = TimeStamp();
				entity.ModifiedBy = item.ModifiedBy;
				UOW.GetRepositoryAsync<Ticket>().UpdateAsync(entity);
				item.TypeOfLog = (byte)TicketLogSts.ReAssign;
				result = await base.AddAsync(item);
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}
			return result;
		}

		public async Task<IPaginate<Ticket>> GetTicekts(TicketSearch search)
		{
			return await _repository.GetTickets(search);
		}
		public Task<IEnumerable<TicketLog>> GetTicketLog(Guid id)
		{
			return UOW.GetRepositoryAsync<TicketLog>().GetAsync(
				predicate: p => p.TicketId == id && !string.IsNullOrEmpty(p.Response),
				include: i => i.Include(x => x.RepliedBy),
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
		}

		public Task<TicketLog> TicketResponse(Guid id)
		{
			return UOW.GetRepositoryAsync<TicketLog>().SingleAsync(
				predicate: p => p.ID == id && !string.IsNullOrEmpty(p.Response),
				include: i => i.Include(x => x.Ticket).ThenInclude(x => x.RaiseBy)
						.Include(x => x.Ticket).ThenInclude(x => x.TicketStatus).Include(x => x.RepliedBy),
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
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

			DeskGroup grpEmp = await GetGroup(loginEmpId);

			//Ticket raised person can edit their response until ticket will close
			if (loginEmpId != ticket.RaiseById)
			{
				//Checking login user has permission to edit 
				//When ticket raised person is not same as login user
				if (grpEmp == null)
				{
					//Checking Login Employee is Manager or not
					if (!await IsManager(ticket.DepartmentId, loginEmpId))
					{
						result.AddMessageItem(new MessageItem(Resource.Invalid_User));
						return result;
					}
				}
				else
				{
					//Checking Login Group Employee has access to edit
					if (grpEmp.CanEditTicket != 1)
					{
						result.AddMessageItem(new MessageItem(Resource.Does_Not_Have_Permission_To_Edit));
						return result;
					}
				}
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
