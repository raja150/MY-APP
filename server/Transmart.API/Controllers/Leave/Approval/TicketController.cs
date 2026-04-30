using Microsoft.AspNetCore.Mvc;
using TranSmart.API.Extensions;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.SelfService;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.SelfService.List;
using TranSmart.Domain.Models.SelfService.Request;
using TranSmart.Service.Approval;

namespace TranSmart.API.Controllers.Leave.Approval
{
	[Route("api/Leave/Approval/[controller]")]
	[ApiController]
	public partial class TicketController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IApprovalTicketService _service;
		public TicketController(IApprovalTicketService service, IMapper mapper)
		{
			_mapper = mapper;
			_service = service;
		}
		[HttpGet]
		[ApiAuthorize(Core.Permission.SA_Tickets, Core.Privilege.Read)]
		public async Task<IActionResult> TicketPaginate([FromQuery] BaseSearch baseSearch)
		{
			return Ok(_mapper.Map<Models.Paginate<TicketList>>(await _service.GetPaginate(baseSearch)));
		}
		[HttpPut]
		[ApiAuthorize(Core.Permission.SA_Tickets, Core.Privilege.Read)]
		public async Task<IActionResult> Put(TicketRequest model)
		{
			Result<Ticket> result = await _service.UpdateAsync(_mapper.Map<Ticket>(model));
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<TicketRequest>(result.ReturnValue));
			}
			return BadRequest(result);
		}

	}
}
