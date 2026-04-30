using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MockQueryable.Moq;
using Moq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using Transmart.Services.UnitTests.Services.Payroll.Data;
using Transmart.Services.UnitTests.Services.Payroll.PayRollSetData;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Enums;
using TranSmart.Service;
using TranSmart.Service.Payroll;
using TranSmart.Service.PayRoll;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Payroll
{

	public class PayRollServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly PayRollService payRollService;
		private readonly Mock<DbContext> _context;
		private readonly EmployeeDataGenerator _employeeData;
		public PayRollServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			payRollService = new PayRollService(uow.Object);
			_context = new Mock<DbContext>();
			_employeeData = new EmployeeDataGenerator();
		}

		[Theory]
		[ClassData(typeof(ActiveEmployeeEmployeeWorkdaysDataGenerator))]
		public void ActiveEmployeeWorkDays(decimal monthDays, DateTime startDate, DateTime endDate,
			DateTime joinData, decimal WorkDays)
		{
			//Act
			var result = payRollService.ActiveEmployeeWorkdays(monthDays, startDate, endDate, joinData);

			//Assert
			Assert.Equal(WorkDays, result);
		}

		[Theory]
		[ClassData(typeof(ResignedEmployeeEmployeeWorkdaysDataGenerator))]
		public void ResignedEmployeeWorkdays(decimal monthDays, DateTime startDate, DateTime endDate,
			DateTime joinData, DateTime? leavingOn, decimal WorkDays)
		{

			//Act
			var result = payRollService.ResignedEmployeeWorkdays(monthDays, startDate, endDate, joinData, leavingOn.Value);

			//Assert
			Assert.Equal(WorkDays, result);
		}

		[Fact]
		public async Task Release_Status_Release_Throw_Exception()
		{
			// Arrange & Mock
			var payMonthid = Guid.NewGuid();

			var paymonth = new List<PayMonth>()
			{
				new PayMonth
				{
					ID=payMonthid,
					Year=2022,
					Month=5,
					Status=(int)PayMonthStatus.Released,
					Start=DateTime.Parse("2022-07-13 "),
					End=DateTime.Parse("2022-07-10 ")
				},

			};

			var mockSet = paymonth.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, mockSet);

			// Act
			var src = await payRollService.Release(payMonthid);

			//Assert
			Assert.False(src.HasNoError);
		}

		[Fact]
		public async Task Release_DataNotExist_Throw_Exception()
		{
			// Arrange & Mock
			var payMonthid = Guid.NewGuid();

			var paymonth = new List<PayMonth>()
			{
				new PayMonth
				{
					ID=Guid.NewGuid(),
					Year=2022,
					Month=5,
					Status=(int)PayMonthStatus.Open,
					Start=DateTime.Parse("2022-07-13 "),
					End=DateTime.Parse("2022-07-10 ")
				},

			};

			var mockSet = paymonth.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, mockSet);

			// Act
			var src = await payRollService.Release(payMonthid);

			//Assert
			Assert.False(src.HasNoError);
		}

		[Fact]
		public async Task Release_Status_InProcess_Throw_Exception()
		{
			// Arrange & Mock
			var payMonthid = Guid.NewGuid();

			var paymonth = new List<PayMonth>()
			{
				new PayMonth
				{
					ID=payMonthid,
					Year=2022,
					Month=5,
					Status=(int)PayMonthStatus.InProcess,
					Start=DateTime.Parse("2022-07-13 "),
					End=DateTime.Parse("2022-07-10 ")
				},

			};

			var mockSet = paymonth.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, mockSet);

			// Act
			var src = await payRollService.Release(payMonthid);

			//Assert
			Assert.True(src.HasNoError);
		}

		[Fact]
		public async Task Release_IsCatch_Exception()
		{
			// Arrange & Mock
			uow.Setup(x => x.SaveChangesAsync()).Throws(new Exception("Error when save data"));

			var payMonthid = Guid.NewGuid();

			var paymonth = new List<PayMonth>()
			{
				new PayMonth
				{
					ID=payMonthid,
					Year=2022,
					Month=5,
					Status=(int)PayMonthStatus.InProcess,
					Start=DateTime.Parse("2022-07-13 "),
					End=DateTime.Parse("2022-07-10 ")
				},

			};

			var mockSet = paymonth.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, mockSet);

			// Act
			var src = await payRollService.Release(payMonthid);

			//Assert
			Assert.False(src.HasNoError);
		}

		[Fact]
		public async Task Release_Status_IsOpen_Throw_Exception()
		{
			// Arrange & Mock

			var payMonthid = Guid.NewGuid();
			var paymonth = new List<PayMonth>()
			{
				new PayMonth
				{
					ID=payMonthid,
					Year=2022,
					Month=5,
					Status=(int)PayMonthStatus.Open,
					Start=DateTime.Parse("2022-07-13 "),
					End=DateTime.Parse("2022-07-10 ")
				},

			};

			var mockSet = paymonth.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, mockSet);

			// Act
			var payRoll = await payRollService.Release(payMonthid);

			//Assert
			Assert.False(payRoll.HasNoError);
		}

		[Fact]//month Null
		public async Task Hold_DataNotExist_Throw_Exception()
		{
			var paymonthid = Guid.NewGuid();

			var paymonths = new List<PayMonth>()
			{
				new PayMonth
				{
					ID=Guid.NewGuid(),
					Year=2022,
					Month=8,
					Status=(int)PayMonthStatus.InActive,
					Start=DateTime.Parse("2022-08-01"),
					End=DateTime.Parse("2022-08-31")
				},
				new PayMonth
				{
					ID=Guid.NewGuid(),
					Year=2022,
					Month=9,
					Status=(int)PayMonthStatus.InActive,
					Start=DateTime.Parse("2022-09-01"),
					End=DateTime.Parse("2022-09-30")
				}
			};

			var mockSet = paymonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, mockSet);


			var src = await payRollService.Hold(paymonthid);


			//Assert
			Assert.True(src.HasError);

		}

		[Fact]//Satisfied status Release
		public async Task Hold_Data_NotExist_In_Released_Throw_Exception()
		{
			var payMonthId = Guid.NewGuid();

			var paymonths = new List<PayMonth>()
			{
				new PayMonth
				{
					ID=payMonthId,
					Year=2022,
					Month=5,
					Status=(int)PayMonthStatus.Released,
					Start =DateTime.Parse("2022-07-19"),
					End=DateTime.Parse("2022-07-19")
				}
			};

			var mockSet = paymonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, mockSet);

			var src = await payRollService.Hold(payMonthId);

			//Assert
			Assert.False(src.HasNoError);
		}

		[Fact]//Satisfied status InProcess
		public async Task Hold_Data_NotExist_In_InProcess_Throw_exception()
		{
			var payMonthId = Guid.NewGuid();

			var paymonths = new List<PayMonth>()
			{
				new PayMonth
				{
					ID=payMonthId,
					Year=2022,
					Month=5,
					Status=(int)PayMonthStatus.InProcess,
					Start =DateTime.Parse("2022-07-19"),
					End=DateTime.Parse("2022-07-19")
				}
			};

			var mockSet = paymonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, mockSet);

			var src = await payRollService.Hold(payMonthId);

			//Assert
			Assert.False(src.HasNoError);
		}

		[Fact]//with No Exception
		public async Task Hold_DataExist_DataUpdates()
		{
			var payMonthId = Guid.NewGuid();

			var paymonths = new List<PayMonth>()
			{
				new PayMonth
				{
					ID=payMonthId,
					Year=2022,
					Month=7,
					Status=(int)PayMonthStatus.Active,
					Start =DateTime.Parse("2022-07-01"),
					End=DateTime.Parse("2022-07-31")
				},
				new PayMonth
				{
					ID=payMonthId,
					Year=2022,
					Month=8,
					Status=(int)PayMonthStatus.Open,
					Start =DateTime.Parse("2022-08-01"),
					End=DateTime.Parse("2022-08-31")
				}
			};

			var mockSet = paymonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, mockSet);

			var src = await payRollService.Hold(payMonthId);

			//Assert
			Assert.True(src.HasNoError);
		}

		[Fact]
		public async Task Hold_IsCatch_exception()
		{
			uow.Setup(x => x.SaveChangesAsync()).Throws(new Exception("Error when save data"));

			var payMonthId = Guid.NewGuid();

			var paymonths = new List<PayMonth>()
			{
				new PayMonth
				{
					ID=payMonthId,
					Year=1996,
					Month=5,
					Status=(int)PayMonthStatus.Active,
					Start =DateTime.Parse("2022-07-19"),
					End=DateTime.Parse("2022-07-19")
				}
			};

			var mockSet = paymonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, mockSet);

			var src = await payRollService.Hold(payMonthId);

			//Assert
			Assert.False(src.HasNoError);
		}


		[Fact]
		public async Task Delete_DataExist_Return_Value()
		{
			#region Arrange & Mock
			var payMonthid = Guid.NewGuid();

			var paysheet = new List<PaySheet>()
			{
				new PaySheet{ ID=Guid.NewGuid(), PayMonthId=payMonthid }
			};
			var paymonths = new List<PayMonth>()
			{
				new PayMonth
				{
					ID=payMonthid,
					Year=2022,
					Month=5,
					Status=(int)PayMonthStatus.Released,
					FinancialYearId=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					Start=DateTime.Parse("2022-07-13"),
					End=DateTime.Parse("2022-07-19")
				}
			};

			var declarationSetting = new List<DeclarationSetting>() { new DeclarationSetting() { ID = Guid.NewGuid() } };

			var mockSet = paysheet.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, mockSet);

			var mockSet1 = paymonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, mockSet1);

			var DeclarationSettingmockSet = declarationSetting.AsQueryable().BuildMockDbSet();
			SetData.MockDeclarationSetting(uow, _context, DeclarationSettingmockSet);

			#endregion

			// Act
			var delete = await payRollService.Delete(payMonthid);

			//Assert
			Assert.True(delete.IsSuccess);
			uow.Verify(m => m.SaveChangesAsync());
		}

		[Fact]
		public async Task Delete_DataNotExist_Throw_Exception()
		{
			#region
			var payMonthid = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559");

			var paysheet = new List<PaySheet>()
			{
				new PaySheet
				{
					ID=Guid.NewGuid(),
					PayMonthId=payMonthid,
				},
			};

			var paymonths = new List<PayMonth>()
			{
				  new PayMonth
				{
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					Year=1997,
					Month=6,
					FinancialYearId = Guid.NewGuid(),
					Status=1,
					Start=DateTime.Parse("2022-07-13"),
					End=DateTime.Parse("2022-07-19")
				},
			};

			var declarationSetting = new List<DeclarationSetting>() { new DeclarationSetting() { ID = Guid.NewGuid() } };

			#endregion

			// Mock

			var mockSet = paysheet.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, mockSet);

			var mockSet1 = paymonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, mockSet1);

			var DeclarationSettingmockSet = declarationSetting.AsQueryable().BuildMockDbSet();
			SetData.MockDeclarationSetting(uow, _context, DeclarationSettingmockSet);

			// Act
			var delete = await payRollService.Delete(payMonthid);

			//Assert
			Assert.False(delete.IsSuccess);
		}

		[Fact]
		public async Task Delete_IsCatch_Exception()
		{
			#region Arrange & Mock

			var payMonthid = Guid.NewGuid();

			var paysheet = new List<PaySheet>()
			{
				new PaySheet{ ID=Guid.NewGuid(), PayMonthId=payMonthid }
			};
			var paymonths = new List<PayMonth>()
			{
				new PayMonth
				{
					ID=payMonthid,
					Year=2022,
					Month=5,
					Status=(int)PayMonthStatus.Released,
					Start=DateTime.Parse("2022-07-13"),
					End=DateTime.Parse("2022-07-19")
				}
			};

			var declarationSetting = new List<DeclarationSetting>() { new DeclarationSetting() { ID = Guid.NewGuid() } };

			var mockSet = paysheet.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, mockSet);

			var mockSet1 = paymonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, mockSet1);

			var DeclarationSettingmockSet = declarationSetting.AsQueryable().BuildMockDbSet();
			SetData.MockDeclarationSetting(uow, _context, DeclarationSettingmockSet);

			#endregion
			//Exception
			uow.Setup(x => x.SaveChangesAsync()).Throws(new Exception("Error when save data"));

			// Act
			var delete = await payRollService.Delete(payMonthid);

			//Assert
			Assert.False(delete.IsSuccess);
		}


		[Fact]
		public async Task Process_DataNotExist_Throws_Exception()
		{
			#region Arrange

			var payMonthid = Guid.NewGuid();
			var paySettingid = Guid.NewGuid();
			var stateid = Guid.NewGuid();
			var EmpId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c");


			IEnumerable<Employee> employees = _employeeData.GetAllEmployeesData();

			Employee employee = _employeeData.GetEmployeeData();

			Bank bank = new() { Name = "IDFC", IFSCCode = "IDFC0001" };
			var arrears = new List<Arrear>()
			{
				new Arrear()
				{
				Employee = employee,
				EmployeeID =EmpId,
				Month=6,
				Year=2022,
				Pay=12000
				}
			};
			DeductionComponent dedComponent = new() { ID = Guid.NewGuid() };

			var employeePayInfos = new List<EmployeePayInfo>()
			{
				new EmployeePayInfo
				{
					ID= Guid.NewGuid(),
					EmployeeId =EmpId,
					Employee= _employeeData.GetEmployeeData(),
					PayMode=2,
					Bank=bank,
					AccountNo="180040664"
				}
			};
			var empstatutory = new List<EmpStatutory>()
			{
				new EmpStatutory(){ EmpId=EmpId}
			};

			var loans = new List<Loan>()
			{
				new Loan()
				{
					ID=Guid.Parse("717D5281-0AC3-40F6-2D58-08DA6E01097F"),
					LoanAmount=20000,
					EmployeeId=EmpId
				},
			};
			var saldeductions = new List<SalaryDeduction>()
			{
				new SalaryDeduction()
				{
					ID = Guid.NewGuid(),
					Monthly=1500,
					Deduction= dedComponent,
					DeductionId=Guid.NewGuid()
				}
			};

			var paysheetdeductions = new List<PaySheetDeduction>()
			{
				new PaySheetDeduction(){ ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559") }
			};
			var paysheetearnings = new List<PaySheetEarning>()
			{
				new PaySheetEarning()
				{
					ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
					ComponentId=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
					Earning=1500,
					Salary=15000
				}

			};
			var paysheets = new List<PaySheet>()
			{
				new PaySheet
				{
					ID=Guid.NewGuid(),
					PayMonthId=payMonthid,
					EmployeeID=EmpId,
					PresentDays=0,
					Earnings=paysheetearnings,
					Deductions= paysheetdeductions,
					Loan=1000,
					Gross=15000,
					Net=13500
				},
				new PaySheet
				{
					ID=Guid.NewGuid(),
					PayMonthId=payMonthid,
					EmployeeID=Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce"),
					PresentDays=0,
					Earnings=paysheetearnings,
					Deductions= paysheetdeductions,
					Loan=1000,
					Gross=12000,
					Net=12500
				},
				new PaySheet
				{
					PayMonthId=payMonthid,
					EmployeeID=Guid.Parse("2754ba8b-6f86-4f89-b382-10131adbf7ce"),
					PresentDays=23,
					Loan=1200,
					Gross=13000,
					Net=10000,
					Tax=1500
				}
			};

			var loanDeductions = new List<LoanDeduction>()
			{
				new LoanDeduction{ PayMonthId=payMonthid, LoanID=Guid.Parse("717D5281-0AC3-40F6-2D58-08DA6E01097F")}
			};

			PaySettings paySettings = new() { ID = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"), FYFromMonth = 3 };

			FinancialYear fyear = new()
			{
				ID = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
				PaySettingsId = paySettingid,
				PaySettings = paySettings
			};

			var esi = new List<ESI>()
			{
				new ESI { PaySettingsId= paySettingid }
			};
			var earningcComponentList = new List<EarningComponent>()
			{
				new EarningComponent()
				{
					ID = Guid.NewGuid(),
					EarningType=3,
				}
			};
			EarningComponent earningComponents = new()
			{
				ID = Guid.NewGuid(),
				Name = "Salary",
				EarningType = 1,
			};

			var deductionComponents = new List<DeductionComponent>()
			{
				new DeductionComponent
				{
					ID= Guid.NewGuid(),
				}
			};

			var epf = new List<EPF>()
			{
				new EPF
				{
					PaySettingsId = paySettingid
				}
			};
			var payCut = new List<IncentivesPayCut>()
			{
				new IncentivesPayCut
				{
					ID=Guid.Parse("f5890d6a-3358-4e49-9b43-16320ec73876"),
					EmployeeId=EmpId,
					Year=1996,
					Month=5,
					Employee=_employeeData.GetAllEmployeesData().ToList()[0]
				},
				  new IncentivesPayCut
				  {
					ID=Guid.Parse("10800a56-aa28-4bd8-a677-2895a5e32f11"),
					EmployeeId=Guid.NewGuid(),
					Year=1997,
					Month=9,
					Employee=_employeeData.GetAllEmployeesData().ToList()[0]
				  },
			};

			var declarationSetting = new List<DeclarationSetting>()
			{
				new DeclarationSetting
				{
					ID = Guid.NewGuid(),
					PaySettingsId=  paySettingid,
					MaxLimitEightyC=8,
					MaxLimitEightyD=7,
					TaxDedLastMonth =5,
				},

				new DeclarationSetting
				{
					ID = Guid.NewGuid(),
					PaySettingsId=  paySettingid,
					MaxLimitEightyC=1,
					MaxLimitEightyD=0,
					TaxDedLastMonth=8
				},
			};

			var incomeTaxLimit = new List<IncomeTaxLimit>()
			{
				new IncomeTaxLimit()
				{
					EmployeeId=EmpId,Month=5,Year=2022
				}
			};


			var taxSlabs = new List<ProfessionalTaxSlab>()
			{
				new ProfessionalTaxSlab()
				{
					ID = Guid.NewGuid(),
					ProfessionalTax= new ProfessionalTax(){ StateId = stateid,},
					From=1000,
					To=10000
				},
			};

			var paymonths = new List<PayMonth>()
			{
				new PayMonth
				{
					ID=payMonthid,
					FinancialYearId=Guid.NewGuid(),
					Days=15,
					Year=1996,
					Month=5,
					Status=(int)PayMonthStatus.Released,
					Start=DateTime.Parse("2022-07-13 "),
					End=DateTime.Parse("2022-07-19 "),
					FinancialYear=fyear,
				},
				  new PayMonth
				  {
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					Year=1997,
					Month=6,
					Status=4,
					Start=DateTime.Parse("2022-07-13"),
					End=DateTime.Parse("2022-07-19"),
					FinancialYear=fyear
				  },
			};

			var earning = new List<SalaryEarning>()
			{
				new SalaryEarning()
				{
				 ID=Guid.NewGuid(),
				 Monthly=12000,
				 Component=earningComponents,
				 ComponentId=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				}
			};

			var salaries = new List<Salary>()
			{
				new Salary
				{
					ID=payMonthid,
					EmployeeId=Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
					Earnings = earning,
					Deductions=saldeductions,
					Monthly=15000
				},

				new Salary
				{
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					EmployeeId=Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce"),
					Earnings = earning,
					Deductions=saldeductions
				},
				new Salary
				{
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					EmployeeId=Guid.Parse("2754ba8b-6f86-4f89-b382-10131adbf7ce"),
					Earnings = earning,
					Deductions=saldeductions
				},
			};

			var attendanceSum = new List<AttendanceSum>()
			{
				new AttendanceSum()
				{
					EmployeeId=Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
					Employee= _employeeData.GetAllEmployeesData().ToList()[0],
					Month=5,
					Year=1996,
					Present = 0.5m,
					LOP=1.5m,
					Unauthorized=2.2m,
				},
				  new AttendanceSum
				  {
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					Employee= _employeeData.GetAllEmployeesData().ToList()[0]
				  },
			};

			var paySheetDeductions = new List<PaySheetDeduction>()
			{
				new PaySheetDeduction()
				{
					PaySheetId=Guid.NewGuid(),
					ComponentId=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
					PaySheet = new PaySheet()
					{
						 ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
						 WorkDays=26,
						 LOPDays=1,
					}
		}
			};
			var paysheetEarnings = new List<PaySheetEarning>()
			{
				new PaySheetEarning()
				{
					PaySheetId=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					ComponentId=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				}
			};

			var declartions = new List<Declaration>()
			{
				new Declaration()
				{
					EmployeeId=Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
					FinancialYearId=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
				}
			};

			#endregion

			#region Mock

			var arrearMockSet = arrears.AsQueryable().BuildMockDbSet();
			SetData.MockArrear(uow, _context, arrearMockSet);

			var payMonthmockSet = paymonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, payMonthmockSet);

			var IncometaxmockSet = incomeTaxLimit.AsQueryable().BuildMockDbSet();
			SetData.MockIncomeTaxLimit(uow, _context, IncometaxmockSet);

			var proftaxSlabmockSet = taxSlabs.AsQueryable().BuildMockDbSet();
			SetData.MockProfessionalTaxSlab(uow, _context, proftaxSlabmockSet);

			var declarationmockSet = declartions.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, declarationmockSet);

			var declarationsetmockSet = declarationSetting.AsQueryable().BuildMockDbSet();
			SetData.MockDeclarationSetting(uow, _context, declarationsetmockSet);

			var EsimockSet = esi.AsQueryable().BuildMockDbSet();
			SetData.MockESI(uow, _context, EsimockSet);

			var epfmockSet = epf.AsQueryable().BuildMockDbSet();
			SetData.MockEPF(uow, _context, epfmockSet);

			var incentivePaymockSet = payCut.AsQueryable().BuildMockDbSet();
			SetData.MockIncentivePayCut(uow, _context, incentivePaymockSet);

			var employeeMockSet = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, employeeMockSet);

			var empStatutoryMockSet = empstatutory.AsQueryable().BuildMockDbSet();
			SetData.MockEmpStatutory(uow, _context, empStatutoryMockSet);

			var employeePayInfoMockSet = employeePayInfos.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeePayInfo(uow, _context, employeePayInfoMockSet);

			var salaryMockSet = salaries.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeSalary(uow, _context, salaryMockSet);

			var deductionComponentsMockSet = deductionComponents.AsQueryable().BuildMockDbSet();
			SetData.MockDeductionComponents(uow, _context, deductionComponentsMockSet);

			var earningComponentsMockSet = earningcComponentList.AsQueryable().BuildMockDbSet();
			SetData.MockEarningComponents(uow, _context, earningComponentsMockSet);

			var paySheetMockSet = paysheets.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, paySheetMockSet);

			var loanDedMockSet = loanDeductions.AsQueryable().BuildMockDbSet();
			SetData.MockLoanDeductionAsync(uow, _context, loanDedMockSet);

			var loanMockSet = loans.AsQueryable().BuildMockDbSet();
			SetData.MockLoan(uow, _context, loanMockSet);
			SetData.MockLoanAsync(uow, _context, loanMockSet);

			var attendanceMockSet = attendanceSum.AsQueryable().BuildMockDbSet();
			SetData.MockAttendanceSum(uow, _context, attendanceMockSet);

			var paysheetDedcutionMockSet = paySheetDeductions.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheetDeduction(uow, _context, paysheetDedcutionMockSet);

			var paysheetEarningMockSet = paysheetEarnings.AsQueryable().BuildMockDbSet();
			SetData.MockPaysheetEarningAsync(uow, _context, paysheetEarningMockSet);

			#endregion

			// Act
			var process = await payRollService.Process(payMonthid);

			//Assert
			Assert.False(process.IsSuccess);

		}


		[Fact]
		public async Task Process_Declaration_DataNotExist()
		{
			#region Arrange

			var payMonthid = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var paySettingid = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559");
			var stateid = Guid.Parse("CFDB3D77-0807-491C-B1C6-6290FBB7FC4D");
			var EmpId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c");

			IEnumerable<Employee> employees = _employeeData.GetAllEmployeesData();

			Employee employee = _employeeData.GetEmployeeData();

			Bank bank = new() { Name = "IDFC", IFSCCode = "IDFC0001" };
			var arrears = new List<Arrear>()
			{
				new Arrear()
				{
				Employee = employee,
				EmployeeID = EmpId,
				Month=6,
				Year=1995,
				Pay=12000
				}
			};
			DeductionComponent dedComponent = new() { ID = Guid.NewGuid() };

			var employeePayInfos = new List<EmployeePayInfo>()
			{
				new EmployeePayInfo
				{
					ID= Guid.NewGuid(),
					EmployeeId =EmpId,
					Employee= _employeeData.GetEmployeeData(),
					PayMode=3,
					Bank=bank,
					AccountNo="180040664"
				}
			};
			var empstatutory = new List<EmpStatutory>()
			{
				new EmpStatutory(){ EmpId= EmpId}
			};

			var loans = new List<Loan>()
			{
				new Loan()
				{
					ID=Guid.Parse("717D5281-0AC3-40F6-2D58-08DA6E01097F"),
					LoanAmount=20000,
					EmployeeId=EmpId
				},
			};
			var saldeductions = new List<SalaryDeduction>()
			{
				new SalaryDeduction()
				{
					ID = Guid.NewGuid(),
					Monthly=1500,
					Deduction= dedComponent,
					DeductionId=Guid.NewGuid()
				}
			};

			var paysheetdeductions = new List<PaySheetDeduction>()
			{
				new PaySheetDeduction(){ ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559") }
			};
			var paysheetearnings = new List<PaySheetEarning>()
			{
				new PaySheetEarning()
				{
					ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
					ComponentId=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
					Earning=1500,
					Salary=15000
				}

			};
			var paysheets = new List<PaySheet>()
			{
				new PaySheet
				{
					ID=Guid.NewGuid(),
					PayMonthId=payMonthid,
					EmployeeID=EmpId,
					PresentDays=0,
					Earnings=paysheetearnings,
					Deductions= paysheetdeductions,
					Loan=1000,
					Gross=15000,
					Net=13500
				},
				new PaySheet
				{
					ID=Guid.NewGuid(),
					PayMonthId=payMonthid,
					EmployeeID=Guid.NewGuid(),
					PresentDays=0,
					Earnings=paysheetearnings,
					Deductions= paysheetdeductions,
					Loan=1000,
					Gross=12000,
					Net=12500
				},
				new PaySheet
				{
					PayMonthId=payMonthid,
					EmployeeID=Guid.NewGuid(),
					PresentDays=23,
					Loan=1200,
					Gross=13000,
					Net=10000,
					Tax=1500
				}
			};

			var loanDeductions = new List<LoanDeduction>()
			{
				new LoanDeduction{ PayMonthId=payMonthid, LoanID=Guid.Parse("717D5281-0AC3-40F6-2D58-08DA6E01097F")}
			};

			PaySettings paySettings = new() { ID = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"), FYFromMonth = 3 };

			FinancialYear fyear = new()
			{
				ID = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
				PaySettingsId = paySettingid,
				PaySettings = paySettings
			};

			var esi = new List<ESI>()
			{
				new ESI { PaySettingsId= paySettingid }
			};
			var earningcComponentList = new List<EarningComponent>()
			{
				new EarningComponent()
				{
					ID = Guid.NewGuid(),
					EarningType=3,
				}
			};
			EarningComponent earningComponents = new()
			{
				ID = Guid.NewGuid(),
				Name = "Salary",
				EarningType = 1,
			};

			var deductionComponents = new List<DeductionComponent>()
			{
				new DeductionComponent
				{
					ID= Guid.NewGuid(),
				}
			};

			var epf = new List<EPF>()
			{
				new EPF
				{
					PaySettingsId = paySettingid
				}
			};
			var payCut = new List<IncentivesPayCut>()
			{
				new IncentivesPayCut
				{
					ID=Guid.Parse("f5890d6a-3358-4e49-9b43-16320ec73876"),
					EmployeeId=EmpId,
					Year=1996,
					Month=5,
					Employee=_employeeData.GetAllEmployeesData().ToList()[0]
				},
				  new IncentivesPayCut
				  {
					ID=Guid.Parse("10800a56-aa28-4bd8-a677-2895a5e32f11"),
					EmployeeId=Guid.NewGuid(),
					Year=1997,
					Month=9,
					Employee=_employeeData.GetAllEmployeesData().ToList()[0]
				  },
			};

			var declarationSetting = new List<DeclarationSetting>()
			{
				new DeclarationSetting
				{
					ID = Guid.NewGuid(),
					PaySettingsId=  paySettingid,
					MaxLimitEightyC=0,
					MaxLimitEightyD=7,
					TaxDedLastMonth =5,
				},

				new DeclarationSetting
				{
					ID = Guid.NewGuid(),
					PaySettingsId=  paySettingid,
					MaxLimitEightyC=1,
					MaxLimitEightyD=0,
					TaxDedLastMonth=8
				},
			};

			var incomeTaxLimit = new List<IncomeTaxLimit>()
			{
				new IncomeTaxLimit()
				{
					EmployeeId=EmpId,Month=5,Year=1996
				}
			};


			var taxSlabs = new List<ProfessionalTaxSlab>()
			{
				new ProfessionalTaxSlab()
				{
					ID = Guid.NewGuid(),
					ProfessionalTax= new ProfessionalTax(){ StateId = stateid,},
					From=1000,
					To=10000
				},
			};

			var paymonths = new List<PayMonth>()
			{
				new PayMonth
				{
					ID=payMonthid,
					FinancialYearId=Guid.NewGuid(),
					Days=15,
					Year=2022,
					Month=5,
					Status=(int)PayMonthStatus.InProcess,
					Start=DateTime.Parse("2022-07-13 "),
					End=DateTime.Parse("2022-07-19 "),
					FinancialYear=fyear,
				},
				  new PayMonth
				  {
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					Year=2022,
					Month=6,
					Status=4,
					Start=DateTime.Parse("2022-07-13"),
					End=DateTime.Parse("2022-07-19"),
					FinancialYear=fyear
				  },
			};

			var earning = new List<SalaryEarning>()
			{
				new SalaryEarning()
				{
				 ID=Guid.NewGuid(),
				 Monthly=12000,
				 Component=earningComponents,
				 ComponentId=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				}
			};

			var salaries = new List<Salary>()
			{
				new Salary
				{
					ID=payMonthid,
					EmployeeId=EmpId,
					Earnings = earning,
					Deductions=saldeductions,
					Monthly=15000
				},

				new Salary
				{
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					EmployeeId=Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce"),
					Earnings = earning,
					Deductions=saldeductions
				},
				new Salary
				{
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					EmployeeId=Guid.Parse("2754ba8b-6f86-4f89-b382-10131adbf7ce"),
					Earnings = earning,
					Deductions=saldeductions
				},
			};

			var attendanceSum = new List<AttendanceSum>()
			{
				new AttendanceSum()
				{
					EmployeeId=EmpId,
					Employee= _employeeData.GetAllEmployeesData().ToList()[0],
					Month=5,
					Year=1996,
					Present = 0.5m,
					LOP=1.5m,
					Unauthorized=2.2m,
				},
				  new AttendanceSum
				  {
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					Employee= _employeeData.GetAllEmployeesData().ToList()[0]
				  },
			};

			var paySheetDeductions = new List<PaySheetDeduction>()
			{
				new PaySheetDeduction()
				{
					PaySheetId=Guid.NewGuid(),
					ComponentId=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
					PaySheet = new PaySheet()
					{
						 ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
						 WorkDays=26,
						 LOPDays=1,
					}
		}
			};
			var paysheetEarnings = new List<PaySheetEarning>()
			{
				new PaySheetEarning()
				{
					PaySheetId=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					ComponentId=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				}
			};

			var declartions = new List<Declaration>()
			{
				new Declaration()
				{
					EmployeeId=EmpId,
					FinancialYearId=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
				}
			};

			#endregion

			#region Mock

			var arrearMockSet = arrears.AsQueryable().BuildMockDbSet();
			SetData.MockArrear(uow, _context, arrearMockSet);

			var payMonthmockSet = paymonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, payMonthmockSet);

			var IncometaxmockSet = incomeTaxLimit.AsQueryable().BuildMockDbSet();
			SetData.MockIncomeTaxLimit(uow, _context, IncometaxmockSet);

			var proftaxSlabmockSet = taxSlabs.AsQueryable().BuildMockDbSet();
			SetData.MockProfessionalTaxSlab(uow, _context, proftaxSlabmockSet);

			var declarationmockSet = declartions.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, declarationmockSet);

			var declarationsetmockSet = declarationSetting.AsQueryable().BuildMockDbSet();
			SetData.MockDeclarationSetting(uow, _context, declarationsetmockSet);

			var EsimockSet = esi.AsQueryable().BuildMockDbSet();
			SetData.MockESI(uow, _context, EsimockSet);

			var epfmockSet = epf.AsQueryable().BuildMockDbSet();
			SetData.MockEPF(uow, _context, epfmockSet);

			var incentivePaymockSet = payCut.AsQueryable().BuildMockDbSet();
			SetData.MockIncentivePayCut(uow, _context, incentivePaymockSet);

			var employeeMockSet = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, employeeMockSet);

			var empStatutoryMockSet = empstatutory.AsQueryable().BuildMockDbSet();
			SetData.MockEmpStatutory(uow, _context, empStatutoryMockSet);

			var employeePayInfoMockSet = employeePayInfos.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeePayInfo(uow, _context, employeePayInfoMockSet);

			var salaryMockSet = salaries.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeSalary(uow, _context, salaryMockSet);

			var deductionComponentsMockSet = deductionComponents.AsQueryable().BuildMockDbSet();
			SetData.MockDeductionComponents(uow, _context, deductionComponentsMockSet);

			var earningComponentsMockSet = earningcComponentList.AsQueryable().BuildMockDbSet();
			SetData.MockEarningComponents(uow, _context, earningComponentsMockSet);

			var paySheetMockSet = paysheets.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, paySheetMockSet);

			var loanDedMockSet = loanDeductions.AsQueryable().BuildMockDbSet();
			SetData.MockLoanDeductionAsync(uow, _context, loanDedMockSet);

			var loanMockSet = loans.AsQueryable().BuildMockDbSet();
			SetData.MockLoan(uow, _context, loanMockSet);
			SetData.MockLoanAsync(uow, _context, loanMockSet);

			var attendanceMockSet = attendanceSum.AsQueryable().BuildMockDbSet();
			SetData.MockAttendanceSum(uow, _context, attendanceMockSet);

			var paysheetDedcutionMockSet = paySheetDeductions.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheetDeduction(uow, _context, paysheetDedcutionMockSet);

			var paysheetEarningMockSet = paysheetEarnings.AsQueryable().BuildMockDbSet();
			SetData.MockPaysheetEarningAsync(uow, _context, paysheetEarningMockSet);

			#endregion

			// Act
			var process = await payRollService.Process(payMonthid);

			//Assert
			Assert.False(process.IsSuccess);

		}
		[Fact]
		public async Task Process_Declaration_Test()
		{
			#region Arrange

			var payMonthid = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var paySettingid = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559");
			var stateid = Guid.Parse("CFDB3D77-0807-491C-B1C6-6290FBB7FC4D");
			var EmpId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c");
			var declarationId = Guid.NewGuid();

			IEnumerable<Employee> employees = _employeeData.GetAllEmployeesData();

			Employee employee = _employeeData.GetEmployeeData();

			Bank bank = new() { Name = "IDFC", IFSCCode = "IDFC0001" };
			var arrears = new List<Arrear>()
			{
				new Arrear()
				{
				Employee = employee,
				EmployeeID = EmpId,
				Month=6,
				Year=1995,
				Pay=12000
				}
			};
			DeductionComponent dedComponent = new() { ID = Guid.NewGuid() };

			var employeePayInfos = new List<EmployeePayInfo>()
			{
				new EmployeePayInfo
				{
					ID= Guid.NewGuid(),
					EmployeeId =EmpId,
					Employee= _employeeData.GetEmployeeData(),
					PayMode=3,
					Bank=bank,
					AccountNo="180040664"
				}
			};
			var empstatutory = new List<EmpStatutory>()
			{
				new EmpStatutory(){ EmpId= EmpId}
			};

			var loans = new List<Loan>()
			{
				new Loan()
				{
					ID=Guid.Parse("717D5281-0AC3-40F6-2D58-08DA6E01097F"),
					LoanAmount=20000,
					EmployeeId=EmpId
				},
			};
			var saldeductions = new List<SalaryDeduction>()
			{
				new SalaryDeduction()
				{
					ID = Guid.NewGuid(),
					Monthly=1500,
					Deduction= dedComponent,
					DeductionId=Guid.NewGuid()
				}
			};

			var paysheetdeductions = new List<PaySheetDeduction>()
			{
				new PaySheetDeduction(){ ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559") }
			};
			var paysheetearnings = new List<PaySheetEarning>()
			{
				new PaySheetEarning()
				{
					ID = Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
					ComponentId=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
					Earning=1500,
					Salary=15000
				}

			};
			var paysheets = new List<PaySheet>()
			{
				new PaySheet
				{
					ID=Guid.NewGuid(),
					PayMonthId=payMonthid,
					PayMonth = new PayMonth
					{
						ID=Guid.NewGuid(),
						FinancialYearId = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
						Status = (int)PayMonthStatus.Released
					},
					EmployeeID=EmpId,
					PresentDays=0,
					Earnings=paysheetearnings,
					Deductions= paysheetdeductions,
					Loan=1000,
					Gross=15000,
					Net=13500
				},
				new PaySheet
				{
					ID=Guid.NewGuid(),
					PayMonthId=payMonthid,
					EmployeeID=Guid.NewGuid(),
					PresentDays=0,
					Earnings=paysheetearnings,
					Deductions= paysheetdeductions,
					Loan=1000,
					Gross=12000,
					Net=12500
				},
				new PaySheet
				{
					PayMonthId=payMonthid,
					EmployeeID=Guid.NewGuid(),
					PresentDays=23,
					Loan=1200,
					Gross=13000,
					Net=10000,
					Tax=1500
				}
			};

			var loanDeductions = new List<LoanDeduction>()
			{
				new LoanDeduction{ PayMonthId=payMonthid, LoanID=Guid.Parse("717D5281-0AC3-40F6-2D58-08DA6E01097F")}
			};

			PaySettings paySettings = new() { ID = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"), FYFromMonth = 3 };

			FinancialYear fyear = new()
			{
				ID = Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8522"),
				PaySettingsId = paySettingid,
				PaySettings = paySettings,
				FromDate = DateTime.Now.AddDays(-5),
				ToDate = DateTime.Now.AddDays(5)
			};

			var esi = new List<ESI>()
			{
				new ESI { PaySettingsId= paySettingid }
			};
			var earningcComponentList = new List<EarningComponent>()
			{
				new EarningComponent()
				{
					ID = Guid.NewGuid(),
					EarningType=3,
				}
			};
			EarningComponent earningComponents = new()
			{
				ID = Guid.NewGuid(),
				Name = "Salary",
				EarningType = 1,
			};

			var deductionComponents = new List<DeductionComponent>()
			{
				new DeductionComponent
				{
					ID= Guid.NewGuid(),
				}
			};

			var epf = new List<EPF>()
			{
				new EPF
				{
					PaySettingsId = paySettingid
				}
			};
			var payCut = new List<IncentivesPayCut>()
			{
				new IncentivesPayCut
				{
					ID=Guid.Parse("f5890d6a-3358-4e49-9b43-16320ec73876"),
					EmployeeId=EmpId,
					Year=2022,
					Month=5,
					Employee=_employeeData.GetAllEmployeesData().ToList()[0]
				},
				  new IncentivesPayCut
				  {
					ID=Guid.Parse("10800a56-aa28-4bd8-a677-2895a5e32f11"),
					EmployeeId=Guid.NewGuid(),
					Year=1997,
					Month=9,
					Employee=_employeeData.GetAllEmployeesData().ToList()[0]
				  },
			};

			var declarationSetting = new List<DeclarationSetting>()
			{
				new DeclarationSetting
				{
					ID = Guid.NewGuid(),
					PaySettingsId=  paySettingid,
					MaxLimitEightyC=10000,
					MaxLimitEightyD=15000,
					TaxDedLastMonth =5,
				},

				new DeclarationSetting
				{
					ID = Guid.NewGuid(),
					PaySettingsId=  paySettingid,
					MaxLimitEightyC=1,
					MaxLimitEightyD=0,
					TaxDedLastMonth=8
				},
			};

			var incomeTaxLimit = new List<IncomeTaxLimit>()
			{
				new IncomeTaxLimit()
				{
					EmployeeId=EmpId,Month=5,Year=1996
				}
			};


			var taxSlabs = new List<ProfessionalTaxSlab>()
			{
				new ProfessionalTaxSlab()
				{
					ID = Guid.NewGuid(),
					ProfessionalTax= new ProfessionalTax(){ StateId = stateid,},
					From=1000,
					To=10000
				},
			};

			var paymonths = new List<PayMonth>()
			{
				new PayMonth
				{
					ID=payMonthid,
					FinancialYearId=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					Days=15,
					Year=2022,
					Month=5,
					Status=(int)PayMonthStatus.InProcess,
					Start=DateTime.Parse("2022-07-13 "),
					End=DateTime.Parse("2022-07-19 "),
					FinancialYear=fyear,
				},
				  new PayMonth
				  {
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					Year=2022,
					Month=6,
					Status=4,
					Start=DateTime.Parse("2022-07-13"),
					End=DateTime.Parse("2022-07-19"),
					FinancialYear=fyear
				  },
			};

			var earning = new List<SalaryEarning>()
			{
				new SalaryEarning()
				{
				 ID=Guid.NewGuid(),
				 Monthly=12000,
				 Component=earningComponents,
				 ComponentId=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				}
			};

			var salaries = new List<Salary>()
			{
				new Salary
				{
					ID=payMonthid,
					EmployeeId=EmpId,
					Earnings = earning,
					Deductions=saldeductions,
					Monthly=15000
				},

				new Salary
				{
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					EmployeeId=Guid.Parse("72e20573-6359-4a12-8f9a-2351c16040ce"),
					Earnings = earning,
					Deductions=saldeductions
				},
				new Salary
				{
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					EmployeeId=Guid.Parse("2754ba8b-6f86-4f89-b382-10131adbf7ce"),
					Earnings = earning,
					Deductions=saldeductions
				},
			};

			var attendanceSum = new List<AttendanceSum>()
			{
				new AttendanceSum()
				{
					EmployeeId=EmpId,
					Employee= _employeeData.GetAllEmployeesData().ToList()[0],
					Month=5,
					Year=1996,
					Present = 0.5m,
					LOP=1.5m,
					Unauthorized=2.2m,
				},
				  new AttendanceSum
				  {
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					Employee= _employeeData.GetAllEmployeesData().ToList()[0]
				  },
			};
			var section6AOther = new List<Section6AOther>
			{
				new Section6AOther{DeclarationId = declarationId}
			};

			var paySheetDeductions = new List<PaySheetDeduction>()
			{
				new PaySheetDeduction()
				{
					PaySheetId=Guid.NewGuid(),
					ComponentId=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
					PaySheet = new PaySheet()
					{
						 ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
						 WorkDays=26,
						 LOPDays=1,
					}
		}
			};
			var paysheetEarnings = new List<PaySheetEarning>()
			{
				new PaySheetEarning()
				{
					PaySheetId=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					ComponentId=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
				}
			};

			var declartions = new List<Declaration>()
			{
				new Declaration()
				{
					ID = declarationId,
					EmployeeId=EmpId,
					FinancialYearId=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
				}
			};
			var lateComers = new List<Latecomers> { new Latecomers
			{
				Month = 5,
				Year= 2022,
				EmployeeID = EmpId,
				NumberOfDays=20,
			} };
			var prevEmployment = new List<PrevEmployment>
			{
				new PrevEmployment
				{
				  DeclarationId = declarationId
				}
			};
			var empBonus = new List<EmpBonus> { new EmpBonus
			{
				ReleasedOn = DateTime.Now.AddDays(2),
				Amount= 2000,
			} };
			var oldRegimSlab = new List<OldRegimeSlab> { new OldRegimeSlab
			{
				PaySettingsId = paySettingid,
				IncomeFrom = 1000,
				IncomeTo= 2000,
				TaxRate = 2
			} };
			var hraDeclaration = new List<HraDeclaration>
			{
				new HraDeclaration
				{
					DeclarationId= declarationId,
					Total = 5000,
					Amount=33000
				},
				new HraDeclaration
				{
					DeclarationId= declarationId,
					Total = 6000,
					Amount=30000
				}
			};
			var declarationAllowances = new List<DeclarationAllowance> {
				new DeclarationAllowance
				{
					DeclarationId= declarationId,
					Amount = 2000,
					Name = "declaration-Allowance"
				} };
			var homeLoanPay = new List<HomeLoanPay> {
				new HomeLoanPay {
					DeclarationId= declarationId,
					NameOfLender = "Ravi"
				} };
			var letOutProperty = new List<LetOutProperty> {
				new LetOutProperty {
					DeclarationId= declarationId,
					AnnualRentReceived = 10000
				} };
			var otherIncomeSource = new List<OtherIncomeSources>
			{
				new OtherIncomeSources
				{
					DeclarationId= declarationId,
					InterestOnSaving = 2
				}
			};
			var section6A80CWages = new List<Section6A80CWages> {
				new Section6A80CWages {
					DeclarationId= declarationId,
					Amount = 4000,
				} };
			var section6A80C = new List<Section6A80C> {
				new Section6A80C {
					DeclarationId= declarationId,
					Amount = 4000,
				} };
			var section6A80D = new List<Section6A80D> {
				new Section6A80D {
					DeclarationId= declarationId,
					Amount = 4000,
					Qualified = 500,
					Section80D= new Section80D {
						Limit= 500,
					}
				} };
			#endregion

			#region Mock

			var arrearMockSet = arrears.AsQueryable().BuildMockDbSet();
			SetData.MockArrear(uow, _context, arrearMockSet);

			var payMonthmockSet = paymonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, payMonthmockSet);

			var IncometaxmockSet = incomeTaxLimit.AsQueryable().BuildMockDbSet();
			SetData.MockIncomeTaxLimit(uow, _context, IncometaxmockSet);

			var proftaxSlabmockSet = taxSlabs.AsQueryable().BuildMockDbSet();
			SetData.MockProfessionalTaxSlab(uow, _context, proftaxSlabmockSet);

			var declarationmockSet = declartions.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, declarationmockSet);

			var declarationsetmockSet = declarationSetting.AsQueryable().BuildMockDbSet();
			SetData.MockDeclarationSetting(uow, _context, declarationsetmockSet);

			var EsimockSet = esi.AsQueryable().BuildMockDbSet();
			SetData.MockESI(uow, _context, EsimockSet);

			var epfmockSet = epf.AsQueryable().BuildMockDbSet();
			SetData.MockEPF(uow, _context, epfmockSet);

			var incentivePaymockSet = payCut.AsQueryable().BuildMockDbSet();
			SetData.MockIncentivePayCut(uow, _context, incentivePaymockSet);

			var employeeMockSet = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, employeeMockSet);

			var empStatutoryMockSet = empstatutory.AsQueryable().BuildMockDbSet();
			SetData.MockEmpStatutory(uow, _context, empStatutoryMockSet);

			var employeePayInfoMockSet = employeePayInfos.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeePayInfo(uow, _context, employeePayInfoMockSet);

			var salaryMockSet = salaries.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeSalary(uow, _context, salaryMockSet);

			var deductionComponentsMockSet = deductionComponents.AsQueryable().BuildMockDbSet();
			SetData.MockDeductionComponents(uow, _context, deductionComponentsMockSet);

			var earningComponentsMockSet = earningcComponentList.AsQueryable().BuildMockDbSet();
			SetData.MockEarningComponents(uow, _context, earningComponentsMockSet);

			var paySheetMockSet = paysheets.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, paySheetMockSet);

			var loanDedMockSet = loanDeductions.AsQueryable().BuildMockDbSet();
			SetData.MockLoanDeductionAsync(uow, _context, loanDedMockSet);

			var loanMockSet = loans.AsQueryable().BuildMockDbSet();
			SetData.MockLoan(uow, _context, loanMockSet);
			SetData.MockLoanAsync(uow, _context, loanMockSet);

			var attendanceMockSet = attendanceSum.AsQueryable().BuildMockDbSet();
			SetData.MockAttendanceSum(uow, _context, attendanceMockSet);

			var paysheetDedcutionMockSet = paySheetDeductions.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheetDeduction(uow, _context, paysheetDedcutionMockSet);

			var paysheetEarningMockSet = paysheetEarnings.AsQueryable().BuildMockDbSet();
			SetData.MockPaysheetEarningAsync(uow, _context, paysheetEarningMockSet);

			var lateComersMockSet = lateComers.AsQueryable().BuildMockDbSet();
			SetData.MockLateComers(uow, _context, lateComersMockSet);

			var prevEmpMockSet = prevEmployment.AsQueryable().BuildMockDbSet();
			SetData.MockPrevEmployment(uow, _context, prevEmpMockSet);

			var mockEmpBonus = empBonus.AsQueryable().BuildMockDbSet();
			SetData.MockEmpBonus(uow, _context, mockEmpBonus);

			var mockOldRegimSlab = oldRegimSlab.AsQueryable().BuildMockDbSet();
			SetData.MockOldRegimeSlab(uow, _context, mockOldRegimSlab);

			var mockHraDeclaration = hraDeclaration.AsQueryable().BuildMockDbSet();
			SetData.MockHRADeclaration(uow, _context, mockHraDeclaration);

			var mockDeclarationAllowances = declarationAllowances.AsQueryable().BuildMockDbSet();
			SetData.MockDeclarationAllowance(uow, _context, mockDeclarationAllowances);

			var mockHomeLoanPay = homeLoanPay.AsQueryable().BuildMockDbSet();
			SetData.MockHomeLoanPay(uow, _context, mockHomeLoanPay);
			var mockLetoutProperty = letOutProperty.AsQueryable().BuildMockDbSet();
			SetData.MockLetOutProperty(uow, _context, mockLetoutProperty);
			var mockOtherIncomeSource = otherIncomeSource.AsQueryable().BuildMockDbSet();
			SetData.MockOtherIncomeSource(uow, _context, mockOtherIncomeSource);
			var mockSection6A80CWages = section6A80CWages.AsQueryable().BuildMockDbSet();
			SetData.MockSection6A80CWages(uow,_context, mockSection6A80CWages);
			var mockSection6A80C = section6A80C.AsQueryable().BuildMockDbSet();
			SetData.MockSection6A80C(uow, _context, mockSection6A80C);
			var mockSection6A80D = section6A80D.AsQueryable().BuildMockDbSet();
			SetData.MockSection6A80D(uow, _context, mockSection6A80D);
			var mockSection6AOther = section6AOther.AsQueryable().BuildMockDbSet();
			SetData.MockSection6AOther(uow, _context, mockSection6AOther);
			#endregion

			// Act
			var process = await payRollService.Process(payMonthid);

			//Assert
			Assert.True(process.IsSuccess);

		}

		[Fact]
		public async Task Get_Employee_Present_Days()
		{
			//Arrange
			Guid employeeID = Guid.NewGuid();
			var atdSum = new AttendanceSum { EmployeeId = employeeID, Present = (int)AttendanceStatus.Present, Year = (byte)DateTime.Today.Year, Month = (byte)DateTime.Today.Month, LOP = 1 };
			//Mock
			var repo = new Mock<RepositoryAsync<AttendanceSum>>(_dbContext.Object);
			repo.Setup(x => x.SingleAsync(It.IsAny<Expression<Func<AttendanceSum, bool>>>(),
				  It.IsAny<Func<IQueryable<AttendanceSum>, IOrderedQueryable<AttendanceSum>>>(),
				  It.IsAny<Func<IQueryable<AttendanceSum>, IIncludableQueryable<AttendanceSum, object>>>(), true)).ReturnsAsync(atdSum);

			uow.Setup(m => m.GetRepositoryAsync<AttendanceSum>()).Returns(repo.Object);

			//Act
			var result = await payRollService.AttendanceSum(employeeID, DateTime.Today.Month, DateTime.Today.Year, 0);
			//Assert
			Assert.Equal(new Tuple<decimal, decimal, decimal, decimal>(1, 1, 0, 0), result);
		}

		[Theory]
		[InlineData(1, 2022, 20)]
		[InlineData(4, 2023, 0)]
		public async Task EmployeeAttendanceSum(int month, int year, int days)
		{
			//Arrange
			var employeeId = Guid.NewGuid();
			IEnumerable<AttendanceSum> attendanceData = new List<AttendanceSum>()
			{
				new AttendanceSum
				{
					ID = Guid.NewGuid(), EmployeeId = employeeId, Month = 1, Year = 2022, Present = days, LOP= 1, Unauthorized=0
				},
				new AttendanceSum
				{
					ID = Guid.NewGuid(), EmployeeId = employeeId, Month = 1, Year = 2022, Present = 20, LOP= 1, Unauthorized=0
				},
				new AttendanceSum
				{
					ID = Guid.NewGuid(), EmployeeId = Guid.NewGuid(), Month = 2, Year = 2022, Present = 20, LOP= 1, Unauthorized=0
				}
			}.AsQueryable();

			//ApplyLeave
			var summary = attendanceData.AsQueryable().BuildMockDbSet();

			// setup DbSet for Mock

			_dbContext.Setup(x => x.Set<AttendanceSum>()).Returns(summary.Object);
			var attendanceRepository = new RepositoryAsync<AttendanceSum>(_dbContext.Object);
			uow.Setup(m => m.GetRepositoryAsync<AttendanceSum>()).Returns(attendanceRepository);

			//Act
			var result = await payRollService.AttendanceSum(employeeId, month, year, 0);

			//Assert
			Assert.Equal(days, result.Item1);
		}

		[Fact]
		public async Task AttendanceSum_DataExist_Return()
		{
			var empId = Guid.NewGuid();
			int month = 6;
			int year = 2022;

			var attendanceSum = new List<AttendanceSum>()
			 {
				 new AttendanceSum()
				 {
					 EmployeeId=empId,
					 Month= 6,
					 Year=2022,
					 Present=22,
					 LOP=1,
					 Unauthorized=1,
				 }
			 };

			//Mock
			var AttendanceSumCompMockData = attendanceSum.AsQueryable().BuildMockDbSet();
			SetData.MockAttendanceSum(uow, _context, AttendanceSumCompMockData);

			var result = await payRollService.AttendanceSum(empId, month, year, 0);

			Assert.Equal(22, result.Item1);
			Assert.Equal(1, result.Item2);
			Assert.Equal(1, result.Item3);
		}

		[Fact]
		public async Task AttendanceSum_DataNotExist_Return_Zero()
		{
			var empId = Guid.NewGuid();
			int month = 6;
			int year = 2022;

			var attendanceSum = new List<AttendanceSum>()
			 {
				 new AttendanceSum()
				 {
					 EmployeeId=empId,
					 Month= 5,
					 Year=2022,
					 Present=22,
					 LOP=1,
					 Unauthorized=1,
				 }
			 };

			//Mock
			var AttendanceSumCompMockData = attendanceSum.AsQueryable().BuildMockDbSet();
			SetData.MockAttendanceSum(uow, _context, AttendanceSumCompMockData);

			var result = await payRollService.AttendanceSum(empId, month, year, 0);

			Assert.Equal(0, result.Item1);
		}
	}
}

