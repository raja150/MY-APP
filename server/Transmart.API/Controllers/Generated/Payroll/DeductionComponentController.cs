using AutoMapper;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Payroll;
using TranSmart.Service.Payroll;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TranSmart.API.Extensions;
using System.Threading.Tasks;

namespace TranSmart.API.Controllers.Payroll
{
    [Route("api/Payroll/[controller]")]
    [ApiController]
    public partial class DeductionComponentController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IDeductionComponentService _service;
        public DeductionComponentController(IMapper mapper, IDeductionComponentService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] string OrderBy)
        {
            return Ok(_mapper.Map<IEnumerable<DeductionComponentList>>(await _service.GetList(OrderBy)));
        }

        [HttpGet("Paginate")]
        [ApiAuthorize(Core.Permission.PS_DeductionComponent, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<DeductionComponentList>>(await _service.GetPaginate(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission.PS_DeductionComponent, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<DeductionComponentModel>(await _service.GetById(id)));
        }

        [HttpPost]
        [ApiAuthorize(Core.Permission.PS_DeductionComponent, Core.Privilege.Create)]
        public async Task<IActionResult> Post(DeductionComponentModel model)
        {
            Result<DeductionComponent> result = await _service.AddAsync(_mapper.Map<DeductionComponent>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<DeductionComponentModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
        [ApiAuthorize(Core.Permission.PS_DeductionComponent, Core.Privilege.Update)]
        public async Task<IActionResult> Put(DeductionComponentModel model)
        {
            Result<DeductionComponent> result = await _service.UpdateAsync(_mapper.Map<DeductionComponent>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<DeductionComponentModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

    }
}
