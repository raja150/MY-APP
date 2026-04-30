using Microsoft.AspNetCore.Mvc;
using TranSmart.Domain.Models.OnlineTest;
using TranSmart.Domain.Models.OnlineTest.List;
using TranSmart.Service.OnlineTest;

namespace TranSmart.API.Controllers.OnlineTest
{
	[Route("api/OnlineTest/[controller]")]
	[ApiController]
	public class ResultController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IResultService _service;

		public ResultController(IMapper mapper, IResultService service)
		{
			_mapper = mapper;
			_service = service;
		}

		[HttpGet("Paginate")]
		public async Task<IActionResult> Paginate([FromQuery] ResultSearch search)
		{
			return Ok(_mapper.Map<Models.Paginate<ResultList>>(await _service.GetPaginate(search)));
		}

		[HttpGet("Summary/{id}")]
		public async Task<IActionResult> Summary(Guid id)
		{
			return Ok(await _service.GetSummary(id));
		}

		[HttpPut("ReTake/{empId}/{paperId}")]
		public async Task<IActionResult> ReTake(Guid empId, Guid paperId)
		{
			var result = await _service.AllowReTake(paperId, empId);
			if (result.IsSuccess)
			{
				return Ok(result);
			}
			return BadRequest(result);
		}
	}
}
