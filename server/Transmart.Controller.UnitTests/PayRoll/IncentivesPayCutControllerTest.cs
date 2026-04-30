using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers.PayRoll;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.PayRoll.List;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Controller.UnitTests.PayRoll
{
	public class IncentivesPayCutControllerTest : ControllerTestBase
	{
		private readonly Mock<IIncentivesPayCutService> _service;
		private readonly Mock<IPayMonthService> _payMonthService;
		private readonly IncentivesPayCutController _controller;
		public IncentivesPayCutControllerTest() : base()
		{
			_service = new Mock<IIncentivesPayCutService>();
			_payMonthService = new Mock<IPayMonthService>();
			_controller = new IncentivesPayCutController(Mapper, _service.Object, _payMonthService.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim },
					RouteData = new Microsoft.AspNetCore.Routing.RouteData()
				}
			};
		}
		[Fact]
		public async Task Post_Test()
		{
			//Arrange
			var payCuts = new IncentivesPayCut
			{
				EmployeeId = EmployeeId,
				Month = DateTime.Today.Month,
				Year = DateTime.Today.Year,
			};
			var result = new TranSmart.Core.Result.Result<IncentivesPayCut>();
			var payMonthResult = new TranSmart.Core.Result.Result<PayMonth>();
			_payMonthService.Setup(x => x.GetPayMonth(It.IsAny<Guid>())).Returns(Task.FromResult(payMonthResult.ReturnValue = new PayMonth { ID = Guid.NewGuid(), Month = 12, Year = 2022 }));
			_service.Setup(x => x.AddAsync(It.IsAny<IncentivesPayCut>())).Callback((IncentivesPayCut payCut) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = payCuts;
			}).ReturnsAsync(result);
			//Act
			var response = await _controller.Post(new TranSmart.Domain.Models.PayRoll.Request.IncentivesPayCutRequest
			{
				PayMonthId = Guid.NewGuid(),
				EmployeeId = EmployeeId,
			});
			var okResult = response as OkObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as TranSmart.Domain.Models.PayRoll.Response.IncentivesPayCutModel).EmployeeId);
		}
		[Fact]
		public async Task Post_PayMonth_Null_Test()
		{
			//Arrange
			var payCuts = new IncentivesPayCut
			{
				EmployeeId = EmployeeId,
				Month = DateTime.Today.Month,
				Year = DateTime.Today.Year,
			};
			var result = new TranSmart.Core.Result.Result<IncentivesPayCut>();
			var payMonthResult = new TranSmart.Core.Result.Result<PayMonth>();
			_payMonthService.Setup(x => x.GetPayMonth(It.IsAny<Guid>())).Returns(Task.FromResult(payMonthResult.ReturnValue = null));
			_service.Setup(x => x.AddAsync(It.IsAny<IncentivesPayCut>())).Callback((IncentivesPayCut payCut) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = payCuts;
			}).ReturnsAsync(result);
			//Act
			var response = await _controller.Post(new TranSmart.Domain.Models.PayRoll.Request.IncentivesPayCutRequest
			{
				PayMonthId = Guid.NewGuid(),
				EmployeeId = EmployeeId,
			});
			var okResult = response as ObjectResult;
			//Assert
			Assert.Equal(400, okResult.StatusCode);
		}
		[Fact]
		public async Task Post_Test_throw_Exception()
		{
			//Arrange
			var payCuts = new IncentivesPayCut
			{
				EmployeeId = EmployeeId,
				Month = DateTime.Today.Month,
				Year = DateTime.Today.Year,
			};
			var result = new TranSmart.Core.Result.Result<IncentivesPayCut>();
			var payMonthResult = new TranSmart.Core.Result.Result<PayMonth>();
			_payMonthService.Setup(x => x.GetPayMonth(It.IsAny<Guid>())).Returns(Task.FromResult(payMonthResult.ReturnValue = new PayMonth { ID = Guid.NewGuid(), Month = 12, Year = 2022 }));
			_service.Setup(x => x.AddAsync(It.IsAny<IncentivesPayCut>())).Callback((IncentivesPayCut payCut) =>
			{
				result.IsSuccess = false;
				result.ReturnValue = payCuts;
			}).ReturnsAsync(result);
			//Act
			var response = await _controller.Post(new TranSmart.Domain.Models.PayRoll.Request.IncentivesPayCutRequest
			{
				PayMonthId = Guid.NewGuid(),
				EmployeeId = EmployeeId,
			});
			var okResult = response as ObjectResult;
			//Assert
			Assert.Equal(400, okResult.StatusCode);
		}

		[Fact]
		public async Task Put_Test()
		{
			var payCuts = new IncentivesPayCut
			{
				EmployeeId = EmployeeId,
				Month = DateTime.Today.Month,
				Year = DateTime.Today.Year,
			};
			var result = new TranSmart.Core.Result.Result<IncentivesPayCut>();
			var payMonthResult = new TranSmart.Core.Result.Result<PayMonth>();
			_service.Setup(x => x.UpdateAsync(It.IsAny<IncentivesPayCut>())).Callback((IncentivesPayCut payCut) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = payCuts;
			}).ReturnsAsync(result);
			//Act
			var response = await _controller.Put(new TranSmart.Domain.Models.PayRoll.Request.IncentivesPayCutRequest
			{
				PayMonthId = Guid.NewGuid(),
				EmployeeId = EmployeeId,
			});
			var okResult = response as OkObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as TranSmart.Domain.Models.PayRoll.Response.IncentivesPayCutModel).EmployeeId);
		}
		[Fact]
		public async Task Put_Test_Trow_Exception()
		{
			var payCuts = new IncentivesPayCut
			{
				EmployeeId = EmployeeId,
				Month = DateTime.Today.Month,
				Year = DateTime.Today.Year,
			};
			var result = new TranSmart.Core.Result.Result<IncentivesPayCut>();
			var payMonthResult = new TranSmart.Core.Result.Result<PayMonth>();
			_service.Setup(x => x.UpdateAsync(It.IsAny<IncentivesPayCut>())).Callback((IncentivesPayCut payCut) =>
			{
				result.IsSuccess = false;
				result.ReturnValue = payCuts;
			}).ReturnsAsync(result);
			//Act
			var response = await _controller.Put(new TranSmart.Domain.Models.PayRoll.Request.IncentivesPayCutRequest
			{
				PayMonthId = Guid.NewGuid(),
				EmployeeId = EmployeeId,
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
			var list = new List<IncentivesPayCut> { new IncentivesPayCut
			{
				EmployeeId= EmployeeId,
				Month = 12,
				Year = 2022,
			},
			 new IncentivesPayCut
			{
				EmployeeId= Guid.NewGuid(),
				Month = 12,
				Year = 2022,
			}}.AsQueryable();
			_service.Setup(x => x.GetPaginate(It.IsAny<BaseSearch>())).ReturnsAsync(list.ToPaginate(0, 10));
			//Act
			var response = await _controller.Paginate(new BaseSearch());
			var okResult = response as ObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
				
		}
		[Fact]
		public async Task Get_Test()
		{
			//Arrange
			var paycutId = Guid.NewGuid();
			var record = new IncentivesPayCut
			{
				ID = paycutId,
				EmployeeId = EmployeeId,
				Month = 12,
				Year = 2022,
			};
			_service.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(record);
			//Act
			var response = await _controller.Get(paycutId);
			var okResult = response as ObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as TranSmart.Domain.Models.PayRoll.Response.IncentivesPayCutModel).EmployeeId);
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
