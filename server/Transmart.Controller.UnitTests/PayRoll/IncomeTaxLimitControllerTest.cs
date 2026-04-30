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
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Models.PayRoll.List;
using TranSmart.Domain.Models;
using TranSmart.Service.PayRoll;
using Xunit;

namespace Transmart.Controller.UnitTests.PayRoll
{
	public class IncomeTaxLimitControllerTest : ControllerTestBase
	{
		private readonly Mock<IIncomeTaxLimitService> _service;
		private readonly Mock<TranSmart.Service.Payroll.IPayMonthService> _payMonthService;
		private readonly IncomeTaxLimitController _controller;
		public IncomeTaxLimitControllerTest() : base()
		{
			_service = new Mock<IIncomeTaxLimitService>();
			_payMonthService = new Mock<TranSmart.Service.Payroll.IPayMonthService>();
			_controller = new IncomeTaxLimitController(Mapper, _service.Object, _payMonthService.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};
		}
		[Fact]
		public async Task Post_Test()
		{
			//Arrange
			var incomeTaxLimit = new IncomeTaxLimit
			{
				EmployeeId = EmployeeId,
				Month = 1,
				Year = 2023,
				Amount = 80000
			};
			var result = new TranSmart.Core.Result.Result<IncomeTaxLimit>();
			var payMonthResult = new TranSmart.Core.Result.Result<PayMonth>();
			_payMonthService.Setup(x => x.GetPayMonth(It.IsAny<Guid>())).ReturnsAsync(payMonthResult.ReturnValue = new PayMonth { ID = Guid.NewGuid(), Month = 1, Year = 2023 });
			_service.Setup(x => x.AddAsync(It.IsAny<IncomeTaxLimit>())).Callback((IncomeTaxLimit limit) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = limit;
			}).ReturnsAsync(result);
			//Act
			var response = await _controller.Post(new TranSmart.Domain.Models.PayRoll.Request.IncomeTaxLimitRequest
			{
				PayMonthId = Guid.NewGuid(),
				EmployeeId = EmployeeId,
				Amount = 80000
			});
			var okResult = response as OkObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(80000, (okResult.Value as TranSmart.Domain.Models.PayRoll.Response.IncomeTaxLimitModel).Amount);
		}
		[Fact]
		public async Task Post_Test_PayMonth_Null_Throw_Exception()
		{
			//Arrange
			var incomeTaxLimit = new IncomeTaxLimit
			{
				EmployeeId = EmployeeId,
				Month = 1,
				Year = 2023,
				Amount = 80000
			};
			var result = new TranSmart.Core.Result.Result<IncomeTaxLimit>();
			var payMonthResult = new TranSmart.Core.Result.Result<PayMonth>();
			_payMonthService.Setup(x => x.GetPayMonth(It.IsAny<Guid>())).ReturnsAsync(payMonthResult.ReturnValue = null);
			_service.Setup(x => x.AddAsync(It.IsAny<IncomeTaxLimit>())).Callback(() => { result.IsSuccess = false; }).ReturnsAsync(result);
			//Act
			var response = await _controller.Post(new TranSmart.Domain.Models.PayRoll.Request.IncomeTaxLimitRequest { PayMonthId = Guid.NewGuid() });
			var okResult = response as ObjectResult;
			//
			Assert.NotNull(okResult);
			Assert.Equal(400, okResult.StatusCode);
		}
		[Fact]
		public async Task Post_Test_Throw_BadRequest()
		{
			//Arrange
			var incomeTaxLimit = new IncomeTaxLimit
			{
				EmployeeId = EmployeeId,
				Month = 1,
				Year = 2023,
				Amount = 80000
			};
			var result = new TranSmart.Core.Result.Result<IncomeTaxLimit>();
			var payMonthResult = new TranSmart.Core.Result.Result<PayMonth>();
			_payMonthService.Setup(x => x.GetPayMonth(It.IsAny<Guid>())).ReturnsAsync(payMonthResult.ReturnValue = new PayMonth { ID = Guid.NewGuid() });
			_service.Setup(x => x.AddAsync(It.IsAny<IncomeTaxLimit>())).Callback(() => { result.IsSuccess = false; }).ReturnsAsync(result);
			//Act
			var response = await _controller.Post(new TranSmart.Domain.Models.PayRoll.Request.IncomeTaxLimitRequest { PayMonthId = Guid.NewGuid() });
			var okResult = response as ObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(400, okResult.StatusCode);
		}
		[Fact]
		public async Task Put_Test()
		{
			//Arrange
			var incomTaxLimit = new IncomeTaxLimit { EmployeeId = EmployeeId, Month = 1, Year = 2023, Amount = 68000 };
			var result = new TranSmart.Core.Result.Result<IncomeTaxLimit>();
			var payMonthResult = new TranSmart.Core.Result.Result<PayMonth>();
			_payMonthService.Setup(x => x.GetPayMonth(It.IsAny<Guid>())).ReturnsAsync(payMonthResult.ReturnValue = new PayMonth { ID = Guid.NewGuid() });
			_service.Setup(x => x.UpdateAsync(It.IsAny<IncomeTaxLimit>())).Callback(() =>
			{
				result.IsSuccess = true;
				result.ReturnValue = incomTaxLimit;
			}).ReturnsAsync(result);
			//Act
			var response = await _controller.Put(new TranSmart.Domain.Models.PayRoll.Request.IncomeTaxLimitRequest { PayMonthId = Guid.NewGuid() });
			var okResult = response as ObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(68000, (okResult.Value as TranSmart.Domain.Models.PayRoll.Response.IncomeTaxLimitModel).Amount);
		}
		[Fact]
		public async Task Put_Test_Throws_Exception()
		{
			//Arrange
			var incomTaxLimit = new IncomeTaxLimit { EmployeeId = EmployeeId, Month = 1, Year = 2023, Amount = 68000 };
			var result = new TranSmart.Core.Result.Result<IncomeTaxLimit>();
			var payMonthResult = new TranSmart.Core.Result.Result<PayMonth>();
			_payMonthService.Setup(x => x.GetPayMonth(It.IsAny<Guid>())).ReturnsAsync(payMonthResult.ReturnValue = new PayMonth { ID = Guid.NewGuid() });
			_service.Setup(x => x.UpdateAsync(It.IsAny<IncomeTaxLimit>())).Callback(() =>
			{
				result.IsSuccess = false;
				result.ReturnValue = null;
			}).ReturnsAsync(result);
			//Act
			var response = await _controller.Put(new TranSmart.Domain.Models.PayRoll.Request.IncomeTaxLimitRequest { PayMonthId = Guid.NewGuid() });
			var okResult = response as ObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(400, okResult.StatusCode);
		}
		[Fact]
		public async Task Paginate_Test()
		{
			//Arrange
			var list = new List<IncomeTaxLimit> {
				new IncomeTaxLimit
				{
					EmployeeId = EmployeeId,
					Month= 1,
					Year= 2023,
					Amount = 72000
				},
				new IncomeTaxLimit
				{
					EmployeeId = Guid.NewGuid(),
					Month= 1,
					Year= 2023,
					Amount = 78000
				}}.AsQueryable();
			_service.Setup(x => x.GetPaginate(It.IsAny<BaseSearch>())).ReturnsAsync(list.ToPaginate(0, 10));
			//Act
			var response = await _controller.Paginate(new BaseSearch());
			var okResult = response as OkObjectResult;
			//Assert
			Assert.NotNull(response);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(1, ((TranSmart.API.Models.Paginate<IncomeTaxLimitList>)okResult.Value).Items.Count(x => x.EmployeeId == EmployeeId));
		}
		[Fact]
		public async Task GetById_Test()
		{
			//Arrange
			var incomeTaxLimitId = Guid.NewGuid();
			var record = new IncomeTaxLimit { ID = incomeTaxLimitId, EmployeeId = EmployeeId, Month = 1, Year = 2023, Amount = 80000 };
			_service.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(record);
			//Act
			var response = await _controller.Get(incomeTaxLimitId);
			var okResult = response as OkObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as TranSmart.Domain.Models.PayRoll.Response.IncomeTaxLimitModel).EmployeeId);
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
