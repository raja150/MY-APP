//using Microsoft.AspNetCore.Mvc;
//using TranSmart.Domain.Models.OnlineTest;
//using TranSmart.Domain.Models.OnlineTest.List;
//using TranSmart.Domain.Models.OnlineTest.Response;
//using TranSmart.Service.OnlineTest;

//namespace TranSmart.API.Controllers.SelfService
//{
//	[Route("api/SelfService/[controller]")]
//	[ApiController]
//	public class ResultController : BaseController
//	{
//		private readonly IMapper _mapper;
//		private readonly IResultService _service;

//		public ResultController(IMapper mapper, IResultService service)
//		{
//			_mapper = mapper;
//			_service = service;
//		}

//		[HttpGet("Paper")]//Filter
//		public async Task<IActionResult> GetPaperList()
//		{
//			return Ok(_mapper.Map<IEnumerable<ResultPaperModel>>(await _service.GetPapers(LOGIN_USER_EMPId)));
//		}


//		[HttpGet("Paginate")]
//		public async Task<IActionResult> GetPaginate([FromQuery] ResultSearch search)
//		{
//			search.RefId = LOGIN_USER_EMPId;
//			return Ok(_mapper.Map<Models.Paginate<ResultList>>(await _service.EmpPaginate(search)));
//		}
//	}
//}
