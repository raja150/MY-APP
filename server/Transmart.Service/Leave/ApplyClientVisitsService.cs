using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave.Search;

namespace TranSmart.Service.Leave
{
	public partial interface IApplyClientVisitsService : IBaseService<ApplyClientVisits>
	{
		Task<Result<ApplyClientVisits>> Cancel(Guid id, Guid employeeId);
		Task<Result<ApplyClientVisits>> Approve(Guid Id, Guid approvedEmpId, bool isAdminRequest);
		Task<Result<ApplyClientVisits>> Reject(Guid Id, string rejectReson, Guid rejectedEmpId);
		Task<IPaginate<ApplyClientVisits>> GetLMPaginate(BaseSearch baseSearch);
		Task<IEnumerable<ApplyClientVisits>> GetPastFutureVisits(Guid employeeId, DateTime fromDate, DateTime toDate);
		Task<IPaginate<ApplyClientVisits>> ClientVisit(ApplyClientVisitSearch search);
		Task UpdateAttendanceIfApproved(ApplyClientVisits visits);
	}
	public partial class ApplyClientVisitsService : BaseService<ApplyClientVisits>, IApplyClientVisitsService
	{
		private readonly IAttendanceService _attService;
		public ApplyClientVisitsService(IUnitOfWork uow, IAttendanceService attendanceService) : base(uow)
		{
			_attService = attendanceService;
		}
		#region SelfService
		public override async Task<ApplyClientVisits> GetById(Guid id)
		{
			return await UOW.GetRepositoryAsync<ApplyClientVisits>().SingleAsync(
				predicate: x => x.ID == id,
				include: i => i.Include(x => x.Employee).ThenInclude(x => x.Designation),
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
		}

		public override async Task<IPaginate<ApplyClientVisits>> GetPaginate(BaseSearch baseSearch)
		{
			ApplyClientVisitSearch ClientVisitSearch = (ApplyClientVisitSearch)baseSearch;
			return await UOW.GetRepositoryAsync<ApplyClientVisits>().GetPageListAsync(
			   predicate: x => (string.IsNullOrEmpty(ClientVisitSearch.Name) || x.PlaceOfVisit.Contains(baseSearch.Name))
				&& (string.IsNullOrEmpty(ClientVisitSearch.FromDate.ToString()) || x.FromDate.Date >= ClientVisitSearch.FromDate.Value.Date)
				&& (string.IsNullOrEmpty(ClientVisitSearch.ToDate.ToString()) || x.ToDate.Date <= ClientVisitSearch.ToDate.Value.Date)
				&& x.EmployeeId == ClientVisitSearch.RefId,
			   include: i => i.Include(x => x.Employee).Include(x => x.ApprovedBy),
			  index: baseSearch.Page, size: baseSearch.Size, sortBy: baseSearch.SortBy ?? "PlaceOfVisit", ascending: !baseSearch.IsDescend);
		}
		public override async Task CustomValidation(ApplyClientVisits item, Result<ApplyClientVisits> result)
		{
			if (result.HasError) return;

			if (item.FromDate > item.ToDate)
			{
				result.AddMessageItem(new MessageItem(nameof(ApplyClientVisits.FromDate), Resource.From_Date_Should_be_less_then_The_To_Date));
			}
			var anyExistVisits = await UOW.GetRepositoryAsync<ApplyClientVisits>().HasRecordsAsync(x => x.EmployeeId == item.EmployeeId
				  && x.ID != item.ID && (x.Status != (int)ClientVisitStatus.Cancelled && x.Status != (int)ClientVisitStatus.Rejected)
				  && item.FromDate.Date <= x.ToDate.Date && item.ToDate.Date >= x.FromDate.Date);

			if (anyExistVisits)
			{

				result.AddMessageItem(new MessageItem
					(nameof(ApplyClientVisits.FromDate), Resource.Client_Visit_Already_Applied));
			}
			var leaveApproved = await UOW.GetRepositoryAsync<ApplyLeave>().HasRecordsAsync(x => x.EmployeeId == item.EmployeeId
																&& (x.Status == (int)ApplyLeaveSts.Approved || x.Status == (int)ApplyLeaveSts.Applied)
																&& item.FromDate.Date <= x.ToDate.Date && item.ToDate.Date >= x.FromDate.Date);
			if (leaveApproved)
			{
				result.AddMessageItem(new MessageItem(nameof(ApplyClientVisits.FromDate), Resource.Already_Leves_Applied_For_this_Dates));
			}
			var wfhApplied = await UOW.GetRepositoryAsync<ApplyWfh>().HasRecordsAsync(x => x.EmployeeId == item.EmployeeId
															&& (x.Status == (int)WfhStatus.Applied || x.Status == (int)WfhStatus.Approved)
															&& item.FromDate.Date <= x.ToDateC.Date && item.ToDate.Date >= x.FromDateC.Date);
			if (wfhApplied)
			{
				result.AddMessageItem(new MessageItem(nameof(ApplyClientVisits.FromDate), Resource.Already_WFH_Applied_For_this_Dates));
			}

			await base.CustomValidation(item, result);
		}

		public async Task<Result<ApplyClientVisits>> Cancel(Guid id, Guid employeeId)
		{
			var result = new Result<ApplyClientVisits>();
			try
			{

				ApplyClientVisits entity = await UOW.GetRepositoryAsync<ApplyClientVisits>().SingleAsync(x => x.ID == id);
				if (entity == null || entity.EmployeeId != employeeId)
				{
					result.AddMessageItem(new MessageItem(Resource.Invalid));
					return result;
				}
				if (entity.Status == (int)ClientVisitStatus.Cancelled) return result;
				if (entity.Status != (int)ClientVisitStatus.Applied)
				{
					result.AddMessageItem(new MessageItem(entity.Status == (int)WfhStatus.Approved ?
						Resource.Client_Visit_Already_Approved : Resource.Client_Visit_Already_Rejected));
					return result;
				}
				entity.Status = (int)ClientVisitStatus.Cancelled;
				UOW.GetRepositoryAsync<ApplyClientVisits>().UpdateAsync(entity);
				await UOW.SaveChangesAsync();
				result.IsSuccess = true;
				result.ReturnValue = entity;
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex.Message));
			}
			return result;
		}

		#endregion

		#region LeaveManagement
		public async Task<IPaginate<ApplyClientVisits>> GetLMPaginate(BaseSearch baseSearch)
		{
			ApplyClientVisitSearch leaveSearch = (ApplyClientVisitSearch)baseSearch;
			leaveSearch.Status = leaveSearch.Status == 0 ? (int)ClientVisitStatus.Applied : leaveSearch.Status;
			return await UOW.GetRepositoryAsync<ApplyClientVisits>().GetPageListAsync(
				predicate: x => (string.IsNullOrEmpty(baseSearch.Name) || x.Employee.Name.Contains(baseSearch.Name))
						 && (leaveSearch.FromDate == null || x.FromDate.Date >= leaveSearch.FromDate.Value.Date)
						 && (leaveSearch.ToDate == null || x.ToDate.Date <= leaveSearch.ToDate.Value.Date)
						 && (leaveSearch.RefId == null || x.Employee.ReportingToId == leaveSearch.RefId || x.Employee.ReportingTo.ReportingToId == leaveSearch.RefId)
						 && x.Status == leaveSearch.Status,
				  include: i => i.Include(x => x.Employee).ThenInclude(x => x.Department).Include(x => x.Employee).ThenInclude(x => x.Designation).Include(x => x.ApprovedBy),
				index: baseSearch.Page, size: baseSearch.Size, sortBy: baseSearch.SortBy ?? "FromDate", ascending: !baseSearch.IsDescend);

		}
		public async Task<IEnumerable<ApplyClientVisits>> GetPastFutureVisits(Guid employeeId, DateTime fromDate, DateTime toDate)
		{
			return await UOW.GetRepositoryAsync<ApplyClientVisits>().GetAsync(
				predicate: p => p.EmployeeId == employeeId && p.FromDate >= fromDate.AddDays(-7) && p.ToDate <= toDate.AddDays(7),
				 include: i => i.Include(x => x.Employee).ThenInclude(x => x.Designation),
				orderBy: o => o.OrderBy(x => x.FromDate));
		}
		public async Task UpdateAttendanceIfApproved(ApplyClientVisits visits)
		{
			for (var attDate = visits.FromDate; attDate < visits.ToDate; attDate = attDate.AddDays(1))
			{
				var att = await UOW.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == visits.EmployeeId && x.AttendanceDate.Date == attDate.Date);
				if (att != null)
				{
					await _attService.ChangeAttStatus(att, (int)att.AttendanceStatus, 1, 0, 0, 0, false, 0, false, null, 0);
				}
			}
		}
		public async Task<Result<ApplyClientVisits>> Approve(Guid Id, Guid approvedEmpId, bool isAdminRequest)
		{
			var result = new Result<ApplyClientVisits>();

			ApplyClientVisits entity = await UOW.GetRepositoryAsync<ApplyClientVisits>().SingleAsync(x => x.ID == Id);

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

			if (entity.Status != (int)ClientVisitStatus.Applied)
			{
				result.AddMessageItem(new MessageItem(entity.Status == (int)ClientVisitStatus.Approved ? Resource.Client_Visit_Already_Approved : Resource.Client_Visit_Already_Rejected));
				return result;
			}
			await UpdateAttendanceIfApproved(entity);
			entity.Status = (int)ClientVisitStatus.Approved;
			entity.ApprovedById = approvedEmpId;
			return await base.UpdateAsync(entity);
		}
		public async Task<Result<ApplyClientVisits>> Reject(Guid Id, string rejectReson, Guid rejectedEmpId)
		{
			var result = new Result<ApplyClientVisits>();
			try
			{
				ApplyClientVisits entity = await UOW.GetRepositoryAsync<ApplyClientVisits>().SingleAsync(x => x.ID == Id);

				var attendance = await UOW.GetRepositoryAsync<Attendance>().HasRecordsAsync(x => x.EmployeeId == entity.EmployeeId
																				&& x.AttendanceDate.Date >= entity.FromDate.Date
																				&& x.AttendanceDate.Date <= entity.ToDate.Date);
				if (attendance)
				{
					result.AddMessageItem(new MessageItem(Resource.Attendance_Already_Executed));
					return result;
				}
				entity.Status = (int)ClientVisitStatus.Rejected;
				entity.AdminReason = rejectReson;
				entity.ApprovedById = rejectedEmpId;
				UOW.GetRepositoryAsync<ApplyClientVisits>().UpdateAsync(entity);
				await UOW.SaveChangesAsync();
				result.IsSuccess = true;
				result.ReturnValue = entity;
			}
			catch(Exception ex)
			{
				 result.AddMessageItem(new MessageItem(ex.Message));
			}
			return result;
		}

		#endregion

		#region Approve
		public async Task<IPaginate<ApplyClientVisits>> ClientVisit(ApplyClientVisitSearch search)
		{
			return await UOW.GetRepositoryAsync<ApplyClientVisits>().GetPaginateAsync(
				predicate: x => (string.IsNullOrEmpty(search.Name) || x.Employee.No.Contains(search.Name))
				&& (string.IsNullOrEmpty(search.FromDate.ToString()) || x.FromDate.Date >= search.FromDate.Value.Date)
			   && (string.IsNullOrEmpty(search.ToDate.ToString()) || x.ToDate.Date <= search.ToDate.Value.Date)
				&& x.Employee.ReportingToId == search.RefId,
			   //&& (!search.Status.HasValue || x.Status == search.Status),
			   orderBy: o => o.OrderByDescending(x => x.AddedAt),
			   include: i => i.Include(x => x.Employee).ThenInclude(x => x.Designation),
				index: search.Page, size: search.Size);
		}
		#endregion
	}
}
