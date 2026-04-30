using Microsoft.AspNetCore.Mvc;
using TranSmart.Domain.Entities.OnlineTest;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.OnlineTest.List;
using TranSmart.Domain.Models.OnlineTest.Request;
using TranSmart.Service.OnlineTest;

namespace TranSmart.API.Controllers.OnlineTest
{

	[Route("api/OnlineTest/[controller]")]
	[ApiController]
	public class TestEmployeeController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly ITestEmployeeService _service;

		public TestEmployeeController(IMapper mapper, ITestEmployeeService service)
		{
			_mapper = mapper;
			_service = service;
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] TestEmployeeRequest request)
		{
			TestEmployee entity = _mapper.Map<TestEmployee>(request);
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
			return Ok(_mapper.Map<Models.Paginate<TestEmployeeList>>(await _service.GetPaginate(basesearch)));
		}


		[HttpPut("Delete/{TestEmpId}")]
		public async Task<IActionResult> Remove([FromRoute] Guid TestEmpId)
		{
			var result = await _service.DeleteEmployee(TestEmpId);
			if (result.IsSuccess)
			{
				return Ok();
			}
			return BadRequest();
		}


	}

}
