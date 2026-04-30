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
    public partial class LeaveSettingsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ILeaveSettingsService _service;
        public LeaveSettingsController(IMapper mapper, ILeaveSettingsService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] string OrderBy)
        {
            return Ok(_mapper.Map<IEnumerable<LeaveSettingsList>>(await _service.GetList(OrderBy)));
        }

        [HttpGet("Paginate")]
        [ApiAuthorize(Core.Permission.LM_LeaveSettings, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<LeaveSettingsList>>(await _service.GetPaginate(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission.LM_LeaveSettings, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<LeaveSettingsModel>(await _service.GetById(id)));
        }

        [HttpPost]
        [ApiAuthorize(Core.Permission.LM_LeaveSettings, Core.Privilege.Create)]
        public async Task<IActionResult> Post(LeaveSettingsModel model)
        {
            Result<LeaveSettings> result = await _service.AddAsync(_mapper.Map<LeaveSettings>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<LeaveSettingsModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
        [ApiAuthorize(Core.Permission.LM_LeaveSettings, Core.Privilege.Update)]
        public async Task<IActionResult> Put(LeaveSettingsModel model)
        {
            Result<LeaveSettings> result = await _service.UpdateAsync(_mapper.Map<LeaveSettings>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<LeaveSettingsModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

    }
}
