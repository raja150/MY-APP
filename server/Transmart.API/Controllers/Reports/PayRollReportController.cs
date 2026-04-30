using Microsoft.AspNetCore.Mvc;
using TranSmart.API.Extensions;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Service.Reports;

namespace TranSmart.API.Controllers.Reports
{
	[Route("api/[controller]")]
	[ApiController]
	public class PayRollReportController : BaseController
	{
		private readonly IPayRollReportService _service;
		public PayRollReportController(IPayRollReportService service)
		{
			_service = service;
		}
		[HttpGet("Arrears")]
		[ApiReportAuthorize(Core.ReportPermission.Payroll_Arrears)]
		public async Task<IActionResult> Arrears(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? payMonthAndYear)
		{
			var result = new Result<Arrear>();
			if (payMonthAndYear.HasValue)
			{
				int month = payMonthAndYear.Value.Month;
				int year = payMonthAndYear.Value.Year;
				return Ok(await _service.Arrears(departmentId, designationId, teamId, employeeId, year, month));

			}
			result.AddMessageItem(new MessageItem("Select Pay Month And Year"));
			return BadRequest(result);

		}
		[HttpGet("EmployeeESI")]
		[ApiReportAuthorize(Core.ReportPermission.Payroll_ESI)]
		public async Task<IActionResult> EmployeeESI(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? payMonthAndYear)
		{
			var result = new Result<EmpStatutory>();
			if (payMonthAndYear.HasValue)
			{
				int month = payMonthAndYear.Value.Month;
				int year = payMonthAndYear.Value.Year;
				return Ok(await _service.EmployeeESI(departmentId, designationId, teamId, employeeId, year, month));
			}
			result.AddMessageItem(new MessageItem("Select Pay Month And Year"));
			return BadRequest(result);

		}
		[HttpGet("EmployeeEPF")]
		[ApiReportAuthorize(Core.ReportPermission.Payroll_EPF)]
		public async Task<IActionResult> EmployeeEPF(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? payMonthAndYear)
		{
			var result = new Result<EmpStatutory>();
			if (payMonthAndYear.HasValue)
			{
				int month = payMonthAndYear.Value.Month;
				int year = payMonthAndYear.Value.Year;
				return Ok(await _service.EmployeeEPF(departmentId, designationId, teamId, employeeId, year, month));
			}
			result.AddMessageItem(new MessageItem("Select Pay Month And Year"));
			return BadRequest(result);
		}
		[HttpGet("IncentivePay")]
		[ApiReportAuthorize(Core.ReportPermission.Payroll_IncentivePay)]
		public async Task<IActionResult> IncentivesPayCut(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? payMonthAndYear)
		{
			var result = new Result<IncentivesPayCut>();
			if (payMonthAndYear.HasValue)
			{
				int month = payMonthAndYear.Value.Month;
				int year = payMonthAndYear.Value.Year;
				return Ok(await _service.IncentivesPayCuts(departmentId, designationId, teamId, employeeId, year, month));
			}
			result.AddMessageItem(new MessageItem("Select Pay Month And Year"));
			return BadRequest(result);

		}

		[HttpGet("ProfessionalTax")]
		[ApiReportAuthorize(Core.ReportPermission.Payroll_ProfessionalTax)]
		public async Task<IActionResult> ProfessionalTax(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? payMonthAndYear)
		{
			var result = new Result<PaySheet>();
			if (payMonthAndYear.HasValue)
			{
				int month = payMonthAndYear.Value.Month;
				int year = payMonthAndYear.Value.Year;
				return Ok(await _service.GetProfessionalTax(departmentId, designationId, teamId, employeeId, year, month));
			}
			result.AddMessageItem(new MessageItem("Select Pay Month And Year"));
			return BadRequest(result);

		}

		[HttpGet("IncomeTax")]
		[ApiReportAuthorize(Core.ReportPermission.Payroll_IncomeTax)]
		public async Task<IActionResult> GetIncomeTax(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, DateTime? payMonthAndYear)
		{
			var result = new Result<PaySheet>();
			if (payMonthAndYear.HasValue)
			{
				int month = payMonthAndYear.Value.Month;
				int year = payMonthAndYear.Value.Year;
				return Ok(await _service.GetIncomeTax(departmentId, designationId, teamId, employeeId, year, month));
			}
			result.AddMessageItem(new MessageItem("Select Pay Month And Year"));
			return BadRequest(result);

		}

		[HttpGet("PaymentInfo")]
		public async Task<IActionResult> PaymentInfo(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId)
		{
			return Ok(await _service.GetPaymentInfo(departmentId, designationId, teamId, employeeId));
		}
		[HttpGet("ProvidentFundInfo")]
		public async Task<IActionResult> ProvidentFundInfo(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int? type)
		{
			return Ok(await _service.GetProvidentFundInfo(departmentId, designationId, teamId, employeeId, type));
		}
		[HttpGet("ESIInfo")]
		public async Task<IActionResult> ESIInfo(Guid? departmentId, Guid? designationId, Guid? teamId, Guid? employeeId, int? type)
		{
			return Ok(await _service.GetESIInfo(departmentId, designationId, teamId, employeeId, type));
		}
	}
}
