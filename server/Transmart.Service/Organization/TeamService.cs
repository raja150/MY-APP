using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;

namespace TranSmart.Service.Organization
{
    public partial interface ITeamService : IBaseService<Team>
    {
        Task<IPaginate<Team>> GetAllWeekOffTeam(BaseSearch search);
        Task<Result<Team>> UpdateWeekOff(TeamAllocationModel item);

        Task<Result<Team>> DeleteWeekOffSetup(Team item);
    }
    public partial class TeamService : BaseService<Team>, ITeamService
    {
        public async Task<IPaginate<Team>> GetAllWeekOffTeam(BaseSearch search)
        {
            return await UOW.GetRepositoryAsync<Team>().GetPageListAsync(
                predicate: x => x.WeekOffSetupId == search.RefId,
                index: search.Page, size: search.Size, sortBy: search.SortBy ?? "Type", ascending: !search.IsDescend);
        }

        public async Task<Result<Team>> UpdateWeekOff(TeamAllocationModel item)
        {
            Result<Team> result = new Result<Team>();
            var location = await UOW.GetRepositoryAsync<Team>().SingleAsync(x => x.ID == item.TeamId);

            if (location.WeekOffSetupId != null && location.ID == item.TeamId)
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

        public async Task<Result<Team>> DeleteWeekOffSetup(Team item)
        {
            Result<Team> result = new Result<Team>();
            var team = await UOW.GetRepositoryAsync<Team>().SingleAsync(x => x.ID == item.ID);
            if (team.WeekOffSetupId == null)
            {
                result.AddMessageItem(new MessageItem(Resource.Invalid_Item));
                return result;
            }
            team.WeekOffSetupId = null;
            _ = await UpdateOnlyAsync(team);
            result.ReturnValue = team;
            result.IsSuccess = true;
            return result;
        }
    }
}
