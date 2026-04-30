using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using LeaveTypeImportRef = TranSmart.API.Models.Import;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Service.Leave;
using TranSmart.Service.Organization;
using TranSmart.API.Models;

namespace TranSmart.API.Controllers.Leave
{
	[Route("api/Leave/[controller]")]
	[ApiController]
	public class LeaveBalanceController : ControllerBase
	{
		private readonly Services.Import.LeaveBalanceService leaveBalance = new();

		private readonly IAdjustLeaveService _service;
		private readonly IEmployeeService _EmpService;
		private readonly ILeaveTypeService _LeaveTypeService;
		public LeaveBalanceController(IAdjustLeaveService service, IEmployeeService EmpSevice, ILeaveTypeService LTypeService)
		{
			_service = service;
			_EmpService = EmpSevice;
			_LeaveTypeService = LTypeService;

		}

		[HttpPost("file")]
		public async Task<IActionResult> Post([FromForm] ImportData model)
		{
			try
			{

				IEnumerable<LeaveTypeImportRef.LeaveBalance> list = leaveBalance.ToModel(model.FormFile.OpenReadStream());
				IEnumerable<LeaveType> LTypeList = await _LeaveTypeService.GetList("Name");
				IEnumerable<Employee> EmpList = (await _EmpService.GetList("Name")).ToList();
				foreach (LeaveTypeImportRef.LeaveBalance item in list)
				{
					var LModel = new AdjustLeave
					{
						EmployeeId = EmpList.Where(x => x.No.Equals(item.EmployeeCode, StringComparison.OrdinalIgnoreCase)).Select(e => e.ID).FirstOrDefault(),
						ID = Guid.NewGuid(),
						NewBalance = item.Leaves,
						Reason = item.Reason,
						LeaveTypeId = LTypeList.Where(l => l.Code.Equals(item.LeaveType, StringComparison.OrdinalIgnoreCase)).Select(l => l.ID).FirstOrDefault()
					};
					await _service.AddAsync(LModel);
				}
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}
	}
}
