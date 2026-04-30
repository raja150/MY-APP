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
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave.Search;

namespace TranSmart.Service.Approval
{
    public partial interface IApprovalCompensatoryWorkingDayService : IBaseService<ApplyCompo>
    {
        Task<IPaginate<ApplyCompo>> GetAllList(StatusSearch search);
    }
    public partial class ApprovalCompensatoryWorkingDayService : BaseService<ApplyCompo>, IApprovalCompensatoryWorkingDayService
    {
        public ApprovalCompensatoryWorkingDayService(IUnitOfWork uow) : base(uow)
        {

        }
        public async Task<IPaginate<ApplyCompo>> GetAllList(StatusSearch search)
        {
            return await UOW.GetRepositoryAsync<ApplyCompo>().GetPageListAsync(
                predicate: x => (string.IsNullOrEmpty(search.Name) || x.Employee.Name.Contains(search.Name))
                && (x.Employee.ReportingToId == search.RefId)
                && (!search.Status.HasValue || x.Status == search.Status),
                include: i => i.Include(x => x.Employee).ThenInclude(x => x.Designation)
                                .Include(x => x.Employee).ThenInclude(x => x.Department)
                                .Include(x => x.ApprovedBy),
                index: search.Page, size: search.Size, sortBy: search.SortBy ?? "FromDate", ascending: !search.IsDescend);
        }

        public override async Task OnBeforeUpdate(ApplyCompo item, Result<ApplyCompo> executionResult)
        {
            LeaveBalance leaveBalance = new LeaveBalance();
            LeaveSettings settings =await UOW.GetRepositoryAsync<LeaveSettings>().SingleAsync();
            //from date and to date are same then it considering as 0
            int NoOfLeaves = (item.ToDate - item.FromDate).Days + 1;

            if (item.Status == 1)
            {
                leaveBalance.ApplyCompensatoryId = item.ID;
                leaveBalance.LeaveTypeId = settings.CompoLeaveTypeId;
                leaveBalance.EmployeeId = (Guid)item.EmployeeId;
                leaveBalance.Leaves = NoOfLeaves;
                leaveBalance.Type = (int)Data.LeaveTypesScreens.CompensatoryWorkingDay;
                await UOW.GetRepositoryAsync<LeaveBalance>().AddAsync(leaveBalance);
            }
            await base.OnBeforeUpdate(item, executionResult); 
        }  
    }
}

