using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TranSmart.API.Extensions;
using TranSmart.API.Services;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models.Payroll;
using TranSmart.Service;
using TranSmart.Service.Payroll;

namespace TranSmart.API.Controllers.Payroll
{
	[Route("api/PayRoll/[controller]")]
	[ApiController]
	public class PayRunController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IPayRollService _service;
		private readonly IPayMonthService _monService;
		private readonly IApplicationAuditLogService _auditLogService;
		private readonly ICacheService _cacheService;
		public PayRunController(IMapper mapper, IPayRollService service, IPayMonthService monService,
			IApplicationAuditLogService auditLogService, ICacheService cacheService)
		{
			_mapper = mapper;
			_service = service;
			_monService = monService;
			_auditLogService = auditLogService;
			_cacheService = cacheService;
		}

		[HttpPut("{Id}")]
		[ApiAuthorize(Core.Permission.PS_Payrun, Core.Privilege.Read)]
		public async Task<IActionResult> Run(Guid Id)
		{
			Result<PaySheet> payRunResult = new();
			await _auditLogService.GetAccesedUser(UserId, "Executed PayRoll", IpAddress, (Core.Permission.PS_Payrun).ToString());

			//Check payroll running status
			var isRunning = _cacheService.PayRunUser();
			if (isRunning)
			{
				//Return error when payroll already executing
				payRunResult.AddMessageItem(new MessageItem("Please Wait... Another user payRun executing"));
				return BadRequest(payRunResult);
			}
			Result<PaySheet> result = await _service.Process(Id);
			//remove payRun flag from cache
			_cacheService.RemovePayRunUser();

			if (result.HasError)
			{
				return BadRequest(result);
			}
			return Ok(_mapper.Map<PayMonthModel>(await _monService.GetById(Id)));
		}

		[HttpPut("Release/{Id}")]
		[ApiAuthorize(Core.Permission.PS_Payrun, Core.Privilege.Read)]
		public async Task<IActionResult> Release(Guid Id)
		{
			await _auditLogService.GetAccesedUser(UserId, "Release PayRoll", IpAddress, (Core.Permission.PS_Payrun).ToString());
			Result<PaySheet> result = await _service.Release(Id);
			if (result.HasError)
			{
				return BadRequest(result);
			}
			return Ok(_mapper.Map<PayMonthModel>(await _monService.GetById(Id)));
		}

		[HttpPut("Hold/{Id}")]
		[ApiAuthorize(Core.Permission.PS_Payrun, Core.Privilege.Read)]
		public async Task<IActionResult> Hold(Guid Id)
		{
			await _auditLogService.GetAccesedUser(UserId, "Hold PayRoll", IpAddress, (Core.Permission.PS_Payrun).ToString());
			Result<PaySheet> result = await _service.Hold(Id);
			if (result.HasError)
			{
				return BadRequest(result);
			}
			return Ok(_mapper.Map<PayMonthModel>(await _monService.GetById(Id)));
		}

		[HttpDelete("{Id}")]
		[ApiAuthorize(Core.Permission.PS_Payrun, Core.Privilege.Delete)]
		public async Task<IActionResult> Delete(Guid Id)
		{
			await _auditLogService.GetAccesedUser(UserId, "Delete PayRoll", IpAddress, (Core.Permission.PS_Payrun).ToString());
			Result<PaySheet> result = await _service.Delete(Id);
			if (result.HasError)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}
	}
}
