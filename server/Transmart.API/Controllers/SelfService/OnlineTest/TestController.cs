using Microsoft.AspNetCore.Mvc;
using TranSmart.Domain.Entities.OnlineTest;
using TranSmart.Domain.Models.OnlineTest.List;
using TranSmart.Domain.Models.OnlineTest.Request;
using TranSmart.Domain.Models.OnlineTest.Response;
using TranSmart.Service.SelfService.OnlineTest;

namespace TranSmart.API.Controllers.SelfService.OnlineTest
{
	[Route("api/SelfService/[controller]")]
	[ApiController]
	public class TestController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly ITestService _service;
		public TestController(IMapper mapper, ITestService testService)
		{
			_mapper = mapper;
			_service = testService;
		}

		[HttpPost]
		public async Task<IActionResult> SaveAnswer(TestRequest model)
		{
			var entity = _mapper.Map<List<ResultQuestion>>(model.Answer);
			var result = await _service.SaveAnswer(entity, model.PaperId, model.IsFinish, LOGIN_USER_EMPId);
			if (result.IsSuccess)
			{
				return Ok();
			}
			return BadRequest(result);
		}

		[HttpGet("Start/{paperId}")] //Start
		public async Task<IActionResult> Start(Guid paperId)
		{
			var result = await _service.Start(paperId, LOGIN_USER_EMPId);
			if (result.HasNoError)
			{
				return Ok(_mapper.Map<List<TestModel>>(result.ReturnValue));
			}
			return BadRequest(result);
		}

		[HttpGet("List")]
		public async Task<IActionResult> TestList()
		{
			return Ok(await _service.TestList(LOGIN_USER_EMPId, LOGIN_USER_DEPTID, LOGIN_USER_DESGID));
		}

		[HttpGet("IsAllowed/{paperId}")]
		public async Task<IActionResult> IsAllowed(Guid paperId)
		{
			return Ok(await _service.IsAllowed(LOGIN_USER_EMPId, paperId));
		}

	}
}
