using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using Transmart.Services.UnitTests.Services.Payroll.PayRollData;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Enums;
using TranSmart.Service.PayRoll;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Payroll
{
	public class IncomeTaxServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly Mock<DbContext> _context;
		private readonly IncomeTaxNewRegmine _incomeTaxNew;
		private readonly IncomeTaxOldRegmine _incomeTaxOld;
		private readonly IncomeTaxCalculator _incomeTaxCalculator;
		private readonly List<NewRegimeSlab> newRegimeSlab;
		private readonly List<OldRegimeSlab> oldRegimeSlab;
		private readonly DeclarationSetting settings;
		private readonly Salary salaries;
		private readonly int dueMonths;
		private readonly List<PaySheet> paySheet;
		private readonly PrevEmployment prevEmp;
		private readonly EmployeeDataGenerator _employeeData;
		public IncomeTaxServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_context = new Mock<DbContext>();
			salaries = new Salary { Earnings = new List<SalaryEarning>() };
			newRegimeSlab = new List<NewRegimeSlab>();
			oldRegimeSlab = new List<OldRegimeSlab>();
			settings = new DeclarationSetting();
			dueMonths = new int();
			paySheet = new List<PaySheet>();
			prevEmp = new PrevEmployment();
			_employeeData = new EmployeeDataGenerator();
			_incomeTaxNew = new IncomeTaxNewRegmine(uow.Object, newRegimeSlab);
			_incomeTaxOld = new IncomeTaxOldRegmine(uow.Object, settings, salaries, dueMonths, paySheet, prevEmp, oldRegimeSlab);
			_incomeTaxCalculator = new IncomeTaxCalculator(uow.Object, new FinancialYear { });

		}


		private void HRADeclarationMockData()
		{
			var hradeclarations = new List<HraDeclaration>()
			{
				new HraDeclaration(){ DeclarationId=Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578"),Total=12000}
			};
			//Mock
			var mockSet = hradeclarations.AsQueryable().BuildMockDbSet();
			SetData.MockHRADeclaration(uow, _context, mockSet);
		}

		private void NewRegimeMockData()
		{
			var setting = new PaySettings()
			{
				ID = Guid.NewGuid(),
			};
			var newRegime = new List<NewRegimeSlab>()
			{
				new NewRegimeSlab()
				{
					PaySettingsId=Guid.Parse("E4E800A8-CC80-4C52-82FD-122DAA0137EA"),
					PaySettings=setting,
					IncomeFrom =1500,
					IncomeTo=1200,
					TaxRate=5
				}
			};

			//Mock
			var newRegimemockSet = newRegime.AsQueryable().BuildMockDbSet();
			SetData.MockNewRegimeSlab(uow, _context, newRegimemockSet);
		}

		private void OldRegimeMockData()
		{
			var setting = new PaySettings()
			{
				ID = Guid.NewGuid(),
			};
			var oldRegime = new List<OldRegimeSlab>()
			{
				new OldRegimeSlab()
				{
					PaySettingsId=Guid.NewGuid(),
					PaySettings=setting,
					IncomeFrom =1500,
					IncomeTo=1200,
					TaxRate=5
				}
			};

			//Mock
			var OldRegimemockSet = oldRegime.AsQueryable().BuildMockDbSet();
			SetData.MockOldRegimeSlab(uow, _context, OldRegimemockSet);
		}

		private void OtherincomesourcesMockdata()
		{
			var otherincomeSrc = new List<OtherIncomeSources>
			{
				new OtherIncomeSources
				{
					DeclarationId = Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578"),
					OtherSources = 1000,
					InterestOnSaving = 1200,
					InterestOnFD = 1500
				},
				new OtherIncomeSources
				{
					DeclarationId = Guid.Parse("1B74C53D-0483-496A-8CFF-5D4A9C0B4889"),
					OtherSources = 1000,
					InterestOnSaving = 2100,
					InterestOnFD = 1300
				}
			};

			var otherincomeSrcMockSet = otherincomeSrc.AsQueryable().BuildMockDbSet();
			SetData.MockOtherIncomeSources(uow, _context, otherincomeSrcMockSet);
		}

		private void ProfessionalTaxSlabMockData()
		{
			var professionalTaxSlabs = new List<ProfessionalTaxSlab>()
			{
				new ProfessionalTaxSlab()
				{
					ID=Guid.NewGuid(),
					From=5000,
					To=13000,
					Amount=1200,
					ProfessionalTax=new ProfessionalTax
					{
						ID = Guid.NewGuid(),
						StateId = Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578"),
						State = new State
						{
							ID = Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578"),
							Name = "Telangana",
							Country = "India",
							Status = true,
						}
					},

				}
			};
			//Mock
			var professionalTaxSlabsMockSet = professionalTaxSlabs.AsQueryable().BuildMockDbSet();
			SetData.MockProfessionalTaxSlab(uow, _context, professionalTaxSlabsMockSet);
		}

		private void Section6AOthersMockData()
		{
			var section6AOthers = new List<Section6AOther>()
			{
				new Section6AOther()
				{
					DeclarationId=Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578"),
					Qualified=200
				}
			};
			var section6AOthersMockSet = section6AOthers.AsQueryable().BuildMockDbSet();
			SetData.MockSection6AOther(uow, _context, section6AOthersMockSet);
		}

		private void Section6A80DQualifiedMockData()
		{
			Section80D section80D = new()
			{
				Limit = 200,
			};

			var section6A8DQualifies = new List<Section6A80D>()
			{
				new Section6A80D()
				{
					DeclarationId=Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578"),
					Qualified=10,
					Section80D=section80D,

				}
			};
			var section6A8DQualifiesMockSet = section6A8DQualifies.AsQueryable().BuildMockDbSet();
			SetData.MockSection6A80D(uow, _context, section6A8DQualifiesMockSet);
		}

		private void Section6A80CQualifiedMockData()
		{
			var section6A8CQualifies = new List<Section6A80C>()
			{
				new Section6A80C()
				{
					DeclarationId=Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578"),
					Amount=10000

				}
			};
			var section6A8CQualifiesMockSet = section6A8CQualifies.AsQueryable().BuildMockDbSet();
			SetData.MockSection6A80C(uow, _context, section6A8CQualifiesMockSet);
		}

		private void Section6A80CWagesMockData()
		{
			var section6A8CWages = new List<Section6A80CWages>()
			{
				new Section6A80CWages()
				{
					DeclarationId=Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578"),
					ComponentId =Guid.Parse("E4E800A8-CC80-4C52-82FD-122DAA0137EA"),
					Amount=500,
					AddedAt = DateTime.Now
				},
				new Section6A80CWages()
				{
					DeclarationId=Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578"),
					ComponentId =Guid.Parse("E4E800A8-CC80-4C52-82FD-122DAA0137EA"),
					Amount=10000
				},

				new Section6A80CWages()
				{
					ID=Guid.NewGuid(),
					ComponentId=Guid.NewGuid(),
					Amount=12000
				},
				new Section6A80CWages()
				{
					ID=Guid.NewGuid(),
					ComponentId=Guid.Parse("E4E800A8-CC80-4C52-82FD-122DAA0137EA"),
					Amount=15000
				},


			};
			var section6A8CWagesMockSet = section6A8CWages.AsQueryable().BuildMockDbSet();
			SetData.MockSection6A80CWages(uow, _context, section6A8CWagesMockSet);
		}

		private void LetOutPropertyMockData()
		{
			var letOutProperties = new List<LetOutProperty>()
			{
				new LetOutProperty()
				{
					DeclarationId=Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578"),
					AnnualRentReceived=72000,
					MunicipalTaxPaid=12000,
					InterestPaid=2000
				}
			};
			var letOutPropertyMockSet = letOutProperties.AsQueryable().BuildMockDbSet();
			SetData.MockLetOutProperty(uow, _context, letOutPropertyMockSet);
		}

		private void HomeLoanPayMockData()
		{
			var homeLoanInterest = new List<HomeLoanPay>()
			{
				new HomeLoanPay()
				{
					DeclarationId=Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578"),
					InterestPaid=2000
				},
				new HomeLoanPay()
				{
					DeclarationId=Guid.NewGuid(),
					InterestPaid=2002
				}
			};
			var homeLoanMockSet = homeLoanInterest.AsQueryable().BuildMockDbSet();
			SetData.MockHomeLoanPay(uow, _context, homeLoanMockSet);
		}

		private void PaySheetEarningMockData()
		{
			var paySheetEarnings = new List<PaySheetEarning>()
			{
				new PaySheetEarning()
				{
					EarningType=1,
					Earning=123,
				},
				new PaySheetEarning()
				{
					EarningType=2,
					Earning=100,
				}
			};
			var deduction = new List<PaySheetDeduction>()
			{
				new PaySheetDeduction()
				{
					DeductType=1,
					Deduction=100
				}
			};

			var paySheets = new List<PaySheet>()
			{
				new PaySheet()
				{
					Deductions=deduction,
					Earnings=paySheetEarnings,
				}
			};
			var paySheetMockSet = paySheets.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, paySheetMockSet);
		}



		[Fact]
		public async Task OtherIncome_DataExist_Return()
		{
			var declarationid = Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578");

			OtherincomesourcesMockdata();

			//Act 
			var income = await _incomeTaxNew.OtherIncome(declarationid);

			//Assert
			Assert.Equal(3700, income);
		}

		[Fact]
		public async Task OtherIncome_DataNotExist_Return_Zero()
		{
			var declarationid = Guid.Parse("3F598FBF-D2E7-4B72-8A86-0B91CDE40578");

			OtherincomesourcesMockdata();

			//Act 
			var income = await _incomeTaxNew.OtherIncome(declarationid);

			//Assert
			Assert.Equal(0, income);
		}

		[Fact]
		public void CalculateTaxFromSlabsTest()
		{
			int taxableIncome = 250200;
			int taxActual = 10;
			var taxSlabs = new List<TaxBracket>
			{
				new TaxBracket { Low = 0, High = 250000, Rate = 0.00m },
				new TaxBracket { Low = 250001, High = 500000, Rate = 5m },
				new TaxBracket { Low = 500001, High = 1000000, Rate = 20m },
				new TaxBracket { Low = 1000001, High = 99999999, Rate = 30m }
			};
			//Act 
			var tax = _incomeTaxNew.TaxCalculator(taxableIncome, taxSlabs.ToArray());

			//Assert
			Assert.Equal(taxActual, tax);
		}

		[Fact]

		public async Task GetPTaxPredictTest()
		{
			int grossTaxable = 12000;
			var stateid = Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578");

			ProfessionalTaxSlabMockData();

			//Act 
			var tax = await _incomeTaxCalculator.GetPTaxPredict(grossTaxable, stateid);
			//Assert
			Assert.Equal(1200, tax);
		}

		[Fact]
		public void IncomeFromHome()
		{
			//Arrange & Mock

			int incomefromHome = 5000;
			int incomeFromLetOut = 2500;
			int expected = 7500;

			var setting = new List<DeclarationSetting>()
			{
				new DeclarationSetting(){HouseLoanInt = 1250},
			};

			var declarationSettingMockSet = setting.AsQueryable().BuildMockDbSet();
			SetData.MockDeclarationSettings(uow, _context, declarationSettingMockSet);

			//Act
			var income = _incomeTaxOld.IncomeFromHome(incomefromHome, incomeFromLetOut);

			//Assert
			Assert.Equal(expected, income);
		}


		[Fact]
		public async Task VIOthersections()
		{
			var declarationid = Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578");
			Section6AOthersMockData();

			//Act 
			var dd = await _incomeTaxOld.VIOtherSections(declarationid);
			//Assert
			Assert.Equal(200, dd);
		}

		[Fact]
		public async Task VIEightyDQualified()
		{
			int maxLimit = 5;
			var declarationid = Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578");
			Section6A80DQualifiedMockData();

			//Act 
			var eightD = await _incomeTaxOld.VIEightyDQualified(maxLimit, declarationid);
			//Assert
			Assert.Equal(5, eightD);
		}

		[Fact]
		public async Task VIEightyCQualified()
		{
			int maxLimit = 50000;
			int wages = 12000;
			int epf = 1200;
			var declarationid = Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578");

			Section6A80CQualifiedMockData();

			//Act 
			var eightC = await _incomeTaxOld.VIEightyCQualified(maxLimit, wages, epf, 0, declarationid);
			//Assert
			Assert.Equal(23200, eightC);
		}

		[Fact]
		public async Task IncomeFromLetOutProperty()
		{
			var declarationid = Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578");
			LetOutPropertyMockData();

			//Act 
			var letout = await _incomeTaxOld.IncomeFromLetOutProperty(declarationid);
			//Assert
			Assert.Equal(40000, letout.Item2);
		}

		[Fact]
		public async Task HomeLoanInterest_DataExist_Return_PaidInterest()
		{
			var declarationid = Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578");
			int expected = -2000;
			HomeLoanPayMockData();

			//Act 
			var loan = await _incomeTaxOld.HomeLoanInterest(declarationid);
			//Assert
			Assert.Equal(expected, loan.Item2);
		}

		[Fact]
		public async Task HomeLoanInterest_DataNotExist_Return_Zero()
		{
			var declarationid = Guid.Parse("3F598FBF-D2E7-4B72-8A86-0B91CDE40578");
			int expected = 0;
			HomeLoanPayMockData();

			//Act 
			var loan = await _incomeTaxOld.HomeLoanInterest(declarationid);
			//Assert
			Assert.Equal(expected, loan.Item2);
		}


		[Theory]//Issue  
		[InlineData(2, "2F598FBF-D2E7-4B72-8A86-0B91CDE40578", "E4E800A8-CC80-4C52-82FD-122DAA0137EA")]
		public async Task VIEightyCQualifiedFromDeductionsTest(int dueMonths, Guid declarationid, Guid componentid)
		{
			#region Arrange & Mock

			Section6A80CWagesMockData();

			var deduction = new List<PaySheetDeduction>()
			{
				new PaySheetDeduction(){ DeductType=1, Deduction=100}
			};
			var paySheets = new List<PaySheet>()
			{
				new PaySheet(){Deductions=deduction}
			};

			DeductionComponent deductionComp = new() { ID = componentid, Deduct = 1 };

			var salDeductions = new List<SalaryDeduction>()
			{
				new SalaryDeduction()
				{
					DeductionId=Guid.Parse("E4E800A8-CC80-4C52-82FD-122DAA0137EA"),
					Deduction=deductionComp,
					Monthly=500
				}
			};

			var salaryDeductionMockSet = salDeductions.AsQueryable().BuildMockDbSet();
			SetData.MockSalaryDeduction(uow, _context, salaryDeductionMockSet);

			var paySheetMockSet = paySheets.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, paySheetMockSet);

			#endregion

			//Act 
			var Cqualified = await _incomeTaxOld.VIEightyCQualifiedFromDeductions(salDeductions, declarationid, dueMonths, paySheets);
			//Assert
			Assert.Equal(1100, Cqualified);
		}


		[Fact]
		public async Task Allowance_DataExist_Returns_Allowance()
		{
			#region Arrange

			var declarationid = Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578");
			var componentid = Guid.Parse("E4E800A8-CC80-4C52-82FD-122DAA0137EA");
			int DueMonths = 2;

			HRADeclarationMockData();

			EarningComponent earningComp = new()
			{
				ID = componentid,
				EarningType = (int)EarningType.Basic,
			};
			EarningComponent earningComp1 = new()
			{
				ID = componentid,
				EarningType = (int)EarningType.HRA,
				Name = "HRA"
			};
			EarningComponent earningComp2 = new()
			{
				ID = componentid,
				EarningType = (int)EarningType.FoodCoupon,
				Name = "Food"
			};
			var salEarnings = new List<SalaryEarning>()
			{
				new SalaryEarning()
				{
					Component=earningComp1,
					ComponentId=componentid,
					Monthly=1300,
				},
				new SalaryEarning()
				{
					Component=earningComp,
					ComponentId=Guid.NewGuid(),
					Monthly=200
				},
				new SalaryEarning()
				{
					Component=earningComp2,
					ComponentId =componentid,
					Monthly=1500
				},
			};
			var declarationAllowance = new List<DeclarationAllowance>()
			{
				new DeclarationAllowance()
				{
					DeclarationId=declarationid,
					ComponentId=componentid,
					Amount=12000
				},
				new DeclarationAllowance()
				{
					DeclarationId=declarationid,
					ComponentId=Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578"),
					Amount=12000
				},
			};

			DeclarationSetting decsettingss = new()
			{
				PaySettingsId = Guid.Parse("8F589FBF-D2E7-4B72-8A87-0B91CDE40678"),
			};

			Salary salaryy = new() { Earnings = salEarnings };

			int dueMonth = 3;

			var paySheetEarnings = new List<PaySheetEarning>()
			{
				new PaySheetEarning()
				{
					EarningType=(int)EarningType.Basic,
					Earning=2500,
					ComponentId=componentid
				},
				new PaySheetEarning()
				{
					EarningType=(int)EarningType.HRA,
					Earning=100,
				}
			};
			var paySheets = new List<PaySheet>()
			{
				new PaySheet(){ Earnings=paySheetEarnings}
			};

			PrevEmployment prevEmps = new() { ID = Guid.NewGuid() };

			var oldRegimeSlabs = new List<OldRegimeSlab>()
			{
				new OldRegimeSlab(){ IncomeFrom =1200,IncomeTo=1250 }
			};
			#endregion

			var IncomeTax = new IncomeTaxOldRegmine(uow.Object, decsettingss, salaryy, dueMonth, paySheets, prevEmps, oldRegimeSlabs);

			//Mock
			var salaryEarningMockSet = salEarnings.AsQueryable().BuildMockDbSet();
			SetData.MockSalaryEarning(uow, _context, salaryEarningMockSet);

			var paySheetMockSet = paySheets.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, paySheetMockSet);

			var declarationAllowanceMockSet = declarationAllowance.AsQueryable().BuildMockDbSet();
			SetData.MockDeclarationAllowance(uow, _context, declarationAllowanceMockSet);

			//Act 
			var allowance = await IncomeTax.AllowancesCalc(salEarnings, declarationid, DueMonths, paySheets);
			//Assert
			Assert.Equal(9550, allowance);
		}

		[Fact]
		public async Task Allowance()
		{
			#region Arrange

			var declarationid = Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578");
			var componentid = Guid.Parse("E4E800A8-CC80-4C52-82FD-122DAA0137EA");
			int DueMonths = 2;

			HRADeclarationMockData();

			EarningComponent earningComp = new()
			{
				ID = componentid,
				EarningType = (int)EarningType.Basic,
			};

			EarningComponent earningComp2 = new()
			{
				ID = componentid,
				EarningType = (int)EarningType.FoodCoupon,
				Name = "Food"
			};
			var salEarnings = new List<SalaryEarning>()
			{
				new SalaryEarning()
				{
					Component=earningComp,
					ComponentId=componentid,
					Monthly=1300,
				},
				new SalaryEarning()
				{
					Component=earningComp,
					ComponentId=Guid.NewGuid(),
					Monthly=200
				},
				new SalaryEarning()
				{
					Component=earningComp2,
					ComponentId =componentid,
					Monthly=1500
				},
			};
			var declarationAllowance = new List<DeclarationAllowance>()
			{
				new DeclarationAllowance()
				{
					DeclarationId=declarationid,
					ComponentId=componentid,
					Amount=12000
				},
				new DeclarationAllowance()
				{
					DeclarationId=declarationid,
					ComponentId=Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578"),
					Amount=12000
				},
			};

			DeclarationSetting decsettingss = new()
			{
				PaySettingsId = Guid.Parse("8F589FBF-D2E7-4B72-8A87-0B91CDE40678"),
			};

			Salary salaryy = new() { Earnings = salEarnings };

			int dueMonth = 3;

			var paySheetEarnings = new List<PaySheetEarning>()
			{
				new PaySheetEarning()
				{
					EarningType=(int)EarningType.Basic,
					Earning=2500,
					ComponentId=componentid
				},
				new PaySheetEarning()
				{
					EarningType=(int)EarningType.HRA,
					Earning=100,
				}
			};
			var paySheets = new List<PaySheet>()
			{
				new PaySheet(){ Earnings=paySheetEarnings}
			};

			PrevEmployment prevEmps = new() { ID = Guid.NewGuid() };

			var oldRegimeSlabs = new List<OldRegimeSlab>()
			{
				new OldRegimeSlab(){ IncomeFrom =1200,IncomeTo=1250 }
			};
			#endregion

			var IncomeTax = new IncomeTaxOldRegmine(uow.Object, decsettingss, salaryy, dueMonth, paySheets, prevEmps, oldRegimeSlabs);

			//Mock
			var salaryEarningMockSet = salEarnings.AsQueryable().BuildMockDbSet();
			SetData.MockSalaryEarning(uow, _context, salaryEarningMockSet);

			var paySheetMockSet = paySheets.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, paySheetMockSet);

			var declarationAllowanceMockSet = declarationAllowance.AsQueryable().BuildMockDbSet();
			SetData.MockDeclarationAllowance(uow, _context, declarationAllowanceMockSet);

			//Act 
			var allowance = await IncomeTax.AllowancesCalc(salEarnings, declarationid, DueMonths, paySheets);
			//Assert
			Assert.Equal(8100, allowance);

		}

		[Fact]//OldRegime
		public async Task Calculate()
		{
			#region Arrange
			var id = Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578");
			var componentid = Guid.Parse("E4E800A8-CC80-4C52-82FD-122DAA0137EA");
			var employeeid = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c");

			HRADeclarationMockData();

			var paySheetEarnings = new List<PaySheetEarning>()
			{
				new PaySheetEarning(){ EarningType=1,Earning=1230 },
				new PaySheetEarning(){EarningType=2,Earning=1000},
				new PaySheetEarning(){EarningType=9,Earning=100}
			};

			var paySheetDed = new List<PaySheetDeduction>()
			{
				new PaySheetDeduction(){DeductType=1}
			};

			var earningComp = new EarningComponent() { ID = componentid, EarningType = 1 };
			EarningComponent earningComp0 = new() { EarningType = 1 };
			EarningComponent earningComp1 = new() { ID = Guid.NewGuid(), EarningType = 2, Name = "HRA" };
			EarningComponent earningComp2 = new() { ID = componentid, EarningType = 9, Name = "Food" };

			var salEarnings = new List<SalaryEarning>()
			{
				new SalaryEarning()
				{
					Component=earningComp1,
					ComponentId=componentid,
					Monthly=1300
				},
				new SalaryEarning()
				{
					Component=earningComp,
					ComponentId=Guid.NewGuid(),
					Monthly=1200
				},
				new SalaryEarning()
				{
					Component=earningComp2,
					ComponentId =componentid,
					Monthly=1500,
				},

				new SalaryEarning()
				{
					Component=earningComp0,
					ComponentId =componentid,
					Monthly=1500
				},
			};
			var salaryy = new List<Salary>() { new Salary() { Earnings = salEarnings } };

			var declarationAllowance = new List<DeclarationAllowance>()
			{
				new DeclarationAllowance()
				{
					DeclarationId=id,
					ComponentId=componentid,
					Amount=120
				},
				new DeclarationAllowance()
				{
					DeclarationId=Guid.NewGuid(),
					ComponentId=Guid.NewGuid(),
					Amount=120
				}
			};

			var epf = new List<EPF>() { new EPF() { PaySettingsId = id } };

			DeductionComponent Dedcomp = new() { Deduct = 1 };

			var saldeductions = new List<SalaryDeduction>()
			{
				new SalaryDeduction(){Deduction = Dedcomp,Monthly=120}
			};

			var salearnings = new List<SalaryEarning>()
			 {
				 new SalaryEarning()
				 {
					 Monthly = 5000,
					 Annually = 60000,
					 Component=earningComp0
				 },
				  new SalaryEarning()
				  {
					 Monthly = 1000,
					 Annually = 12000,
					 Component=earningComp1,
					 ComponentId=componentid
				  },
				 new SalaryEarning()
				 {
					 Monthly = 10000,
					 Annually = 120000,
					 Component=earningComp2,
					 ComponentId=componentid
				 }
			};

			Section6A80CWagesMockData();

			Section6A80CQualifiedMockData();

			Section6A80DQualifiedMockData();

			Section6AOthersMockData();

			HomeLoanPayMockData();

			LetOutPropertyMockData();

			OtherincomesourcesMockdata();

			Declaration declaration = new()
			{
				ID = id,
				EmployeeId = employeeid,
				Tax = 1100,
				Perquisites = 12000,
				PreviousEmployment = 300000,
				Salary = 20000,
				Allowance = 2000,
				StandardDeduction = 110,
				TaxOnEmployment = 120,
				EPF = 120,
				OtherSections = 150,
			};

			var empStatutory = new List<EmpStatutory>()
			{
				new EmpStatutory(){EmpId = employeeid}
			};

			var earningComponents = new List<EarningComponent>()
			{
				new EarningComponent(){ID=componentid,EarningType=1}
			};

			DeclarationSetting decsettingss = new()
			{
				PaySettingsId = id,
				MaxLimitEightyC = 1200,
				MaxLimitEightyD = 1500
			};
			Salary salary = new()
			{
				EmployeeId = employeeid,
				Earnings = salearnings,
				Deductions = saldeductions
			};

			int dueMonth = 1;

			var paySheets = new List<PaySheet>()
			{
				new PaySheet()
				{
					Earnings=paySheetEarnings,
					EPF=1200,
					Deductions=paySheetDed
				}
			};

			PrevEmployment prevEmps = new()
			{
				ID = Guid.NewGuid(),
			};

			var oldRegimeSlabs = new List<OldRegimeSlab>()
			{
				new OldRegimeSlab()
				{
					IncomeFrom =1200,
					IncomeTo=1250
				}
			};

			#endregion

			var src = new IncomeTaxOldRegmine(uow.Object, decsettingss, salary, dueMonth, paySheets, prevEmps, oldRegimeSlabs);

			#region Mock

			var EarningComponentMockSet = earningComponents.AsQueryable().BuildMockDbSet();
			SetData.MockEarningComponent(uow, _context, EarningComponentMockSet);

			var salaryEarningMockSet = salearnings.AsQueryable().BuildMockDbSet();
			SetData.MockSalaryEarning(uow, _context, salaryEarningMockSet);

			var paySheetMockSet = paySheets.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, paySheetMockSet);

			var declarationAllowanceMockSet = declarationAllowance.AsQueryable().BuildMockDbSet();
			SetData.MockDeclarationAllowance(uow, _context, declarationAllowanceMockSet);

			var salaryMockSet = salaryy.AsQueryable().BuildMockDbSet();
			SetData.MockSalary(uow, _context, salaryMockSet);

			var EpfMockSet = epf.AsQueryable().BuildMockDbSet();
			SetData.MockEPF(uow, _context, EpfMockSet);

			var EmpStatutoryMockSet = empStatutory.AsQueryable().BuildMockDbSet();
			SetData.MockEmpStatutory(uow, _context, EmpStatutoryMockSet);

			var SalDeductionMockSet = saldeductions.AsQueryable().BuildMockDbSet();
			SetData.MockSalaryDeduction(uow, _context, SalDeductionMockSet);
			#endregion
			//Act
			await src.Calculate(declaration);

			//Assert
			Assert.NotNull(declaration);
		}


		[Fact]
		public void IncomeTaxCalculator_Calculate_Return_NewRegim()
		{
			#region Arrange
			var id = Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578");
			var empId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c");
			int paidMonths = 3;
			bool Docalcluate = true;

			IEnumerable<Employee> employees = _employeeData.GetAllEmployeesData();

			HRADeclarationMockData();

			var salarys = new List<Salary>()
			{
				new Salary()
				{
					EmployeeId=empId,
					Monthly=12000
				}
			};

			var prevEmps = new List<PrevEmployment>()
			{
				new PrevEmployment()
				{
					DeclarationId=id,
					IncomeAfterException=1200,
					IncomeTax=5600,
					ProfessionalTax=1200
				}
			};
			PaySheetEarningMockData();

			OtherincomesourcesMockdata();

			ProfessionalTaxSlabMockData();

			OldRegimeMockData();

			NewRegimeMockData();

			var paySheetEarnings = new List<PaySheetEarning>()
			{
				new PaySheetEarning()
				{
					EarningType=1,
					Earning=123,

				},
				new PaySheetEarning()
				{
					EarningType=2,
					Earning=100,
				}
			};

			var paySheets = new List<PaySheet>()
			{
				new PaySheet()
				{
					Earnings=paySheetEarnings,
					Gross=1200,
					PayCut=500,
					LOP=1,
					UA=5,
					LC=11,

				}
			};

			DeclarationSetting decsettingss = new()
			{
				PaySettingsId = Guid.NewGuid(),
				EducationCess = 15000
			};

			Declaration declaration = new()
			{
				ID = id,
				EmployeeId = empId,
				Tax = 13000,
				Allowance = 2000,
				StandardDeduction = 110,
				EPF = 120,
				OtherSections = 150,
				IsNewRegime = true,
				Relief = 11000,
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
				ReleasedOn = DateTime.Parse("2019-02-02"),
				Amount = 2000
			} };
			#endregion

			//Mock
			_ = _context.GetRepositoryAsyncDbSet(uow, salarys);

			var employeeMockSet = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDataGenerator(uow, _context, employeeMockSet);


			var PrevEmployementMockSet = prevEmps.AsQueryable().BuildMockDbSet();
			SetData.MockPrevEmployment(uow, _context, PrevEmployementMockSet);

			var bonusMockData = bonus.AsQueryable().BuildMockDbSet();
			SetData.MockEmpBonus(uow, _context, bonusMockData);

			var incomeTax = new IncomeTaxCalculator(uow.Object, new FinancialYear { });
			var salary = new Salary()
			{
				EmployeeId = empId,
				Monthly = 12000
			};
			//Act
			var dd = incomeTax.Calculate(salary, paySheets, declaration, decsettingss, empId, paidMonths, Docalcluate);

			//Assert
			Assert.True(declaration.TaxPaid == 5600);
			Assert.True(dd.IsCompleted);
		}


		[Fact]
		public void Calculate_Return_OldRegim_Data()
		{
			#region Arrange
			var id = Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578");
			var empId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c");
			int paidMonths = 3;
			bool Docalcluate = true;

			IEnumerable<Employee> employees = _employeeData.GetAllEmployeesData();

			HRADeclarationMockData();

			var salary = new Salary()
			{
				EmployeeId = empId,
				Monthly = 12000
			};

			var prevEmps = new List<PrevEmployment>()
			{
				new PrevEmployment()
				{
					DeclarationId=id,
					IncomeAfterException=1200,
					IncomeTax=5600,
					ProfessionalTax=1200
				}
			};
			PaySheetEarningMockData();

			OtherincomesourcesMockdata();

			ProfessionalTaxSlabMockData();

			OldRegimeMockData();

			NewRegimeMockData();

			var paySheetEarnings = new List<PaySheetEarning>()
			{
				new PaySheetEarning()
				{
					EarningType=1,
					Earning=123,

				},
				new PaySheetEarning()
				{
					EarningType=2,
					Earning=100,
				}
			};

			var paySheets = new List<PaySheet>()
			{
				new PaySheet()
				{
					Earnings=paySheetEarnings,
					Gross=1200,
					PayCut=500,
					LOP=1,
					UA=5,
					LC=11,

				}
			};

			DeclarationSetting decsettingss = new()
			{
				PaySettingsId = Guid.NewGuid(),
				EducationCess = 15000
			};

			Declaration declaration = new()
			{
				ID = id,
				EmployeeId = empId,
				Tax = 13000,
				Allowance = 2000,
				StandardDeduction = 110,
				EPF = 120,
				OtherSections = 150,
				IsNewRegime = false,
				Relief = 11000,
			};

			#endregion

			//Mock 

			var employeeMockSet = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDataGenerator(uow, _context, employeeMockSet);


			var PrevEmployementMockSet = prevEmps.AsQueryable().BuildMockDbSet();
			SetData.MockPrevEmployment(uow, _context, PrevEmployementMockSet);

			var incomeTax = new IncomeTaxCalculator(uow.Object, new FinancialYear { });

			//Act
			var dd = incomeTax.Calculate(salary, paySheets, declaration, decsettingss, empId, paidMonths, Docalcluate);

			//Assert
			Assert.True(dd.IsCompleted);
		}

		//DoCaluate = false
		[Fact]
		public void Calculate_DataNotExist_Return_Null()
		{
			#region Arrange
			var id = Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578");
			var empId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c");
			int paidMonths = 3;
			bool Docalcluate = false;

			IEnumerable<Employee> employees = _employeeData.GetAllEmployeesData();

			HRADeclarationMockData();

			var salary = new Salary()
			{
				EmployeeId = empId,
				Monthly = 12000
			};

			var prevEmps = new List<PrevEmployment>()
			{
				new PrevEmployment()
				{
					DeclarationId=id,
					IncomeAfterException=1200,
					IncomeTax=5600,
					ProfessionalTax=1200
				}
			};
			PaySheetEarningMockData();

			OtherincomesourcesMockdata();

			ProfessionalTaxSlabMockData();

			OldRegimeMockData();

			NewRegimeMockData();

			var paySheetEarnings = new List<PaySheetEarning>()
			{
				new PaySheetEarning()
				{
					EarningType=1,
					Earning=123,

				},
				new PaySheetEarning()
				{
					EarningType=2,
					Earning=100,
				}
			};

			var paySheets = new List<PaySheet>()
			{
				new PaySheet()
				{
					Earnings=paySheetEarnings,
					Gross=1200,
					PayCut=500,
					LOP=1,
					UA=5,
					LC=11,
				}
			};

			DeclarationSetting decsettingss = new()
			{
				PaySettingsId = Guid.NewGuid(),
				EducationCess = 15000
			};

			Declaration declaration = new()
			{
				ID = id,
				EmployeeId = empId,
				Tax = 13000,
				Allowance = 2000,
				StandardDeduction = 110,
				EPF = 120,
				OtherSections = 150,
				IsNewRegime = false,
				Relief = 11000,
				Salary = 108683
			};

			#endregion

			//Mock

			var employeeMockSet = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDataGenerator(uow, _context, employeeMockSet);


			var PrevEmployementMockSet = prevEmps.AsQueryable().BuildMockDbSet();
			SetData.MockPrevEmployment(uow, _context, PrevEmployementMockSet);

			var incomeTax = new IncomeTaxCalculator(uow.Object, new FinancialYear { });

			//Act
			var dd = incomeTax.Calculate(salary, paySheets, declaration, decsettingss, empId, paidMonths, Docalcluate);

			//Assert
			Assert.True(dd.IsCompleted);
		}


		[Fact]
		public async Task NewRegim_Calculate()
		{
			OtherincomesourcesMockdata();

			Declaration declaration = new()
			{
				ID = Guid.Parse("2F598FBF-D2E7-4B72-8A86-0B91CDE40578"),
				Salary = 12000,
				PreviousEmployment = 150000
			};


			var newRegimeSlabs = new List<NewRegimeSlab>()
			{
				new NewRegimeSlab()
				{
					IncomeFrom =1200,
					IncomeTo=1250
				}
			};

			var incomeTax = new IncomeTaxNewRegmine(uow.Object, newRegimeSlabs);

			//Mock
			var NewRegimMockData = newRegimeSlabs.AsQueryable().BuildMockDbSet();
			SetData.MockNewRegimeSlab(uow, _context, NewRegimMockData);


			await incomeTax.Calculate(declaration);

			Assert.NotNull(declaration);
		}


	}

}
