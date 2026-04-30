using Microsoft.AspNetCore.Mvc;
using TranSmart.Domain.Models.OnlineTest;
using TranSmart.Domain.Models.OnlineTest.List;
using TranSmart.Domain.Models.OnlineTest.Request;
using TranSmart.Service.OnlineTest;

namespace TranSmart.API.Controllers.OnlineTest
{
	[Route("api/OnlineTest/[controller]")]
	[ApiController]
	public class CorrectionController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly ICorrectionService _service;
		public CorrectionController(IMapper mapper, ICorrectionService service)
		{
			_mapper = mapper;
			_service = service;
		}


		[HttpGet("List")]
		public async Task<IActionResult> GetList()
		{
			return Ok(await _service.CorrectionTestList(LOGIN_USER_EMPId));
		}


		[HttpGet("Question/{TestId}")]
		public async Task<IActionResult> GetQuestion(Guid testId)
		{
			return Ok(await _service.GetQuestions(testId));
		}

		[HttpGet("Answer/{questionId}")]
		public async Task<IActionResult> GetAnswer(Guid questionId)
		{
			return Ok(_mapper.Map<IEnumerable<AnswerModel>>(await _service.GetAnswers(questionId)));
		}

		[HttpPut("Correction")]
		public async Task<IActionResult> Correction(CorrectionRequest request)
		{
			var result = await _service.ManualCorrection(request.ManualAns, request.ID);
			if (result.IsSuccess)
			{
				return Ok();
			}
			return BadRequest();
		}

	}
}
