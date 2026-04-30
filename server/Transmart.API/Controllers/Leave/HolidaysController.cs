using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Domain.Models.Leave;
using TranSmart.Domain.Models.Leave.Search;

namespace TranSmart.API.Controllers.Leave
{
    public partial class HolidaysController
    {
        [HttpGet("Future")]
        public async Task<IActionResult> HolidaysPaginate([FromQuery] HolidaysSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<HolidaysList>>(await _service.Future(baseSearch)));
        }
        [HttpGet("Past")]
        public async Task<IActionResult> PastHolidaysPaginate([FromQuery] HolidaysSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<HolidaysList>>(await _service.Past(baseSearch)));
        }
    }
}
