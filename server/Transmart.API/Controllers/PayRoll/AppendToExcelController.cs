using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TranSmart.Service.PayRoll;

namespace TranSmart.API.Controllers.PayRoll
{
	[Route("api/PayRoll/[controller]")]
	[ApiController]
	public class AppendToExcelController : BaseController
	{
		private readonly IAppendToExcelService _service;
		public AppendToExcelController(IAppendToExcelService service)
		{
			_service = service;
		}
		[HttpGet("Download/{type}/{payMonthId}")]
		public async Task<IActionResult> AppendToExcel(string type, Guid payMonthId)
		{
			switch (type.ToLower())
			{
				case "register_of_leaves":
					return Ok(await _service.AppendLeaveDetailsToExcel(LOGIN_USER_EMPId));
				case "weekly_holidays":
					return Ok(await _service.AppendWeeklyHolidaysDetailsToExcel());
				case "payment_of_wages":
					return Ok(await _service.AppendPaySheetToExcel(payMonthId));
				case "register_of_employment":
					return Ok(await _service.RegisterOfEmployment());
				case "register_for_equal_remuneration":
					return Ok(await _service.WageRegisterforEqualRemuneration(payMonthId));
				case "pf_upload":
					return Ok(await _service.PFUploadFormat(payMonthId));
				case "esi":
					return Ok(await _service.ESITemplate(payMonthId));
				case "bank_sheet":
					return Ok(await _service.AppendBankSheetToExcel(payMonthId));
				default: return BadRequest("Invalid Sheet");
			}
		}
	}
}
