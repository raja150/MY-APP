using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TranSmart.API.Extensions;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models.Organization;
using TranSmart.Domain.Models.SelfService;

namespace TranSmart.API.Controllers.Organization
{

	public partial class EmployeeController : BaseController
	{
		[HttpGet("Emps/{empId}")]
		public async Task<IActionResult> GetEmp(Guid empId)
		{
			var item = await _service.GetEmp(empId);
			if (item != null)
			{
				return Ok(_mapper.Map<EmployeeModel>(item));
			}
			return NoContent();
		}

		[HttpGet("searchEmpList")]
		public async Task<IActionResult> SearchEmp([FromQuery] string name, Guid raiseById)
		{
			return Ok(_mapper.Map<List<EmployeeInfoModel>>(await _service.SearchEmp(name, LOGIN_USER_EMPId, raiseById)));
		}

		[HttpGet("searchEmp")]
		public async Task<IActionResult> SearchEmp([FromQuery] string name)
		{
			return Ok(_mapper.Map<IEnumerable<EmployeeInfoModel>>(await _service.SearchEmp(name)));
		}

		[HttpGet("EmpDetails")]
		public async Task<IActionResult> GetEmployeeDetails()
		{
			var id = LOGIN_USER_EMPId;
			return Ok(_mapper.Map<EmployeeAddModel>(await _service.GetDetails(id)));
		}

		[HttpGet("ResignationDetails")]
		public async Task<IActionResult> GetResignationDetails()
		{
			var id = LOGIN_USER_EMPId;
			return Ok(_mapper.Map<EmployeeResignationModel>(await _service.GetResignationDetails(id)));
		}

		[HttpGet("ContactDetails")]
		public async Task<IActionResult> GetContactDetails()
		{
			var id = LOGIN_USER_EMPId;
			return Ok(_mapper.Map<IEnumerable<EmployeeFamilyModel>>(await _service.GetContactDetails(id)));
		}

		[HttpGet("PresentAddress")]
		public async Task<IActionResult> GetPresentAddress()
		{
			var id = LOGIN_USER_EMPId;
			return Ok(_mapper.Map<EmployeePresentAdModel>(await _service.GetPresentAddress(id)));
		}
		[HttpGet("PermanentAddress")]
		public async Task<IActionResult> GetPermanentAddress()
		{
			var id = LOGIN_USER_EMPId;
			return Ok(_mapper.Map<EmployeePermanentAdModel>(await _service.GetPermanentAddress(id)));
		}

		[HttpGet("EmergencyAddress")]
		public async Task<IActionResult> GetEmergencyAddress()
		{
			var id = LOGIN_USER_EMPId;
			return Ok(_mapper.Map<EmployeeEmergencyAdModel>(await _service.GetEmergencyAddress(id)));
		}

		[HttpGet("Birthdays")]
		public async Task<IActionResult> GetBirthdays()
		{
			return Ok(_mapper.Map<List<EmployeeInfoModel>>(await _service.GetBirthdays(LOGIN_USER_DEPTID)));
		}

		[HttpGet("AllBirthdays")]
		[ApiAuthorize(Core.Permission.Org_Employee, Core.Privilege.Read)]
		public async Task<IActionResult> GetBirthdays([FromQuery] DateTime from, [FromQuery] DateTime to)
		{
			return Ok(_mapper.Map<List<EmployeeInfoModel>>(await _service.GetBirthdays(from, to)));
		}

		[HttpGet("LeavesApprovedEmployees")]
		public async Task<IActionResult> LeavesApprovedEmployees()
		{
			return Ok(_mapper.Map<List<EmployeeModel>>(await _service.LeavesApprovedEmployees(LOGIN_USER_DEPTID)));
		}

		[HttpGet("ApprovedPendingEmployees")]
		public async Task<IActionResult> ApprovedPendingEmployees()
		{
			return Ok(_mapper.Map<List<EmployeeModel>>(await _service.ApprovedPendingEmployees(LOGIN_USER_EMPId)));
		}

		[HttpGet("ApprovedPendingWFHEmployees")]
		public async Task<IActionResult> ApprovedPendingWFHEmployees()
		{
			var employeeId = LOGIN_USER_EMPId;
			return Ok(_mapper.Map<List<EmployeeModel>>(await _service.ApprovedPendingWFHEmployees(employeeId)));
		}

		[HttpGet("Image/{id}")]
		public async Task<IActionResult> GetEmployee(Guid id)
		{
			return Ok(_mapper.Map<EmployeeImageModel>(await _service.GetById(id)));
		}

		[HttpGet("Info")]
		public async Task<IActionResult> Info()
		{
			Employee employee = await _service.GetById(LOGIN_USER_EMPId);
			if (employee == null)
			{
				return BadRequest();
			}
			return Ok(new { employee.Name, employee.No });
		}
	}
}
