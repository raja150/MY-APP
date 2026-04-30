using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core.Extension;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Enums;
using TranSmart.Service.PayRoll;
using static TranSmart.Service.PayRoll.IncomeTaxNewRegmine;

namespace TranSmart.Service.Payroll
{
	public interface IPayCalculator
	{
		Task<PaySheet> CalculateSalary(Salary salaryItem, PaySheet paySheet, Guid stateId);
	}
	public class PayCalculator : IPayCalculator
	{
		private readonly IUnitOfWork _uow;

		public PayCalculator(IUnitOfWork uow)
		{
			_uow = uow;
		}
		public int PaidMonths { get; set; }
		public decimal MonthDays { get; set; }
		public DateTime MonthStart { get; set; }
		public int FYFromMonth { get; set; }

		private int _taxDedLastMonth;
		public int TaxDedLastMonth
		{
			get { return _taxDedLastMonth; }
			set
			{
				_taxDedLastMonth = value;
				int i;
				for (i = 0; i < 12; i++)
				{
					if ((FYFromMonth + i) == 12 && value == (FYFromMonth + i))
					{
						break;
					}
					else if (value == (FYFromMonth + i) % 12)
					{
						break;
					}
				}

				_taxSplitMonth = Math.Max(i + 1 - PaidMonths, 0);
			}
		}

		private int _taxSplitMonth;
		public int TaxSplitMonth => _taxSplitMonth;
		public Guid FYId { get; set; }
		public int PayMonth { get; set; }
		public int PayYear { get; set; }
		public ESI ESISettings { get; set; }
		public EPF EPFSettings { get; set; }
		public DeclarationSetting DeclarationSetting { get; set; }
		public FinancialYear FinancialYear { get; set; }
		public IEnumerable<ProfessionalTaxSlab> PTaxSlabs { get; set; }
		public IEnumerable<EarningComponent> Earnings { get; set; }
		public IEnumerable<DeductionComponent> Deductions { get; set; }

		public IIncomeTaxCalculator TaxCalculater { get; set; }

		public async Task<PaySheet> CalculateSalary(Salary salaryItem, PaySheet paySheet, Guid stateId)
		{
			//Attendance details getting from attendance sum table
			//var attendance = await AttendanceSum(paySheet.EmployeeID, PayMonth, PayYear, MonthDays);
			//var lateComingDays = await LateComingDays(paySheet.EmployeeID, PayMonth, PayYear);

			//paySheet.PresentDays = attendance.Item1;
			//paySheet.LOPDays = attendance.Item2;
			//paySheet.UADays = attendance.Item3;
			//paySheet.LCDays = lateComingDays;
			paySheet.Salary = salaryItem.Monthly;
			//EPF configuration list
			EmpStatutory empStatutory = await _uow.GetRepositoryAsync<EmpStatutory>().SingleAsync(x => x.EmpId == paySheet.EmployeeID);

			List<PaySheetEarning> paySheetEarnings = PayEarrings(paySheet.ID, MonthDays, paySheet.WorkDays, paySheet.LOPDays, salaryItem.Earnings);
			List<PaySheetDeduction> paySheetDualDeduction = PayDualDeductions(paySheetEarnings);
			List<PaySheetDeduction> paySheetDeduction = PayDeductions(paySheet.ID, MonthDays, paySheet.WorkDays, paySheet.LOPDays, salaryItem.Deductions);
			paySheetDeduction.AddRange(paySheetDualDeduction);

			//Add or Update Components  
			CollectionCompareResult<PaySheetEarning> compareEarnings = paySheet.Earnings.Compare(paySheetEarnings, (x, y) => x.ComponentId.Equals(y.ComponentId));
			foreach (PaySheetEarning comp in compareEarnings.Same)
			{
				PaySheetEarning editComp = paySheetEarnings.FirstOrDefault(x => x.ComponentId == comp.ComponentId);
				if (!comp.Equals(editComp))
				{
					comp.Update(editComp);
				}
			}
			foreach (PaySheetEarning comp in compareEarnings.Added)
			{
				comp.PaySheetId = paySheet.ID;
				paySheet.Earnings.Add(comp);
			}

			foreach (PaySheetEarning comp in compareEarnings.Deleted)
			{
				paySheet.Earnings.Remove(comp);
				_uow.GetRepositoryAsync<PaySheetEarning>().DeleteAsync(comp);
			}

			CollectionCompareResult<PaySheetDeduction> compareDed = paySheet.Deductions.Compare(paySheetDeduction, (x, y) => x.ComponentId.Equals(y.ComponentId));
			foreach (PaySheetDeduction comp in compareDed.Same)
			{
				PaySheetDeduction editComp = paySheetDeduction.FirstOrDefault(x => x.ComponentId == comp.ComponentId);
				if (!comp.Equals(editComp))
				{
					comp.Update(editComp);
				}
			}
			foreach (PaySheetDeduction comp in compareDed.Added)
			{
				comp.PaySheetId = paySheet.ID;
				paySheet.Deductions.Add(comp);
			}

			foreach (PaySheetDeduction comp in compareDed.Deleted)
			{
				paySheet.Deductions.Remove(comp);
				_uow.GetRepositoryAsync<PaySheetDeduction>().DeleteAsync(comp);
			}


			paySheet.LOP = LOPAmount(paySheet.Earnings);
			paySheet.UA = AttendanceDeductions(salaryItem.Earnings, MonthDays, paySheet.UADays);
			paySheet.LC = AttendanceDeductions(salaryItem.Earnings, MonthDays, paySheet.LCDays);
			paySheet.Arrears = await _uow.GetRepositoryAsync<Arrear>().SumOfIntAsync(x => x.EmployeeID == paySheet.EmployeeID
				&& x.Month == PayMonth && x.Year == PayYear, x => x.Pay);

			paySheet.Gross = paySheet.Earnings.Sum(x => x.Salary) + paySheet.Incentive + paySheet.Arrears;
			Tuple<bool, int, int, string> tuple = await ESICalculation(paySheet.EmployeeID,
				paySheet.Earnings,
				salaryItem.Earnings, empStatutory);
			paySheet.ESIApplied = tuple.Item1;
			paySheet.ESI = tuple.Item2;
			paySheet.ESIGross = tuple.Item3;
			paySheet.ESINo = tuple.Item4;


			Tuple<int, int, string> pfValues =
				ProvidentFundCalc.ForSalary(EPFSettings, Earnings, paySheet.Earnings, empStatutory);

			paySheet.EPF = pfValues.Item1;
			paySheet.EPFGross = pfValues.Item2;
			paySheet.EPFNo = pfValues.Item3;

			paySheet.PTax = CalculatePTax(paySheet.Gross, stateId);
			paySheet.Tax = await CalculateIncomeTax(salaryItem, paySheet, stateId);
			paySheet.Loan = await CalcualteLoanAmount(paySheet.EmployeeID, paySheet.PayMonthId);

			int deductionTotal = paySheet.Deductions.Sum(x => x.Deduction);
			paySheet.Deduction = paySheet.PayCut + paySheet.LOP + paySheet.EPF +
				paySheet.ESI + paySheet.PTax + paySheet.Tax + deductionTotal + paySheet.Loan + paySheet.UA + paySheet.LC;
			paySheet.Net = Math.Max(paySheet.Gross - paySheet.Deduction, 0);
			return paySheet;
		}

		public async Task<int> CalculateIncomeTax(Salary salaryItem, PaySheet paySheet, Guid stateId)
		{
			//while reprocess the salary revert back old tax value
			int previousTax = paySheet.Tax;
			var taxLimit = await _uow.GetRepositoryAsync<IncomeTaxLimit>()
							.SingleAsync(P => P.EmployeeId == paySheet.EmployeeID && P.Month == PayMonth && P.Year == PayYear);
			var declaration = await _uow.GetRepositoryAsync<Declaration>()
							.SingleAsync(P => P.EmployeeId == paySheet.EmployeeID && P.FinancialYearId == FYId);

			var declarationGuid = Guid.NewGuid();
			bool isNewDeclaration = false; //To know declaration is already available or not
			if (declaration == null)
			{
				isNewDeclaration = true;
				declaration = new Declaration { ID = declarationGuid, EmployeeId = paySheet.EmployeeID, FinancialYearId = FYId, IsNewRegime = false };
			}

			var paySheets = (await _uow.GetRepositoryAsync<PaySheet>()
					.GetAsync(x => x.EmployeeID == paySheet.EmployeeID
						&& x.PayMonth.FinancialYearId == FYId && x.PayMonth.Status == (int)PayMonthStatus.Released
						&& x.PayMonth.ID != paySheet.ID,
					include: o => o.Include(x => x.Earnings).ThenInclude(x => x.Component)
								   .Include(x => x.Deductions).ThenInclude(x => x.Component))).ToList();

			paySheets.Add(paySheet);

			//Income tax calculation depending on regime
			//IIncomeTaxCalculator calculater = new IncomeTaxCalculator(_uow, FinancialYear);
			//Adding one to paid months for current process month.
			await TaxCalculater.Calculate(salaryItem, paySheets, declaration, DeclarationSetting, stateId, PaidMonths + 1, false);

			int taxPay = taxLimit == null ? IncomeTaxAmount(declaration.Due, previousTax) : taxLimit.Amount;
			declaration.TaxPaid += taxPay - previousTax;
			declaration.Due = declaration.TaxPayable - declaration.TaxPaid;

			if (isNewDeclaration)
			{
				await _uow.GetRepositoryAsync<Declaration>().AddAsync(declaration, declarationGuid);
			}
			else
			{
				_uow.GetRepositoryAsync<Declaration>().UpdateAsync(declaration);
			}

			return taxPay;
		}



		//Employee loan recovery 
		public async Task<int> CalcualteLoanAmount(Guid empId, Guid payMonthId)
		{
			int loanAmount = 0;
			var loans = await _uow.GetRepositoryAsync<Loan>().GetAsync(x => x.EmployeeId == empId && x.Due > 0);
			if (loans.Any())
			{
				foreach (var item in loans)
				{
					var deduction = await _uow.GetRepositoryAsync<LoanDeduction>().SingleAsync(x => x.EmployeeID == empId
						&& x.PayMonthId == payMonthId && x.LoanID == item.ID);
					int instalment = 0;
					if (deduction == null)
					{
						instalment = Math.Min(item.MonthlyAmount, item.Due.Value);
						await _uow.GetRepositoryAsync<LoanDeduction>().AddAsync(new LoanDeduction
						{
							LoanID = item.ID,
							EmployeeID = item.EmployeeId,
							PayMonthId = payMonthId,
							Deducted = instalment,
						});
					}
					else
					{
						instalment = Math.Min(item.MonthlyAmount, item.Due.Value + deduction.Deducted);
						deduction.Deducted = instalment;
						_uow.GetRepositoryAsync<LoanDeduction>().UpdateAsync(deduction);
					}
					loanAmount += instalment;
				}
			}
			else
			{
				//Loan deducted and pay sheet again re-processing
				//then already deducted amount 
				var deductions = await _uow.GetRepositoryAsync<LoanDeduction>().GetAsync(x => x.EmployeeID == empId
					 && x.PayMonthId == payMonthId, include: x => x.Include(i => i.Loan));
				foreach (var (deduction, instalment) in from deduction in deductions
														let instalment = Math.Min(deduction.Loan.MonthlyAmount, deduction.Deducted)
														select (deduction, instalment))
				{
					if (deduction.Deducted != instalment)
					{
						deduction.Deducted = instalment;
						_uow.GetRepositoryAsync<LoanDeduction>().UpdateAsync(deduction);
					}

					loanAmount += instalment;
				}
			}
			return loanAmount;
		}

		#region
		//public async Task<Tuple<decimal, decimal, decimal, decimal>> Attendance(Guid employeeId, DateTime fromDate, DateTime toDate)
		//{
		//	//Here considering the Present days and leaves and unauthorized leaves
		//	var empAttDetails = await _uow.GetRepositoryAsync<Domain.Entities.Leave.Attendance>()
		//		.GetAsync(a => a.EmployeeId == employeeId && a.AttendanceDate >= fromDate && a.AttendanceDate <= toDate);

		//	decimal empPresent = empAttDetails.Count(a => a.AttendanceStatus == (int)AttendanceStatus.Present);

		//	decimal empAbsent = empAttDetails.Count(a => a.AttendanceStatus == (int)AttendanceStatus.Absent);

		//	decimal empUnauthorized = empAttDetails.Where(a => a.AttendanceStatus == (int)AttendanceStatus.Unautherized && a.UADays.HasValue)
		//		.Sum(x => x.UADays.Value);

		//	decimal empLateComing = empAttDetails.Count(a => a.AttendanceStatus == (int)AttendanceStatus.Late);

		//	return new Tuple<decimal, decimal, decimal, decimal>(empPresent, empAbsent, empUnauthorized, empLateComing);
		//}

		//public async Task<Tuple<decimal, decimal, decimal>> AttendanceSum(Guid employeeId, int month, int year, decimal monthDays)
		//{
		//	//Here considering the Present days and leaves and unauthorized leaves
		//	var empAttDetails = await _uow.GetRepositoryAsync<Domain.Entities.Leave.AttendanceSum>()
		//		.SingleAsync(a => a.EmployeeId == employeeId && a.Month == month && a.Year == year);

		//	if (empAttDetails != null)
		//	{
		//		return new Tuple<decimal, decimal, decimal>(empAttDetails.Present, empAttDetails.LOP,
		//			empAttDetails.Unauthorized);
		//	}
		//	return new Tuple<decimal, decimal, decimal>(0, monthDays, 0);
		//}

		//public async Task<decimal> LateComingDays(Guid employeeId, int month, int year)
		//{
		//	//Here considering the number of days Late
		//	return await _uow.GetRepositoryAsync<Latecomers>()
		//		.SumOfDecimalAsync(a => a.EmployeeID == employeeId && a.Month == month && a.Year == year, x=>x.NumberOfDays);

		//}

		//Earrings
		public static List<PaySheetEarning> PayEarrings(Guid paySheetId, decimal monthDays, decimal workDays, decimal lop, ICollection<SalaryEarning> salaryEarnings)
		{
			return salaryEarnings.Select(empCompo => new PaySheetEarning
			{
				PaySheetId = paySheetId,
				ComponentId = empCompo.ComponentId,
				HeaderName = empCompo.Component.Name,
				EarningType = empCompo.Component.EarningType,
				Salary = Salary(empCompo.Monthly, monthDays, workDays),
				Earning = EarningsAfterLop(empCompo.Component.ProrataBasis, empCompo.Monthly, monthDays, workDays, lop),
			}).ToList();
		}

		//Dual Components
		public List<PaySheetDeduction> PayDualDeductions(ICollection<PaySheetEarning> paySheetEarnings)
		{
			IEnumerable<DeductionComponent> deductionComponents = Deductions.Where(x => x.EarningId != null);
			return (from item in deductionComponents
					let paySheetEarning = paySheetEarnings.FirstOrDefault(x => x.ComponentId == item.EarningId)
					where paySheetEarning != null
					select new PaySheetDeduction
					{
						ComponentId = item.ID,
						PaySheetId = paySheetEarning.PaySheetId,
						HeaderName = item.Name,
						DeductType = item.Deduct,
						Deduction = paySheetEarning.Earning,
						Salary = paySheetEarning.Salary
					}).ToList();

		}
		//Deduction
		public static List<PaySheetDeduction> PayDeductions(Guid paySheetId, decimal monthDays, decimal workDays, decimal lop, ICollection<SalaryDeduction> salaryDeductions)
		{
			return salaryDeductions.Select(empCompo => new PaySheetDeduction
			{
				PaySheetId = paySheetId,
				DeductType = empCompo.Deduction.Deduct,
				Salary = Salary(empCompo.Monthly, monthDays, workDays),
				Deduction = EarningsAfterLop(empCompo.Deduction.ProrataBasis, empCompo.Monthly, monthDays, workDays, lop),
				HeaderName = empCompo.Deduction.Name,
				ComponentId = empCompo.DeductionId
			}).ToList();
		}

		/// <summary>
		/// Calculates employees unauthorized and late coming days deduction amount
		/// </summary>
		/// <param name="salaryEarnings">Employee monthly earning salary components</param>
		/// <param name="days"></param>
		/// <returns>Total deduction amount</returns>
		public static int AttendanceDeductions(ICollection<SalaryEarning> salaryEarnings, decimal monthDays, decimal days)
		{
			int deduction = 0;
			foreach (var item in salaryEarnings)
			{
				deduction += DeductionsAmount(item.Component.ProrataBasis, item.Monthly, monthDays, days);
			}
			return deduction;
		}

		//ESI 
		public async Task<Tuple<bool, int, int, string>> ESICalculation(Guid employeeId,
			ICollection<PaySheetEarning> paySheetEarnings,
			ICollection<SalaryEarning> salaryEarnings,
			EmpStatutory empStatutory)
		{
			if (ESISettings?.EmployeesCont == null
				|| ESISettings?.ESISalaryLimit == null) return new Tuple<bool, int, int, string>(false, 0, 0, "-");

			var esiMonthly = Earnings.SelectMany(x => salaryEarnings.Where(y => y.ComponentId == x.ID && x.ESIContribution)).Sum(x => x.Monthly);
			var esiEarning = Earnings.SelectMany(x => paySheetEarnings.Where(y => y.ComponentId == x.ID && x.ESIContribution)).Sum(x => x.Earning);


			//Find employee previous month ESI status
			PaySheet previousSal = await _uow.GetRepositoryAsync<PaySheet>().SingleAsync
					(x => x.EmployeeID == employeeId && x.PayMonth.End <= MonthStart,
						orderBy: o => o.OrderByDescending(x => x.PayMonth.End));

			//No salary paid to employee on last month then consider as new employee
			bool esiPreviousMonth = previousSal != null && previousSal.ESIApplied;

			bool applicable = CheckESIApplicability(esiMonthly, esiPreviousMonth);
			decimal employeeContribution = ESISettings.EmployeesCont ?? 0;
			return new Tuple<bool, int, int, string>(applicable,
				applicable ? (int)Math.Ceiling(esiEarning * (employeeContribution / 100)) : 0,
				esiEarning, empStatutory == null || empStatutory.ESINo == null ? "-" : empStatutory.ESINo);
		}

		public bool CheckESIApplicability(int esiWages, bool esiPreviousMonth)
		{
			//After the commencement of a contribution period, even if the gross salary of an employee
			//exceeds ESI monthly limit, the employee continues to be covered under the ESI scheme till the
			//end of that contribution period. 

			//When less than ESI limit then employee should pay
			if (esiWages <= ESISettings.ESISalaryLimit) return true;
			//Every half year ESI calculation reset
			if ((PayMonth == 4 || PayMonth == 10)
				&& esiWages <= ESISettings.ESISalaryLimit) return true;

			if (PayMonth != 4 && PayMonth != 10 && esiPreviousMonth)
			{
				return true;
			}

			return false;
		}



		//LOP
		public static int LOPAmount(ICollection<PaySheetEarning> paySheetEarnings)
		{
			return paySheetEarnings.Sum(x => x.Salary - x.Earning);
		}
		//Professional Tax
		public int CalculatePTax(int grossTaxable, Guid stateId)
		{
			ProfessionalTaxSlab pftSlab = PTaxSlabs.FirstOrDefault(x => x.ProfessionalTax.StateId == stateId
																		&& (x.From <= grossTaxable && x.To >= grossTaxable));
			return pftSlab?.Amount ?? 0;
		}

		//TaxDeductible
		public int IncomeTaxAmount(int due, int previousTax)
		{
			//Employee paid more than tax amount then payable is zero
			if (due < 0) return 0;

			//Salary process month is last month then deduct all due amount 
			if (TaxDedLastMonth == PayMonth || due == 0 || _taxSplitMonth == 0)
			{
				return due + previousTax;
			}

			//Round to nearest 10
			return (int)(Math.Ceiling(((due + previousTax) * 1.0 / _taxSplitMonth) / 10.0d) * 10);
		}

		public static int Salary(int amount, decimal monthDays, decimal workDays)
		{
			return workDays >= monthDays ? amount : (int)Math.Round(amount * 1.0m / monthDays * workDays, 0);
		}
		public static int EarningsAfterLop(bool prorate, int amount, decimal monthDays, decimal workDays, decimal lop)
		{
			if (prorate && (lop >= monthDays || lop >= workDays || monthDays < workDays))
			{
				return 0;
			}

			if (prorate && lop > 0 || monthDays > workDays)
			{
				int calAmount = Salary(amount, monthDays, workDays);
				return calAmount - (int)Math.Round(amount / monthDays * lop, 0, MidpointRounding.ToEven);
			}
			return amount;
		}

		public static int DeductionsAmount(bool prorate, int amount, decimal monthDays, decimal lopDays)
		{
			if (prorate && lopDays >= monthDays)
			{
				return 0;
			}

			if (prorate && lopDays > 0)
			{
				return (int)Math.Round(amount / monthDays * lopDays, 0);
			}
			return 0;
		}
		#endregion
	}
}
