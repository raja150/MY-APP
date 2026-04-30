using Microsoft.AspNetCore.Mvc;
using TranSmart.API.Extensions;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models.Leave.Approval;
using TranSmart.Domain.Models.Leave.Model;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Domain.Models.SelfService;
using TranSmart.Service.Leave;

namespace TranSmart.API.Controllers.Leave
{

	[Route("api/Leave/[controller]")]
	[ApiController]
	public partial class ApplyClientVisitsController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IApplyClientVisitsService _service;
		public ApplyClientVisitsController(IMapper mapper, IApplyClientVisitsService service)
		{
			_mapper = mapper;
			_service = service;
		}
		#region SelfService
		[HttpGet("GetList")]
		public async Task<IActionResult> GetList([FromQuery] string OrderBy)
		{
			return Ok(_mapper.Map<IEnumerable<ApplyClientVisitsModel>>(await _service.GetList(OrderBy)));
		}

		[HttpGet("Paginate")]
		[ApiAuthorize(Core.Permission.SS_ApplyClientVisits, Core.Privilege.Read)]
		public async Task<IActionResult> Paginate([FromQuery] ApplyClientVisitSearch baseSearch)
		{
			baseSearch.RefId = LOGIN_USER_EMPId;
			return Ok(_mapper.Map<Models.Paginate<ApplyClientVisitsList>>(await _service.GetPaginate(baseSearch)));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			return Ok(_mapper.Map<ApplyClientVisitModel>(await _service.GetById(id)));
		}

		[HttpPost]
		[ApiAuthorize(Core.Permission.SS_ApplyClientVisits, Core.Privilege.Create)]
		public async Task<IActionResult> Post(ApplyClientVisitsModel model)
		{
			model.EmployeeId = LOGIN_USER_EMPId;
			model.Status = (int)ClientVisitStatus.Applied;
			ApplyClientVisits entity = _mapper.Map<ApplyClientVisits>(model);
			Result<ApplyClientVisits> result = await _service.AddAsync(entity);
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<ApplyClientVisitsModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}

		[HttpPut]
		[ApiAuthorize(Core.Permission.SS_ApplyClientVisits, Core.Privilege.Update)]
		public async Task<IActionResult> Put(ApplyClientVisitsModel model)
		{
			model.EmployeeId = LOGIN_USER_EMPId;
			Result<ApplyClientVisits> result = await _service.UpdateAsync(_mapper.Map<ApplyClientVisits>(model));
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<ApplyClientVisitsModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}

		[HttpGet("Search")]
		public async Task<IActionResult> Search(string name)
		{
			return Ok(_mapper.Map<IEnumerable<ApplyClientVisitsModel>>(await _service.Search(name)));
		}
		[HttpPut("SelfService/Cancel/{id}")]
		public async Task<IActionResult> Cancel(Guid id)
		{
			Result<ApplyClientVisits> result = await _service.Cancel(id, LOGIN_USER_EMPId);
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<ApplyClientVisitsModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}

		#endregion

		#region LeaveManagement
		[HttpGet("LMPaginate")]
		public async Task<IActionResult> LMPaginate([FromQuery] ApplyClientVisitSearch baseSearch)
		{
			return Ok(_mapper.Map<Models.Paginate<ApplyClientVisitsList>>(await _service.GetLMPaginate(baseSearch)));
		}

		[HttpGet("LM/{employeeId}")]
		public async Task<IActionResult> GetPastFutureVisits([FromRoute] Guid employeeId, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
		{
			return Ok(_mapper.Map<IEnumerable<ApplyClientVisitsModel>>(await _service.GetPastFutureVisits(employeeId, fromDate, toDate)));
		}

		[HttpPost("LeaveManagement")]
		public async Task<IActionResult> PostLM(ApplyClientVisitsModel model)
		{
			model.Status = Convert.ToInt32(ClientVisitStatus.Approved);
			model.ApprovedById = LOGIN_USER_EMPId;
			Result<ApplyClientVisits> result = await _service.AddAsync(_mapper.Map<ApplyClientVisits>(model));
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<ApplyClientVisits>(result.ReturnValue));
			}
			return BadRequest(result);
		}
		[HttpPut("LM/Approve")]
		public async Task<IActionResult> PutLM(ApproveRequest model)
		{
			Result<ApplyClientVisits> result;
			result = model.IsApproved ?
				   await _service.Approve(model.ID, LOGIN_USER_EMPId, true) :
				  await _service.Reject(model.ID, model.AdminReason, LOGIN_USER_EMPId);

			return result.IsSuccess ? Ok(_mapper.Map<ApplyClientVisitsModel>(result.ReturnValue)) : BadRequest(result);
		}
		#endregion

		#region Approve
		[HttpGet("Approval/Paginate")]
		[ApiAuthorize(Core.Permission.SA_ClientVisitApplications, Core.Privilege.Read)]
		public async Task<IActionResult> Approval([FromQuery] ApplyClientVisitSearch baseSearch)
		{
			baseSearch.RefId = LOGIN_USER_EMPId;
			return Ok(_mapper.Map<Models.Paginate<ApplyClientVisitsList>>(await _service.GetLMPaginate(baseSearch)));
		}
		[HttpPut("Approve")]
		[ApiAuthorize(Core.Permission.SA_ClientVisitApplications, Core.Privilege.Update)]
		public async Task<IActionResult> ApprovalPut(ApproveRequest model)
		{
			Result<ApplyClientVisits> result;
			result = model.IsApproved ?
				await _service.Approve(model.ID, LOGIN_USER_EMPId, false) :
				await _service.Reject(model.ID, model.AdminReason, LOGIN_USER_EMPId);

			return result.IsSuccess ? Ok(_mapper.Map<ApplyClientVisitsModel>(result.ReturnValue)) : BadRequest(result);
		}
		#endregion
	}
}
