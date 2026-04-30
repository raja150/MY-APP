using AutoMapper;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Helpdesk;
using TranSmart.Service.Helpdesk;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TranSmart.API.Extensions;
using System.Threading.Tasks;
using TranSmart.API.ManageServices;

namespace TranSmart.API.Controllers.Helpdesk
{
	[Route("api/Helpdesk/[controller]")]
	[ApiController]
	public partial class DeskGroupEmployeeController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IDeskGroupEmployeeService _service;
		public DeskGroupEmployeeController(IMapper mapper, IDeskGroupEmployeeService service)
		{
			_mapper = mapper;
			_service = service;
		}

		[HttpGet("GetList")]
		public async Task<IActionResult> GetList([FromQuery] string OrderBy)
		{
			return Ok(_mapper.Map<IEnumerable<DeskGroupEmployeeList>>(await _service.GetList(OrderBy)));
		}

		[HttpGet("Paginate")]
		[ApiAuthorize(Core.Permission.HD_DeskGroup, Core.Privilege.Read)]
		public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
		{
			return Ok(_mapper.Map<Models.Paginate<DeskGroupEmployeeList>>(await _service.GetPaginate(baseSearch)));
		}

		[HttpGet("{id}")]
		[ApiAuthorize(Core.Permission.HD_DeskGroup, Core.Privilege.Read)]
		public async Task<IActionResult> Get(Guid id)
		{
			return Ok(_mapper.Map<DeskGroupEmployeeModel>(await _service.GetById(id)));
		}

		[HttpPost]
		[ApiAuthorize(Core.Permission.HD_DeskGroup, Core.Privilege.Create)]
		public async Task<IActionResult> Post(DeskGroupEmployeeModel model, [FromServices] IServiceFactory serviceFactory)
		{
			Result<DeskGroupEmployee> result = await _service.AddAsync(_mapper.Map<DeskGroupEmployee>(model));
			if (result.IsSuccess)
			{
				IManageService _manageService = serviceFactory.GetService("DeskGroupEmployee");
				await _manageService.PostAction(result.ReturnValue.ID);
				return Ok(_mapper.Map<DeskGroupEmployeeModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}

		[HttpPut]
		[ApiAuthorize(Core.Permission.HD_DeskGroup, Core.Privilege.Update)]
		public async Task<IActionResult> Put(DeskGroupEmployeeModel model, [FromServices] IServiceFactory serviceFactory)
		{
			Result<DeskGroupEmployee> result = await _service.UpdateAsync(_mapper.Map<DeskGroupEmployee>(model));
			if (result.IsSuccess)
			{
				IManageService _manageService = serviceFactory.GetService("DeskGroupEmployee");
				await _manageService.PutAction(result.ReturnValue.ID);
				return Ok(_mapper.Map<DeskGroupEmployeeModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}
	}
}
