using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Enums;


namespace TranSmart.Service.Payroll
{
	public interface IPayRollService : IBaseService<PaySheet>
	{
		Task<Result<PaySheet>> Process(Guid Id);
		Task<Result<PaySheet>> Release(Guid Id);
		Task<Result<PaySheet>> Hold(Guid Id);
		Task<Result<PaySheet>> Delete(Guid Id);
		decimal ActiveEmployeeWorkdays(decimal monthDays, DateTime startDate, DateTime endDate, DateTime dateOfJoining);
		decimal ResignedEmployeeWorkdays(decimal monthDays, DateTime monthStart, DateTime monthEnd, DateTime dateOfJoining, DateTime lastWorkingDate);
	}
	public class PayRollService : BaseService<PaySheet>, IPayRollService
	{
		public PayRollService(IUnitOfWork uow) : base(uow)
		{

		}
		public async Task<Result<PaySheet>> Release(Guid Id)
		{
			var result = new Result<PaySheet>();
			PayMonth month = await UOW.GetRepositoryAsync<PayMonth>().SingleAsync(x => x.ID == Id);
			if (month == null)
			{
				result.IsSuccess = false;
				result.AddMessageItem(new MessageItem("Selected month is not a valid", FeedbackType.Error));
			}
			else if (month.Status == (int)PayMonthStatus.Released)
			{
				result.IsSuccess = false;
				result.AddMessageItem(new MessageItem("Selected pay month is already released.", FeedbackType.Error));
			}

			else if (month.Status != (int)PayMonthStatus.InProcess)
			{
				result.IsSuccess = false;
				result.AddMessageItem(new MessageItem("Please process pay month before release.", FeedbackType.Error));
			}
			if (result.HasNoError)
			{
				month.Status = (int)PayMonthStatus.Released;
				UOW.GetRepositoryAsync<PayMonth>().UpdateAsync(month);
				PayMonth nextMonth = await UOW.GetRepositoryAsync<PayMonth>().SingleAsync(x => x.Start >= month.End,
					orderBy: o => o.OrderBy(x => x.Start));
				if (nextMonth != null)
				{
					nextMonth.Status = (int)PayMonthStatus.Open;
					UOW.GetRepositoryAsync<PayMonth>().UpdateAsync(nextMonth);
				}
				try
				{
					await UOW.SaveChangesAsync();
					result.IsSuccess = true;
				}
				catch (Exception ex)
				{
					result.AddMessageItem(new MessageItem(ex));
				}
			}
			return result;
		}
		public async Task<Result<PaySheet>> Hold(Guid Id)
		{
			var result = new Result<PaySheet>();
			PayMonth month = await UOW.GetRepositoryAsync<PayMonth>().SingleAsync(x => x.ID == Id);
			if (month == null)
			{
				result.AddMessageItem(new MessageItem("Selected month is not a valid", FeedbackType.Error));
				return result;
			}
			else if (month.Status == (int)PayMonthStatus.Released)
			{
				result.AddMessageItem(new MessageItem("Selected pay month is already released.", FeedbackType.Error));
			}
			else if (month.Status == (int)PayMonthStatus.InProcess)
			{
				result.AddMessageItem(new MessageItem("Please process pay month before release.", FeedbackType.Error));
			}
			PayMonth nextMonth = await UOW.GetRepositoryAsync<PayMonth>().SingleAsync(x => x.Start >= month.End,
				   orderBy: o => o.OrderBy(x => x.Start));
			if (nextMonth == null || nextMonth.Status > (int)PayMonthStatus.Open)
			{
				result.AddMessageItem(new MessageItem("Sorry!, you can not hold selected pay month.", FeedbackType.Error));
				return result;
			}

			if (result.HasNoError)
			{
				month.Status = (int)PayMonthStatus.InProcess;
				UOW.GetRepositoryAsync<PayMonth>().UpdateAsync(month);

				nextMonth.Status = (int)PayMonthStatus.Open;
				UOW.GetRepositoryAsync<PayMonth>().UpdateAsync(nextMonth);

				try
				{
					await UOW.SaveChangesAsync();
					result.IsSuccess = true;
				}
				catch (Exception ex)
				{
					result.AddMessageItem(new MessageItem(ex));
				}
			}
			return result;
		}
		public virtual async Task<Result<PaySheet>> Process(Guid Id)
		{
			//pay month id for particular month and year
			var result = new Result<PaySheet>();


			var payMonth = await UOW.GetRepositoryAsync<PayMonth>().SingleAsync(x => x.ID == Id,
				include: o => o.Include(x => x.FinancialYear).ThenInclude(x => x.PaySettings));

			if (payMonth.Status == (int)PayMonthStatus.Released)
			{
				result.IsSuccess = false;
				result.AddMessageItem(new MessageItem("Sorry! selected month is already released", FeedbackType.Error));
				return result;
			}

			//Find previous month status
			//To know the pay run pending months while calculating declaration
			//previous pay month should be closed
			var previousPayMonth = await UOW.GetRepositoryAsync<PayMonth>().SingleAsync
					(x => x.End <= payMonth.Start, orderBy: o => o.OrderByDescending(x => x.End));

			if (previousPayMonth != null &&
				previousPayMonth.Status != (int)PayMonthStatus.Released)
			{
				result.IsSuccess = false;
				result.AddMessageItem(new MessageItem("Sorry! previous pay month is not released.", FeedbackType.Error));
				return result;
			}
			var ptaxSlabs = await UOW.GetRepositoryAsync<ProfessionalTaxSlab>().GetAsync(include: o => o.Include(x => x.ProfessionalTax));

			var declarationSetting = await UOW.GetRepositoryAsync<DeclarationSetting>()
				.SingleAsync(x => x.PaySettingsId == payMonth.FinancialYear.PaySettingsId);

			if (declarationSetting == null || declarationSetting.MaxLimitEightyC == 0 || declarationSetting.MaxLimitEightyD == 0)
			{
				result.IsSuccess = false;
				result.AddMessageItem(new MessageItem("IT declaration 80c max limit is missing. " +
					"Please update the values on the settings page.", FeedbackType.Error));
				return result;
			}

			ESI esiOrg = await UOW.GetRepositoryAsync<ESI>()
				.SingleAsync(x => x.PaySettingsId == payMonth.FinancialYear.PaySettingsId);
			EPF epfOrg = await UOW.GetRepositoryAsync<EPF>()
				.SingleAsync(x => x.PaySettingsId == payMonth.FinancialYear.PaySettingsId);

			//IEnumerable<IncentivesPayCut>
			var transIncentives = await UOW.GetRepositoryAsync<IncentivesPayCut>()
				.GetAsync(x => x.Year == payMonth.Year && x.Month == payMonth.Month);

			var newSheetItem = new List<PaySheet>();
			var updateSheetItem = new List<PaySheet>();

			//Calculate pay all active employees
			var employees = await UOW.GetRepositoryAsync<Employee>().GetAsync(x => x.Status == 1
			   && ((x.DateOfJoining <= payMonth.End && !x.LastWorkingDate.HasValue)
			   || (x.LastWorkingDate >= payMonth.Start && x.LastWorkingDate <= payMonth.End)) && x.WorkType.SalaryPaying == true,
			   include: x => x.Include(i => i.WorkLocation));

			//get all active employees
			var payInfos = await UOW.GetRepositoryAsync<EmployeePayInfo>().GetAsync(x =>
				(x.Employee.DateOfJoining <= payMonth.End && !x.Employee.LastWorkingDate.HasValue)
			   || (x.Employee.LastWorkingDate >= payMonth.Start),
			   include: x => x.Include(i => i.Bank));

			int paidMonths = await FindPaidMonths(payMonth);

			//Prepare service class object with all predefined values 
			var calculator = new PayCalculator(UOW)
			{
				MonthDays = payMonth.Days,
				PayMonth = payMonth.Month,
				PayYear = payMonth.Year,
				FYId = payMonth.FinancialYearId,
				MonthStart = payMonth.Start,
				PaidMonths = paidMonths,
				FYFromMonth = payMonth.FinancialYear.PaySettings.FYFromMonth,
				TaxDedLastMonth = declarationSetting.TaxDedLastMonth,
				ESISettings = esiOrg,
				EPFSettings = epfOrg,
				PTaxSlabs = ptaxSlabs.ToList(),
				Earnings = (await UOW.GetRepositoryAsync<EarningComponent>().GetAsync()).ToList(),
				Deductions = (await UOW.GetRepositoryAsync<DeductionComponent>().GetAsync()).ToList(),
				DeclarationSetting = declarationSetting,
				FinancialYear = payMonth.FinancialYear,
				TaxCalculater = new PayRoll.IncomeTaxCalculator(UOW, payMonth.FinancialYear)
			};

			//to separate pay sheet list 
			bool isNew;

			//Calculate salary for each employee
			foreach (Employee employee in employees)
			{
				Salary salaryItem = await UOW.GetRepositoryAsync<Salary>().SingleAsync(x => x.EmployeeId == employee.ID,
					include: x => x.Include(o => o.Earnings).ThenInclude(o => o.Component)
					.Include(o => o.Deductions).ThenInclude(o => o.Deduction));

				PaySheet paySheet = await UOW.GetRepositoryAsync<PaySheet>().SingleAsync(x => x.EmployeeID == employee.ID && x.PayMonthId == payMonth.ID,
					   include: o => o.Include(x => x.Earnings).Include(x => x.Deductions));

				var incentivePayCut = transIncentives.FirstOrDefault(x => x.EmployeeId == employee.ID);
				isNew = false;
				if (paySheet == null)
				{
					isNew = true;
					paySheet = new PaySheet
					{
						ID = Guid.NewGuid(),
						EmployeeID = employee.ID,
						PayMonthId = payMonth.ID,
						Earnings = new List<PaySheetEarning>(),
						Deductions = new List<PaySheetDeduction>(),
					};
				}

				paySheet.Incentive = incentivePayCut == null ? 0 : incentivePayCut.Incentives;
				paySheet.PayCut = incentivePayCut == null ? 0 : incentivePayCut.PayCut;
				paySheet.WorkDays = employee.LastWorkingDate.HasValue ?
						ResignedEmployeeWorkdays(payMonth.Days, payMonth.Start, payMonth.End,
													employee.DateOfJoining, employee.LastWorkingDate.Value)
						: ActiveEmployeeWorkdays(payMonth.Days, payMonth.Start, payMonth.End, employee.DateOfJoining);

				//

				var attendance = await AttendanceSum(paySheet.EmployeeID, calculator.PayMonth, calculator.PayYear, calculator.MonthDays);

				paySheet.PresentDays = attendance.Item1;
				paySheet.LOPDays = attendance.Item2;
				paySheet.UADays = attendance.Item3;
				//Calculate work days once again depending on employee leave.
				//When employee on long leave, he/she apply leave with off time leave type.
				//Those days removing from employee working days.
				//For those days salary not paying and LOP also not calculating. 
				if (attendance.Item4 > 0)
				{
					paySheet.WorkDays -= attendance.Item4;
				}
				paySheet.LCDays = await LateComingDays(paySheet.EmployeeID, calculator.PayMonth, calculator.PayYear);
				//

				//Present days zero then or employee salary information not available
				//if (paySheet.PresentDays == 0 || salaryItem == null) [Mohan]
				if (salaryItem == null)
				{
					await Revert(paySheet, payMonth.FinancialYearId);
				}
				else
				{
					await calculator.CalculateSalary(salaryItem, paySheet, employee.WorkLocation.StateId);
				}

				var payInfo = payInfos.FirstOrDefault(x => x.EmployeeId == employee.ID);
				UpdatePaymentInfo(paySheet, payInfo);

				if (isNew) { newSheetItem.Add(paySheet); } else { updateSheetItem.Add(paySheet); }
			}

			//Delete employees missing in present run 
			await DeleteEmployeeMissing(payMonth, newSheetItem, updateSheetItem);
			await UOW.SaveChangesAsync();

			await UpdatePaySheet(payMonth);

			await LoansDueUpdate(payMonth);

			await UOW.SaveChangesAsync();

			result.IsSuccess = true;
			return result;
		}

		private async Task DeleteEmployeeMissing(PayMonth payMonth, List<PaySheet> newSheetItem, List<PaySheet> updateSheetItem)
		{
			var paySheets = await UOW.GetRepositoryAsync<PaySheet>().GetAsync(x => x.PayMonthId == payMonth.ID
						, include: o => o.Include(x => x.Earnings).Include(x => x.Deductions));
			foreach (var item in paySheets)
			{
				var va1 = newSheetItem.Any(x => x.EmployeeID == item.EmployeeID);
				var va2 = updateSheetItem.Any(x => x.EmployeeID == item.EmployeeID);
				if (!va1 && !va2)
				{
					UOW.GetRepositoryAsync<PaySheet>().DeleteAsync(item);
				}
			}
			await UOW.GetRepositoryAsync<PaySheet>().AddAsync(newSheetItem);
			UOW.GetRepositoryAsync<PaySheet>().UpdateAsync(updateSheetItem);
		}

		private async Task UpdatePaySheet(PayMonth payMonth)
		{
			payMonth.Cost = await UOW.GetRepositoryAsync<PaySheet>().SumOfIntAsync(x => x.PayMonthId == payMonth.ID, x => x.Gross);
			payMonth.Net = await UOW.GetRepositoryAsync<PaySheet>().SumOfIntAsync(x => x.PayMonthId == payMonth.ID, x => x.Net);
			payMonth.Employees = await UOW.GetRepositoryAsync<PaySheet>().GetCountAsync(x => x.PayMonthId == payMonth.ID);
			payMonth.Status = (int)PayMonthStatus.InProcess;
			UOW.GetRepositoryAsync<PayMonth>().UpdateAsync(payMonth);
		}

		private async Task LoansDueUpdate(PayMonth payMonth)
		{
			var loanDeductions = await UOW.GetRepositoryAsync<LoanDeduction>().GetAsync(x => x.PayMonthId == payMonth.ID);

			foreach (var deduction in loanDeductions.Select(x => x.LoanID))
			{
				var loan = await UOW.GetRepositoryAsync<Loan>().SingleAsync(x => x.ID == deduction);
				var amount = await UOW.GetRepositoryAsync<LoanDeduction>().SumOfIntAsync(x => x.LoanID == deduction, x => x.Deducted);
				loan.Due = loan.LoanAmount - amount;
				UOW.GetRepositoryAsync<Loan>().UpdateAsync(loan);
			}
		}

		private static void UpdatePaymentInfo(PaySheet paySheet, EmployeePayInfo payInfo)
		{
			if (payInfo == null)
			{
				paySheet.PayMode = 0;
				return;
			}

			paySheet.DebitBankId = payInfo.BankId;
			paySheet.PayMode = payInfo.PayMode;
			switch (payInfo.PayMode)
			{
				case 1: //Internal bank transfer
					paySheet.BankName = payInfo.Bank.Name;
					paySheet.BankIFSC = payInfo.Bank.IFSCCode;
					paySheet.BankACNo = payInfo.AccountNo;
					break;
				case 2: //NEFT//Online
					paySheet.BankName = payInfo.BankName;
					paySheet.BankIFSC = payInfo.IFSCCode;
					paySheet.BankACNo = payInfo.AccountNo;
					break;
			}
		}

		private async Task<int> FindPaidMonths(PayMonth payMonth)
		{
			//Find salary released months to know due months in current year
			return await UOW.GetRepositoryAsync<PayMonth>().GetCountAsync(x => x.FinancialYearId == payMonth.FinancialYearId
										&& x.Status == (int)PayMonthStatus.Released);
		}

		public async Task<Result<PaySheet>> Delete(Guid Id)
		{
			Result<PaySheet> result = new(false);
			var month = await UOW.GetRepositoryAsync<PayMonth>().SingleAsync(x => x.ID == Id);
			if (month == null)
			{
				result.AddMessageItem(new MessageItem("Invalid month selected"));
				return result;
			}

			var paySheets = (await UOW.GetRepositoryAsync<PaySheet>().GetAsync(x => x.PayMonthId == Id,
				include: o => o.Include(x => x.Earnings).Include(x => x.Deductions))).ToList();

			foreach (var paySheet in paySheets)
			{
				await Revert(paySheet, month.FinancialYearId);
				UOW.GetRepositoryAsync<PaySheet>().DeleteAsync(paySheet);
			}

			month.Cost = 0;
			month.Net = 0;
			month.Employees = 0;
			month.Status = (int)PayMonthStatus.Open;
			UOW.GetRepositoryAsync<PayMonth>().UpdateAsync(month);

			try
			{
				await UOW.SaveChangesAsync();
				result.IsSuccess = true;
			}
			catch (Exception ex)
			{
				result.AddMessageItem(new MessageItem(ex));
			}

			return result;
		}

		public decimal ActiveEmployeeWorkdays(decimal monthDays, DateTime startDate, DateTime endDate,
		DateTime dateOfJoining)
		{
			if (dateOfJoining > endDate) return 0;

			//Here considering the Present days, leaves and unauthorized leaves 

			return dateOfJoining >= startDate ?
				endDate.Subtract(dateOfJoining).Days + 1
				: monthDays;
		}

		public decimal ResignedEmployeeWorkdays(decimal monthDays, DateTime monthStart, DateTime monthEnd,
			DateTime dateOfJoining, DateTime lastWorkingDate)
		{
			//Here considering the Present days, leaves and unauthorized leaves 

			if (lastWorkingDate < monthStart
				|| dateOfJoining > monthEnd) return 0;

			//Employee last working date is not in current month then pay for month days
			if (dateOfJoining < monthStart && lastWorkingDate > monthEnd)
				return monthDays;

			if (dateOfJoining >= monthStart && lastWorkingDate <= monthEnd)
				return lastWorkingDate.Subtract(dateOfJoining).Days + 1;

			if (dateOfJoining >= monthStart)
				return monthEnd.Subtract(dateOfJoining).Days + 1;

			if (dateOfJoining < monthStart && lastWorkingDate <= monthEnd)
				return lastWorkingDate.Subtract(monthStart).Days + 1;
			return 0;
		}

		public async Task Revert(PaySheet paySheet, Guid FYId)
		{
			paySheet.NoPay();

			if (paySheet.Loan > 0)
			{
				var loanDeductions = await UOW.GetRepositoryAsync<LoanDeduction>().GetAsync(x => x.EmployeeID == paySheet.EmployeeID
										&& x.PayMonthId == paySheet.PayMonthId, include: x => x.Include(i => i.Loan));
				foreach (var deduction in loanDeductions)
				{
					var loan = await UOW.GetRepositoryAsync<Loan>().SingleAsync(x => x.ID == deduction.LoanID);
					loan.Due += deduction.Deducted;
					UOW.GetRepositoryAsync<LoanDeduction>().DeleteAsync(deduction);
					UOW.GetRepositoryAsync<Loan>().UpdateAsync(loan);
				}
			}

			//Revert income tax 
			if (paySheet.Tax > 0)
			{
				Declaration declarationUpate = await UOW.GetRepositoryAsync<Declaration>().SingleAsync(P => P.EmployeeId == paySheet.EmployeeID
				&& P.FinancialYearId == FYId);
				if (declarationUpate != null)
				{
					declarationUpate.TaxPaid -= paySheet.Tax;
					declarationUpate.Due = declarationUpate.TaxPayable - declarationUpate.TaxPaid;
					UOW.GetRepositoryAsync<Declaration>().UpdateAsync(declarationUpate);
				}
			}
		}

		public async Task<Tuple<decimal, decimal, decimal, decimal>> AttendanceSum(Guid employeeId, int month, int year, decimal monthDays)
		{
			//Here considering the Present days and leaves and unauthorized leaves
			var attendance = await UOW.GetRepositoryAsync<Domain.Entities.Leave.AttendanceSum>()
				.SingleAsync(a => a.EmployeeId == employeeId && a.Month == month && a.Year == year);

			if (attendance != null)
			{
				return new Tuple<decimal, decimal, decimal, decimal>(attendance.Present, attendance.LOP,
					attendance.Unauthorized, attendance.OffDays);
			}
			return new Tuple<decimal, decimal, decimal, decimal>(0, monthDays, 0, 0);
		}


		public async Task<decimal> LateComingDays(Guid employeeId, int month, int year)
		{
			//Here considering the number of days Late
			return await UOW.GetRepositoryAsync<Latecomers>()
				.SumOfDecimalAsync(a => a.EmployeeID == employeeId && a.Month == month && a.Year == year, x => x.NumberOfDays);

		}

	}
}
