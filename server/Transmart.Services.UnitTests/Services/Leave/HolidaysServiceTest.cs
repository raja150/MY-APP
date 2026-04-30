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
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Service.Leave;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Leave
{
	public class HolidaysServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IHolidaysService _holidayservice;
		private readonly Mock<DbContext> _context;
		public HolidaysServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_holidayservice = new HolidaysService(uow.Object);
			_context = new Mock<DbContext>();
		}

		private void HolidaysMockData()
		{
			#region Arrange
			IEnumerable<Holidays> data = new List<Holidays>
			{
				new Holidays
				{
					ID = Guid.NewGuid(),
					Name = "Diwali",
					Date = DateTime.Parse("2022-07-10"),
					Description = "dfgf",
					ReprApplication = "dfgf",
					Type = 1,
					Duration = 2,
				},
				new Holidays
				{

					ID = Guid.NewGuid(),
					Name = "Pongal",
					Date = DateTime.Parse("2022-07-1"),
					Description = "dfgf",
					ReprApplication = "dfgf",
					Type = 1,
					Duration = 2,
				}

			};

			#endregion

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockHolidays(uow, _context, mockSet);
		}

		[Fact]
		public async Task PastHolidays_SearchHolidaysBetweenFromDateAndToDate_GetValidRecords()
		{
			HolidaysSearch holidaysSearch = new()
			{
				FromDate = DateTime.Parse("2022-06-28"),
				ToDate = DateTime.Parse("2022-07-9"),
				SortBy = "Employee",
				Size = 10,
				Page = 0,
			};

			HolidaysMockData();

			//Act
			var hs = await _holidayservice.Past(holidaysSearch);

			//Assert
			Assert.Equal(1, hs.Count);
		}

		[Fact]
		public async Task UpcommingHolidays_SearchHolidaysBetweenFromDateAndToDate_GetValidRecords()
		{
			#region Arrange
			HolidaysSearch holidaysSearch = new()
			{
				SortBy = "Employee",
				FromDate = DateTime.Now,
				ToDate = DateTime.Now.AddDays(6),
				Size = 10,
				Page = 0,
			};

			IEnumerable<Holidays> data = new List<Holidays>
			{
				new Holidays
				{
					ID = Guid.NewGuid(),
					Name = "Diwali",
					Date = DateTime.Now.AddDays(1),
					Description = "dfgf",
					ReprApplication = "dfgf",
					Type = 1,
					Duration = 2,
				},
				new Holidays
				{

					ID = Guid.NewGuid(),
					Name = "Pongal",
					Date = DateTime.Now.AddDays(3),
					Description = "dfgf",
					ReprApplication = "dfgf",
					Type = 1,
					Duration = 2,
				},
				new Holidays
				{

					ID = Guid.NewGuid(),
					Name = "Dussehra",
					Date = DateTime.Now.AddDays(10),
					Description = "dfgf",
					ReprApplication = "dfgf",
					Type = 1,
					Duration = 2,
				}

			};

			#endregion

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockHolidays(uow, _context, mockSet);

			//Act
			var hs = await _holidayservice.Future(holidaysSearch);

			//Assert
			Assert.Equal(2, hs.Count);
		}

		[Fact]
		public async Task CustomValidation_DateAlreadyExist_ThrowException()
		{
			//Arrange
			Result<Holidays> result = new();
			Holidays holidays = new()
			{
				ID = Guid.NewGuid(),
				Name = "Dussehra",
				Date = DateTime.Parse("2022-07-10"),
				Description = "dfgf",
				ReprApplication = "dfgf",
				Type = 1,
				Duration = 2,
			};

			HolidaysMockData();

			//Act
			var holidayservice = new HolidaysService(uow.Object);
			await holidayservice.CustomValidation(holidays, result);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public async Task CustomValidation_DataSaved_WithOutException()
		{
			//Arrange
			Result<Holidays> result = new();

			Holidays holidays = new()
			{
				ID = Guid.NewGuid(),
				Name = "Dussehra",
				Date = DateTime.Parse("2022-08-29"),
				Description = "dfgf",
				ReprApplication = "dfgf",
				Type = 1,
				Duration = 2,
			};

			HolidaysMockData();
			//Act
			var holidayservice = new HolidaysService(uow.Object);
			await holidayservice.CustomValidation(holidays, result);

			//Assert
			Assert.True(result.HasNoError);
		}
	}
}
