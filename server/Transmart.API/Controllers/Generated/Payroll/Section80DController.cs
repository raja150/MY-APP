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
    public partial class Section80DController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ISection80DService _service;
        public Section80DController(IMapper mapper, ISection80DService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] string OrderBy)
        {
            return Ok(_mapper.Map<IEnumerable<Section80DList>>(await _service.GetList(OrderBy)));
        }

        [HttpGet("Paginate")]
        [ApiAuthorize(Core.Permission.PS_Section80D, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<Section80DList>>(await _service.GetPaginate(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission.PS_Section80D, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<Section80DModel>(await _service.GetById(id)));
        }

        [HttpPost]
        [ApiAuthorize(Core.Permission.PS_Section80D, Core.Privilege.Create)]
        public async Task<IActionResult> Post(Section80DModel model)
        {
            Result<Section80D> result = await _service.AddAsync(_mapper.Map<Section80D>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<Section80DModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
        [ApiAuthorize(Core.Permission.PS_Section80D, Core.Privilege.Update)]
        public async Task<IActionResult> Put(Section80DModel model)
        {
            Result<Section80D> result = await _service.UpdateAsync(_mapper.Map<Section80D>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<Section80DModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

    }
}
