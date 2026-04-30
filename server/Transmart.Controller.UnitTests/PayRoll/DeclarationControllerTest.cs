using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Payroll;
using TranSmart.API.Extensions;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Models.Payroll.Request;
using TranSmart.Domain.Models.Payroll.Response;
using TranSmart.Service;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Controller.UnitTests.PayRoll
{
	public class DeclarationControllerTest : ControllerTestBase
	{
		private readonly Mock<IDeclarationService> _declarationService;
		private readonly Mock<IFinancialYearService> _fyservice;
		private readonly Mock<IApplicationAuditLogService> _appauditlogService;
		private readonly DeclarationController _controller;
		public DeclarationControllerTest()
		{
			_declarationService = new Mock<IDeclarationService>();
			_fyservice = new Mock<IFinancialYearService>();
			_appauditlogService = new Mock<IApplicationAuditLogService>();
			_controller = new DeclarationController(Mapper, _declarationService.Object, _fyservice.Object, _appauditlogService.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext()
					{
						User = Claim,
						Connection = { RemoteIpAddress = System.Net.IPAddress.Any }
					}
				},
			};
		}

		//[Fact]
		//public async Task Save_Test()
		//{
		//	Result<Declaration> result = new();
		//	_declarationService.Setup(x => x.AddAsync(It.IsAny<Declaration>())).Callback((Declaration declaration) =>
		//	{
		//		result.IsSuccess = true;
		//		result.ReturnValue = declaration;
		//	}).ReturnsAsync(result);
		//	_declarationService.Setup(x => x.GetById(It.IsAny<Guid>()))
		//		.ReturnsAsync((Guid id) => result.ReturnValue = new Declaration { EmployeeId = EmployeeId });
		//	var controller = _controller;

		//	var attributes = controller.GetType().GetMethod("Save").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
		//	var response = await controller.Save(new DeclarationRequest(), Guid.NewGuid());
		//	var okResult = response as OkObjectResult;

		//	Assert.True(attributes.Any());
		//	Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.PS_Declaration);
		//	Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Create);

		//	Assert.NotNull(okResult);
		//	Assert.Equal(200, okResult.StatusCode);
		//	Assert.Equal(EmployeeId, (okResult.Value as DeclarationModel).EmployeeId);
		//}

		//[Fact]
		//public async Task Modify_Test()
		//{
		//	Result<Declaration> result = new();
		//	_declarationService.Setup(x => x.UpdateAsync(It.IsAny<Declaration>())).Callback((Declaration declaration) =>
		//	{
		//		result.IsSuccess = true;
		//		result.ReturnValue = declaration;
		//	}).ReturnsAsync(result);
		//	_declarationService.Setup(x => x.GetById(It.IsAny<Guid>()))
		//		.ReturnsAsync((Guid id) => result.ReturnValue = new Declaration { EmployeeId = EmployeeId });
		//	var controller = _controller;

		//	var attributes = controller.GetType().GetMethod("Modify").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
		//	var response = await controller.Modify(new DeclarationRequest(), Guid.NewGuid());
		//	var okResult = response as OkObjectResult;

		//	Assert.True(attributes.Any());
		//	Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.PS_Declaration);
		//	Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Update);

		//	Assert.NotNull(okResult);
		//	Assert.Equal(200, okResult.StatusCode);
		//	Assert.Equal(EmployeeId, (okResult.Value as DeclarationModel).EmployeeId);
		//}

		[Fact]
		public async Task Declaration_GetMyDelaration_Test()
		{
			// Arrange  
			var result = new Result<Declaration>();
			_declarationService.Setup(x => x.GetDeclaration(It.IsAny<Guid>(), It.IsAny<Guid>()))
				.ReturnsAsync((Guid financialId, Guid empId) => result.ReturnValue = new Declaration { EmployeeId = EmployeeId });

			var controller = new DeclarationController(Mapper, _declarationService.Object, _fyservice.Object, _appauditlogService.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext()
					{
						User = Claim,
						Connection = { RemoteIpAddress = new System.Net.IPAddress(1822542) }
					}
				}
			};
			// Act
			var attributes = controller.GetType().GetMethod("GetMyDeclaration").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.GetMyDeclaration(Guid.NewGuid());
			var okResult = response as OkObjectResult;

			Assert.True(attributes.Any());
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SS_Declaration);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as DeclarationModel).EmployeeId);
		}
		[Fact]
		public async Task Put_Test()
		{
			//Arrange
			var result = new Result<Declaration>();
			_declarationService.Setup(x => x.GetSettings(It.IsAny<Guid>()))
				.ReturnsAsync(new DeclarationSetting { HouseLoanInt = 15000, MaxLimitEightyC = 10000, MaxLimitEightyD = 15000, Lock = 0 });
			_declarationService.Setup(x => x.UpdateAsync(It.IsAny<Declaration>())).
				Callback((Declaration declaration) =>
				{
					result.IsSuccess = true;
					result.ReturnValue = declaration;
				}).ReturnsAsync(result);
			_declarationService.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Declaration { EmployeeId = EmployeeId });

			//Act
			var response = await _controller.Put(new DeclarationRequest());
			var okResult = response as OkObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as DeclarationModel).EmployeeId);
		}
		[Fact]
		public async Task Put_Test_Throw_ITDeclaraion_Locked()
		{
			//Arrange
			var result = new Result<Declaration>();
			_declarationService.Setup(x => x.GetSettings(It.IsAny<Guid>()))
				.ReturnsAsync(new DeclarationSetting { HouseLoanInt = 15000, MaxLimitEightyC = 10000, MaxLimitEightyD = 15000, Lock = 1 });
			_declarationService.Setup(x => x.UpdateAsync(It.IsAny<Declaration>())).
				Callback((Declaration declaration) =>
				{
					result.IsSuccess = true;
					result.ReturnValue = declaration;
				}).ReturnsAsync(result);
			_declarationService.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Declaration { EmployeeId = EmployeeId });

			//Act
			var response = await _controller.Put(new DeclarationRequest());
			var okResult = response as ObjectResult;
			//Assert
			Assert.Equal(400, okResult.StatusCode);
			Assert.Equal("IT Declaration is locked", okResult.Value);
		}
		[Fact]
		public async Task AdminSave_Test()
		{
			//Arrange
			var result = new Result<Declaration>();
			_appauditlogService.Setup(x => x.GetAccesedUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(Task.FromResult(new AplicationAuditLog
				{
					AccesedAt = DateTime.Now,
					AccesedBy = "JCR",
					IPAddress = "192.168.200.182",
					Action = "Create",
					Entity = "Salary"
				}));
			_declarationService.Setup(x => x.AddAsync(It.IsAny<Declaration>())).
				Callback((Declaration declaration) =>
				{
					result.IsSuccess = false;
					result.ReturnValue = declaration;
				}).ReturnsAsync(result);
			//Act
			var response = await _controller.AdminSave(new DeclarationRequest { EmployeeId = EmployeeId});
			var okResult = response as ObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}
	}
}
