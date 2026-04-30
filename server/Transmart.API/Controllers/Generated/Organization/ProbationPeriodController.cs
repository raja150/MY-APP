using AutoMapper;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;
using TranSmart.Service.Organization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TranSmart.API.Extensions;
using System.Threading.Tasks;

namespace TranSmart.API.Controllers.Organization
{
    [Route("api/Organization/[controller]")]
    [ApiController]
    public partial class ProbationPeriodController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IProbationPeriodService _service;
        public ProbationPeriodController(IMapper mapper, IProbationPeriodService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] string OrderBy)
        {
            return Ok(_mapper.Map<IEnumerable<ProbationPeriodList>>(await _service.GetList(OrderBy)));
        }

        [HttpGet("Paginate")]
        [ApiAuthorize(Core.Permission.Org_ProbationPeriod, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<ProbationPeriodList>>(await _service.GetPaginate(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission.Org_ProbationPeriod, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<ProbationPeriodModel>(await _service.GetById(id)));
        }

        [HttpPost]
        [ApiAuthorize(Core.Permission.Org_ProbationPeriod, Core.Privilege.Create)]
        public async Task<IActionResult> Post(ProbationPeriodModel model)
        {
            Result<ProbationPeriod> result = await _service.AddAsync(_mapper.Map<ProbationPeriod>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<ProbationPeriodModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
        [ApiAuthorize(Core.Permission.Org_ProbationPeriod, Core.Privilege.Update)]
        public async Task<IActionResult> Put(ProbationPeriodModel model)
        {
            Result<ProbationPeriod> result = await _service.UpdateAsync(_mapper.Map<ProbationPeriod>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<ProbationPeriodModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

    }
}
