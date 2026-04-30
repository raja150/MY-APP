using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using TranSmart.API.Extensions;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Models.PayRoll.List;
using TranSmart.Domain.Models.PayRoll.Response;
using TranSmart.Domain.Models;
using TranSmart.Service.Payroll;

namespace TranSmart.API.Controllers.PayRoll
{
	[Route("api/Payroll/[controller]")]
	[ApiController]
	public partial class EmployeePayInfoController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IEmployeePayInfoService _service;
		public EmployeePayInfoController(IMapper mapper, IEmployeePayInfoService service)
		{
			_mapper = mapper;
			_service = service;
		}

		[HttpGet("GetList")]
		public async Task<IActionResult> GetList([FromQuery] string OrderBy)
		{
			return Ok(_mapper.Map<IEnumerable<EmployeePayInfoList>>(await _service.GetList(OrderBy)));
		}

		[HttpGet("Paginate")]
		[ApiAuthorize(Core.Permission.PS_EmployeePayInfo, Core.Privilege.Read)]
		public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
		{
			return Ok(_mapper.Map<Models.Paginate<EmployeePayInfoList>>(await _service.GetPaginate(baseSearch)));
		}

		[HttpGet("{id}")]
		[ApiAuthorize(Core.Permission.PS_EmployeePayInfo, Core.Privilege.Read)]
		public async Task<IActionResult> Get(Guid id)
		{
			return Ok(_mapper.Map<EmployeePayInfoModel>(await _service.GetById(id)));
		}

		[HttpPost]
		[ApiAuthorize(Core.Permission.PS_EmployeePayInfo, Core.Privilege.Create)]
		public async Task<IActionResult> Post(EmployeePayInfoModel model)
		{
			Result<EmployeePayInfo> result = await _service.AddAsync(_mapper.Map<EmployeePayInfo>(model));
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<EmployeePayInfoModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}

		[HttpPut]
		[ApiAuthorize(Core.Permission.PS_EmployeePayInfo, Core.Privilege.Update)]
		public async Task<IActionResult> Put(EmployeePayInfoModel model)
		{
			Result<EmployeePayInfo> result = await _service.UpdateAsync(_mapper.Map<EmployeePayInfo>(model));
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<EmployeePayInfoModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}

	}
}
