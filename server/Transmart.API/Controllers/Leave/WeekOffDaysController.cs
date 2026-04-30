using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave;

namespace TranSmart.API.Controllers.Leave
{
    public partial class WeekOffDaysController : BaseController
    {
        [HttpGet("WeekOffDays")]
        public async Task<IActionResult> GetAllWeekOffDays([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<TranSmart.Domain.Models.Leave.List.WeekOffDaysList>>(await _service.GetAllWeekOffDays(baseSearch)));
        }

        [HttpPut("DeleteWeekOffDays/{id}")]
        public async Task<IActionResult> DeleteWeekOffDays(Guid id)
        {
            Result<WeekOffDays> result = await _service.DeleteWeekOffDays(id);
            if (!result.HasError)
            {
                return Ok(_mapper.Map<WeekOffDaysModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }


    }
}