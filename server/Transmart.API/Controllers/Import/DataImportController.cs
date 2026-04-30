//using Microsoft.AspNetCore.Mvc;
//using System.Text;
//using System.Text.RegularExpressions;
//using TranSmart.API.Extensions;
//using TranSmart.API.Models;
//using TranSmart.API.Models.Import;
//using TranSmart.Core.Extension;
//using TranSmart.Core.Result;
//using TranSmart.Core.Util;
//using TranSmart.Domain.Entities.Leave;
//using TranSmart.Domain.Entities.OnlineTest;
//using TranSmart.Domain.Entities.Organization;
//using TranSmart.Domain.Entities.Payroll;
//using TranSmart.Domain.Entities.PayRoll;
//using TranSmart.Domain.Enums;
//using TranSmart.Domain.Models.OnlineTest.Response;
////using TranSmart.Service.Leave;
//using TranSmart.Service.OnlineTest;
//using TranSmart.Service.Organization;
//using TranSmart.Service.Payroll;
//using TranSmart.Service.PayRoll;
//using TranSmart.Service.Schedules;
//using ImportRef = TranSmart.API.Models.Import;

//namespace TranSmart.API.Controllers.Import
//{
//	[Route("api/[controller]")]
//	[ApiController]
//	public class DataImportController : BaseController
//	{
//		private readonly IMapper _mapper;
//		private readonly IAdjustLeaveService _leaveService;
//		private readonly IAttendanceService _attendanceService;
//		private readonly IIncentivesPayCutService _incentivesPayCutService;
//		private readonly IEarningComponentService _earningComponentService;
//		private readonly ISalaryService _salService;
//		private readonly IEmployeePayInfoService _employeePayInfoService;
//		private readonly IEmployeeService _employeeService;
//		private readonly ILeaveTypeService _leaveTypeService;
//		private readonly IPayMonthService _payMonthService;
//		private readonly IEmpStatutoryService _empStatutoryService;
//		private readonly IEmpBonusService _bonusService;
//		private readonly IArrearService _arrearService;
//		private readonly IIncomeTaxLimitService _incomeTaxLimitService;
//		private readonly IDeductionComponentService _deductionComponentService;
//		private readonly IBankService _bankService;
//		private readonly IDepartmentService _departmentService;
//		private readonly IDesignationService _designationService;
//		private readonly ILocationService _locService;
//		private readonly ILatecomersService _latecomersService;
//		private readonly IAttendanceSumService _attendanceSumService;
//		private readonly ILOBService _lobService;
//		private readonly IFunctionalAreaService _functionalAreaService;
//		private readonly ITeamService _teamService;
//		private readonly IWorkTypeService _workTypeService;
//		private readonly IPaperService _paperService;
//		private readonly IQuestionService _questionService;

//		public DataImportController(IMapper mapper,
//			IAdjustLeaveService service,
//			IEmployeeService empService, ILeaveTypeService leaveType,
//			IAttendanceService attService, IEmpBonusService bonusService,
//			IScheduleService scheduleService, IEarningComponentService componentService,
//			ISalaryService salaryService, IPayMonthService payMonthService,
//			IEmpStatutoryService empStatutoryService, IArrearService arrearService,
//			IIncentivesPayCutService incentivesPayCutService,
//			IIncomeTaxLimitService incomeTaxLimitService,
//			IDeductionComponentService deductionComponentService,
//			IEmployeePayInfoService employeePayInfoService,
//			IBankService bankService,
//			IDepartmentService departmentService,
//			IDesignationService designationService,
//			IPaperService paperService, IQuestionService questionService,
//			ILocationService locService, ILatecomersService latecomersService, IAttendanceSumService attendanceSumService,
//			ILOBService lobService, IFunctionalAreaService functionalAreaService, ITeamService teamService, IWorkTypeService workTypeService)
//		{
//			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
//			_employeeService = empService;
//			_leaveService = service;
//			_leaveTypeService = leaveType;
//			_attendanceService = attService;
//			_bonusService = bonusService;
//			_arrearService = arrearService;
//			_earningComponentService = componentService;
//			_salService = salaryService;
//			_payMonthService = payMonthService;
//			_empStatutoryService = empStatutoryService;
//			_incentivesPayCutService = incentivesPayCutService;
//			_incomeTaxLimitService = incomeTaxLimitService;
//			_deductionComponentService = deductionComponentService;
//			_employeePayInfoService = employeePayInfoService;
//			_bankService = bankService;
//			_departmentService = departmentService;
//			_designationService = designationService;
//			_locService = locService;
//			_latecomersService = latecomersService;
//			_attendanceSumService = attendanceSumService;
//			_lobService = lobService;
//			_functionalAreaService = functionalAreaService;
//			_teamService = teamService;
//			_workTypeService = workTypeService;
//			_paperService = paperService;
//			_questionService = questionService;
//		}

//		#region Sample XLSX File

//		[HttpGet("Sample")]
//		public async Task<IActionResult> Sample([FromQuery] string type)
//		{
//			switch (type.ToLower())
//			{
//				case "leave_balance":
//					var leave = new Services.Import.LeaveBalanceService();
//					return new DownloadFile(leave.Sample(), $"{type}.xlsx");
//				case "attendancelogsimport":
//					var attendance = new Services.Import.AttendanceService();
//					return new DownloadFile(attendance.Sample(), $"{type}.xlsx");
//				case "incentives_pay_cut":
//					var payCutService = new Services.Import.IncentivesPayCutService();
//					return new DownloadFile(payCutService.Sample(), $"{type}.xlsx");
//				case "epfesi":
//					var empStatutoryService = new Services.Import.EmpStatutoryImportService();
//					return new DownloadFile(empStatutoryService.Sample(), $"{type}.xlsx");
//				case "paysheet":
//					var payDictionaryList = new Dictionary<int, string>();
//					List<DeductionComponent> deductionList = (await _deductionComponentService.GetList("DisplayOrder")).ToList();
//					List<EarningComponent> earningList = (await _earningComponentService.GetList("DisplayOrder")).ToList();
//					var headerSet1 = new string[] { "Employee Code", "Work Days", "Present Days", "LOP Days", "UA Days", "Salary" };
//					foreach (string item in headerSet1)
//					{
//						payDictionaryList.Add(payDictionaryList.Count, item);
//					}
//					//Add earnings and deduction headers between static headers
//					foreach (EarningComponent item in earningList)
//					{
//						payDictionaryList.Add(payDictionaryList.Count, "Salary " + item.Name);
//					}
//					foreach (DeductionComponent item in deductionList.Where(x => x.EarningId == null))
//					{
//						payDictionaryList.Add(payDictionaryList.Count, "Salary " + item.Name);
//					}
//					foreach (EarningComponent item in earningList)
//					{
//						payDictionaryList.Add(payDictionaryList.Count, "Earning " + item.Name);
//					}

//					foreach (DeductionComponent item in deductionList.Where(x => x.EarningId == null))
//					{
//						payDictionaryList.Add(payDictionaryList.Count, "Deduction " + item.Name);
//					}
//					var headerSet2 = new string[] { "Loan", "Gross", "Deduction",
//						"Net", "Incentive", "Arrears", "LOP", "PayCut", "EPF", "ESI", "ESI Applied", "PTax", "Tax", "Hold", "EPF No",
//						"ESI No", "Bank Name", "Bank IFSC", "Bank AcNo", "Cheque Pay" };
//					foreach (string item in headerSet2)
//					{
//						payDictionaryList.Add(payDictionaryList.Count, item);
//					}

//					var paySheetImportService = new Services.Import.PaysheetImportService();
//					return new DownloadFile(paySheetImportService.Sample(payDictionaryList), $"{type}.xlsx");
//				case "bonus":
//					var bonus = new Services.Import.BonusService();
//					return new DownloadFile(bonus.Sample(), $"{type}.xlsx");
//				case "arrear":
//					var arrear = new Services.Import.ArrearService();
//					return new DownloadFile(arrear.Sample(), $"{type}.xlsx");
//				case "incometaxlimit":
//					var IncomeTaxLimit = new Services.Import.IncomeTaxLimitService();
//					return new DownloadFile(IncomeTaxLimit.Sample(), $"{type}.xlsx");
//				case "salaries":
//					var salaryList = new Dictionary<int, string>();
//					List<EarningComponent> salayEarning = (await _earningComponentService.GetList("DisplayOrder")).ToList();
//					List<DeductionComponent> salaryDeduction = (await _deductionComponentService.GetList("DisplayOrder")).ToList();
//					var salariesHeader = new string[] { "Employee Code", "Salary Per Month" };
//					foreach (string item in salariesHeader)
//					{
//						salaryList.Add(salaryList.Count, item);
//					}
//					foreach (EarningComponent item in salayEarning.Where(x => x.Status))
//					{
//						salaryList.Add(salaryList.Count, item.Name);
//					}
//					foreach (DeductionComponent item in salaryDeduction.Where(x => x.Name != "Food Coupons"))
//					{
//						salaryList.Add(salaryList.Count, item.Name);
//					}
//					var salImport = new Services.Import.SalaryImportService();
//					return new DownloadFile(salImport.Sample(salaryList), $"{type}.xlsx");
//				case "paymentinfo":
//					var payInfoImport = new Services.Import.PaymentInfoService();
//					return new DownloadFile(payInfoImport.Sample(), $"{type}.xlsx");
//				case "attendanceupdate":
//					var attendanceStatus = new Services.Import.AttendanceStatusService();
//					return new DownloadFile(attendanceStatus.Sample(), $"{type}.xlsx");
//				case "employee":
//					var employeeService = new Services.Import.EmployeeService();
//					return new DownloadFile(employeeService.Sample(), $"{type}.xlsx");
//				case "latecomers":
//					var latecomers = new Services.Import.LatecomersService();
//					return new DownloadFile(latecomers.Sample(), $"{type}.xlsx");
//				case "attendance":
//					var attendances = new Services.Import.AttendanceImportService();
//					return new DownloadFile(attendances.Sample(), $"{type}.xlsx");
//				case "question":
//					var questions = new Services.Import.QuestionService();
//					return new DownloadFile(questions.Sample(), $"{type}.xlsx");
//				case "addattendance":
//					var addattendance = new Services.Import.AddAttendanceService();
//					return new DownloadFile(addattendance.Sample(), $"{type}.xlsx");
//				default:
//					return BadRequest("Invalid parameter");
//			}
//		}
//		#endregion

//		#region Get Attendance
//		[HttpGet("Attendance")]
//		[ApiAuthorize(Core.Permission.PS_Import, Core.Privilege.Read)]
//		public async Task<IActionResult> GetAttendanceReports(DateTime fromDate, DateTime toDate)
//		{
//			var dictionaryList = new Dictionary<string, Dictionary<string, string>>();
//			List<TranSmart.Domain.Entities.Leave.LeaveType> lTypeList = (await _leaveTypeService.GetList("")).ToList();
//			List<Employee> empList = (await _employeeService.GetList("")).ToList();

//			List<TranSmart.Domain.Entities.Leave.Attendance> attendance = (await _attendanceService.GetAttendance(fromDate, toDate)).ToList();
//			foreach (Employee emp in empList)//Emp wise
//			{
//				var columnsList = new Dictionary<string, string>
//				{
//					{ "Employee Code", emp.No },
//					{ "Employee Name", emp.Name }
//				};
//				for (DateTime date = fromDate; date <= toDate; date = date.AddDays(1))//date Wise
//				{
//					TranSmart.Domain.Entities.Leave.Attendance attDate = attendance.FirstOrDefault(a => a.AttendanceDate.Date == date.Date);
//					if (attDate != null)
//					{
//						switch (attDate.AttendanceStatus)
//						{
//							case 0:
//								columnsList.Add(attDate.AttendanceDate.ToString(), "Present");
//								break;
//							case 1:
//								string leaveType = lTypeList.Where(l => l.ID == attDate.LeaveTypeID).Select(l => l.Code).FirstOrDefault();
//								columnsList.Add(attDate.AttendanceDate.ToString(), "Leave( " + leaveType + ")");
//								break;
//							case 2:
//								columnsList.Add(attDate.AttendanceDate.ToString(), "Absent");
//								break;
//							case 3:
//								columnsList.Add(attDate.AttendanceDate.ToString(), "WeekOff");
//								break;
//							case 4:
//								columnsList.Add(attDate.AttendanceDate.ToString(), "Holiday");
//								break;
//							case 5:
//								columnsList.Add(attDate.AttendanceDate.ToString(), "Unauthorized");
//								break;
//							default:
//								columnsList.Add(attDate.AttendanceDate.ToString(), "Default");
//								break;
//						}
//					}
//					else
//					{
//						columnsList.Add(date.ToString(), "");
//					}
//				}
//				dictionaryList.Add(emp.No, columnsList);
//			}
//			var attImport = new Services.Import.AttendanceService();
//			return new DownloadFile(attImport.Attendance(dictionaryList), $"Attendance.xlsx");
//		}

//		#endregion

//		#region Arrear

//		[HttpPost("Arrear")]
//		public async Task<IActionResult> Arrear([FromForm] ImportData model)
//		{
//			var result = new FileImportResult<List<ImportRef.ArrearModel>>();
//			var results = new FileImportResult<List<Arrear>>();
//			if (!model.Date.HasValue)
//			{
//				result.AddMessageItem(new MessageItem("Please select month and year"));
//				return BadRequest(result);
//			}
//			if (!await _payMonthService.CheckPayMonthIsOpen(model.Date.Value.Year, model.Date.Value.Month))
//			{
//				result.AddMessageItem(new MessageItem("Selected month pay roll is closed"));
//				return BadRequest(result);
//			}

//			var arrear = new Services.Import.ArrearService();
//			if (arrear.ValidateHeaders(model.FormFile.OpenReadStream()))
//			{
//				IEnumerable<ImportRef.ArrearModel> list = arrear.ToModel(model.FormFile.OpenReadStream());
//				IEnumerable<Employee> employees = await _employeeService.GetList("Name");
//				var arrears = new List<Arrear>();
//				var errors = new List<ImportRef.ArrearModel>();
//				foreach (ImportRef.ArrearModel item in list)
//				{
//					Employee e = employees.FirstOrDefault(x => x.No.Equals(item.EmployeeCode, StringComparison.OrdinalIgnoreCase));
//					if (e == null)
//					{
//						item.Error = "Invalid employee code";
//						errors.Add(item);
//						continue;
//					}
//					if (int.TryParse(item.Pay.ToString(), out int pay) && pay < 0)
//					{
//						item.Error = "Pay should not be negative value";
//						errors.Add(item);
//						continue;
//					}
//					arrears.Add(new Arrear
//					{
//						ID = Guid.NewGuid(),
//						EmployeeID = e.ID,
//						Pay = item.Pay,
//						Month = model.Date.Value.Month,
//						Year = model.Date.Value.Year
//					});
//				}
//				if (errors.Count > 0)
//				{
//					result.AddMessageItem(new MessageItem("Invalid data found in sheet. Please correct the data and re-upload."));
//					result.ReturnValue = errors;
//					result.Headers = _mapper.Map<List<Core.HeaderModel>>(arrear.GetAllColumns());
//				}
//				else
//				{
//					var bulkAddResult = await _arrearService.AddBulk(arrears, model.Date.Value.Month, model.Date.Value.Year);
//					if (bulkAddResult.HasNoError)
//					{
//						result.Headers = new List<Core.HeaderModel>
//						{
//							new Core.HeaderModel
//							{
//								Name = "Total Employees",
//								Order = 1,
//								PropertyName = "employeeCode"
//							},
//							new Core.HeaderModel
//							{
//								Name = "Sum of Pay",
//								Order = 2,
//								PropertyName = "pay"
//							}
//						};
//						result.ReturnValue = new List<ArrearModel>()
//						{
//							new ArrearModel
//							{
//								EmployeeCode = bulkAddResult.ReturnValue["Employees"].ToString(),
//								Pay = bulkAddResult.ReturnValue["Pay"]
//							}
//						};
//					}
//					return bulkAddResult.HasError ? BadRequest(bulkAddResult) : Ok(result);
//				}
//				return Ok(result);
//			}
//			else
//			{
//				return BadRequest(result);
//			}
//		}
//		#endregion

//		#region IncomeTaxLimit

//		[HttpPost("IncomeTaxLimit")]
//		[ApiAuthorize(Core.Permission.PS_Import, Core.Privilege.Read)]

//		public async Task<IActionResult> IncomeTaxLimit([FromForm] ImportData model)
//		{
//			try
//			{
//				var result = new FileImportResult<List<ImportRef.IncomeTaxLimitModel>>();
//				if (!model.Date.HasValue)
//				{
//					result.AddMessageItem(new MessageItem("Please select month and year"));
//					return BadRequest(result);
//				}
//				var payMonth = await _payMonthService.GetPayMonthDetails(model.Date.Value.Year, model.Date.Value.Month);

//				if (payMonth == null)
//				{
//					result.AddMessageItem(new MessageItem("Invalid pay month selected."));
//					return BadRequest(result);
//				}
//				var pMonth = await _payMonthService.CheckPayMonthIsOpen(model.Date.Value.Year, model.Date.Value.Month);
//				if (!pMonth)
//				{
//					result.AddMessageItem(new MessageItem("Selected pay month is closed."));
//					return BadRequest(result);
//				}
//				var bonus = new Services.Import.IncomeTaxLimitService();
//				if (bonus.ValidateHeaders(model.FormFile.OpenReadStream()))
//				{
//					IEnumerable<ImportRef.IncomeTaxLimitModel> list = bonus.ToModel(model.FormFile.OpenReadStream());
//					IEnumerable<Employee> employees = (await _employeeService.GetList("Name")).ToList();
//					var incomeTaxLimit = new List<IncomeTaxLimit>();
//					var errors = new List<ImportRef.IncomeTaxLimitModel>();

//					foreach (ImportRef.IncomeTaxLimitModel item in list)
//					{
//						Employee e = employees.FirstOrDefault(x => x.No.Equals(item.EmployeeId, StringComparison.OrdinalIgnoreCase));
//						if (e == null)
//						{
//							item.Error = "Invalid employee code";
//							errors.Add(item);
//							continue;
//						}
//						if (int.TryParse(item.Amount.ToString(), out int amount) && amount < 0)
//						{
//							item.Error = "Amount should not be negative value";
//							errors.Add(item);
//							continue;
//						}
//						incomeTaxLimit.Add(new IncomeTaxLimit
//						{
//							ID = Guid.NewGuid(),
//							EmployeeId = e.ID,
//							Month = payMonth.Month,
//							Year = payMonth.Year,
//							Amount = item.Amount
//						});
//					}
//					if (errors.Count > 0)
//					{
//						result.AddMessageItem(new MessageItem("Invalid data found in sheet. Please correct the data and re-upload."));
//						result.ReturnValue = errors;
//						result.Headers = _mapper.Map<List<Core.HeaderModel>>(bonus.GetAllColumns());
//					}
//					else
//					{
//						var bulkAddResult = await _incomeTaxLimitService.AddBulk(incomeTaxLimit, payMonth.Month, payMonth.Year);
//						return bulkAddResult.HasError ? BadRequest(bulkAddResult) : Ok(bulkAddResult);
//					}
//					return Ok(result);
//				}
//				else
//				{
//					return BadRequest(result);
//				}
//			}
//			catch (Exception ex)
//			{
//				return BadRequest(ex.Message);
//			}
//		}

//		#endregion

//		#region Bonus

//		[HttpPost("Bonus")]
//		public async Task<IActionResult> Bonus([FromForm] ImportData model)
//		{
//			var result = new FileImportResult<List<ImportRef.BonusModel>>();
//			var bonus = new Services.Import.BonusService();
//			if (bonus.ValidateHeaders(model.FormFile.OpenReadStream()))
//			{
//				IEnumerable<ImportRef.BonusModel> list = bonus.ToModel(model.FormFile.OpenReadStream());
//				IEnumerable<Employee> employees = (await _employeeService.GetList("Name")).ToList();
//				var empBonus = new List<EmpBonus>();
//				var errors = new List<ImportRef.BonusModel>();
//				foreach (ImportRef.BonusModel item in list)
//				{
//					Employee e = employees.FirstOrDefault(x => x.No.Equals(item.EmployeeCode, StringComparison.OrdinalIgnoreCase));
//					if (e == null)
//					{
//						item.Error = "Invalid employee code";
//						errors.Add(item);
//						continue;
//					}
//					if (int.TryParse(item.Bonus.ToString(), out int Bonus) && Bonus < 0)
//					{
//						item.Error = "Bonus should not be negative value";
//						errors.Add(item);
//						continue;
//					}
//					empBonus.Add(new EmpBonus
//					{
//						ID = Guid.NewGuid(),
//						EmployeeId = e.ID,
//						ReleasedOn = item.ReleasedOn,
//						Amount = item.Bonus
//					});

//				}
//				if (errors.Count > 0)
//				{
//					result.AddMessageItem(new MessageItem("Invalid data found in sheet. Please correct the data and re-upload."));
//					result.ReturnValue = errors;
//					result.Headers = _mapper.Map<List<Core.HeaderModel>>(bonus.GetAllColumns());
//				}
//				else
//				{
//					var bulkAddResult = await _bonusService.AddBulk(empBonus);
//					if (bulkAddResult.HasNoError)
//					{
//						var headersList = new List<Core.HeaderModel>
//						{
//							new Core.HeaderModel{ Name = "Total Employees", Order = 1, PropertyName ="employeeCode"},
//							new Core.HeaderModel{ Name = "Bonus", Order = 1, PropertyName ="bonus"}
//						};
//						result.Headers = headersList;
//						result.ReturnValue = new List<BonusModel>
//						{
//							new BonusModel
//							{
//								EmployeeCode = bulkAddResult.ReturnValue["Employees"].ToString(),
//								Bonus = bulkAddResult.ReturnValue["Bonus"]
//							}
//						};
//					}
//					return bulkAddResult.HasError ? BadRequest(bulkAddResult) : Ok(result);
//				}
//				return Ok(result);
//			}
//			else
//			{
//				return BadRequest(result);
//			}

//		}
//		#endregion

//		#region Leave Balance

//		[HttpPost("Leavebalance")]
//		[ApiAuthorize(Core.Permission.LM_Import, Core.Privilege.Create)]

//		public async Task<IActionResult> LeaveBalance([FromForm] ImportData model)
//		{
//			var result = new FileImportResult<List<ImportRef.LeaveBalance>>();
//			var leaves = new Services.Import.LeaveBalanceService();
//			if (leaves.ValidateHeaders(model.FormFile.OpenReadStream()))
//			{
//				IEnumerable<ImportRef.LeaveBalance> list = leaves.ToModel(model.FormFile.OpenReadStream());
//				IEnumerable<LeaveType> leaveTypes = await _leaveTypeService.GetList("Name");
//				IEnumerable<Employee> employees = (await _employeeService.GetList("Name")).ToList();
//				var balances = new List<AdjustLeave>();
//				var errors = new List<ImportRef.LeaveBalance>();
//				foreach (ImportRef.LeaveBalance item in list)
//				{
//					Employee employee = employees.FirstOrDefault(x => x.No.Equals(item.EmployeeCode, StringComparison.OrdinalIgnoreCase));
//					LeaveType leaveType = leaveTypes.FirstOrDefault(l => l.Code.Equals(item.LeaveType, StringComparison.OrdinalIgnoreCase));
//					if (employee == null)
//					{
//						item.Error = "Invalid employee code";
//						errors.Add(item);
//					}
//					else if (leaveType == null)
//					{
//						item.Error = "Invalid leave type";
//						errors.Add(item);
//					}
//					else
//					{
//						balances.Add(new AdjustLeave
//						{
//							EmployeeId = employee.ID,
//							ID = Guid.NewGuid(),
//							NewBalance = item.Leaves,
//							Reason = item.Reason,
//							LeaveTypeId = leaveType.ID,
//							EffectiveFrom = item.EffectiveFrom,
//							EffectiveTo = item.EffectiveTo,
//						});
//					}
//				}
//				if (errors.Count > 0)
//				{
//					result.AddMessageItem(new MessageItem("Invalid data found in sheet. Please correct the data and re-upload."));
//					result.ReturnValue = errors;
//					result.Headers = _mapper.Map<List<Core.HeaderModel>>(leaves.GetAllColumns());
//				}
//				else
//				{
//					var bulkAddResult = await _leaveService.AddBulk(balances);
//					return bulkAddResult.HasError ? BadRequest(bulkAddResult) : Ok(bulkAddResult);
//				}
//				return Ok(result);
//			}
//			else
//			{
//				return BadRequest(result);
//			}
//		}
//		#endregion

//		#region Attendance

//		[HttpPost("AttendanceLogsImport")]

//		public async Task<IActionResult> Attendance([FromForm] ImportData model)
//		{
//			var result = new FileImportResult<List<ImportRef.AttendanceLogsImportModel>>();

//			var LogsList = new List<TranSmart.Domain.Entities.Leave.BiometricAttLogs>();

//			var attendance = new Services.Import.AttendanceService();

//			if (attendance.ValidateHeaders(model.FormFile.OpenReadStream()))
//			{
//				IEnumerable<TranSmart.API.Models.Import.AttendanceLogsImportModel> list = attendance.ToModel(model.FormFile.OpenReadStream());

//				var errors = new List<ImportRef.AttendanceLogsImportModel>();
//				foreach (TranSmart.API.Models.Import.AttendanceLogsImportModel item in list)//Attendance list from excel
//				{
//					var movementInLog = new BiometricAttLogs
//					{
//						EmpCode = item.EmployeeCode.ToUpper().Replace("AVONTIX", ""),
//						MovementTime = item.InTime,
//						MovementType = 0
//					};
//					LogsList.Add(movementInLog);

//					var movementOutLog = new TranSmart.Domain.Entities.Leave.BiometricAttLogs
//					{
//						EmpCode = item.EmployeeCode.ToUpper().Replace("AVONTIX", ""),
//						MovementTime = item.OutTime,
//						MovementType = 1
//					};
//					LogsList.Add(movementOutLog);
//				}
//				await _attendanceService.BiometricLogsImport(LogsList);//Passing to service


//				if (errors.Count > 0)
//				{
//					result.AddMessageItem(new MessageItem("Invalid data found in sheet. Please correct the data and re-upload."));
//					result.ReturnValue = errors;
//					result.Headers = _mapper.Map<List<Core.HeaderModel>>(attendance.GetAllColumns());
//					return Ok(result);
//				}
//				return Ok(result);
//			}
//			else
//			{
//				result.AddMessageItem(new MessageItem("Invalid headers in sheet."));
//				return BadRequest(result);
//			}
//		}


//		#endregion

//		#region Salaries

//		[HttpPost("Salaries")]
//		[ApiAuthorize(Core.Permission.PS_EmployeeSalary, Core.Privilege.Update)]
//		public async Task<IActionResult> Salaries([FromForm] ImportData model)
//		{
//			var employees = await _employeeService.GetList("No");
//			var components = await _earningComponentService.GetList("DisplayOrder");
//			var deductions = await _deductionComponentService.GetList("DisplayOrder");
//			var salService = new Services.Import.SalaryImportService();
//			var invalidEmployees = new Dictionary<string, string>();
//			Dictionary<int, Dictionary<string, string>> _list = salService.ToDictionary(model.FormFile.OpenReadStream());
//			var result = new Result<string>();

//			//No data in sheet then return no content HTTP status
//			if (_list.Count == 0) { return NoContent(); }

//			//Verify headers and return bad request when headers are not a valid names
//			foreach (var headerName in _list.FirstOrDefault().Value.Select(x => x.Key))
//			{
//				if (!(string.Compare(headerName.Trim(), "Employee Code", true) == 0
//					|| string.Compare(headerName.Trim(), "Salary Per Month", true) == 0) && headerName.Trim().Length > 0)
//				{
//					EarningComponent component = components.FirstOrDefault(x => x.Name.Trim().Equals(headerName.Trim(), StringComparison.OrdinalIgnoreCase));
//					DeductionComponent dedComponent = deductions.FirstOrDefault(x => x.Name.Trim().Equals(headerName.Trim(), StringComparison.OrdinalIgnoreCase));
//					if (component == null && dedComponent == null)
//					{
//						result.AddMessageItem(new MessageItem("Invalid component header name " + headerName));
//						break;
//					}
//				}
//			}
//			if (result.HasError)
//			{
//				return BadRequest(result);
//			}
//			var salaries = new List<Salary>();
//			foreach (KeyValuePair<int, Dictionary<string, string>> items in _list)
//			{
//				string employeeCode = string.Empty;
//				var salary = new Salary()
//				{
//					Earnings = new List<SalaryEarning>(),
//					Deductions = new List<SalaryDeduction>()
//				};
//				foreach (KeyValuePair<string, string> item in items.Value.Where(x => x.Value.Length > 0))
//				{

//					switch (item.Key)
//					{
//						case "Employee Code":
//							var employee = employees.FirstOrDefault(x => x.No.Equals(item.Value, StringComparison.OrdinalIgnoreCase));
//							if (employee == null)
//							{
//								employeeCode = item.Value;
//								invalidEmployees.CheckAndAdd(item.Value, $"Invalid {item.Key}");
//								//ignore employee and move to next row
//								continue;
//							}
//							else
//							{
//								employeeCode = item.Value;
//								salary.EmployeeId = employee.ID;
//							}
//							break;
//						case "Salary Per Month":
//							if (decimal.TryParse(item.Value, out decimal monthly) && monthly > 0)
//							{
//								salary.Monthly = (int)Math.Round(monthly, 0);
//								salary.Annually = salary.Monthly * 12;
//							}
//							else
//							{
//								//monthly salary is failed to convert
//								//ignore employee and move to next row  
//								invalidEmployees.CheckAndAdd(employeeCode, $"Invalid value in {item.Key} column");
//								//ignore employee and move to next row
//								continue;
//							}
//							break;
//						default:
//							EarningComponent component = components.FirstOrDefault(x => x.Name.Equals(item.Key, StringComparison.OrdinalIgnoreCase));
//							if (component != null)
//							{
//								if (decimal.TryParse(item.Value, out decimal monthlyAmt))
//								{
//									salary.Earnings.Add(new SalaryEarning
//									{
//										ComponentId = component.ID,
//										Monthly = (int)Math.Round(monthlyAmt, 0),
//										Annually = (int)Math.Round(monthlyAmt, 0) * 12,
//										FromTemplate = false,
//									});
//								}
//								else
//								{
//									invalidEmployees.CheckAndAdd(employeeCode, $"Invalid value in {item.Key} column");
//									//ignore employee and move to next row
//									continue;
//								}
//							}

//							DeductionComponent dedComponent = deductions.FirstOrDefault(x => x.Name.Equals(item.Key, StringComparison.OrdinalIgnoreCase));
//							if (dedComponent != null)
//							{
//								if (decimal.TryParse(item.Value, out decimal monthlyAmt))
//								{
//									salary.Deductions.Add(new SalaryDeduction
//									{
//										DeductionId = dedComponent.ID,
//										Monthly = (int)Math.Round(monthlyAmt, 0),
//									});
//								}
//								else
//								{
//									invalidEmployees.CheckAndAdd(employeeCode, $"Invalid value in {item.Key} column");
//									//ignore employee and move to next row
//									continue;
//								}
//							}
//							break;
//					}
//				}
//				salary.CTC = 0;
//				//Ignore when employee is not found
//				if (salary.EmployeeId != Guid.Empty)
//				{
//					salaries.Add(salary);
//				}
//			}
//			if (invalidEmployees.Any())
//			{
//				result.AddMessageItem(new MessageItem("Invalid data found in sheet. Please correct the data and re-upload."));
//				var resulrReturn = new List<Dictionary<string, string>>();
//				foreach (var (items, d) in from items in _list
//										   let d = new Dictionary<string, string>()
//										   select (items, d))
//				{
//					foreach (KeyValuePair<string, string> item in items.Value.Where(x => x.Value.Length > 0))
//					{
//						d.Add(StringUtil.ToCamelCase(item.Key.Replace(" ", "")), item.Value);
//					}

//					var invalidEmps = invalidEmployees.FirstOrDefault(k => k.Key.Equals(items.Value["Employee Code"])).Value;
//					var invalidSalarys = invalidEmployees.FirstOrDefault(k => k.Key.Equals(items.Value["Salary Per Month"])).Value;
//					if (invalidEmps != null)
//					{
//						d.Add("error", invalidEmps);
//						resulrReturn.Add(d);
//					}
//					if (invalidSalarys != null)
//					{
//						d.Add("error", invalidSalarys);
//						resulrReturn.Add(d);
//					}
//				}
//				var headers = new List<Core.HeaderModel>();
//				foreach (var (items, d) in from items in _list
//										   let d = new Dictionary<string, string>()
//										   select (items, d))
//				{
//					headers.AddRange(from KeyValuePair<string, string> item in items.Value
//									 select new Core.HeaderModel
//									 {
//										 PropertyName = item.Key.Replace(" ", ""),
//										 Name = item.Key,
//										 Order = headers.Count
//									 });
//					break;
//				}
//				headers.Add(new Core.HeaderModel { Name = "Error", PropertyName = "error", Order = headers.Count });
//				var result1 = new FileImportResult<List<Dictionary<string, string>>>();
//				result1.AddMessageItem(new MessageItem("Invalid data found in sheet. Please correct the data and re-upload."));
//				result1.Headers = headers;
//				result1.ReturnValue = resulrReturn;
//				return Ok(result1);
//			}
//			var addResult = await _salService.AddBulk(salaries);
//			return addResult.HasError ? BadRequest(addResult) : Ok(addResult);
//		}


//		#endregion

//		#region EPF ESI

//		[HttpPost("EmpStatutory")]
//		public async Task<IActionResult> EmpStatutory([FromForm] ImportData model)
//		{
//			var empStatuoryservice = new Services.Import.EmpStatutoryImportService();
//			var invalidEmployees = new Dictionary<string, string>();
//			var empStatutoryList = new List<EmpStatutory>();
//			var result = new Result<string>();

//			List<Employee> _empList = (await _employeeService.GetList("No")).ToList();
//			Dictionary<int, Dictionary<string, string>> _list = empStatuoryservice.ToDictionary(model.FormFile.OpenReadStream());
//			var resulrReturn = new List<Dictionary<string, string>>();
//			foreach (KeyValuePair<int, Dictionary<string, string>> items in _list)
//			{
//				var statutory = new EmpStatutory();
//				foreach (KeyValuePair<string, string> item in items.Value)
//				{
//					switch (item.Key)
//					{
//						case "Employee Code":
//							Employee e = _empList.FirstOrDefault(e => e.No.Equals(item.Value, StringComparison.OrdinalIgnoreCase));
//							if (e == null)
//							{
//								invalidEmployees.CheckAndAdd(item.Value, $"Invalid {item.Key}");
//							}
//							else
//							{
//								statutory.EmpId = e.ID;
//							}
//							break;
//						case "Enable PF":
//							var validYes = item.Value.Equals("yes", StringComparison.InvariantCultureIgnoreCase);
//							var validNo = item.Value.Equals("no", StringComparison.InvariantCultureIgnoreCase);
//							if (!validYes && !validNo)
//							{
//								invalidEmployees.CheckAndAdd(item.Value, $"Invalid {item.Key}");
//							}
//							else
//							{
//								statutory.EnablePF = item.Value.Equals("yes", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
//								statutory.EmployeeContrib = 3;
//							}
//							break;
//						case "Enable ESI":
//							validYes = item.Value.Equals("yes", StringComparison.InvariantCultureIgnoreCase);
//							validNo = item.Value.Equals("no", StringComparison.InvariantCultureIgnoreCase);
//							if (!validYes && !validNo)
//							{
//								invalidEmployees.CheckAndAdd(item.Value, $"Invalid {item.Key}");
//							}
//							else
//							{
//								statutory.EnableESI = item.Value.Equals("yes", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
//							}
//							break;
//						case "EPF Acc No":
//							statutory.EmployeesProvid = item.Value;
//							break;
//						case "ESI Acc No":
//							statutory.ESINo = item.Value;
//							break;
//						case "UAN":
//							statutory.UAN = item.Value;
//							break;
//						default:
//							break;
//					}

//				}

//				//Ignore when employee is not found
//				if (statutory.EmpId != Guid.Empty)
//				{
//					empStatutoryList.Add(statutory);
//				}
//			}
//			if (invalidEmployees.Any())
//			{
//				foreach (var (items, d) in from items in _list
//										   let d = new Dictionary<string, string>()
//										   select (items, d))
//				{
//					foreach (KeyValuePair<string, string> item in items.Value.Where(x => x.Value.Length > 0))
//					{
//						d.Add(StringUtil.ToCamelCase(item.Key.Replace(" ", "")), item.Value);
//					}
//					var invalidEmps = invalidEmployees.FirstOrDefault(k => k.Key.Equals(items.Value["Employee Code"])).Value;
//					var invalidEPF = invalidEmployees.FirstOrDefault(k => k.Key.Equals(items.Value["Enable PF"])).Value;
//					var invalidESI = invalidEmployees.FirstOrDefault(k => k.Key.Equals(items.Value["Enable ESI"])).Value;
//					if (invalidEmps != null)
//					{
//						d.Add("error", invalidEmps);
//						resulrReturn.Add(d);
//					}
//					else if (invalidEPF != null)
//					{
//						d.Add("error", invalidEPF);
//						resulrReturn.Add(d);
//					}
//					else if (invalidESI != null)
//					{
//						d.Add("error", invalidESI);
//						resulrReturn.Add(d);
//					}
//				}
//				var headers = new List<Core.HeaderModel>();
//				foreach (var (items, d) in from items in _list
//										   let d = new Dictionary<string, string>()
//										   select (items, d))
//				{
//					headers.AddRange(from KeyValuePair<string, string> item in items.Value
//									 select new Core.HeaderModel
//									 {
//										 PropertyName = item.Key.Replace(" ", ""),
//										 Name = item.Key,
//										 Order = headers.Count
//									 });
//					break;
//				}
//				headers.Add(new Core.HeaderModel { Name = "Error", PropertyName = "error", Order = headers.Count });
//				var result1 = new FileImportResult<List<Dictionary<string, string>>>();
//				result1.AddMessageItem(new MessageItem("Invalid data found in sheet. Please correct the data and re-upload."));
//				result1.Headers = headers;
//				result1.ReturnValue = resulrReturn;
//				return Ok(result1);
//			}
//			var addResult = _empStatutoryService.AddBulk(empStatutoryList).GetAwaiter().GetResult();

//			return addResult.HasError ? BadRequest(addResult) : Ok(addResult);

//		}
//		#endregion

//		#region PaySheet
//		[HttpPost("PaySheet")]
//		public async Task<IActionResult> PaySheet([FromForm] ImportData model)
//		{
//			var result = new Result<string>();
//			//var pMonth = await _payMonthService.CheckPayMonthIsOpen(model.Date.Value.Year, model.Date.Value.Month);
//			//if (!pMonth)
//			//{
//			//	result.AddMessageItem(new MessageItem("Selected month pay roll is closed."));
//			//	return BadRequest(result);
//			//}
//			var invalidEmployees = new Dictionary<string, string>();
//			List<Employee> _empList = (await _employeeService.GetList("No")).ToList();
//			var earningComponents = _earningComponentService.GetList("DisplayOrder").GetAwaiter().GetResult().ToList();
//			var deductionComponents = _deductionComponentService.GetList("DisplayOrder").GetAwaiter().GetResult().ToList();

//			var payMonth = await _payMonthService.GetPayMonthDetails(model.Date.Value.Year, model.Date.Value.Month);

//			if (payMonth == null)
//			{
//				result.AddMessageItem(new MessageItem("Invalid month pay selected."));
//				return BadRequest(result);
//			}
//			var paySheetImportService = new Services.Import.PaysheetImportService();
//			Dictionary<int, Dictionary<string, string>> _list = paySheetImportService.ToDictionary(model.FormFile.OpenReadStream());
//			var paySheets = new List<PaySheet>();

//			foreach (KeyValuePair<int, Dictionary<string, string>> items in _list)
//			{
//				var paySheet = new PaySheet
//				{
//					PayMonthId = payMonth.ID
//				};
//				var sheetEarning = new List<PaySheetEarning>();
//				var sheetDeductions = new List<PaySheetDeduction>();
//				string employeeCode = string.Empty;
//				foreach (KeyValuePair<string, string> item in items.Value)
//				{
//					switch (item.Key.Trim())
//					{
//						case "Employee Code":
//							Employee employee = _empList.FirstOrDefault(x => x.No.Equals(item.Value, StringComparison.OrdinalIgnoreCase));
//							if (employee == null)
//							{
//								employeeCode = item.Value;
//								invalidEmployees.CheckAndAdd(item.Value, $"Invalid {item.Key}");
//								//ignore employee and move to next row
//								continue;
//							}
//							else
//							{
//								employeeCode = item.Value;
//								paySheet.EmployeeID = employee.ID;
//								paySheet.WorkDays = payMonth.Days;
//							}
//							break;
//						case "Work Days":
//							if (string.IsNullOrWhiteSpace(item.Value) || Convert.ToDecimal(item.Value) < 0)
//							{
//								invalidEmployees.CheckAndAdd(employeeCode, $"Invalid {item.Key} column");
//								continue;
//							}
//							else
//							{
//								paySheet.WorkDays = Convert.ToDecimal(item.Value);
//							}
//							break;
//						case "Present Days":
//							if (string.IsNullOrWhiteSpace(item.Value) || Convert.ToDecimal(item.Value) < 0)
//							{
//								invalidEmployees.CheckAndAdd(employeeCode, $"Invalid {item.Key} column");
//								continue;
//							}
//							else
//							{
//								paySheet.PresentDays = Convert.ToDecimal(item.Value);
//							}
//							break;
//						case "LOP Days":
//							decimal.TryParse(item.Value, out decimal lopDays);
//							if (lopDays < 0)
//							{
//								invalidEmployees.CheckAndAdd(employeeCode, $"Invalid {item.Key} column");
//								continue;
//							}
//							paySheet.LOPDays = lopDays;

//							break;
//						case "UA Days":
//							decimal.TryParse(item.Value, out decimal uaDays);
//							if (uaDays < 0)
//							{
//								invalidEmployees.CheckAndAdd(employeeCode, $"Invalid {item.Key} column");
//								continue;
//							}
//							paySheet.UADays = uaDays;

//							break;
//						case "Salary":
//							if (string.IsNullOrWhiteSpace(item.Value) || Convert.ToInt32(item.Value) < 0)
//							{
//								invalidEmployees.CheckAndAdd(employeeCode, $"Invalid {item.Key} column");
//								continue;
//							}
//							else
//							{
//								paySheet.Salary = Convert.ToInt32(item.Value);
//							}
//							break;
//						case "Gross":
//							int.TryParse(item.Value, out int gross);
//							if (gross < 0)
//							{
//								invalidEmployees.CheckAndAdd(employeeCode, $"Invalid {item.Key} column");
//								continue;
//							}
//							paySheet.Gross = gross;
//							break;
//						case "Deduction":
//							int.TryParse(item.Value, out int deduct);
//							if (deduct < 0)
//							{
//								invalidEmployees.CheckAndAdd(employeeCode, $"Invalid {item.Key} column");
//								continue;
//							}
//							paySheet.Deduction = deduct;
//							break;
//						case "Net":
//							if (string.IsNullOrWhiteSpace(item.Value) || Convert.ToInt32(item.Value) < 0)
//							{
//								invalidEmployees.CheckAndAdd(employeeCode, $"Invalid {item.Key} column");
//								continue;
//							}
//							else
//							{
//								paySheet.Net = Convert.ToInt32(item.Value);
//							}
//							break;
//						case "Incentive":
//							int.TryParse(item.Value, out int incentive);
//							if (incentive < 0)
//							{
//								invalidEmployees.CheckAndAdd(employeeCode, $"Invalid {item.Key} column");
//								continue;
//							}
//							paySheet.Incentive = incentive;
//							break;
//						case "Arrears":
//							int.TryParse(item.Value, out int arrears);
//							if (arrears < 0)
//							{
//								invalidEmployees.CheckAndAdd(employeeCode, $"Invalid {item.Key} column");
//								continue;
//							}
//							paySheet.Arrears = arrears;
//							break;
//						case "LOP":
//							int.TryParse(item.Value, out int lop);
//							if (lop < 0)
//							{
//								invalidEmployees.CheckAndAdd(employeeCode, $"Invalid {item.Key} column");
//								continue;
//							}
//							paySheet.LOP = lop;
//							break;
//						case "PayCut":
//							int.TryParse(item.Value, out int payCut);
//							if (payCut < 0)
//							{
//								invalidEmployees.CheckAndAdd(employeeCode, $"Invalid {item.Key} column");
//								continue;
//							}
//							paySheet.PayCut = payCut;
//							break;
//						case "EPF":
//							int.TryParse(item.Value, out int epf);
//							if (epf < 0)
//							{
//								invalidEmployees.CheckAndAdd(employeeCode, $"Invalid {item.Key} column");
//								continue;
//							}
//							paySheet.EPF = epf;
//							break;
//						case "ESI":
//							int.TryParse(item.Value, out int esi);
//							if (esi < 0)
//							{
//								invalidEmployees.CheckAndAdd(employeeCode, $"Invalid {item.Key} column");
//								continue;
//							}
//							paySheet.ESI = esi;
//							break;
//						case "ESI Applied":
//							paySheet.ESIApplied = item.Value == "1"
//								|| item.Value.Equals("Yes", StringComparison.OrdinalIgnoreCase)
//								|| item.Value.Equals("Y", StringComparison.OrdinalIgnoreCase);
//							break;
//						case "PTax":
//							int.TryParse(item.Value, out int pTax);
//							if (pTax < 0)
//							{
//								invalidEmployees.CheckAndAdd(employeeCode, $"Invalid {item.Key} column");
//								continue;
//							}
//							paySheet.PTax = pTax;
//							break;
//						case "Tax":
//							int.TryParse(item.Value, out int tax);
//							if (tax < 0)
//							{
//								invalidEmployees.CheckAndAdd(employeeCode, $"Invalid {item.Key} column");
//								continue;
//							}
//							paySheet.Tax = tax;
//							break;
//						case "Hold":
//							paySheet.Hold = item.Value == "1"
//								|| item.Value.Equals("Yes", StringComparison.OrdinalIgnoreCase)
//								|| item.Value.Equals("Y", StringComparison.OrdinalIgnoreCase);
//							break;
//						case "EPF No":
//							paySheet.EPFNo = item.Value;
//							break;
//						case "ESI No":
//							paySheet.ESINo = item.Value;
//							break;
//						case "Bank Name":
//							paySheet.BankName = item.Value;
//							break;
//						case "Bank IFSC":
//							paySheet.BankIFSC = item.Value;
//							break;
//						case "Bank AcNo":
//							paySheet.BankACNo = item.Value;
//							break;
//						case "Loan":
//							int.TryParse(item.Value, out int loan);
//							if (loan < 0)
//							{
//								invalidEmployees.CheckAndAdd(employeeCode, $"Invalid {item.Key} column");
//								continue;
//							}
//							paySheet.Loan = loan;
//							break;
//						default:
//							string[] name = Array.Empty<string>();
//							if (item.Key.StartsWith("Salary "))
//							{
//								name = item.Key.Split("Salary ");
//							}
//							else if (item.Key.StartsWith("Earning "))
//							{
//								name = item.Key.Split("Earning ");
//							}
//							else if (item.Key.StartsWith("Deduction "))
//							{
//								name = item.Key.Split("Deduction ");
//							}
//							EarningComponent earning = earningComponents.FirstOrDefault(x => x.Name.Equals(name.Length == 2 ? name[1].Trim() : "", StringComparison.OrdinalIgnoreCase));
//							DeductionComponent deduction = deductionComponents.FirstOrDefault(x => x.Name.Equals(name.Length == 2 ? name[1].Trim() : "", StringComparison.OrdinalIgnoreCase));

//							if (earning != null)
//							{
//								if (decimal.TryParse(item.Value, out decimal earnings) && earnings >= 0)
//								{
//									var earningItem = sheetEarning.FirstOrDefault(x => x.ComponentId == earning.ID);
//									if (earningItem == null)
//									{
//										earningItem = new PaySheetEarning
//										{
//											ComponentId = earning.ID,
//											HeaderName = earning.Name,
//											EarningType = earning.EarningType
//										};
//										sheetEarning.Add(earningItem);
//									}
//									earningItem.Salary = item.Key.StartsWith("Salary ") ? (int)earnings : earningItem.Salary;
//									earningItem.Earning = item.Key.StartsWith("Earning ") ? (int)earnings : earningItem.Earning;

//									//Earning component is dual case (applicable for deduction also)
//									//then add record for deduction component also.
//									DeductionComponent deductionDual = deductionComponents.FirstOrDefault(x => x.EarningId == earning.ID);
//									if (deductionDual != null)
//									{
//										var deductionItem = sheetDeductions.FirstOrDefault(x => x.ComponentId == deductionDual.ID);
//										if (deductionItem == null)
//										{
//											deductionItem = new PaySheetDeduction
//											{
//												ComponentId = deductionDual.ID,
//												HeaderName = deductionDual.Name,
//												DeductType = deductionDual.Deduct,
//											};
//											sheetDeductions.Add(deductionItem);
//										}
//										deductionItem.Salary = item.Key.StartsWith("Salary ") ? (int)earnings : deductionItem.Salary;
//										deductionItem.Deduction = item.Key.StartsWith("Earning ") ? (int)earnings : deductionItem.Deduction;
//									}
//								}
//								else
//								{
//									invalidEmployees.CheckAndAdd(employeeCode, $"Invalid value in {item.Key} column");
//									continue;
//								}
//							}
//							else if (deduction != null)
//							{
//								if (int.TryParse(item.Value, out int deductions) && deductions >= 0)
//								{
//									var sheetDeductionItem = sheetDeductions.FirstOrDefault(x => x.ComponentId == deduction.ID);

//									if (sheetDeductionItem == null)
//									{
//										sheetDeductionItem = new PaySheetDeduction
//										{
//											ComponentId = deduction.ID,
//											HeaderName = deduction.Name,
//											DeductType = deduction.Deduct,
//										};
//										sheetDeductions.Add(sheetDeductionItem);
//									}
//									sheetDeductionItem.Salary = item.Key.StartsWith("Salary ") ? deductions : sheetDeductionItem.Salary;
//									sheetDeductionItem.Deduction = item.Key.StartsWith("Deduction ") ? deductions : sheetDeductionItem.Deduction;
//								}
//								else
//								{
//									invalidEmployees.CheckAndAdd(employeeCode, $"Invalid value in {item.Key} column");
//									continue;
//								}
//							}
//							else
//							{
//								invalidEmployees.CheckAndAdd(employeeCode, $"Invalid header name {item.Key}");
//								continue;
//							}
//							break;

//					}

//				}
//				if (paySheet.EmployeeID != Guid.Empty)
//				{
//					paySheet.Deductions = sheetDeductions;
//					paySheet.Earnings = sheetEarning;
//					paySheets.Add(paySheet);
//				}
//			}
//			if (invalidEmployees.Any())
//			{
//				result.AddMessageItem(new MessageItem("Invalid data found in sheet. Please correct the data and re-upload."));
//				var resultReturn = new List<Dictionary<string, string>>();
//				foreach (var (payItems, d) in from payItems in _list
//											  let d = new Dictionary<string, string>()
//											  select (payItems, d))
//				{
//					foreach (KeyValuePair<string, string> payItem in payItems.Value.Where(x => x.Value.Length > 0))
//					{
//						d.Add(StringUtil.ToCamelCase(payItem.Key.Replace(" ", "")), payItem.Value);
//					}

//					var a = invalidEmployees.FirstOrDefault(k => k.Key.Equals(payItems.Value["Employee Code"])).Value;
//					if (a != null)
//					{
//						d.Add("error", a);
//						resultReturn.Add(d);
//					}
//				}
//				var headers = new List<Core.HeaderModel>();
//				foreach (var (item, d) in from itemss in _list
//										  let d = new Dictionary<string, string>()
//										  select (itemss, d))
//				{
//					headers.AddRange(from KeyValuePair<string, string> payItem in item.Value
//									 select new Core.HeaderModel
//									 {
//										 PropertyName = payItem.Key.Replace(" ", ""),
//										 Name = payItem.Key,
//										 Order = headers.Count
//									 });
//					break;
//				}
//				headers.Add(new Core.HeaderModel { Name = "Error", PropertyName = "error", Order = headers.Count });
//				var result1 = new FileImportResult<List<Dictionary<string, string>>>();
//				result1.AddMessageItem(new MessageItem("Invalid data found in sheet. Please correct the data and re-upload."));
//				result1.Headers = headers;
//				result1.ReturnValue = resultReturn;
//				return Ok(result1);
//			}

//			var addResult = await _payMonthService.AddUpdatePaysheet(paySheets);
//			return addResult.HasError ? BadRequest(addResult) : Ok(addResult);

//		}
//		#endregion

//		#region Incentives Pay cut

//		[HttpPost("IncentivesPayCut")]
//		public async Task<IActionResult> IncentivesPayCut([FromForm] ImportData model)
//		{
//			var result = new FileImportResult<List<ImportRef.IncentivesPayCutModel>>();
//			if (!model.Date.HasValue)
//			{
//				result.AddMessageItem(new MessageItem("Please select month and year"));
//				return BadRequest(result);
//			}
//			if (!await _payMonthService.CheckPayMonthIsOpen(model.Date.Value.Year, model.Date.Value.Month))
//			{
//				result.AddMessageItem(new MessageItem("Selected month pay roll is closed"));
//				return BadRequest(result);
//			}
//			var mTIncentives = new Services.Import.IncentivesPayCutService();
//			if (mTIncentives.ValidateHeaders(model.FormFile.OpenReadStream()))
//			{
//				IEnumerable<ImportRef.IncentivesPayCutModel> list = mTIncentives.ToModel(model.FormFile.OpenReadStream());
//				IEnumerable<Employee> employees = (await _employeeService.GetList("Name")).ToList();
//				var mTIncentivePayCut = new List<IncentivesPayCut>();
//				var errors = new List<ImportRef.IncentivesPayCutModel>();
//				foreach (ImportRef.IncentivesPayCutModel item in list)
//				{
//					Employee e = employees.FirstOrDefault(x => x.No.Equals(item.EmployeeCode, StringComparison.OrdinalIgnoreCase));
//					if (e == null)
//					{
//						item.Error = "Invalid employee code";
//						errors.Add(item);
//						continue;
//					}
//					if (item.FaxFilesAndArrears < 0 || item.OtherInc < 0 || item.TTeamInc < 0 || item.SpotInc < 0 || item.PunctualityInc < 0 || item.ProductionInc < 0
//						|| item.CentumClub < 0 || item.DoublePay < 0 || item.FirstMinuteInc < 0 || item.NightShift < 0 || item.SundayInc < 0 || item.WeeklyStarInc < 0
//						|| item.ExternalQualityFeedbackDed < 0 || item.InternalQualityFeedbackDed < 0 || item.UnauthorizedLeaveDed < 0 || item.LateComingDed < 0 || item.OtherDed < 0)
//					{
//						item.Error = "Value should not be negative";
//						errors.Add(item);
//						continue;
//					}
//					mTIncentivePayCut.Add(new IncentivesPayCut
//					{
//						ID = Guid.NewGuid(),
//						EmployeeId = e.ID,
//						Year = model.Date.Value.Year,
//						Month = model.Date.Value.Month,
//						CentumClub = item.CentumClub,
//						DoublePay = item.DoublePay,
//						ExternalQualityFeedbackDed = item.ExternalQualityFeedbackDed,
//						FaxFilesAndArrears = item.FaxFilesAndArrears,
//						FirstMinuteInc = item.FirstMinuteInc,
//						InternalQualityFeedbackDed = item.InternalQualityFeedbackDed,
//						LateComingDed = item.LateComingDed,
//						NightShift = item.NightShift,
//						OtherDed = item.OtherDed,
//						OtherInc = item.OtherInc,
//						ProductionInc = item.ProductionInc,
//						PunctualityInc = item.PunctualityInc,
//						TTeamInc = item.TTeamInc,
//						UnauthorizedLeaveDed = item.UnauthorizedLeaveDed,
//						SpotInc = item.SpotInc,
//						WeeklyStarInc = item.WeeklyStarInc,
//						SundayInc = item.SundayInc,

//					});
//				}
//				if (errors.Count > 0)
//				{
//					result.AddMessageItem(new MessageItem("Invalid data found in sheet. Please correct the data and re-upload."));
//					result.ReturnValue = errors;
//					result.Headers = _mapper.Map<List<Core.HeaderModel>>(mTIncentives.GetAllColumns());
//				}
//				else
//				{
//					var bulkAddResult = await _incentivesPayCutService.AddBulk(mTIncentivePayCut);
//					if (bulkAddResult.HasNoError)
//					{
//						var headersList = new List<Core.HeaderModel>
//						{
//							new Core.HeaderModel{Name = "Total Employees",Order = 1,PropertyName = "employeeCode"},
//							new Core.HeaderModel{Name = "Incentives",Order = 2,PropertyName = "incentives"},
//							new Core.HeaderModel{Name = "Pay Cut",Order = 3,PropertyName = "payCut"}
//						};
//						result.Headers = headersList;

//						result.ReturnValue = new List<IncentivesPayCutModel>
//						{
//							new IncentivePayCutResultHeadersModel
//							{
//								EmployeeCode = bulkAddResult.ReturnValue["Employees"].ToString(),
//								Incentives = bulkAddResult.ReturnValue["Incentives"],
//								PayCut = bulkAddResult.ReturnValue["PayCut"]
//							}
//						};
//					}
//					return bulkAddResult.HasError ? BadRequest(bulkAddResult) : Ok(result);
//				}
//				return Ok(result);
//			}
//			else
//			{
//				result.AddMessageItem(new MessageItem("Invalid headers in sheet."));
//				return BadRequest(result);
//			}
//		}
//		#endregion

//		#region Payment Info
//		[HttpPost("PaymentInfo")]
//		public async Task<IActionResult> PaymentInfo([FromForm] ImportData model)
//		{
//			var result = new FileImportResult<List<PaymentInfoModel>>();
//			var payInfoImportService = new Services.Import.PaymentInfoService();
//			//Header validation
//			if (!payInfoImportService.ValidateHeaders(model.FormFile.OpenReadStream()))
//			{
//				return BadRequest();
//			}
//			IEnumerable<PaymentInfoModel> paymentInfoList = payInfoImportService.ToModel(model.FormFile.OpenReadStream());
//			IEnumerable<Employee> _empList = await _employeeService.GetList("No");
//			IEnumerable<Bank> bankList = await _bankService.GetList("Name");

//			var errors = new List<PaymentInfoModel>();
//			var empPayInfo = new List<EmployeePayInfo>();
//			Regex regexAccountNo = new("(^[0-9]*$)");
//			Regex regexIFSCCode = new("(^[A-Z]{4}0[A-Z0-9]{6}$)");

//			foreach (PaymentInfoModel item in paymentInfoList)
//			{
//				Employee employee = _empList.FirstOrDefault(x => x.No.Equals(item.EmpCode, StringComparison.OrdinalIgnoreCase));
//				Bank bank = bankList.FirstOrDefault(x => x.Name.Equals(item.Bank, StringComparison.OrdinalIgnoreCase));
//				if (bank == null)
//				{
//					item.Error = "Invalid Employer Bank.";
//					continue;
//				}
//				var payInfo = new EmployeePayInfo
//				{
//					BankId = bank.ID
//				};

//				if (employee == null)
//				{
//					item.Error = "Invalid employee code";
//				}
//				else
//				{
//					payInfo.EmployeeId = employee.ID;
//				}
//				var stringBuilder = new StringBuilder();
//				if (item.PayMode.Equals("Bank Transfer", StringComparison.OrdinalIgnoreCase)
//							   || item.PayMode.Equals("BankTransfer", StringComparison.OrdinalIgnoreCase))
//				{
//					payInfo.PayMode = 1;
//					if (string.IsNullOrEmpty(item.AccountNo))
//					{
//						stringBuilder.Append("Account no is required. ");
//					}
//					if (!regexAccountNo.IsMatch(item.AccountNo))
//					{
//						stringBuilder.Append("Account number should digits only. ");
//					}
//					else if (bank.BankNoLength != item.AccountNo.Length)
//					{
//						stringBuilder.Append("Account number should be same as bank account no of digits(" + bank.BankNoLength + "). ");
//					}
//					if (stringBuilder.Length == 0)
//					{
//						payInfo.AccountNo = item.AccountNo;
//					}
//					else { item.Error = stringBuilder.ToString(); }
//				}

//				else if (item.PayMode.Equals("Online", StringComparison.OrdinalIgnoreCase))
//				{
//					payInfo.PayMode = 2;
//					if (string.IsNullOrEmpty(item.BankName))
//					{
//						stringBuilder.Append("Bank Name is required. ");
//					}
//					if (string.IsNullOrEmpty(item.AccountNo))
//					{
//						stringBuilder.Append("Account number is required. ");
//					}
//					if (!regexAccountNo.IsMatch(item.AccountNo))
//					{
//						stringBuilder.Append("Account number should digits only. ");
//					}
//					if (string.IsNullOrEmpty(item.IFSCCode))
//					{
//						stringBuilder.Append("IFSC Code is required. ");
//					}
//					if (!regexIFSCCode.IsMatch(item.IFSCCode))
//					{
//						stringBuilder.Append("Invalid IFSC Code. ");
//					}
//					if (stringBuilder.Length == 0)
//					{
//						payInfo.AccountNo = item.AccountNo;
//						payInfo.BankName = item.BankName;
//						payInfo.IFSCCode = item.IFSCCode;
//					}
//					else { item.Error = stringBuilder.ToString(); }
//				}
//				else
//				{
//					item.Error = "Invalid pay mode";
//				}

//				if (string.IsNullOrEmpty(item.Error))
//				{
//					empPayInfo.Add(payInfo);
//				}
//				else
//				{
//					errors.Add(item);
//				}
//			}

//			if (errors.Any(x => x.Error.Length > 0))
//			{
//				result.AddMessageItem(new MessageItem("Invalid data found in sheet. Please correct the data and re-upload."));
//				result.ReturnValue = errors;
//				result.Headers = _mapper.Map<List<Core.HeaderModel>>(payInfoImportService.GetAllColumns());
//				return Ok(result);
//			}

//			var addResult = await _employeePayInfoService.AddPaymentInfo(empPayInfo);
//			return addResult.HasError ? BadRequest(addResult) : Ok(addResult);
//		}
//		#endregion

//		#region Attendance Status
//		[HttpPost("AttendanceUpdate")]
//		public async Task<IActionResult> AttendanceStatus([FromForm] ImportData model)
//		{
//			var result = new FileImportResult<List<ImportRef.AttendanceStatusModel>>();
//			var attendanceStatusService = new Services.Import.AttendanceStatusService();
//			//Header validation
//			if (!attendanceStatusService.ValidateHeaders(model.FormFile.OpenReadStream()))
//			{
//				return BadRequest("Invalid Headers");
//			}
//			IEnumerable<ImportRef.AttendanceStatusModel> attendanceStatusList = attendanceStatusService.ToModel(model.FormFile.OpenReadStream());
//			IEnumerable<Employee> _empList = await _employeeService.GetList("No");
//			IEnumerable<LeaveType> leaveList = await _leaveTypeService.GetList("Name");
//			var attDetailsList = new List<TranSmart.Domain.Models.LM_Attendance.List.AttendanceDetails>();
//			var errors = new List<ImportRef.AttendanceStatusModel>();

//			foreach (ImportRef.AttendanceStatusModel item in attendanceStatusList)
//			{
//				Employee employee = _empList.FirstOrDefault(x => x.No.Equals(item.EmpCode, StringComparison.OrdinalIgnoreCase));
//				var leaveType = leaveList.FirstOrDefault(x => x.Code.Equals(item.LeaveType, StringComparison.OrdinalIgnoreCase));
//				var attDetails = new TranSmart.Domain.Models.LM_Attendance.List.AttendanceDetails();
//				if (employee == null)
//				{
//					item.Error = "Invalid employee code";
//				}
//				else
//				{
//					attDetails.EmployeeId = employee.ID;
//				}
//				if (!item.AttendanceDate.Equals(null))
//				{
//					attDetails.AttendanceDate = item.AttendanceDate;
//				}
//				else
//				{
//					item.Error = "Attendance Date is required.";
//				}
//				if (item.AttendanceStatus != null)
//				{
//					if (item.AttendanceStatus.Trim().Equals("Present", StringComparison.OrdinalIgnoreCase))
//					{
//						attDetails.AttendanceStatusID = (int)TranSmart.Data.AttendanceStatus.Present;
//						var att = (int)Data.AttendanceStatus.Present;
//						attDetails.AttendanceStatus = Convert.ToString(att);
//					}
//					else if (item.AttendanceStatus.Trim().Equals("Absent", StringComparison.OrdinalIgnoreCase))
//					{
//						attDetails.AttendanceStatusID = (int)TranSmart.Data.AttendanceStatus.Absent;
//						var att = (int)Data.AttendanceStatus.Absent;
//						attDetails.AttendanceStatus = Convert.ToString(att);
//					}
//					else if (item.AttendanceStatus.Trim().Equals("Leave", StringComparison.OrdinalIgnoreCase))
//					{
//						attDetails.AttendanceStatusID = (int)TranSmart.Data.AttendanceStatus.Leave;
//						var att = (int)Data.AttendanceStatus.Leave;
//						attDetails.AttendanceStatus = Convert.ToString(att);
//					}
//					else if (item.AttendanceStatus.Trim().Equals("WeekOff", StringComparison.OrdinalIgnoreCase))
//					{
//						attDetails.AttendanceStatusID = (int)TranSmart.Data.AttendanceStatus.WeekOff;
//						var att = (int)Data.AttendanceStatus.WeekOff;
//						attDetails.AttendanceStatus = Convert.ToString(att);
//					}
//					else if (item.AttendanceStatus.Trim().Equals("WFH", StringComparison.OrdinalIgnoreCase))
//					{
//						attDetails.AttendanceStatusID = (int)TranSmart.Data.AttendanceStatus.WFH;
//						var att = (int)Data.AttendanceStatus.WFH;
//						attDetails.AttendanceStatus = Convert.ToString(att);
//					}
//					else if (item.AttendanceStatus.Trim().Equals("Holiday", StringComparison.OrdinalIgnoreCase))
//					{
//						attDetails.AttendanceStatusID = (int)TranSmart.Data.AttendanceStatus.Holiday;
//						var att = (int)Data.AttendanceStatus.Holiday;
//						attDetails.AttendanceStatus = Convert.ToString(att);
//					}
//					else if (item.AttendanceStatus.Trim().Equals("HalfDayPresent", StringComparison.OrdinalIgnoreCase))
//					{
//						attDetails.AttendanceStatusID = (int)TranSmart.Data.AttendanceStatus.HalfDayPresent;
//						var att = (int)Data.AttendanceStatus.HalfDayPresent;
//						attDetails.AttendanceStatus = Convert.ToString(att);
//					}
//					else if (item.AttendanceStatus.Trim().Equals("HalfDayLeave", StringComparison.OrdinalIgnoreCase))
//					{
//						attDetails.AttendanceStatusID = (int)TranSmart.Data.AttendanceStatus.HalfDayLeave;
//						var att = (int)Data.AttendanceStatus.HalfDayLeave;
//						attDetails.AttendanceStatus = Convert.ToString(att);
//					}
//					else if (item.AttendanceStatus.Trim().Equals("HalfDayWFH", StringComparison.OrdinalIgnoreCase))
//					{
//						attDetails.AttendanceStatusID = (int)TranSmart.Data.AttendanceStatus.HalfDayWFH;
//						var att = (int)Data.AttendanceStatus.HalfDayWFH;
//						attDetails.AttendanceStatus = Convert.ToString(att);
//					}
//					else if (item.AttendanceStatus.Trim().Equals("HalfDayAbsent", StringComparison.OrdinalIgnoreCase))
//					{
//						attDetails.AttendanceStatusID = (int)TranSmart.Data.AttendanceStatus.HalfDayAbsent;
//						var att = (int)Data.AttendanceStatus.HalfDayAbsent;
//						attDetails.AttendanceStatus = Convert.ToString(att);
//					}
//					else if (item.AttendanceStatus.Trim().Equals("Unautherized", StringComparison.OrdinalIgnoreCase))
//					{
//						attDetails.AttendanceStatusID = (int)TranSmart.Data.AttendanceStatus.Unautherized;
//						var att = (int)Data.AttendanceStatus.Unautherized;
//						attDetails.AttendanceStatus = Convert.ToString(att);
//					}
//					else if (item.AttendanceStatus.Trim().Equals("Late", StringComparison.OrdinalIgnoreCase))
//					{
//						attDetails.AttendanceStatusID = (int)TranSmart.Data.AttendanceStatus.Late;
//						var att = (int)Data.AttendanceStatus.Late;
//						attDetails.AttendanceStatus = Convert.ToString(att);
//					}
//					else
//					{
//						item.Error = "Invalid Attendance Status";
//					}
//					//attDetails.AttendanceStatus = item.AttendanceStatus;
//					if ((attDetails.AttendanceStatusID == (int)TranSmart.Data.AttendanceStatus.Leave || attDetails.AttendanceStatusID == (int)TranSmart.Data.AttendanceStatus.HalfDayLeave) && leaveType == null)
//					{
//						item.Error = "Invalid leave type";
//					}
//				}
//				else
//				{
//					item.Error = "Attendance Status is Required";
//				}

//				if (string.Equals(item.HalfDay, "Yes", StringComparison.OrdinalIgnoreCase) || string.Equals(item.HalfDay, "Y", StringComparison.OrdinalIgnoreCase))
//				{
//					attDetails.IsHalfDay = true;
//					if (item.HalfDayType != null)
//					{
//						if (item.HalfDayType.Trim().Equals("HalfDayPresent", StringComparison.OrdinalIgnoreCase))
//						{
//							attDetails.HalfDayType = (int)TranSmart.Data.AttendanceStatus.HalfDayPresent;
//						}
//						else if (item.HalfDayType.Trim().Equals("HalfDayLeave", StringComparison.OrdinalIgnoreCase))
//						{
//							attDetails.HalfDayType = (int)TranSmart.Data.AttendanceStatus.HalfDayLeave;
//						}
//						else if (item.HalfDayType.Trim().Equals("HalfDayWFH", StringComparison.OrdinalIgnoreCase))
//						{
//							attDetails.HalfDayType = (int)TranSmart.Data.AttendanceStatus.HalfDayWFH;
//						}
//						else if (item.HalfDayType.Trim().Equals("HalfDayAbsent", StringComparison.OrdinalIgnoreCase))
//						{
//							attDetails.HalfDayType = (int)TranSmart.Data.AttendanceStatus.HalfDayAbsent;
//						}
//						else
//						{
//							item.Error = "Invalid half day type.";
//						}
//						if (attDetails.HalfDayType == (int)TranSmart.Data.AttendanceStatus.HalfDayLeave && leaveType == null)
//						{
//							item.Error = "Invalid leave type";
//						}
//					}
//					else
//					{
//						item.Error = "Half Day Type is required.";
//					}
//				}
//				else
//				{
//					attDetails.IsHalfDay = false;
//				}

//				if (item.LeaveType != null && (attDetails.AttendanceStatusID == (int)TranSmart.Data.AttendanceStatus.Leave || attDetails.AttendanceStatusID == (int)TranSmart.Data.AttendanceStatus.HalfDayLeave) ||
//							attDetails.HalfDayType == (int)TranSmart.Data.AttendanceStatus.HalfDayLeave)
//				{
//					LeaveType leave = leaveList.FirstOrDefault(x => x.Code.Equals(item.LeaveType, StringComparison.OrdinalIgnoreCase));
//					attDetails.LeaveTypeId = leave.ID;
//				}
//				if (string.Equals(item.IsFirstOff, "Yes", StringComparison.OrdinalIgnoreCase) || string.Equals(item.IsFirstOff, "Y", StringComparison.OrdinalIgnoreCase))
//				{
//					attDetails.IsFirstOff = true;
//				}
//				else
//				{
//					attDetails.IsFirstOff = false;
//				}
//				if (string.Equals(item.IsUnauthorized, "Yes", StringComparison.OrdinalIgnoreCase) || string.Equals(item.IsUnauthorized, "Y", StringComparison.OrdinalIgnoreCase))
//				{
//					if (item.Unauthorized != null)
//					{
//						attDetails.Unauthorized = item.Unauthorized;
//					}
//				}
//				else
//				{
//					attDetails.Unauthorized = 0;
//				}

//				if (string.IsNullOrEmpty(item.Error))
//				{
//					attDetailsList.Add(attDetails);
//				}
//				else
//				{
//					errors.Add(item);
//				}
//			}

//			if (errors.Any(x => x.Error.Length > 0))
//			{
//				result.AddMessageItem(new MessageItem("Invalid data found in sheet. Please correct the data and re-upload."));
//				result.ReturnValue = errors;
//				result.Headers = _mapper.Map<List<Core.HeaderModel>>(attendanceStatusService.GetAllColumns());
//				return Ok(result);
//			}

//			var addResult = await _attendanceService.AttendanceUpdate(attDetailsList, LOGIN_USER_EMPId);
//			addResult.ReturnValue = null;
//			return addResult.HasError ? BadRequest(addResult) : Ok(addResult);
//		}

//		[HttpPost("AddAttendance")]
//		public async Task<IActionResult> AddAttendance([FromForm] ImportData model)
//		{
//			var result = new FileImportResult<List<ImportRef.AddAttendanceModel>>();
//			var addAttendanceService = new TranSmart.API.Services.Import.AddAttendanceService();
//			//Header validation
//			if (!addAttendanceService.ValidateHeaders(model.FormFile.OpenReadStream()))
//			{
//				return BadRequest("Invalid Headers");
//			}
//			IEnumerable<ImportRef.AddAttendanceModel> addAttendanceList = addAttendanceService.ToModel(model.FormFile.OpenReadStream());
//			IEnumerable<Employee> _empList = await _employeeService.GetList("No");
//			var attendanceList = new List<TranSmart.Domain.Entities.Leave.Attendance>();
//			var errors = new List<ImportRef.AddAttendanceModel>();

//			foreach (ImportRef.AddAttendanceModel item in addAttendanceList)
//			{
//				Employee employee = _empList.FirstOrDefault(x => x.No.Equals(item.EmpCode, StringComparison.OrdinalIgnoreCase));
//				var attDetails = new TranSmart.Domain.Entities.Leave.Attendance();
//				if (employee == null)
//				{
//					item.Error = "Invalid employee code";
//				}
//				else
//				{
//					attDetails.EmployeeId = employee.ID;
//				}
//				if (!item.AttendanceDate.Equals(null))
//				{
//					attDetails.AttendanceDate = item.AttendanceDate;
//				}
//				else
//				{
//					item.Error = "Attendance Date is required.";
//				}
//				if (string.IsNullOrEmpty(item.Error))
//				{
//					attendanceList.Add(attDetails);
//				}
//				else
//				{
//					errors.Add(item);
//				}
//			}

//			if (errors.Any(x => x.Error.Length > 0))
//			{
//				result.AddMessageItem(new MessageItem("Invalid data found in sheet. Please correct the data and re-upload."));
//				result.ReturnValue = errors;
//				result.Headers = _mapper.Map<List<Core.HeaderModel>>(addAttendanceService.GetAllColumns());
//				return Ok(result);
//			}

//			var addResult = await _attendanceService.AddAttendance(attendanceList);
//			addResult.ReturnValue = null;
//			return addResult.HasError ? BadRequest(addResult) : Ok(addResult);
//		}
//		#endregion

//		#region Employee
//		[HttpPost("Employee")]
//		public async Task<IActionResult> Employee([FromForm] ImportData model)
//		{
//			var result = new FileImportResult<List<ImportRef.EmployeeModel>>();
//			var employeeService = new Services.Import.EmployeeService();
//			var empInfo = new List<Employee>();
//			var departments = await _departmentService.GetList("Name");
//			var designations = await _designationService.GetList("Name");
//			var locations = await _locService.GetList("Name");
//			var lineOfBusiness = await _lobService.GetList("Name");
//			var functionalArea = await _functionalAreaService.GetList("Name");
//			var teams = await _teamService.GetList("Name");
//			var workTypes = await _workTypeService.GetList("Name");

//			//Header validation
//			if (!employeeService.ValidateHeaders(model.FormFile.OpenReadStream()))
//			{
//				return BadRequest("Invalid headers found in sheet.");
//			}
//			IEnumerable<ImportRef.EmployeeModel> employeeList = employeeService.ToModel(model.FormFile.OpenReadStream());
//			IEnumerable<Employee> _empList = await _employeeService.GetList("No");
//			var errors = new List<ImportRef.EmployeeModel>();
//			foreach (ImportRef.EmployeeModel item in employeeList)
//			{
//				var addEmp = new Employee();
//				Employee employee = _empList.FirstOrDefault(x => x.No.Equals(item.EmpCode, StringComparison.OrdinalIgnoreCase));
//				var error = new StringBuilder();
//				if (employee == null)
//				{
//					addEmp.No = item.EmpCode;
//					error.Append(string.IsNullOrEmpty(item.Name) ? "Employee name is required." : "");
//					error.Append(string.IsNullOrEmpty(item.MobileNumber) ? "Mobile number is required." : "");
//					error.Append(string.IsNullOrEmpty(item.Gender) ? "Gender is required." : "");
//					error.Append(!item.DateOfBirth.HasValue ? "Date of birth is required." : "");
//					error.Append(!item.DateOfJoining.HasValue ? "Date of joining is required." : "");
//					error.Append(string.IsNullOrEmpty(item.WorkLocation) ? "Work location is required." : "");
//					error.Append(string.IsNullOrEmpty(item.AadhaarNumber) ? "Aadhaar number is required." : "");
//					error.Append(string.IsNullOrEmpty(item.Designation) ? "Designation is required." : "");
//					error.Append(string.IsNullOrEmpty(item.Department) ? "Department is required." : "");
//					error.Append(string.IsNullOrEmpty(item.WorkFromHome) ? "Work From Home is required." : "");
//					error.Append(string.IsNullOrEmpty(item.Team) ? "Team is required." : "");
//					error.Append(string.IsNullOrEmpty(item.WorkType) ? "Work Type is required." : "");
//					error.Append(string.IsNullOrEmpty(item.FirstName) ? "First Name is required." : "");
//					error.Append(!item.DOBC.HasValue ? "DOB As Per Certificate is required." : "");
//					item.Error = error.ToString();
//				}
//				else
//				{
//					item.Error = "Employee already exist.";
//				}

//				Location location = locations.FirstOrDefault(x => x.Name.Equals(item.WorkLocation, StringComparison.OrdinalIgnoreCase));
//				Department department = departments.FirstOrDefault(x => x.Name.Equals(item.Department, StringComparison.OrdinalIgnoreCase));
//				Designation designation = designations.FirstOrDefault(x => x.Name.Equals(item.Designation, StringComparison.OrdinalIgnoreCase));
//				LOB lob = lineOfBusiness.FirstOrDefault(x => x.Name.Equals(item.LOB, StringComparison.OrdinalIgnoreCase));
//				FunctionalArea area = functionalArea.FirstOrDefault(x => x.Name.Equals(item.FunctionalArea, StringComparison.OrdinalIgnoreCase));
//				Team team = teams.FirstOrDefault(x => x.Name.Equals(item.Team, StringComparison.OrdinalIgnoreCase));
//				WorkType workType = workTypes.FirstOrDefault(x => x.Name.Equals(item.WorkType, StringComparison.OrdinalIgnoreCase));

//				if (!string.IsNullOrEmpty(item.Error))
//				{
//					errors.Add(item);
//				}
//				else if (item.AadhaarNumber != "" && !Regex.IsMatch(item.AadhaarNumber, ErrMessages.Regx_Aadhar))
//				{
//					item.Error = "Invalid Aadhar number";
//					errors.Add(item);
//				}
//				else if (item.MobileNumber != "" && !Regex.IsMatch(item.MobileNumber, ErrMessages.Regx_Phone))
//				{
//					item.Error = "Invalid Mobile number";
//				}
//				else if (department == null)
//				{
//					item.Error = "Invalid Department";
//					errors.Add(item);
//				}
//				else if (designation == null)
//				{
//					item.Error = "Invalid Designation";
//					errors.Add(item);
//				}
//				else if (location == null)
//				{
//					item.Error = "Invalid Location";
//					errors.Add(item);
//				}
//				else
//				{
//					int gender = 0;
//					if (item.Gender.Equals("Male", StringComparison.OrdinalIgnoreCase) ||
//								item.Gender.Equals("M", StringComparison.OrdinalIgnoreCase))
//						gender = 1;
//					else if (item.Gender.Equals("Female", StringComparison.OrdinalIgnoreCase) ||
//								item.Gender.Equals("f", StringComparison.OrdinalIgnoreCase))
//						gender = 2;
//					int workFromHome = 0;
//					if (item.WorkFromHome.Equals("Yes", StringComparison.OrdinalIgnoreCase) ||
//						item.WorkFromHome.Equals("Y", StringComparison.OrdinalIgnoreCase))
//					{
//						workFromHome = 1;
//					}
//					else
//					{
//						workFromHome = 0;
//					}
//					int maritalStatus = 0;
//					if (item.MaritalStatus.Equals("Married", StringComparison.OrdinalIgnoreCase))
//					{
//						maritalStatus = 1;
//					}
//					else if (item.MaritalStatus.Equals("Single", StringComparison.OrdinalIgnoreCase))
//					{
//						maritalStatus = 2;
//					}
//					else
//					{
//						maritalStatus = 3;
//					}
//					int employeeStatus = 0;
//					if (item.Status.Equals("Active", StringComparison.OrdinalIgnoreCase))
//					{
//						employeeStatus = 1;
//					}
//					else if (item.Status.Equals("Resigned", StringComparison.OrdinalIgnoreCase))
//					{
//						employeeStatus = 2;
//					}

//					empInfo.Add(new Employee
//					{
//						No = item.EmpCode,
//						Name = item.Name,
//						Gender = gender,
//						DateOfBirth = item.DateOfBirth.Value,
//						DateOfJoining = item.DateOfJoining.Value,
//						WorkLocationId = location.ID,
//						AadhaarNumber = item.AadhaarNumber,
//						DesignationId = designation.ID,
//						DepartmentId = department.ID,
//						TeamId = team.ID,
//						WorkTypeId = workType.ID,
//						MobileNumber = item.MobileNumber,
//						WorkFromHome = workFromHome,
//						FirstName = item.FirstName,
//						MiddleName = item.MiddleName,
//						LastName = item.LastName,
//						DOBC = item.DOBC.Value,
//						MaritalStatus = maritalStatus,
//						LOBId = lob?.ID,
//						FunctionalAreaId = area?.ID,
//						Status = employeeStatus
//					});
//				}
//			}
//			if (errors.Any(x => x.Error.Length > 0))
//			{
//				result.AddMessageItem(new MessageItem("Invalid data found in sheet. Please correct the data and re-upload."));
//				result.ReturnValue = errors;
//				result.Headers = _mapper.Map<List<Core.HeaderModel>>(employeeService.GetAllColumns());
//				return Ok(result);
//			}
//			var addResult = await _employeeService.AddEmployee(empInfo);
//			return addResult.HasError ? BadRequest(addResult) : Ok(addResult);
//		}
//		#endregion

//		#region Latecomers
//		[HttpPost("Latecomers")]
//		public async Task<IActionResult> Latecomers([FromForm] ImportData model)
//		{
//			var result = new FileImportResult<List<LatecomersModel>>();
//			if (!model.Date.HasValue)
//			{
//				result.AddMessageItem(new MessageItem("Please select Month and Year"));
//				return BadRequest(result);
//			}
//			if (!await _payMonthService.CheckPayMonthIsOpen(model.Date.Value.Year, model.Date.Value.Month))
//			{
//				result.AddMessageItem(new MessageItem("Selected month pay roll is closed"));
//				return BadRequest(result);
//			}
//			var latecomer = new Services.Import.LatecomersService();
//			if (latecomer.ValidateHeaders(model.FormFile.OpenReadStream()))
//			{
//				IEnumerable<ImportRef.LatecomersModel> list = latecomer.ToModel(model.FormFile.OpenReadStream());
//				IEnumerable<Employee> employees = await _employeeService.GetList("Name");
//				var latecomers = new List<Latecomers>();
//				var errors = new List<ImportRef.LatecomersModel>();
//				foreach (ImportRef.LatecomersModel item in list)
//				{
//					Employee e = employees.FirstOrDefault(x => x.No.Equals(item.EmployeeCode, StringComparison.OrdinalIgnoreCase));
//					if (e == null)
//					{
//						item.Error = "Invalid employee code";
//						errors.Add(item);
//						continue;
//					}
//					if (decimal.TryParse(item.NumberOfdays.ToString(), out decimal numberOfDays) && numberOfDays < 0)
//					{
//						item.Error = "Number of days should not negative value";
//						errors.Add(item);
//						continue;
//					}
//					latecomers.Add(new Latecomers
//					{
//						ID = Guid.NewGuid(),
//						EmployeeID = e.ID,
//						Month = model.Date.Value.Month,
//						Year = model.Date.Value.Year,
//						NumberOfDays = item.NumberOfdays,
//					});
//				}
//				if (errors.Count > 0)
//				{
//					result.AddMessageItem(new MessageItem("Invalid data found in sheet. Please correct the data and re-upload."));
//					result.ReturnValue = errors;
//					result.Headers = _mapper.Map<List<Core.HeaderModel>>(latecomer.GetAllColumns());
//				}
//				else
//				{
//					var latecomersAddResult = await _latecomersService.AddBulk(latecomers, model.Date.Value.Month, model.Date.Value.Year);
//					return latecomersAddResult.HasError ? BadRequest(latecomersAddResult) : Ok(latecomersAddResult);
//				}
//				return Ok(result);
//			}
//			else
//			{
//				return BadRequest(result);
//			}
//		}
//		#endregion

//		#region AttendanceImport
//		[HttpPost("Attendance")]
//		public async Task<IActionResult> AttendanceImport([FromForm] ImportData model)
//		{
//			var result = new FileImportResult<List<AttendanceImportModel>>();
//			if (!model.Date.HasValue)
//			{
//				result.AddMessageItem(new MessageItem("Please select Month and Year"));
//				return BadRequest(result);
//			}
//			if (!await _payMonthService.CheckPayMonthIsOpen(model.Date.Value.Year, model.Date.Value.Month))
//			{
//				result.AddMessageItem(new MessageItem("Selected month pay roll is closed"));
//				return BadRequest(result);
//			}
//			var attendanceImport = new Services.Import.AttendanceImportService();
//			if (attendanceImport.ValidateHeaders(model.FormFile.OpenReadStream()))
//			{
//				IEnumerable<ImportRef.AttendanceImportModel> list = attendanceImport.ToModel(model.FormFile.OpenReadStream());
//				IEnumerable<Employee> employees = await _employeeService.GetList("Name");
//				var attendanceSum = new List<AttendanceSum>();
//				var errors = new List<ImportRef.AttendanceImportModel>();
//				foreach (ImportRef.AttendanceImportModel item in list)
//				{
//					Employee e = employees.FirstOrDefault(x => x.No.Equals(item.EmployeeCode, StringComparison.OrdinalIgnoreCase));
//					if (e == null)
//					{
//						item.Error = "Invalid employee code";
//						errors.Add(item);
//						continue;
//					}
//					attendanceSum.Add(new AttendanceSum
//					{
//						ID = Guid.NewGuid(),
//						EmployeeId = e.ID,
//						Month = (byte)model.Date.Value.Month,
//						Year = (short)model.Date.Value.Year,
//						Present = item.Present,
//						LOP = item.LOP,
//						Unauthorized = item.Unauthorized,
//						OffDays = item.OffDays,
//					});
//				}
//				if (errors.Count > 0)
//				{
//					result.AddMessageItem(new MessageItem("Invalid data found in sheet. Please correct the data and re-upload."));
//					result.ReturnValue = errors;
//					result.Headers = _mapper.Map<List<Core.HeaderModel>>(attendanceImport.GetAllColumns());
//				}
//				else
//				{
//					var attendanceImportAddResult = await _attendanceSumService.AddBulk(attendanceSum, model.Date.Value.Year, model.Date.Value.Month);
//					return attendanceImportAddResult.HasError ? BadRequest(attendanceImportAddResult) : Ok(attendanceImportAddResult);
//				}
//				return Ok(result);
//			}
//			else
//			{
//				return BadRequest(result);
//			}
//		}
//		#endregion


//		#region Question
//		[HttpPost("Question")]
//		public async Task<IActionResult> Question([FromForm] QuestionImportData model)
//		{
//			var result = new FileImportResult<List<ImportRef.QuestionModel>>();
//			var results = new FileImportResult<List<Question>>();

//			if (model.FormFile.FileName.Split(".").Last() != "xlsx")
//			{
//				result.AddMessageItem(new MessageItem("Please import .xlsx file format"));
//				return BadRequest(result);
//			}

//			var question = new Services.Import.QuestionService();
//			if (question.ValidateHeaders(model.FormFile.OpenReadStream()))
//			{
//				var list = question.ToModel(model.FormFile.OpenReadStream());
//				if (!list.Any())
//				{
//					result.AddMessageItem(new MessageItem("No data found in a sheet"));
//					return BadRequest(result);
//				}

//				if (list.GroupBy(x => x.Question).Any(g => g.Count() > 1))
//				{
//					result.AddMessageItem(new MessageItem("Duplicate questions in a sheet, Please correct the data and re-upload."));
//					return BadRequest(result);
//				}

//				var questions = new List<TranSmart.Domain.Models.OnlineTest.Response.QuestionModel>();
//				var errors = new List<ImportRef.QuestionModel>();
//				var paper = await _paperService.GetById(model.PaperId);
//				foreach (var item in list)
//				{
//					var choices = new List<ChoiceModel>();
//					if (paper.MoveToLive)
//					{
//						item.Error = "Paper is in live, questions can't be added";
//						errors.Add(item);
//					}
//					else if (!paper.Status)
//					{
//						item.Error = "Paper is disabled";
//						errors.Add(item);
//					}
//					else if (string.IsNullOrEmpty(item.Question))
//					{
//						item.Error = "Question is required";
//						errors.Add(item);
//					}
//					else if (string.IsNullOrEmpty(item.Key))
//					{
//						item.Error = "Key is required";
//						errors.Add(item);
//					}
//					else if (string.IsNullOrEmpty(item.Type))
//					{
//						item.Error = "Type is required";
//						errors.Add(item);
//					}
//					else if (ConstUtil.QueType(item.Type.Replace(" ", "")) == 0)
//					{
//						item.Error = "Invalid type";
//						errors.Add(item);
//					}
//					else if ((!string.IsNullOrEmpty(item.OptionA) || !string.IsNullOrEmpty(item.OptionB)
//						|| !string.IsNullOrEmpty(item.OptionC) || !string.IsNullOrEmpty(item.OptionD)
//						|| !string.IsNullOrEmpty(item.OptionE) || !string.IsNullOrEmpty(item.OptionF))
//						 && !item.Type.Equals("single", StringComparison.OrdinalIgnoreCase)
//						 && !item.Type.Equals("multiple", StringComparison.OrdinalIgnoreCase))
//					{
//						item.Error = $"options not allowed for {item.Type} type question";
//						errors.Add(item);
//					}
//					else if ((item.Type.Equals("single", StringComparison.OrdinalIgnoreCase) ||
//						item.Type.Equals("multiple", StringComparison.OrdinalIgnoreCase))
//						&& (string.IsNullOrEmpty(item.OptionA) || string.IsNullOrEmpty(item.OptionB)))
//					{
//						item.Error = "Option A and B are required";
//						errors.Add(item);
//					}
//					else if (item.Type.Replace(" ", "").ToLower() == "torf")
//					{
//						if (item.Key.ToLower() != "t" && item.Key.ToLower() != "f")
//						{
//							item.Error = "Invalid key";
//							errors.Add(item);
//						}
//						else
//						{
//							item.Key = item.Key.ToLower() == "t" ? "true" : "false";
//						}
//					}
//					else
//					{
//						if (!string.IsNullOrEmpty(item.OptionC))
//						{
//							choices.Add(new ChoiceModel
//							{
//								Text = item.OptionC.Trim(),
//								Choice = "C",
//								SNo = 3
//							});
//						}
//						if (!string.IsNullOrEmpty(item.OptionD))
//						{
//							if (string.IsNullOrEmpty(item.OptionC))
//							{
//								item.Error = "Please enter options in order";
//								errors.Add(item);
//							}
//							else
//							{
//								choices.Add(new ChoiceModel
//								{
//									Text = item.OptionD.Trim(),
//									Choice = "D",
//									SNo = 4
//								});
//							}
//						}
//						if (!string.IsNullOrEmpty(item.OptionE))
//						{
//							if (string.IsNullOrEmpty(item.OptionD))
//							{
//								item.Error = "Please enter options in order";
//								errors.Add(item);
//							}
//							else
//							{
//								choices.Add(new ChoiceModel
//								{
//									Text = item.OptionE.Trim(),
//									SNo = 5,
//									Choice = "E",
//								});
//							}
//						}
//						if (!string.IsNullOrEmpty(item.OptionF))
//						{
//							if (string.IsNullOrEmpty(item.OptionE))
//							{
//								item.Error = "Please enter options in order";
//								errors.Add(item);
//							}
//							else
//							{
//								choices.Add(new ChoiceModel
//								{
//									Text = item.OptionF.Trim(),
//									Choice = "F",
//									SNo = 6
//								});
//							}
//						}
//						if (!string.IsNullOrEmpty(item.OptionA))
//						{
//							choices.Add(new ChoiceModel
//							{
//								Text = item.OptionA.Trim(),
//								Choice = "A",
//								SNo = 1
//							});
//							item.Key = item.Key.ToUpper();
//						}
//						if (!string.IsNullOrEmpty(item.OptionB))
//						{
//							choices.Add(new ChoiceModel
//							{
//								Text = item.OptionB.Trim(),
//								Choice = "B",
//								SNo = 2
//							});
//						}
//						if (choices.GroupBy(x => x.Text).Any(g => g.Count() > 1))
//						{
//							item.Error = "Duplicate options not allowed";
//							errors.Add(item);
//						}

//						byte type = ConstUtil.QueType(item.Type.Replace(" ", ""));
//						if (type == (byte)QuestionType.Single && string.IsNullOrEmpty(item.Error))
//						{
//							if (item.Key.Replace(",", "").Length > 1)
//							{
//								item.Error = "Multi answers not allowed in single type";
//								errors.Add(item);
//							}
//							else if (choices.FirstOrDefault(x => x.SNo == ConstUtil.GetSNo(item.Key.ToUpper().Trim(',', ' '))) == null)
//							{
//								item.Error = "Check the key";
//								errors.Add(item);
//							}
//						}
//						else if (type == (byte)QuestionType.Multiple && string.IsNullOrEmpty(item.Error))
//						{
//							if (item.Key.Replace(",", "").Replace(" ", "").Length > choices.Count)
//							{
//								item.Error = "Keys are more than options";
//								errors.Add(item);
//							}
//							else
//							{
//								foreach (var key in item.Key.Replace(" ", "").Split(","))
//								{
//									if (!string.IsNullOrEmpty(key) && choices.FirstOrDefault(x => x.SNo == ConstUtil.GetSNo(key.ToUpper())) == null)
//									{
//										item.Error = "Check the key";
//										errors.Add(item);
//										break;
//									}
//								}
//							}

//						}
//					}

//					if (!errors.Any())
//					{
//						questions.Add(new TranSmart.Domain.Models.OnlineTest.Response.QuestionModel
//						{
//							Text = string.Format("<p>{0}</p>", item.Question),
//							PaperId = paper.ID,
//							Type = ConstUtil.QueType(item.Type.Replace(" ", "")),
//							Choices = choices,
//							Key = item.Key.Replace(" ", "").Trim(',', ' '),
//						});
//					}
//				}

//				if (errors.Any())
//				{
//					result.AddMessageItem(new MessageItem("Invalid data found in sheet. Please correct the data and re-upload."));
//					result.ReturnValue = errors;
//					result.Headers = _mapper.Map<List<Core.HeaderModel>>(question.GetAllColumns());
//					return Ok(result);
//				}
//				var addBulkResult = await _questionService.AddBulk(_mapper.Map<List<Question>>(questions), questions);
//				if (addBulkResult.HasError)
//				{
//					return BadRequest(addBulkResult);
//				}
//				return Ok(addBulkResult);
//			}
//			else
//			{
//				result.AddMessageItem(new MessageItem("Invalid headers"));
//				return BadRequest(result);
//			}
//		}

//		#endregion
//	}
//}
