using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Organization.Setup;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;
using TranSmart.Service.Organization;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Organization
{
	public class LocationServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly ILocationService _service;
		private readonly Mock<DbContext> _context;
		public LocationServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new LocationService(uow.Object);
			_context = new Mock<DbContext>();
		}


		[Fact]
		public async Task GetAllWeekOffLoc_SpecificReferenceAndWeekOffSetup_ReturnsWeekOffSetupLocationsList()
		{
			// Arrange & Mock
			var id = Guid.Parse("0cfbd7a5-bb2c-486c-b7a8-63f445848d0a");
			var mockToDatabase = new List<Location>
			{
			   new Location
			   {
				   WeekOffSetupId = id
			   },
			   new Location
			   {
				   WeekOffSetupId = id
			   },
			   new Location
			   {
				   WeekOffSetupId = Guid.NewGuid()
			   }
			};
			
			var mockLocation = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockLocation(uow, _context, mockLocation);

			var baseSearch = new BaseSearch()
			{
				RefId = id
			};

			//Assert
			var dd = await _service.GetAllWeekOffLoc(baseSearch);

			//Act
			Assert.Equal(2, dd.Count);
		}



		[Fact]
		public async Task UpdateWeekoff_NewWeekOffSetup_NoException()
		{
			// Arrange & Mock
			var id = Guid.Parse("0cfbd7a5-bb2c-486c-b7a8-63f445848d0a");
			var mockToDatabase = new List<Location>
			{
			   new Location
			   {
				  ID = id
			   }
			};
			
			var mockLocation = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockLocation(uow, _context, mockLocation);

			var item = new LocationAllocationModel()
			{
				LocationId = id,
				WeekOffSetupId = Guid.NewGuid()
			};

			//Assert
			var dd = await _service.UpdateWeekoff(item);

			//Act
			Assert.True(dd.IsSuccess);
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
		}




		[Fact]
		public async Task UpdateWeekoffTest_ExistWeekOffSetup_ThrowException()
		{
			// Arrange & Mock
			var id = Guid.Parse("0cfbd7a5-bb2c-486c-b7a8-63f445848d0a");
            var mockToDatabase = new List<Location>
			{
			   new Location
			   {
				  ID = id,
				  WeekOffSetupId = Guid.NewGuid()
			   }
			};

			var mockLocation = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockLocation(uow, _context, mockLocation);

			var locationAllocationModel = new LocationAllocationModel()
			{
				LocationId = id
			};

			//Assert
			var result = await _service.UpdateWeekoff(locationAllocationModel);

			//Act
			Assert.True( result.HasError);
		}



		[Fact]
		public async Task DeleteWeekOffSetup_SpecificLocation_NoException()
		{
			// Arrange & Mock
			var id = Guid.Parse("5cfbd7a5-bb2c-486c-b7a8-63f445848d0a");
			var mockToDatabase = new List<Location>
			{
			   new Location
			   {
				  ID = id
			   }
			};

			var mockLocation = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockLocation(uow, _context, mockLocation);

			var location = new Location()
			{
				ID = id
			};

			//Assert
			var dd = await _service.DeleteWeekOffSetup(location);

			//Act
			Assert.True(dd.IsSuccess);
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
		}


		[Fact]
		public async Task DeleteWeekOffSetup_NewLocation_ThrowException()
		{
			// Arrange & Mock
			var mockToDatabase = new List<Location>
			{
			   new Location
			   {
				  ID = Guid.NewGuid()
			   }
			};

			var mockLocation = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockLocation(uow, _context, mockLocation);

			var item = new Location()
			{
				ID = Guid.NewGuid()
			};

			//Assert
			var dd = await _service.DeleteWeekOffSetup(item);

			//Act
			Assert.True(dd.HasError);
		}
	}
}
