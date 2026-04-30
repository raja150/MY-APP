using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;

namespace TranSmart.API.Controllers.Organization
{
    public partial class LocationController : BaseController
    {
        [HttpGet("WeekOffSetups")]
        public async Task<IActionResult> GetAllWeekOffLocations([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<LocationList>>(await _service.GetAllWeekOffLoc(baseSearch)));
        }
        [HttpPut("LocationWeekOffSetup")]
        public async Task<IActionResult> LocWeekOffSetup(LocationAllocationModel model)
        {
            Result<Location> result = await _service.UpdateWeekoff(model);
            if (!result.HasError)
            {
                return Ok(_mapper.Map<LocationModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut("DeleteWeekOffSetup")]
        public async Task<IActionResult> DeleteWeekOffSetup(LocationModel model)
        {
            Result<Location> result = await _service.DeleteWeekOffSetup(_mapper.Map<Location>(model));
            if (!result.HasError)
            {
                return Ok(_mapper.Map<LocationModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }
    }
}
