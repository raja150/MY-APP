using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Extensions;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave;
using TranSmart.Domain.Models.Leave.Approval;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Service.Leave;

namespace TranSmart.API.Controllers.Leave
{
	[Route("api/Leave/[controller]")]
	public partial class ApplyWfhController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IApplyWfhService _service;
		public ApplyWfhController(IMapper mapper, IApplyWfhService service)
		{
			_mapper = mapper;
			_service = service;
		}
		#region LeaveManagement 
		[HttpGet("GetList")]
		public async Task<IActionResult> GetList([FromQuery] string OrderBy)
		{
			return Ok(_mapper.Map<IEnumerable<ApplyWfhModel>>(await _service.GetList(OrderBy)));
		}

		[HttpGet("Paginate")]
		[ApiAuthorize(Core.Permission.LM_WFH, Core.Privilege.Read)]
		public async Task<IActionResult> Paginate([FromQuery] ApplyWfhSearch baseSearch)
		{
			return Ok(_mapper.Map<Models.Paginate<ApplyWfhList>>(await _service.GetPaginate(baseSearch)));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			return Ok(_mapper.Map<ApplyWfhModel>(await _service.GetById(id)));
		}
		[HttpGet("LM/{employeeId}")]
		public async Task<IActionResult> GEtPastFutureWFH([FromRoute] Guid employeeId, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
		{
			return Ok(_mapper.Map<IEnumerable<ApplyWfhModel>>(await _service.GetPastFutureWFH(employeeId, fromDate, toDate)));
		}
		[HttpPost("LeaveManagement")]
		public async Task<IActionResult> PostLM(ApplyWfhModel model)
		{
			model.Status = Convert.ToInt32(WfhStatus.Approved);
			model.ApprovedById = LOGIN_USER_EMPId;
			Result<ApplyWfh> result = await _service.AddAsync(_mapper.Map<ApplyWfh>(model));
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<ApplyWfhModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}
		[HttpPut("LM/Approve")]
		public async Task<IActionResult> LeavePut(ApproveRequest model)
		{
			Result<ApplyWfh> result;
			result = model.IsApproved ?
				   await _service.Approve(model.ID, LOGIN_USER_EMPId, true) :
				  await _service.Reject(model.ID, model.AdminReason, LOGIN_USER_EMPId);

			return result.IsSuccess ? Ok(_mapper.Map<ApplyWfhModel>(result.ReturnValue)) : BadRequest(result);
		}
		[HttpPut("LM/Reject")]
		public async Task<IActionResult> RejectWfhAfterApprove(ApproveRequest model)
		{
			Result<ApplyWfh> result = await _service.RejectAfterApprove(model.ID, model.RejectReason, LOGIN_USER_EMPId);
			return result.IsSuccess ? Ok(_mapper.Map<ApplyWfhModel>(result.ReturnValue)) : BadRequest(result);
		}
		#endregion


		#region SelfService
		[HttpPost("SelfService")]
		[ApiAuthorize(Core.Permission.SS_ApplyWFH, Core.Privilege.Create)]
		public async Task<IActionResult> Post(ApplyWfhModel model)
		{
			model.EmployeeId = LOGIN_USER_EMPId;
			Result<ApplyWfh> result = await _service.AddAsync(_mapper.Map<ApplyWfh>(model));
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<ApplyWfhModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}
		[HttpPut("SelfService")]
		[ApiAuthorize(Core.Permission.SS_ApplyWFH, Core.Privilege.Update)]
		public async Task<IActionResult> Put(ApplyWfhModel model)
		{
			model.EmployeeId = LOGIN_USER_EMPId;
			Result<ApplyWfh> result = await _service.UpdateAsync(_mapper.Map<ApplyWfh>(model));
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<ApplyWfhModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}
		[HttpPut("SelfService/Cancel/{id}")]
		public async Task<IActionResult> Cancel(Guid id)
		{
			Result<ApplyWfh> result = await _service.Cancel(id, LOGIN_USER_EMPId);
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<ApplyWfhModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}

		[HttpGet("Search")]
		public async Task<IActionResult> Search(string name)
		{
			return Ok(_mapper.Map<IEnumerable<ApplyWfhModel>>(await _service.Search(name)));
		}
		[HttpGet("SelfServiceSearch")]
		public async Task<IActionResult> SelfServiceSearch([FromQuery] ApplyWfhSearch baseSearch)
		{
			baseSearch.RefId = LOGIN_USER_EMPId;
			return Ok(_mapper.Map<Models.Paginate<ApplyWfhList>>(await _service.SelfServiceSearch(baseSearch)));
		}
		#endregion


		#region Approval
		[HttpGet("Approval/Paginate")]
		[ApiAuthorize(Core.Permission.SA_WFHApplicationApprove, Core.Privilege.Read)]
		public async Task<IActionResult> Approvals([FromQuery] ApplyWfhSearch baseSearch)
		{
			baseSearch.RefId = LOGIN_USER_EMPId;
			return Ok(_mapper.Map<Models.Paginate<ApplyWfhList>>(await _service.GetPaginate(baseSearch)));
		}
		//Calling from pop up
		[HttpGet("Approval/{id}")]
		[ApiAuthorize(Core.Permission.SA_WFHApplicationApprove, Core.Privilege.Read)]
		public async Task<IActionResult> GetApprovalById(Guid id)
		{
			return Ok(_mapper.Map<ApplyWfhModel>(await _service.GetLeave(id, LOGIN_USER_EMPId)));
		}
		[HttpPut("Approve")]
		[ApiAuthorize(Core.Permission.SA_WFHApplicationApprove, Core.Privilege.Update)]
		public async Task<IActionResult> ApprovalPut(ApproveRequest model)
		{
			Result<ApplyWfh> result;
			result = model.IsApproved ?
				await _service.Approve(model.ID, LOGIN_USER_EMPId, false) :
				await _service.Reject(model.ID, model.AdminReason, LOGIN_USER_EMPId);

			return result.IsSuccess ? Ok(_mapper.Map<ApplyWfhModel>(result.ReturnValue)) : BadRequest(result);
		}
		#endregion
	}
}
