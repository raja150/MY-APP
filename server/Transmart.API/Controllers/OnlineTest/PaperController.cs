using Microsoft.AspNetCore.Mvc;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.OnlineTest;
using TranSmart.Domain.Models.OnlineTest;
using TranSmart.Domain.Models.OnlineTest.List;
using TranSmart.Domain.Models.OnlineTest.Request;
using TranSmart.Domain.Models.OnlineTest.Response;
using TranSmart.Service.OnlineTest;

namespace TranSmart.API.Controllers.OnlineTest
{
	[Route("api/OnlineTest/[controller]")]
	[ApiController]
	public class PaperController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IPaperService _service;

		public PaperController(IMapper mapper, IPaperService service)
		{
			_mapper = mapper;
			_service = service;
		}

		

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(Guid id)
		{
			return Ok(_mapper.Map<PaperModel>(await _service.GetById(id)));
		}


		[HttpGet("papers")] //filter in question list
		public async Task<IActionResult> GetPapers()
		{
			return Ok(_mapper.Map<IEnumerable<PaperModel>>(await _service.GetPapers(LOGIN_USER_EMPId)));
		}

		[HttpGet("Paginate")]
		public async Task<IActionResult> Paginate([FromQuery] PaperSearch baseSearch)
		{
			return Ok(_mapper.Map<Models.Paginate<PaperList>>(await _service.GetPaginate(baseSearch)));
		}

		[HttpPost]
		public async Task<IActionResult> Post(PaperRequest request)
		{
			Result<Paper> result = await _service.AddAsync(_mapper.Map<Paper>(request));
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<PaperRequest>(result.ReturnValue));
			}
			return BadRequest(result);
		}

		[HttpPut]
		public async Task<IActionResult> Put(PaperRequest request)
		{
			Result<Paper> result = await _service.UpdateAsync(_mapper.Map<Paper>(request));
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<PaperRequest>(result.ReturnValue));
			}
			return BadRequest(result);

		}

		[HttpGet("Duplicate/{paperId}")]
		public async Task<IActionResult> Duplicate(Guid paperId)
		{
			var result = await _service.Duplicate(paperId);
			if (result.IsSuccess)
			{
				return Ok();
			}
			return BadRequest(result);
		}
	}
}
