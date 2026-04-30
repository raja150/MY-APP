using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Core.Util;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave.Search;

namespace TranSmart.Service.Leave
{
	public partial interface IApplyWfhService : IBaseService<ApplyWfh>
	{
		Task<IPaginate<ApplyWfh>> SelfServiceSearch(BaseSearch search);
		Task<Result<ApplyWfh>> Approve(Guid Id, Guid approvedEmpId, bool isAdminRequest);
		Task<Result<ApplyWfh>> Reject(Guid Id, string rejectReson, Guid rejectedEmpId);
		Task<Result<ApplyWfh>> Cancel(Guid id, Guid employeeId);
		Task<ApplyWfh> GetLeave(Guid Id, Guid approverId);
		Task<IEnumerable<ApplyWfh>> GetPastFutureWFH(Guid employeeId, DateTime fromDate, DateTime toDate);
		Task<Result<ApplyWfh>> RejectAfterApprove(Guid id, string rejectReason, Guid rejectedEmpId);
	}
	public partial class ApplyWfhService : BaseService<ApplyWfh>, IApplyWfhService
	{
		private readonly IAttendanceService _attService;
		public ApplyWfhService(IUnitOfWork uow, IAttendanceService attService) : base(uow)
		{
			_attService = attService;
		}
		public override async Task<ApplyWfh> GetById(Guid id)
		{
			return await UOW.GetRepositoryAsync<ApplyWfh>().SingleAsync(
				predicate: x => x.ID == id,
				include: i => i.Include(x => x.Employee).ThenInclude(x => x.Designation),
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
		}
		public async Task<IEnumerable<ApplyWfh>> GetPastFutureWFH(Guid employeeId, DateTime fromDate, DateTime toDate)
		{
			return await UOW.GetRepositoryAsync<ApplyWfh>().GetAsync(
				predicate: p => p.EmployeeId == employeeId && p.FromDateC >= fromDate.AddDays(-7) && p.ToDateC <= toDate.AddDays(7),
				 include: i => i.Include(x => x.Employee).ThenInclude(x => x.Designation),
				orderBy: o => o.OrderBy(x => x.FromDateC));
		}
		public async Task<ApplyWfh> GetLeave(Guid Id, Guid approverId)
		{
			return await UOW.GetRepositoryAsync<ApplyWfh>().SingleAsync(x => x.ID == Id
				   && (x.Employee.ReportingToId == approverId || x.Employee.ReportingTo.ReportingToId == approverId),
					include: x => x.Include(i => i.Employee).ThenInclude(x => x.Designation)
					.Include(x => x.Employee).ThenInclude(x => x.ReportingTo));
		}
		public override async Task<IPaginate<ApplyWfh>> GetPaginate(BaseSearch baseSearch)
		{
			ApplyWfhSearch leaveSearch = (ApplyWfhSearch)baseSearch;
			leaveSearch.Status = leaveSearch.Status == 0 ? (int)WfhStatus.Applied : leaveSearch.Status;
			return await UOW.GetRepositoryAsync<ApplyWfh>().GetPageListAsync(
				predicate: x => (string.IsNullOrEmpty(baseSearch.Name) || x.Employee.Name.Contains(baseSearch.Name))
						 && (leaveSearch.FromDate == null || x.FromDateC.Date >= leaveSearch.FromDate.Value.Date)
						 && (leaveSearch.ToDate == null || x.ToDateC.Date <= leaveSearch.ToDate.Value.Date)
						 && (leaveSearch.RefId == null || x.Employee.ReportingToId == leaveSearch.RefId || x.Employee.ReportingTo.ReportingToId == leaveSearch.RefId)
						 && x.Status == leaveSearch.Status,
				  include: i => i.Include(x => x.Employee).ThenInclude(x => x.Department)
												.Include(x => x.Employee).ThenInclude(x => x.Designation).Include(x => x.ApprovedBy)
												.Include(x => x.Employee).ThenInclude(x => x.ReportingTo),
				index: baseSearch.Page, size: baseSearch.Size, sortBy: baseSearch.SortBy ?? "FromDateC", ascending: !baseSearch.IsDescend);

		}
		public virtual async Task<IPaginate<ApplyWfh>> SelfServiceSearch(BaseSearch search)
		{
			ApplyWfhSearch leaveSearch = (ApplyWfhSearch)search;
			return await UOW.GetRepositoryAsync<ApplyWfh>().GetPaginateAsync(
				predicate: x => (string.IsNullOrEmpty(leaveSearch.Name) || x.Employee.Name.Contains(search.Name))
				&& (leaveSearch.FromDate == null || x.FromDateC.Date >= leaveSearch.FromDate.Value.Date)
				&& (leaveSearch.ToDate == null || x.ToDateC.Date <= leaveSearch.ToDate.Value.Date)
				&& x.EmployeeId == leaveSearch.RefId,
				include: i => i.Include(x => x.Employee).Include(x => x.ApprovedBy),
				orderBy: o => o.OrderByDescending(x => x.FromDateC).ThenByDescending(m => m.Status),
				index: search.Page, size: search.Size);
		}
		public async Task<Result<ApplyWfh>> Approve(Guid Id, Guid approvedEmpId, bool isAdminRequest)
		{
			var result = new Result<ApplyWfh>();

			ApplyWfh entity = await UOW.GetRepositoryAsync<ApplyWfh>().SingleAsync(x => x.ID == Id);
			if (!isAdminRequest)
			{
				var employeeEntity = await UOW.GetRepositoryAsync<Employee>().SingleAsync(x => x.ID == entity.EmployeeId,
				include: x => x.Include(o => o.ReportingTo));
				if (employeeEntity != null && approvedEmpId != employeeEntity.ReportingToId && approvedEmpId != employeeEntity.ReportingTo.ReportingToId)
				{
					result.AddMessageItem(new MessageItem(Resource.You_DoNot_Have_Permission));
					return result;
				}
			}

			if (entity.Status != (int)WfhStatus.Applied)
			{
				result.AddMessageItem(new MessageItem(entity.Status == (int)WfhStatus.Approved ? Resource.WFH_Already_Approved : Resource.WFH_Already_Rejected));
				return result;
			}
			entity.Status = (int)WfhStatus.Approved;
			entity.ApprovedById = approvedEmpId;

			// If WFH approved for old dates then updating the required changes in attendance
			for (DateTime attDate = entity.FromDateC; attDate <= entity.ToDateC; attDate = attDate.AddDays(1))
			{
				var isHalfDay = (attDate.Date == entity.FromDateC.Date && entity.FromHalf)
								|| (attDate.Date == entity.ToDateC.Date && entity.ToHalf);


				var isFirstHalf = Leave_WFH_Util.IsFirstHalf(entity.FromDateC, entity.ToDateC, entity.FromHalf, attDate.Date);

				Attendance att = await UOW.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == entity.EmployeeId && x.AttendanceDate.Date == attDate.Date);

				var remainingHalf = 0;
				if (att != null && attDate.Date < DateTime.Now.Date)
				{
					if (isHalfDay)
					{
						remainingHalf = _attService.GetAttRemainingHalfDay((int)att.AttendanceStatus);
					}

					if (isHalfDay)
					{
						await _attService.ChangeAttStatus(att, isFirstHalf ? (int)AttendanceStatus.HalfDayWFH : remainingHalf, 0, 0, 0, 0, true,
														   isFirstHalf ? remainingHalf : (int)AttendanceStatus.HalfDayWFH, isFirstHalf, att.LeaveTypeID, 0);
					}
					else
					{
						await _attService.ChangeAttStatus(att, (int)AttendanceStatus.WFH, 0, 0, 0, 1, false, 0, false, null, 0);
					}
				}
			}
			try
			{
				UOW.GetRepositoryAsync<ApplyWfh>().UpdateAsync(entity);
				await UOW.SaveChangesAsync();
				result.IsSuccess = true;
				result.ReturnValue = entity;
				return result;
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex.Message));
			}
			return result;
		}
		public async Task<Result<ApplyWfh>> Reject(Guid Id, string rejectReson, Guid rejectedEmpId)
		{
			var result = new Result<ApplyWfh>();
			ApplyWfh entity = await UOW.GetRepositoryAsync<ApplyWfh>().SingleAsync(x => x.ID == Id);

			if (entity.Status != (int)WfhStatus.Applied)
			{
				result.AddMessageItem(new MessageItem(entity.Status == (int)WfhStatus.Approved ? Resource.WFH_Already_Approved : Resource.WFH_Already_Rejected));
				return result;
			}
			entity.Status = (int)WfhStatus.Rejected;
			entity.AdminReason = rejectReson;
			entity.ApprovedById = rejectedEmpId;
			try
			{
				UOW.GetRepositoryAsync<ApplyWfh>().UpdateAsync(entity);
				await UOW.SaveChangesAsync();
				result.IsSuccess = true;
				result.ReturnValue = entity;
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}
			return result;

		}
		public async Task<Result<ApplyWfh>> RejectAfterApprove(Guid id, string rejectReason, Guid rejectedEmpId)
		{
			var result = new Result<ApplyWfh>();

			var entity = await UOW.GetRepositoryAsync<ApplyWfh>().SingleAsync(x => x.ID == id);
			var attendance = await UOW.GetRepositoryAsync<Attendance>().HasRecordsAsync(x => x.EmployeeId == entity.EmployeeId
																		&& x.AttendanceDate.Date >= entity.FromDateC.Date
																		&& x.AttendanceDate.Date <= entity.ToDateC.Date);
			if (attendance)
			{
				result.AddMessageItem(new MessageItem(Resource.Attendance_Already_Executed));
				return result;
			}
			entity.Status = (int)WfhStatus.Rejected;
			entity.AdminReason = rejectReason;
			entity.ApprovedById = rejectedEmpId;
			try
			{
				UOW.GetRepositoryAsync<ApplyWfh>().UpdateAsync(entity);
				await UOW.SaveChangesAsync();
				result.IsSuccess = true;
				result.ReturnValue = entity;
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}
			return result;
		}
		public async Task<Result<ApplyWfh>> Cancel(Guid id, Guid employeeId)
		{
			var result = new Result<ApplyWfh>();
			try
			{

				ApplyWfh entity = await UOW.GetRepositoryAsync<ApplyWfh>().SingleAsync(x => x.ID == id);
				if (entity == null || entity.EmployeeId != employeeId)
				{
					result.AddMessageItem(new MessageItem(Resource.Invalid));
					return result;
				}
				if (entity.Status == (int)WfhStatus.Cancelled) return result;
				if (entity.Status != (int)WfhStatus.Applied)
				{
					result.AddMessageItem(new MessageItem(entity.Status == (int)WfhStatus.Approved ?
						Resource.WFH_Already_Approved : Resource.WFH_Already_Rejected));
					return result;
				}
				entity.Status = (int)WfhStatus.Cancelled;
				UOW.GetRepositoryAsync<ApplyWfh>().UpdateAsync(entity);
				await UOW.SaveChangesAsync();
				result.IsSuccess = true;
				result.ReturnValue = entity;
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}
			return result;
		}
		public override async Task CustomValidation(ApplyWfh item, Result<ApplyWfh> result)
		{
			if (result.HasError) return;
			if ((item.ToDateC.Date - item.FromDateC.Date).TotalDays > 5)
			{
				result.AddMessageItem(new MessageItem(Resource.Cannot_Apply_WFH_More_Than_Six_Days));
			}

			#region ApplyLeave Validation
			//List of Applied Leave's in between fromDate and toDate
			var applyLeave = await UOW.GetRepositoryAsync<ApplyLeave>().GetAsync(x => x.EmployeeId == item.EmployeeId
															&& (x.Status == (int)ApplyLeaveSts.Approved || x.Status == (int)ApplyLeaveSts.Applied)
															&& item.FromDateC.Date <= x.ToDate.Date && item.ToDateC.Date >= x.FromDate.Date);
			//Applied leave is more then one day(fromDate != toDate) then fromHalf represents secondHalf, toHalf represents firstHalf
			//if now applied wfh for oneDay(fromDate == toDate) then fromHalf represents firstHalf and toHalf represents secondHalf
			//any wfh Date Matched or halfDays matched Then we are throwing exception
			if (applyLeave.Any(x => x.FromDate != x.ToDate))
			{
				var leaveValidate = applyLeave.Any(x => x.FromDate.Date == item.FromDateC.Date && x.FromHalf == item.ToHalf
															|| x.ToDate.Date == item.ToDateC.Date && x.ToHalf == item.FromHalf
															|| x.FromDate.Date == item.FromDateC.Date && x.FromDate == item.ToDateC && !item.ToHalf && !item.FromHalf
															|| x.FromDate.Date == item.FromDateC.Date && x.FromDate == item.ToDateC && item.ToHalf && !item.FromHalf
															|| x.ToDate.Date == item.FromDateC.Date && x.ToDate == item.ToDateC && !item.ToHalf && !item.FromHalf
															|| x.ToDate.Date == item.FromDateC.Date && x.ToDate == item.ToDateC && !item.ToHalf && item.FromHalf);
				if ((item.FromDateC != item.ToDateC && applyLeave.Any()) || leaveValidate)
				{
					result.AddMessageItem(new MessageItem(nameof(ApplyWfh.FromDateC), Resource.Already_Leves_Applied_For_this_Dates));
					return;
				}
				//applied one day wfh,which is in between applied leave dates,throwing exception
				if (applyLeave.Any(x => item.FromDateC.Date < x.ToDate.Date && item.ToDateC.Date > x.FromDate.Date))
				{
					result.AddMessageItem(new MessageItem(nameof(ApplyWfh.FromDateC), Resource.Already_Leves_Applied_For_this_Dates));
					return;
				}
			}
			//Applied leave is one day(fromDate == toDate) then fromHalf represents firstHalf and toHalf represents secondHalf
			//if now applied wfh for more then one day then fromHalf represents secondHalf and toHalf represents firstHalf
			//we are not allowing to apply wfh if any date matched
			if (applyLeave.Any(x => x.FromDate == x.ToDate))
			{
				var oneDayLeave = applyLeave.Any(x => x.FromDate.Date == item.FromDateC.Date && x.FromHalf == item.FromHalf
																|| x.ToDate.Date == item.ToDateC.Date && x.ToHalf == item.ToHalf);
				if ((item.FromDateC != item.ToDateC && applyLeave.Any()) || oneDayLeave)
				{
					result.AddMessageItem(new MessageItem(nameof(ApplyWfh.FromDateC), Resource.Already_Leves_Applied_For_this_Dates));
					return;
				}
			}
			#endregion

			var clientVisit = await UOW.GetRepositoryAsync<ApplyClientVisits>().HasRecordsAsync(x => x.EmployeeId == item.EmployeeId
															&& (x.Status == (int)ClientVisitStatus.Approved || x.Status == (int)ClientVisitStatus.Applied)
															&& item.FromDateC.Date <= x.ToDate.Date && item.ToDateC.Date >= x.FromDate.Date);
			if (clientVisit)
			{
				result.AddMessageItem(new MessageItem(nameof(ApplyWfh.FromDateC), Resource.Client_Visit_Applied_For_These_Days));
				return;
			}
			var isApproved = await UOW.GetRepositoryAsync<ApplyWfh>().HasRecordsAsync(x => x.ID == item.ID
									&& x.Status != Convert.ToInt32(WfhStatus.Applied));
			if (isApproved)
			{
				result.AddMessageItem(new MessageItem(Resource.Already_Approved));
				return;
			}
			if (item.FromDateC > item.ToDateC)
			{
				result.AddMessageItem(new MessageItem
					(nameof(ApplyWfh.FromDateC), Resource.From_Date_Should_be_less_then_The_To_Date));
				return;
			}
			if (item.FromDateC == item.ToDateC && item.FromHalf && item.ToHalf)
			{
				result.AddMessageItem(new MessageItem(Resource.FirstHalf_Or_SecondHalf_Should_Select));
				return;
			}

			var applyWFHDay = await UOW.GetRepositoryAsync<ApplyWfh>().HasRecordsAsync(x => x.EmployeeId == item.EmployeeId
				 && x.ID != item.ID && x.Status != (int)WfhStatus.Cancelled && x.Status != (int)WfhStatus.Rejected
				 && x.FromDateC <= item.ToDateC && item.FromDateC <= x.ToDateC);

			if (applyWFHDay)
			{
				result.AddMessageItem(new MessageItem
					(nameof(ApplyWfh.FromDateC), Resource.WFH_Already_Approved_Dates));
				return;
			}

			await base.CustomValidation(item, result);
		}
		public override async Task OnBeforeAdd(ApplyWfh item, Result<ApplyWfh> executionResult)
		{
			if (item.Status == (int)WfhStatus.Approved)
			{
				// If WFH approved for old dates then updating the required changes in attendance
				for (DateTime attDate = item.FromDateC; attDate <= item.ToDateC; attDate = attDate.AddDays(1))
				{
					var isHalfDay = (attDate.Date == item.FromDateC.Date && item.FromHalf)
								|| (attDate.Date == item.ToDateC.Date && item.ToHalf);


					var isFirstHalf = Leave_WFH_Util.IsFirstHalf(item.FromDateC, item.ToDateC, item.FromHalf, attDate.Date);

					Attendance att = await UOW.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == item.EmployeeId && x.AttendanceDate.Date == attDate.Date);
					var remainingHalf = 0;
					if (att != null && attDate.Date < DateTime.Now.Date)
					{
						if (isHalfDay)
						{
							remainingHalf = _attService.GetAttRemainingHalfDay((int)att.AttendanceStatus);
							await _attService.ChangeAttStatus(att, isFirstHalf ? (int)AttendanceStatus.HalfDayWFH : remainingHalf, 0, 0, 0, 0, true,
															  isFirstHalf ? remainingHalf : (int)AttendanceStatus.HalfDayWFH, isFirstHalf, att.LeaveTypeID, 0);
						}
						else
						{
							await _attService.ChangeAttStatus(att, (int)AttendanceStatus.WFH, 0, 0, 0, 1, false, 0, false, null, 0);
						}
					}
				}
			}
			await base.OnBeforeAdd(item, executionResult);
		}
	}
}
