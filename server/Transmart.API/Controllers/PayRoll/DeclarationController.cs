using Aspose.Cells;
using Microsoft.AspNetCore.Mvc;
using TranSmart.API.Extensions;
using TranSmart.API.Services;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Payroll;
using TranSmart.Domain.Models.Payroll.List;
using TranSmart.Domain.Models.Payroll.Request;
using TranSmart.Domain.Models.Payroll.Response;
using TranSmart.Domain.Models.PayRoll.Search;
using TranSmart.Service;
using TranSmart.Service.Payroll;

namespace TranSmart.API.Controllers.Payroll
{
	[Route("api/PayRoll/[controller]")]
	[ApiController]
	public class DeclarationController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IDeclarationService _service;
		private readonly IFinancialYearService _fyservice;
		private readonly IApplicationAuditLogService _auditLogService;
		public DeclarationController(IMapper mapper, IDeclarationService service,
			IFinancialYearService financialYearService, IApplicationAuditLogService auditLogService)
		{
			_mapper = mapper;
			_service = service;
			_fyservice = financialYearService;
			_auditLogService = auditLogService;
		}

		[HttpPost]
		public async Task<IActionResult> Post(DeclarationRequest model)
		{
			if (model == null || (!IS_USER_EMPLOYEE)) return BadRequest("Invalid user");
			return await Save(model, base.LOGIN_USER_EMPId);
		}

		[HttpPost("Save")]
		[ApiAuthorize(Core.Permission.PS_Declaration, Core.Privilege.Update)]
		public async Task<IActionResult> AdminSave(DeclarationRequest model)
		{
			await _auditLogService.GetAccesedUser(UserId, "Created", IpAddress, Core.Permission.PS_Declaration.ToString());
			if (model == null || (!model.EmployeeId.HasValue)) return BadRequest("Invalid user");

			return await Save(model, model.EmployeeId.Value);
		}

		private async Task<IActionResult> Save(DeclarationRequest model, Guid empId)
		{
			Declaration declaration = _mapper.Map<Declaration>(model);
			declaration.EmployeeId = empId;
			var result = new Result<Declaration>();
			if (result.HasNoError)
			{
				result = await _service.AddAsync(declaration);
			}
			if (result.HasError)
			{
				return BadRequest(result);
			}

			else
			{
				return Ok(_mapper.Map<DeclarationModel>(await _service.GetById(result.ReturnValue.ID)));
			}
		}

		//For self employee
		[HttpPut]
		public async Task<IActionResult> Put(DeclarationRequest model)
		{
			if (model == null || (!IS_USER_EMPLOYEE)) return BadRequest("Invalid user");
			var item = await _service.GetSettings(model.FinancialYearId);
			if (item != null && item.Lock == 1)
			{
				return BadRequest("IT Declaration is locked");
			}
			return await Modify(model, base.LOGIN_USER_EMPId);
		}
		//for admin
		[HttpPut("Modify")]
		public async Task<IActionResult> AdminModify(DeclarationRequest model)
		{
			await _auditLogService.GetAccesedUser(UserId, "Modified", IpAddress, (Core.Permission.PS_Declaration).ToString());
			if (model == null || (!model.EmployeeId.HasValue)) return BadRequest("Invalid user");
			return await Modify(model, model.EmployeeId.Value);
		}

		private async Task<IActionResult> Modify(DeclarationRequest model, Guid empId)
		{
			Declaration entity = _mapper.Map<Declaration>(model);
			entity.EmployeeId = empId;
			Result<Declaration> result = await _service.UpdateAsync(entity);
			if (result.HasError)
			{
				return BadRequest(result);
			}
			return Ok(_mapper.Map<DeclarationModel>(await _service.GetById(model.ID)));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			await _auditLogService.GetAccesedUser(UserId, "Read", IpAddress, (Core.Permission.PS_Declaration).ToString());
			return Ok(_mapper.Map<DeclarationModel>(await _service.GetById(id)));
		}

		[HttpGet("MyDeclaration")]
		[ApiAuthorize(Core.Permission.SS_Declaration, Core.Privilege.Read)]
		public async Task<IActionResult> GetMyDeclaration([FromQuery] Guid Id)
		{
			if (!IS_USER_EMPLOYEE) return BadRequest();
			var v = await _service.GetDeclaration(Id, base.LOGIN_USER_EMPId);
			return Ok(_mapper.Map<DeclarationModel>(v));
		}

		[HttpGet("Declaration/{id}")]
		public async Task<IActionResult> GetDeclaration(Guid Id)
		{
			return Ok(_mapper.Map<DeclarationModel>(await _service.GetById(Id)));
		}

		[HttpGet("Paginate")]
		[ApiAuthorize(Core.Permission.PS_Declaration, Core.Privilege.Read)]
		public async Task<IActionResult> Paginate([FromQuery] DeclarationSearch baseSearch)
		{
			if (string.IsNullOrEmpty(baseSearch.SortBy))
			{
				baseSearch.SortBy = "Employee.Name";
				baseSearch.IsDescend = false;
			}
			await _auditLogService.GetAccesedUser(UserId, "Viewed", IpAddress, (Core.Permission.PS_Declaration).ToString());
			return Ok(_mapper.Map<Models.Paginate<DeclarationList>>(await _service.GetPaginate(baseSearch)));
		}

		[HttpGet("OpenYears")]
		//[ApiAuthorize(Core.Permission.PS_Declaration, Core.Privilege.Read)]
		public async Task<IActionResult> OpenYears()
		{
			return Ok(_mapper.Map<List<OpenFinancialYearList>>(await _fyservice.OpenYears()));
		}
		[HttpPut("CalculateForAll/{id}")]
		public async Task<IActionResult> CalculateForAll(Guid id)
		{
			await _auditLogService.GetAccesedUser(UserId, "Calculate for all", IpAddress, (Core.Permission.PS_Declaration).ToString());
			await _service.CalculateForAll(id);
			return Ok();
		}		 

		[HttpGet("TaxInfo/{id}/{password}")]
		[ApiAuthorize(Core.Permission.PS_Declaration, Core.Privilege.Read)]
		public async Task<IActionResult> PaySheet(Guid id, string password)
		{
			var disctionaryList = new Dictionary<string, object[]>();
			IEnumerable<Declaration> list = await _service.Declaration(id);
			var headers = new Dictionary<string, Tuple<string, int>>();

			headers.Add("Employee Code", new Tuple<string, int>("Employee Code", headers.Count));
			headers.Add("Employee Name", new Tuple<string, int>("Employee Name", headers.Count));
			headers.Add("Department", new Tuple<string, int>("Department", headers.Count));
			headers.Add("Designation", new Tuple<string, int>("Designation", headers.Count));
			headers.Add("PAN", new Tuple<string, int>("PAN", headers.Count));
			headers.Add("Salary", new Tuple<string, int>("Salary", headers.Count));
			headers.Add("Perquisites", new Tuple<string, int>("Perquisites", headers.Count));
			headers.Add("Previous Employment", new Tuple<string, int>("Previous Employment", headers.Count));
			headers.Add("Total Salary", new Tuple<string, int>("Total Salary", headers.Count));
			headers.Add("Allowance", new Tuple<string, int>("Allowance", headers.Count));
			headers.Add("Balance", new Tuple<string, int>("Balance", headers.Count));
			headers.Add("Standard Deduction", new Tuple<string, int>("Standard Deduction", headers.Count));
			headers.Add("TaxOn Employment", new Tuple<string, int>("TaxOn Employment", headers.Count));
			headers.Add("Deductions", new Tuple<string, int>("Deductions", headers.Count));
			headers.Add("Income Chargeable", new Tuple<string, int>("Income Chargeable", headers.Count));
			headers.Add("House Income", new Tuple<string, int>("House Income", headers.Count));
			headers.Add("Other Income", new Tuple<string, int>("Other Income", headers.Count));
			headers.Add("Gross Total", new Tuple<string, int>("Gross Total", headers.Count));
			headers.Add("EightyC", new Tuple<string, int>("EightyC", headers.Count));
			headers.Add("EPF", new Tuple<string, int>("EPF", headers.Count));
			headers.Add("EightyD", new Tuple<string, int>("EightyD", headers.Count));
			headers.Add("Other Sections", new Tuple<string, int>("Other Sections", headers.Count));
			headers.Add("Taxable", new Tuple<string, int>("Taxable", headers.Count));
			headers.Add("Tax", new Tuple<string, int>("Tax", headers.Count));
			headers.Add("Relief", new Tuple<string, int>("Relief", headers.Count));
			headers.Add("Cess", new Tuple<string, int>("Cess", headers.Count));
			headers.Add("Tax Payable", new Tuple<string, int>("Tax Payable", headers.Count));
			headers.Add("Tax Paid", new Tuple<string, int>("Tax Paid", headers.Count));
			headers.Add("Due", new Tuple<string, int>("Due", headers.Count));


			if (list.Any())
			{

				foreach (var declaration in list)
				{
					object[] Columnsfields = new object[headers.Count];
					Columnsfields[headers["Employee Code"].Item2] = declaration.Employee.No;
					Columnsfields[headers["Employee Name"].Item2] = declaration.Employee.Name;
					Columnsfields[headers["Department"].Item2] = declaration.Employee.Department.Name;
					Columnsfields[headers["Designation"].Item2] = declaration.Employee.Designation.Name;
					Columnsfields[headers["PAN"].Item2] = declaration.Employee.PanNumber;
					Columnsfields[headers["Salary"].Item2] = declaration.Salary;
					Columnsfields[headers["Perquisites"].Item2] = declaration.Perquisites;

					Columnsfields[headers["Previous Employment"].Item2] = declaration.PreviousEmployment;
					Columnsfields[headers["Total Salary"].Item2] = declaration.TotalSalary;
					Columnsfields[headers["Allowance"].Item2] = declaration.Allowance;

					Columnsfields[headers["Balance"].Item2] = declaration.Balance;
					Columnsfields[headers["Standard Deduction"].Item2] = declaration.StandardDeduction;
					Columnsfields[headers["TaxOn Employment"].Item2] = declaration.TaxOnEmployment;
					Columnsfields[headers["Deductions"].Item2] = declaration.Deductions;
					Columnsfields[headers["Income Chargeable"].Item2] = declaration.IncomeChargeable;
					Columnsfields[headers["House Income"].Item2] = declaration.HouseIncome;
					Columnsfields[headers["Other Income"].Item2] = declaration.OtherIncome;
					Columnsfields[headers["Gross Total"].Item2] = declaration.GrossTotal;
					Columnsfields[headers["EightyC"].Item2] = declaration.EightyC;
					Columnsfields[headers["EPF"].Item2] = declaration.EPF;
					Columnsfields[headers["EightyD"].Item2] = declaration.EightyD;
					Columnsfields[headers["Other Sections"].Item2] = declaration.OtherSections;
					Columnsfields[headers["Taxable"].Item2] = declaration.Taxable;
					Columnsfields[headers["Tax"].Item2] = declaration.Tax;
					Columnsfields[headers["Relief"].Item2] = declaration.Relief;
					Columnsfields[headers["Cess"].Item2] = declaration.Cess;
					Columnsfields[headers["Tax Payable"].Item2] = declaration.TaxPayable;
					Columnsfields[headers["Tax Paid"].Item2] = declaration.TaxPaid;
					Columnsfields[headers["Due"].Item2] = declaration.Due;

					disctionaryList.Add(declaration.Employee.No, Columnsfields);
				}
			}
			var book = new Workbook(ClosedXmlGeneric.DataExport("IT_Declaration", headers, disctionaryList));

			book.Settings.Password = password;
			var stream = new MemoryStream();
			book.Save(stream, SaveFormat.Xlsx);
			stream.Seek(0, SeekOrigin.Begin);
			return new DownloadFile(stream, $"IT_Declaration.xlsx");

		}
		[HttpGet("Settings/{id}")]
		public async Task<IActionResult> GetDeclarationSetting(Guid id)
		{
			return Ok(_mapper.Map<DeclarationSettingModel>(await _service.GetDeclarationSettings(id)));
		}
	}
}
