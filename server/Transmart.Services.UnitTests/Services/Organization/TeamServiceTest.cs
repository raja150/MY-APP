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
	public class TeamServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly ITeamService _service;
		private readonly Mock<DbContext> _context;
		public TeamServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new TeamService(uow.Object);
			_context = new Mock<DbContext>();
		}


		[Fact]
		public async Task GetAllWeekOffTeam_SpecificReferenceAndWeekOffSetup_ReturnAllWeekOffTeams()
		{
			// Arrange & Mock
			var id = Guid.Parse("0cfbd7a5-bb2c-486c-b7a8-63f445848d0a");
			var  mockToDatabase = new List<Team>
			{
			    new Team
			    {
					WeekOffSetupId = id
			    },
				new Team
			    {
					WeekOffSetupId = id
			    },
				new Team
			    {
					WeekOffSetupId = Guid.NewGuid()
			    }
			};
			
			var mockTeam = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockTeam(uow, _context, mockTeam);

			var baseSearch = new BaseSearch()
			{
				RefId = id
			};

			//Assert
			var dd = await _service.GetAllWeekOffTeam(baseSearch);

			//Act
			Assert.Equal(2, dd.Count);
		}



		[Fact]
		public async Task UpdateWeekoff_SpecificTeam_NoException()
		{
			// Arrange & Mock
			var id = Guid.Parse("7cfbd7a5-bb2c-486c-b7a8-63f445848d0a");
			var mockToDatabase = new List<Team>
			{
			   new Team
			   {
				  ID = id
			   }
			};
			var mockLocation = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockTeam(uow, _context, mockLocation);

			var item = new TeamAllocationModel()
			{
				TeamId = id
			};

			//Assert
			var dd = await _service.UpdateWeekOff(item);

			//Act
			Assert.True(dd.IsSuccess);
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
		}



		[Fact]
		public async Task UpdateWeekoff_ExistweekOffSetup_ThrowException()
		{
			// Arrange & Mock
			var id = Guid.Parse("7cfbd7a5-bb2c-486c-b7a8-63f445848d0a");
			var mockToDatabase = new List<Team>
			{
			   new Team
			   {
				  ID = id,
				  WeekOffSetupId = Guid.NewGuid()
			   }
			};
			var mockLocation = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockTeam(uow, _context, mockLocation);

			var item = new TeamAllocationModel()
			{
				TeamId = id
			};

			//Assert
			var dd = await _service.UpdateWeekOff(item);

			//Act
			Assert.True(dd.HasError);
		}



		[Fact]
		public async Task DeleteWeekOffSetup_ExistWeekOffSetup_NoException()
		{
			// Arrange & Mock
			var id = Guid.Parse("7cfbd7a5-bb2c-486c-b7a8-63f445848d0a");
			var mockToDatabase = new List<Team>
			{
			   new Team
			   {
				 ID = id,
				 WeekOffSetupId = Guid.NewGuid()
			   }
			};
			
			var mockTeam = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockTeam(uow, _context, mockTeam);

			var team = new Team()
			{
				ID = id
			};

			//Assert
			var dd = await _service.DeleteWeekOffSetup(team);

			//Act
			Assert.True(dd.IsSuccess);
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
		}



		[Fact]
		public async Task DeleteWeekOffSetup_WeekOffSetupNotExist_ThrowException()
		{
			// Arrange & Mock
			var id = Guid.Parse("7cfbd7a5-bb2c-486c-b7a8-63f445848d0a");
			var mockToDatabase = new List<Team>
			{
			   new Team
			   {
				  ID = id
			   }
			};

			var mockTeam = mockToDatabase.AsQueryable().BuildMockDbSet();
			SetData.MockTeam(uow, _context, mockTeam);

			var team = new Team()
			{
				ID = id
			};

			//Assert
			var dd = await _service.DeleteWeekOffSetup(team);

			//Act
			Assert.True(dd.HasError);
		}
	}
}
