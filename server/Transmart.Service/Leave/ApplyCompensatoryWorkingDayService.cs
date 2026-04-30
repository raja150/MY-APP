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
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;

namespace TranSmart.Service.Leave
{
	public partial interface IApplyCompensatoryWorkingDayService : IBaseService<ApplyCompo>
	{
		Task<IPaginate<ApplyCompo>> SelfServiceSearch(BaseSearch search);
	}
	public partial class ApplyCompensatoryWorkingDayService : BaseService<ApplyCompo>, IApplyCompensatoryWorkingDayService
	{
		public ApplyCompensatoryWorkingDayService(IUnitOfWork uow) : base(uow)
		{

		}

		public override async Task<ApplyCompo> GetById(Guid id)
		{
			return await UOW.GetRepositoryAsync<ApplyCompo>().SingleAsync(
				predicate: x => x.ID == id,
				include: i => i.Include(x => x.Shift),
				orderBy: o => o.OrderByDescending(x => x.AddedAt));
		}

		public override async Task<IPaginate<ApplyCompo>> GetPaginate(BaseSearch baseSearch)
		{
			return await UOW.GetRepositoryAsync<ApplyCompo>().GetPageListAsync(
				predicate: x => (string.IsNullOrEmpty(baseSearch.Name)
					 || x.ReasonForApply.Contains(baseSearch.Name)) && (x.EmployeeId == baseSearch.RefId),
				index: baseSearch.Page, size: baseSearch.Size, sortBy: baseSearch.SortBy ?? "FromDate", ascending: !baseSearch.IsDescend);
		}

		public override async Task CustomValidation(ApplyCompo item, Result<ApplyCompo> result)
		{
			var leaveBalance = new LeaveBalance();
			var settings = await UOW.GetRepositoryAsync<LeaveSettings>().SingleAsync();
			//from date and to date are same then it considering as 0
			int NoOfLeaves = (item.ToDate - item.FromDate).Days + 1;
			item.Status = (int)ApplyCompoSts.Applied;

			var applyCompensatoryWorkingDay = await UOW.GetRepositoryAsync<ApplyCompo>().HasRecordsAsync(x => x.EmployeeId == item.EmployeeId
			   && x.ID != item.ID && x.Status != (int)ApplyCompoSts.Applied
			   && ((item.FromDate >= x.FromDate && item.FromDate <= x.ToDate) || (item.ToDate <= x.FromDate && item.ToDate <= x.ToDate)));

			if (item.FromDate > item.ToDate)
			{
				result.AddMessageItem(new MessageItem
					(nameof(ApplyCompo.FromDate), Resource.From_Date_AlwaysGreater_than_or_Equal_to_Today_Date));
			}

			if (applyCompensatoryWorkingDay)
			{
				result.AddMessageItem(new MessageItem
					(nameof(ApplyCompo.FromDate), Resource.Already_Leves_Applied_For_this_Dates));
			}
			if (settings != null && item.Status == 1)
			{
				leaveBalance.ApplyCompensatory = item;
				leaveBalance.LeaveTypeId = settings.CompoLeaveTypeId;
				leaveBalance.EmployeeId = (Guid)item.EmployeeId;
				leaveBalance.Leaves = NoOfLeaves;
				leaveBalance.Type = (int)Data.LeaveTypesScreens.CompensatoryWorkingDay;
				await UOW.GetRepositoryAsync<LeaveBalance>().AddAsync(leaveBalance);
			}
			await base.CustomValidation(item, result);
		}
		public virtual async Task<IPaginate<ApplyCompo>> SelfServiceSearch(BaseSearch search)
		{
			return await UOW.GetRepositoryAsync<ApplyCompo>().GetPaginateAsync(
				predicate: x => x.EmployeeId == search.RefId,
				include: i => i.Include(x => x.Employee).Include(x => x.ApprovedBy),
				index: search.Page, size: search.Size);
		}
	}
}
