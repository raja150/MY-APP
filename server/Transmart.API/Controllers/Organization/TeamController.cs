using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;

namespace TranSmart.API.Controllers.Organization
{
    public partial class TeamController : BaseController
    {
        [HttpGet("WeekOffSetups")]
        public async Task<IActionResult> GetAllWeekOffTeams([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<TeamList>>(await _service.GetAllWeekOffTeam(baseSearch)));
        }

        [HttpPut("TeamWeekOffSetup")]
        public async Task<IActionResult> TeamWeekOffSetup(TeamAllocationModel model)
        {
            Result<Team> result = await (_service.UpdateWeekOff(model));
            if (!result.HasError)
            {
                return Ok(_mapper.Map<TeamModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }


        [HttpPut("DeleteWeekOffSetup")]
        public async Task<IActionResult> DeleteWeekOffSetup(TeamModel model)
        {
            Result<Team> result = await _service.DeleteWeekOffSetup(_mapper.Map<Team>(model));
            if (!result.HasError)
            {
                return Ok(_mapper.Map<TeamModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }
    }
}
