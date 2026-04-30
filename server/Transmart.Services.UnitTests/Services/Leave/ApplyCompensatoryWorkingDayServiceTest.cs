using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using Transmart.Services.UnitTests.Services.SelfServiceData;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models;
using TranSmart.Service.Leave;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Leave
{
    public class ApplyCompensatoryWorkingDayServiceTest
    {
        private readonly Mock<TranSmartContext> _dbContext;
        private readonly Mock<UnitOfWork<TranSmartContext>> uow;
        private readonly IApplyCompensatoryWorkingDayService _applyCompensatoryWorkingDayService;
		private readonly EmployeeDataGenerator _employeeData;
		private readonly Mock<DbContext> _context;
		public ApplyCompensatoryWorkingDayServiceTest()
        {
            var builder = new DbContextOptionsBuilder<TranSmartContext>();
            builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
            var app = new Mock<IApplicationUser>();
            var dbContextOptions = builder.Options;
            _dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
            uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
            _applyCompensatoryWorkingDayService = new ApplyCompensatoryWorkingDayService(uow.Object);
            _employeeData = new EmployeeDataGenerator();
			_context = new Mock<DbContext>();
		}

		private void ApplyCompoMockData()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();

			IEnumerable<ApplyCompo> data = new List<ApplyCompo>
			{
				new ApplyCompo
				{
					EmployeeId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					ID = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					AdminReason = "hfgh",
					FromDate = DateTime.Parse("2022-07-15"),
					ToDate = DateTime.Parse("2022-07-15"),
					ReasonForApply= "dfgf",
					Status = 1,
				},
				new ApplyCompo
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					AdminReason = "hfgh",
					FromDate = DateTime.Parse("2022-07-15"),
					ToDate = DateTime.Parse("2022-07-15"),
					ReasonForApply = "dfgf",
					Status = 1,
				}

			};

			IEnumerable<LeaveBalance> leaveBalance = new List<LeaveBalance>
			{
				new LeaveBalance
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),

				}
			};

			IEnumerable<LeaveSettings> leaveSettings = new List<LeaveSettings>
			{
				new  LeaveSettings
				{
					CompoLeaveTypeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),

				}
			};

			#endregion

			#region Mock

			//ApplyCompensatoryWorkingDay
			var mockSetApplyCompensatoryWorkingDay = data.AsQueryable().BuildMockDbSet();
			SetData.MockApplyCompensatoryWorkingDay(uow, _context, mockSetApplyCompensatoryWorkingDay);

			//LeaveBalance
			var mockSetLeaveBalance = leaveBalance.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			//LeaveSettings
			var mockSetLeaveSettings = leaveSettings.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveSettings(uow, _context, mockSetLeaveSettings);

			#endregion
		}

		[Fact]
		public async Task Get_SearchWithId_GetValidRecords()
		{
			ApplyCompoMockData();

			//Act
			var applyCompensatory = await _applyCompensatoryWorkingDayService.GetById(Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"));

			//Assert
			Assert.Equal(Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"), applyCompensatory.ID);
		}

		[Fact]
		public async Task GetPaginate_SearchWithReference_GetValidRecords()
		{
			//Arrange
			ApplyCompoMockData();
			BaseSearch baseSearch = new()
			{
				SortBy = "Employee",
				Size = 10,
				Page = 0,
				RefId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6")
			};

			//Act
			var applyCompensatory = await _applyCompensatoryWorkingDayService.GetPaginate(baseSearch);

			//Assert
			Assert.Equal(1, applyCompensatory.Items.Count(x => x.EmployeeId == Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6")));
		}

        [Fact]
        public async Task CustomValidation_DataAdded_WithOutException()
        {
            // Arrange
            var employee = _employeeData.GetEmployeeData();
			Result<ApplyCompo> result = new();
			ApplyCompoMockData();

			ApplyCompo applyCompo = new()
            {
                EmployeeId = employee.ID,
                AdminReason = "dfdgdf",
                ReasonForApply = "fbdxfb",
                FromDate = DateTime.Parse("2022-07-17"),
                ToDate = DateTime.Parse("2022-07-18"),
                Status = 1,
            };

			//Act
			var service = new ApplyCompensatoryWorkingDayService(uow.Object);
			await service.CustomValidation(applyCompo, result);

			//Assert
			Assert.True(result.HasNoError);
		}

		[Fact]
		public async Task CustomValidation_FromDateLessThenToDate_ThrowException()
		{
			// Arrange
			var employee = _employeeData.GetEmployeeData();
			Result<ApplyCompo> result = new();
			ApplyCompoMockData();

			ApplyCompo applyCompo = new()
			{
				EmployeeId = employee.ID,
				AdminReason = "dfdgdf",
				ReasonForApply = "fbdxfb",
				FromDate = DateTime.Parse("2022-07-18"),
				ToDate = DateTime.Parse("2022-07-17"),
				Status = 1,
			};

			//Act
			var service = new ApplyCompensatoryWorkingDayService(uow.Object);
			await service.CustomValidation(applyCompo, result);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public async Task CustomValidation_AlreadyApplied_ThrowException()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			Result<ApplyCompo> result = new();
			IEnumerable<ApplyCompo> data = new List<ApplyCompo>
			{
				new ApplyCompo
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					AdminReason = "hfgh",
					ReasonForApply="fbdxfb",
					FromDate = DateTime.Parse("2022-07-15"),
					ToDate = DateTime.Parse("2022-07-15"),
					Status = 1,
				},
				new ApplyCompo
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					AdminReason = "hfgh",
					ReasonForApply="fbdxfb",
					FromDate = DateTime.Parse("2022-07-15"),
					ToDate = DateTime.Parse("2022-07-15"),
					Status = 2,
				}

			};
			IEnumerable<LeaveBalance> leaveBalance = new List<LeaveBalance>
			{
				new LeaveBalance
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),

				}
			};

			IEnumerable<LeaveSettings> leaveSettings = new List<LeaveSettings>
			{
				new  LeaveSettings
				{
					CompoLeaveTypeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),

				}
			};
			ApplyCompo applyCompo = new()
			{
				EmployeeId = employee.ID,
				AdminReason = "dfdgdf",
				ReasonForApply = "fbdxfb",
				FromDate = DateTime.Parse("2022-07-15"),
				ToDate = DateTime.Parse("2022-07-15"),
				Status = 1,
			};

			#endregion

			#region Mock

			//ApplyCompensatoryWorkingDay
			var mockSetApplyCompensatoryWorkingDay = data.AsQueryable().BuildMockDbSet();
			SetData.MockApplyCompensatoryWorkingDay(uow, _context, mockSetApplyCompensatoryWorkingDay);

			//LeaveBalance
			var mockSetLeaveBalance = leaveBalance.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			//LeaveSettings
			var mockSetLeaveSettings = leaveSettings.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveSettings(uow, _context, mockSetLeaveSettings);

			#endregion

			//Act
			var service = new ApplyCompensatoryWorkingDayService(uow.Object);
			await service.CustomValidation(applyCompo, result);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public async Task SelfServiceSearch_SearchWithReference_GetValidRecords()
		{
			//Arrange
			ApplyCompoMockData();

			BaseSearch baseSearch = new()
			{
				SortBy = "Employee",
				Size = 10,
				Page = 0,
				RefId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6")
			};

			// Act
			var applyCompensatory = await _applyCompensatoryWorkingDayService.SelfServiceSearch(baseSearch);

			//Assert
			Assert.Equal(1, applyCompensatory.Items.Count(x => x.EmployeeId == Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6")));
		}

	}
}
