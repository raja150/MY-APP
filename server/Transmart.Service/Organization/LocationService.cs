using System;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;

namespace TranSmart.Service.Organization
{
    public partial interface ILocationService : IBaseService<Location>
    {
        Task<IPaginate<Location>> GetAllWeekOffLoc(BaseSearch search);
        Task<Result<Location>> UpdateWeekoff(LocationAllocationModel item);
        Task<Result<Location>> DeleteWeekOffSetup(Location item);

    }
    public partial class LocationService : BaseService<Location>, ILocationService
    {
        public async Task<IPaginate<Location>> GetAllWeekOffLoc(BaseSearch search)
        {
            return await UOW.GetRepositoryAsync<Location>().GetPageListAsync(
                predicate: x => x.WeekOffSetupId == search.RefId,
                index: search.Page, size: search.Size, sortBy: search.SortBy ?? "Type", ascending: !search.IsDescend);
        }

        public async Task<Result<Location>> UpdateWeekoff(LocationAllocationModel item)
        {
            Result<Location> result = new Result<Location>();
            var location = await UOW.GetRepositoryAsync<Location>().SingleAsync(x => x.ID == item.LocationId);

            if (location.WeekOffSetupId != null && location.ID == item.LocationId)
            {
                result.AddMessageItem(new MessageItem(Resource.Week_Already_Exist));
                return result;
            }
            location.WeekOffSetupId = item.WeekOffSetupId;
            _ = await UpdateOnlyAsync(location);
            result.ReturnValue = location;
            result.IsSuccess = true;
            return result;
        }     

        public async Task<Result<Location>> DeleteWeekOffSetup(Location item)
        {
            Result<Location> result = new Result<Location>();
            var location = await UOW.GetRepositoryAsync<Location>().SingleAsync(x => x.ID == item.ID);
            if (location == null)
            {
                result.AddMessageItem(new MessageItem(Resource.Invalid_Item));
                return result;
            }
            location.WeekOffSetupId = null;
            _ = await UpdateOnlyAsync(location);
            result.ReturnValue = location;
            result.IsSuccess = true;
            return result;
        }
    }
}
