using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Enums;
using TranSmart.Service.Leave;

namespace TranSmart.Service.PayRoll
{
	public interface IAppendToExcelService : IBaseService<ApplyLeave>
	{
		Task<Stream> AppendLeaveDetailsToExcel(Guid empId);
		Task<Stream> AppendWeeklyHolidaysDetailsToExcel();
		Task<Stream> AppendPaySheetToExcel(Guid payMonthId);
		Task<Stream> RegisterOfEmployment();
		Task<Stream> WageRegisterforEqualRemuneration(Guid payMonthId);
		Task<Stream> PFUploadFormat(Guid payMonthId);
		Task<Stream> ESITemplate(Guid payMonthId);
		Task<Stream> AppendBankSheetToExcel(Guid payMonthId);
	}
	public class AppendToExcelService : BaseService<ApplyLeave>, IAppendToExcelService
	{
		public AppendToExcelService(IUnitOfWork uow) : base(uow)
		{
		}
		public async Task<Stream> AppendLeaveDetailsToExcel(Guid empId)
		{
			var leaveBalances = await UOW.GetRepositoryAsync<LeaveBalance>().GetAsync(
									predicate: p => !p.LeaveType.DefaultPayoff, 
									include: i => i.Include(x => x.LeaveType));

			var leaveRequests = await UOW.GetRepositoryAsync<ApplyLeave>().GetAsync();

			XLWorkbook Workbook = new XLWorkbook(@"C:\Sheet\Register_OfLeaves.xlsx");
			IXLWorksheet Worksheet = Workbook.Worksheet("Register of Leave");

			var lvRequestLvType = await UOW.GetRepositoryAsync<ApplyLeaveType>().GetAsync(include: i => i.Include(x => x.ApplyLeave).Include(x => x.LeaveType));
			int applySheetRowNo = 12, approveSheetRowNo = 12, rejectSheetRowNo = 12, typeCount = 0, NumberOfLastRow = 0;
			foreach (var item in lvRequestLvType.Where(x => x.ApplyLeave.EmployeeId == empId).GroupBy(x => new { x.LeaveTypeId }).Select(x => x.Key))
			{
				typeCount++;
				var availableLeaves = leaveBalances.Where(x => x.LeaveTypeId == item.LeaveTypeId && x.EmployeeId == empId).Sum(x => x.Leaves);
				var aplLeave = lvRequestLvType.Where(x => x.ApplyLeave.EmployeeId == empId && x.ApplyLeave.Status == (byte)ApplyLeaveSts.Applied && x.LeaveTypeId == item.LeaveTypeId).ToList();
				var approvedLeaves = lvRequestLvType.Where(x => x.ApplyLeave.EmployeeId == empId && x.ApplyLeave.Status == (byte)ApplyLeaveSts.Approved && x.LeaveTypeId == item.LeaveTypeId).ToList();
				var rejectedLeaves = lvRequestLvType.Where(x => x.ApplyLeave.EmployeeId == empId && x.ApplyLeave.Status == (byte)ApplyLeaveSts.Rejected && x.LeaveTypeId == item.LeaveTypeId).ToList();
				if (typeCount > 1)
				{
					Worksheet.Cell($"G{NumberOfLastRow + 1}").Value = lvRequestLvType.FirstOrDefault(x => x.LeaveTypeId == item.LeaveTypeId)?.LeaveType.Name;
					Worksheet.Cell($"G{NumberOfLastRow + 1}").Style.Font.FontColor = XLColor.Red;
				}
				for (int i = 0; i < aplLeave.Count; i++)
				{
					//var approvedCount = leaveBalances.Single()
					Worksheet.Cell(applySheetRowNo, 2).SetValue(aplLeave[i].ApplyLeave.FromDate.Date.ToString("MM/dd/yyyy"));
					Worksheet.Cell(applySheetRowNo, 3).SetValue(aplLeave[i].ApplyLeave.ToDate.Date.ToString("MM/dd/yyyy"));
					Worksheet.Cell(applySheetRowNo, 4).Value = aplLeave[i].ApplyLeave.NoOfLeaves;
					applySheetRowNo++;
				}
				for (int i = 0; i < approvedLeaves.Count; i++)
				{
					Worksheet.Cell(approveSheetRowNo, 6).SetValue(approvedLeaves[i].ApplyLeave.FromDate.Date.ToString("MM/dd/yyyy"));
					Worksheet.Cell(approveSheetRowNo, 7).SetValue(approvedLeaves[i].ApplyLeave.ToDate.Date.ToString("MM/dd/yyyy"));
					Worksheet.Cell(approveSheetRowNo, 8).Value = approvedLeaves[i].ApplyLeave.NoOfLeaves;
					Worksheet.Cell(approveSheetRowNo, 9).Value = availableLeaves;
					approveSheetRowNo++;
				}
				for (int i = 0; i < rejectedLeaves.Count; i++)
				{
					Worksheet.Cell(rejectSheetRowNo, 10).SetValue(rejectedLeaves[i].ApplyLeave.FromDate.Date.ToString("MM/dd/yyyy"));
					Worksheet.Cell(rejectSheetRowNo, 11).SetValue(rejectedLeaves[i].ApplyLeave.ToDate.Date.ToString("MM/dd/yyyy"));
					Worksheet.Cell(rejectSheetRowNo, 12).Value = rejectedLeaves[i].ApplyLeave.NoOfLeaves;
					Worksheet.Cell(rejectSheetRowNo, 13).Value = rejectedLeaves[i].ApplyLeave.RejectReason;
					rejectSheetRowNo++;
				}
				NumberOfLastRow = Worksheet.LastRowUsed().RowNumber();
				if (typeCount == 1)
				{//with typeCount condition(1 or >1) we are achiving LeaveTypeName first and below related Leaves
					Worksheet.Range($"A{11}:f{11}").Merge();
					Worksheet.Range($"G{11}:o{11}").Merge();
					Worksheet.Cell($"G{11}").Value = lvRequestLvType.FirstOrDefault(x => x.LeaveTypeId == item.LeaveTypeId).LeaveType.Name;
					Worksheet.Cell($"G{11}").Style.Font.FontColor = XLColor.Red;
				}
				Worksheet.Range($"A{NumberOfLastRow + 1}:f{NumberOfLastRow + 1}").Merge();
				Worksheet.Range($"G{NumberOfLastRow + 1}:o{NumberOfLastRow + 1}").Merge();

				applySheetRowNo = NumberOfLastRow + 2; approveSheetRowNo = NumberOfLastRow + 2; rejectSheetRowNo = NumberOfLastRow + 2;
			}
			Stream excelStream = new MemoryStream();
			Workbook.SaveAs(excelStream);
			excelStream.Position = 0;
			return excelStream;
		}
		public async Task<Stream> AppendWeeklyHolidaysDetailsToExcel()
		{
			XLWorkbook Workbook = new XLWorkbook(@"C:\Sheet\weeklyHolidays.xlsx");
			IXLWorksheet Worksheet = Workbook.Worksheet("Sheet1");
			var employees = await UOW.GetRepositoryAsync<Employee>().GetAsync(include: i => i.Include(x => x.Department).ThenInclude(x => x.WeekOffSetup));
			int NumberOfLastRow = 5, siNo = 0;
			foreach (var item in employees)
			{
				siNo++;
				Worksheet.Cell(NumberOfLastRow, 1).Value = siNo;
				Worksheet.Cell(NumberOfLastRow, 2).Value = item.Name;
				Worksheet.Cell(NumberOfLastRow, 3).Value = item.Department.WeekOffSetup != null ? item.Department.WeekOffSetup.Name : "-";
				Worksheet.Cell(NumberOfLastRow, 4).Value = "Remarks";
				NumberOfLastRow++;
			}
			Stream excelStream = new MemoryStream();
			Workbook.SaveAs(excelStream);
			excelStream.Position = 0;
			return excelStream;
		}


		public async Task<Stream> AppendPaySheetToExcel(Guid payMonthId)
		{
			XLWorkbook Workbook = new XLWorkbook(@"C:\Sheet\AppendPayseet.xlsx");
			IXLWorksheet Worksheet = Workbook.Worksheet("June 2022");
			int NumberOfLastRow = Worksheet.LastRowUsed().RowNumber();
			int sheetRowNo = NumberOfLastRow + 1;
			int sNo = 1;
			var employeeDetails = await UOW.GetRepositoryAsync<Employee>().GetAsync(
				include: i => i.Include(x => x.Designation));
			foreach (Employee emp in employeeDetails)
			{
				var salary = await UOW.GetRepositoryAsync<Salary>().SingleAsync(x => x.EmployeeId == emp.ID);
				var paySheet = await UOW.GetRepositoryAsync<PaySheet>().SingleAsync(x => x.EmployeeID == emp.ID && x.PayMonthId == payMonthId,
										include: i => i.Include(x => x.Deductions));
				var deductions = 0;
				if (salary != null)
				{
					var paySheetDeductions = await UOW.GetRepositoryAsync<PaySheetDeduction>().GetAsync();
					deductions = paySheetDeductions.Where(x => x.PaySheetId == paySheet.ID).Sum(x => x.Deduction);
					var salaryEarning = await UOW.GetRepositoryAsync<SalaryEarning>().GetAsync(x => x.SalaryId == salary.ID,
												include: i => i.Include(x => x.Component));

					Worksheet.Cell(sheetRowNo, 1).Value = sNo;
					Worksheet.Cell(sheetRowNo, 2).Value = emp.Name;
					Worksheet.Cell(sheetRowNo, 3).Value = emp.Designation.Name;
					Worksheet.Cell(sheetRowNo, 4).Value = salaryEarning.FirstOrDefault(x => x.Component.EarningType == 1)?.Monthly;
					Worksheet.Cell(sheetRowNo, 5).Value = salaryEarning.FirstOrDefault(x => x.Component.EarningType == 2)?.Monthly;
					Worksheet.Cell(sheetRowNo, 6).Value = salaryEarning.FirstOrDefault(x => x.Component.EarningType == 10)?.Monthly;
					Worksheet.Cell(sheetRowNo, 7).Value = salaryEarning.FirstOrDefault(x => x.Component.EarningType == 9)?.Monthly;
					Worksheet.Cell(sheetRowNo, 8).Value = salaryEarning.FirstOrDefault(x => x.Component.EarningType == 0)?.Monthly;
					Worksheet.Cell(sheetRowNo, 9).Value = Convert.ToInt32(Worksheet.Cell(sheetRowNo, 4).Value) + Convert.ToInt32(Worksheet.Cell(sheetRowNo, 5).Value)
															+ Convert.ToInt32(Worksheet.Cell(sheetRowNo, 6).Value) + Convert.ToInt32(Worksheet.Cell(sheetRowNo, 7).Value)
															+ Convert.ToInt32(Worksheet.Cell(sheetRowNo, 8).Value);
					Worksheet.Cell(sheetRowNo, 10).Value = 0;
					Worksheet.Cell(sheetRowNo, 11).Value = 0;
					Worksheet.Cell(sheetRowNo, 12).Value = 0;
					Worksheet.Cell(sheetRowNo, 13).Value = Convert.ToInt32(Worksheet.Cell(sheetRowNo, 10).Value) + Convert.ToInt32(Worksheet.Cell(sheetRowNo, 11).Value)
															+ Convert.ToInt32(Worksheet.Cell(sheetRowNo, 12).Value);
					Worksheet.Cell(sheetRowNo, 14).Value = paySheet.ESI;
					Worksheet.Cell(sheetRowNo, 15).Value = paySheet.EPF;
					Worksheet.Cell(sheetRowNo, 16).Value = paySheet.PTax;
					Worksheet.Cell(sheetRowNo, 17).Value = paySheet.Tax;
					Worksheet.Cell(sheetRowNo, 18).Value = Convert.ToInt32(Worksheet.Cell(sheetRowNo, 14).Value) + Convert.ToInt32(Worksheet.Cell(sheetRowNo, 15).Value)
															+ Convert.ToInt32(Worksheet.Cell(sheetRowNo, 16).Value) + Convert.ToInt32(Worksheet.Cell(sheetRowNo, 17).Value);
					Worksheet.Cell(sheetRowNo, 19).Value = 0;
					Worksheet.Cell(sheetRowNo, 20).Value = paySheet.LOP;
					Worksheet.Cell(sheetRowNo, 21).Value = deductions;
					Worksheet.Cell(sheetRowNo, 22).Value = Convert.ToInt32(Worksheet.Cell(sheetRowNo, 19).Value) + Convert.ToInt32(Worksheet.Cell(sheetRowNo, 20).Value)
															+ Convert.ToInt32(Worksheet.Cell(sheetRowNo, 21).Value); ;
					Worksheet.Cell(sheetRowNo, 23).Value = Convert.ToInt32(Worksheet.Cell(sheetRowNo, 9).Value) + Convert.ToInt32(Worksheet.Cell(sheetRowNo, 13).Value)
															- Convert.ToInt32(Worksheet.Cell(sheetRowNo, 18).Value)
															- Convert.ToInt32(Worksheet.Cell(sheetRowNo, 22).Value);
					Worksheet.Cell(sheetRowNo, 24).Value = paySheet.AddedAt.Date.ToString("dd/MM/yyyy");
					sheetRowNo++;
					sNo++;
				}
			}
			Stream excelStream = new MemoryStream();
			Workbook.SaveAs(excelStream);
			excelStream.Position = 0;
			return excelStream;
		}
		public async Task<Stream> PFUploadFormat(Guid payMonthId)
		{
			XLWorkbook workbook = new XLWorkbook(@"C:\Sheet\PF.xlsx");
			IXLWorksheet ws = workbook.Worksheet("sheet1");
			int lastRow = ws.LastRowUsed().RowNumber();
			int sheetRowNum = lastRow + 1;
			var statutory = await UOW.GetRepositoryAsync<EmpStatutory>().GetAsync();
			foreach (var item in statutory)
			{
				var paySheet = await UOW.GetRepositoryAsync<PaySheet>().SingleAsync(predicate: p => p.EmployeeID == item.EmpId
									&& p.PayMonthId == payMonthId, include: i => i.Include(x => x.Employee));
				var EPFConts = paySheet.Gross > 30000 ? ((paySheet.EPFGross * 12 / 100) + 5000) : (paySheet.EPFGross * 12 / 100);

				ws.Cell(sheetRowNum, 1).Value = item.UAN;
				ws.Cell(sheetRowNum, 2).Value = paySheet.Employee.Name;
				ws.Cell(sheetRowNum, 3).Value = paySheet.Gross;
				ws.Cell(sheetRowNum, 4).Value = paySheet.EPFGross;
				ws.Cell(sheetRowNum, 5).Value = 0;
				ws.Cell(sheetRowNum, 6).Value = paySheet.EPFGross;
				ws.Cell(sheetRowNum, 7).Value = EPFConts;
				ws.Cell(sheetRowNum, 8).Value = 0;
				ws.Cell(sheetRowNum, 9).Value = EPFConts - 0;
				ws.Cell(sheetRowNum, 10).Value = paySheet.LOPDays;
				ws.Cell(sheetRowNum, 12).Value = "Remarks";
				sheetRowNum++;
			}
			Stream excelStream = new MemoryStream();
			workbook.SaveAs(excelStream);
			excelStream.Position = 0;
			return excelStream;
		}

		public async Task<Stream> ESITemplate(Guid payMonthId)
		{
			XLWorkbook workbook = new XLWorkbook(@"C:\Sheet\ESI_Template.xlsx");
			IXLWorksheet ws = workbook.Worksheet("sheet1");
			int lastRow = ws.LastRowUsed().RowNumber();
			int sheetRowNum = lastRow + 1;
			var statutory = await UOW.GetRepositoryAsync<EmpStatutory>().GetAsync();
			foreach (var item in statutory)
			{
				var paySheet = await UOW.GetRepositoryAsync<PaySheet>().SingleAsync(predicate: p => p.EmployeeID == item.EmpId
									&& p.PayMonthId == payMonthId, include: i => i.Include(x => x.Employee));
				var EPFConts = paySheet.Gross > 30000 ? ((paySheet.EPFGross * 12 / 100) + 5000) : (paySheet.EPFGross * 12 / 100);

				ws.Cell(sheetRowNum, 1).Value = item.ESINo;
				ws.Cell(sheetRowNum, 2).Value = paySheet.Employee.Name;
				ws.Cell(sheetRowNum, 3).Value = paySheet.PresentDays;
				ws.Cell(sheetRowNum, 4).Value = paySheet.Salary;
				ws.Cell(sheetRowNum, 5).Value = "Reason";
				ws.Cell(sheetRowNum, 6).Value = "";
				sheetRowNum++;
			}
			Stream excelStream = new MemoryStream();
			workbook.SaveAs(excelStream);
			excelStream.Position = 0;
			return excelStream;
		}
		public async Task<Stream> RegisterOfEmployment()
		{
			XLWorkbook Workbook = new XLWorkbook(@"C:\Sheet\AppendRegisterofEmployment.xlsx");
			IXLWorksheet Worksheet = Workbook.Worksheet("Attendance-June2022-Hyd");
			int NumberOfLastRow = Worksheet.LastRowUsed().RowNumber();
			int sheetRowNo = NumberOfLastRow + 1;
			int sNo = 1;
			var employeeDetails = await UOW.GetRepositoryAsync<Employee>().GetAsync(
				include: i => i.Include(x => x.Designation));
			foreach (Employee emp in employeeDetails)
			{
				Worksheet.Cell(sheetRowNo, 1).Value = sNo;
				Worksheet.Cell(sheetRowNo, 2).Value = emp.Name;
				Worksheet.Cell(sheetRowNo, 3).Value = emp.No;
				Worksheet.Cell(sheetRowNo, 4).Value = emp.Gender == 1 ? "Male" : "Female";
				Worksheet.Cell(sheetRowNo, 5).Value = (int)((DateTime.Now - emp.DateOfBirth).TotalDays / 365.242199);
				Worksheet.Cell(sheetRowNo, 6).Value = 0;
				Worksheet.Cell(sheetRowNo, 7).Value = 0;
				Worksheet.Cell(sheetRowNo, 8).Value = 0;
				Worksheet.Cell(sheetRowNo, 9).Value = 0;
				Worksheet.Cell(sheetRowNo, 10).Value = 0;
				Worksheet.Cell(sheetRowNo, 11).Value = 0;
				Worksheet.Cell(sheetRowNo, 12).Value = 0;
				Worksheet.Cell(sheetRowNo, 13).Value = 0;
				Worksheet.Cell(sheetRowNo, 14).Value = 0;
				sheetRowNo++;
				sNo++;
			}
			Stream excelStream = new MemoryStream();
			Workbook.SaveAs(excelStream);
			excelStream.Position = 0;
			return excelStream;
		}
		public async Task<Stream> WageRegisterforEqualRemuneration(Guid payMonthId)
		{
			XLWorkbook Workbook = new XLWorkbook(@"C:\Sheet\AppendWageRegisterforEqualRemuneration.xlsx");
			IXLWorksheet Worksheet = Workbook.Worksheet("Register of Wages");
			int NumberOfLastRow = Worksheet.LastRowUsed().RowNumber();
			int sheetRowNo = NumberOfLastRow + 1;
			int sNo = 1;
			var employeeDetails = await UOW.GetRepositoryAsync<Employee>().GetAsync(
				include: i => i.Include(x => x.Designation));
			foreach (Employee emp in employeeDetails)
			{
				var salary = await UOW.GetRepositoryAsync<Salary>().SingleAsync(x => x.EmployeeId == emp.ID);
				var paySheet = await UOW.GetRepositoryAsync<PaySheet>().SingleAsync(x => x.EmployeeID == emp.ID && x.PayMonthId == payMonthId,
										include: i => i.Include(x => x.Deductions));
				if (salary != null)
				{
					var salaryEarning = await UOW.GetRepositoryAsync<SalaryEarning>().GetAsync(x => x.SalaryId == salary.ID,
												include: i => i.Include(x => x.Component));

					Worksheet.Cell(sheetRowNo, 1).Value = sNo;
					Worksheet.Cell(sheetRowNo, 2).Value = emp.Name;
					Worksheet.Cell(sheetRowNo, 3).Value = 0;
					Worksheet.Cell(sheetRowNo, 4).Value = paySheet.PresentDays;
					Worksheet.Cell(sheetRowNo, 5).Value = 0;
					Worksheet.Cell(sheetRowNo, 6).Value = salaryEarning.FirstOrDefault(x => x.Component.EarningType == 1)?.Monthly;
					Worksheet.Cell(sheetRowNo, 7).Value = 0;
					Worksheet.Cell(sheetRowNo, 8).Value = 0;
					Worksheet.Cell(sheetRowNo, 9).Value = 0;
					Worksheet.Cell(sheetRowNo, 10).Value = salaryEarning.FirstOrDefault(x => x.Component.EarningType == 2)?.Monthly;
					Worksheet.Cell(sheetRowNo, 11).Value = 0;
					Worksheet.Cell(sheetRowNo, 12).Value = Convert.ToInt32(Worksheet.Cell(sheetRowNo, 6).Value) + Convert.ToInt32(Worksheet.Cell(sheetRowNo, 7).Value)
															+ Convert.ToInt32(Worksheet.Cell(sheetRowNo, 8).Value) + Convert.ToInt32(Worksheet.Cell(sheetRowNo, 9).Value)
															+ Convert.ToInt32(Worksheet.Cell(sheetRowNo, 10).Value) + Convert.ToInt32(Worksheet.Cell(sheetRowNo, 11).Value);
					Worksheet.Cell(sheetRowNo, 13).Value = paySheet.EPF;
					Worksheet.Cell(sheetRowNo, 14).Value = paySheet.ESI;
					Worksheet.Cell(sheetRowNo, 15).Value = 0;
					Worksheet.Cell(sheetRowNo, 16).Value = paySheet.Tax;
					Worksheet.Cell(sheetRowNo, 17).Value = 0;
					Worksheet.Cell(sheetRowNo, 18).Value = 0;
					Worksheet.Cell(sheetRowNo, 19).Value = 0;
					Worksheet.Cell(sheetRowNo, 20).Value = Convert.ToInt32(Worksheet.Cell(sheetRowNo, 13).Value) + Convert.ToInt32(Worksheet.Cell(sheetRowNo, 14).Value.ToString())
															+ Convert.ToInt32(Worksheet.Cell(sheetRowNo, 15).Value) + Convert.ToInt32(Worksheet.Cell(sheetRowNo, 16).Value.ToString())
															+ Convert.ToInt32(Worksheet.Cell(sheetRowNo, 17).Value) + Convert.ToInt32(Worksheet.Cell(sheetRowNo, 18).Value.ToString())
															+ Convert.ToInt32(Worksheet.Cell(sheetRowNo, 19).Value);
					Worksheet.Cell(sheetRowNo, 21).Value = Convert.ToInt32(Worksheet.Cell(sheetRowNo, 12).Value)
															- Convert.ToInt32(Worksheet.Cell(sheetRowNo, 20).Value);
					Worksheet.Cell(sheetRowNo, 22).Value = 0;
					Worksheet.Cell(sheetRowNo, 23).Value = 0;
					Worksheet.Cell(sheetRowNo, 24).Value = paySheet.AddedAt.Date.ToString("dd/MM/yyyy");
					Worksheet.Cell(sheetRowNo, 25).Value = 0;
					sheetRowNo++;
					sNo++;
				}
			}
			Stream excelStream = new MemoryStream();
			Workbook.SaveAs(excelStream);
			excelStream.Position = 0;
			return excelStream;
		}
		public async Task<Stream> AppendBankSheetToExcel(Guid payMonthId)
		{
			XLWorkbook workbook = new XLWorkbook(@"C:\Sheet\Bank_Sheet.xlsx");

			//Master Sheet
			IXLWorksheet masterWorksheet = workbook.Worksheet("Master");
			int masterNumberOfLastRow = masterWorksheet.LastRowUsed().RowNumber();
			int masterSheetRowNo = masterNumberOfLastRow + 1;
			int masterSNo = 1;
			int totalAmount = 0, IdfcAmount = 0, otherAmount = 0, holdAmount = 0;
			var employeeDetails = await UOW.GetRepositoryAsync<Employee>().GetAsync(
				include: i => i.Include(x => x.Designation));
			foreach (Employee emp in employeeDetails)
			{
				var payInfo = await UOW.GetRepositoryAsync<EmployeePayInfo>().SingleAsync(x => x.EmployeeId == emp.ID,
				include: i => i.Include(x => x.Bank));
				var paySheet = await UOW.GetRepositoryAsync<PaySheet>().SingleAsync(x => x.EmployeeID == emp.ID && x.PayMonthId == payMonthId);
				if (payInfo != null)
				{
					masterWorksheet.Cell(masterSheetRowNo, 1).Value = masterSNo;
					masterWorksheet.Cell(masterSheetRowNo, 2).Value = emp.Name;
					masterWorksheet.Cell(masterSheetRowNo, 3).Value = emp.Designation.Name;
					masterWorksheet.Cell(masterSheetRowNo, 4).Value = paySheet.Net;
					masterWorksheet.Cell(masterSheetRowNo, 5).Value = payInfo.AccountNo != "" ? payInfo.AccountNo : "-";
					masterWorksheet.Cell(masterSheetRowNo, 6).Value = payInfo.Bank.Name;
					masterWorksheet.Cell(masterSheetRowNo, 7).Value = payInfo.Bank.IFSCCode;
					masterSheetRowNo++;
					masterSNo++;
					totalAmount += paySheet.Net;
					if (paySheet.Hold)
					{
						holdAmount += paySheet.Net;
					}
					else if (payInfo.PayMode == 1)
					{
						IdfcAmount += paySheet.Net;
					}
					else
					{
						otherAmount += paySheet.Net;
					}
				}

			}
			masterNumberOfLastRow = masterWorksheet.LastRowUsed().RowNumber();
			masterWorksheet.Range($"A{masterNumberOfLastRow + 1}:B{masterNumberOfLastRow + 6}").Merge();
			masterWorksheet.Cell(masterSheetRowNo, 3).Value = "Grand Total";
			masterWorksheet.Cell(masterSheetRowNo, 4).Value = totalAmount;
			masterWorksheet.Cell(masterSheetRowNo + 2, 3).Value = "IDFC";
			masterWorksheet.Cell(masterSheetRowNo + 2, 4).Value = IdfcAmount;
			masterWorksheet.Cell(masterSheetRowNo + 3, 3).Value = "Other";
			masterWorksheet.Cell(masterSheetRowNo + 3, 4).Value = otherAmount;
			masterWorksheet.Cell(masterSheetRowNo + 4, 3).Value = "Hold";
			masterWorksheet.Cell(masterSheetRowNo + 4, 4).Value = holdAmount;
			masterWorksheet.Cell(masterSheetRowNo + 5, 3).Value = "Total";
			masterWorksheet.Cell(masterSheetRowNo + 5, 4).Value = holdAmount + IdfcAmount + otherAmount;

			//IDFC Bank Sheet
			IXLWorksheet IDFCWorksheet = workbook.Worksheet("IDFC");
			int IDFCNumberOfLastRow = IDFCWorksheet.LastRowUsed().RowNumber();
			int IDFCsheetRowNo = IDFCNumberOfLastRow + 1;
			int IDFCSNo = 1, IDFCBankTotal = 0;
			foreach (Employee emp in employeeDetails)
			{
				var payInfo = await UOW.GetRepositoryAsync<EmployeePayInfo>().SingleAsync(x => x.EmployeeId == emp.ID && x.PayMode == 1,
					include: i => i.Include(x => x.Bank));
				var paySheet = await UOW.GetRepositoryAsync<PaySheet>().SingleAsync(x => x.EmployeeID == emp.ID && x.PayMonthId == payMonthId);
				if (payInfo != null)
				{
					IDFCWorksheet.Cell(IDFCsheetRowNo, 1).Value = IDFCSNo;
					IDFCWorksheet.Cell(IDFCsheetRowNo, 2).Value = emp.Name;
					IDFCWorksheet.Cell(IDFCsheetRowNo, 3).Value = emp.Designation.Name;
					IDFCWorksheet.Cell(IDFCsheetRowNo, 4).Value = paySheet.Net;
					IDFCWorksheet.Cell(IDFCsheetRowNo, 5).Value = payInfo.AccountNo;
					IDFCWorksheet.Cell(IDFCsheetRowNo, 6).Value = payInfo.Bank.IFSCCode;
					IDFCsheetRowNo++;
					IDFCSNo++;
					IDFCBankTotal += paySheet.Net;
				}

			}
			IDFCNumberOfLastRow = IDFCWorksheet.LastRowUsed().RowNumber();
			IDFCWorksheet.Range($"A{IDFCNumberOfLastRow + 1}:B{IDFCNumberOfLastRow + 1}").Merge();
			IDFCWorksheet.Cell(IDFCsheetRowNo, 3).Value = "Total";
			IDFCWorksheet.Cell(IDFCsheetRowNo, 4).Value = IDFCBankTotal;

			//OTHER BANK Sheet
			IXLWorksheet otherBankWorksheet = workbook.Worksheet("OTHER BANK");
			int otherBankNumberOfLastRow = otherBankWorksheet.LastRowUsed().RowNumber();
			int otherBanksheetRowNo = otherBankNumberOfLastRow + 1;
			int otherBankSNo = 1, otherBankTotal = 0;
			foreach (Employee emp in employeeDetails)
			{
				var payInfo = await UOW.GetRepositoryAsync<EmployeePayInfo>().SingleAsync(x => x.EmployeeId == emp.ID && x.PayMode == 3);
				var paySheet = await UOW.GetRepositoryAsync<PaySheet>().SingleAsync(x => x.EmployeeID == emp.ID && x.PayMonthId == payMonthId);
				if (payInfo != null)
				{
					otherBankWorksheet.Cell(otherBanksheetRowNo, 1).Value = otherBankSNo;
					otherBankWorksheet.Cell(otherBanksheetRowNo, 2).Value = emp.Name;
					otherBankWorksheet.Cell(otherBanksheetRowNo, 3).Value = emp.Designation.Name;
					otherBankWorksheet.Cell(otherBanksheetRowNo, 4).Value = paySheet.Net;
					otherBankWorksheet.Cell(otherBanksheetRowNo, 5).Value = payInfo.AccountNo != "" ? payInfo.AccountNo : "-";
					otherBankWorksheet.Cell(otherBanksheetRowNo, 6).Value = payInfo.IFSCCode != "" ? payInfo.IFSCCode : "-";
					otherBanksheetRowNo++;
					otherBankSNo++;
					otherBankTotal += paySheet.Net;
				}

			}
			otherBankNumberOfLastRow = otherBankWorksheet.LastRowUsed().RowNumber();
			otherBankWorksheet.Range($"A{otherBankNumberOfLastRow + 1}:B{otherBankNumberOfLastRow + 1}").Merge();
			otherBankWorksheet.Cell(otherBanksheetRowNo, 3).Value = "Total";
			otherBankWorksheet.Cell(otherBanksheetRowNo, 4).Value = otherBankTotal;

			//Cheque sheet
			IXLWorksheet chequeWorksheet = workbook.Worksheet("APPLIED");
			int chequeNumberOfLastRow = chequeWorksheet.LastRowUsed().RowNumber();
			int chequeSheetRowNo = chequeNumberOfLastRow + 1;
			int chequeSNo = 1, chequeTotal = 0;
			foreach (Employee emp in employeeDetails)
			{
				var payInfo = await UOW.GetRepositoryAsync<EmployeePayInfo>().SingleAsync(x => x.EmployeeId == emp.ID && x.PayMode == 2);
				var paySheet = await UOW.GetRepositoryAsync<PaySheet>().SingleAsync(x => x.EmployeeID == emp.ID && x.PayMonthId == payMonthId && !x.Hold);
				if (payInfo != null && paySheet != null)
				{
					chequeWorksheet.Cell(chequeSheetRowNo, 1).Value = chequeSNo;
					chequeWorksheet.Cell(chequeSheetRowNo, 2).Value = emp.Name;
					chequeWorksheet.Cell(chequeSheetRowNo, 3).Value = emp.Designation.Name;
					chequeWorksheet.Cell(chequeSheetRowNo, 4).Value = paySheet.Net;
					//chequeWorksheet.Cell(chequeSheetRowNo, 5).Value = payInfo.ChequeNo;
					chequeSheetRowNo++;
					chequeSNo++;
					chequeTotal += paySheet.Net;
				}

			}
			chequeNumberOfLastRow = chequeWorksheet.LastRowUsed().RowNumber();
			chequeWorksheet.Range($"A{chequeNumberOfLastRow + 1}:B{chequeNumberOfLastRow + 1}").Merge();
			chequeWorksheet.Cell(chequeSheetRowNo, 3).Value = "Total";
			chequeWorksheet.Cell(chequeSheetRowNo, 4).Value = chequeTotal;

			//Salary Hold Sheet
			IXLWorksheet holdWorksheet = workbook.Worksheet("HOLD");
			int holdNumberOfLastRow = holdWorksheet.LastRowUsed().RowNumber();
			int holdSheetRowNo = holdNumberOfLastRow + 1;
			int holdSNo = 1, holdTotal = 0;
			foreach (Employee emp in employeeDetails)
			{
				var payInfo = await UOW.GetRepositoryAsync<EmployeePayInfo>().SingleAsync(x => x.EmployeeId == emp.ID);
				var paySheet = await UOW.GetRepositoryAsync<PaySheet>().SingleAsync(x => x.EmployeeID == emp.ID && x.PayMonthId == payMonthId && x.Hold);

				if (paySheet != null && payInfo != null)
				{

					holdWorksheet.Cell(holdSheetRowNo, 1).Value = holdSNo;
					holdWorksheet.Cell(holdSheetRowNo, 2).Value = emp.Name;
					holdWorksheet.Cell(holdSheetRowNo, 3).Value = emp.Designation.Name;
					holdWorksheet.Cell(holdSheetRowNo, 4).Value = paySheet.Net;
					holdWorksheet.Cell(holdSheetRowNo, 5).Value = payInfo.AccountNo != "" ? payInfo.AccountNo : "-";
					holdWorksheet.Cell(holdSheetRowNo, 6).Value = payInfo.IFSCCode != "" ? payInfo.IFSCCode : "-";
					holdSheetRowNo++;
					holdSNo++;
					holdTotal += paySheet.Net;
				}

			}
			holdNumberOfLastRow = holdWorksheet.LastRowUsed().RowNumber();
			holdWorksheet.Range($"A{holdNumberOfLastRow + 1}:B{holdNumberOfLastRow + 1}").Merge();
			holdWorksheet.Cell(holdSheetRowNo, 3).Value = "Total";
			holdWorksheet.Cell(holdSheetRowNo, 4).Value = holdTotal;

			Stream excelStream = new MemoryStream();
			workbook.SaveAs(excelStream);
			excelStream.Position = 0;
			return excelStream;
		}
	}
}
