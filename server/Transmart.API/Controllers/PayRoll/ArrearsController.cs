using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Extensions;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Payroll;
using TranSmart.Domain.Models.PayRoll.List;
using TranSmart.Domain.Models.PayRoll.Request;
using TranSmart.Domain.Models.PayRoll.Response;
using TranSmart.Service.Payroll;
using TranSmart.Service.Reports.LMS;

namespace TranSmart.API.Controllers.Payroll
{
    [Route("api/PayRoll/[controller]")]
    [ApiController]
    public class ArrearsController : BaseController
    {

        private readonly IMapper _mapper;
        private readonly IArrearService _service;
        private readonly IPayMonthService _pmService;
        public ArrearsController(IMapper mapper, IArrearService service, IPayMonthService pmService)
        {
            _mapper = mapper;
            _service = service;
            _pmService = pmService;

        }

        [HttpGet("Paginate")]
		[ApiAuthorize(Core.Permission.PS_Arrears, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<ArrearsList>>(await _service.GetPaginate(baseSearch)));
        }


        [HttpGet("{id}")]
		[ApiAuthorize(Core.Permission.PS_Arrears, Core.Privilege.Read)]
        public async Task<IActionResult>  Get(Guid id)
        {
            return Ok(_mapper.Map<ArrearsModel>(await _service.GetById(id)));
        }

        [HttpPost]
		[ApiAuthorize(Core.Permission.PS_Arrears, Core.Privilege.Create)]
        public async Task<IActionResult> Post(ArrearsRequest model)
        {
            Result<Arrear> executionResult = new();

            PayMonth month = await _pmService.GetPayMonth((Guid)model.PayMonthId);
            if (month == null)
            {
				executionResult.AddMessageItem(new MessageItem("Pay month is required"));
				return BadRequest(executionResult);
            }
            Arrear request = _mapper.Map<Arrear>(model);
            request.Month = month.Month;
            request.Year = month.Year;

            Result<Arrear> result = await _service.AddAsync(request);
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<ArrearsModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
		[ApiAuthorize(Core.Permission.PS_Arrears, Core.Privilege.Update)]
        public async Task<IActionResult> Put(ArrearsRequest model)
        {
            Arrear request = _mapper.Map<Arrear>(model);
            Result<Arrear> result = await _service.UpdateAsync(request);

            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<ArrearsModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string name)
        {
            return Ok(_mapper.Map<IEnumerable<ArrearsModel>>(await _service.Search(name)));
        }
		[HttpGet("PayMonths")]
		[ApiAuthorize(Core.Permission.PS_Arrears, Core.Privilege.Read)]
		public async Task<IActionResult> Months()
		{
			return Ok(_mapper.Map<List<PayMonthModel>>(await _pmService.GetPayMonths()));
		}
	}
}
