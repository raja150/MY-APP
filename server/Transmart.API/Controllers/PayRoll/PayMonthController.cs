using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using Aspose.Cells;
using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using TranSmart.API.Extensions;
using TranSmart.API.Models;
using TranSmart.API.Services;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Payroll;
using TranSmart.Domain.Models.Payroll.Response;
using TranSmart.Service;
using TranSmart.Service.Organization;
using TranSmart.Service.Payroll;


namespace TranSmart.API.Controllers.Payroll
{
	[Route("api/PayRoll/[controller]")]
	[ApiController]
	public class PayMonthController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IPayMonthService _service;
		private readonly IEarningComponentService _earningService;
		private readonly IDeductionComponentService _deductionService;
		private readonly IEmpStatutoryService _empstatutoryservice;
		private readonly IOrganizationsService _orgService;
		private readonly IApplicationAuditLogService _auditLogService;

		public PayMonthController(IMapper mapper, IPayMonthService service,
			IEarningComponentService earningService,
			IOrganizationsService orgService,
			IDeductionComponentService deductionService, IEmpStatutoryService empStatutoryService,
			IApplicationAuditLogService auditLogService)
		{
			_mapper = mapper;
			_service = service;
			_earningService = earningService;
			_deductionService = deductionService;
			_empstatutoryservice = empStatutoryService;
			_orgService = orgService;
			_auditLogService = auditLogService;
		}
		[HttpGet("Taxes/{id}")]
		[ApiAuthorize(Core.Permission.PS_Payrun, Core.Privilege.Read)]
		public async Task<IActionResult> GetTaxes(Guid id)
		{
			await _auditLogService.GetAccesedUser(UserId, "Viewed", IpAddress, (Core.Permission.PS_Payrun).ToString());
			return Ok(await _service.TaxDeductions(id));
		}
		[HttpGet("PayMonthsList")]
		[ApiAuthorize(Core.Permission.PS_Payrun, Core.Privilege.Read)]
		public async Task<IActionResult> PayMonthList([FromQuery] BaseSearch search)
		{
			await _auditLogService.GetAccesedUser(UserId, "Viewed", IpAddress, (Core.Permission.PS_Payrun).ToString());
			return Ok(_mapper.Map<Models.Paginate<PayMonthList>>(await _service.GetPayMonthList(search.RefId, search)));
		}
		[HttpGet("Months")]
		[ApiAuthorize(Core.Permission.PS_Payrun, Core.Privilege.Read)]
		public async Task<IActionResult> Months()
		{
			return Ok(_mapper.Map<List<PayMonthModel>>(await _service.GetPayMonths()));
		}
		[HttpGet("{id}")]
		[ApiAuthorize(Core.Permission.PS_Payrun, Core.Privilege.Read)]
		public async Task<IActionResult> Get(Guid id)
		{
			await _auditLogService.GetAccesedUser(UserId, "Read", IpAddress, (Core.Permission.PS_Payrun).ToString());
			return Ok(_mapper.Map<PayMonthModel>(await _service.GetById(id)));
		}

		[HttpGet("Employees")]
		[ApiAuthorize(Core.Permission.PS_Payrun, Core.Privilege.Read)]
		public async Task<IActionResult> Employees([FromQuery] BaseSearch search)
		{
			await _auditLogService.GetAccesedUser(UserId, "Viewed", IpAddress, (Core.Permission.PS_Payrun).ToString());
			return Ok(_mapper.Map<Models.Paginate<PaySheetModel>>(await _service.GetPaySheets(search)));
		}

		[HttpGet("HoldEmployees")]
		[ApiAuthorize(Core.Permission.PS_Payrun, Core.Privilege.Read)]
		public async Task<IActionResult> HoldEmployees([FromQuery] BaseSearch search)
		{
			return Ok(_mapper.Map<Models.Paginate<PaySheetModel>>(await _service.GetHoldPaysheet(search)));
		}

		[HttpGet("HoldSalary/{id}")]
		[ApiAuthorize(Core.Permission.PS_Payrun, Core.Privilege.Read)]
		public async Task<IActionResult> HoldSalary(Guid id)
		{
			await _service.HoldSalary(id);
			return Ok();
		}
		[HttpGet("ReleaseSalary/{id}")]
		[ApiAuthorize(Core.Permission.PS_Payrun, Core.Privilege.Read)]
		public async Task<IActionResult> ReleaseSalary(Guid id)
		{
			Result<PaySheet> result = await _service.ReleaseSalary(id);
			if (result.HasError)
			{
				return BadRequest(result);
			}
			return Ok(_mapper.Map<PaySheetModel>(result.ReturnValue));
		}

		[HttpGet("PaySlip")]
		[ApiAuthorize(Core.Permission.PS_Payrun, Core.Privilege.Read)]
		public async Task<IActionResult> GetEmployeePay(Guid Id, Guid EmployeeId)
		{
			try
			{
				return Ok(_mapper.Map<PaySlipModel>(await _service.GetEmployeePay(Id, EmployeeId)));
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpGet("MyPaySlip")]
		[ApiAuthorize(Core.Permission.SS_SalaryDetails, Core.Privilege.Read)]
		public async Task<IActionResult> GetMyPaySlip(Guid Id)
		{
			if (!IS_USER_EMPLOYEE) return BadRequest();
			return Ok(_mapper.Map<PaySlipModel>(await _service.GetEmployeePay(Id, LOGIN_USER_EMPId)));
		}
		[HttpGet("SendPayslips")]
		[ApiAuthorize(Core.Permission.PS_Payrun, Core.Privilege.Read)]
		public async Task<IActionResult> PaySlips(Guid PayMonthID)
		{
			await _auditLogService.GetAccesedUser(UserId, "Send PaySlips", IpAddress, (Core.Permission.PS_Payrun).ToString());

			try
			{
				PayMonth payMonth = await _service.GetById(PayMonthID);
				//Dictionary<string, object[]> disctionaryList = new Dictionary<string, object[]>();
				var list = await _service.GetSheets(payMonth.Year, payMonth.Month);
				//IEnumerable<EarningComponent> earnings = await _earningService.GetList("Name");
				//IEnumerable<DeductionComponent> deductions = await _deductionService.GetList("Name");
				IEnumerable<EmpStatutory> _statutoryList = await _empstatutoryservice.GetList("Id");

				foreach (PaySheet psheet in list.ToList())
				{

					var paySlip = new PaySlip
					{
						MailId = string.IsNullOrEmpty(psheet.Employee.WorkEmail) ? psheet.Employee.PersonalEmail : psheet.Employee.WorkEmail,
						EmpCode = psheet.Employee.No,
						EmpName = psheet.Employee.Name,
						Designation = psheet.Employee.Designation.Name
					};
					if (_statutoryList.Any(x => x.EmpId == psheet.Employee.ID))
					{
						paySlip.PFNo = string.IsNullOrEmpty(_statutoryList.FirstOrDefault(x => x.EmpId == psheet.Employee.ID).UAN) ? "" : _statutoryList.FirstOrDefault(x => x.EmpId == psheet.Employee.ID).UAN;
						paySlip.ESINo = string.IsNullOrEmpty(_statutoryList.FirstOrDefault(x => x.EmpId == psheet.Employee.ID).ESINo) ? "" : _statutoryList.FirstOrDefault(x => x.EmpId == psheet.Employee.ID).ESINo;
					}
					paySlip.Month = psheet.PayMonth.Name;
					paySlip.WorkingDays = psheet.WorkDays;
					paySlip.PresentedDays = psheet.PresentDays;
					paySlip.LeavesTaken = psheet.LOPDays;//Need to add in main table
					paySlip.LOP = psheet.LOPDays;
					paySlip.Incentives = psheet.Incentive;
					paySlip.Deductions = psheet.PayCut;
					paySlip.NetPayble = psheet.Net;
					paySlip.EarningList = new List<Earning>();
					paySlip.DeductionsList = new List<Deductions>();

					foreach (PaySheetEarning item in psheet.Earnings.OrderBy(x => x.Component.DisplayOrder))
					{
						if (!(item.Component.HideWhenZero && item.Earning == 0))
						{
							paySlip.EarningList.Add(new Earning { Name = item.HeaderName, Value = item.Earning });
						}
					}
					foreach (PaySheetDeduction item in psheet.Deductions)
					{
						paySlip.DeductionsList.Add(new Deductions { Name = item.HeaderName, Value = item.Deduction });
					}

					paySlip.EarningList.Add(new Earning { Name = "Arrears", Value = psheet.Arrears });
					paySlip.EarningList.Add(new Earning { Name = "Incentives", Value = psheet.Incentive });

					//Deductions
					paySlip.DeductionsList.Add(new Deductions { Name = "Professional Tax", Value = psheet.PTax });
					paySlip.DeductionsList.Add(new Deductions { Name = "PF", Value = psheet.EPF });

					paySlip.DeductionsList.Add(new Deductions { Name = "ESI", Value = psheet.ESI });

					paySlip.DeductionsList.Add(new Deductions { Name = "Income Tax", Value = psheet.Tax });

					paySlip.DeductionsList.Add(new Deductions { Name = "LOP for Leaves", Value = psheet.LOP });

					paySlip.DeductionsList.Add(new Deductions { Name = "Deduction", Value = psheet.PayCut });
					paySlip.DeductionsList.Add(new Deductions { Name = "Salary In Advance", Value = psheet.Loan });

					paySlip.GrossEarnings = psheet.Gross;
					paySlip.GrossDeductions = psheet.Deduction;
					var pedConv = new PdfGenerator();
					var pdfstream = pedConv.ConvertClassToPdf(paySlip);
					// MailService ms = new MailService()
					// if (string.IsNullOrEmpty(FromMail))
					string FromMail = "yvsrao@mail.avontixindia.com";

					if (string.IsNullOrEmpty(paySlip.MailId))
						paySlip.MailId = "jhkrishna@mail.avontixindia.com";
					await MailService.SendPaySlips(pdfstream, FromMail, paySlip.MailId, paySlip.EmpCode, paySlip.Month);
					await _service.PaySlipSendedOn(psheet.ID);
					#region unwanted
					//slip.BalanceEL = 0;
					//slip.BalanceCL = 0;

					//paySlip.BASIC = psheet.Earnings.Where(e => e.HeaderName.Equals("BASIC", StringComparison.OrdinalIgnoreCase)).FirstOrDefault().Earning;
					//paySlip.HRA = psheet.Earnings.Where(e => e.HeaderName.Equals("HRA", StringComparison.OrdinalIgnoreCase)).FirstOrDefault().Earning;
					//paySlip.MedicalAndTransAllow = psheet.Earnings.Where(e => e.HeaderName.Equals("Medical & Transport Allowance", StringComparison.OrdinalIgnoreCase)).FirstOrDefault().Earning;
					//paySlip.FoodCoupons = psheet.Earnings.Where(e => e.HeaderName.Equals("Food Coupons", StringComparison.OrdinalIgnoreCase)).FirstOrDefault().Earning;
					//paySlip.SpecialAllow = psheet.Earnings.Where(e => e.HeaderName.Equals("Special Allowance", StringComparison.OrdinalIgnoreCase)).FirstOrDefault().Earning;

					//paySlip.Arrears = incen.Arrears; //psheet.Arrears
					//paySlip.Incentives = psheet.Incentive;

					//paySlip.ProfessionalTax = psheet.PTax;
					//paySlip.PF = psheet.EPF;
					//paySlip.ESI = psheet.ESI;
					//paySlip.IncomeTax = psheet.Tax;

					//paySlip.SalaryAdvance = 0;

					//paySlip.LOPForLeaves = psheet.LOP;
					//paySlip.Deductions = psheet.Deduction;
					//paySlip.GrossEarnings = psheet.Gross;


					//paySlip.GrossDeductions = 0;

					//paySlip.NetPayble = psheet.Net;

					//paySlip.NetPaybleText = "";
					//paySlip.BabkAccNo = psheet.BankACNo;
					//paySlip.FaxFilesAndArrears = incen.FaxFilesAndSpecialIncentives;
					//paySlip.FaxFilesAndArrears = incen.FaxFilesAndSpecialIncentives;
					//paySlip.ProductionIncentives = incen.Production;
					//paySlip.SpotIncentives = incen.SpotIncentives;
					//paySlip.SundayIncentives = incen.Sunday;
					//paySlip.CentumDoubleCentumClub = incen.CentumClub;
					//paySlip.FirstMinuteIncentive = incen.FirstMinIncentive;
					//paySlip.OtherIncentives = 0;
					//paySlip.NightShiftIncentives = 0;
					//paySlip.WeeklyStarIncentives = 0;
					//paySlip.TTeamIncentives = incen.TeamIncentives;
					//paySlip.DoublePay = 0;
					//paySlip.OtherDeductions = 0;
					//paySlip.InternalQualityFeedback = incen.InternalFeedBack;
					//paySlip.ExternalQualityFeedback = incen.ExternalFeedBack;
					//paySlip.UnauthorizedLeaveDeduction = 0;
					//paySlip.TotalIncentives = psheet.Incentive;
					//paySlip.TotalDeductions = psheet.Deduction;
					//paySlip.LIAmount = 0;
					#endregion
				}
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		[HttpGet("PaySheet/{id}/{password}")]
		[ApiAuthorize(Core.Permission.PS_Payrun, Core.Privilege.Read)]
		public async Task<IActionResult> PaySheet(Guid id, string password)
		{
			await _auditLogService.GetAccesedUser(UserId, "Downloaded ExcelSheet", IpAddress, (Core.Permission.PS_Payrun).ToString());
			var disctionaryList = new Dictionary<string, object[]>();
			IEnumerable<PaySheet> list = await _service.GetPaysheet(id);
			IEnumerable<EarningComponent> earnings = await _earningService.GetList("DisplayOrder");
			IEnumerable<DeductionComponent> deductions = await _deductionService.GetList("DisplayOrder");

			var headers = new Dictionary<string, Tuple<string, int>>();

			headers.Add("Employee Code", new Tuple<string, int>("Employee Code", headers.Count));
			headers.Add("Employee Name", new Tuple<string, int>("Employee Name", headers.Count));
			headers.Add("Branch", new Tuple<string, int>("Branch", headers.Count));
			headers.Add("Department", new Tuple<string, int>("Department", headers.Count));
			headers.Add("Designation", new Tuple<string, int>("Designation", headers.Count));
			headers.Add("Date Of Joining", new Tuple<string, int>("Date Of Joining", headers.Count));
			headers.Add("PAN", new Tuple<string, int>("PAN", headers.Count));
			headers.Add("Salary", new Tuple<string, int>("Salary", headers.Count));
			headers.Add("Salary Earned", new Tuple<string, int>("Salary Earned", headers.Count));
			foreach (EarningComponent item in earnings.OrderBy(x => x.DisplayOrder))
			{
				headers.Add(item.ID.ToString().ToLower(), new Tuple<string, int>(item.Name, headers.Count));
			}
			headers.Add("Incentives", new Tuple<string, int>("Incentives", headers.Count));
			headers.Add("Arrears", new Tuple<string, int>("Arrears", headers.Count));
			headers.Add("Gross Earnings", new Tuple<string, int>("Gross Earnings", headers.Count));

			foreach (DeductionComponent item in deductions)
			{
				headers.Add(item.ID.ToString().ToLower(), new Tuple<string, int>(item.Name, headers.Count));
			}
			headers.Add("LOP", new Tuple<string, int>("LOP", headers.Count));
			headers.Add("Pay Cut", new Tuple<string, int>("Pay Cut", headers.Count));
			headers.Add("ESI", new Tuple<string, int>("ESI", headers.Count));
			headers.Add("EPFNo", new Tuple<string, int>("EPFNo", headers.Count));
			headers.Add("PTAX", new Tuple<string, int>("PTAX", headers.Count));
			headers.Add("Tax", new Tuple<string, int>("Tax", headers.Count));
			headers.Add("Gross Deductions", new Tuple<string, int>("Gross Deductions", headers.Count));
			headers.Add("Net", new Tuple<string, int>("Net", headers.Count));


			if (list.Any())
			{
				foreach (var paySheet in list)
				{
					object[] Columnsfields = new object[headers.Count];
					Columnsfields[headers["Employee Code"].Item2] = paySheet.Employee.No;
					Columnsfields[headers["Employee Name"].Item2] = paySheet.Employee.Name;
					Columnsfields[headers["Branch"].Item2] = "";
					Columnsfields[headers["Department"].Item2] = paySheet.Employee.Department.Name;
					Columnsfields[headers["Designation"].Item2] = paySheet.Employee.Designation.Name;
					Columnsfields[headers["Date Of Joining"].Item2] = paySheet.Employee.DateOfJoining.ToString();
					Columnsfields[headers["PAN"].Item2] = paySheet.Employee.PanNumber;
					Columnsfields[headers["Salary"].Item2] = paySheet.Salary;
					Columnsfields[headers["Salary Earned"].Item2] = paySheet.Gross;

					Columnsfields[headers["Incentives"].Item2] = paySheet.Incentive;
					Columnsfields[headers["Arrears"].Item2] = paySheet.Arrears;
					Columnsfields[headers["Gross Earnings"].Item2] = paySheet.Gross;

					Columnsfields[headers["LOP"].Item2] = paySheet.LOP;
					Columnsfields[headers["Pay Cut"].Item2] = paySheet.PayCut;
					Columnsfields[headers["ESI"].Item2] = paySheet.ESI;
					Columnsfields[headers["EPFNo"].Item2] = paySheet.ESINo;
					Columnsfields[headers["PTAX"].Item2] = paySheet.PTax;
					Columnsfields[headers["Tax"].Item2] = paySheet.Tax;
					Columnsfields[headers["Gross Deductions"].Item2] = paySheet.Deduction;
					Columnsfields[headers["Net"].Item2] = paySheet.Net;

					if (paySheet.Earnings.Any())
					{
						foreach (PaySheetEarning earningitem in paySheet.Earnings)
						{
							Columnsfields[headers[earningitem.ComponentId.ToString().ToLower()].Item2] = earningitem.Earning;
						}
					}
					if (paySheet.Deductions.Any())
					{
						foreach (PaySheetDeduction deduction in paySheet.Deductions)
						{
							Columnsfields[headers[deduction.ComponentId.ToString().ToLower()].Item2] = deduction.Deduction;
						}
					}
					disctionaryList.Add(paySheet.Employee.No, Columnsfields);
				}
			}
			var book = new Workbook(ClosedXmlGeneric.DataExport("Pay_Sheet", headers, disctionaryList));

			book.Settings.Password = password;
			var stream = new MemoryStream();
			book.Save(stream, SaveFormat.Xlsx);
			stream.Seek(0, SeekOrigin.Begin);
			return new DownloadFile(stream, $"Pay_Sheet.xlsx");

		}


		[HttpGet("MyPayMonths")]
		[ApiAuthorize(Core.Permission.SS_SalaryDetails, Core.Privilege.Read)]
		public async Task<IActionResult> GetMyPayMonths()
		{
			return Ok(_mapper.Map<List<EmployeePayMonthModel>>(await _service.GetSalarySlips(LOGIN_USER_EMPId)));
		}
		[HttpGet("DownloadMyPaySlip/{monthId}")]
		[ApiAuthorize(Core.Permission.SS_SalaryDetails, Core.Privilege.Read)]
		public async Task<IActionResult> DownloadMyPaySlip(Guid monthId)
		{
			if (!IS_USER_EMPLOYEE) return BadRequest();
			return await PaySlip(monthId, LOGIN_USER_EMPId);
		}
		//DownloadPaySlip
		[HttpGet("DownloadPaySlip/{monthId}/{id}")]
		[ApiAuthorize(Core.Permission.PS_Payrun, Core.Privilege.Read)]
		public async Task<IActionResult> DownloadPaySlip(Guid monthId, Guid id)
		{
			return await PaySlip(monthId, id);
		}

		private async Task<IActionResult> PaySlip(Guid monthId, Guid employeeId)
		{
			string contentPath = Environment.CurrentDirectory;

			var paySlip = await _service.GetEmployeePay(monthId, employeeId);
			var org = await _orgService.GetOrg();

			PaySlipPdfModel model = _mapper.Map<PaySlipPdfModel>(paySlip);
			List<PaySheetEarning> earnings = paySlip.Earnings.ToList();
			List<PaySheetDeduction> deductions = paySlip.Deductions.ToList();

			if (paySlip.Arrears > 0)
			{
				earnings.Add(new PaySheetEarning { HeaderName = "Arrears", Earning = paySlip.Arrears });
			}
			if (paySlip.Incentive > 0)
			{
				earnings.Add(new PaySheetEarning { HeaderName = "Incentive", Earning = paySlip.Incentive });
			}
			if (paySlip.PayCut > 0)
			{
				deductions.Add(new PaySheetDeduction { HeaderName = "Pay Cut", Deduction = paySlip.PayCut });
			}
			if (paySlip.LOP > 0)
			{
				deductions.Add(new PaySheetDeduction { HeaderName = "LOP", Deduction = paySlip.LOP });
			}
			if (paySlip.LC > 0)
			{
				deductions.Add(new PaySheetDeduction { HeaderName = "Late Coming Deduction", Deduction = paySlip.LC });
			}
			if (paySlip.UA > 0)
			{
				deductions.Add(new PaySheetDeduction { HeaderName = "Unauthorized Salary Deduction", Deduction = paySlip.UA });
			}
			deductions.Add(new PaySheetDeduction { HeaderName = "ESI", Deduction = paySlip.ESI });
			deductions.Add(new PaySheetDeduction { HeaderName = "PF", Deduction = paySlip.EPF });
			deductions.Add(new PaySheetDeduction { HeaderName = "Provisional Tax", Deduction = paySlip.PTax });
			deductions.Add(new PaySheetDeduction { HeaderName = "Income Tax", Deduction = paySlip.Tax });

			if (org != null)
			{
				model.Address = new CompanyAddress()
				{
					Address1 = string.Concat(org.AddressOne, ", ", org.AddressSecond),
					Address2 = string.Concat(org.City, ", ", org.State, ", ", org.Pincode),
				};
			}
			model.Components = new List<PaySlipPdfComponentsModel>();

			for (int i = 0; i < Math.Max(earnings.Count, deductions.Count); i++)
			{
				model.Components.Add(new PaySlipPdfComponentsModel
				{
					Earning = earnings.Count > i ? earnings[i].HeaderName : string.Empty,
					EarningAmount = earnings.Count > i ? earnings[i].Earning.ToString() : string.Empty,
					Deduction = deductions.Count > i ? deductions[i].HeaderName : string.Empty,
					DeductionAmount = deductions.Count > i ? deductions[i].Deduction.ToString() : string.Empty,
				});
			}
			var xmlStream = new MemoryStream();
			var x = new XmlSerializer(model.GetType());
			x.Serialize(xmlStream, model);
			xmlStream.Position = 0;
			var xmlDoc = new XPathDocument(xmlStream);
			XPathNavigator iter = xmlDoc.CreateNavigator();

			var stream = new MemoryStream();
			var transform = new XslTransform();

			transform.Load(Path.Combine(contentPath, "XSLT", "PaySlip.xslt"));
			transform.Transform(iter, null, stream);
			stream.Position = 0;

			try
			{
				using var streampdf = new MemoryStream();
				var writer = new PdfWriter(streampdf);
				var pdf = new PdfDocument(writer);
				var doc = new Document(pdf);

				var converterProperties = new ConverterProperties();
				HtmlConverter.ConvertToPdf(stream, pdf, converterProperties);
				doc.Close();
				//To know HTML content for only testing
				// string a = System.Text.Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length)

				return new DownloadFile(new MemoryStream(streampdf.ToArray()), $"payslip.pdf");
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
		[HttpGet("GetAllMonths")]
		[ApiAuthorize(Core.Permission.PS_Payrun, Core.Privilege.Read)]
		public async Task<IActionResult> GetAllMonths()
		{
			return Ok(_mapper.Map<List<PayMonthModel>>(await _service.GetAllMonths()));
		}


		[HttpGet("AnnualSalary/{fyId}")]
		[ApiAuthorize(Core.Permission.SS_SalaryDetails, Core.Privilege.Read)]
		public async Task<IActionResult> GetAnnualSalary(Guid fyId)
		{
			Guid emp = LOGIN_USER_EMPId;
			return Ok(_mapper.Map<IEnumerable<SalarySummaryModel>>(await _service.GetEmployeeAnnualSalary(emp, fyId)));
		}

		//[HttpGet("PaySheet/{id}/{password}")]
		//public IActionResult GetPaySheet(Guid id, string password)
		//{
		//    List<PaySheet> list = _service.GetPaysheet(id).ToList();
		//    List<SheetModel> model = new();
		//    foreach (var item in list)
		//    {
		//        model.Add(new SheetModel
		//        {
		//            Employee_Code = item.Employee.No,
		//            EmployeeName = item.Employee.Name,
		//            Branch = "",
		//            Department = item.Employee.Department.Name,
		//            Designation = item.Employee.Designation.Name,
		//            DOj = item.Employee.DateOfJoining,
		//            PAN = item.Employee.PanNumber,
		//            Salary = item.Salary,
		//            SalaryEarned = item.Gross,
		//            Basic = 0,
		//            HRA = 0,
		//            Medical = 0,
		//            FoodCoupons = 0,
		//            SpecialAllowance = 0,
		//            Incentives = item.Incentive,
		//            Arrears = item.Arrears,
		//            GrossEarnings = item.Gross,
		//            Lop = item.LOP,
		//            PayCut = item.PayCut,
		//            ESI = item.ESI,
		//            EPFNo = item.EPFNo,
		//            PTax = item.PTax,
		//            Tax = item.Tax,
		//            GrossDeductions = item.Deduction,
		//            Net = item.Net,
		//        });
		//    }
		//    string dataDir = @"G:\F-Drive Data";
		//    // Create directory if it is not already present.
		//    bool IsExists = Directory.Exists(dataDir);
		//    if (!IsExists)
		//        Directory.CreateDirectory(dataDir);

		//    // Instantiate a new Workbook
		//    Workbook book = new Workbook();
		//    Worksheet sheet = book.Worksheets[0];
		//    ImportTableOptions imp = new ImportTableOptions();
		//    imp.InsertRows = true;
		//    string[] headers = { "Employee Code","Employee Name","Branch","Department","Designation","DOj","PAN","Salary","Salary Earned",
		//                        "Basic","HRA","Medical","Food Coupons","Special  Allowance","Incentives","Arrears","Gross Earnings",
		//                        "Lop","Pay Cut","ESI","EPF No","PTax","Tax","Gross Deductions","Net"};
		//    sheet.Cells.ImportCustomObjects(model,
		//            new string[]{ "Employee_Code","EmployeeName","Branch","Department","Designation","DOj","PAN","Salary","SalaryEarned",
		//                        "Basic","HRA","Medical","FoodCoupons","SpecialAllowance","Incentives","Arrears","GrossEarnings",
		//                        "Lop","PayCut","ESI","EPFNo","PTax","Tax","GrossDeductions","Net"},
		//            true,
		//            0,
		//            0,
		//            list.Count,
		//            true,
		//            "dd/mm/yyyy",
		//             false);
		//    // Auto-fit all the columns
		//    book.Worksheets[0].AutoFitColumns();

		//    //Password protect the file.
		//    book.Settings.Password = password;
		//    // Save the Excel file
		//    book.Save(dataDir + "PaySheet.xlsx");
		//    return Ok();
		//}
	}
}
