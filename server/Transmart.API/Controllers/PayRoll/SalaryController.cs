using Aspose.Cells;
using Microsoft.AspNetCore.Mvc;
using TranSmart.API.Extensions;
using TranSmart.API.Services;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Payroll.List;
using TranSmart.Domain.Models.Payroll.Request;
using TranSmart.Domain.Models.Payroll.Response;
using TranSmart.Service;
using TranSmart.Service.Payroll;

namespace TranSmart.API.Controllers.Payroll
{
	[Route("api/PayRoll/[controller]")]
	[ApiController]
	public partial class SalaryController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly ISalaryService _service;
		private readonly IApplicationAuditLogService _auditLogService;
		public SalaryController(IMapper mapper, ISalaryService service, IApplicationAuditLogService auditLogService)
		{
			_mapper = mapper;
			_service = service;
			_auditLogService = auditLogService;
		}

		[HttpGet("Paginate")]
		[ApiAuthorize(Core.Permission.PS_EmployeeSalary, Core.Privilege.Read)]
		public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
		{
			await _auditLogService.GetAccesedUser(UserId, "View", IpAddress, (Core.Permission.PS_EmployeeSalary).ToString());
			return Ok(_mapper.Map<Models.Paginate<SalaryList>>(await _service.GetPaginate(baseSearch)));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			await _auditLogService.GetAccesedUser(UserId, "Read", IpAddress, (Core.Permission.PS_EmployeeSalary).ToString());
			return Ok(_mapper.Map<SalaryModel>(await _service.GetById(id)));
		}

		[HttpPost]
		[ApiAuthorize(Core.Permission.PS_EmployeeSalary, Core.Privilege.Read)]
		public async Task<IActionResult> Post(SalaryRequest model)
		{
			await _auditLogService.GetAccesedUser(UserId, "Created", IpAddress, (Core.Permission.PS_EmployeeSalary).ToString());
			var result = new Result<Salary>();
			OnPostValidation(model, result);
			if (result.HasError) return BadRequest(result);
			result = await _service.AddAsync(_mapper.Map<Salary>(model));
			if (result.HasError) return BadRequest(result);


			return Ok(_mapper.Map<SalaryModel>(result.ReturnValue));
		}

		[HttpPut]
		public async Task<IActionResult> Put(SalaryRequest model)
		{
			await _auditLogService.GetAccesedUser(UserId, "Modified", IpAddress, Core.Permission.PS_EmployeeSalary.ToString());
			var result = new Result<Salary>();
			OnPutValidation(model, result);
			if (result.HasError) return BadRequest(result);

			result = await _service.UpdateAsync(_mapper.Map<Salary>(model));
			if (result.HasError) return BadRequest(result);
			return Ok(_mapper.Map<SalaryModel>(result.ReturnValue));
		}

		[HttpGet("Components/{templateId}")]
		public async Task<IActionResult> GetTemplateComponents(Guid templateId)
		{
			return Ok(_mapper.Map<List<SalaryEarningModel>>(await _service.GetTemplateComponents(templateId)));
		}
		[HttpGet("Salary/{EmpId}")]
		public async Task<IActionResult> GetSalary(Guid EmpId)
		{

			var item = _mapper.Map<SalaryModel>(await _service.EmpSalary(EmpId));
			if (item != null)
			{
				return Ok(item);
			}
			else
			{
				return BadRequest();
			}
		}

		[HttpGet("EmployeeSalary")]
		public async Task<IActionResult> EmployeeSalary(string password)
		{
			var salaryList = new Dictionary<Guid, Dictionary<string, string>>();
			var salaries = await _service.GetEmployeesSalary();

			//salary list dictionary
			foreach (var salary in salaries)
			{
				var emp = new Dictionary<string, string>
				{
					{ "Employee No", salary.Employee.No },
					{ "Employee Name", salary.Employee.Name },
					{ "Department", salary.Employee.Department.Name },
					{ "Designation", salary.Employee.Designation.Name }
				};

				foreach (var earningComponent in
					salary.Earnings
					.OrderBy(x => x.Component.DisplayOrder).ThenBy(x => x.Component.Name))
				{
					emp.Add(earningComponent.Component.Name, earningComponent.Monthly.ToString());
				}
				foreach (var deductionComponent in
					salary.Deductions.OrderBy(x => x.Deduction.Name))
				{
					emp.Add(deductionComponent.Deduction.Name, deductionComponent.Monthly.ToString());
				}
				salaryList.Add(salary.ID, emp);
			}

			var list = new Dictionary<string, object[]>();
			var headers = new Dictionary<string, Tuple<string, int>>();

			//Headers
			foreach (var item in salaryList)
			{
				foreach (var item2 in item.Value)
				{
					if (headers.ContainsKey(item2.Key)) continue;
					headers.Add(item2.Key, new Tuple<string, int>(item2.Key, headers.Count));
				}
			}
			//column values
			foreach (var item in salaryList)
			{
				object[] Columnsfields = new object[headers.Count];
				foreach (var item2 in item.Value)
				{
					Columnsfields[headers[item2.Key].Item2] = item2.Value;
				}
				list.Add(item.Key.ToString(), Columnsfields);
			}
			var book = new Workbook(ClosedXmlGeneric.DataExport("Employee Salary", headers, list));

			book.Settings.Password = password;
			var stream = new MemoryStream();
			book.Save(stream, SaveFormat.Xlsx);
			stream.Seek(0, SeekOrigin.Begin);
			return new DownloadFile(stream, $"Employee Salary.xlsx");
		}

		#region Validation
		private static void OnPostValidation(SalaryRequest model, Result<Salary> executionResult)
		{
			if (model.Earnings.Where(x => !x.IsDeleted).GroupBy(x => x.ComponentId).Any(g => g.Count() > 1))
			{
				executionResult.AddMessageItem(new MessageItem("Duplicate Earning items"));
			}
			if (model.Deductions.Where(x => !x.IsDeleted).GroupBy(x => x.DeductionId).Any(g => g.Count() > 1))
			{
				executionResult.AddMessageItem(new MessageItem("Duplicate Deduction items"));
			}

		}
		private static void OnPutValidation(SalaryRequest model, Result<Salary> result)
		{
			if (model.Earnings.Where(x => !x.IsDeleted).GroupBy(x => x.ComponentId).Any(g => g.Count() > 1))
			{
				result.AddMessageItem(new MessageItem("Duplicate Earning items"));
			}
			if (model.Deductions.Where(x => !x.IsDeleted).GroupBy(x => x.DeductionId).Any(g => g.Count() > 1))
			{
				result.AddMessageItem(new MessageItem("Duplicate Deduction items"));
			}
		}

		#endregion
	}
}
