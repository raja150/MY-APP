using AutoMapper;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave;
using TranSmart.Service.Leave;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TranSmart.API.Extensions;
using System.Threading.Tasks;

namespace TranSmart.API.Controllers.Leave
{
    [Route("api/Leave/[controller]")]
    [ApiController]
    public partial class WeekOffDaysController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IWeekOffDaysService _service;
        public WeekOffDaysController(IMapper mapper, IWeekOffDaysService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] string OrderBy)
        {
            return Ok(_mapper.Map<IEnumerable<WeekOffDaysList>>(await _service.GetList(OrderBy)));
        }

        [HttpGet("Paginate")]
        [ApiAuthorize(Core.Permission.LM_WeekOffSetup, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<WeekOffDaysList>>(await _service.GetPaginate(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission.LM_WeekOffSetup, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<WeekOffDaysModel>(await _service.GetById(id)));
        }

        [HttpPost]
        [ApiAuthorize(Core.Permission.LM_WeekOffSetup, Core.Privilege.Create)]
        public async Task<IActionResult> Post(WeekOffDaysModel model)
        {
            Result<WeekOffDays> result = await _service.AddAsync(_mapper.Map<WeekOffDays>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<WeekOffDaysModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
        [ApiAuthorize(Core.Permission.LM_WeekOffSetup, Core.Privilege.Update)]
        public async Task<IActionResult> Put(WeekOffDaysModel model)
        {
            Result<WeekOffDays> result = await _service.UpdateAsync(_mapper.Map<WeekOffDays>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<WeekOffDaysModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

    }
}
