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
	public class TestDepartmentController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly ITestDepartmentService _service;

		public TestDepartmentController(IMapper mapper, ITestDepartmentService service)
		{
			_mapper = mapper;
			_service = service;
		}


		[HttpPost]
		public async Task<IActionResult> Post(TestDepartmentRequest request)
		{
			TestDepartment entity =  _mapper.Map<TestDepartment>(request);
			var result = await _service.AddAsync(entity);
			if (result.HasError)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}


		[HttpGet("Paginate")]
		public async Task<IActionResult> Paginate ([FromQuery] BaseSearch basesearch)
		{
			return Ok(_mapper.Map<Models.Paginate<TestDepartmentList>>(await _service.GetPaginate(basesearch)));
		}


		[HttpPut("Delete/{testDeptId}")]
		public async Task<IActionResult> Remove([FromRoute] Guid testDeptId)
		{
			Result<TestDepartment> result = await _service.DeleteDept(testDeptId);
			if (!result.HasError)
			{
				return Ok();
			}
			return BadRequest(result);
		}
	}
}
