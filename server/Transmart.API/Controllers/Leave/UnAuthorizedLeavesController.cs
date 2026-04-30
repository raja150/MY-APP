using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models.Leave;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave.Model;
using AutoMapper;
using System.Collections.Generic;
using TranSmart.API.Extensions;
using TranSmart.Service.Leave;
using TranSmart.Domain.Models.Leave.Search;

namespace TranSmart.API.Controllers.Leave
{
	[Route("api/LM/[controller]")]
	[ApiController]
	public partial class UnAuthorizedLeavesController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IUnAuthorizedLeavesService _service;
		public UnAuthorizedLeavesController(IMapper mapper, IUnAuthorizedLeavesService service)
		{
			_mapper = mapper;
			_service = service;
		}

		[HttpGet("Paginate")]
		public async Task<IActionResult> Paginate([FromQuery] UnAuthorizedLeavesSearch baseSearch)
		{
			baseSearch.RefId = LOGIN_USER_EMPId;
			return Ok(_mapper.Map<Models.Paginate<UnAuthorizedLeavesModel>>(await _service.GetPaginate(baseSearch)));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			return Ok(_mapper.Map<UnAuthorizedLeavesModel>(await _service.GetById(id)));
		}

		[HttpPost]
		public async Task<IActionResult> Post(UnAuthorizedLeavesModel model)
		{
			model.RefId = LOGIN_USER_EMPId;
			Result<UnAuthorizedLeaves> result = await _service.AddAsync(_mapper.Map<UnAuthorizedLeaves>(model));
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<UnAuthorizedLeavesModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}

		[HttpPut]
		public async Task<IActionResult> Put(UnAuthorizedLeavesModel model)
		{
			Result<UnAuthorizedLeaves> result = await _service.UpdateAsync(_mapper.Map<UnAuthorizedLeaves>(model));
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<UnAuthorizedLeavesModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}
	}
}
