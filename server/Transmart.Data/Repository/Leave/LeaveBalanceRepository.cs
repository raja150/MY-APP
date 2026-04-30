using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Reports;
using TranSmart.Domain.Models.Reports.LMS;

namespace TranSmart.Data.Repository.Leave
{
	public interface ILeaveBalanceRepository
	{
		Task<IEnumerable<dynamic>> GetByEmployee(Guid employeeId);
		Task<IEnumerable<LeaveBalancesModel>> Summary(Guid leaveTypeId);
		Task<IEnumerable<LeaveBalancesModel>> BalanceReport(Guid? deptId, Guid? designId, Guid? teamId, Guid? employeeId, Guid? leaveTypeId);
		Task<IEnumerable<LeaveBalancesModel>> MyTeamLeaveBalanceReport(Guid? designationId, Guid? employeeId, Guid? leaveTypeId, Guid reportingToId);
		Task<IPaginate<LeaveBalanceDetailsModel>> LeaveBalanceDetails(BaseSearch search);
	}

	public class LeaveBalanceRepository : ILeaveBalanceRepository
	{
		private readonly TranSmartContext _context;
		public LeaveBalanceRepository(TranSmartContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<LeaveBalancesModel>> BalanceReport(Guid? deptId, Guid? designId, Guid? teamId, Guid? employeeId, Guid? leaveTypeId)
		{
			return await _context.Leave_LeaveBalance.Where(
								p => (string.IsNullOrEmpty(deptId.ToString()) || p.Employee.DepartmentId == deptId)
								  && (string.IsNullOrEmpty(designId.ToString()) || p.Employee.DesignationId == designId)
								  && (string.IsNullOrEmpty(teamId.ToString()) || p.Employee.TeamId == teamId)
								  && (string.IsNullOrEmpty(leaveTypeId.ToString()) || p.LeaveTypeId == leaveTypeId)
								  && (string.IsNullOrEmpty(employeeId.ToString()) || p.EmployeeId == employeeId)
								  && (p.EffectiveFrom <= DateTime.Today)
								  && (p.EffectiveTo >= DateTime.Today))
				.Include(x => x.Employee)
				.GroupBy(g => new { g.EmployeeId, g.LeaveTypeId })
				.Select(s => new
				{
					Balance = s.Sum(x => x.Leaves),
					s.Key.EmployeeId,
					s.Key.LeaveTypeId,
				}).Join(_context.Leave_LeaveType.Where(x => x.Status), g => g.LeaveTypeId, t => t.ID,
				(g, t) => new
				{
					Balance = g.Balance,
					LeaveType = t.Name,
					g.EmployeeId,
				}).Join(_context.Organization_Employee.Include(c => c.Designation), g => g.EmployeeId, t => t.ID,
				(g, t) => new LeaveBalancesModel
				{
					Balance = g.Balance,
					LeaveType = g.LeaveType,
					Name = t.Name,
					No = t.No,
					Designation = t.Designation.Name,
					ID = t.ID
				})
				.ToListAsync();
		}

		public async Task<IPaginate<LeaveBalanceDetailsModel>> LeaveBalanceDetails(BaseSearch search)
		{
			return await _context.Leave_LeaveBalance.Where(
								p => p.EmployeeId == search.RefId)
				.Include(x => x.Employee)
				.GroupBy(g => new { g.EmployeeId, g.LeaveTypeId, g.EffectiveFrom, g.EffectiveTo })
				.Select(s => new
				{
					Balance = s.Sum(x => x.Leaves),
					s.Key.EmployeeId,
					s.Key.LeaveTypeId,
					s.Key.EffectiveFrom,
					s.Key.EffectiveTo
				}).Where(x => x.Balance > 0).OrderByDescending(x => x.EffectiveFrom)
				.Join(_context.Leave_LeaveType.Where(x => x.Status), g => g.LeaveTypeId, t => t.ID,
				(g, t) => new LeaveBalanceDetailsModel
				{
					Leaves = g.Balance,
					Name = t.Name,
					EffectiveFrom = g.EffectiveFrom,
					EffectiveTo = g.EffectiveTo
				}).ToPaginateAsync(index: search.Page, size: search.Size);
		}

		public async Task<IEnumerable<dynamic>> GetByEmployee(Guid employeeId)
		{
			return await _context.Leave_LeaveBalance.Where(x => x.EmployeeId == employeeId
								  && (x.EffectiveFrom <= DateTime.Today)
								  && (x.EffectiveTo >= DateTime.Today)
								  && !x.LeaveType.DefaultPayoff)
				 .GroupBy(g => new { g.LeaveTypeId })
				.Select(s => new
				{
					Available = s.Sum(x => x.Leaves),
					s.Key.LeaveTypeId
				}).Join(_context.Leave_LeaveType.Where(x => x.Status), g => g.LeaveTypeId, t => t.ID,
				(g, t) => new
				{
					t.ID,
					LeaveType = t.Name,
					g.Available,
					t.Code,
				}).ToListAsync();
		}

		public async Task<decimal> GetByEmployeeLeaveType(Guid employeeId, Guid leaveTypeId)
		{
			return await _context.Leave_LeaveBalance
				.Where(x => x.EmployeeId == employeeId && x.LeaveTypeId == leaveTypeId).SumAsync(x => x.Leaves);
		}

		public async Task<IEnumerable<LeaveBalancesModel>> Summary(Guid leaveTypeId)
		{
			return await _context.Leave_LeaveBalance.Where(x => x.LeaveTypeId == leaveTypeId)
				.GroupBy(g => new { g.EmployeeId, g.LeaveTypeId })
			   .Select(s => new
			   {
				   Balance = s.Sum(x => x.Leaves),
				   EmployeeId = s.Key.EmployeeId
			   }).Join(_context.Organization_Employee, g => g.EmployeeId, t => t.ID,
			   (g, t) => new LeaveBalancesModel
			   {
				   Balance = g.Balance,
				   Name = t.Name,
				   ID = t.ID,
				   LeaveType = t.Name,
				   Leaves = g.Balance,
			   }).ToListAsync();
		}
		public async Task<IEnumerable<LeaveBalancesModel>> MyTeamLeaveBalanceReport(Guid? designationId, Guid? employeeId, Guid? leaveTypeId, Guid reportingToId)
		{
			return await _context.Leave_LeaveBalance.Where(
				p => (string.IsNullOrEmpty(designationId.ToString()) || p.Employee.DesignationId == designationId)
								   && (string.IsNullOrEmpty(leaveTypeId.ToString()) || p.LeaveTypeId == leaveTypeId)
								   && (string.IsNullOrEmpty(employeeId.ToString()) || p.EmployeeId == employeeId)
								   && (p.EffectiveFrom <= DateTime.Today)
								   && (p.EffectiveTo >= DateTime.Today)
								   && (p.Employee.ReportingToId == reportingToId))
				.Include(x => x.Employee)
				.GroupBy(g => new { g.EmployeeId, g.LeaveTypeId })
				.Select(s => new
				{
					Balance = s.Sum(x => x.Leaves),
					s.Key.EmployeeId,
					s.Key.LeaveTypeId,
				}).Join(_context.Leave_LeaveType.Where(x => x.Status), g => g.LeaveTypeId, t => t.ID,
				(g, t) => new
				{
					g.Balance,
					LeaveType = t.Name,
					g.EmployeeId,
				}).Join(_context.Organization_Employee.Include(c => c.Designation), g => g.EmployeeId, t => t.ID,
				(g, t) => new LeaveBalancesModel
				{
					Balance = g.Balance,
					LeaveType = g.LeaveType,
					Name = t.Name,
					No = t.No,
					Designation = t.Designation.Name,
					ID = t.ID
				}).Where(x => x.Balance > 0).ToListAsync();
		}
	}
}
