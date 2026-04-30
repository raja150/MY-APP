using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Payroll;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.PayRoll.List;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Controller.UnitTests.PayRoll
{
	public class ArrearsControllerTest : ControllerTestBase
	{
		private readonly Mock<IArrearService> _service;
		private readonly Mock<IPayMonthService> _payMonthService;
		private readonly ArrearsController _controller;
		public ArrearsControllerTest() : base()
		{
			_service = new Mock<IArrearService>();
			_payMonthService = new Mock<IPayMonthService>();
			_controller = new ArrearsController(Mapper, _service.Object, _payMonthService.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim },
					RouteData = new Microsoft.AspNetCore.Routing.RouteData()
				}
			};
		}
		[Fact]
		public async Task Post_Arrears_Test()
		{
			//Arrange
			var arrears = new Arrear
			{
				EmployeeID = EmployeeId,
				Month = 12,
				Year = 2022,
				Pay = 33000
			};
			var result = new TranSmart.Core.Result.Result<Arrear>();
			var payMonthResult = new TranSmart.Core.Result.Result<PayMonth>();
			_payMonthService.Setup(x => x.GetPayMonth(It.IsAny<Guid>())).ReturnsAsync(payMonthResult.ReturnValue = new PayMonth { ID = Guid.NewGuid(), Month = 12, Year = 2022 });
			_service.Setup(x => x.AddAsync(It.IsAny<Arrear>())).Callback((Arrear arrear) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = arrears;
			}).ReturnsAsync(result);
			//Act
			var response = await _controller.Post(new TranSmart.Domain.Models.PayRoll.Request.ArrearsRequest
			{
				PayMonthId = Guid.NewGuid(),
				EmployeeId = EmployeeId,
				Pay = 200
			});
			var okResult = response as OkObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(33000, (okResult.Value as TranSmart.Domain.Models.PayRoll.Response.ArrearsModel).Pay);
		}
		[Fact]
		public async Task Post_PayMonth_Null_Throw_Exception_Test()
		{
			//Arrange
			var arrears = new Arrear
			{
				EmployeeID = EmployeeId,
				Month = 12,
				Year = 2022,
				Pay = 33000
			};
			var result = new TranSmart.Core.Result.Result<Arrear>();
			var payMonthResult = new TranSmart.Core.Result.Result<PayMonth>();
			_payMonthService.Setup(x => x.GetPayMonth(It.IsAny<Guid>())).Returns(Task.FromResult(payMonthResult.ReturnValue = null));
			_service.Setup(x => x.AddAsync(It.IsAny<Arrear>())).Callback((Arrear payCut) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = arrears;
			}).ReturnsAsync(result);
			//Act
			var response = await _controller.Post(new TranSmart.Domain.Models.PayRoll.Request.ArrearsRequest
			{
				PayMonthId = Guid.NewGuid(),
				EmployeeId = EmployeeId,
				Pay = 1200
			});
			var okResult = response as ObjectResult;
			//Assert
			Assert.Equal(400, okResult.StatusCode);
		}
		[Fact]
		public async Task Post_Arrears_Test_Throw_Exception()
		{
			//Arrange
			var arrears = new Arrear
			{
				EmployeeID = EmployeeId,
				Month = 12,
				Year = 2022,
				Pay = 33000
			};
			var result = new TranSmart.Core.Result.Result<Arrear>();
			var payMonthResult = new TranSmart.Core.Result.Result<PayMonth>();
			_payMonthService.Setup(x => x.GetPayMonth(It.IsAny<Guid>())).ReturnsAsync(payMonthResult.ReturnValue = new PayMonth { ID = Guid.NewGuid(), Month = 12, Year = 2022 });
			_service.Setup(x => x.AddAsync(It.IsAny<Arrear>())).Callback((Arrear arrear) =>
			{
				result.IsSuccess = false;
				result.ReturnValue = arrears;
			}).ReturnsAsync(result);
			//Act
			var response = await _controller.Post(new TranSmart.Domain.Models.PayRoll.Request.ArrearsRequest
			{
				PayMonthId = Guid.NewGuid(),
				EmployeeId = EmployeeId,
				Pay = 200
			});
			var okResult = response as ObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(400, okResult.StatusCode);
		}
		[Fact]
		public async Task Put_Test()
		{
			//Arrange
			var arrears = new Arrear
			{
				EmployeeID = EmployeeId,
				Month = 12,
				Year = 2022,
				Pay = 62000
			};
			var result = new TranSmart.Core.Result.Result<Arrear>();
			var payMonthResult = new TranSmart.Core.Result.Result<PayMonth>();
			_service.Setup(x => x.UpdateAsync(It.IsAny<Arrear>())).Callback((Arrear arrear) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = arrears;
			}).ReturnsAsync(result);
			//Act
			var response = await _controller.Put(new TranSmart.Domain.Models.PayRoll.Request.ArrearsRequest
			{
				EmployeeId = EmployeeId,
				Pay = 64000
			});
			var okResult = response as OkObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as TranSmart.Domain.Models.PayRoll.Response.ArrearsModel).EmployeeId);
			Assert.Equal(62000, (okResult.Value as TranSmart.Domain.Models.PayRoll.Response.ArrearsModel).Pay);
		}
		[Fact]
		public async Task Put_Test_Throw_Exception()
		{
			//Arrange
			var arrears = new Arrear
			{
				EmployeeID = EmployeeId,
				Month = 12,
				Year = 2022,
				Pay = 62000
			};
			var result = new TranSmart.Core.Result.Result<Arrear>();
			var payMonthResult = new TranSmart.Core.Result.Result<PayMonth>();
			_service.Setup(x => x.UpdateAsync(It.IsAny<Arrear>())).Callback((Arrear arrear) =>
			{
				result.IsSuccess = false;
				result.ReturnValue = arrears;
			}).ReturnsAsync(result);
			//Act
			var response = await _controller.Put(new TranSmart.Domain.Models.PayRoll.Request.ArrearsRequest
			{
				EmployeeId = EmployeeId,
				Pay = 64000
			});
			var okResult = response as ObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(400, okResult.StatusCode);
		}
		[Fact]
		public async Task Paginate_Test()
		{
			//Arrange
			var list = new List<Arrear> {
				new Arrear
				{
					EmployeeID = EmployeeId,
					Month= 1,
					Year= 2023,
					Pay = 72000
				},
				new Arrear
				{
					EmployeeID = Guid.NewGuid(),
					Month= 1,
					Year= 2023,
					Pay = 78000
				}}.AsQueryable();
			_service.Setup(x => x.GetPaginate(It.IsAny<BaseSearch>())).ReturnsAsync(list.ToPaginate(0, 10));
			//Act
			var response = await _controller.Paginate(new BaseSearch());
			var okResult = response as OkObjectResult;
			//Assert
			Assert.NotNull(response);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(1, ((TranSmart.API.Models.Paginate<ArrearsList>)okResult.Value).Items.Count(x => x.EmployeeId == EmployeeId));
		}
		[Fact]
		public async Task GetById_Test()	
		{
			//Arrange
			var arrearId= Guid.NewGuid();
			var record = new Arrear { ID = arrearId, EmployeeID = EmployeeId, Month = 1, Year = 2023, Pay = 80000 };
			_service.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(record);
			//Act
			var response = await _controller.Get(arrearId);
			var okResult = response as OkObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200 , okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as TranSmart.Domain.Models.PayRoll.Response.ArrearsModel).EmployeeId);
		}
		[Fact]
		public async Task Search_Test()
		{
			//Arrange
			_service.Setup(x => x.Search(It.IsAny<string>()));
			//Act
			var response = await _controller.Search(It.IsAny<string>());
			var okResult = response as ObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}
	}
}
