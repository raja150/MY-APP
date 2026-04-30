using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TranSmart.API.Extensions;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave;
using TranSmart.Domain.Models.Leave.Approval;
using TranSmart.Domain.Models.Leave.List;
using TranSmart.Domain.Models.Leave.Model;
using TranSmart.Domain.Models.Leave.Request;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Domain.Models.Reports;
using TranSmart.Domain.Models.SelfService;
using TranSmart.Service.Leave;

namespace TranSmart.API.Controllers.Leave
{
	[Route("api/LM/[controller]")]
	[ApiController]
	public partial class ApplyLeaveController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IApplyLeaveService _service;

		public ApplyLeaveController(IMapper mapper, IApplyLeaveService service)
		{
			_mapper = mapper;
			_service = service;
		}

		#region Leave Management  
		[HttpGet("Leave/Paginate")]
		[ApiAuthorize(Core.Permission.LM_Leaves, Core.Privilege.Read)]
		public async Task<IActionResult> LMApprovalPaginate([FromQuery] ApplyLeaveSearch baseSearch)
		{
			return Ok(_mapper.Map<Models.Paginate<ApplyLeavesList>>(await _service.ApprovalPaginate(baseSearch)));
		}

		[HttpPost("Leave")]
		[ApiAuthorize(Core.Permission.LM_Leaves, Core.Privilege.Create)]
		public async Task<IActionResult> PostLeave(ApplyLeaveRequest model)
		{
			model.IsPlanned = model.FromDate.Date > DateTime.Now.Date;
			ApplyLeave entity = _mapper.Map<ApplyLeave>(model);
			entity.ApprovedById = LOGIN_USER_EMPId;
			Result<ApplyLeave> result = await _service.AddApprovedLeaveAsync(entity);
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<ApplyLeaveModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}

		[HttpPut("Leave/Approve")]
		[ApiAuthorize(Core.Permission.LM_Leaves, Core.Privilege.Update)]
		public async Task<IActionResult> LeavePut(ApproveRequest model)
		{
			Result<ApplyLeave> result;
			result = model.IsApproved ?
				await _service.Approve(model.ID, LOGIN_USER_EMPId, true) :
				await _service.Reject(model.ID, model.RejectReason, LOGIN_USER_EMPId);

			return result.IsSuccess ? Ok(_mapper.Map<ApplyLeavesModel>(result.ReturnValue)) : BadRequest(result);
		}

		[HttpPut("Leave/Reject")]
		public async Task<IActionResult> RejectLeaveAfterApprove(ApproveRequest model)
		{
			Result<ApplyLeave> result = await _service.RejectAfterApprove(model.ID, model.RejectReason, LOGIN_USER_EMPId);
			return result.IsSuccess ? Ok(_mapper.Map<ApplyLeavesModel>(result.ReturnValue)) : BadRequest(result);
		}

		[HttpGet("Leave/{id}")]
		[ApiAuthorize(Core.Permission.LM_Leaves, Core.Privilege.Read)]
		public async Task<IActionResult> GetLeave(Guid id)
		{
			return Ok(_mapper.Map<ApplyLeaveModel>(await _service.GetById(id)));
		}
		[HttpGet("Leaves/{employeeId}")]
		public async Task<IActionResult> GetPastFutureWeekLeaves([FromRoute] Guid employeeId, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
		{
			return Ok(_mapper.Map<List<ApplyLeaveModel>>(await _service.GetWeekLeaves(employeeId, fromDate, toDate)));
		}
		[HttpGet("Leave/MonthLeaves")]
		public async Task<IActionResult> GetMonthLeaves(int month)
		{
			return Ok(_mapper.Map<List<LeaveInfoModel>>(await _service.GetLeaves(month, LOGIN_USER_DEPTID, LOGIN_USER_EMPId)));
		}
		[HttpGet("Leave/BetweenDates")]
		public async Task<IActionResult> GetLeavesBetweenTwoDates([FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
		{
			return Ok(_mapper.Map<List<LeaveInfoModel>>(await _service.GetLeavesBetweenTwoDates(fromDate, toDate, LOGIN_USER_DEPTID, LOGIN_USER_EMPId)));
		}
		[HttpGet("LeaveBalance/EmpLeaveType/{LeaveTypeId}/{EmpId}/{fromDate}/{toDate}")]
		public async Task<IActionResult> GetLeaveBalanceByEmpLeaveType(Guid LeaveTypeId, Guid EmpId, DateTime fromDate, DateTime toDate)
		{
			return Ok(await _service.GetLeaveBalanceByLeaveType(LeaveTypeId, EmpId, fromDate, toDate));
		}
		[HttpGet("EmpLeaveType/{EmpId}")]
		[ApiAuthorize(Core.Permission.LM_Leaves, Core.Privilege.Read)]
		public async Task<IActionResult> GetEmployeeLeaveTypes(Guid EmpId)
		{
			return Ok(await _service.GetEmployeeLeaveTypes(EmpId));
		}
		#endregion

		#region Self Service 
		[HttpGet("SelfService/Paginate")]
		[ApiAuthorize(Core.Permission.SS_ApplyLeaves, Core.Privilege.Read)]
		public async Task<IActionResult> SelfServicePaginate([FromQuery] ApplyLeaveSearch baseSearch)
		{
			baseSearch.RefId = LOGIN_USER_EMPId;
			if (string.IsNullOrEmpty(baseSearch.SortBy))
			{
				baseSearch.SortBy = "FromDate";
				baseSearch.IsDescend = true;
			}
			return Ok(_mapper.Map<Models.Paginate<ApplyLeaveList>>(await _service.SelfServiceSearch(baseSearch)));
		}
		[HttpGet("SelfService/{id}")]
		[ApiAuthorize(Core.Permission.SS_ApplyLeaves, Core.Privilege.Read)]
		public async Task<IActionResult> Get(Guid id)
		{
			return Ok(_mapper.Map<ApplyLeaveModel>(await _service.GetById(id)));
		}

		[HttpPost("SelfService")]
		[ApiAuthorize(Core.Permission.SS_ApplyLeaves, Core.Privilege.Create)]
		public async Task<IActionResult> SelfServicePost(ApplyLeaveRequest model)
		{
			model.EmployeeId = LOGIN_USER_EMPId;
			model.IsPlanned = model.FromDate.Date > DateTime.Now.Date;
			Result<ApplyLeave> result = await _service.MaximumLeavesValidation(_mapper.Map<ApplyLeave>(model));
			if (result.HasNoError)
			{
				result = await _service.AddAsync(_mapper.Map<ApplyLeave>(model));
				if (result.IsSuccess)
				{
					return Ok(_mapper.Map<ApplyLeaveModel>(result.ReturnValue));
				}
				return BadRequest(result);
			}
			return BadRequest(result);
		}

		[HttpPut("SelfService")]
		[ApiAuthorize(Core.Permission.SS_ApplyLeaves, Core.Privilege.Update)]
		public async Task<IActionResult> SelfServicePut(ApplyLeaveRequest model)
		{
			model.EmployeeId = LOGIN_USER_EMPId;
			model.IsPlanned = model.FromDate.Date > DateTime.Now.Date;
			Result<ApplyLeave> result = await _service.MaximumLeavesValidation(_mapper.Map<ApplyLeave>(model));
			if (result.HasNoError)
			{
				result = await _service.UpdateAsync(_mapper.Map<ApplyLeave>(model));
				if (result.IsSuccess)
				{
					return Ok(_mapper.Map<ApplyLeaveModel>(result.ReturnValue));
				}
				return BadRequest(result);
			}
			return BadRequest(result);
		}

		[HttpPut("SelfService/Cancel/{id}")]
		[ApiAuthorize(Core.Permission.SS_ApplyLeaves, Core.Privilege.Update)]
		public async Task<IActionResult> Cancel(Guid id)
		{
			Result<ApplyLeave> result = await _service.CancelAsync(id, LOGIN_USER_EMPId);
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<ApplyLeaveModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}
		[HttpGet("LeaveBalance/LeaveType/{LeaveTypeId}/{fromDate}/{toDate}")]
		[ApiAuthorize(Core.Permission.SS_ApplyLeaves, Core.Privilege.Read)]
		public async Task<IActionResult> GetLeaveBalanceByLeaveType(Guid LeaveTypeId, DateTime fromDate, DateTime toDate)
		{
			return Ok(await _service.GetLeaveBalanceByLeaveType(LeaveTypeId, LOGIN_USER_EMPId, fromDate, toDate));
		}
		[HttpGet("EmpLeaveType"),Authorize]
		//[ApiAuthorize(Core.Permission.SS_ApplyLeaves, Core.Privilege.Read)]
		public async Task<IActionResult> GetEmployeeLeaveTypes()
		{
			return Ok(await _service.GetEmployeeLeaveTypes(LOGIN_USER_EMPId));
		}
		#endregion

		#region Approval
		// Paginate
		[HttpGet("Approval/Paginate")]
		[ApiAuthorize(Core.Permission.SA_LeaveApplication, Core.Privilege.Read)]
		public async Task<IActionResult> ApprovalPaginate([FromQuery] ApplyLeaveSearch baseSearch)
		{
			baseSearch.RefId = LOGIN_USER_EMPId;
			return Ok(_mapper.Map<Models.Paginate<ApplyLeavesList>>(await _service.ApprovalPaginate(baseSearch)));
		}

		//Calling from pop up
		[HttpGet("Approval/{id}")]
		[ApiAuthorize(Core.Permission.SA_LeaveApplication, Core.Privilege.Read)]
		public async Task<IActionResult> GetApprovalById(Guid id)
		{
			return Ok(_mapper.Map<LeaveInfoModel>(await _service.GetLeave(id, LOGIN_USER_EMPId)));
		}

		[HttpPut("Approval/Approve")]
		[ApiAuthorize(Core.Permission.SA_LeaveApplication, Core.Privilege.Update)]
		public async Task<IActionResult> ApprovalPut(ApproveRequest model)
		{
			Result<ApplyLeave> result;
			result = model.IsApproved ?
				await _service.Approve(model.ID, LOGIN_USER_EMPId, false) :
				await _service.Reject(model.ID, model.RejectReason, LOGIN_USER_EMPId);

			return result.IsSuccess ? Ok(_mapper.Map<ApplyLeavesModel>(result.ReturnValue)) : BadRequest(result);
		}

		[HttpGet("LeaveDetails")]
		public async Task<IActionResult> LeaveDetailsPaginate([FromQuery] BaseSearch baseSearch)
		{
			baseSearch.RefId = LOGIN_USER_EMPId;
			return Ok(_mapper.Map<Models.Paginate<LeaveRequestDetailsModel>>(await _service.LeaveDetails(baseSearch)));
		}
		#endregion

	}
}
