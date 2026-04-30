using Microsoft.AspNetCore.Mvc;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.OnlineTest;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.OnlineTest.List;
using TranSmart.Domain.Models.OnlineTest.Request;
using TranSmart.Domain.Models.OnlineTest.Response;
using TranSmart.Service.OnlineTest;

namespace TranSmart.API.Controllers.OnlineTest
{
	[Route("api/OnlineTest/[controller]")]
	[ApiController]
	public class TestDesignationController : BaseController
	{
		public readonly IMapper _mapper;
		public readonly ITestDesignationService _service;

		public TestDesignationController(IMapper mapper, ITestDesignationService service)
		{
			_mapper = mapper;
			_service = service;
		}


		[HttpPost]
		public async Task<IActionResult> Post(TestDesignationRequest request)
		{
			TestDesignation entity = _mapper.Map<TestDesignation>(request);
			var result = await _service.AddAsync(entity);
			if (result.HasError)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

		[HttpGet("Paginate")]
		public async Task<IActionResult> Paginate([FromQuery] BaseSearch basesearch)
		{
			return Ok(_mapper.Map<Models.Paginate<TestDesignationList>>(await _service.GetPaginate(basesearch)));
		}


		[HttpPut("Delete/{testDesId}")]
		public async Task<IActionResult> Remove([FromRoute] Guid testDesId)
		{
			Result<TestDesignation> result = await _service.DeleteDesignation(testDesId);
			if (!result.HasError)
			{
				return Ok();
			}
			return BadRequest(result);
		}
	}
}
