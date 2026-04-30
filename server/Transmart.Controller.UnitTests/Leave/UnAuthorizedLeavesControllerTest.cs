using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Leave;
using TranSmart.API.Extensions;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave.Model;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Domain.Models.SelfService;
using TranSmart.Service.Leave;
using Xunit;

namespace Transmart.Controller.UnitTests.Leave
{
	public class UnAuthorizedLeavesControllerTest : ControllerTestBase
	{
		private Mock<IUnAuthorizedLeavesService> _unAuthorizedLeavesService;
		private readonly UnAuthorizedLeavesController _controller;

		public UnAuthorizedLeavesControllerTest() : base()
		{
			_unAuthorizedLeavesService = new Mock<IUnAuthorizedLeavesService>();
			_controller = new UnAuthorizedLeavesController(Mapper, _unAuthorizedLeavesService.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};
		}

		[Theory]
		[InlineData(1)]
		public async Task UnAuthorizedLeaves_Paginate_Test(int count)
		{
			// Arrange  
			var list = new List<UnAuthorizedLeaves>
			{
				new UnAuthorizedLeaves
				{
					EmployeeId = EmployeeId
				},
				new UnAuthorizedLeaves
				{
					EmployeeId = Guid.NewGuid()
				}
			}.AsQueryable();

			_unAuthorizedLeavesService.Setup(x => x.GetPaginate(It.IsAny<BaseSearch>())).ReturnsAsync(list.ToPaginate(0, 10));

			var controller = _controller;

			// Act
			var response = await controller.Paginate(new UnAuthorizedLeavesSearch());
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(count, ((TranSmart.API.Models.Paginate<UnAuthorizedLeavesModel>)okResult.Value).Items.Where(x => x.EmployeeId == EmployeeId).Count());

		}

		[Fact]
		public async Task UnAuthorizedLeaves_Get_GetValidRecords()
		{
			// Arrange  
			var id = Guid.NewGuid();

			Result<UnAuthorizedLeaves> result = new();
			_unAuthorizedLeavesService.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(result.ReturnValue = new UnAuthorizedLeaves { ID = id, EmployeeId = EmployeeId }));

			var controller = _controller;

			// Act
			var response = await controller.Get(id);
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, ((UnAuthorizedLeavesModel)okResult.Value).EmployeeId);


		}


		[Fact]
		public async Task UnAuthorizedLeaves_Post_Test()
		{
			// Arrange  
			Result<UnAuthorizedLeaves> result = new();
			_unAuthorizedLeavesService.Setup(x => x.AddAsync(It.IsAny<UnAuthorizedLeaves>())).Callback((UnAuthorizedLeaves unAuthorizedLeaves) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = unAuthorizedLeaves;

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var response = await controller.Post(new UnAuthorizedLeavesModel { EmployeeId = EmployeeId });
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as UnAuthorizedLeavesModel).EmployeeId);
		}

		[Fact]
		public async Task UnAuthorizedLeaves_Post_BadRequest()
		{
			// Arrange  
			Result<UnAuthorizedLeaves> result = new();
			_unAuthorizedLeavesService.Setup(x => x.AddAsync(It.IsAny<UnAuthorizedLeaves>())).Callback((UnAuthorizedLeaves unAuthorizedLeaves) =>
			{
				result.IsSuccess = false;
				result.ReturnValue = unAuthorizedLeaves;

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var response = await controller.Post(new UnAuthorizedLeavesModel { EmployeeId = EmployeeId });
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Null(okResult);
		}

		[Fact]
		public async Task UnAuthorizedLeaves_Put_Test()
		{
			// Arrange  
			Result<UnAuthorizedLeaves> result = new();
			_unAuthorizedLeavesService.Setup(x => x.UpdateAsync(It.IsAny<UnAuthorizedLeaves>())).Callback((UnAuthorizedLeaves unAuthorizedLeaves) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = unAuthorizedLeaves;

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var response = await controller.Put(new UnAuthorizedLeavesModel { EmployeeId = EmployeeId});
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as UnAuthorizedLeavesModel).EmployeeId);
		}

		[Fact]
		public async Task UnAuthorizedLeaves_Put_BadRequest()
		{
			// Arrange  
			Result<UnAuthorizedLeaves> result = new();
			_unAuthorizedLeavesService.Setup(x => x.UpdateAsync(It.IsAny<UnAuthorizedLeaves>())).Callback((UnAuthorizedLeaves unAuthorizedLeaves) =>
			{
				result.IsSuccess = false;
				result.ReturnValue = unAuthorizedLeaves;

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var response = await controller.Put(new UnAuthorizedLeavesModel { EmployeeId = EmployeeId });
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Null(okResult);
		}


	}
}
