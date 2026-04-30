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
    public partial class HolidaysController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IHolidaysService _service;
        public HolidaysController(IMapper mapper, IHolidaysService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] string OrderBy)
        {
            return Ok(_mapper.Map<IEnumerable<HolidaysList>>(await _service.GetList(OrderBy)));
        }

        [HttpGet("Paginate")]
        [ApiAuthorize(Core.Permission.LM_Holidays, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<HolidaysList>>(await _service.GetPaginate(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission.LM_Holidays, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<HolidaysModel>(await _service.GetById(id)));
        }

        [HttpPost]
        [ApiAuthorize(Core.Permission.LM_Holidays, Core.Privilege.Create)]
        public async Task<IActionResult> Post(HolidaysModel model)
        {
            Result<Holidays> result = await _service.AddAsync(_mapper.Map<Holidays>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<HolidaysModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
        [ApiAuthorize(Core.Permission.LM_Holidays, Core.Privilege.Update)]
        public async Task<IActionResult> Put(HolidaysModel model)
        {
            Result<Holidays> result = await _service.UpdateAsync(_mapper.Map<Holidays>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<HolidaysModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

    }
}
