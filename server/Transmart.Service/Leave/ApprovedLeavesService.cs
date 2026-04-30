using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Leave;

namespace TranSmart.Service.Leave
{
    public partial class ApprovedLeavesService : BaseService<ApprovedLeaves>
    {
        public override async Task OnBeforeAdd(ApprovedLeaves item, Result<ApprovedLeaves> executionResult)
        {
            var PresentBalance = new LeaveBalance();
            PresentBalance.PreconsumableLeave = item;
            PresentBalance.LeaveTypeId = item.LeaveTypeId;
            PresentBalance.EmployeeId = item.EmployeeId;
            PresentBalance.Leaves = Convert.ToDecimal(item.NoOfLeaves);
            PresentBalance.Type = (int)Data.LeaveTypesScreens.AssignPreLeaves;
            await UOW.GetRepositoryAsync<LeaveBalance>().AddAsync(PresentBalance);

            await base.OnBeforeAdd(item, executionResult);
        }
        public override async Task OnBeforeUpdate(ApprovedLeaves item, Result<ApprovedLeaves> executionResult)
        {
            LeaveBalance approvedLeaves = await UOW.GetRepositoryAsync<LeaveBalance>().SingleAsync(x => x.LeaveTypeId == item.LeaveTypeId);
            approvedLeaves.PreconsumableLeave = item;
            approvedLeaves.LeaveTypeId = item.LeaveTypeId;
            approvedLeaves.EmployeeId = item.EmployeeId;
            approvedLeaves.Leaves = Convert.ToDecimal(item.NoOfLeaves);
            approvedLeaves.Type = (int)Data.LeaveTypesScreens.AssignPreLeaves;

            UOW.GetRepositoryAsync<LeaveBalance>().UpdateAsync(approvedLeaves);
            await base.OnBeforeUpdate(item, executionResult);
        }
        public override async Task CustomValidation(ApprovedLeaves item, Result<ApprovedLeaves> result)
        {
            if (result.HasError) return;
            var approvedLeaves = await UOW.GetRepositoryAsync<ApprovedLeaves>().HasRecordsAsync(x => x.ID != item.ID &&x.EmployeeId == item.EmployeeId && x.LeaveTypeId == item.LeaveTypeId);
            if (approvedLeaves)
            {
                result.AddMessageItem(new MessageItem(nameof(ApprovedLeaves.LeaveTypeId), Resource.ALREDY_APPPLIED_PRECONSUMABLE_LEAVES));
            }
            await  base.CustomValidation(item, result);
        }
    }
}
