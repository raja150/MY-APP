using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Transmart.Services.UnitTests;
using Transmart.Services.UnitTests.Services.Payroll.PayRollData;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Service.Payroll;
using Xunit;

namespace TranSmart.Services.UnitTests
{
	public class FinancialYearTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly FinancialYearService financialYearService;
		private readonly Mock<DbContext> _context;
		public FinancialYearTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			financialYearService = new FinancialYearService(uow.Object);
			_context = new Mock<DbContext>();
		}

		[Fact]
		public async Task CustomValidation_MonthsNotNull_ThrowError()
		{
			#region Arrange
			var financialYears = new List<FinancialYear>
			{
				new FinancialYear
				{
					ID=Guid.NewGuid(),
					FromYear=2020,
					ToYear=2021
				}
			};
			var payMonths = new List<PayMonth>
			{
				new PayMonth
				{
					ID=Guid.NewGuid(),
					Status=1
				}
			};

			#endregion

			#region Mock

			var mockSet = financialYears.AsQueryable().BuildMockDbSet();
			SetData.MockFinancialYear(uow, _context, mockSet);

			var payMonthsMockSet = payMonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, payMonthsMockSet);


			#endregion

			var excutionResult = new Result<FinancialYear>();
			//Act
			await financialYearService.CustomValidation(
				new FinancialYear
				{
					ID = Guid.NewGuid(),
					FromYear = 2020,
					ToYear = 2021,
				}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasError);
		}

		[Fact]
		public async Task CustomValidation_FinancialLessThanTwoYears_ThrowError()
		{
			#region Arrange
			var financialYears = new List<FinancialYear>
			{
				new FinancialYear
				{
					ID=Guid.NewGuid(),
					FromYear=2020,
					ToYear=2021
				},
				new FinancialYear
				{
					ID=Guid.NewGuid(),
				}
			};
			var payMonths = new List<PayMonth>
			{
				new PayMonth
				{
					ID=Guid.Parse("6D43C8BB-4B07-4C34-5157-08DA64AB8559"),
					Status=0
				},
				new PayMonth
				{
					ID=Guid.Parse("6D53C8BB-4B07-4C34-5157-08DA64AB8559"),
				}
			};
			#endregion

			#region Mock

			var mockSet = financialYears.AsQueryable().BuildMockDbSet();
			SetData.MockFinancialYear(uow, _context, mockSet);

			var payMonthsMockSet = payMonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, payMonthsMockSet);

			#endregion

			var excutionResult = new Result<FinancialYear>();
			//Act
			await financialYearService.CustomValidation(
				new FinancialYear
				{
					ID = Guid.NewGuid(),
					FromYear = 2019,
					ToYear = 2020,
				}, excutionResult);
			Assert.True(excutionResult.HasError);
		}

		[Fact]
		public async Task CustomValidation_FromYearToYearDifferenceIsNotOne_ThrowError()
		{
			//Arrange
			var financialYears = new List<FinancialYear>
			{
				new FinancialYear
				{
					ID=Guid.NewGuid(),
					FromYear=2020,
					ToYear=2021
				},
				new FinancialYear
				{
					ID=Guid.NewGuid(),
				}
			};
			var payMonths = new List<PayMonth>
			{
				new PayMonth
				{
					ID=Guid.Parse("6D43C8BB-4B07-4C34-5157-08DA64AB8559"),
					Status=0
				},
				new PayMonth
				{
					ID=Guid.Parse("6D53C8BB-4B07-4C34-5157-08DA64AB8559"),
				}
			};

			// Mock
			var mockSet = financialYears.AsQueryable().BuildMockDbSet();
			SetData.MockFinancialYear(uow, _context, mockSet);

			var payMonthsMockSet = payMonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, payMonthsMockSet);

			var excutionResult = new Result<FinancialYear>();
			//Act
			await financialYearService.CustomValidation(
				new FinancialYear
				{
					ID = Guid.NewGuid(),
					FromYear = 2020,
					ToYear = 2022,
				}, excutionResult);
			Assert.True(excutionResult.HasError);
		}

		[Fact]
		public async Task CustomValidation_FinacialYearExists_ThrowError()
		{
			#region Arrange
			var financialYears = new List<FinancialYear>
			{
				new FinancialYear
				{
					ID=Guid.NewGuid(),
					FromYear=2020,
					ToYear=2021
				},
				new FinancialYear
				{
					ID=Guid.NewGuid(),
				}
			};
			var payMonths = new List<PayMonth>
			{
				new PayMonth
				{
					ID=Guid.NewGuid(),
					Status=0
				},
				new PayMonth
				{
					ID=Guid.NewGuid(),
				}
			};
			#endregion

			#region Mock

			var mockSet = financialYears.AsQueryable().BuildMockDbSet();
			SetData.MockFinancialYear(uow, _context, mockSet);

			var payMonthsMockSet = payMonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, payMonthsMockSet);

			#endregion

			var excutionResult = new Result<FinancialYear>();
			//Act
			await financialYearService.CustomValidation(
				new FinancialYear
				{
					ID = Guid.NewGuid(),
					FromYear = 2020,
					ToYear = 2021,
				}, excutionResult);
			Assert.True(excutionResult.HasError);

		}

		[Fact]
		public async Task CustomValidation_FromYearExists_ThrowError()
		{
			var financialYearId = Guid.NewGuid();
			#region Arrange
			var financialYears = new List<FinancialYear>
			{
				new FinancialYear
				{
					ID=financialYearId,
					FromYear=2020,
					ToYear=2021
				},
				new FinancialYear
				{
					ID=Guid.NewGuid(),
				}
			};
			var payMonths = new List<PayMonth>
			{
				new PayMonth
				{
					ID=Guid.NewGuid(),
					Status=0
				},
				new PayMonth
				{
					ID=Guid.NewGuid(),
				}
			};
			#endregion

			#region Mock

			var mockSet = financialYears.AsQueryable().BuildMockDbSet();
			SetData.MockFinancialYear(uow, _context, mockSet);

			var payMonthsMockSet = payMonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, payMonthsMockSet);

			#endregion

			var excutionResult = new Result<FinancialYear>();
			//Act
			await financialYearService.CustomValidation(
			   new FinancialYear
			   {
				   ID = financialYearId,
				   FromYear = 2021,
				   ToYear = 2021,
			   }, excutionResult);
			Assert.True(excutionResult.HasError);

		}

		[Fact]
		public async Task CustomValidation_ToYearExists_ThrowError()
		{
			var financialYearId = Guid.NewGuid();
			#region Arrange
			var financialYears = new List<FinancialYear>
			{
				new FinancialYear
				{
					ID=financialYearId,
					FromYear=2020,
					ToYear=2021
				},
				new FinancialYear
				{
					ID=Guid.NewGuid(),
				}
			};
			var payMonths = new List<PayMonth>
			{
				new PayMonth
				{
					ID=Guid.NewGuid(),
					Status=0
				},
				new PayMonth
				{
					ID=Guid.NewGuid(),
				}
			};
			#endregion

			#region Mock

			var mockSet = financialYears.AsQueryable().BuildMockDbSet();
			SetData.MockFinancialYear(uow, _context, mockSet);

			var payMonthsMockSet = payMonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, payMonthsMockSet);

			#endregion

			var excutionResult = new Result<FinancialYear>();
			//Act
			await financialYearService.CustomValidation(
				new FinancialYear
				{
					ID = financialYearId,
					FromYear = 2020,
					ToYear = 2022,
				}, excutionResult);
			Assert.True(excutionResult.HasError);

		}

		[Fact]
		public async Task CustomValidation_NoError_IsSuccess()
		{
			var financialYearId = Guid.NewGuid();
			#region Arrange
			var financialYears = new List<FinancialYear>
			{
				new FinancialYear
				{
					ID=financialYearId,
					FromYear=2020,
					ToYear=2021
				},
				new FinancialYear
				{
					ID=Guid.NewGuid(),
				}
			};
			var payMonths = new List<PayMonth>
			{
				new PayMonth
				{
					ID=Guid.NewGuid(),
					Status=0
				},
				new PayMonth
				{
					ID=Guid.NewGuid(),
				}
			};
			#endregion

			#region Mock

			var mockSet = financialYears.AsQueryable().BuildMockDbSet();
			SetData.MockFinancialYear(uow, _context, mockSet);

			var payMonthsMockSet = payMonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, payMonthsMockSet);

			#endregion

			var excutionResult = new Result<FinancialYear>();
			//Act
			await financialYearService.CustomValidation(
				new FinancialYear
				{
					ID = Guid.NewGuid(),
					FromYear = 2021,
					ToYear = 2022,
				}, excutionResult);
			Assert.True(excutionResult.HasNoError);
		}

		[Fact]
		public async Task OnBeforeAdd_HasError_ThrowError()
		{
			var financialYearId = Guid.NewGuid();
			#region Arrange
			var organizations = new List<Organizations>
			{
				new Organizations
				{
					ID=Guid.Parse("6D14C8BB-4B07-4C34-5157-08DA64AB8559"),
					MonthStartDay=26,
				}
			};
			var paySettings = new List<PaySettings>
			{
				new PaySettings
				{
					ID=Guid.Parse("6D13C8BB-4B07-4C34-5157-08DA64AB8559"),
					Organization=new Organizations{ID=Guid.Parse("6D14C8BB-4B07-4C34-5157-08DA64AB8559"),MonthStartDay=2},
					FYFromMonth=4,
					OrganizationId = organizations.FirstOrDefault().ID
				}
			};

			var financialYears = new List<FinancialYear>
			{
				new FinancialYear
				{
					ID=financialYearId,
					FromYear=2020,
					ToYear=2021
				},
				new FinancialYear
				{
					ID=Guid.NewGuid(),
				}
			};
			var payMonths = new List<PayMonth>
			{
				new PayMonth
				{
					ID=Guid.NewGuid(),
					Status=0
				},
				new PayMonth
				{
					ID=Guid.NewGuid(),
				}
			};
			#endregion

			#region Mock

			var mockSet = financialYears.AsQueryable().BuildMockDbSet();
			SetData.MockFinancialYear(uow, _context, mockSet);

			var payMonthsMockSet = payMonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, payMonthsMockSet);

			var paySettingsMockSet = paySettings.AsQueryable().BuildMockDbSet();
			SetData.MockPaySettings(uow, _context, paySettingsMockSet);

			var organizationsMockSet = organizations.AsQueryable().BuildMockDbSet();
			SetData.MockOrganizations(uow, _context, organizationsMockSet);
			var payMonthsAdded = new List<PayMonth>();
			payMonthsMockSet.Setup(x => x.AddAsync(It.IsAny<PayMonth>(), CancellationToken.None)).Callback((PayMonth a, CancellationToken token) =>
			{
				payMonthsAdded.Add(a);

			});


			#endregion

			var excutionResult = new Result<FinancialYear>();
			//Act
			await financialYearService.OnBeforeAdd(
				new FinancialYear
				{
					ID = Guid.NewGuid(),
					FromYear = DateTime.Now.Year,
					ToYear = DateTime.Now.Year + 1,
					PaySettingsId = paySettings.FirstOrDefault().ID
				}, excutionResult);

			Assert.Equal(12, payMonthsAdded.Count());
			Assert.Equal($"March - {DateTime.Now.Year + 1}", payMonthsAdded[11].Name);
			Assert.True(excutionResult.HasNoError);
		}

		[Fact]
		public async Task OnBeforeAdd_PaySettingsIsNull_ThrowError()
		{
			var financialYearId = Guid.NewGuid();
			#region Arrange
			var financialYears = new List<FinancialYear>
			{
				new FinancialYear
				{
					ID=financialYearId,
					FromYear=2020,
					ToYear=2021
				},
				new FinancialYear
				{
					ID=Guid.NewGuid(),
				}
			};
			var payMonths = new List<PayMonth>
			{
				new PayMonth
				{
					ID=Guid.NewGuid(),
					Status=0
				},
				new PayMonth
				{
					ID=Guid.NewGuid(),
				}
			};
			var paySettings = new List<PaySettings>
			{
				new PaySettings
				{
					ID=Guid.Parse("6D13C8BB-4B07-4C34-5157-08DA64AB8559"),
					Organization=new Organizations{ID=Guid.Parse("6D14C8BB-4B07-4C34-5157-08DA64AB8559"),MonthStartDay=2},
					FYFromMonth=3
				}
			};
			var organizations = new List<Organizations>
			{
				new Organizations
				{
					ID=Guid.Parse("6D14C8BB-4B07-4C34-5157-08DA64AB8559"),
				}
			};
			#endregion

			#region Mock

			var mockSet = financialYears.AsQueryable().BuildMockDbSet();
			SetData.MockFinancialYear(uow, _context, mockSet);

			var payMonthsMockSet = payMonths.AsQueryable().BuildMockDbSet();
			SetData.MockPayMonth(uow, _context, payMonthsMockSet);

			var paySettingsMockSet = paySettings.AsQueryable().BuildMockDbSet();
			SetData.MockPaySettings(uow, _context, paySettingsMockSet);

			var organizationsMockSet = organizations.AsQueryable().BuildMockDbSet();
			SetData.MockOrganizations(uow, _context, organizationsMockSet);

			#endregion

			var excutionResult = new Result<FinancialYear>();
			//Act
			await financialYearService.OnBeforeAdd(
				new FinancialYear
				{
					ID = Guid.NewGuid(),
					FromYear = 2021,
					ToYear = 2022,
				}, excutionResult);
			Assert.True(excutionResult.HasError);
		}

		[Fact]
		public async Task OnBeforeAdd_PaySettingsIsNotNull_IsSuccess()
		{
			var paySettingsId = Guid.NewGuid();
			#region Arrange
			var financialYears = new List<FinancialYear>
			{
				new FinancialYear
				{
					ID=Guid.NewGuid(),
					FromYear=2020,
					ToYear=2021
				},
				new FinancialYear
				{
					ID=Guid.NewGuid(),
				}
			};
			var payMonths = new List<PayMonth>
			{
				new PayMonth
				{
					ID=Guid.NewGuid(),
					Status=0
				},
				new PayMonth
				{
					ID=Guid.NewGuid(),
				}
			};
			var paySettings = new List<PaySettings>
			{
				new PaySettings
				{
					ID=paySettingsId,
					Organization=new Organizations{ID=Guid.Parse("6D14C8BB-4B07-4C34-5157-08DA64AB8559"),MonthStartDay=2},
					FYFromMonth=3
				}
			};
			var organizations = new List<Organizations>
			{
				new Organizations
				{
					ID=Guid.Parse("6D14C8BB-4B07-4C34-5157-08DA64AB8559"),
				}
			};
			#endregion

			#region Mock

			_ = _context.GetRepositoryAsyncDbSet(uow, financialYears);
			_ = _context.GetRepositoryAsyncDbSet(uow, payMonths);
			var paySettingsMockSet = paySettings.AsQueryable().BuildMockDbSet();
			SetData.MockPaySettings(uow, _context, paySettingsMockSet);
			var organizationsMockSet = organizations.AsQueryable().BuildMockDbSet();
			SetData.MockOrganizations(uow, _context, organizationsMockSet);

			#endregion

			var excutionResult = new Result<FinancialYear>();
			//Act
			await financialYearService.OnBeforeAdd(
				new FinancialYear
				{
					ID = Guid.NewGuid(),
					FromYear = 2021,
					ToYear = 2022,
					PaySettingsId = paySettingsId,
				}, excutionResult);
			var list = await uow.Object.GetRepositoryAsync<PayMonth>().GetAsync();

			//Assert
			Assert.Equal(14, list.Count());
		}

		[Fact]
		public async Task OpenYearsTest_GetClosedList_IsSuccess()
		{
			// Arrange
			var financialYears = new List<FinancialYear>
			{
				new FinancialYear
				{
					ID=Guid.NewGuid(),
					Closed=true
				},
				new FinancialYear
				{
					ID=Guid.NewGuid(),
					Closed=false
				}
			};

			// Mock
			var financialYearsMockSet = financialYears.AsQueryable().BuildMockDbSet();
			SetData.MockFinancialYear(uow, _context, financialYearsMockSet);

			//Act
			var dd = await financialYearService.OpenYears();

			//Assert
			Assert.True(dd.Count() == 1);
		}
	}

}

