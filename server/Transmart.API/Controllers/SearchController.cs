using Microsoft.AspNetCore.Mvc;
using TranSmart.API.Services;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models.Organization;
using TranSmart.Service;

namespace TranSmart.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SearchController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly ISearchService _service;
		private readonly ICacheService _cacheService;
		public SearchController(IMapper mapper, ISearchService service, ICacheService cacheService)
		{
			_mapper = mapper;
			_service = service;
			_cacheService = cacheService;
		}


		[HttpGet("SearchByMonth/{monthName}")]
		public IActionResult SearchAccount(string monthName)
		{
			return Ok(_service.GetPayMonths(monthName));
		}


		[HttpGet("SearchByEmployee/{employeeName}")]
		public async Task<IActionResult> SearchEmployeeAccount(string employeeName)
		{
			return Ok(await _service.GetEmployee(employeeName));
		}
		[HttpGet("SearchByEmpName")]
		public async Task<IActionResult> SearchByEmpName([FromQuery] string name)
		{
			return Ok(_mapper.Map<IEnumerable<EmployeeInfoModel>>(await _service.GetEmployeeDetails(name)));
		}

		[HttpGet("Employee/{name}")]
		public async Task<IActionResult> GetSearchedEmployees(string name)
		{
			var value = _cacheService.GetRolePrivileges().Result.FirstOrDefault(x => x.RoleId == RoleId
				   && x.PageId == Permissions.Attribute[(int)Permission.Org_Employee]);

			if (value == null || (value.Privilege & (int)Privilege.Read) != (int)Privilege.Read)
			{
				return Ok(await _cacheService.SearchedEmployee(name, LOGIN_USER_DEPTID));
			}
			return Ok(await _cacheService.SearchedEmployee(name, null));
		}
		[HttpGet("Designation/{name}/{monthId}")]//{id}/{monthId}
		public async Task<IActionResult> GetDesignations(string name, Guid monthId)//Guid id, 
		{
			return Ok((await _service.GetDesignations(monthId)).Where(x => x.Name.Contains(name)));
		}
		[HttpGet("SearchTeamEmployee/{name}")]
		public async Task<IActionResult> GetTeamEmployees(string name)
		{
			var result = new Result<Employee>();
			var data = await _cacheService.GetTeamEmployeeBySearch(LOGIN_USER_EMPId, name);
			if (data == null)
			{
				result.AddMessageItem(new MessageItem("Login user doesn't belongs to any team"));
				return BadRequest(result);
			}
			return Ok(await _cacheService.GetTeamEmployeeBySearch(LOGIN_USER_EMPId, name));
		}
		[HttpGet("HelpDesk/{name}/{ticketRaisedById}")]
		public async Task<IActionResult> HelpDeskSearchEmps(string name, Guid ticketRaisedById)
		{
			var emps = await _cacheService.SearchedEmployee(name, null);
			return Ok(emps.Where(x => x.ID != ticketRaisedById));
		}
		[HttpGet("DeskGroupEmps/{name}/{deptId}")]
		public async Task<IActionResult> HelpDeskSearchEmpsWithHDDept(string name, Guid deptId)
		{
			var emps = await _cacheService.DeskGroupEmps(Guid.Empty);
			return Ok(emps.Where(x => x.HdDeptId == deptId && x.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase)));
		}
		[HttpGet("ResultEmp/{name}")]
		public async Task<IActionResult> ResultEmpSearchWithName(string name)
		{
			return Ok(await _cacheService.GetResultEmp(name));
		}

		[HttpGet("ResultPaper/{name}")]
		public async Task<IActionResult> ResultPaperSearchWithName(string name)
		{
			return Ok(await _cacheService.GetResultPaper(name));    
		}

		[HttpGet("Paper/{name}")]
		public async Task<IActionResult> PaperSearchWithName(string name)
		{
			return Ok(await _cacheService.GetPaper(name));
		}

	}
}
