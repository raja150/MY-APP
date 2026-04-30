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
    public partial class ProfessionalTaxController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IProfessionalTaxService _service;
        public ProfessionalTaxController(IMapper mapper, IProfessionalTaxService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] string OrderBy)
        {
            return Ok(_mapper.Map<IEnumerable<ProfessionalTaxList>>(await _service.GetList(OrderBy)));
        }

        [HttpGet("Paginate")]
        [ApiAuthorize(Core.Permission.PS_ProfessionalTax, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<ProfessionalTaxList>>(await _service.GetPaginate(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission.PS_ProfessionalTax, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<ProfessionalTaxModel>(await _service.GetById(id)));
        }

        [HttpPost]
        [ApiAuthorize(Core.Permission.PS_ProfessionalTax, Core.Privilege.Create)]
        public async Task<IActionResult> Post(ProfessionalTaxModel model)
        {
            Result<ProfessionalTax> result = await _service.AddAsync(_mapper.Map<ProfessionalTax>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<ProfessionalTaxModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
        [ApiAuthorize(Core.Permission.PS_ProfessionalTax, Core.Privilege.Update)]
        public async Task<IActionResult> Put(ProfessionalTaxModel model)
        {
            Result<ProfessionalTax> result = await _service.UpdateAsync(_mapper.Map<ProfessionalTax>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<ProfessionalTaxModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

    }
}
