using AutoMapper;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Helpdesk;
using TranSmart.Service.Helpdesk;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TranSmart.API.Extensions;
using System.Threading.Tasks;

namespace TranSmart.API.Controllers.Helpdesk
{
    [Route("api/Helpdesk/[controller]")]
    [ApiController]
    public partial class TicketStatusController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ITicketStatusService _service;
        public TicketStatusController(IMapper mapper, ITicketStatusService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] string OrderBy)
        {
            return Ok(_mapper.Map<IEnumerable<TicketStatusList>>(await _service.GetList(OrderBy)));
        }

        [HttpGet("Paginate")]
        [ApiAuthorize(Core.Permission.HD_TicketStatus, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<TicketStatusList>>(await _service.GetPaginate(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission.HD_TicketStatus, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<TicketStatusModel>(await _service.GetById(id)));
        }

        [HttpPost]
        [ApiAuthorize(Core.Permission.HD_TicketStatus, Core.Privilege.Create)]
        public async Task<IActionResult> Post(TicketStatusModel model)
        {
            Result<TicketStatus> result = await _service.AddAsync(_mapper.Map<TicketStatus>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<TicketStatusModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
        [ApiAuthorize(Core.Permission.HD_TicketStatus, Core.Privilege.Update)]
        public async Task<IActionResult> Put(TicketStatusModel model)
        {
            Result<TicketStatus> result = await _service.UpdateAsync(_mapper.Map<TicketStatus>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<TicketStatusModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

    }
}
