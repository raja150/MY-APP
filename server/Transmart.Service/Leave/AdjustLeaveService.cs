
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TranSmart.Service.Leave
{
	public partial interface IAdjustLeaveService : IBaseService<AdjustLeave>
	{
		public Task<Result<AdjustLeave>> AddBulk(List<AdjustLeave> items);
	}

	public partial class AdjustLeaveService : BaseService<AdjustLeave>, IAdjustLeaveService
	{
		public async Task<Result<AdjustLeave>> AddBulk(List<AdjustLeave> items)
		{
			var result = new Result<AdjustLeave>();
			foreach (AdjustLeave item in items)
			{
				LeaveBalance PresentBalance = new();
				PresentBalance.CustomizedBal = item;
				PresentBalance.LeaveTypeId = item.LeaveTypeId;
				PresentBalance.EmployeeId = item.EmployeeId;
				PresentBalance.Leaves = item.NewBalance;
				PresentBalance.Type = (int)Data.LeaveTypesScreens.CustomizeLeaveBalance;
				PresentBalance.EffectiveFrom = item.EffectiveFrom;
				PresentBalance.EffectiveTo = item.EffectiveTo;
				PresentBalance.LeavesAddedOn = TimeStamp();
				await UOW.GetRepositoryAsync<LeaveBalance>().AddAsync(PresentBalance);
			}
			await UOW.GetRepositoryAsync<AdjustLeave>().AddAsync(items);
			await UOW.SaveChangesAsync();
			return result;
		}

		public override async Task OnBeforeAdd(AdjustLeave item, Result<AdjustLeave> executionResult)
		{
			var leavesSum = await UOW.GetRepositoryAsync<LeaveBalance>().SumOfDecimalAsync(x => x.LeaveTypeId == item.LeaveTypeId
									&& x.EmployeeId == item.EmployeeId, sumBy: x => x.Leaves);

			if (leavesSum + item.NewBalance >= 0)
			{
				var leaveBalance = new LeaveBalance
				{
					CustomizedBal = item,
					LeaveTypeId = item.LeaveTypeId,
					EmployeeId = item.EmployeeId,
					Leaves = item.NewBalance,
					Type = (int)LeaveTypesScreens.CustomizeLeaveBalance,
					EffectiveFrom = item.EffectiveFrom,
					EffectiveTo = item.EffectiveTo,
					LeavesAddedOn = TimeStamp()
				};

				await UOW.GetRepositoryAsync<LeaveBalance>().AddAsync(leaveBalance);
				await base.OnBeforeAdd(item, executionResult);
			}
			else
			{
				executionResult.AddMessageItem(new MessageItem("New Balance should not be less than the adjusted Leave Balance"));
			}

		}
		public override async Task OnBeforeUpdate(AdjustLeave item, Result<AdjustLeave> executionResult)
		{

			var leaveBalanceSum = await UOW.GetRepositoryAsync<LeaveBalance>().SumOfDecimalAsync(
							x => x.LeaveTypeId == item.LeaveTypeId && x.EmployeeId == item.EmployeeId, sumBy: x => x.Leaves);
			var existingRecord = await UOW.GetRepositoryAsync<LeaveBalance>().SingleAsync(x => x.CustomizedBalId == item.ID);

			if (leaveBalanceSum - existingRecord.Leaves + item.NewBalance >= 0)
			{
				LeaveBalance updateLeaveBalance = await UOW.GetRepositoryAsync<LeaveBalance>().SingleAsync(x => x.CustomizedBalId == item.ID);
				updateLeaveBalance.LeaveTypeId = item.LeaveTypeId;
				updateLeaveBalance.EmployeeId = item.EmployeeId;
				updateLeaveBalance.Leaves = item.NewBalance;
				updateLeaveBalance.EffectiveFrom = item.EffectiveFrom;
				updateLeaveBalance.EffectiveTo = item.EffectiveTo;
				UOW.GetRepositoryAsync<LeaveBalance>().UpdateAsync(updateLeaveBalance);
				await base.OnBeforeUpdate(item, executionResult);
			}
			else
			{
				executionResult.AddMessageItem(new MessageItem("Already Used Leaves"));
			}
		}
	}
}
