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
using TranSmart.API.Controllers.PayRoll;
using TranSmart.API.Extensions;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave.Request;
using TranSmart.Domain.Models.Organization;
using TranSmart.Domain.Models.PayRoll.List;
using TranSmart.Domain.Models.PayRoll.Response;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Controller.UnitTests.PayRoll
{
	public class EmployeePayInfoControllerTest : ControllerTestBase
	{
		private readonly Mock<IEmployeePayInfoService> _service;

		private readonly EmployeePayInfoController _controller;
		public EmployeePayInfoControllerTest() : base()
		{
			_service = new Mock<IEmployeePayInfoService>();
			_controller = new EmployeePayInfoController(Mapper, _service.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim },
				}
			};
		}
		[Fact]
		public async Task PayInfo_GetList_Test()
		{
			//Arrange
			var list = new List<EmployeePayInfo> {
				new EmployeePayInfo
				{
					ID = EmployeeId,
					PayMode = 1
				},
				new EmployeePayInfo
				{
					ID = Guid.NewGuid(),
					PayMode = 2

				}}.AsQueryable();
			_service.Setup(x => x.GetList(It.IsAny<string>())).ReturnsAsync(list);

			//Act
			var response = await _controller.GetList(It.IsAny<string>());
			var okResult = response as OkObjectResult;

			//Assert
			Assert.NotNull(response);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, ((List<EmployeePayInfoList>)okResult.Value)[0].ID);
		}
		[Fact]
		public async Task PayInfo_Paginate_Test()
		{
			//Arrange
			var list = new List<EmployeePayInfo> {
				new EmployeePayInfo
				{
					ID = EmployeeId,
					PayMode = 1
				},
				new EmployeePayInfo
				{
					ID = Guid.NewGuid(),
					PayMode = 2

				}}.AsQueryable();
			_service.Setup(x => x.GetPaginate(It.IsAny<BaseSearch>())).ReturnsAsync(list.ToPaginate(0, 10));
			//Act
			var attributes = _controller.GetType().GetMethod("Paginate").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await _controller.Paginate(new BaseSearch());
			var okResult = response as OkObjectResult;
			//Assert
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.PS_EmployeePayInfo);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

			Assert.NotNull(response);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(1, ((TranSmart.API.Models.Paginate<EmployeePayInfoList>)okResult.Value).Items.Count(x => x.ID == EmployeeId));
		}

		[Fact]
		public async Task PayInfo_GetById_Test()
		{
			//Arrange
			var payInfoId = Guid.NewGuid();
			var record = new EmployeePayInfo { ID = payInfoId, EmployeeId = EmployeeId, PayMode = 1 };
			_service.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(record);
			//Act
			var attributes = _controller.GetType().GetMethod("Get").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);

			var response = await _controller.Get(payInfoId);
			var okResult = response as OkObjectResult;
			//Assert
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.PS_EmployeePayInfo);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as EmployeePayInfoModel).EmployeeId);
		}

		[Fact]
		public async Task PayInfo_Post_Test()
		{
			// Arrange  
			var result = new Result<EmployeePayInfo>();
			_service.Setup(x => x.AddAsync(It.IsAny<EmployeePayInfo>())).Callback(() =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new EmployeePayInfo { EmployeeId = EmployeeId };
			}).ReturnsAsync(result);
			
			// Act
			var attributes = _controller.GetType().GetMethod("Post").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var resposne = await _controller.Post(new EmployeePayInfoModel());
			var okResult = resposne as OkObjectResult;

			// Assert
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.PS_EmployeePayInfo);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Create);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as EmployeePayInfoModel).EmployeeId);
		}

		[Fact]
		public async Task PayInfo_Post_Throw_Exception()
		{
			// Arrange  
			var result = new Result<EmployeePayInfo>();
			_service.Setup(x => x.AddAsync(It.IsAny<EmployeePayInfo>())).Callback(() =>
			{
				result.IsSuccess = false;
				result.ReturnValue = new EmployeePayInfo { EmployeeId = EmployeeId };
			}).ReturnsAsync(result);

			// Act
			var attributes = _controller.GetType().GetMethod("Post").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var resposne = await _controller.Post(new EmployeePayInfoModel());
			var okResult = resposne as OkObjectResult;

			// Assert
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.PS_EmployeePayInfo);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Create);

			Assert.Null(okResult);
		}

		[Fact]
		public async Task PayInfo_Put_Test()
		{
			//Arrange
			var result = new Result<EmployeePayInfo>();
			_service.Setup(x => x.UpdateAsync(It.IsAny<EmployeePayInfo>())).Callback(() =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new EmployeePayInfo { EmployeeId = EmployeeId, PayMode = 1 };
			}).ReturnsAsync(result);

			//Act
			var attributes = _controller.GetType().GetMethod("Put").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await _controller.Put(new EmployeePayInfoModel
			{
				EmployeeId = EmployeeId,
				PayMode = 2
			});
			var okResult = response as OkObjectResult;
			//Assert
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.PS_EmployeePayInfo);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Update);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as EmployeePayInfoModel).EmployeeId);
			
		}
		[Fact]
		public async Task PayInfo_Put_Throw_Exception()
		{
			var result = new Result<EmployeePayInfo>();
			_service.Setup(x => x.UpdateAsync(It.IsAny<EmployeePayInfo>())).Callback(() =>
			{
				result.IsSuccess = false;
				result.ReturnValue = new EmployeePayInfo { EmployeeId = EmployeeId, PayMode = 1 };
			}).ReturnsAsync(result);

			//Act
			var attributes = _controller.GetType().GetMethod("Put").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);

			var response = await _controller.Put(new EmployeePayInfoModel
			{
				EmployeeId = EmployeeId,
				PayMode = 2
			});
			var okResult = response as OkObjectResult;
			//Assert
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.PS_EmployeePayInfo);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Update);

			Assert.Null(okResult);
		}
	}
}
