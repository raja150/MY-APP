using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Organization;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;
using TranSmart.Service.Organization;
using Xunit;

namespace Transmart.Controller.UnitTests.Organization
{
	public class TeamControllerTest : ControllerTestBase
	{
		private readonly Mock<ITeamService> _service;
		private readonly TeamController _controller;
		public TeamControllerTest() : base()
		{
			_service = new Mock<ITeamService>();
			_controller = new TeamController(Mapper, _service.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};
		}


		[Fact]
		public async Task GetAllWeekOffLocationsTest()
		{
			// Arrange  
			_service.Setup(x => x.GetAllWeekOffTeam(It.IsAny<BaseSearch>()));

			// Act
			var response = await _controller.GetAllWeekOffTeams(new BaseSearch());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}



		[Fact]
		public async Task TeamWeekOffSetupTest()
		{
			// Arrange  
			var result = new Result<Team>();
			_service.Setup(x => x.UpdateWeekOff(It.IsAny<TeamAllocationModel>())).Callback(() =>
			{
				result.IsSuccess = false;
				result.ReturnValue = null;
			}).ReturnsAsync(result);

			// Act
			var response = await _controller.TeamWeekOffSetup(new TeamAllocationModel());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}

		
				[Fact]
		public async Task DeleteWeekOffSetupTest()
		{
			// Arrange  
			var result = new Result<Team>();
			_service.Setup(x => x.DeleteWeekOffSetup(It.IsAny<Team>())).Callback(() =>
			{
				result.IsSuccess = false;
				result.ReturnValue = null;
			}).ReturnsAsync(result);

			// Act
			var response = await _controller.DeleteWeekOffSetup(new TeamModel());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}
	}
}

