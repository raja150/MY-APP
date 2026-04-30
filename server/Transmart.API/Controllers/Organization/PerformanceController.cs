using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;
using TranSmart.Service.Organization;

namespace TranSmart.API.Controllers.Organization
{
	[Route("api/Organization/[controller]")]
	[ApiController]
	public class PerformanceController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IPerformanceService _service;
		public PerformanceController(IMapper mapper, IPerformanceService service)
		{
			_mapper = mapper;
			_service = service;
		}
		[HttpGet("Paginate")]
		public async Task<IActionResult> Paginate([FromQuery] PerformanceSearch baseSearch)
		{
			return Ok(_mapper.Map<Models.Paginate<PerformanceModel>>(await _service.GetPaginate(baseSearch)));
		}
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			return Ok(_mapper.Map<PerformanceModel>(await _service.GetById(id)));
		}
		[HttpPost]
		public async Task<IActionResult> Post(PerformanceModel model)
		{
			Result<Performance> result = await _service.AddAsync(_mapper.Map<Performance>(model));
			if (!result.HasError)
			{
				return Ok(_mapper.Map<PerformanceModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}
		[HttpPut]
		public async Task<IActionResult> Put(PerformanceModel model)
		{
			Result<Performance> result = await _service.UpdateAsync(_mapper.Map<Performance>(model));
			if (!result.HasError)
			{
				return Ok(_mapper.Map<PerformanceModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}
	}
}
