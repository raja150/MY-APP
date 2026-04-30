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
using TranSmart.API.Controllers.Leave;
using TranSmart.API.Controllers.PayRoll;
using TranSmart.API.Extensions;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Domain.Models.PayRoll.List;
using TranSmart.Domain.Models.PayRoll.Request;
using TranSmart.Domain.Models.PayRoll.Response;
using TranSmart.Domain.Models.SelfService;
using TranSmart.Service.Leave;
using TranSmart.Service.Payroll;
using TranSmart.Service.PayRoll;
using Xunit;

namespace Transmart.Controller.UnitTests.PayRoll
{
	public class LateComersControllerTest : ControllerTestBase
	{
		private readonly Mock<ILatecomersService> _latecomersService;
		private readonly LateComersController _controller;
		private readonly Mock<IPayMonthService> _pmService;

		public LateComersControllerTest() : base()
		{
			_latecomersService = new Mock<ILatecomersService>();
			_pmService = new Mock<IPayMonthService>();
			_controller = new LateComersController(Mapper, _latecomersService.Object, _pmService.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};
		}

		[Fact]
		public async Task LateComersController_Paginate_GetValidRecords()
		{
			//Arrange
			var list = new List<Latecomers>
			{
				new Latecomers
				{
					EmployeeID= EmployeeId,
					Month = 12,
					Year = 2022,
				},
				new Latecomers
				{
					EmployeeID= Guid.NewGuid(),
					Month = 12,
					Year = 2022,
				}
			}.AsQueryable();

			_latecomersService.Setup(x => x.GetPaginate(It.IsAny<BaseSearch>())).ReturnsAsync(list.ToPaginate(0, 10));
			//Act
			var response = await _controller.Paginate(new BaseSearch());
			var okResult = response as ObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(1, ((TranSmart.API.Models.Paginate<LatecomersList>)okResult.Value).Items.Count(x => x.EmployeeId == EmployeeId));

		}

		[Fact]
		public async Task LateComersController_Get_Test()
		{
			//Arrange
			var id = Guid.NewGuid();
			var record = new Latecomers
			{
				ID = id,
				EmployeeID = EmployeeId,
				Month = 12,
				Year = 2022,
			};
			_latecomersService.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(record);
			//Act
			var response = await _controller.Get(id);
			var okResult = response as ObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as TranSmart.Domain.Models.PayRoll.Response.LatecomersModel).EmployeeId);
		}

		[Fact]
		public async Task LateComersController_Post_Test()
		{
			//Arrange
			var lateCdata = new LatecomersRequest
			{
				ID= Guid.NewGuid(),
				EmployeeId = EmployeeId,
				PayMonthId = Guid.NewGuid(),
				NumberOfDays=20,
			};
			var result = new TranSmart.Core.Result.Result<Latecomers>();
			var payMonthResult = new TranSmart.Core.Result.Result<PayMonth>();
			_pmService.Setup(x => x.GetPayMonth(It.IsAny<Guid>())).Returns(Task.FromResult(payMonthResult.ReturnValue = new PayMonth { ID = Guid.NewGuid(), Month = 12, Year = 2022 }));
			_latecomersService.Setup(x => x.AddAsync(It.IsAny<Latecomers>())).Callback((Latecomers latecomers) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = latecomers;
			}).ReturnsAsync(result);
			//Act
			var response = await _controller.Post(lateCdata);
			var okResult = response as OkObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, ((LatecomersModel)okResult.Value).EmployeeId);
		}

		[Fact]
		public async Task LateComersController_Post_PayMonthNull()
		{
			//Arrange
			var lateCdata = new LatecomersRequest
			{
				ID = Guid.NewGuid(),
				EmployeeId = EmployeeId,
				PayMonthId = Guid.NewGuid(),
				NumberOfDays = 20,
			};
			var result = new TranSmart.Core.Result.Result<Latecomers>();
			var payMonthResult = new TranSmart.Core.Result.Result<PayMonth>();
			_pmService.Setup(x => x.GetPayMonth(It.IsAny<Guid>())).Returns(Task.FromResult(payMonthResult.ReturnValue = null));
			_latecomersService.Setup(x => x.AddAsync(It.IsAny<Latecomers>())).Callback((Latecomers latecomers) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = latecomers;
			}).ReturnsAsync(result);
			//Act
			var response = await _controller.Post(lateCdata);
			var okResult = response as OkObjectResult;
			//Assert
			Assert.Null(okResult);
		}
		[Fact]
		public async Task LateComersController_Post_BadRequest()
		{
			//Arrange
			var lateCdata = new LatecomersRequest
			{
				ID = Guid.NewGuid(),
				EmployeeId = EmployeeId,
				PayMonthId = Guid.NewGuid(),
				NumberOfDays = 20,
			};
			var result = new TranSmart.Core.Result.Result<Latecomers>();
			var payMonthResult = new TranSmart.Core.Result.Result<PayMonth>();
			_pmService.Setup(x => x.GetPayMonth(It.IsAny<Guid>())).Returns(Task.FromResult(payMonthResult.ReturnValue = new PayMonth()));
			_latecomersService.Setup(x => x.AddAsync(It.IsAny<Latecomers>())).Callback((Latecomers latecomers) =>
			{
				result.IsSuccess = false;
				result.ReturnValue = latecomers;
			}).ReturnsAsync(result);
			//Act
			var response = await _controller.Post(lateCdata);
			var okResult = response as OkObjectResult;
			//Assert
			Assert.Null(okResult);
		}

		[Fact]
		public async Task LateComersController_Put_Test()
		{
			var lateCdata = new LatecomersRequest
			{
				ID = Guid.NewGuid(),
				EmployeeId = EmployeeId,
				PayMonthId = Guid.NewGuid(),
				NumberOfDays = 20,
			};
			var result = new TranSmart.Core.Result.Result<Latecomers>();
			_latecomersService.Setup(x => x.UpdateAsync(It.IsAny<Latecomers>())).Callback((Latecomers latecomers) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = latecomers;
			}).ReturnsAsync(result);
			//Act
			var response = await _controller.Put(lateCdata);
			var okResult = response as OkObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as LatecomersModel).EmployeeId);
		}

		[Fact]
		public async Task LateComersController_Put_BadRequest()
		{
			var lateCdata = new LatecomersRequest
			{
				ID = Guid.NewGuid(),
				EmployeeId = EmployeeId,
				PayMonthId = Guid.NewGuid(),
				NumberOfDays = 20,
			};
			var result = new TranSmart.Core.Result.Result<Latecomers>();
			_latecomersService.Setup(x => x.UpdateAsync(It.IsAny<Latecomers>())).Callback((Latecomers latecomers) =>
			{
				result.IsSuccess = false;
				result.ReturnValue = latecomers;
			}).ReturnsAsync(result);
			//Act
			var response = await _controller.Put(lateCdata);
			var okResult = response as OkObjectResult;
			//Assert
			Assert.Null(okResult);
		}
		[Fact]
		public async Task LateComersController_Search_Test()
		{
			//Arrange
			_latecomersService.Setup(x => x.Search(It.IsAny<string>()));
			//Act
			var response = await _controller.Search(It.IsAny<string>());
			var okResult = response as ObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}
	}
}
