using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
	public class WeekOffDaysServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IWeekOffDaysService _service;
		private readonly Mock<DbContext> _context;

		public WeekOffDaysServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new WeekOffDaysService(uow.Object);
			_context = new Mock<DbContext>();
		}

		private void DeleteWeekOffDaysMockData()
		{
			//Arrange
			IEnumerable<WeekOffDays> data = new List<WeekOffDays>
			{
				new WeekOffDays
				{
					ID = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					WeekOffSetupId=Guid.NewGuid(),
					Status = 1,
				},
				new WeekOffDays
				{
					ID = Guid.NewGuid(),
					WeekOffSetupId=Guid.NewGuid(),
					Status = 1,
				}

			};

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockWeekOffDays(uow, _context, mockSet);
		}

		private void WeekOffDaysMOckData()
		{
			//Arrange
			IEnumerable<WeekOffDays> data = new List<WeekOffDays>
			{
				new WeekOffDays
				{
					ID = Guid.NewGuid(),
					WeekOffSetupId=Guid.NewGuid(),
					Status = 1,
					WeekDate=DateTime.Parse("2022-8-22"),
					WeekDay=2
				},
				new WeekOffDays
				{
					ID = Guid.NewGuid(),
					WeekOffSetupId=Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					Status = 1,
					WeekDate=DateTime.Parse("2022-8-22"),
					WeekDay=1
				}
			};

			// Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockWeekOffDays(uow, _context, mockSet);
		}

		[Fact]
		public async Task DeleteWeekOffDays_SaveChangesFailed_ThrowException()
		{
			//Arrange
			IEnumerable<WeekOffDays> data = new List<WeekOffDays>
			{
				new WeekOffDays
				{
					ID = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					WeekOffSetupId=Guid.NewGuid(),
					Status = 1,
				},
				new WeekOffDays
				{
					ID = Guid.NewGuid(),
					WeekOffSetupId=Guid.NewGuid(),
					Status = 1,
				}

			};

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockWeekOffDays(uow, _context, mockSet);

			uow.Setup(x => x.SaveChangesAsync()).Throws(new InvalidOperationException());
			// Act
			var weekOffDays = await _service.DeleteWeekOffDays(Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"));

			//Assert
			Assert.False(weekOffDays.IsSuccess);
		}

		[Fact]
		public async Task DeleteWeekOffDays_InvalidItem_ThrowException()
		{
			DeleteWeekOffDaysMockData();
			// Act
			var weekOffDays = await _service.DeleteWeekOffDays(Guid.Parse("bdd2fa1c-93ac-4298-9d4a-0035b96edff6"));

			//Assert
			Assert.False(weekOffDays.IsSuccess);
		}

		[Fact]
		public async Task DeleteWeekOffDays_DataSave_WithOutException()
		{
			DeleteWeekOffDaysMockData();
			// Act
			var weekOffDays = await _service.DeleteWeekOffDays(Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"));

			//Assert
			uow.Verify(c => c.SaveChangesAsync());
			Assert.True(weekOffDays.HasNoError);
		}

		[Fact]
		public async Task GetAllWeekOffDays_SearchWithReference_GetValidRecords()
		{
			BaseSearch baseSearch = new()
			{
				SortBy = "Employee",
				Size = 10,
				Page = 0,
				RefId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6")
			};

			WeekOffDaysMOckData();

			//Act
			var weekOffDays = await _service.GetAllWeekOffDays(baseSearch);

			//Assert
			Assert.Equal(1, weekOffDays.Items.Count(x => x.WeekOffSetupId == Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6")));
		}

		[Fact]
		public async Task CustomValidation_WeekDayAndWeekNoInMonthRequired_ThrowException()
		{
			WeekOffDaysMOckData();
			Result<WeekOffDays> result = new();
			WeekOffDays weekOff = new()
			{
				ID = Guid.NewGuid(),
				WeekOffSetupId = Guid.NewGuid(),
				Type = 1,
			};

			// Act
			var service = new WeekOffDaysService(uow.Object);
			await service.CustomValidation(weekOff, result);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public async Task CustomValidation_WeekDayAndWeekInYearRequired_ThrowException()
		{
			WeekOffDaysMOckData();
			Result<WeekOffDays> result = new();
			WeekOffDays weekOff = new()
			{
				ID = Guid.NewGuid(),
				WeekOffSetupId = Guid.NewGuid(),
				Type = 2,
			};

			// Act
			var service = new WeekOffDaysService(uow.Object); 
			await service.CustomValidation(weekOff, result);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public async Task CustomValidation_WeekDateAndStatusRequired_ThrowException()
		{
			WeekOffDaysMOckData();
			Result<WeekOffDays> result = new();
			WeekOffDays weekOff = new()
			{
				ID = Guid.NewGuid(),
				WeekOffSetupId = Guid.NewGuid(),
				Type = 3,
			};

			// Act
			var service = new WeekOffDaysService(uow.Object);
			await service.CustomValidation(weekOff, result);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public async Task CustomValidation_WeekDateAlreadyExist_ThrowException()
		{
			WeekOffDaysMOckData();
			Result<WeekOffDays> result = new();
			WeekOffDays weekOff = new()
			{
				ID = Guid.NewGuid(),
				WeekOffSetupId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
				Type = 3,
				Status = 1,
				WeekDay = 2,
				WeekDate = DateTime.Parse("2022-8-22"),
				WeekInYear = 2022,
				WeekNoInMonth = "1st"

			};

			// Act
			var service = new WeekOffDaysService(uow.Object);
			await service.CustomValidation(weekOff, result);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public async Task CustomValidation_WeekDayAlreadyExist_ThrowException()
		{
			//Arrange
			WeekOffDaysMOckData();
			Result<WeekOffDays> result = new();

			WeekOffDays weekOff = new()
			{
				ID = Guid.NewGuid(),
				WeekOffSetupId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
				Type = 1,
				Status = 1,
				WeekDay = 1,
				WeekDate = DateTime.Parse("2022-8-22"),
				WeekInYear = 2022,
				WeekNoInMonth = "1st"

			};

			// Act
			var service = new WeekOffDaysService(uow.Object);
			await service.CustomValidation(weekOff, result);

			//Assert
			Assert.True(result.HasError);

		}

		[Fact]
		public async Task CustomValidation_DataSaved_WithOutException()
		{
			WeekOffDaysMOckData();
			Result<WeekOffDays> result = new();
			WeekOffDays weekOff = new()
			{
				ID = Guid.NewGuid(),
				WeekOffSetupId = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
				Type = 3,
				Status = 1,
				WeekDay = 2,
				WeekDate = DateTime.Parse("2022-8-22"),
				WeekInYear = 2022,
				WeekNoInMonth = "1st"

			};

			// Act
			var service = new WeekOffDaysService(uow.Object);
			await service.CustomValidation(weekOff, result);

			//Assert
			Assert.True(result.HasNoError);
		}

	}
}
