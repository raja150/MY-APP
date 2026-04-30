using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers;
using TranSmart.API.Services;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models.Cache;
using TranSmart.Domain.Models.Leave.Approval;
using TranSmart.Service;
using Xunit;

namespace Transmart.Controller.UnitTests
{
	public class SearchControllerTest : ControllerTestBase
	{
		private Mock<ISearchService> _searchService;
		private Mock<ICacheService> _cacheService;

		private readonly SearchController _controller;
		public SearchControllerTest() : base()
		{
			_searchService = new Mock<ISearchService>();
			_cacheService = new Mock<ICacheService>();
			_controller = new SearchController(Mapper, _searchService.Object, _cacheService.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};
		}


		[Fact]
		public void SearchAccountTest()
		{
			// Arrange
			_searchService.Setup(x => x.GetPayMonths(It.IsAny<string>()));

			// Act
			var response = _controller.SearchAccount(It.IsAny<string>());
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}


		[Fact]
		public async Task SearchEmployeeAccountTest()
		{
			// Arrange
			_searchService.Setup(x => x.GetEmployee(It.IsAny<string>()));

			// Act
			var response = await _controller.SearchEmployeeAccount(It.IsAny<string>());
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}


		[Fact]
		public async void SearchByEmpNameTest()
		{
			_ = _searchService.Setup(x => x.GetEmployeeDetails(It.IsAny<string>()));

			// Act
			var response = await _controller.SearchByEmpName(It.IsAny<string>());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}


		[Fact]
		public async Task GetSearchedEmployeesTest()
		{
			// Arrange
			_cacheService.Setup(x => x.SearchedEmployee(It.IsAny<string>(),It.IsAny<Guid>()));

			// Act
			var response = await _controller.GetSearchedEmployees(It.IsAny<string>());
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}


		[Fact]
		public async Task GetDesignationsTest()
		{
			// Arrange
			_searchService.Setup(x => x.GetDesignations(It.IsAny<Guid>()));

			// Act
			var response = await _controller.GetDesignations(It.IsAny<string>(), It.IsAny<Guid>());
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}

		[Fact]
		public async Task GetTeamEmployeesTest()
		{
			// Arrange
			var list = new List<EmployeeSearchCache>
			{
				new EmployeeSearchCache
				{
					Name = "Vamshi",
					No = "AVONTIX2063"
				}
			};
			_cacheService.Setup(x => x.GetTeamEmployeeBySearch(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(list);

			// Act
			var response = await _controller.GetTeamEmployees("Vamshi");
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal("Vamshi", ((List<EmployeeSearchCache>)okResult.Value)[0].Name);
		}
		[Fact]
		public async Task GetTeamEmployees_BadRequest()
		{
			// Arrange
			var list = new List<EmployeeSearchCache>();

			_cacheService.Setup(x => x.GetTeamEmployeeBySearch(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(list = null);

			// Act
			var response = await _controller.GetTeamEmployees("Vamshi");
			var okResult = response as OkObjectResult;

			//assert
			Assert.Null(okResult);
		}
	}
}
