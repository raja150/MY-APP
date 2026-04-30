using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.DailyEvents;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave;
using TranSmart.Service.DailyEvents;

namespace TranSmart.API.Controllers.DailyEvents
{
	[Route("api/Daily_Events/[controller]")]
	[ApiController]
	public class DailyEventsController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IDailyEventsService _service;
		public DailyEventsController(IMapper mapper, IDailyEventsService service)
		{
			_mapper = mapper;
			_service = service;
		}

		[HttpGet("GetList")]
		public async Task<IActionResult> GetList([FromQuery] string OrderBy)
		{
			return Ok(_mapper.Map<IEnumerable<DailyEvent>>(await _service.GetList(OrderBy)));
		} 


		[HttpGet("Paginate")]
		public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
		{
			return Ok(_mapper.Map<Models.Paginate<DailyEvent>>(await _service.GetPaginate(baseSearch)));
		}

		[HttpGet("{id}")]
		public DailyEvent Get(Guid id)
		{
			return _mapper.Map<DailyEvent>(_service.GetById(id));
		}

		[HttpPost]
		public async Task<IActionResult> Post(DailyEvent model)
		{
			Result<DailyEvent> result = await _service.AddAsync(_mapper.Map<DailyEvent>(model));
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<DailyEvent>(result.ReturnValue));
			}
			return BadRequest(result);
		}

		[HttpPut]
		public async Task<IActionResult> Put(DailyEvent model)
		{
			Result<DailyEvent> result = await _service.UpdateAsync(_mapper.Map<DailyEvent>(model));
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<DailyEvent>(result.ReturnValue));
			}
			return BadRequest(result);
		}

		[HttpGet("Search")]
		public IEnumerable<DailyEvent> Search(string name)
		{
			return _mapper.Map<IEnumerable<DailyEvent>>(_service.Search(name));
		}
	}
}

