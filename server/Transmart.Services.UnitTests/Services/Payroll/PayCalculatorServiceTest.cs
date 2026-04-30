using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MockQueryable.Moq;
using Moq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using Transmart.Services.UnitTests.Services.Payroll.Data;
using Transmart.Services.UnitTests.Services.Payroll.PayRollSetData;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Enums;
using TranSmart.Service.Payroll;
using TranSmart.Service.PayRoll;
using Xunit;

namespace TranSmart.Services.UnitTests
{
	public class PayCalculatorTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly Mock<DbContext> _context;
		private readonly EmployeeDataGenerator _employeeData;
		private readonly Mock<IIncomeTaxCalculator> _taxCalculator;
		public PayCalculatorTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_context = new Mock<DbContext>();
			_employeeData = new EmployeeDataGenerator();
			_taxCalculator = new Mock<IIncomeTaxCalculator>();
		}
		
		[Theory]
		[InlineData(true, 31000, 31, 31, 0, 31000)]
		[InlineData(true, 31000, 31, 31, 5.5, 25500)]
		[InlineData(true, 31000, 31, 31, 0.5, 30500)]
		[InlineData(true, 31000, 31, 31, 31, 0)]
		[InlineData(true, 31000, 31, 31, 32, 0)]
		[InlineData(true, 31000, 31, 11, 0.5, 10500)]
		[InlineData(true, 31000, 31, 11, 1, 10000)]
		[InlineData(true, 31000, 31, 11, 11, 0)]
		[InlineData(true, 31000, 31, 11, 13, 0)]
		[InlineData(true, 31000, 31, 11, 0, 11000)]
		[InlineData(true, 31000, 31, 32, 0, 0)]
		[InlineData(true, 31000, 31, 31, 1, 30000)]
		[InlineData(true, 31000, 31, 32, 5, 0)]

		public void Get_Employee_LossOfPay(bool ProrataBasis, int Amount,
			decimal monthDays, decimal workDays, decimal LOP, int CalcAmount)
		{
			//Arrange
			var src = new PayCalculator(uow.Object)
			{
				MonthDays = 31
			};
			//Act
			var val = PayCalculator.EarningsAfterLop(ProrataBasis, Amount, monthDays, workDays, LOP);
			//Assert
			Assert.Equal(CalcAmount, val);
		}

		[Theory]
		[InlineData(1000, 0)]
		[InlineData(10000, 0)]
		[InlineData(15000, 100)]
		[InlineData(50000, 200)]
		public void Calculate_Employee_Professional_Tax(int Gross, int Tax)
		{
			//Arrange
			Guid stateId = Guid.NewGuid();
			var professionalTax = new ProfessionalTax { StateId = stateId };
			var professionalTaxSlabs = new List<ProfessionalTaxSlab>
			{
				new ProfessionalTaxSlab{ProfessionalTax = professionalTax,Amount = 0,From = 0,To = 10000},
				new ProfessionalTaxSlab{ProfessionalTax = professionalTax,Amount = 100,From = 10001,To = 15000},
				new ProfessionalTaxSlab{ProfessionalTax = professionalTax,Amount = 200,From = 15001,To = 99999999}
			};
			var src = new PayCalculator(uow.Object) { PTaxSlabs = professionalTaxSlabs };
			//Act 
			//Assert
			Assert.Equal(Tax, src.CalculatePTax(Gross, stateId));
		}

		[Theory]
		[InlineData(4, 1, 1000, 0, 4, 0, 100)]
		[InlineData(4, 4, 10000, 0, 4, 0, 10000)]
		[InlineData(4, 12, 10000, 0, 4, 0, 1120)]
		[InlineData(4, 1, 7898, 1111, 6, 2, 1130)]
		[InlineData(4, 1, 11111, 0, 4, 0, 1120)]
		[InlineData(4, 1, 10000, 0, 5, 1, 1120)]
		[InlineData(4, 1, 8889, 0, 6, 2, 1120)]
		[InlineData(4, 1, 7778, 0, 7, 3, 1120)]
		[InlineData(4, 1, 6667, 0, 8, 4, 1120)]
		[InlineData(4, 1, 5556, 0, 9, 5, 1120)]
		[InlineData(4, 1, 4445, 0, 10, 6, 1120)]
		[InlineData(4, 1, 3334, 0, 11, 7, 1120)]
		[InlineData(4, 1, 2223, 0, 12, 8, 1120)]
		[InlineData(4, 1, 1119, 0, 1, 9, 1119)]
		[InlineData(4, 1, 0, 0, 2, 10, 0)]
		[InlineData(4, 5, 3333, 0, 4, 0, 1670)]
		[InlineData(4, 5, 1666, 0, 5, 1, 1666)]
		public void Update_Income_Tax_To_Declaration(int FYFromMonth, int TaxDedLastMonth, int Due, int PreviousTax,
			int PayMonth, int PaidMonths, int Result)
		{
			//Arrange
			var src = new PayCalculator(uow.Object)
			{
				MonthDays = 31,
				PayMonth = PayMonth,
				PaidMonths = PaidMonths,
				FYFromMonth = FYFromMonth,
				TaxDedLastMonth = TaxDedLastMonth,
				DeclarationSetting = new DeclarationSetting { TaxDedLastMonth = TaxDedLastMonth }
			};
			//Act
			//Assert
			var result = src.IncomeTaxAmount(Due, PreviousTax);
			Assert.Equal(Result, result);
		}

		[Theory]
		[InlineData(4, 12, 10, 0)]
		[InlineData(4, 12, 0, 9)]
		[InlineData(4, 1, 0, 10)]
		[InlineData(4, 3, 0, 12)]
		[InlineData(4, 2, 0, 11)]
		[InlineData(1, 12, 0, 12)]
		[InlineData(1, 9, 0, 9)]
		[InlineData(6, 2, 0, 9)]
		public void Tax_Paid_Split_Months(int FYFromMonth, int TaxDedLastMonth, int paidMonths, int Result)
		{
			//Arrange
			var src = new PayCalculator(uow.Object)
			{
				FYFromMonth = FYFromMonth,
				PaidMonths = paidMonths,
				TaxDedLastMonth = TaxDedLastMonth,
			};
			//Act
			//Assert 
			Assert.Equal(Result, src.TaxSplitMonth);
		}

		[Theory]
		[ClassData(typeof(EsiDataGenerator))]
		public async Task Employee_Should_Pay_ESI(int ESISalaryLimit, decimal? EmployeesCont,
			int PayMonth, bool ESIApplied, int ESIAmount, bool Applicable,
			Tuple<int, int, int> salary, decimal LOPDays, int ESIWages)
		{
			var repo = new Mock<RepositoryAsync<PaySheet>>(_dbContext.Object);
			repo.Setup(x => x.SingleAsync(It.IsAny<Expression<Func<PaySheet, bool>>>(),
				  It.IsAny<Func<IQueryable<PaySheet>, IOrderedQueryable<PaySheet>>>(),
				  It.IsAny<Func<IQueryable<PaySheet>, IIncludableQueryable<PaySheet, object>>>(), true)).ReturnsAsync(new PaySheet
				  {
					  ESIApplied = ESIApplied
				  });
			uow.Setup(m => m.GetRepositoryAsync<PaySheet>()).Returns(repo.Object);

			Guid basicId = Guid.NewGuid();
			Guid hraId = Guid.NewGuid();
			Guid allowancesId = Guid.NewGuid();
			var earnings = new List<EarningComponent>
			{
				new EarningComponent { ID = basicId, ESIContribution = true, ProrataBasis = true },
				new EarningComponent { ID = hraId, ESIContribution = true, ProrataBasis = false},
				new EarningComponent { ID = allowancesId, ESIContribution = false, ProrataBasis = false }
			};
			var src = new PayCalculator(uow.Object)
			{
				MonthDays = 31,
				Earnings = earnings,
				PayMonth = PayMonth
			};
			if (ESISalaryLimit > 0)
			{
				src.ESISettings = new ESI { ESISalaryLimit = ESISalaryLimit, EmployeesCont = EmployeesCont };
			}
			ICollection<SalaryEarning> salaryEarning = new List<SalaryEarning>
			{
				new SalaryEarning
				{
					ComponentId = basicId,
					Monthly = salary.Item1
				},
				new SalaryEarning
				{
					ComponentId = hraId,
					Monthly = salary.Item2
				},
				new SalaryEarning
				{
					ComponentId = allowancesId,
					Monthly = salary.Item3
				}
			};
			ICollection<PaySheetEarning> paySheetEarnings = new List<PaySheetEarning>
			{
				new PaySheetEarning
				{
					ComponentId = basicId,
					Earning = PayCalculator.EarningsAfterLop(true, salary.Item1, src.MonthDays, src.MonthDays - LOPDays, LOPDays),
					Salary = salary.Item1
				},
				new PaySheetEarning
				{
					ComponentId = hraId,
					Earning = PayCalculator.EarningsAfterLop(true, salary.Item2, src.MonthDays, src.MonthDays - LOPDays, LOPDays),
					Salary = salary.Item2
				},
				new PaySheetEarning
				{
					ComponentId = allowancesId,
					Earning = PayCalculator.EarningsAfterLop(true, salary.Item3, src.MonthDays, src.MonthDays - LOPDays, LOPDays),
					Salary = salary.Item3
				}
			};
			var empStatutory = new EmpStatutory { EnableESI = 1, ESINo = "ABCD" };
			//Act
			Tuple<bool, int, int, string> esi = await src.ESICalculation(Guid.NewGuid(), paySheetEarnings, salaryEarning, empStatutory);
			//Assert
			//Assert.Equal(new Tuple<bool, int, int, string>(Applicable, ESIAmount, ESIWages, "ABCD"), esi);
			Assert.Equal(Applicable, esi.Item1);
			Assert.Equal(ESIAmount, esi.Item2);
			Assert.Equal(ESIWages, esi.Item3);
		}

		[Theory]
		[InlineData(4, 25000, false, false)]
		[InlineData(4, 20000, true, true)]
		[InlineData(4, 21000, false, true)]
		[InlineData(4, 21001, false, false)]

		[InlineData(5, 25000, true, true)]
		[InlineData(5, 25000, false, false)]
		[InlineData(5, 20000, true, true)]
		[InlineData(5, 20000, false, true)]
		[InlineData(6, 25000, true, true)]
		[InlineData(10, 25000, true, false)]
		[InlineData(10, 25000, false, false)]
		[InlineData(10, 20000, true, true)]
		[InlineData(10, 20000, false, true)]
		[InlineData(11, 25000, true, true)]
		[InlineData(11, 25000, false, false)]
		[InlineData(11, 20000, true, true)]
		[InlineData(11, 20000, false, true)]
		public void Check_ESI_Applicability(int payMonth, int esiWages, bool esiPreviousMonth, bool result)
		{
			var src = new PayCalculator(uow.Object)
			{
				PayMonth = payMonth,
				ESISettings = new ESI { ESISalaryLimit = 21000 }
			};

			var val = src.CheckESIApplicability(esiWages, esiPreviousMonth);
			Assert.Equal(result, val);
		}

		[Theory]
		[InlineData(true, 300, 30, 10, 100)]
		[InlineData(true, 100, 30, 10, 33)]
		[InlineData(false, 100, 30, 10, 0)]
		[InlineData(true, 100, 30, 30, 0)]
		[InlineData(true, 100, 30, 32, 0)]
		[InlineData(false, 100, 30, 0, 0)]
		public void DeductionsAmount(bool prorate, int amount, decimal monthDays, decimal lopDays, int expected)
		{
			var src = new PayCalculator(uow.Object);
			//Act
			var result = PayCalculator.DeductionsAmount(prorate, amount, monthDays, lopDays);

			//Assert
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData(300, 30, 10, 100)]
		[InlineData(100, 30, 10, 33)]
		[InlineData(100, 30, 34, 100)]
		public void Salary(int amount, decimal monthDays, decimal workDays, int expected)
		{
			var src = new PayCalculator(uow.Object);
			//Act
			var result = PayCalculator.Salary(amount, monthDays, workDays);

			//Assert
			Assert.Equal(expected, result);
		}

		[Fact]
		public void LOPAmount()
		{
			var src = new PayCalculator(uow.Object);

			var paySheetEarnings = new List<PaySheetEarning> { new PaySheetEarning
			{
				Salary = 1000, Earning = 950
			}, new PaySheetEarning{Salary = 1000, Earning = 1000
			} };
			//Act
			var result = PayCalculator.LOPAmount(paySheetEarnings);

			//Assert
			Assert.Equal(50, result);
		}

		//Mohan to do
		//[Theory]
		//[InlineData(15000, 1, 10000, 1200)]
		//[InlineData(15000, 2, 10000, 1200)]
		//[InlineData(15000, 2, 20000, 1800)]
		//[InlineData(15000, 3, 20000, 0)]
		//public void EpfCalculation(int restrictedWage, int contribType, int pfWages, int expected)
		//{
		//	var src = new PayCalculator(uow.Object );

		//	//Act
		//	var result = src.EpfCalculation(restrictedWage, contribType, pfWages);

		//	//Assert
		//	Assert.Equal(expected, result);
		//}

		//[Fact] 
		//public void EPFAmount( )
		//{
		//	var src = new PayCalculator(uow.Object );
		//	src.EPFSettings = new EPF { IncludeinCTC = true };
		//	var sheetEarnings = new List<PaySheetEarning>();
		//	sheetEarnings.Add(new PaySheetEarning {   })
		//	var empStatutory = new EmpStatutory()
		//	{

		//	}
		//	//Act
		//	var result = src.EPFAmount(paySheetEarnings, empStatutory);

		//	//Assert
		//	Assert.Equal(expected, result);
		//}


		[Theory]
		[InlineData(15000, false, 5, false)]
		[InlineData(15000, true, 5, true)]
		[InlineData(11000, true, 5, true)]
		[InlineData(11000, false, 5, true)]
		[InlineData(15000, false, 4, false)]
		[InlineData(11000, true, 4, true)]
		[InlineData(12000, false, 4, true)]
		[InlineData(13000, false, 4, false)]
		[InlineData(15000, false, 10, false)]
		[InlineData(15000, true, 10, false)]
		[InlineData(11000, true, 10, true)]
		[InlineData(11000, false, 10, true)]
		[InlineData(15000, false, 9, false)]
		[InlineData(15000, true, 9, true)]
		[InlineData(10000, true, 9, true)]
		[InlineData(10000, false, 9, true)]
		public void Check_ESI_Applicablity(int esiWages, bool esipreviousMonth, int payMonth, bool result)
		{
			var src = new PayCalculator(uow.Object)
			{
				PayMonth = payMonth,
				ESISettings = new ESI { ESISalaryLimit = 12000 }
			};
			//Act
			var dd = src.CheckESIApplicability(esiWages, esipreviousMonth);

			//Assert
			Assert.Equal(result, dd);
		}

		[Fact]
		public async Task ESICalculation()
		{
			#region Arrange
			var empId = Guid.NewGuid();
			var id = Guid.NewGuid();

			var paySheets = new List<PaySheet>()
			{
				new PaySheet(){ EmployeeID=empId,ESIApplied=true,
					PayMonth=new PayMonth(){End=DateTime.Parse("2020-07-10")}},

				new PaySheet(){ EmployeeID=Guid.Parse("2B74C53D-0483-496A-8CFF-5D4A9C0B4889"),ESIApplied=false,
					PayMonth=new PayMonth(){End=DateTime.Parse("2020-07-10")}},
			};

			var src = new PayCalculator(uow.Object)
			{
				MonthStart = DateTime.Parse("2020-07-10"),
				ESISettings = new ESI
				{
					PaySettingsId = Guid.NewGuid(),
					EnableESI = 1,
					EmployeesCont = 12,
					ESISalaryLimit = 21000
				},
				Earnings = new List<EarningComponent> { new EarningComponent() { ID = id, ESIContribution = true } },
			};

			EarningComponent earningComp = new() { ID = id, ESIContribution = true };

			EmpStatutory empStatutory = new()
			{
				ID = Guid.NewGuid(),
				EnableESI = 1,
				ESINo = "TS70"
			};
			var paySheetEarnings = new List<PaySheetEarning>()
			{
				new PaySheetEarning{ComponentId=id,Salary=15000,Earning = 1200,Component=earningComp}
			};
			var salaryEarnings = new List<SalaryEarning>()
			{
				new SalaryEarning{ComponentId=id,Monthly=15000, Component=earningComp}
			};
			#endregion

			//Mock
			var paysheetsMockData = paySheets.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, paysheetsMockData);

			Tuple<bool, int, int, string> dd = await src.ESICalculation(empId, paySheetEarnings, salaryEarnings, empStatutory);

			Assert.True(dd.Item1);
			Assert.Equal(144, dd.Item2);
			Assert.Equal(1200, dd.Item3);
			Assert.Equal("TS70", dd.Item4);
		}

		[Fact]
		public void Employee_Attendance_Deduction()
		{
			decimal monthdays = 24; decimal days = 12;
			EarningComponent earningComponent = new() { ProrataBasis = true };

			var salEarning = new List<SalaryEarning>() { new SalaryEarning() { Component = earningComponent, Monthly = 12000 } };


			var src = new PayCalculator(uow.Object);
			var dd = PayCalculator.AttendanceDeductions(salEarning, monthdays, days);

			Assert.Equal(6000, dd);
		}

		[Fact]
		public void Pay_Deduction()
		{
			var empId = Guid.NewGuid(); decimal monthDays = 24; decimal workdays = 12; decimal lop = 2;

			DeductionComponent deductComponent = new() { Deduct = 1, ProrataBasis = true, Name = "EPF" };

			var salDeduction = new List<SalaryDeduction>()
			{ new SalaryDeduction() { DeductionId=Guid.NewGuid(),Deduction = deductComponent, Monthly = 12000 },
			  new SalaryDeduction() { DeductionId=Guid.NewGuid(),Deduction = deductComponent, Monthly = 15000 }
			};

			var src = new PayCalculator(uow.Object);
			var dd = PayCalculator.PayDeductions(empId, monthDays, workdays, lop, salDeduction);

			Assert.Equal(2, dd.Count);
		}

		[Fact]
		public void PayDual_Deduction()
		{
			var src = new PayCalculator(uow.Object)
			{
				Deductions = new List<DeductionComponent>() { new DeductionComponent() { EarningId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c") } }
			};
			EarningComponent earningComp = new() { ID = Guid.NewGuid() };

			var deductComponent = new List<DeductionComponent>() { new DeductionComponent() { ID = Guid.NewGuid(), Deduct = 1, ProrataBasis = true, Name = "EPF", EarningId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"), Earning = earningComp } };

			var paySheetEarning = new List<PaySheetEarning>()
			{
				new PaySheetEarning {PaySheetId=Guid.NewGuid(),ComponentId=Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),Salary=15000,Earning = 1200},
				new PaySheetEarning{ComponentId=Guid.NewGuid(),Salary=15000,Earning = 1200}
			};

			//Mock
			var DeductionCompMockData = deductComponent.AsQueryable().BuildMockDbSet();
			SetData.MockDeductionComponents(uow, _context, DeductionCompMockData);


			//Act

			var dd = src.PayDualDeductions(paySheetEarning);

			Assert.Single(dd);
		}

		[Fact]
		public void Employee_PayEarning()
		{
			var paySheetId = Guid.NewGuid();
			decimal monthDays = 24; decimal workdays = 12; decimal lop = 2;

			EarningComponent earningComponent = new() { EarningType = 1, ProrataBasis = true, Name = "EPF" };

			var salEarning = new List<SalaryEarning>()
			{ new SalaryEarning() { ComponentId=Guid.NewGuid(),Component = earningComponent, Monthly = 12000 },
			  new SalaryEarning() { ComponentId=Guid.NewGuid(),Component = earningComponent, Monthly = 15000 }
			};


			var src = new PayCalculator(uow.Object);
			var dd = PayCalculator.PayEarrings(paySheetId, monthDays, workdays, lop, salEarning);

			Assert.Equal(2, dd.Count);
		}

		[Fact]
		public async Task Calculate_LoanAmount()
		{
			#region Arrange

			var empId = Guid.NewGuid();
			var payMonthId = Guid.NewGuid();
			var loanId = Guid.NewGuid();

			var src = new PayCalculator(uow.Object);

			var loan = new List<Loan>()
			 {
				 new Loan
				 {
					 ID=loanId,
					 EmployeeId = empId,
					 Due = 1200,
					 MonthlyAmount=120
				 }
			 };

			var loanDeduction = new List<LoanDeduction>()
			{
				new LoanDeduction()
				{
					EmployeeID=empId,
					PayMonthId=payMonthId,
					Deducted=120,
					LoanID=loanId,
					Loan=new Loan()
					{
						ID=loanId,
						MonthlyAmount=1200,
					},

				}

			};
			#endregion

			//Mock
			var loanDeductionMockData = loanDeduction.AsQueryable().BuildMockDbSet();
			SetData.MockLoanDeductionAsync(uow, _context, loanDeductionMockData);

			var loanMockData = loan.AsQueryable().BuildMockDbSet();
			SetData.MockLoanAsync(uow, _context, loanMockData);

			var dd = await src.CalcualteLoanAmount(empId, payMonthId);

			Assert.Equal(120, dd);
		}

		[Fact]//Loan loans _Null
		public async Task Calculate_LoanAmount_DataNotExist_Returns_LoanAmount()
		{
			#region Arrange

			var empId = Guid.NewGuid();
			var payMonthId = Guid.NewGuid();
			var loanId = Guid.NewGuid();
			var src = new PayCalculator(uow.Object);

			var loan = new List<Loan>()
			 {
				 new Loan
				 {
					 ID=loanId,
					 EmployeeId = Guid.NewGuid(),
					 Due = 1200,
					 MonthlyAmount=120
				 }
			 };

			var loanDeduction = new List<LoanDeduction>()
			{

				new LoanDeduction()
				{
					EmployeeID=empId,
					PayMonthId=payMonthId,
					LoanID=Guid.NewGuid(),
					Loan=new Loan()
					{
						ID=Guid.NewGuid(),
						MonthlyAmount=1200,
					},
					Deducted=1500
				}
			};
			#endregion

			//Mock
			var loanDeductionMockData = loanDeduction.AsQueryable().BuildMockDbSet();
			SetData.MockLoanDeductionAsync(uow, _context, loanDeductionMockData);

			var loanMockData = loan.AsQueryable().BuildMockDbSet();
			SetData.MockLoanAsync(uow, _context, loanMockData);

			var dd = await src.CalcualteLoanAmount(empId, payMonthId);

			Assert.Equal(1200, dd);
		}

		[Fact]//Loan loans NotNull  
		public async Task Calculate_LoanAmount_DataExist_Returns_LoanAmount()
		{
			#region Arrange

			var empId = Guid.NewGuid();
			var payMonthId = Guid.NewGuid();
			var loanId = Guid.NewGuid();
			var src = new PayCalculator(uow.Object);

			var loan = new List<Loan>()
			 {
				 new Loan
				 {
					 ID=loanId,
					 EmployeeId = empId,
					 Due = 1200,
					 MonthlyAmount=120
				 }
			 };

			var loanDeduction = new List<LoanDeduction>()
			{
				new LoanDeduction()
				{
					EmployeeID=empId,
					PayMonthId=payMonthId,
					Loan=new Loan()
					{
						ID=Guid.NewGuid(),
						MonthlyAmount=1200,
					},
					Deducted=150
				}
			};
			#endregion

			//Mock
			var loanDeductionMockData = loanDeduction.AsQueryable().BuildMockDbSet();
			SetData.MockLoanDeductionAsync(uow, _context, loanDeductionMockData);

			var loanMockData = loan.AsQueryable().BuildMockDbSet();
			SetData.MockLoanAsync(uow, _context, loanMockData);

			var dd = await src.CalcualteLoanAmount(empId, payMonthId);

			Assert.Equal(120, dd);
		}

		[Fact]
		public async Task Calculate_IncomeTax_DataExist_Updates()
		{
			#region Arrange
			var employee = _employeeData.GetAllEmployeesData();
			var empId = Guid.Parse("D2B349A3-CC44-11EC-BD15-001E67DFC742");
			var financialId = Guid.NewGuid();
			var stateId = Guid.NewGuid();

			var declarationSetting = new DeclarationSetting()
			{
				ID = Guid.NewGuid(),
			};

			var paySheet = new List<PaySheet>()
			{
				new PaySheet()
				{
					ID=Guid.NewGuid(),
					EmployeeID=empId,
					PayMonth = new PayMonth
					{
						ID=Guid.NewGuid(),
						FinancialYearId=financialId,
						Status=4,
					}
				}
			};
			var payCalculator = new PayCalculator(uow.Object)
			{
				PayMonth = 5,
				PaidMonths = 3,
				PayYear = 2021,
				FYId = financialId,
				DeclarationSetting = declarationSetting
			};

			PaySheet paySheets = new()
			{
				ID = Guid.NewGuid(),
				EmployeeID = empId,
				Tax = 12000,
			};

			var incomeTaxLimit = new List<IncomeTaxLimit>()
			{
				new IncomeTaxLimit()
				{
					EmployeeId=empId,
					Amount=12000,
					Month=5,
					Year=2021
				}
			};
			var prevEmp = new List<PrevEmployment>() { new PrevEmployment { DeclarationId = Guid.NewGuid() } };

			var declarations = new List<Declaration>()
			{
				new Declaration()
				{
					 EmployeeId=empId,
					 FinancialYearId=financialId,
					 TaxPayable=2500,
					 Due=1300
				}
			};
			var payMonths = new List<PayMonth>()
			{
				new PayMonth()
				{
					ID=Guid.NewGuid(),
					FinancialYearId=financialId,
					Status=(int)PayMonthStatus.Released,
				}
			};
			var salaryy = new List<Salary>() { new Salary() { EmployeeId = empId } };
			var salary = new Salary()
			{
				EmployeeId = empId
			};
			var bonus = new List<EmpBonus>() { new EmpBonus
			{
				ID = Guid.NewGuid(),
				EmployeeId = empId,
				ReleasedOn = DateTime.Parse("2020-02-04"),
				Amount = 1000
			} ,
			 new EmpBonus
			{
				ID = Guid.NewGuid(),
				EmployeeId = empId,
				ReleasedOn = DateTime.Parse("2020-02-02"),
				Amount = 2000
			}
			};
			var pTaxSlab = new List<ProfessionalTaxSlab>()
			{
				new ProfessionalTaxSlab
				{
					ID = Guid.NewGuid(),
					From = 0 ,
					To = 1000,Amount = 2000,
					ProfessionalTax = new ProfessionalTax
					{
						ID = Guid.NewGuid(), StateId = stateId ,
					}
				}
			};

			var oldRegim = new List<OldRegimeSlab>() { new OldRegimeSlab() {
							ID = Guid.NewGuid(),
							IncomeFrom = 0 ,
							IncomeTo = 1000,
						}};
			var newRegim = new List<NewRegimeSlab>() { new NewRegimeSlab() {
							ID = Guid.NewGuid(),
							IncomeFrom = 0 ,
							IncomeTo = 2000,
						}};
			#endregion

			#region    Mock

			var mockEmployee = employee.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			var PrevEmployement = prevEmp.AsQueryable().BuildMockDbSet();
			SetData.MockPrevEmployment(uow, _context, PrevEmployement);

			var declarationSettingLimitMockData = new List<DeclarationSetting>() { declarationSetting }.AsQueryable().BuildMockDbSet();
			SetData.MockDeclarationSetting(uow, _context, declarationSettingLimitMockData);

			var incomeTaxLimitMockData = incomeTaxLimit.AsQueryable().BuildMockDbSet();
			SetData.MockIncomeTaxLimit(uow, _context, incomeTaxLimitMockData);

			var DeclarationMockData = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, DeclarationMockData);

			var PayMonthMockData = payMonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, PayMonthMockData);

			var paySheetMockData = paySheet.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, paySheetMockData);

			var SalaryMockData = salaryy.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeSalary(uow, _context, SalaryMockData);

			var bonusMockData = bonus.AsQueryable().BuildMockDbSet();
			SetData.MockEmpBonus(uow, _context, bonusMockData);

			var oldRegimMockData = oldRegim.AsQueryable().BuildMockDbSet();
			SetData.MockOldRegimeSlab(uow, _context, oldRegimMockData);

			var newRegimMockData = newRegim.AsQueryable().BuildMockDbSet();
			SetData.MockNewRegimeSlab(uow, _context, newRegimMockData);

			var pTaxSlabMockData = pTaxSlab.AsQueryable().BuildMockDbSet();
			SetData.MockProfessionalTaxSlab(uow, _context, pTaxSlabMockData);

			#endregion
			payCalculator.TaxCalculater = _taxCalculator.Object;

			_taxCalculator.Setup(x => x.Calculate(It.IsAny<Salary>(), It.IsAny<List<PaySheet>>(), It.IsAny<Declaration>(),
															It.IsAny<DeclarationSetting>(), It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<bool>())).
															Returns(Task.FromResult(new Declaration { Due = 2000, TaxPayable = 4000 }));

			var tax = await payCalculator.CalculateIncomeTax(salary, paySheets, stateId);

			Assert.Equal(12000, tax);
		}

		[Fact]
		public async Task Calculate_IncomeTax_DataNotExist_It_will_Add()
		{
			#region Arrange
			var employee = _employeeData.GetAllEmployeesData();
			var empid = Guid.Parse("D2B349A3-CC44-11EC-BD15-001E67DFC742");
			var financialid = Guid.NewGuid();
			var stateid = Guid.NewGuid();

			var declarationSetting = new DeclarationSetting()
			{
				ID = Guid.NewGuid(),
			};

			var paySheet = new List<PaySheet>()
			{
				new PaySheet()
				{
					ID=Guid.NewGuid(),
					EmployeeID=empid,
					PayMonth = new PayMonth
					{
						ID=Guid.NewGuid(),
						FinancialYearId=financialid,
						Status=4,
					}
				}
			};
			var payCalculator = new PayCalculator(uow.Object)
			{
				PayMonth = 5,
				PaidMonths = 3,
				PayYear = 2021,
				FYId = financialid,
				DeclarationSetting = declarationSetting
			};

			PaySheet paysheets = new()
			{
				ID = Guid.NewGuid(),
				EmployeeID = empid,
				Tax = 12000,
			};

			var incomeTaxLimit = new List<IncomeTaxLimit>()
			{
				new IncomeTaxLimit()
				{
					EmployeeId=empid,
					Amount=12000,
					Month=5,
					Year=2021
				}
			};
			var prevEmp = new List<PrevEmployment>() { new PrevEmployment { DeclarationId = Guid.NewGuid() } };

			var declarations = new List<Declaration>()
			{
				new Declaration()
				{
					 EmployeeId=Guid.NewGuid(),
					 FinancialYearId=financialid,
					 TaxPayable=2500,
					 Due=1300
				}
			};
			var payMonths = new List<PayMonth>()
			{
				new PayMonth()
				{
					ID=Guid.NewGuid(),
					FinancialYearId=financialid,
					Status=(int)PayMonthStatus.Released,
				}
			};
			var salaryy = new List<Salary>() { new Salary() { EmployeeId = empid } };
			var salary = new Salary()
			{
				EmployeeId = empid
			};
			#endregion

			#region    Mock

			var mockEmployee = employee.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			var PrevEmployement = prevEmp.AsQueryable().BuildMockDbSet();
			SetData.MockPrevEmployment(uow, _context, PrevEmployement);

			var declarationSettingLimitMockData = new List<DeclarationSetting>() { declarationSetting }.AsQueryable().BuildMockDbSet();
			SetData.MockDeclarationSetting(uow, _context, declarationSettingLimitMockData);

			var incomeTaxLimitMockData = incomeTaxLimit.AsQueryable().BuildMockDbSet();
			SetData.MockIncomeTaxLimit(uow, _context, incomeTaxLimitMockData);

			var DeclarationMockData = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, DeclarationMockData);

			var PayMonthMockData = payMonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, PayMonthMockData);

			var paySheetMockData = paySheet.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, paySheetMockData);

			var SalaryMockData = salaryy.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeSalary(uow, _context, SalaryMockData);

			payCalculator.TaxCalculater = _taxCalculator.Object;

			_taxCalculator.Setup(x => x.Calculate(It.IsAny<Salary>(), It.IsAny<List<PaySheet>>(), It.IsAny<Declaration>(),
															It.IsAny<DeclarationSetting>(), It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<bool>())).
															Returns(Task.FromResult(new Declaration { Due = 2000, TaxPayable = 4000 }));
			#endregion


			var tax = await payCalculator.CalculateIncomeTax(salary, paysheets, stateid);

			Assert.Equal(12000, tax);
		}


		[Fact]
		public void DeductionAmount_DataNotSatisfies_Returns_Zero()
		{
			bool propate = true; int amount = 1200; decimal monthdays = 18; decimal lopdays = 18;
			var src = new PayCalculator(uow.Object);
			var dd = PayCalculator.DeductionsAmount(propate, amount, monthdays, lopdays);

			Assert.Equal(0, dd);
		}

		[Fact]
		public void DeductionAmount_DataSatisfies_Returns_Value()
		{
			bool propate = true; int amount = 1200; decimal monthdays = 18; decimal lopdays = 15;
			var src = new PayCalculator(uow.Object);
			var dd = PayCalculator.DeductionsAmount(propate, amount, monthdays, lopdays);

			Assert.Equal(1000, dd);
		}


		[Fact]
		public void EarningAfterLOP_DataNotSatisfies_Returns_Zero()
		{
			bool propate = true; int amount = 1200; decimal monthdays = 18; decimal workdays = 10; decimal lopdays = 18;
			var src = new PayCalculator(uow.Object);
			var dd = PayCalculator.EarningsAfterLop(propate, amount, monthdays, workdays, lopdays);

			Assert.Equal(0, dd);
		}

		[Fact]
		public void EarningAfterLOP_DataSatisfies_Returns_Value()
		{
			bool propate = true; int amount = 1200; decimal monthdays = 18; decimal workdays = 12; decimal lopdays = 10;
			var src = new PayCalculator(uow.Object);
			var dd = PayCalculator.EarningsAfterLop(propate, amount, monthdays, workdays, lopdays);

			Assert.Equal(133, dd);
		}


		[Fact]
		public void Salary_DataSatisfies_Returns_CalculatedAmount()
		{
			int amount = 22000; decimal monthdays = 18; decimal workdays = 10;
			var src = new PayCalculator(uow.Object);
			var dd = PayCalculator.Salary(amount, monthdays, workdays);

			Assert.Equal(12222, dd);
		}

		[Fact]
		public void Salary_DataNotSatisfies_Returns_Amount()
		{
			int amount = 18000; decimal monthdays = 12; decimal workdays = 18;
			var src = new PayCalculator(uow.Object);
			var dd = PayCalculator.Salary(amount, monthdays, workdays);

			Assert.Equal(18000, dd);
		}


		[Fact]
		public void CalculatePTax_DataExist_Return_Amount()
		{
			int grossTaxable = 2200;
			var stateid = Guid.NewGuid();

			var profTaxSlabs = new List<ProfessionalTaxSlab>()
			{
				new ProfessionalTaxSlab
				{
					ProfessionalTax= new ProfessionalTax{ StateId=stateid },
					From=1000,
					To=3000,
					Amount=5000
				}
			};
			//Mock
			var profTaxSlabMockData = profTaxSlabs.AsQueryable().BuildMockDbSet();
			SetData.MockProfessionalTaxSlab(uow, _context, profTaxSlabMockData);

			var src = new PayCalculator(uow.Object) { PTaxSlabs = profTaxSlabs };

			var dd = src.CalculatePTax(grossTaxable, stateid);

			Assert.Equal(5000, dd);
		}

		[Fact]
		public void CalculatePTax_DataNotExist_Returns_Zero()
		{
			int grossTaxable = 2200;
			var stateid = Guid.NewGuid();

			var profTaxSlabs = new List<ProfessionalTaxSlab>()
			{
				new ProfessionalTaxSlab
				{
					ProfessionalTax= new ProfessionalTax{ StateId=Guid.NewGuid() },
					From=1000,
					To=3000
				}
			};
			//Mock
			var profTaxSlabMockData = profTaxSlabs.AsQueryable().BuildMockDbSet();
			SetData.MockProfessionalTaxSlab(uow, _context, profTaxSlabMockData);

			var src = new PayCalculator(uow.Object) { PTaxSlabs = profTaxSlabs };

			var dd = src.CalculatePTax(grossTaxable, stateid);

			Assert.Equal(0, dd);
		}


		[Fact]
		public void Loss_Of_Pay_Amount()
		{
			var paySheetEarnings = new List<PaySheetEarning>()
			{
				new PaySheetEarning
				{
					Salary=1800,
					Earning=1500
				}
			};
			var src = new PayCalculator(uow.Object);
			var dd = PayCalculator.LOPAmount(paySheetEarnings);

			Assert.Equal(300, dd);
		}
	}



}
