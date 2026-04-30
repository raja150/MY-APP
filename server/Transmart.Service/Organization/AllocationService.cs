using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;

namespace TranSmart.Service.Organization
{
    public partial interface IAllocationService : IBaseService<Allocation>
    {
        Task<IPaginate<Allocation>> GetAllWeekOffEmployees(BaseSearch search);
        Task<Result<Allocation>> UpdateAllocation(Allocation item);
        Task<Result<Allocation>> DeleteWeekOffSetup(Allocation item);
        Task<Allocation> GetEmployee(Guid id);
    }
    public partial class AllocationService : BaseService<Allocation>
    {
         public  async Task<Allocation> GetEmployee(Guid id)
        {
            return await UOW.GetRepositoryAsync<Allocation>().SingleAsync(
                predicate: x => x.ID == id,
                include: i => i.Include(x => x.Shift).Include(x => x.WeekOffSetup).Include(x=>x.Employee),
                orderBy: o => o.OrderByDescending(x => x.AddedAt));
        }

        public async Task<IPaginate<Allocation>> GetAllWeekOffEmployees(BaseSearch search)
        {
            return await UOW.GetRepositoryAsync<Allocation>().GetPageListAsync(
                predicate: x => x.WeekOffSetupId == search.RefId.Value,
                include: i => i.Include(x => x.Employee).ThenInclude(x=>x.Department)
                .Include(x => x.Employee).ThenInclude(x => x.Designation)
                .Include(x => x.Shift),
                index: search.Page, size: search.Size, sortBy: search.SortBy ?? "Type", ascending: !search.IsDescend);
        }


        public async Task<Result<Allocation>> UpdateAllocation(Allocation item)
        {
            Result<Allocation> result = new Result<Allocation>();
            var allocation = await UOW.GetRepositoryAsync<Allocation>().SingleAsync(x => x.EmployeeId == item.EmployeeId);
            if (allocation == null)
            {
                result.AddMessageItem(new MessageItem(Resource.Please_Give_Allocation_Emp_Screen));
                return result;
            }
            if (allocation.WeekOffSetupId != null && allocation.EmployeeId == item.EmployeeId)
            {
                result.AddMessageItem(new MessageItem(Resource.Week_Already_Exist));
                return result;
            }
            allocation.WeekOffSetupId = item.WeekOffSetupId;
            _ = await UpdateOnlyAsync(allocation);
            result.ReturnValue = allocation;
            result.IsSuccess = true;
            return result;
        }

        public async Task<Result<Allocation>> DeleteWeekOffSetup(Allocation item)
        {
            Result<Allocation> result = new Result<Allocation>();
            var allocation = await UOW.GetRepositoryAsync<Allocation>().SingleAsync(x => x.ID == item.ID);
            if (allocation == null)
            {
                result.AddMessageItem(new MessageItem(Resource.Invalid_Item));
                return result;
            }
            allocation.WeekOffSetupId = null;
            _ = await UpdateOnlyAsync(allocation);
            result.ReturnValue = allocation;
            result.IsSuccess = true;
            return result;
        }
    }

}
