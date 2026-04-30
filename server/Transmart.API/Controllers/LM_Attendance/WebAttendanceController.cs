using Microsoft.AspNetCore.Mvc;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models.LM_Attendance;
using TranSmart.Domain.Models.LM_Attendance.List;
using TranSmart.Domain.Models.LM_Attendance.Request;
using TranSmart.Domain.Models.LM_Attendance.Responce;
using TranSmart.Service.LM_Attendance;

namespace TranSmart.API.Controllers.LM_Attendance
{
	[Route("api/[controller]")]
	[ApiController]
	public class WebAttendanceController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IWebAttendanceService _service;

		public WebAttendanceController(IMapper mapper, IWebAttendanceService service)
		{
			_mapper = mapper;
			_service = service;
		}
		#region SelfAttendance
		[HttpGet("IsPunchIn")]
		public async Task<IActionResult> GetPunchIn()
		{
			var item = await _service.GetPunchIn(LOGIN_USER_EMPId);
			if (item != null)
			{
				return Ok(_mapper.Map<WebAttendanceModel>(item));
			}
			return NoContent();
		}
		[HttpGet("IsPunchedEmployee")]
		public async Task<IActionResult> IsPunchedEmployee()
		{
			var result = await _service.IsPunchEmployee(LOGIN_USER_EMPId);
			return Ok(result);
		}
		[HttpPost]
		public async Task<IActionResult> Post(WebAttendanceRequest model)
		{
			model.EmployeeId = LOGIN_USER_EMPId;
			WebAttendance entity = _mapper.Map<WebAttendance>(model);
			Result<WebAttendance> result = await _service.AddAsync(entity);
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<WebAttendanceModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}
		[HttpPut]
		public async Task<IActionResult> Put()
		{
			var employeeId = LOGIN_USER_EMPId;
			Result<WebAttendance> result = await _service.UpdateTimings(employeeId);
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<WebAttendanceModel>(result.ReturnValue));
			}
			else
			{
				return BadRequest(result);
			}
		}
		[HttpPut("RePunchIn")]
		public async Task<IActionResult> RePunchIn()
		{
			var empId = LOGIN_USER_EMPId;
			Result<WebAttendance> result = await _service.RePunchIn(empId);
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<WebAttendanceModel>(result.ReturnValue));
			}
			else
			{
				return BadRequest(result);
			}
		}
		#endregion

		#region Approval
		[HttpGet("Paginate")]
		public async Task<IActionResult> ApprovalPaginate([FromQuery] WebAttendanceSearch baseSearch)
		{
			baseSearch.RefId = LOGIN_USER_EMPId;
			return Ok(_mapper.Map<Models.Paginate<WebAttendanceList>>(await _service.GetPaginate(baseSearch)));
		}
		[HttpGet("Approval/{id}")]
		public async Task<IActionResult> GetApprovalById(Guid id)
		{
			return Ok(_mapper.Map<WebAttendanceModel>(await _service.GetWebAttendance(id, LOGIN_USER_EMPId)));
		}
		[HttpPut("Approval")]
		public async Task<IActionResult> ApprovalUpdate(WebAttendanceRequest model)
		{
			Result<WebAttendance> result;
			model.ApprovedById = LOGIN_USER_EMPId;
			result = model.IsApproved ? await _service.ApprovalUpdate(model) : await _service.Reject(model);
			if (result.IsSuccess)
			{
				return Ok(_mapper.Map<WebAttendanceModel>(result.ReturnValue));
			}
			else
			{
				return BadRequest(result);
			}
		}
		#endregion

	}
}
