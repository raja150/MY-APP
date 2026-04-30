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
using TranSmart.API.ManageServices;
using TranSmart.Service.Payroll;

namespace TranSmart.API.Controllers.Organization
{
	[Route("api/Organization/[controller]")]
	[ApiController]
	public partial class EmployeeController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IEmployeeService _service;
		public EmployeeController(IMapper mapper, IEmployeeService service)
		{
			_mapper = mapper;
			_service = service;
		}



		[HttpGet("GetList")]
		public async Task<IActionResult> GetList([FromQuery] string OrderBy)
		{
			return Ok(_mapper.Map<IEnumerable<EmployeeList>>(await _service.GetList(OrderBy)));
		}

		[HttpGet("Paginate")]
		[ApiAuthorize(Core.Permission.Org_Employee, Core.Privilege.Read)]
		public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
		{
			return Ok(_mapper.Map<Models.Paginate<EmployeeList>>(await _service.GetPaginate(baseSearch)));
		}

		[HttpGet("{id}")]
		[ApiAuthorize(Core.Permission.Org_Employee, Core.Privilege.Read)]
		public async Task<IActionResult> Get(Guid id)
		{
			return Ok(_mapper.Map<EmployeeModel>(await _service.GetById(id)));
		}

		[HttpPost]
		[ApiAuthorize(Core.Permission.Org_Employee, Core.Privilege.Create)]
		public async Task<IActionResult> Post(EmployeeModel model, [FromServices] IServiceFactory serviceFactory)
		{
			Result<Employee> result = await _service.AddAsync(_mapper.Map<Employee>(model));
			if (result.IsSuccess)
			{
				//IManageService _manageService = serviceFactory.GetService("Employee");
				//await _manageService.PostAction(result.ReturnValue.ID);
				return Ok(_mapper.Map<EmployeeModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}

		[HttpPut]
		[ApiAuthorize(Core.Permission.Org_Employee, Core.Privilege.Update)]
		public async Task<IActionResult> Put(EmployeeModel model, [FromServices] IServiceFactory serviceFactory)
		{
			Result<Employee> result = await _service.UpdateAsync(_mapper.Map<Employee>(model));
			if (result.IsSuccess)
			{
				//IManageService _manageService = serviceFactory.GetService("Employee");
				//await _manageService.PutAction(result.ReturnValue.ID);
				return Ok(_mapper.Map<EmployeeModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}

	}
}
