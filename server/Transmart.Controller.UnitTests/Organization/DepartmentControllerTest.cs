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
	public class DepartmentControllerTest : ControllerTestBase
	{
		private readonly Mock<IDepartmentService> _departmentService;
		private readonly DepartmentController _controller;
		public DepartmentControllerTest() : base()
		{
			_departmentService = new Mock<IDepartmentService>();
			_controller = new DepartmentController(Mapper, _departmentService.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};
		}


		[Fact]
		public async Task GetAllWeekOffDept()
		{
			// Arrange  
			_departmentService.Setup(x => x.GetAllWeekOffDepts(It.IsAny<BaseSearch>()));

			// Act
			var response = await _controller.GetAllWeekOffDept(new BaseSearch());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}



		[Fact]
		public async Task UpdateWeekoffTest()
		{
			// Arrange
			TranSmart.Core.Result.Result<Department> result = new();
			_departmentService.Setup(x => x.UpdateWeekoff(It.IsAny<DepartmentAllocationModel>())).Callback((DepartmentAllocationModel allocation) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new Department { ID = DepartmentId };
			}).ReturnsAsync(result);

			// Act
			var response = await _controller.DeptWeekOffSetup(new DepartmentAllocationModel());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}


		[Fact]
		public async Task DeleteWeekOffSetupTest()
		{
			// Arrange
			TranSmart.Core.Result.Result<Department> result = new();
			_departmentService.Setup(x => x.DeleteWeekOffSetup(It.IsAny<DepartmentAllocationModel>())).Callback((DepartmentAllocationModel allocation) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new Department { ID = DepartmentId };
			}).ReturnsAsync(result);

			// Act
			var response = await _controller.DeleteWeekOffSetup(new DepartmentAllocationModel());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}
	}
}

