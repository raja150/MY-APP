using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using Transmart.Services.UnitTests.Services.Payroll.PayRollData;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;
using TranSmart.Service.Payroll;
using Xunit;
using static Transmart.Services.UnitTests.Services.Payroll.Data.EsiDataGenerator;

namespace Transmart.Services.UnitTests.Services.Payroll
{
	public class DeclarationServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IDeclarationService _declarationService;
		private readonly Mock<DbContext> _context;
		private readonly EmployeeDataGenerator _employeeData;

		public DeclarationServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);

			_declarationService = new DeclarationService(uow.Object);
			_context = new Mock<DbContext>();
			_employeeData = new EmployeeDataGenerator();

		}

		[Fact]
		public async Task Get_GetWithFinancialYearId_IsSuccess()
		{
			var financialYearId = Guid.NewGuid();
			//Arrange
			var declarations = new List<Declaration>()
			{
				new Declaration
				{
					ID=Guid.NewGuid(),
					EmployeeId=Guid.NewGuid(),
					FinancialYearId=financialYearId
				},
				new Declaration
				{
					ID=Guid.NewGuid(),
					EmployeeId=Guid.NewGuid(),
					FinancialYearId=Guid.NewGuid(),
				},
			};

			//Mock
			var mockSet = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, mockSet);

			// Asset

			await _declarationService.Declaration(financialYearId);
			var result = await uow.Object.GetRepositoryAsync<Declaration>().SingleAsync(x => x.FinancialYearId == financialYearId);
			//Assert
			Assert.Equal(financialYearId, result.FinancialYearId);
		}

		[Fact]
		public async Task GetDeclaration_GetWithFinancialYrIdAndEmployeeId_GetData()
		{
			var financialYearId = Guid.NewGuid();
			var employeeId = Guid.NewGuid();
			#region Arrange

			var declarations = new List<Declaration>()
			{
				new Declaration
				{
					ID=financialYearId,
					EmployeeId=employeeId,
					FinancialYearId=financialYearId
				},
				  new Declaration
				{
					ID=Guid.NewGuid(),
					EmployeeId=employeeId,
					FinancialYearId=Guid.NewGuid()
				},
			};

			#endregion

			//Mock
			var mockSet = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, mockSet);

			// Asset

			var dd = await _declarationService.GetDeclaration(financialYearId, employeeId);

			//Assert
			Assert.Equal(financialYearId, dd.ID);
			Assert.Equal(employeeId, dd.EmployeeId);
		}

		[Fact]
		public async Task GetDeclarationSettings_PaySettingsExists_GetData()
		{
			var settingsId = Guid.NewGuid();
			#region Arrange
			var declarationSettings = new List<DeclarationSetting>()
			{
				  new DeclarationSetting
				{
					ID=settingsId,
					PaySettingsId=Guid.NewGuid(),
				},
				  new DeclarationSetting
				{
					ID=Guid.NewGuid(),
					PaySettingsId=settingsId,
				},
			};
			#endregion

			// Mock
			var declarationSettingsockSet = declarationSettings.AsQueryable().BuildMockDbSet();
			SetData.MockDeclarationSettings(uow, _context, declarationSettingsockSet);

			// Asset

			var dd = await _declarationService.GetDeclarationSettings(settingsId);

			//Assert
			Assert.Equal(settingsId, dd.PaySettingsId);
		}

		[Fact]
		public async Task GetSettings_FinancialYearNotNull_GetData()
		{
			var financialYearId = Guid.NewGuid();
			var paySettingsId = Guid.NewGuid();

			#region Arrange
			var financialYears = new List<FinancialYear>()
			{
				new FinancialYear
				{
					ID=financialYearId,
					PaySettingsId=paySettingsId,
				},
				  new FinancialYear
				{
					ID=Guid.NewGuid(),
					PaySettingsId=financialYearId,
				},
			};

			var declarationSettings = new List<DeclarationSetting>()
			{
				new DeclarationSetting
				{
					ID=Guid.NewGuid(),
					PaySettingsId=paySettingsId,
				},
				  new DeclarationSetting
				{
					ID=Guid.NewGuid(),
					PaySettingsId=paySettingsId,
				},
			};

			#endregion

			#region Mock
			var declarationSettingsMockSet = declarationSettings.AsQueryable().BuildMockDbSet();
			SetData.MockDeclarationSettings(uow, _context, declarationSettingsMockSet);

			var financialYearsMockSet = financialYears.AsQueryable().BuildMockDbSet();
			SetData.MockFinancialYear(uow, _context, financialYearsMockSet);
			#endregion

			//Asset

			var dd = await _declarationService.GetSettings(financialYearId);
			var financialYearList = await uow.Object.GetRepositoryAsync<FinancialYear>().SingleAsync(x => x.ID == financialYearId);
			//Assert
			Assert.Equal(financialYearList.PaySettingsId, dd.PaySettingsId);
		}

		[Fact]
		public async Task Get_DeclarationExists_GetData()
		{
			var declarationId = Guid.NewGuid();

			//Arrange
			var declarations = new List<Declaration>()
			{
				new Declaration
				{
					ID=declarationId,
					EmployeeId=Guid.NewGuid(),
				}
			};

			// Mock
			var declarationMockSet = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, declarationMockSet);

			//Asset

			var dd = await _declarationService.GetById(declarationId);

			//Assert
			Assert.Equal(declarationId, dd.ID);
		}

		[Fact]
		public async Task GetPaginate_GetDeclarationPageList_GetPaginateList()
		{
			var employeeId = Guid.NewGuid();
			var loginEmp = Guid.NewGuid();
			#region Arrange

			var baseSearch = new BaseSearch
			{
				SortBy = "Employee",
				Size = 10,
				Page = 0,
				RefId = loginEmp
			};

			var declarations = new List<Declaration>
			{
				new Declaration
				{
					ID=Guid.NewGuid(),
					EmployeeId=employeeId,
				},
				new Declaration
				{
					 ID=Guid.NewGuid(),
					EmployeeId=employeeId,
				}

			};
			#endregion

			#region Mock
			var mockSet = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, mockSet);
			#endregion

			// Assert

			var declaration = await _declarationService.GetPaginate(baseSearch);
			Assert.True(declaration.Count == 2);
		}

		[Theory]
		[ClassData(typeof(DeclarationDataForCustomValidation))]
		public void CustomValidation_ValidateData_IsSuccess(DeclarationData data)
		{
			#region Arrange

			var declarations = new List<Declaration>()
			{
				new Declaration
				{
					ID=Guid.NewGuid(),
					FinancialYearId=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					FinancialYear=new FinancialYear{ID=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"), FromDate=data.fromDate,ToDate=data.toDate},
					EmployeeId=Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AB8562"),
					IsNewRegime=false
				}
			};
			var financialYears = new List<FinancialYear>()
			{
				new FinancialYear
				{
					ID=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					Closed=data.closed,
					FromDate=data.fromDate,
					ToDate=data.toDate
				},
				new FinancialYear
				{
					ID=Guid.NewGuid(),
					Closed=data.closed
				}
			};
			var hraDeclarations = new List<HraDeclaration>()
			{
				new HraDeclaration
				{
					ID=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					RentalFrom=DateTime.Parse("2020-10-21"),
					RentalTo=DateTime.Parse("2020-10-21"),
				},
				new HraDeclaration
				{
					ID=Guid.Parse("4E2C495B-DD5E-4318-FC16-08DA64AB8562"),
					RentalFrom=DateTime.Parse("2020-10-21"),
					RentalTo=DateTime.Parse("2020-10-28"),
				}
			};
			var hraDeclaration = new List<HraDeclaration>()
			{
				new HraDeclaration
				{
					ID=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					RentalFrom=DateTime.Parse("2020-10-21"),
					RentalTo=DateTime.Parse("2020-10-28"),
					DeclarationId=Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562"),
					Declaration=new Declaration{ID=Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562")},
					Amount=data.hraLinesAmount
				},
				new HraDeclaration
				{
					ID=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					RentalFrom=DateTime.Parse("2020-10-21"),
					RentalTo=DateTime.Parse("2020-10-28"),
					DeclarationId=Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562"),
					Declaration=new Declaration{ID=Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562")},
					Amount=data.hraLinesAmount
				}
			};
			var section80CLines = new List<Section6A80C>
			{
				new Section6A80C
				{
					ID=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					Section80CId=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					Amount=data.section80CAmount
				},
				new Section6A80C
				{
					ID=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					Section80CId=data.section80CId,
					Amount=data.section80CAmount
				}
			};
			var section80DLines = new List<Section6A80D>
			{
				new Section6A80D
				{
					ID=Guid.NewGuid(),
					Section80DId=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					Amount=data.section80DAmount
				},
				new Section6A80D
				{
					ID=Guid.NewGuid(),
					Section80DId=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					Amount=data.section80DAmount
				}
			};
			var sectionOtherLines = new List<Section6AOther>
			{
				new Section6AOther
				{
					ID=Guid.NewGuid(),
					OtherSectionsId=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					Amount=data.otherSectionAmount
				},
				new Section6AOther
				{
					ID=Guid.NewGuid(),
					OtherSectionsId=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					Amount=data.otherSectionAmount
				}
			};
			var declaration = new Declaration
			{
				ID = Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562"),
				IsNewRegime = true,
				HRALines = hraDeclaration,
				Section80CLines = section80CLines,
				Section80DLines = section80DLines,
				SectionOtherLines = sectionOtherLines,
				EmployeeId = data.employeeId,
				FinancialYearId = data.financialId,
				FinancialYear = new FinancialYear
				{
					ID = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					FromDate = data.fromDate,
					ToDate = data.toDate,
					Closed = data.closed
				},
			};
			var emptyDeclaration = new Declaration();

			#endregion

			#region Mock

			var declarationsMockSet = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, declarationsMockSet);

			var financialYearsMockSet = financialYears.AsQueryable().BuildMockDbSet();
			SetData.MockFinancialYear(uow, _context, financialYearsMockSet);

			var hraDeclarationsMockSet = hraDeclarations.AsQueryable().BuildMockDbSet();
			SetData.MockHRADeclaration(uow, _context, hraDeclarationsMockSet);

			var section80CLinesMockSet = section80CLines.AsQueryable().BuildMockDbSet();
			SetData.MockSection6A80C(uow, _context, section80CLinesMockSet);

			var section80DLinesMockSet = section80DLines.AsQueryable().BuildMockDbSet();
			SetData.MockSection6A80D(uow, _context, section80DLinesMockSet);

			var sectionOtherLinesMockSet = sectionOtherLines.AsQueryable().BuildMockDbSet();
			SetData.MockSection6AOther(uow, _context, sectionOtherLinesMockSet);

			#endregion

			// Asset

			var executionResult = new Result<Declaration>();
			_ = _declarationService.CustomValidation(data.isDeclaration ? emptyDeclaration : declaration, executionResult);

			//Act
			Assert.Equal(data.result, executionResult.HasNoError);
		}

		[Fact]
		public async Task AddAsync_IsSuccess()
		{
			#region Arrange
			//var employees = _employeeData.GetAllEmployeesData();
			var employeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c");
			var section80DId = Guid.NewGuid();
			var otherSectionId = Guid.NewGuid();
			var employeeList = new List<Employee>() { new Employee { ID = employeeId, WorkLocation = new Location { StateId = Guid.NewGuid() } } };
			var declarations = new List<Declaration>()
			{
				new Declaration
				{
					ID=Guid.NewGuid(),
					FinancialYearId=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					FinancialYear=new FinancialYear{ID=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"), FromDate=DateTime.Parse("2018-12-12"),ToDate=DateTime.Parse("2019-12-12")},
					EmployeeId=Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AB8562"),
					IsNewRegime=false
				}
			};
			var financialYears = new List<FinancialYear>()
			{
				new FinancialYear
				{
					ID=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					Closed=false,
					FromDate=DateTime.Parse("2018-12-12"),
					ToDate=DateTime.Parse("2019-12-12")
				},
				new FinancialYear
				{
					ID=Guid.NewGuid(),
					Closed=false
				}
			};
			var hraDeclarations = new List<HraDeclaration>()
			{
				new HraDeclaration
				{
					ID=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					RentalFrom=DateTime.Parse("2020-10-21"),
					RentalTo=DateTime.Parse("2020-10-21"),
				},
				new HraDeclaration
				{
					ID=Guid.Parse("4E2C495B-DD5E-4318-FC16-08DA64AB8562"),
					RentalFrom=DateTime.Parse("2020-10-21"),
					RentalTo=DateTime.Parse("2020-10-28"),
				}
			};
			var hraDeclaration = new List<HraDeclaration>()
			{
				new HraDeclaration
				{
					ID=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					RentalFrom=DateTime.Now.AddYears(-3),
					RentalTo=DateTime.Now.AddYears(-2),
					DeclarationId=Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562"),
					Declaration=new Declaration{ID=Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562")},
					Amount=120
				},
				new HraDeclaration
				{
					ID=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					RentalFrom=DateTime.Parse("2021-10-21"),
					RentalTo=DateTime.Parse("2022-10-28"),
					DeclarationId=Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562"),
					Declaration=new Declaration{ID=Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562")},
					Amount=120
				}
			};
			var section80CLines = new List<Section6A80C>
			{
				new Section6A80C
				{
					ID=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					Section80CId=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					Amount=4000
				},
				new Section6A80C
				{
					ID=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					Section80CId=Guid.Parse("5E2C455B-DD5E-4318-FC26-08DA64AB8562"),
					Amount=4000
				}
			};
			var section80D = new List<Section80D> { new Section80D
			{
				ID = section80DId,
				Name = "Section 80D",
				Limit = 15000,
				Status = true,
			},
			new Section80D{
				ID = section80DId,
				Name = "80D Section",
				Limit = 10000,
				Status = true
		} };
			var section80DLines = new List<Section6A80D>
			{
				new Section6A80D
				{
					ID=Guid.NewGuid(),
					Section80DId=section80DId,
					Amount=1000,
				},
				new Section6A80D
				{
					ID=Guid.NewGuid(),
					Section80DId=section80DId,
					Amount=0
				}
			};
			var otherSections = new List<OtherSections> { new OtherSections
			{
				ID = otherSectionId,
				Limit = 20000,
				Name = "OtherSection",
				Status = true,
			} };
			var sectionOtherLines = new List<Section6AOther>
			{
				new Section6AOther
				{
					ID=Guid.NewGuid(),
					OtherSectionsId=otherSectionId,
					Amount=0
				},
				new Section6AOther
				{
					ID=Guid.NewGuid(),
					OtherSectionsId=otherSectionId,
					Amount=0
				}
			};
			var homeLoanPay = new HomeLoanPay
			{
				ID = Guid.NewGuid(),
				InterestPaid = 6000
			};
			var declaration = new Declaration
			{
				ID = Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562"),
				IsNewRegime = true,
				HomeLoanPay = homeLoanPay,
				HRALines = hraDeclaration,
				Section80CLines = section80CLines,
				Section80DLines = section80DLines,
				SectionOtherLines = sectionOtherLines,
				//EmployeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DB64AB8562"),
				EmployeeId = employeeId,
				FinancialYearId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
				FinancialYear = new FinancialYear { ID = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"), FromDate = DateTime.Parse("2018-12-12"), ToDate = DateTime.Parse("2019-12-12"), Closed = false },
				Salary = 1200
			};
			var declarationSettings = new List<DeclarationSetting>()
			{
				new DeclarationSetting
				{
					ID=Guid.NewGuid(),
					MaxLimitEightyC = 30000,
					MaxLimitEightyD = 40000,
					HouseLoanInt = 6000
				}
			};
			var paySheets = new List<PaySheet>() {
				new PaySheet
				{
					ID=Guid.NewGuid(),
					EmployeeID=employeeId,
					PayMonth=new PayMonth{FinancialYearId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562") },
					Deduction = 2000,
					Deductions = new List<PaySheetDeduction>{new PaySheetDeduction { DeductType = (int)DeductionType.Pre} },
				}
			};
			var payMonths = new List<PayMonth>() {
				new PayMonth
				{
					ID=Guid.NewGuid(),
					Status=(int)PayMonthStatus.Released,
					FinancialYearId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
				}
			};
			var salaries = new List<Salary>
			{
				new Salary
				{
					ID=Guid.NewGuid(),
					Monthly=120,
					EmployeeId=employeeId,
				}

			};
			var prevEmployees = new List<PrevEmployment>
			{
				new PrevEmployment
				{
					ID=Guid.NewGuid()
				}
			};
			var otherIncomeSources = new List<OtherIncomeSources> {
				new OtherIncomeSources
				{
					ID=Guid.NewGuid(),
					DeclarationId=Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562")
				}
			};
			var professionalTaxSlab = new List<ProfessionalTaxSlab>();
			var newRegimeSlabs = new List<NewRegimeSlab>
			{
				new NewRegimeSlab
				{
					ID=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
					IncomeFrom=120,
					IncomeTo=1520,
					TaxRate=10
				}
			};
			var bonus = new List<EmpBonus>() { new EmpBonus
			{
				ID = Guid.NewGuid(),
				EmployeeId = employeeId,
				ReleasedOn = DateTime.Parse("2020-02-04"),
				Amount = 1000
			} ,
			 new EmpBonus
			 {
				ID = Guid.NewGuid(),
				EmployeeId = employeeId,
				ReleasedOn = DateTime.Parse("2020-02-02"),
				Amount = 2000
			} };
			#endregion

			#region Mock
			_ = _context.GetRepositoryAsyncDbSet(uow, declarations);

			var financialYearsMockSet = financialYears.AsQueryable().BuildMockDbSet();
			SetData.MockFinancialYear(uow, _context, financialYearsMockSet);

			var hraDeclarationsMockSet = hraDeclarations.AsQueryable().BuildMockDbSet();
			SetData.MockHRADeclaration(uow, _context, hraDeclarationsMockSet);

			var section80CLinesMockSet = section80CLines.AsQueryable().BuildMockDbSet();
			SetData.MockSection6A80C(uow, _context, section80CLinesMockSet);

			var section80DMockSet = section80D.AsQueryable().BuildMockDbSet();
			SetData.MockSection80D(uow, _context, section80DMockSet);

			var section80DLinesMockSet = section80DLines.AsQueryable().BuildMockDbSet();
			SetData.MockSection6A80D(uow, _context, section80DLinesMockSet);

			var otherSectionMockSet = otherSections.AsQueryable().BuildMockDbSet();
			SetData.MockOtherSections(uow, _context, otherSectionMockSet);

			var sectionOtherLinesMockSet = sectionOtherLines.AsQueryable().BuildMockDbSet();
			SetData.MockSection6AOther(uow, _context, sectionOtherLinesMockSet);

			var declarationSettingsMockSet = declarationSettings.AsQueryable().BuildMockDbSet();
			SetData.MockDeclarationSettings(uow, _context, declarationSettingsMockSet);

			var paySheetsMockSet = paySheets.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, paySheetsMockSet);

			var payMonthsMockSet = payMonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, payMonthsMockSet);

			var salariesMockSet = salaries.AsQueryable().BuildMockDbSet();
			SetData.MockSalary(uow, _context, salariesMockSet);

			//var employeesMockSet = employees.AsQueryable().BuildMockDbSet();
			//SetData.MockEmployeeDataGenerator(uow, _context, employeesMockSet);

			var _repository = _context.GetRepositoryAsyncDbSet(uow, employeeList);
			_repository.Setup(x => x.SingleAsync(It.IsAny<Expression<Func<Employee, bool>>>(),
			 It.IsAny<Func<IQueryable<Employee>, IOrderedQueryable<Employee>>>(),
			 It.IsAny<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>(), true)).ReturnsAsync(
				 employeeList.FirstOrDefault(x => x.ID == employeeId)
			  );

			var prevEmployeesMockSet = prevEmployees.AsQueryable().BuildMockDbSet();
			SetData.MockPrevEmployment(uow, _context, prevEmployeesMockSet);

			var otherIncomeSourcesMockSet = otherIncomeSources.AsQueryable().BuildMockDbSet();
			SetData.MockOtherIncomeSources(uow, _context, otherIncomeSourcesMockSet);

			var professionalTaxSlabMockSet = professionalTaxSlab.AsQueryable().BuildMockDbSet();
			SetData.MockProfessionalTaxSlab(uow, _context, professionalTaxSlabMockSet);

			var mockSet = newRegimeSlabs.AsQueryable().BuildMockDbSet();
			SetData.MockNewRegimeSlab(uow, _context, mockSet);

			var bonusMockData = bonus.AsQueryable().BuildMockDbSet();
			SetData.MockEmpBonus(uow, _context, bonusMockData);

			#endregion

			// Act
			var dd = await _declarationService.AddAsync(declaration);
			var declarationList = await uow.Object.GetRepositoryAsync<Declaration>().GetAsync();

			//Assert
			Assert.True(dd.HasNoError);
			Assert.Equal(2, declarationList.Count());//Added
			Assert.True(declarationList.First(x => x.ID == declaration.ID).IsNewRegime);//Update
			Assert.Equal(1560, dd.ReturnValue.HRALines.FirstOrDefault().Total);
			uow.Verify(x => x.SaveChangesAsync(), Times.Exactly(2));
		}

		[Theory]
		[ClassData(typeof(DeclarationDataGeneratorForUpdate))]
		public async Task UpdateAsync_ValidData_DataSaved(DeclarationData data)
		{
			#region Arrange
			var employees = _employeeData.GetAllEmployeesData();
			var declarations = new List<Declaration>()
			{
				new Declaration
				{
					ID=Guid.Parse("3E2C595B-DD5E-4318-FC26-08DB64AB8562"),
					FinancialYearId=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					FinancialYear=new FinancialYear{ID=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"), FromDate=data.fromDate,ToDate=data.toDate},
					EmployeeId=Guid.Parse("5E2C595B-DD5E-4318-FC26-08DB64AB8562"),
					IsNewRegime=false
				}
			};
			var financialYears = new List<FinancialYear>()
			{
				new FinancialYear
				{
					ID=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					Closed=data.closed,
					FromDate=data.fromDate,
					ToDate=data.toDate
				},
				new FinancialYear
				{
					ID=Guid.NewGuid(),
					Closed=data.closed
				}
			};
			var hraDeclarations = new List<HraDeclaration>()
			{
				new HraDeclaration
				{
					ID=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					RentalFrom=DateTime.Parse("2020-10-21"),
					RentalTo=DateTime.Parse("2020-10-21"),
					DeclarationId = Guid.Parse("3E2C595B-DD5E-4318-FC26-08DB64AB8562"),
				},
				new HraDeclaration
				{
					ID=Guid.Parse("4E2C495B-DD5E-4318-FC16-08DA64AB8562"),
					RentalFrom=DateTime.Parse("2020-10-21"),
					RentalTo=DateTime.Parse("2020-10-28"),
					DeclarationId = Guid.Parse("3E2C595B-DD5E-4318-FC26-08DB64AB8562"),
				}
			};
			var hraDeclaration = new List<HraDeclaration>()
			{
				new HraDeclaration
				{
					ID=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					RentalFrom=DateTime.Parse("2020-10-21"),
					RentalTo=DateTime.Parse("2020-10-28"),
					DeclarationId=Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562"),
					Declaration=new Declaration{ID=Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562")},
					Amount=data.hraLinesAmount
				},
				new HraDeclaration
				{
					ID=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					RentalFrom=DateTime.Parse("2020-10-21"),
					RentalTo=DateTime.Parse("2020-10-28"),
					DeclarationId=Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562"),
					Declaration=new Declaration{ID=Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562")},
					Amount=data.hraLinesAmount
				}
			};
			var section80CLines = new List<Section6A80C>
			{
				new Section6A80C
				{
					ID=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					Section80CId=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					Amount=data.section80CAmount
				},
				new Section6A80C
				{
					ID=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					Section80CId=data.section80CId,
					Amount=data.section80CAmount
				}
			};
			var section80DLines = new List<Section6A80D>
			{
				new Section6A80D
				{
					ID=Guid.NewGuid(),
					Section80DId=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					Amount=data.section80DAmount
				},
				new Section6A80D
				{
					ID=Guid.NewGuid(),
					Section80DId=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					Amount=data.section80DAmount
				}
			};
			var sectionOtherLines = new List<Section6AOther>
			{
				new Section6AOther
				{
					ID=Guid.NewGuid(),
					OtherSectionsId=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					Amount=data.otherSectionAmount
				},
				new Section6AOther
				{
					ID=Guid.NewGuid(),
					OtherSectionsId=Guid.Parse("5E2C495B-DD5E-4318-FC26-08DA64AB8562"),
					Amount=data.otherSectionAmount
				}
			};
			var declaration = new Declaration
			{
				ID = data.declarationId,
				IsNewRegime = true,
				HRALines = hraDeclaration,
				Section80CLines = section80CLines,
				Section80DLines = section80DLines,
				SectionOtherLines = sectionOtherLines,
				EmployeeId = data.employeeId,
				FinancialYearId = data.financialId,
				FinancialYear = new FinancialYear
				{
					ID = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					FromDate = data.fromDate,
					ToDate = data.toDate,
					Closed = data.closed
				},
				Salary = 1200,
				Allowance = 120
			};
			var emptyDeclaration = new Declaration
			{
				//EmployeeId=Guid.NewGuid()
			};
			var declarationSettings = new List<DeclarationSetting>()
			{
				new DeclarationSetting
				{
					ID=Guid.NewGuid(),
					MaxLimitEightyC=data.maxLimitEightyC,
					MaxLimitEightyD=data.maxLimitEightyD
				}
			};
			var paySheets = new List<PaySheet>() {
				new PaySheet
				{
					ID=Guid.NewGuid(),
					EmployeeID = data.employeeId,
					PayMonth = new PayMonth
					{
						FinancialYearId = data.financialId,
					},
					Earnings = new List<PaySheetEarning>{new PaySheetEarning { Earning = 1000 , EarningType = 1} },
					PTax = 4000,
					Gross = 40000,
					PayCut = 2000,
					LOP =5000,
					UA=1000,
					LC = 2000,
					Deductions = new List<PaySheetDeduction>{new PaySheetDeduction { DeductType = 1 , Deduction = 500} }
				}
			};
			var payMonths = new List<PayMonth>() {
				new PayMonth
				{
					ID=Guid.NewGuid()
				}
			};
			var earningComponent = new EarningComponent
			{
				ID = Guid.NewGuid(),
				EarningType = 9
			};
			var salaryEarnings = new List<SalaryEarning>
			{
				new SalaryEarning
				{ ID = Guid.NewGuid(),
					Component =earningComponent
				}
			};
			var salaryDeductions = new List<SalaryDeduction>
			{
				new SalaryDeduction
				{
					ID=Guid.NewGuid(),
					Deduction=new DeductionComponent{Deduct=1}
				}
			};
			var salaries = new List<Salary>
			{
				new Salary
				{
					ID=Guid.NewGuid(),
					Monthly=120,
					EmployeeId=Guid.Parse("5E2C595B-DD5E-4318-FC26-08DB64AB8562"),
					Earnings=salaryEarnings,
					Deductions=salaryDeductions
				}

			};
			var prevEmp = new PrevEmployment
			{
				ID = Guid.NewGuid(),
				ProvisionalFund = 6000,
			};
			var prevEmployees = new List<PrevEmployment>
			{
				new PrevEmployment
				{
					ID=Guid.NewGuid(),
					ProvisionalFund = 6000,
				}
			};
			var otherIncomeSources = new List<OtherIncomeSources> {
				new OtherIncomeSources
				{
					ID=Guid.NewGuid(),
					DeclarationId=Guid.Parse("4E2C195B-DD5E-4318-FC26-08DA64AB8562")
				}
			};
			var homeLoanPays = new List<HomeLoanPay> {
				new HomeLoanPay
				{
					ID=Guid.NewGuid(),
					DeclarationId=data.declarationId,
					Principle = 100000,
					InterestPaid = 5000
				}
			};
			var letOutProperties = new List<LetOutProperty> {
				new LetOutProperty
				{
					ID=Guid.NewGuid(),
					DeclarationId=data.declarationId,
					AnnualRentReceived = 200000,
					InterestPaid = 10000,
					MunicipalTaxPaid = 4000,
					Principle = 250000
				}
			};
			var otherSections = new List<OtherSections> {
				new OtherSections
				{
					ID=Guid.NewGuid(),
				}
			};
			var declarationAllowances = new List<DeclarationAllowance>
			{
				new DeclarationAllowance
				{
					ID=Guid.NewGuid()
				}
			};
			var professionalTaxSlab = new List<ProfessionalTaxSlab>
			{
				new ProfessionalTaxSlab
				{
					ID=Guid.NewGuid(),
					ProfessionalTax=new ProfessionalTax{ID=Guid.NewGuid(), StateId=Guid.NewGuid()}
				}
			};
			var section6A80Wages = new List<Section6A80CWages>
			{
				new Section6A80CWages
				{
					ID=Guid.NewGuid(),
					DeclarationId=data.declarationId
				}
			};
			var oldRegimeSlabs = new List<OldRegimeSlab>
			{
				new OldRegimeSlab
				{
					ID=Guid.NewGuid(),
					IncomeFrom=100,
					IncomeTo=120
				},
				new OldRegimeSlab
				{
					ID=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
					IncomeFrom=120,
					IncomeTo=120
				}
			};
			var empStatutoryList = new List<EmpStatutory>()
			{
				new EmpStatutory
				{
					ID=Guid.Parse("6D73C8BB-4B04-4C34-5157-08DA64AB8559"),
					EmpId=Guid.Parse("104A8B11-CED2-4A09-5238-08DA64AB8534"),
					UAN="ABCDEFG",
					ESINo="1234"
				},
				new EmpStatutory
				{
					ID=Guid.Parse("104A8B11-CED2-4A09-5158-08DA64AB8559"),
					EmpId=data.employeeId,
					UAN="ABCDEFG",
					ESINo="1254"
				}
			};
			var bonus = new List<EmpBonus>() { new EmpBonus
			{
				ID = Guid.NewGuid(),
				EmployeeId = data.employeeId,
				ReleasedOn = DateTime.Parse("2020-02-04"),
				Amount = 1000
			} ,
			 new EmpBonus
			 {
				ID = Guid.NewGuid(),
				EmployeeId = data.employeeId,
				ReleasedOn = DateTime.Parse("2019-02-02"),
				Amount = 2000
			} };
			#endregion

			#region Mock
			var declarationsMockSet = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, declarationsMockSet);

			var financialYearsMockSet = financialYears.AsQueryable().BuildMockDbSet();
			SetData.MockFinancialYear(uow, _context, financialYearsMockSet);

			var hraDeclarationsMockSet = hraDeclarations.AsQueryable().BuildMockDbSet();
			SetData.MockHRADeclaration(uow, _context, hraDeclarationsMockSet);

			var section80CLinesMockSet = section80CLines.AsQueryable().BuildMockDbSet();
			SetData.MockSection6A80C(uow, _context, section80CLinesMockSet);

			var section80DLinesMockSet = section80DLines.AsQueryable().BuildMockDbSet();
			SetData.MockSection6A80D(uow, _context, section80DLinesMockSet);

			var sectionOtherLinesMockSet = sectionOtherLines.AsQueryable().BuildMockDbSet();
			SetData.MockSection6AOther(uow, _context, sectionOtherLinesMockSet);

			var declarationSettingsMockSet = declarationSettings.AsQueryable().BuildMockDbSet();
			SetData.MockDeclarationSettings(uow, _context, declarationSettingsMockSet);

			var paySheetsMockSet = paySheets.AsQueryable().BuildMockDbSet();
			SetData.MockPaySheet(uow, _context, paySheetsMockSet);

			var payMonthsMockSet = payMonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, payMonthsMockSet);

			var salariesMockSet = salaries.AsQueryable().BuildMockDbSet();
			SetData.MockSalary(uow, _context, salariesMockSet);

			var employeesMockSet = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDataGenerator(uow, _context, employeesMockSet);

			var prevEmployeesMockSet = prevEmployees.AsQueryable().BuildMockDbSet();
			SetData.MockPrevEmployment(uow, _context, prevEmployeesMockSet);

			var otherIncomeSourcesMockSet = otherIncomeSources.AsQueryable().BuildMockDbSet();
			SetData.MockOtherIncomeSources(uow, _context, otherIncomeSourcesMockSet);

			var homeLoanPaysMockSet = homeLoanPays.AsQueryable().BuildMockDbSet();
			SetData.MockHomeLoanPay(uow, _context, homeLoanPaysMockSet);

			var letOutPropertiesMockSet = letOutProperties.AsQueryable().BuildMockDbSet();
			SetData.MockLetOutProperty(uow, _context, letOutPropertiesMockSet);

			var otherSectionsMockSet = otherSections.AsQueryable().BuildMockDbSet();
			SetData.MockOtherSections(uow, _context, otherSectionsMockSet);

			var declarationAllowancesMockSet = declarationAllowances.AsQueryable().BuildMockDbSet();
			SetData.MockDeclarationAllowance(uow, _context, declarationAllowancesMockSet);

			var professionalTaxSlabMockSet = professionalTaxSlab.AsQueryable().BuildMockDbSet();
			SetData.MockProfessionalTaxSlab(uow, _context, professionalTaxSlabMockSet);

			var salaryDeductionsMockSet = salaryDeductions.AsQueryable().BuildMockDbSet();
			SetData.MockSalaryDeduction(uow, _context, salaryDeductionsMockSet);

			var section6A80WagesMockSet = section6A80Wages.AsQueryable().BuildMockDbSet();
			SetData.MockSection6A80CWages(uow, _context, section6A80WagesMockSet);

			var mockSet = oldRegimeSlabs.AsQueryable().BuildMockDbSet();
			SetData.MockOldRegimeSlab(uow, _context, mockSet);

			var epfSet = new List<EPF>().AsQueryable().BuildMockDbSet();
			SetData.MockEPF(uow, _context, epfSet);


			var earningComponents = new List<EarningComponent>();
			earningComponents.Add(earningComponent);
			var earningComponentSet = earningComponents.AsQueryable().BuildMockDbSet();
			SetData.MockEarningComponent(uow, _context, earningComponentSet);

			var mockempStatutorySet = empStatutoryList.AsQueryable().BuildMockDbSet();
			SetData.MockEmpStatutory(uow, _context, mockempStatutorySet);

			var bonusMockData = bonus.AsQueryable().BuildMockDbSet();
			SetData.MockEmpBonus(uow, _context, bonusMockData);

			if (data.isCatch) { uow.Setup(x => x.SaveChangesAsync()).Throws(new InvalidOperationException()); }

			#endregion

			// Act
			var dd = await _declarationService.UpdateAsync(data.isDeclaration ? emptyDeclaration : declaration);

			//Assert
			Assert.Equal(data.result, dd.HasNoError);
		}

		[Fact]
		public async Task UpdateAsync_ValidateDefaultSettingsMissing_ThrowError()
		{
			var declarationSettings = new List<DeclarationSetting>()
			{
				new DeclarationSetting
				{
					ID = Guid.NewGuid(),
					MaxLimitEightyC = 10000,
					MaxLimitEightyD = 15000,
					HouseLoanInt = 6000
				}
			};
			var declarations = new List<Declaration>()
			{
				new Declaration
				{
					EmployeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DB64AB8562"),
					FinancialYearId = Guid.Parse("4E2C595B-DD5E-4318-FC26-08DB64AB8562")
				}
			};
			var financialYears = new List<FinancialYear>();

			var declarationSettingsMockSet = declarationSettings.AsQueryable().BuildMockDbSet();
			SetData.MockDeclarationSettings(uow, _context, declarationSettingsMockSet);

			var declarationsMockSet = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, declarationsMockSet);

			var financialYearsMockSet = financialYears.AsQueryable().BuildMockDbSet();
			SetData.MockFinancialYear(uow, _context, financialYearsMockSet);


			var result = await _declarationService.UpdateAsync(
				new Declaration()
				{
					EmployeeId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DB64AB8562"),
					FinancialYearId = Guid.Parse("4E2C595B-DD5E-4318-FC26-08DB64AB8562")
				});
			Assert.True(result.HasError);
		}

		[Fact]
		public async Task HomeLoanPay_DeleteHomeLoanPay_DeleteData()
		{
			var declarationId = Guid.NewGuid();
			var homeLoanPayId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AB8562");
			#region Arrange
			var homeLoanPay = new List<HomeLoanPay>()
			{
				new HomeLoanPay
				{
					ID=homeLoanPayId,
					DeclarationId=declarationId
				}
			};
			var declaration = new Declaration
			{
				ID = declarationId,
				HomeLoanPay = null
			};

			#endregion

			#region Mock
			var _repository = _context.GetRepositoryAsyncDbSet(uow, homeLoanPay);
			_repository.Setup(x => x.SingleAsync(It.IsAny<Expression<Func<HomeLoanPay, bool>>>(),
			 It.IsAny<Func<IQueryable<HomeLoanPay>, IOrderedQueryable<HomeLoanPay>>>(),
			 It.IsAny<Func<IQueryable<HomeLoanPay>, IIncludableQueryable<HomeLoanPay, object>>>(), true)).ReturnsAsync(
				 homeLoanPay.FirstOrDefault(x => x.ID == homeLoanPayId)
			  );

			#endregion

			// Act
			var newDeclaration = new Declaration { ID = declarationId };
			await _declarationService.HomeLoanPay(declaration, newDeclaration);
			var homeLoanPayList = await uow.Object.GetRepositoryAsync<HomeLoanPay>().GetAsync();
			//Assert
			Assert.Equal(0, homeLoanPayList.Count(x => x.ID == homeLoanPayId));
		}

		[Fact]
		public async Task HomeLoanPay_AddHomeLoanPay_AddData()
		{
			var declarationId = Guid.NewGuid();
			#region Arrange
			var homeLoanPay = new List<HomeLoanPay>()
			{
				new HomeLoanPay
				{
					ID=Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AB8562"),
					DeclarationId=declarationId
				}
			};
			var declarations = new List<Declaration>() { new Declaration { ID = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562") } };
			var declaration = new Declaration
			{
				ID = declarationId,
				HomeLoanPay = new HomeLoanPay { ID = Guid.NewGuid() }
			};

			#endregion

			//Mock
			_ = _context.GetRepositoryAsyncDbSet(uow, homeLoanPay);
			var declarationsMockSet = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, declarationsMockSet);

			// Act
			var newDeclaration = new Declaration { ID = Guid.NewGuid() };
			_ = _declarationService.HomeLoanPay(declaration, newDeclaration);
			var homeLoanPayList = await uow.Object.GetRepositoryAsync<HomeLoanPay>().GetAsync();

			//Assert
			Assert.True(homeLoanPayList.Count() == 2);
		}

		[Fact]
		public async Task HomeLoanPay_UpdateHomeLoanPay_UpdateData()
		{
			var pan = "AAAAAAA";
			var declarationId = Guid.NewGuid();
			#region Arrange
			var homeLoanPay = new List<HomeLoanPay>()
			{
				new HomeLoanPay
				{
					ID=Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AB8562"),
					DeclarationId=declarationId,
					NameOfLender="Mahesh",
					LenderPAN="ASCDHGFD",
					InterestPaid=100
				}
			};
			var declarations = new List<Declaration>() { new Declaration { ID = Guid.NewGuid() } };
			var declaration = new Declaration
			{
				ID = declarationId,
				HomeLoanPay = new HomeLoanPay
				{
					ID = Guid.NewGuid(),
					NameOfLender = "Vishnu",
					LenderPAN = pan,
					InterestPaid = 1000
				}
			};

			#endregion

			#region Mock
			var mockHomeLoanPay = homeLoanPay.AsQueryable().BuildMockDbSet();
			SetData.MockHomeLoanPay(uow, _context, mockHomeLoanPay);

			var declarationsMockSet = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, declarationsMockSet);

			#endregion

			// Act
			var newDeclaration = new Declaration { ID = declarationId };
			await _declarationService.HomeLoanPay(declaration, newDeclaration);
			var homeLoanPayList = await uow.Object.GetRepositoryAsync<HomeLoanPay>().GetAsync();
			var principle = homeLoanPayList.FirstOrDefault(x => x.DeclarationId == declarationId);
			//Assert
			Assert.Equal(1000, principle.InterestPaid);
			Assert.Equal(pan, principle.LenderPAN);
			Assert.Equal("Vishnu", principle.NameOfLender);
		}

		[Fact]
		public async Task OtherIncome_DeleteOtherIncome_DeleteData()
		{
			var declarationId = Guid.NewGuid();
			var otherIncomesourceId = Guid.NewGuid();
			#region Arrange
			var otherIncomeSources = new List<OtherIncomeSources>()
			{
				new OtherIncomeSources
				{
					ID=otherIncomesourceId,
					DeclarationId=declarationId
				}
			};

			var declaration = new Declaration
			{
				ID = declarationId,
			};

			#endregion

			#region Mock
			var _repository = _context.GetRepositoryAsyncDbSet(uow, otherIncomeSources);
			_repository.Setup(x => x.SingleAsync(It.IsAny<Expression<Func<OtherIncomeSources, bool>>>(),
			 It.IsAny<Func<IQueryable<OtherIncomeSources>, IOrderedQueryable<OtherIncomeSources>>>(),
			 It.IsAny<Func<IQueryable<OtherIncomeSources>, IIncludableQueryable<OtherIncomeSources, object>>>(), true)).ReturnsAsync(
				 otherIncomeSources.FirstOrDefault(x => x.ID == otherIncomesourceId)
			  );
			#endregion

			// Act
			var newDeclaration = new Declaration { ID = declarationId };
			await _declarationService.OtherIncome(declaration, newDeclaration);

			//Assert
			var otherIncomeSourceList = await uow.Object.GetRepositoryAsync<OtherIncomeSources>().GetAsync();
			Assert.Equal(0, otherIncomeSourceList.Count(x => x.ID == otherIncomesourceId));
		}

		[Fact]
		public async Task OtherIncome_AddOtherIncome_AddData()
		{
			var declarationId = Guid.NewGuid();
			#region Arrange
			var otherIncomeSources = new List<OtherIncomeSources>()
			{
				new OtherIncomeSources
				{
					ID=Guid.NewGuid(),
					DeclarationId=declarationId
				}
			};
			var declaration = new Declaration
			{
				ID = declarationId,
				IncomeSource = new OtherIncomeSources { ID = Guid.NewGuid() }
			};

			#endregion

			_ = _context.GetRepositoryAsyncDbSet(uow, otherIncomeSources);


			// Act

			var newDeclaration = new Declaration { ID = Guid.NewGuid() };
			await _declarationService.OtherIncome(declaration, newDeclaration);
			var otherIncomeSourcesList = await uow.Object.GetRepositoryAsync<OtherIncomeSources>().GetAsync();
			//Assert 
			Assert.True(otherIncomeSourcesList.Count() == 2);
		}

		[Fact]
		public async Task OtherIncome_UpdateOtherIncome_UpdateData()
		{
			var declarationId = Guid.NewGuid();
			#region Arrange
			var otherIncomeSources = new List<OtherIncomeSources>()
			{
				new OtherIncomeSources
				{
					ID=Guid.NewGuid(),
					DeclarationId=declarationId,
					OtherSources=100,
					InterestOnFD=100,
					InterestOnSaving=100
				}
			};
			var declarations = new List<Declaration>() { new Declaration { ID = declarationId } };
			var declaration = new Declaration
			{
				ID = declarationId,
				IncomeSource = new OtherIncomeSources { ID = Guid.NewGuid(), InterestOnSaving = 1000, InterestOnFD = 1000, OtherSources = 1000 }
			};

			#endregion

			#region Mock
			var otherIncomeSourcesMockSet = otherIncomeSources.AsQueryable().BuildMockDbSet();
			SetData.MockOtherIncomeSources(uow, _context, otherIncomeSourcesMockSet);

			var declarationsMockSet = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, declarationsMockSet);

			#endregion

			// Act

			var newDeclaration = new Declaration { ID = declarationId };
			await _declarationService.OtherIncome(declaration, newDeclaration);
			var otherIncomeSourcesList = await uow.Object.GetRepositoryAsync<OtherIncomeSources>().GetAsync();
			var otherIncomeSource = otherIncomeSourcesList.FirstOrDefault(x => x.DeclarationId == declarationId);
			//Assert
			Assert.Equal(1000, otherIncomeSource.InterestOnFD);
			Assert.Equal(1000, otherIncomeSource.OtherSources);
			Assert.Equal(1000, otherIncomeSource.InterestOnSaving);
		}

		[Fact]
		public async Task PreviousEmployment_DeletePreviousEmployement_DeleteData()
		{
			var declarationId = Guid.NewGuid();
			var prevEmploymentId = Guid.NewGuid();
			// Arrange
			var prevEmployments = new List<PrevEmployment>()
			{
				new PrevEmployment
				{
					ID = prevEmploymentId,
					DeclarationId = Guid.Parse("6c23a138-7a44-4ce2-91c7-d01f57046661")
				}
			};
			var declaration = new Declaration
			{
				ID = declarationId,
				PrevEmployment = null
			};

			//Mock
			var _repository = _context.GetRepositoryAsyncDbSet(uow, prevEmployments);
			_repository.Setup(x => x.SingleAsync(It.IsAny<Expression<Func<PrevEmployment, bool>>>(),
			 It.IsAny<Func<IQueryable<PrevEmployment>, IOrderedQueryable<PrevEmployment>>>(),
			 It.IsAny<Func<IQueryable<PrevEmployment>, IIncludableQueryable<PrevEmployment, object>>>(), true)).ReturnsAsync(
				 prevEmployments.FirstOrDefault(x => x.ID == prevEmploymentId));

			// Act
			var newDeclaration = new Declaration { ID = Guid.Parse("6c23a138-7a44-4ce2-91c7-d01f57046661") };
			await _declarationService.PreviousEmployment(declaration, newDeclaration);
			var list = await uow.Object.GetRepositoryAsync<PrevEmployment>().GetAsync();
			//Assert
			Assert.Equal(0, list.Count(x => x.ID == prevEmploymentId));
		}

		[Fact]
		public async Task PreviousEmployment_AddPreviousEmployement_AddData()
		{
			var declarationId = Guid.NewGuid();
			#region Arrange
			var declarations = new List<Declaration>()
			{
				new Declaration
				{
					ID=Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AB8562"),
					FinancialYearId=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					IsNewRegime=false
				}
			};
			var prevEmployments = new List<PrevEmployment>()
			{
				new PrevEmployment
				{
					ID = Guid.NewGuid(),
					DeclarationId = declarationId
				}
			};
			var declaration = new Declaration
			{
				ID = declarationId,
				PrevEmployment = new PrevEmployment { ID = Guid.NewGuid() }
			};

			#endregion

			#region Mock
			var declarationsMockSet = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, declarationsMockSet);
			_ = _context.GetRepositoryAsyncDbSet(uow, prevEmployments);

			#endregion

			// Act
			var newDeclaration = new Declaration { ID = Guid.NewGuid() };
			await _declarationService.PreviousEmployment(declaration, newDeclaration);
			var list = await uow.Object.GetRepositoryAsync<PrevEmployment>().GetAsync();

			//Assert
			Assert.Equal(2, list.Count());
		}

		[Fact]
		public async Task PreviousEmployment_UpdatePreviousEmployement_UpdateData()
		{
			var declarationId = Guid.NewGuid();
			#region Arrange
			var declarations = new List<Declaration>()
			{
				new Declaration
				{
					ID=Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AB8562"),
					FinancialYearId=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					IsNewRegime=false,
					PrevEmployment=new PrevEmployment{ID=Guid.NewGuid()}
				}
			};
			var prevEmployments = new List<PrevEmployment>()
			{
				new PrevEmployment
				{
					ID = Guid.NewGuid(),
					DeclarationId = declarationId ,
					EncashmentExceptions=100,
					IncomeAfterException=100,
					ProfessionalTax=100,
					ProvisionalFund=100,
					IncomeTax=100
				}
			};
			var declaration = new Declaration
			{
				ID = declarationId,
				PrevEmployment = new PrevEmployment
				{
					EncashmentExceptions = 1000,
					IncomeAfterException = 1000,
					ProfessionalTax = 1000,
					ProvisionalFund = 1000,
					IncomeTax = 1000
				}
			};

			#endregion

			#region Mock
			var declarationsMockSet = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, declarationsMockSet);

			var prevEmploymentsMockSet = prevEmployments.AsQueryable().BuildMockDbSet();
			SetData.MockPrevEmployment(uow, _context, prevEmploymentsMockSet);

			#endregion

			//Act

			var newDeclaration = new Declaration { ID = declarationId };
			await _declarationService.PreviousEmployment(declaration, newDeclaration);
			var list = await uow.Object.GetRepositoryAsync<PrevEmployment>().GetAsync();
			var previousEmployment = list.FirstOrDefault(x => x.DeclarationId == declarationId);
			//Assert
			Assert.Equal(1000, previousEmployment.EncashmentExceptions);
			Assert.Equal(1000, previousEmployment.IncomeTax);
			Assert.Equal(1000, previousEmployment.ProfessionalTax);
			Assert.Equal(1000, previousEmployment.ProvisionalFund);
		}

		[Fact]
		public async Task SectionOther_UpdateSectionOther_UpdateData()
		{
			#region Arrange
			var otherSectionId = Guid.Parse("4E2C495B-DD5E-3318-FC26-08DA64AB8562");

			var declarations = new List<Declaration>()
			{
				new Declaration
				{
					ID=Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AB8562"),
					FinancialYearId=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					IsNewRegime=false
				}
			};
			var section6Aother = new List<Section6AOther>() {
				new Section6AOther {
					ID = Guid.NewGuid(),
					DeclarationId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562") ,
					OtherSectionsId= otherSectionId,
					Amount=100
				},
				new Section6AOther
				{
					ID = Guid.NewGuid(),
					DeclarationId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8561") ,
					OtherSectionsId=Guid.Parse("4E2C495B-DD5E-3318-FC26-08DA64AB8563"),
					Amount=120
				}
			};
			var otherSections = new List<OtherSections>()
			{
				new OtherSections { ID = Guid.Parse("4e2c495b-dd5e-3318-fc26-08da64ab8561"),Limit=500,Name = "OtherSections",Status = true },
				new OtherSections { ID = Guid.Parse("4e2c495b-dd5e-3318-fc26-08da64ab8562"),Limit=200,Name = "OtherSections",Status = true }
			};
			var item = new Declaration
			{
				ID = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
				SectionOtherLines = new List<Section6AOther> {
				new Section6AOther {OtherSectionsId=otherSectionId, Amount=1000 },
				new Section6AOther {OtherSectionsId= otherSectionId, Amount=1000 },
				new Section6AOther {OtherSectionsId=Guid.Parse("4e2c495b-dd5e-3318-fc26-08da64ab8561"), Amount=1000},
				}
			};
			#endregion

			#region Mock
			var declarationsMockSet = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, declarationsMockSet);

			var otherSectionsMockSet = otherSections.AsQueryable().BuildMockDbSet();
			SetData.MockOtherSections(uow, _context, otherSectionsMockSet);

			var section6AotherMockSet = section6Aother.AsQueryable().BuildMockDbSet();
			SetData.MockSection6AOther(uow, _context, section6AotherMockSet);

			#endregion

			// Act

			var declaration = new Declaration { ID = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562") };
			await _declarationService.SectionOther(item, declaration);
			var sectionOtherList = await uow.Object.GetRepositoryAsync<Section6AOther>().GetAsync();
			var otherSectionList = await uow.Object.GetRepositoryAsync<OtherSections>().GetAsync();
			var sectionOtherAmount = sectionOtherList.FirstOrDefault(x => x.OtherSectionsId == otherSectionId);
			var otherSectionAmount = otherSectionList.FirstOrDefault(x => x.ID == otherSectionId);
			//Assert
			Assert.Equal(1000, sectionOtherAmount.Amount);
			Assert.Equal(500, otherSectionAmount.Limit);
		}

		[Fact]
		public async Task SectionOther_AddAndDeleteSectionOther_IsSuccess()
		{
			var section6AId = Guid.NewGuid();
			var newId = Guid.NewGuid();
			var deleteId = Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AB8562");
			#region Arrange
			var section6Aother = new List<Section6AOther>() {
				new Section6AOther
				{
					ID = deleteId,
				},
				new Section6AOther
				{
					ID = Guid.NewGuid(),
					DeclarationId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562") ,
					OtherSectionsId=Guid.Parse("4E2C495B-DD5E-3318-FC26-08DA64AB8563")
				}
			};
			var otherSections = new List<OtherSections>()
			{
				new OtherSections { ID = Guid.Parse("4E2C495B-AD5E-3318-FC26-08DA64AB8562") }
			};
			var item = new Declaration
			{
				ID = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
				SectionOtherLines = new List<Section6AOther> {
					new Section6AOther {ID=section6AId,OtherSectionsId=Guid.Parse("4E2C495B-DD5E-3318-FC26-08DA64AB8562") },
					new Section6AOther {ID=Guid.NewGuid(),OtherSectionsId=Guid.Parse("4E2C495B-DD5E-3318-FC26-08DA64AB8562") },
					new Section6AOther {ID=newId,OtherSectionsId=Guid.Parse("4E2C495B-AD5E-3318-FC26-08DA64AB8562")},
				}
			};

			#endregion

			#region Mock
			var _repository = _context.GetRepositoryAsyncDbSet(uow, section6Aother);
			_repository.Setup(x => x.SingleAsync(It.IsAny<Expression<Func<Section6AOther, bool>>>(),
			 It.IsAny<Func<IQueryable<Section6AOther>, IOrderedQueryable<Section6AOther>>>(),
			 It.IsAny<Func<IQueryable<Section6AOther>, IIncludableQueryable<Section6AOther, object>>>(), true)).ReturnsAsync(
				 section6Aother.FirstOrDefault(x => x.ID == deleteId));
			var otherSectionsMockSet = otherSections.AsQueryable().BuildMockDbSet();
			SetData.MockOtherSections(uow, _context, otherSectionsMockSet);

			#endregion

			// Act
			var declaration = new Declaration { ID = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562") };
			_ = _declarationService.SectionOther(item, declaration);
			var list = await uow.Object.GetRepositoryAsync<Section6AOther>().GetAsync();
			//Assert
			Assert.Equal(1, list.Count(x => x.ID == newId));//For add 
			Assert.Equal(0, list.Count(x => x.ID == deleteId));//For delete 
		}

		[Fact]
		public async Task Section6A80D_AddUpdateAndDeleteSection6A80D_IsSuccess()
		{
			var deleteId = Guid.NewGuid();
			var newId = Guid.NewGuid();
			#region Arrange

			var section6A80Ds = new List<Section6A80D>() {
				new Section6A80D
				{
					ID = Guid.Parse("3E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					DeclarationId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					Section80DId=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8561"),
					Section80D=new Section80D{ID=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8561")},
					Amount=1200
				},
				new Section6A80D
				{
					ID = newId,
					DeclarationId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562") ,
					Section80DId=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8563"),
					Amount=1000
				}
			};
			var declarations = new List<Declaration>()
			{
				new Declaration
				{
					ID=Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AB8562"),
					FinancialYearId=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					IsNewRegime=false,
					Section80DLines=section6A80Ds
				}
			};
			var section6A80D = new List<Section6A80D>() {
				new Section6A80D {
					ID =Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8561"),
					DeclarationId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					Section80DId=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8561"),
					Section80D=new Section80D{ID=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8561")}
				},
				new Section6A80D
				{
					ID = deleteId,
					DeclarationId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562") ,
					Section80DId=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562")
				}
			};
			var item = new Declaration
			{
				ID = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
				Section80DLines = section6A80Ds
			};
			var section80D = new List<Section80D>
			{
				new Section80D
			{
				ID = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
			},
					new Section80D
			{
				ID =Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8561"),
			}

			};

			#endregion

			#region Mock
			var declarationsMockSet = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, declarationsMockSet);
			_ = _context.GetRepositoryAsyncDbSet(uow, section6A80D);
			var section80DMockSet = section80D.AsQueryable().BuildMockDbSet();
			SetData.MockSection80D(uow, _context, section80DMockSet);

			#endregion

			// Act
			var declaration = new Declaration { ID = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562") };
			_ = _declarationService.Section6A80D(item, declaration);
			var list = await uow.Object.GetRepositoryAsync<Section6A80D>().GetAsync();
			var updatedList = list.FirstOrDefault(x => x.Section80DId == Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8561"));
			//Assert
			Assert.Equal(1200, updatedList.Amount);//For Update
			Assert.Equal(0, list.Count(x => x.ID == deleteId));//For delete
			Assert.Equal(1, list.Count(x => x.ID == newId));//For add
		}

		[Fact]
		public async Task Section6A80_AddUpdateAndDeleteSection6A80_IsSuccess()
		{
			var deleteId = Guid.NewGuid();
			var newId = Guid.NewGuid();
			var existingId = Guid.NewGuid();
			var declarationId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562");
			var SectionDeclarationId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562");
			var section80cId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8561");
			#region Arrange

			var section6A80Cs = new List<Section6A80C>() {
				new Section6A80C
				{
					ID = existingId,
					DeclarationId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					Section80CId=section80cId,
					Section80C=new Section80C{ID=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8561")},
					Amount=1600
				},
				new Section6A80C
				{
					ID = newId,
					DeclarationId = SectionDeclarationId ,
					Section80CId=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8563"),
					Amount=1000
				}
			};
			var section6A80C = new List<Section6A80C>() {
				new Section6A80C
				{
					ID =deleteId,
					DeclarationId = Guid.NewGuid(),
					Section80CId=Guid.NewGuid(),
				},
				new Section6A80C
				{
					ID = existingId,
					DeclarationId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					Section80CId=section80cId,
					Section80C=new Section80C{ID=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8561")},
					Amount=1200
				}
			};
			var item = new Declaration { ID = declarationId, Section80CLines = section6A80Cs };
			var section80C = new List<Section80C> { new Section80C { ID = declarationId, } };

			#endregion

			//Mock
			_ = _context.GetRepositoryAsyncDbSet(uow, section6A80C);
			var section80CMockSet = section80C.AsQueryable().BuildMockDbSet();
			SetData.MockSection80C(uow, _context, section80CMockSet);

			// Act
			var declaration = new Declaration { ID = declarationId };
			_ = _declarationService.Section6A80(item, declaration);
			var list = await uow.Object.GetRepositoryAsync<Section6A80C>().GetAsync();
			var updatedList = list.FirstOrDefault(x => x.Section80CId == section80cId);
			//Assert
			Assert.Equal(1600, updatedList.Amount);//For update
			Assert.Equal(1, list.Count(x => x.ID == newId));//For add 
			Assert.Equal(0, list.Count(x => x.ID == deleteId));//For delete 
		}

		[Fact]
		public async Task LetOutProperty_AddUpdateAndDeleteLetOutProperty_IsSuccess()
		{
			var declarationId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562");
			var SectionDeclarationId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562");
			var letOutPropertyId = Guid.Parse("4E2C495B-DD5E-3318-FC26-08DA64AB8562");
			var deleteId = Guid.NewGuid();
			var newId = Guid.NewGuid();
			#region Arrange
			var letOutProperties = new List<LetOutProperty>() {
				new LetOutProperty {
					ID = letOutPropertyId,
					DeclarationId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
				},
				new LetOutProperty
				{
					ID = deleteId,
					DeclarationId = SectionDeclarationId ,
				}
			};
			var letOutPropertys = new List<LetOutProperty>() {
				new LetOutProperty
				{
					ID =letOutPropertyId,
					DeclarationId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					LenderPAN="AAAAAAAAA",
					InterestPaid=1000
				},
				new LetOutProperty
				{
					ID = newId,
					DeclarationId = SectionDeclarationId ,
					InterestPaid=1500
				}
			};
			var item = new Declaration
			{
				ID = declarationId,
				LetOutPropertyLines = letOutPropertys
			};
			#endregion

			_ = _context.GetRepositoryAsyncDbSet(uow, letOutProperties);

			// Act
			var declaration = new Declaration { ID = declarationId };
			_ = _declarationService.LetOutProperty(item, declaration);
			var list = await uow.Object.GetRepositoryAsync<LetOutProperty>().GetAsync();
			var updatedList = list.FirstOrDefault(x => x.ID == letOutPropertyId);
			//Assert
			Assert.Equal(1000, updatedList.InterestPaid);//for update
			Assert.Equal(1, list.Count(x => x.ID == newId));//for add 
			Assert.Equal(0, list.Count(x => x.ID == deleteId));//for delete
		}

		[Fact]
		public async Task HRAUpdate_AddUpdateAndDelete_HRAUpdate()
		{
			var declarationId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562");
			var SectionDeclarationId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562");
			#region Arrange

			var hraDeclaration = new List<HraDeclaration>() {
				new HraDeclaration
				{
					ID = Guid.Parse("4E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					DeclarationId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
				},
				new HraDeclaration
				{
					ID = Guid.NewGuid(),
					DeclarationId = SectionDeclarationId ,
				}
			};
			var hRADeclaration = new List<HraDeclaration>()
			{
				new HraDeclaration
				{
					ID =Guid.Parse("4E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					DeclarationId = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					Amount=10000,
					City="Hyderabad"
				},
				new HraDeclaration
				{
					ID = Guid.NewGuid(),
					DeclarationId = SectionDeclarationId ,
					Amount=12000,
					City="Banglore"
				}
			};
			var declarations = new List<Declaration>()
			{
				new Declaration
				{
					ID=Guid.Parse("5E2C595B-DD5E-4318-FC26-08DA64AB8562"),
					FinancialYearId=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					IsNewRegime=false,
				}
			};
			var item = new Declaration
			{
				ID = declarationId,
				HRALines = hRADeclaration
			};

			#endregion

			var declarationsMockSet = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, declarationsMockSet);
			_ = _context.GetRepositoryAsyncDbSet(uow, hraDeclaration);

			// Act
			var declaration = new Declaration { ID = declarationId };
			_ = _declarationService.HRAUpdate(item, declaration);
			var list = await uow.Object.GetRepositoryAsync<HraDeclaration>().GetAsync();
			var updatedList = list.FirstOrDefault(x => x.DeclarationId == declarationId);
			//Assert
			Assert.Equal(10000, updatedList.Amount);//For update
			Assert.Equal(2, list.Count());//For add and delete
		}

		[Theory]
		[InlineData("2020-11-11", "2020-12-12", 1000, 2000)]
		[InlineData("2021-11-11", "2021-12-12", 500, 1000)]
		[InlineData("2020-08-11", "2020-10-12", 600, 1800)]
		[InlineData("2021-01-11", "2021-02-12", 800, 1600)]
		[InlineData("2021-04-11", "2021-04-11", 1300, 1300)]
		public void HRATotalTest(DateTime rentalFrom, DateTime rentalTo, int amount, int expected)
		{
			var actual = _declarationService.HRATotal(rentalFrom, rentalTo, amount);
			//Assert
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void ClearInputData_HomeLoanPayNotNull_ReturnsHomeLoanPayAsNull()
		{
			#region Arrange

			var declarations = new List<Declaration>
			{
				new Declaration
				{
				ID=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
				IncomeSource=new OtherIncomeSources { ID=Guid.Parse("4E2C495B-DD5E-3318-FC26-08DA64AB8562") },
				HomeLoanPay=new HomeLoanPay{InterestPaid = 0,Principle = 0  }
				}
			};
			var declaration = new Declaration
			{
				ID = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
				IncomeSource = new OtherIncomeSources
				{
					ID = Guid.Parse("4E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					OtherSources = 1,
					InterestOnSaving = 1,
					InterestOnFD = 0
				},
				PrevEmployment = new PrevEmployment
				{
					ID = Guid.NewGuid(),
					IncomeAfterException = 1,
					IncomeTax = 0,
					ProfessionalTax = 1,
					ProvisionalFund = 0,
					EncashmentExceptions = 0
				},
				HomeLoanPay = new HomeLoanPay { InterestPaid = 0, Principle = 0 }
			};
			#endregion

			//Mock
			var declarationsMockSet = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, declarationsMockSet);

			//Asset

			_declarationService.ClearInputData(declaration);

			//Assert
			Assert.Null(declaration.HomeLoanPay);
		}

		[Fact]
		public void ClearInputDataTest_IncomeSourceNotNull_ReturnsIncomeSourceAsNull()
		{
			#region Arrange

			var declarations = new List<Declaration>
			{
				new Declaration
				{
					ID=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
				IncomeSource=new OtherIncomeSources { ID=Guid.Parse("4E2C495B-DD5E-3318-FC26-08DA64AB8562") }
				}
			};
			var declaration = new Declaration
			{
				ID = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
				IncomeSource = new OtherIncomeSources
				{
					ID = Guid.Parse("4E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					OtherSources = 0,
					InterestOnSaving = 0,
					InterestOnFD = 0
				},
				PrevEmployment = new PrevEmployment
				{
					ID = Guid.NewGuid(),
					IncomeAfterException = 2,
					IncomeTax = 0,
					ProfessionalTax = 1,
					ProvisionalFund = 0,
					EncashmentExceptions = 0
				},
				HomeLoanPay = new HomeLoanPay
				{
					InterestPaid = 1,
					Principle = 2
				}
			};
			#endregion

			//Mock
			var declarationsMockSet = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, declarationsMockSet);

			//Asset

			_declarationService.ClearInputData(declaration);

			//Assert
			Assert.Null(declaration.IncomeSource);
		}

		[Fact]
		public void ClearInputDataTest_PrevEmployementNotNull_ReturnsPrevEmployementAsNull()
		{
			#region Arrange

			var declarations = new List<Declaration>
			{
				new Declaration
				{
					ID=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562")
				}
			};
			var declaration = new Declaration
			{
				ID = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
				IncomeSource = new OtherIncomeSources
				{
					ID = Guid.Parse("4E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					OtherSources = 0,
					InterestOnSaving = 1,
					InterestOnFD = 0
				},
				PrevEmployment = new PrevEmployment
				{
					ID = Guid.NewGuid(),
					IncomeAfterException = 0,
					IncomeTax = 0,
					ProfessionalTax = 1,
					ProvisionalFund = 0,
					EncashmentExceptions = 0
				},
				HomeLoanPay = new HomeLoanPay
				{
					InterestPaid = 1,
					Principle = 0
				}
			};
			#endregion

			//Mock
			var declarationsMockSet = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, declarationsMockSet);

			//Asset 
			_declarationService.ClearInputData(declaration);

			//Assert
			Assert.Null(declaration.PrevEmployment);
		}

		[Fact]
		public void ClearInputDataTest_ReturnsAsNotNull()
		{
			#region Arrange
			var declarations = new List<Declaration>
			{
				new Declaration
				{
				ID=Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
				IncomeSource=new OtherIncomeSources { ID=Guid.NewGuid()},
				HomeLoanPay=new HomeLoanPay{ID=Guid.NewGuid()},
				PrevEmployment=new PrevEmployment{ID=Guid.NewGuid()}
				}
			};
			var declaration = new Declaration
			{
				ID = Guid.Parse("5E2C495B-DD5E-3318-FC26-08DA64AB8562"),
				IncomeSource = new OtherIncomeSources
				{
					ID = Guid.Parse("4E2C495B-DD5E-3318-FC26-08DA64AB8562"),
					OtherSources = 0,
					InterestOnSaving = 1,
					InterestOnFD = 0
				},
				PrevEmployment = new PrevEmployment
				{
					ID = Guid.NewGuid(),
					IncomeAfterException = 1,
					IncomeTax = 12,
					ProfessionalTax = 0,
					ProvisionalFund = 0,
					EncashmentExceptions = 0
				},
				HomeLoanPay = new HomeLoanPay
				{
					InterestPaid = 1,
					Principle = 0
				}
			};
			#endregion

			//Mock
			var declarationsMockSet = declarations.AsQueryable().BuildMockDbSet();
			SetData.MockDeclaration(uow, _context, declarationsMockSet);

			//Asset
			_declarationService.ClearInputData(declaration);

			//Assert
			Assert.NotNull(declaration.HomeLoanPay);
			Assert.NotNull(declaration.PrevEmployment);
			Assert.NotNull(declaration.IncomeSource);
		}

		[Fact]
		public async Task GetEmployeeStateId_EmployeeState_GetSingleEmployeeStateData()
		{
			var employeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c");
			var stateId = Guid.NewGuid();
			var employees = new List<Employee>()
			{
				new Employee
				{
					ID=employeeId,
					WorkLocation=new Location{StateId=stateId}
				}
			};

			//Mock
			var employeesMockSet = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDataGenerator(uow, _context, employeesMockSet);

			//Act
			var dd = await _declarationService.GetEmployeeStateId(employeeId);
			//Assert
			Assert.Equal(stateId, dd);
		}
		[Fact]
		public async Task GetEmployeeStateId_EmployeeState_GetNoData()
		{
			var employees = _employeeData.GetAllEmployeesData();

			//Mock
			var employeesMockSet = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDataGenerator(uow, _context, employeesMockSet);

			//Act
			var dd = await _declarationService.GetEmployeeStateId(Guid.NewGuid());
			//Assert
			Assert.Equal(Guid.Empty, dd);
		}
	}
}
