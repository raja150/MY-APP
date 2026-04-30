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
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;
using TranSmart.Service.Organization;
using Xunit;

namespace Transmart.Controller.UnitTests.Organization
{
	public class LocationControllerTest : ControllerTestBase
	{
		private readonly Mock<ILocationService> _service;
		private readonly LocationController _controller;
		public LocationControllerTest() : base()
		{
			_service = new Mock<ILocationService>();
			_controller = new LocationController(Mapper, _service.Object)
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
			_service.Setup(x => x.GetAllWeekOffLoc(It.IsAny<BaseSearch>()));

			// Act
			var response = await _controller.GetAllWeekOffLocations(new BaseSearch());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}



		[Fact]
		public async Task GetAllWeekOffEmployeesTest()
		{
			// Arrange  
			var result = new Result<Location>();
			_service.Setup(x => x.UpdateWeekoff(It.IsAny<LocationAllocationModel>())).Callback(() =>
			 {
				 result.IsSuccess = false;
				 result.ReturnValue = null;
			 }).ReturnsAsync(result);

			// Act
			var response = await _controller.LocWeekOffSetup(new LocationAllocationModel());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}


		[Fact]
		public async Task DeleteWeekOffSetupTest()
		{
			// Arrange  
			var result = new Result<Location>();
			_service.Setup(x => x.DeleteWeekOffSetup(It.IsAny<Location>())).Callback(() =>
			{
				result.IsSuccess = false;
				result.ReturnValue = null;
			}).ReturnsAsync(result);

			// Act
			var response = await _controller.DeleteWeekOffSetup(new LocationModel());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}
	}
}

