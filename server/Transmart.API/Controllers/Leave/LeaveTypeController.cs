using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TranSmart.Domain.Models.Leave;

namespace TranSmart.API.Controllers.Leave
{
	public partial class LeaveTypeController
	{
		[HttpGet("DefaultPayOffType")]
		public async Task<IActionResult> GetDefaultPayOffLeaveType()
		{
			return Ok(await _service.GetDefaultPayOffLeaveType());
		}
		[HttpGet("GetPaidLeaveTypes")]
		public async Task<IActionResult> GetPaidLeaveTypeList()
		{
			return Ok(_mapper.Map<IEnumerable<LeaveTypeList>>(await _service.GetPaidLeaveTypeList()));
		}
	}
}
