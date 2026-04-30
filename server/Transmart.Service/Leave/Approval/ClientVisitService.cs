using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.SelfService;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave.Search;

namespace TranSmart.Service.Leave.Approval
{
	public interface IApprovalClientVisitsService : IBaseService<ApplyClientVisits>
	{
		Task<IPaginate<ApplyClientVisits>> ClientVisit(ApplyClientVisitSearch search);
		//Task<Result<ApplyClientVisits>> Approve(Guid Id, Guid approvedEmpId, bool isApproved, string rejectReson);
		Task<ApplyClientVisits> GetClientVisit(Guid Id, Guid approverId);
		Task<Result<ApplyClientVisits>> Approve(Guid Id, Guid approvedEmpId, bool isAdminRequest);
		Task<Result<ApplyClientVisits>> Reject(Guid Id, string rejectReson, Guid rejectedEmpId);

	}
	public class ApprovalClientVisitsService : BaseService<ApplyClientVisits>, IApprovalClientVisitsService
	{
		public ApprovalClientVisitsService(IUnitOfWork uow) : base(uow)
		{

		}

		public async Task<IPaginate<ApplyClientVisits>> ClientVisit(ApplyClientVisitSearch search)
		{
			return await UOW.GetRepositoryAsync<ApplyClientVisits>().GetPaginateAsync(
				predicate: x => (string.IsNullOrEmpty(search.Name) || x.Employee.No.Contains(search.Name))
				&& (string.IsNullOrEmpty(search.FromDate.ToString()) || x.FromDate.Date >= search.FromDate.Value.Date)
			   && (string.IsNullOrEmpty(search.ToDate.ToString()) || x.ToDate.Date <= search.ToDate.Value.Date)
				&& (x.Employee.ReportingToId == search.RefId || x.Employee.ReportingTo.ReportingToId == search.RefId),
				//&& (!search.Status.HasValue || x.Status == search.Status),
			   orderBy: o => o.OrderByDescending(x => x.AddedAt),
			   include: i => i.Include(x => x.Employee).ThenInclude(x => x.Designation),
				index: search.Page, size: search.Size);
		}

		//public async Task<Result<ApplyClientVisits>> Approve(Guid Id, Guid approvedEmpId, bool isApproved, string rejectReson)
		//{
		//	var result = new Result<ApplyClientVisits>();
		//	var applyVisit = await UOW.GetRepositoryAsync<ApplyClientVisits>().SingleAsync(x => x.ID == Id);
		//	if (applyVisit == null)
		//	{
		//		result.AddMessageItem(new MessageItem(Resource.Invalid_Item));
		//		return result;
		//	}
		//	if (applyVisit.Status != (int)ClientVisitStatus.Applied)
		//	{
		//		result.AddMessageItem(new MessageItem(applyVisit.Status == 1 ? Resource.Client_Visit_Already_Approved : Resource.Client_Visit_Already_Rejected));
		//		return result;
		//	}
		//	var employeeEntity = await UOW.GetRepositoryAsync<Employee>().SingleAsync(x => x.ID == applyVisit.EmployeeId,
		//		include: x => x.Include(o => o.ReportingTo));
		//	if (employeeEntity != null && approvedEmpId != employeeEntity.ReportingToId && approvedEmpId != employeeEntity.ReportingTo.ReportingToId)
		//	{
		//		result.AddMessageItem(new MessageItem(Resource.You_DoNot_Have_Permission));
		//		return result;
		//	}
		//	if (isApproved)
		//	{
		//		applyVisit.Status = (int)ClientVisitStatus.Approved;
		//	}
		//	else
		//	{
		//		applyVisit.Status = (int)ClientVisitStatus.Rejected;
		//		applyVisit.AdminReason = rejectReson;
		//	}
		//	result = await base.UpdateAsync(applyVisit);

		//	return result;
		//}
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
			entity.Status = (int)ClientVisitStatus.Approved;
			entity.ApprovedById = approvedEmpId;
			return await base.UpdateAsync(entity);
		}
		public async Task<Result<ApplyClientVisits>> Reject(Guid Id, string rejectReson, Guid rejectedEmpId)
		{
			var result = new Result<ApplyClientVisits>();
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
			return await base.UpdateAsync(entity);
		}

		public async Task<ApplyClientVisits> GetClientVisit(Guid Id, Guid approverId)
		{
			return await UOW.GetRepositoryAsync<ApplyClientVisits>().SingleAsync(x => x.ID == Id
				  && (x.Employee.ReportingToId == approverId || x.Employee.ReportingTo.ReportingToId == approverId),
				   include: x => x.Include(i => i.Employee).ThenInclude(x => x.Designation));
		}
	}
}
