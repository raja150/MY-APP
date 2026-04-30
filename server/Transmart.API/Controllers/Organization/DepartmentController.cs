using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;

namespace TranSmart.API.Controllers.Organization
{
    public partial class DepartmentController : BaseController
    {
        [HttpGet("WeekOffSetups")]
        public async Task<IActionResult> GetAllWeekOffDept([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<DepartmentList>>(await _service.GetAllWeekOffDepts(baseSearch)));
        }

        [HttpPut("DeptWeekOffSetup")]
        public async Task<IActionResult> DeptWeekOffSetup(DepartmentAllocationModel model)
        {
            Result<Department> result = await _service.UpdateWeekoff(model);
            if (!result.HasError)
            {
                return Ok(_mapper.Map<DepartmentModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut("DeleteWeekOffSetup")]
        public async Task<IActionResult> DeleteWeekOffSetup(DepartmentAllocationModel model)
        {
            Result<Department> result = await _service.DeleteWeekOffSetup(model);
            if (!result.HasError)
            {
                return Ok(_mapper.Map<DepartmentModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }
    }
}
