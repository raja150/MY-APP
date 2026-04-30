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
    public partial class LeaveTypeScheduleController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ILeaveTypeScheduleService _service;
        public LeaveTypeScheduleController(IMapper mapper, ILeaveTypeScheduleService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] string OrderBy)
        {
            return Ok(_mapper.Map<IEnumerable<LeaveTypeScheduleList>>(await _service.GetList(OrderBy)));
        }

        [HttpGet("Paginate")]
        [ApiAuthorize(Core.Permission.LM_LeaveType, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<LeaveTypeScheduleList>>(await _service.GetPaginate(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission.LM_LeaveType, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<LeaveTypeScheduleModel>(await _service.GetById(id)));
        }

        [HttpPost]
        [ApiAuthorize(Core.Permission.LM_LeaveType, Core.Privilege.Create)]
        public async Task<IActionResult> Post(LeaveTypeScheduleModel model)
        {
            Result<LeaveTypeSchedule> result = await _service.AddAsync(_mapper.Map<LeaveTypeSchedule>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<LeaveTypeScheduleModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
        [ApiAuthorize(Core.Permission.LM_LeaveType, Core.Privilege.Update)]
        public async Task<IActionResult> Put(LeaveTypeScheduleModel model)
        {
            Result<LeaveTypeSchedule> result = await _service.UpdateAsync(_mapper.Map<LeaveTypeSchedule>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<LeaveTypeScheduleModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

    }
}
