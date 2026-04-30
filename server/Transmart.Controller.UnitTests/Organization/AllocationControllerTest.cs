using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Organization;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;
using TranSmart.Service.Organization;
using Xunit;

namespace Transmart.Controller.UnitTests.Organization
{
	public class AllocationControllerTest : ControllerTestBase
	{
		private readonly Mock<IAllocationService> _allocationService;
		private readonly AllocationController _controller;
		public AllocationControllerTest() : base()
		{
			_allocationService = new Mock<IAllocationService>();
			_controller = new AllocationController(Mapper, _allocationService.Object)
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
			_allocationService.Setup(x => x.GetEmployee(It.IsAny<Guid>()));

			// Act
			var response = await _controller.GetEmp(It.IsAny<Guid>());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}


		[Fact]
		public async Task GetAllWeekOffEmployeesTest()
		{
			// Arrange  
			var list = new List<Allocation>
			{
				new Allocation
				{
					EmployeeId = EmployeeId
				},
				new Allocation
				{
					EmployeeId = Guid.NewGuid()
				}
			}.AsQueryable();

			_allocationService.Setup(x => x.GetAllWeekOffEmployees(It.IsAny<BaseSearch>())).ReturnsAsync(list.ToPaginate(0, 10));


			// Act
			var response = await _controller.GetAllWeekOffEmployees(new BaseSearch());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode); 
		}



		[Fact]
		public async Task EmpWeekOffSetupTest()
		{
			// Arrange
			TranSmart.Core.Result.Result<Allocation> result = new();
			_allocationService.Setup(x => x.UpdateAllocation(It.IsAny<Allocation>())).Callback((Allocation allocation) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new Allocation { ID = EmployeeId };
			}).ReturnsAsync(result);

			// Act
			var response = await _controller.EmpWeekOffSetup(new AllocationModel());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}



		[Fact]
		public async Task DeleteWeekOffSetupTest()
		{
			// Arrange
			TranSmart.Core.Result.Result<Allocation> result = new();
			_allocationService.Setup(x => x.DeleteWeekOffSetup(It.IsAny<Allocation>())).Callback((Allocation allocation) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new Allocation { ID = EmployeeId };
			}).ReturnsAsync(result);

			// Act
			var response = await _controller.DeleteWeekOffSetup(new AllocationModel());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}
	}
}
