using Microsoft.AspNetCore.Mvc;
using TranSmart.API.Extensions;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Payroll;
using TranSmart.Domain.Models.PayRoll.List;
using TranSmart.Domain.Models.PayRoll.Request;
using TranSmart.Domain.Models.PayRoll.Response;
using TranSmart.Service.Payroll;
using TranSmart.Service.PayRoll;

namespace TranSmart.API.Controllers.PayRoll
{
	[Route("api/PayRoll/[controller]")]
	[ApiController]
	public class IncomeTaxLimitController : BaseController
	{

		private readonly IMapper _mapper;
		private readonly IIncomeTaxLimitService _service;
		private readonly IPayMonthService _pmService;
		public IncomeTaxLimitController(IMapper mapper, IIncomeTaxLimitService service, IPayMonthService pmService)
		{
			_mapper = mapper;
			_service = service;
			_pmService = pmService;

		}

		[HttpGet("Paginate")]
		[ApiAuthorize(Core.Permission.PS_IncomeTaxLimit, Core.Privilege.Read)]
		public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
		{
			return Ok(_mapper.Map<Models.Paginate<IncomeTaxLimitList>>(await _service.GetPaginate(baseSearch)));
		}

		[HttpGet("{id}")]
		[ApiAuthorize(Core.Permission.PS_IncomeTaxLimit, Core.Privilege.Read)]
		public async Task<IActionResult> Get(Guid id)
		{
			return Ok(_mapper.Map<IncomeTaxLimitModel>(await _service.GetById(id)));
		}

		[HttpPost]
		[ApiAuthorize(Core.Permission.PS_IncomeTaxLimit, Core.Privilege.Create)]
		public async Task<IActionResult> Post(IncomeTaxLimitRequest model)
		{
			Result<IncomeTaxLimit> executionResult = new();

			PayMonth month = await _pmService.GetPayMonth((Guid)model.PayMonthId);
			if (month == null)
			{
				executionResult.AddMessageItem(new MessageItem("Pay month is required"));
				return BadRequest(executionResult);
			}
			IncomeTaxLimit request = _mapper.Map<IncomeTaxLimit>(model);
			request.Month = month.Month;
			request.Year = month.Year;

			Result<IncomeTaxLimit> result = await _service.AddAsync(request);
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<IncomeTaxLimitModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}

		[HttpPut]
		[ApiAuthorize(Core.Permission.PS_IncomeTaxLimit, Core.Privilege.Update)]
		public async Task<IActionResult> Put(IncomeTaxLimitRequest model)
		{
			IncomeTaxLimit request = _mapper.Map<IncomeTaxLimit>(model);
			Result<IncomeTaxLimit> result = await _service.UpdateAsync(request);

			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<IncomeTaxLimitModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}

		[HttpGet("Search")]
		public async Task<IActionResult> Search(string name)
		{
			return Ok(_mapper.Map<IEnumerable<IncomeTaxLimitModel>>(await _service.Search(name)));
		}
		[HttpGet("PayMonths")]
		[ApiAuthorize(Core.Permission.PS_IncomeTaxLimit, Core.Privilege.Read)]
		public async Task<IActionResult> Months()
		{
			return Ok(_mapper.Map<List<PayMonthModel>>(await _pmService.GetPayMonths()));
		}

	}
}
