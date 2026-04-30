using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.SelfService;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Cache;

namespace TranSmart.Data.Repository.HelpDesk
{
	public interface ITicketRepository
	{
		Task UpdateTempData();
		Task<IPaginate<Ticket>> GetTickets(TicketSearch search);
		Task<IEnumerable<DeskGroupEmpCache>> GetGroupEmps();
	}
	public class TicketRepository : ITicketRepository
	{
		private readonly TranSmartContext _context;
		public TicketRepository(TranSmartContext context)
		{
			_context = context;
		}

		public async Task UpdateTempData()
		{
			await _context.Connection.QueryAsync(" delete from HD_DepartmentEmployee " +
			" insert into HD_DepartmentEmployee " +
			" select NEWID() Id, g.EmpId, g.DeskDepartmentId, g.groupId from " +
			" (select * from(select  ge.EmployeeId EmpId, " +
				 " dg.DeskDepartmentId  from HD_DeskGroupEmployee GE " +
				 " inner join HD_DepartmentGroup dg on GE.DeskGroupId = dg.GroupsId " +
				 " inner join HD_DeskDepartment dd on dd.ID = dg.DeskDepartmentId " +
				 " group by EmployeeId, DeskDepartmentId) as t1 " +
				 " inner join(select * from (select  ROW_NUMBER() OVER(PARTITION BY EmployeeId ORDER by addedAt desc) " +
				 " AS RowNo, e.EmployeeId, e.DeskGroupId GroupId from HD_DeskGroupEmployee e) as a " +
			" where RowNo = 1) as t2 on t1.EmpId = t2.EmployeeId) g ");
		}

		public async Task<IPaginate<Ticket>> GetTickets(TicketSearch search)
		{
			return await (from Tk in _context.SelfService_Ticket
						  join dt in _context.DepartmentEmployee on Tk.DepartmentId equals dt.DeskDepartmentId
						  join dp in _context.Helpdesk_DeskDepartment on Tk.DepartmentId equals dp.ID
						  join ht in _context.Helpdesk_HelpTopic on Tk.HelpTopicId equals ht.ID
						  join st in _context.Helpdesk_HelpTopicSub on Tk.SubTopicId equals st.ID
						  join rb in _context.Organization_Employee on Tk.RaiseById equals rb.ID
						  join dept in _context.Organization_Department on rb.DepartmentId equals dept.ID
						  join desi in _context.Organization_Designation on rb.DesignationId equals desi.ID
						  join at in _context.Organization_Employee on Tk.AssignedToId equals at.ID into assignEmps
						  from assignedEmp in assignEmps.DefaultIfEmpty()
						  join status in _context.Helpdesk_TicketStatus on Tk.TicketStatusId equals status.ID
						  where (dt.EmployeeId == search.RefId.Value) && (!search.TicketStsId.HasValue || Tk.TicketStatusId == search.TicketStsId.Value)
						  select new Ticket
						  {
							  No = Tk.No,
							  RaisedOn = Tk.RaisedOn,
							  Subject = Tk.Subject,
							  Message = Tk.Message,
							  Department = dp,
							  DepartmentId = Tk.DepartmentId,
							  HelpTopic = ht,
							  SubTopic = st,
							  AssignedTo = assignedEmp,
							  ModifiedAt = Tk.ModifiedAt,
							  ModifiedBy = Tk.ModifiedBy,
							  RaiseBy = new Employee
							  {
								  Designation = desi,
								  Department = dept,
								  Name = rb.Name,
								  No = rb.No,
								  ID = rb.ID
							  },
							  ID = Tk.ID,
							  TicketStatus = status
						  }).Union(from Tk in _context.SelfService_Ticket
								   join dp in _context.Helpdesk_DeskDepartment on Tk.DepartmentId equals dp.ID
								   join ht in _context.Helpdesk_HelpTopic on Tk.HelpTopicId equals ht.ID
								   join st in _context.Helpdesk_HelpTopicSub on Tk.SubTopicId equals st.ID
								   join rb in _context.Organization_Employee on Tk.RaiseById equals rb.ID
								   join dept in _context.Organization_Department on rb.DepartmentId equals dept.ID
								   join desi in _context.Organization_Designation on rb.DesignationId equals desi.ID
								   join at in _context.Organization_Employee on Tk.AssignedToId equals at.ID into assignEmps
								   from assignedEmp in assignEmps.DefaultIfEmpty()
								   join status in _context.Helpdesk_TicketStatus on Tk.TicketStatusId equals status.ID
								   where (dp.ManagerId == search.RefId.Value) && (!search.TicketStsId.HasValue || Tk.TicketStatusId == search.TicketStsId.Value)
								   select new Ticket
								   {
									   No = Tk.No,
									   RaisedOn = Tk.RaisedOn,
									   Subject = Tk.Subject,
									   Message = Tk.Message,
									   Department = dp,
									   DepartmentId = Tk.DepartmentId,
									   HelpTopic = ht,
									   SubTopic = st,
									   AssignedTo = assignedEmp,
									   ModifiedAt = Tk.ModifiedAt,
									   ModifiedBy = Tk.ModifiedBy,
									   RaiseBy = new Employee
									   {
										   Designation = desi,
										   Department = dept,
										   Name = rb.Name,
										   No = rb.No,
										   ID = rb.ID
									   },
									   ID = Tk.ID,
									   TicketStatus = status
								   }).OrderByDescending(o => o.No).ToPaginateAsync(search.Page, search.Size, 0, default(CancellationToken));
		}

		public async Task<IEnumerable<DeskGroupEmpCache>> GetGroupEmps()
		{
			return await _context.DepartmentEmployee.GroupBy(x => new { x.EmployeeId, x.DeskDepartmentId }).Select(x => new
			{
				Id = x.Key.EmployeeId,
				DeptId = x.Key.DeskDepartmentId,
			}).Join(_context.Organization_Employee.Where(x => x.Status == 1), g => g.Id, t => t.ID,
				(g, t) => new
				{
					t.DesignationId,
					t.Name,
					t.No,
					g.Id,
					t.WorkEmail,
					g.DeptId
				}).Join(_context.Organization_Designation.Where(x => x.Status), g => g.DesignationId, t => t.ID,
				(g, t) => new DeskGroupEmpCache
				{
					Name = g.Name,
					Designation = t.Name,
					ID = g.Id,
					WorkMail = g.WorkEmail,
					No = g.No,
					HdDeptId = g.DeptId
				}).ToListAsync();
		}
	}
}
