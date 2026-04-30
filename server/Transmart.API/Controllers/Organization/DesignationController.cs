using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;

namespace TranSmart.API.Controllers.Organization
{
    public partial class DesignationController : BaseController
    {
        [HttpGet("WeekOffSetups")]
        public async Task<IActionResult> GetAllWeekOffDesignations([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<DesignationList>>(await _service.GetAllWeekOffDesign(baseSearch)));
        }

        [HttpPut("DesignWeekOffSetup")]
        public async Task<IActionResult> DesigWeekOffSetup(DesignationAllocationModel model)
        {
            Result<Designation> result = await _service.UpdateWeekoff(model);
            if (!result.HasError)
            {
                return Ok(_mapper.Map<DesignationModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut("DeleteWeekOffSetup")]
        public async Task<IActionResult> DeleteWeekOffSetup(DesignationModel model)
        {
            Result<Designation> result = await _service.DeleteWeekOffSetup(_mapper.Map<Designation>(model));
            if (!result.HasError)
            {
                return Ok(_mapper.Map<DesignationModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }
    }
}
