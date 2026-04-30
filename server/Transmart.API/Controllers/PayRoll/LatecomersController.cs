using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TranSmart.API.Extensions;
using TranSmart.API.Services.Import;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Payroll;
using TranSmart.Domain.Models.PayRoll.List;
using TranSmart.Domain.Models.PayRoll.Request;
using TranSmart.Domain.Models.PayRoll.Response;
using TranSmart.Service.Payroll;
using TranSmart.Service.PayRoll;

namespace TranSmart.API.Controllers.PayRoll
{
	[Route("api/PayRoll/[controller]")]
	[ApiController]
	public class LateComersController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly Service.PayRoll.ILatecomersService _service;
		private readonly IPayMonthService _pmService;
		public LateComersController(IMapper mapper, Service.PayRoll.ILatecomersService service, IPayMonthService pmService)
		{
			_mapper = mapper;
			_service = service;
			_pmService = pmService;
		}
		[HttpGet("Paginate")]
		[ApiAuthorize(Core.Permission.PS_Latecomer, Core.Privilege.Read)]
		public async Task<IActionResult> Paginate([FromQuery] BaseSearch search)
		{
			return Ok(_mapper.Map<Models.Paginate<LatecomersList>>(await _service.GetPaginate(search)));
		}
		[HttpGet("{id}")]
		[ApiAuthorize(Core.Permission.PS_Latecomer, Core.Privilege.Read)]
		public async Task<IActionResult> Get(Guid id)
		{
			return Ok(_mapper.Map<LatecomersModel>(await _service.GetById(id)));
		}
		[HttpPost]
		[ApiAuthorize(Core.Permission.PS_Latecomer, Core.Privilege.Create)]
		public async Task<IActionResult> Post(LatecomersRequest model)
		{
			Result<Latecomers> result = new();
			PayMonth month = await _pmService.GetPayMonth((Guid)model.PayMonthId);
			if (month == null)
			{
				result.AddMessageItem(new MessageItem("Pay month is required"));          
				return BadRequest(result);
			}
			Latecomers request = _mapper.Map<Latecomers>(model);
			request.Month = month.Month;
			request.Year = month.Year;
			Result<Latecomers> executionResult = await _service.AddAsync(request);
			if (executionResult.IsSuccess)
			{
				return Ok(_mapper.Map<LatecomersModel>(executionResult.ReturnValue));
			}
			return BadRequest(executionResult);
		}
		[HttpPut]
		[ApiAuthorize(Core.Permission.PS_Latecomer, Core.Privilege.Update)]
		public async Task<IActionResult>Put(LatecomersRequest model)
		{
			Latecomers request = _mapper.Map<Latecomers>(model);
			Result<Latecomers> result = await _service.UpdateAsync(request);
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<LatecomersModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}
		[HttpGet("Search")]
		public async Task<IActionResult>Search(string name)
		{
			return Ok(_mapper.Map<IEnumerable<LatecomersModel>>(await _service.Search(name)));
		}
		[HttpGet("PayMonths")]
		[ApiAuthorize(Core.Permission.PS_Latecomer, Core.Privilege.Read)]
		public async Task<IActionResult> Months()
		{
			return Ok(_mapper.Map<List<PayMonthModel>>(await _pmService.GetPayMonths()));
		}
	}
}
