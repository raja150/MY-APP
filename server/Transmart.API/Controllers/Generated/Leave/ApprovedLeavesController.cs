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
    public partial class ApprovedLeavesController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IApprovedLeavesService _service;
        public ApprovedLeavesController(IMapper mapper, IApprovedLeavesService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] string OrderBy)
        {
            return Ok(_mapper.Map<IEnumerable<ApprovedLeavesList>>(await _service.GetList(OrderBy)));
        }

        [HttpGet("Paginate")]
        [ApiAuthorize(Core.Permission.LM_ApprovedLeaves, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<ApprovedLeavesList>>(await _service.GetPaginate(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission.LM_ApprovedLeaves, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<ApprovedLeavesModel>(await _service.GetById(id)));
        }

        [HttpPost]
        [ApiAuthorize(Core.Permission.LM_ApprovedLeaves, Core.Privilege.Create)]
        public async Task<IActionResult> Post(ApprovedLeavesModel model)
        {
            Result<ApprovedLeaves> result = await _service.AddAsync(_mapper.Map<ApprovedLeaves>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<ApprovedLeavesModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
        [ApiAuthorize(Core.Permission.LM_ApprovedLeaves, Core.Privilege.Update)]
        public async Task<IActionResult> Put(ApprovedLeavesModel model)
        {
            Result<ApprovedLeaves> result = await _service.UpdateAsync(_mapper.Map<ApprovedLeaves>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<ApprovedLeavesModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

    }
}
