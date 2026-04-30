using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;

namespace TranSmart.API.Controllers.Organization
{
    public partial class AllocationController : BaseController
    {

        [HttpGet("Employee/{id}")]
        public async Task<IActionResult> GetEmp(Guid id)
        {
            return Ok(_mapper.Map<AllocationModel>(await _service.GetEmployee(id)));
        }



        [HttpGet("WeekOffSetups")]
        public async Task<IActionResult> GetAllWeekOffEmployees([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<AllocationList>>(await _service.GetAllWeekOffEmployees(baseSearch)));
        }


        [HttpPut("EmpWeekOffSetup")]
        public async Task<IActionResult> EmpWeekOffSetup(AllocationModel model)
        {
            Result<Allocation> result = await _service.UpdateAllocation(_mapper.Map<Allocation>(model));
            if (!result.HasError)
            {
                return Ok(_mapper.Map<AllocationModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }
        [HttpPut("DeleteWeekOffSetup")]
        public async Task<IActionResult> DeleteWeekOffSetup(AllocationModel model)
        {
            Result<Allocation> result = await _service.DeleteWeekOffSetup(_mapper.Map<Allocation>(model));
            if (!result.HasError)
            {
                return Ok(_mapper.Map<AllocationModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }
    }
}
