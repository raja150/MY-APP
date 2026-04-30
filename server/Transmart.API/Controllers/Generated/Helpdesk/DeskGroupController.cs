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
    public partial class DeskGroupController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IDeskGroupService _service;
        public DeskGroupController(IMapper mapper, IDeskGroupService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] string OrderBy)
        {
            return Ok(_mapper.Map<IEnumerable<DeskGroupList>>(await _service.GetList(OrderBy)));
        }

        [HttpGet("Paginate")]
        [ApiAuthorize(Core.Permission.HD_DeskGroup, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<DeskGroupList>>(await _service.GetPaginate(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission.HD_DeskGroup, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<DeskGroupModel>(await _service.GetById(id)));
        }

        [HttpPost]
        [ApiAuthorize(Core.Permission.HD_DeskGroup, Core.Privilege.Create)]
        public async Task<IActionResult> Post(DeskGroupModel model)
        {
            Result<DeskGroup> result = await _service.AddAsync(_mapper.Map<DeskGroup>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<DeskGroupModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
        [ApiAuthorize(Core.Permission.HD_DeskGroup, Core.Privilege.Update)]
        public async Task<IActionResult> Put(DeskGroupModel model)
        {
            Result<DeskGroup> result = await _service.UpdateAsync(_mapper.Map<DeskGroup>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<DeskGroupModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

    }
}
