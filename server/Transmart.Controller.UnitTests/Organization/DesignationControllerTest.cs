using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Organization;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;
using TranSmart.Service.Organization;
using Xunit;

namespace Transmart.Controller.UnitTests.Organization
{
	public class DesignationControllerTest : ControllerTestBase
	{
		private readonly Mock<IDesignationService> _designationService;
		private readonly DesignationController _controller;
		public DesignationControllerTest() : base()
		{
			_designationService = new Mock<IDesignationService>();
			_controller = new DesignationController(Mapper, _designationService.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};
		}


		[Fact]
		public async Task GetDepListsTest()
		{
			// Arrange  
			_designationService.Setup(x => x.GetAllWeekOffDesign(It.IsAny<BaseSearch>()));

			// Act
			var response = await _controller.GetAllWeekOffDesignations(new BaseSearch());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}



		[Fact]
		public async Task DesigWeekOffSetupTest()
		{
			// Arrange
			TranSmart.Core.Result.Result<Designation> result = new();
			_designationService.Setup(x => x.UpdateWeekoff(It.IsAny<DesignationAllocationModel>())).Callback((DesignationAllocationModel allocation) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new Designation { ID = DesignationId };
			}).ReturnsAsync(result);

			// Act
			var response = await _controller.DesigWeekOffSetup(new DesignationAllocationModel());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}




		[Fact]
		public async Task DeleteWeekOffSetupTest()
		{
			// Arrange
			TranSmart.Core.Result.Result<Designation> result = new();
			_designationService.Setup(x => x.DeleteWeekOffSetup(It.IsAny<Designation>())).Callback((Designation allocation) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new Designation { ID = DesignationId };
			}).ReturnsAsync(result);

			// Act
			var response = await _controller.DeleteWeekOffSetup(new DesignationModel());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}
	}
}

