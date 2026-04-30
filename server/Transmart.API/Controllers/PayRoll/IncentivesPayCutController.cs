
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
using TranSmart.Domain.Models.PayRoll.Request;
using TranSmart.Domain.Models.PayRoll.Response;
using TranSmart.Domain.Models.PayRoll.List;
using TranSmart.Service.Payroll;
using TranSmart.Domain.Models.Payroll;

namespace TranSmart.API.Controllers.PayRoll
{
	[Route("api/PayRoll/[controller]")]
	[ApiController]
	public class IncentivesPayCutController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IIncentivesPayCutService _service;
		private readonly IPayMonthService _pmService;
		public IncentivesPayCutController(IMapper mapper, IIncentivesPayCutService service, IPayMonthService pmService)
		{
			_mapper = mapper;
			_service = service;
			_pmService = pmService;
		}



		[HttpGet("Paginate")]
		[ApiAuthorize(Core.Permission.PS_IncentivePayCut, Core.Privilege.Read)]

		public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
		{
			return Ok(_mapper.Map<Models.Paginate<IncentivesPayCutList>>(await _service.GetPaginate(baseSearch)));
		}

		[HttpGet("{id}")]
		[ApiAuthorize(Core.Permission.PS_IncentivePayCut, Core.Privilege.Read)]

		public async Task<IActionResult> Get(Guid id)
		{
			return Ok(_mapper.Map<IncentivesPayCutModel>(await _service.GetById(id)));
		}

		[HttpPost]
		[ApiAuthorize(Core.Permission.PS_IncentivePayCut, Core.Privilege.Create)]
		public async Task<IActionResult> Post(IncentivesPayCutRequest model)
		{
			Result<IncentivesPayCut> result = new();
			PayMonth month = await _pmService.GetPayMonth((Guid)model.PayMonthId);
			if (month == null)
			{
				result.AddMessageItem(new MessageItem("Pay month is required"));
				return BadRequest(result);
			}
			IncentivesPayCut request = _mapper.Map<IncentivesPayCut>(model);
			request.Month = month.Month;
			request.Year = month.Year;

			result = await _service.AddAsync(request);
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<IncentivesPayCutModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}

		[HttpPut]
		[ApiAuthorize(Core.Permission.PS_IncentivePayCut, Core.Privilege.Update)]
		public async Task<IActionResult> Put(IncentivesPayCutRequest model)
		{
			IncentivesPayCut request = _mapper.Map<IncentivesPayCut>(model);
			Result<IncentivesPayCut> result = await _service.UpdateAsync(request);

			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<IncentivesPayCutModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}


		[HttpGet("Search")]
		public async Task<IActionResult> Search(string name)
		{
			return Ok(_mapper.Map<IEnumerable<IncentivesPayCutModel>>(await _service.Search(name)));
		}

		[HttpGet("Employee")]
		[ApiAuthorize(Core.Permission.SS_SalaryDetails, Core.Privilege.Read)]
		public async Task<IActionResult> GetByEmployee([FromQuery] int month, [FromQuery] int year)
		{
			return Ok(_mapper.Map<IncentivesPayCutModel>(await _service.GetByEmployee(LOGIN_USER_EMPId, month, year)));
		}

		[HttpGet("Employee/{empId}")]
		[ApiAuthorize(Core.Permission.PS_Payrun, Core.Privilege.Read)]
		public async Task<IActionResult> GetByEmployeePayRoll([FromRoute] Guid empId, [FromQuery] int month, [FromQuery] int year)
		{
			return Ok(_mapper.Map<IncentivesPayCutModel>(await _service.GetByEmployee(empId, month, year)));
		}
		[HttpGet("PayMonths")]
		[ApiAuthorize(Core.Permission.PS_IncentivePayCut, Core.Privilege.Read)]
		public async Task<IActionResult> Months()
		{
			return Ok(_mapper.Map<List<PayMonthModel>>(await _pmService.GetPayMonths()));
		}
	}
}
