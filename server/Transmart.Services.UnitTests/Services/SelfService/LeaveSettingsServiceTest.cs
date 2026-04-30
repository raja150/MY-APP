using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.SelfServiceData;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Service.Leave;
using Xunit;

namespace Transmart.Services.UnitTests.Services.SelfService
{
	public class LeaveSettingsServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly ILeaveSettingsService _service;
		private readonly Mock<DbContext> _context;

		public LeaveSettingsServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new LeaveSettingsService(uow.Object);
			_context = new Mock<DbContext>();
		}
		private void LeaveSettingsMockData()
		{
			#region Arrange
			IEnumerable<LeaveSettings> data = new List<LeaveSettings>
			{
				new LeaveSettings
				{
					ID = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					CompoLeaveType=new LeaveType{Name="fhg"},
					CompCreditTo=new LeaveType{Name="fhf"}
				},
				new LeaveSettings
				{
					ID = Guid.NewGuid(),
					CompoLeaveType=new LeaveType{Name="EL"},
					CompCreditTo=new LeaveType{Name="fhf"}
				}

			};
			#endregion

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveSettings(uow, _context, mockSet);
		}

		[Fact]
		public async Task CustomValidation_ShouldNotAllowDuplicates_ThrowException()
		{
			LeaveSettingsMockData();

			LeaveSettings leave = new()
			{
				ID = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
				CompoLeaveType = new LeaveType { Name = "ghgd" },
				CompCreditTo = new LeaveType { Name = "dsdfg" }
			};

			//Act
			var leaveSettings = await _service.AddAsync(leave);

			//Assert
			Assert.False(leaveSettings.IsSuccess);
		}

		[Fact]
		public async Task CustomValidation_DataSaved_WithOutException()
		{
			LeaveSettingsMockData();

			LeaveSettings leave = new()
			{
				ID = Guid.Parse("bdd2fa1c-931c-4298-9d4a-0035b96edff6"),
				CompoLeaveType = new LeaveType { Name = "ghgd" },
				CompCreditTo = new LeaveType { Name = "dsdfg" }
			};

			//Act
			var leaveSettings = await _service.AddAsync(leave);

			//Assert
			Assert.True(leaveSettings.IsSuccess);
		}

	}
}
