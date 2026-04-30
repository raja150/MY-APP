using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using Transmart.Services.UnitTests.Services.Payroll.PayRollSetData;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;
using TranSmart.Service;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Payroll
{
	public class PayMonthServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IPayMonthService _service;
		private readonly Mock<DbContext> _context;
		private readonly EmployeeDataGenerator _employeeData;


		public PayMonthServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new PayMonthService(uow.Object);
			_context = new Mock<DbContext>();
			_employeeData = new EmployeeDataGenerator();


		}

		private void MockPayMonthData()
		{
			FinancialYear Finyear = new()
			{
				ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				Closed = false,
			};

			var paymonths = new List<PayMonth>()
			{
				new PayMonth
				{
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"), Status=(int)PayMonthStatus.Open,Year=1995,Month=11,Name = "Nov",Start=DateTime.Parse("2022-06-06"),FinancialYear=Finyear,FinancialYearId=Guid.Parse("8D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				},
				new PayMonth
				{
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"), Status=3,Year=1996,Month=15,Name = "Aug",Start=DateTime.Parse("2022-05-07"),FinancialYear=Finyear, FinancialYearId=Guid.Parse("7D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				},
				new PayMonth
				{
					ID=Guid.Parse("D2B349A3-CC44-11EC-BD15-001E67DFC742"), Status=(int)PayMonthStatus.Released,Year=1995,Month=5,Name = "June",FinancialYear=Finyear, FinancialYearId=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				},
			};

			// Mock
			var mockSet = paymonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, mockSet);
		}

		private void MockPaySheetData()
		{
			Employee employee = _employeeData.GetEmployeeData();

			PayMonth payMonth = new()
			{
				FinancialYearId = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				Start = DateTime.Parse("2022-06-06"),
				Status = (int)PayMonthStatus.Released
			};
			var paySheet = new List<PaySheet>()
			{
				new PaySheet
				{
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					PayMonthId=Guid.Parse("D2B349A3-CC44-11EC-BD15-001E67DFC742"),
					PayMonth=payMonth,
					Employee=employee,
					EmployeeID = Guid.Parse("594D5FAC-A52C-4949-4FD2-08DA6BDE3F89"),
					Status=3,
					PayslipMailedOn=DateTime.Now,
					Hold=false,
					PTax=1000,
					Tax=500
				},
			};
			//Mock
			var mockSet = paySheet.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, mockSet);
		}


		[Fact]
		public async Task GetMethod()
		{
			var payMonthId = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559");

			MockPayMonthData();

			// Act
			var payMonth = await _service.GetById(payMonthId);
			// Assert
			Assert.Equal(payMonthId, payMonth.ID);
		}

		[Fact]
		public async Task GetPayMonth()
		{
			var payMonthId = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559");

			MockPayMonthData();

			// Act
			var payMonth = await _service.GetPayMonth(payMonthId);
			// Assert
			Assert.Equal(payMonthId, payMonth.ID);
		}

		[Fact]
		public async Task GetPayMonths()
		{
			MockPayMonthData();
			// Act
			var payMonth = await _service.GetPayMonths();
			// Assert
			Assert.Equal(2, payMonth.Count());
		}

		[Fact]
		public async Task GetAllMonths()
		{
			MockPayMonthData();

			//Act 
			var payMonth = await _service.GetAllMonths();

			//Assert
			Assert.Equal(3, payMonth.Count());
		}

		[Fact]
		public async Task GetPayMonthList()
		{
			var payMonthId = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559");

			MockPayMonthData();

			BaseSearch baseSearch = new()
			{
				SortBy = "Employee",
				Size = 10,
				Page = 1,
			};

			//Act
			var payMonth = await _service.GetPayMonthList(payMonthId, baseSearch);

			//Assert
			Assert.Equal(1, payMonth.Count);
		}

		[Fact]
		public async Task GetPayMonthDetails()
		{
			int year = 1995; int month = 5;
			MockPayMonthData();

			// Act
			var payMonth = await _service.GetPayMonthDetails(year, month);

			//Assert
			Assert.Equal(month, payMonth.Month);
			Assert.Equal(year, payMonth.Year);
		}

		[Fact]
		public async Task GetPaysheet()
		{
			IEnumerable<Employee> employees = _employeeData.GetAllEmployeesData();

			BaseSearch baseSearch = new()
			{
				SortBy = "Employee",
				Size = 10,
				Page = 1,
				RefId = Guid.Parse("D2B349A3-CC44-11EC-BD15-001E67DFC742"),
			};

			MockPaySheetData();

			var employeeMockSet = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, employeeMockSet);

			// Act
			var payMonth = await _service.GetPaySheets(baseSearch);

			// Assert
			Assert.Equal(1, payMonth.Count);
		}

		[Fact]
		public async Task GetSheets()
		{
			int year = 1995;
			int month = 5;

			MockPayMonthData();
			MockPaySheetData();

			// Act
			var payMonth = await _service.GetSheets(year, month);

			//Assert
			Assert.Single(payMonth.ToList());
		}

		[Fact]
		public async Task GetHoldPaysheet()
		{
			//Arrange & Mock
			IEnumerable<Employee> employees = _employeeData.GetAllEmployeesData();

			var paySheets = new List<PaySheet>() { new PaySheet { PayMonthId = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"), Hold = true } };

			BaseSearch baseSearch = new()
			{
				SortBy = "Employee",
				Size = 10,
				Page = 1,
				RefId = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
			};

			var employeeMockSet = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, employeeMockSet);

			var paySheetsMockSet = paySheets.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, paySheetsMockSet);

			//Act
			var payMonth = await _service.GetHoldPaysheet(baseSearch);

			//Assert
			Assert.Equal(1, payMonth.Count);
		}

		[Fact]
		public async Task HoldSalary()
		{
			var paySheet = new List<PaySheet>()
			{
				new PaySheet
				{
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					PayMonthId=Guid.Parse("D2B349A3-CC44-11EC-BD15-001E67DFC742"),
					EmployeeID = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					Status=3,
					PayslipMailedOn=DateTime.Now,
					Hold = false
				},
			};
			//Mock
			var mockSet = paySheet.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, mockSet);

			// Act
			await _service.HoldSalary(Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"));

			//Assert
			Assert.True(paySheet.FirstOrDefault().Hold);
			uow.Verify(x => x.SaveChangesAsync());
		}

		[Fact]
		public async Task ReleaseSalary_DataExist_Returns()
		{
			var paySheet = new List<PaySheet>()
			{
				new PaySheet
				{
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					PayMonthId=Guid.Parse("D2B349A3-CC44-11EC-BD15-001E67DFC742"),
					EmployeeID = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					Status=3,
					PayslipMailedOn=DateTime.Now,
					Hold=false
				},
			};
			//Mock
			var mockSet = paySheet.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, mockSet);

			// Act
			var payMonth = await _service.ReleaseSalary(Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"));

			//Assert
			Assert.True(payMonth.HasNoError);
			Assert.False(payMonth.ReturnValue.Hold);
			uow.Verify(x => x.SaveChangesAsync());
		}

		[Fact]
		public async Task ReleaseSalary_DataNotExist_Return_Exception()
		{
			//Arrange & Mock
			var id = Guid.NewGuid();
			var paySheet = new List<PaySheet>()
			{
				new PaySheet
				{
					ID=Guid.NewGuid(),
					PayMonthId=Guid.Parse("D2B349A3-CC44-11EC-BD15-001E67DFC742"),
					EmployeeID = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					Status=3,
					PayslipMailedOn=DateTime.Now,
					Hold=false
				},
			};

			var mockSet = paySheet.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, mockSet);

			// Act
			var payMonth = await _service.ReleaseSalary(id);

			//Assert
			Assert.False(payMonth.HasNoError);
		}

		[Fact]
		public async Task GetPaySheets()
		{
			var payMonthId = Guid.Parse("D2B349A3-CC44-11EC-BD15-001E67DFC742");
			MockPaySheetData();

			// Act
			var payMonth = await _service.GetPaysheet(payMonthId);

			//Assert
			Assert.Single(payMonth.ToList());
		}


		[Fact]
		public async Task GetEmployeePay()
		{
			var id = Guid.Parse("D2B349A3-CC44-11EC-BD15-001E67DFC742");
			var empid = Guid.Parse("594D5FAC-A52C-4949-4FD2-08DA6BDE3F89");
			MockPaySheetData();

			// Act
			var payMonth = await _service.GetEmployeePay(id, empid);

			//Assert
			Assert.Equal(empid, payMonth.EmployeeID);
		}


		[Fact]
		public async Task GetSalarySlips()
		{
			var empid = Guid.Parse("594D5FAC-A52C-4949-4FD2-08DA6BDE3F89");
			MockPaySheetData();

			// Act
			var payMonth = await _service.GetSalarySlips(empid);

			//Assert
			Assert.Single(payMonth.ToList());
		}


		[Fact]
		public async Task PaySlipSendedOn()
		{
			var paySheetid = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559");
			MockPaySheetData();

			// Act
			var paymonth = await _service.PaySlipSendedOn(paySheetid);

			// Assert
			Assert.True(paymonth.HasNoError);
			uow.Verify(m => m.SaveChangesAsync());
		}


		[Fact]
		public async Task AddUpdatePaySheet_DataExist_Updates()
		{
			var paySheet = new List<PaySheet>()
			{
				new PaySheet
				{
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					PayMonthId=Guid.Parse("D2B349A3-CC44-11EC-BD15-001E67DFC742"),
					EmployeeID = Guid.Parse("594D5FAC-A52C-4949-4FD2-08DA6BDE3F89"),
					Status=3,
					PayslipMailedOn=DateTime.Now,
					Hold=false
				},
			};

			var mockSet = paySheet.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, mockSet);

			// Act
			var payMonth = await _service.AddUpdatePaysheet(new List<PaySheet>()
			{
				new PaySheet
				{
					ID=Guid.NewGuid(),
					PayMonthId=Guid.Parse("D2B349A3-CC44-11EC-BD15-001E67DFC742"),
					Status =3,
					Hold = true,
					EmployeeID=Guid.Parse("594D5FAC-A52C-4949-4FD2-08DA6BDE3F89")
				},

			});

			//Assert
			Assert.True(payMonth.HasNoError);
			uow.Verify(m => m.SaveChangesAsync());
		}

		[Fact]
		public async Task AddUpdatePaySheet_DataNotExist_It_Add()
		{
			var paySheet = new List<PaySheet>()
			{
				new PaySheet
				{
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					PayMonthId=Guid.Parse("D2B349A3-CC44-11EC-BD15-001E67DFC742"),
					EmployeeID = Guid.Parse("594D5FAC-A52C-4949-4FD2-08DA6BDE3F89"),
					Status=3,
					PayslipMailedOn=DateTime.Now,
					Hold=false
				},
			};

			var mockSet = paySheet.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, mockSet);

			// Act
			var payMonth = await _service.AddUpdatePaysheet(new List<PaySheet>()
			{
				new PaySheet
				{
					ID=Guid.NewGuid(),
					PayMonthId=Guid.NewGuid(),
					Status =3,
					Hold = false,
					EmployeeID=Guid.NewGuid()
				}

			});

			//Assert
			Assert.True(payMonth.HasNoError);
			uow.Verify(m => m.SaveChangesAsync());
		}


		[Fact]
		public async Task AddUpdatePaySheet_Exception()
		{
			var paySheet = new List<PaySheet>()
			{
				new PaySheet
				{
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					PayMonthId=Guid.Parse("D2B349A3-CC44-11EC-BD15-001E67DFC742"),
					EmployeeID = Guid.Parse("594D5FAC-A52C-4949-4FD2-08DA6BDE3F89"),
					Status=3,
					PayslipMailedOn=DateTime.Now,
					Hold=false
				},
			};

			var mockSet = paySheet.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, mockSet);

			uow.Setup(x => x.SaveChangesAsync()).Throws(new Exception("Error when save data"));

			// Act
			var payMonth = await _service.AddUpdatePaysheet(new List<PaySheet>()
			{
				new PaySheet
				{
					ID=Guid.NewGuid(),
					PayMonthId=Guid.NewGuid(),
					Status =3,
					Hold = false,
					EmployeeID=Guid.NewGuid()
				},

			});

			//Assert
			Assert.False(payMonth.HasNoError);
		}


		[Fact]
		public async Task TaxDeduction()
		{
			var id = Guid.Parse("D2B349A3-CC44-11EC-BD15-001E67DFC742");
			PaySheet paySheet = new()
			{
				PayMonthId = id,
			};
			DeductionComponent component = new()
			{
				Deduct = 1,
			};

			MockPaySheetData();

			var paySheetded = new List<PaySheetDeduction>()
			{
				new PaySheetDeduction
				{
					ID=Guid.Parse("104A8B21-CED2-4A09-5158-08DA64AB8559"),
					PaySheet = paySheet,
					Component=component,
				},
				new PaySheetDeduction
				{
					ID=Guid.Parse("6D73C8BA-4B04-4C34-5157-08DA64AB8559"),
					PaySheet= paySheet,
					Component=component,
				}
			};

			var dedComponents = new List<DeductionComponent>()
			{
				new DeductionComponent
				{
					ID=Guid.Parse("104A8B21-CED2-4A09-5158-08DA64AB8559"),
					Status=false,
					Deduct=1,

				},
				new DeductionComponent
				{
					ID=Guid.Parse("6D73C8BA-4B04-4C34-5157-08DA64AB8559"),
					Deduct=1,
					Status=true,
				}
			};
			//Mock
			var DedComponentmockSet = dedComponents.AsQueryable().BuildMockDbSet();
			SetData.MockDeductionComponents(uow, _context, DedComponentmockSet);

			var PaySheetDedmockSet = paySheetded.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheetDeduction(uow, _context, PaySheetDedmockSet);


			//Act
			var payMonth = await _service.TaxDeductions(id);

			//Assert
			Assert.Equal(2, payMonth.Deductions.Count);
		}


		[Fact]
		public async Task CheckIsPayMonthsOpen()
		{
			int year = 1995;
			int month = 11;
			MockPayMonthData();

			// Act
			var payMonth = await _service.CheckPayMonthIsOpen(year, month);

			//Assert
			Assert.True(payMonth);
		}

		[Fact]
		public async Task GetEmployeeAnnualSalary()
		{
			var empid = Guid.Parse("594D5FAC-A52C-4949-4FD2-08DA6BDE3F89");
			var fyid = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559");

			MockPayMonthData();

			MockPaySheetData();

			// Act
			var payMonth = await _service.GetEmployeeAnnualSalary(empid, fyid);

			//Assert
			Assert.Single(payMonth.ToList());
		}
	}
}
