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
    public partial class LoanController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ILoanService _service;
        public LoanController(IMapper mapper, ILoanService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] string OrderBy)
        {
            return Ok(_mapper.Map<IEnumerable<LoanList>>(await _service.GetList(OrderBy)));
        }

        [HttpGet("Paginate")]
        [ApiAuthorize(Core.Permission.PS_Loan, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<LoanList>>(await _service.GetPaginate(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission.PS_Loan, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<LoanModel>(await _service.GetById(id)));
        }

        [HttpPost]
        [ApiAuthorize(Core.Permission.PS_Loan, Core.Privilege.Create)]
        public async Task<IActionResult> Post(LoanModel model)
        {
            Result<Loan> result = await _service.AddAsync(_mapper.Map<Loan>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<LoanModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
        [ApiAuthorize(Core.Permission.PS_Loan, Core.Privilege.Update)]
        public async Task<IActionResult> Put(LoanModel model)
        {
            Result<Loan> result = await _service.UpdateAsync(_mapper.Map<Loan>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<LoanModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

    }
}
