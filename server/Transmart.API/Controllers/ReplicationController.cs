using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using TranSmart.API.Extensions;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models.Leave.Model;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Domain.Models.SelfService;
using TranSmart.Service.Leave;
using TranSmart.Domain.Models;
using TranSmart.Domain.Entities;
using TranSmart.Service;
using Microsoft.Extensions.Configuration;

namespace TranSmart.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReplicationController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IReplicationService _service;
		private readonly IConfiguration _configuration;
		public ReplicationController(IMapper mapper, IReplicationService service, IConfiguration configuration)
		{
			_mapper = mapper;
			_service = service;
			_configuration = configuration;
		}
		[HttpGet("Coding")]
		public async Task<IActionResult> Coding()	
		{
			Guid departmentId = Guid.Parse(_configuration["Coding"]);
			return Ok(_mapper.Map<IEnumerable<ReplicationModel>>(await _service.GetRelicationData(departmentId)));
		}

		[HttpGet("MT")]
		public async Task<IActionResult> MT()	
		{
			Guid departmentId = Guid.Parse(_configuration["MT"]);
			return Ok(_mapper.Map<IEnumerable<ReplicationModel>>(await _service.GetRelicationData(departmentId)));
		}
		[HttpGet("Billing")]
		public async Task<IActionResult> Billing()
		{
			Guid departmentId = Guid.Parse(_configuration["Billing"]);
			return Ok(_mapper.Map<IEnumerable<ReplicationModel>>(await _service.GetRelicationData(departmentId)));
		}
		[HttpPut]
		public async Task<IActionResult> Put(List<Guid> model)
		{
			Result<Replication> result = await _service.UpdateReplications(model);
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<ReplicationModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}
	}
}
