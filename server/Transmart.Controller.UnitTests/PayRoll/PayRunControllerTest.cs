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
using TranSmart.API.Controllers.Payroll;
using TranSmart.API.Controllers.PayRoll;
using TranSmart.API.Extensions;
using TranSmart.API.Services;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Models.Leave.Request;
using TranSmart.Domain.Models.Payroll;
using TranSmart.Service;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Controller.UnitTests.PayRoll
{
	public class PayRunControllerTest : ControllerTestBase
	{
		private readonly Mock<IPayRollService> _service;
		private readonly Mock<IPayMonthService> _payMonthService;
		private readonly Mock<IApplicationAuditLogService> _auditLogService;
		private readonly Mock<ICacheService> _cacheService;
		private readonly PayRunController _controller;

		public PayRunControllerTest() : base()
		{
			_service = new Mock<IPayRollService>();
			_payMonthService = new Mock<IPayMonthService>();
			_auditLogService = new Mock<IApplicationAuditLogService>();
			_cacheService = new Mock<ICacheService>();

			_controller = new PayRunController(Mapper, _service.Object, _payMonthService.Object, _auditLogService.Object, _cacheService.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext()
					{
						User = Claim,
						Connection =
						{
							RemoteIpAddress = new System.Net.IPAddress(16885952)
						},
						
					},
				},
			};
		}

		[Fact]
		public async Task PayRunController_Run_PayRunUserIsFalse()
		{
			// Arrange  
			Result<PaySheet> result = new();

			_auditLogService.Setup(x => x.GetAccesedUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(Task.FromResult(new AplicationAuditLog
				 {
					 AccesedAt = DateTime.Now,
					 AccesedBy = "JCR",
					 IPAddress = "192.168.200.182",
					 Action = "Create",
					 Entity = "Salary"
				 }));
			_payMonthService.Setup(x => x.GetById(It.IsAny<Guid>()))
				.ReturnsAsync(new PayMonth { ID=EmployeeId, FinancialYearId=Guid.NewGuid()});

			_cacheService.Setup(x => x.PayRunUser()).Returns(false);

			_service.Setup(x => x.Process(It.IsAny<Guid>())).Callback((Guid id) =>
			{
				result.IsSuccess = true;
			}).ReturnsAsync(result);

			var controller = _controller;
			// Act
			var attributes = controller.GetType().GetMethod("Run").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var resposne = await controller.Run(EmployeeId);
			var okResult = resposne as OkObjectResult;

			// Assert
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.PS_Payrun);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, ((PayMonthModel)okResult.Value).ID);
		}
		[Fact]
		public async Task PayRunController_Run_PayRunUserIsTrue()
		{
			// Arrange  
			Result<PaySheet> result = new();

			_auditLogService.Setup(x => x.GetAccesedUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(Task.FromResult(new AplicationAuditLog
				{
					AccesedAt = DateTime.Now,
					AccesedBy = "JCR",
					IPAddress = "192.168.200.182",
					Action = "Create",
					Entity = "Salary"
				}));

			_cacheService.Setup(x => x.PayRunUser()).Returns(true);

			_service.Setup(x => x.Process(It.IsAny<Guid>())).Callback((Guid id) =>
			{
				result.IsSuccess = true;
			}).ReturnsAsync(result);

			var controller = _controller;
			// Act
			var attributes = controller.GetType().GetMethod("Run").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var resposne = await controller.Run(EmployeeId);
			var okResult = resposne as OkObjectResult;

			// Assert
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.PS_Payrun);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

			Assert.Null(okResult);
		}

		[Fact]
		public async Task PayRunController_Release_Test()
		{
			// Arrange  
			Result<PaySheet> result = new();

			_auditLogService.Setup(x => x.GetAccesedUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(Task.FromResult(new AplicationAuditLog
				{
					AccesedAt = DateTime.Now,
					AccesedBy = "JCR",
					IPAddress = "192.168.200.182",
					Action = "Create",
					Entity = "Salary"
				}));
			_payMonthService.Setup(x => x.GetById(It.IsAny<Guid>()))
				.ReturnsAsync(new PayMonth { ID = EmployeeId, FinancialYearId = Guid.NewGuid() });


			_service.Setup(x => x.Release(It.IsAny<Guid>())).Callback((Guid id) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new PaySheet { EmployeeID = EmployeeId };
			}).ReturnsAsync(result);

			var controller = _controller;
			// Act
			var attributes = controller.GetType().GetMethod("Run").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var resposne = await controller.Release(EmployeeId);
			var okResult = resposne as OkObjectResult;

			// Assert
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.PS_Payrun);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, ((PayMonthModel)okResult.Value).ID);
		}

		[Fact]
		public async Task PayRunController_Hold_Test()
		{
			// Arrange  
			Result<PaySheet> result = new();

			_auditLogService.Setup(x => x.GetAccesedUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(Task.FromResult(new AplicationAuditLog
				{
					AccesedAt = DateTime.Now,
					AccesedBy = "JCR",
					IPAddress = "192.168.200.182",
					Action = "Create",
					Entity = "Salary"
				}));
			_payMonthService.Setup(x => x.GetById(It.IsAny<Guid>()))
				.ReturnsAsync(new PayMonth { ID = EmployeeId, FinancialYearId = Guid.NewGuid() });


			_service.Setup(x => x.Hold(It.IsAny<Guid>())).Callback((Guid id) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new PaySheet { EmployeeID = EmployeeId };
			}).ReturnsAsync(result);

			var controller = _controller;
			// Act
			var attributes = controller.GetType().GetMethod("Run").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var resposne = await controller.Hold(EmployeeId);
			var okResult = resposne as OkObjectResult;

			// Assert
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.PS_Payrun);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, ((PayMonthModel)okResult.Value).ID);
		}

		[Fact]
		public async Task PayRunController_Delete_Test()
		{
			// Arrange  
			Result<PaySheet> result = new();

			_auditLogService.Setup(x => x.GetAccesedUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(Task.FromResult(new AplicationAuditLog
				{
					AccesedAt = DateTime.Now,
					AccesedBy = "JCR",
					IPAddress = "192.168.200.182",
					Action = "Create",
					Entity = "Salary"
				}));
			_payMonthService.Setup(x => x.GetById(It.IsAny<Guid>()))
				.ReturnsAsync(new PayMonth { ID = EmployeeId, FinancialYearId = Guid.NewGuid() });


			_service.Setup(x => x.Delete(It.IsAny<Guid>())).Callback((Guid id) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new PaySheet { EmployeeID = EmployeeId };
			}).ReturnsAsync(result);

			var controller = _controller;
			// Act
			var attributes = controller.GetType().GetMethod("Delete").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var resposne = await controller.Delete(EmployeeId);
			var okResult = resposne as OkObjectResult;

			// Assert
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.PS_Payrun);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Delete);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}
	}
}
