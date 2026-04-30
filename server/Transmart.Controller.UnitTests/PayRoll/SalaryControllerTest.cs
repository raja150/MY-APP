using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Payroll; 
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Payroll.List;
using TranSmart.Domain.Models.Payroll.Request;
using TranSmart.Domain.Models.Payroll.Response;
using TranSmart.Domain.Models.PayRoll.List;
using TranSmart.Service;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Controller.UnitTests.PayRoll
{
	public class SalaryControllerTest : ControllerTestBase
	{
		private readonly Mock<ISalaryService> _service;
		private readonly Mock<IApplicationAuditLogService> _auditLogService;
		private readonly SalaryController _controller;
		public SalaryControllerTest() : base()
		{
			_service = new Mock<ISalaryService>();
			_auditLogService = new Mock<IApplicationAuditLogService>();
			_controller = new SalaryController(Mapper, _service.Object, _auditLogService.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext()
					{
						User = Claim,
						Connection =
						{
							RemoteIpAddress = new System.Net.IPAddress(1921682)
						}
					},
					RouteData = new Microsoft.AspNetCore.Routing.RouteData(),

				}
			};
		}
		[Fact]
		public async Task Post_Test()
		{
			var salary = new Salary
			{
				EmployeeId = EmployeeId,
				Annually = 1000000,
				CTC = 1000000,
				Monthly = 70000,
			};
			var result = new TranSmart.Core.Result.Result<Salary>();
			_auditLogService.Setup(x => x.GetAccesedUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(Task.FromResult(new AplicationAuditLog
				{
					AccesedAt = DateTime.Now,
					AccesedBy = "JCR",
					IPAddress = "192.168.200.182",
					Action = "Create",
					Entity = "Salary"
				}));
			_service.Setup(x => x.AddAsync(It.IsAny<Salary>())).Callback((Salary salary) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = salary;
			}).ReturnsAsync(result);
			//Act
			var response = await _controller.Post(new SalaryRequest
			{
				EmployeeId = EmployeeId,
				CTC = 1200000,
				Earnings = new List<SalaryEarningRequest> { new SalaryEarningRequest { IsDeleted = true, ComponentId = Guid.NewGuid() } },
				Deductions = new List<SalaryDeductionRequest> { new SalaryDeductionRequest { IsDeleted = true, DeductionId = Guid.NewGuid() } }
			});
			var okResult = response as OkObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as TranSmart.Domain.Models.Payroll.Response.SalaryModel).EmployeeId);
		}

		[Fact]
		public async Task Post_Test_Throw_Exception()
		{
			//Arrange
			var componentId = Guid.NewGuid();
			var deductionId = Guid.NewGuid();
			var salary = new Salary
			{
				EmployeeId = EmployeeId,
				Annually = 1000000,
				CTC = 1000000,
				Monthly = 70000,
			};
			var result = new TranSmart.Core.Result.Result<Salary>();
			_auditLogService.Setup(x => x.GetAccesedUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(Task.FromResult(new AplicationAuditLog
				{
					AccesedAt = DateTime.Now,
					AccesedBy = "JCR",
					IPAddress = "192.168.200.182",
					Action = "Create",
					Entity = "Salary"
				}));
			_service.Setup(x => x.AddAsync(It.IsAny<Salary>())).Callback((Salary salary) =>
			{
				result.IsSuccess = false;
				result.ReturnValue = salary;
			}).ReturnsAsync(result);
			//Act
			var response = await _controller.Post(new SalaryRequest
			{
				EmployeeId = EmployeeId,
				CTC = 1200000,
				Earnings = new List<SalaryEarningRequest>
				{ new SalaryEarningRequest
					{ IsDeleted = false, ComponentId = componentId },
					new SalaryEarningRequest{
					 IsDeleted = false, ComponentId = componentId }
				},
				Deductions = new List<SalaryDeductionRequest>
				{ new SalaryDeductionRequest { IsDeleted = false, DeductionId = deductionId },
				new SalaryDeductionRequest{  IsDeleted = false, DeductionId = deductionId} }
			});
			var okResult = response as ObjectResult;
			//Assert
			Assert.Equal(400, okResult.StatusCode);
		}
		[Fact]
		public async Task Put_Test()
		{
			var salary = new Salary
			{
				EmployeeId = EmployeeId,
				Annually = 1000000,
				CTC = 1000000,
				Monthly = 70000,
			};
			var result = new TranSmart.Core.Result.Result<Salary>();
			_auditLogService.Setup(x => x.GetAccesedUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(Task.FromResult(new AplicationAuditLog
				{
					AccesedAt = DateTime.Now,
					AccesedBy = "JCR",
					IPAddress = "192.168.200.182",
					Action = "Create",
					Entity = "Salary"
				}));
			_service.Setup(x => x.UpdateAsync(It.IsAny<Salary>())).Callback((Salary salary) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = salary;
			}).ReturnsAsync(result);
			var response = await _controller.Put(new SalaryRequest
			{
				EmployeeId = EmployeeId,
				CTC = 1200000,
				Earnings = new List<SalaryEarningRequest> { new SalaryEarningRequest { IsDeleted = true, ComponentId = Guid.NewGuid() } },
				Deductions = new List<SalaryDeductionRequest> { new SalaryDeductionRequest { IsDeleted = true, DeductionId = Guid.NewGuid() } }
			});
			var okResult = response as OkObjectResult;

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as TranSmart.Domain.Models.Payroll.Response.SalaryModel).EmployeeId);
		}

		[Fact]
		public async Task Put_Test_Throw_Exception()
		{
			//Arrange
			var compomponentId = Guid.NewGuid();
			var deductionId = Guid.NewGuid();
			var salary = new Salary
			{
				EmployeeId = EmployeeId,
				Annually = 1000000,
				CTC = 1000000,
				Monthly = 70000,
			};
			var result = new TranSmart.Core.Result.Result<Salary>();
			_auditLogService.Setup(x => x.GetAccesedUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(Task.FromResult(new AplicationAuditLog
				{
					AccesedAt = DateTime.Now,
					AccesedBy = "JCR",
					IPAddress = "192.168.200.182",
					Action = "Create",
					Entity = "Salary"
				}));
			_service.Setup(x => x.UpdateAsync(It.IsAny<Salary>())).Callback((Salary salary) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = salary;
			}).ReturnsAsync(result);
			var response = await _controller.Put(new SalaryRequest
			{
				EmployeeId = EmployeeId,
				CTC = 1200000,
				Earnings = new List<SalaryEarningRequest> {
					new SalaryEarningRequest { IsDeleted = false, ComponentId = compomponentId },
					new SalaryEarningRequest{IsDeleted = false, ComponentId = compomponentId}
				},
				Deductions = new List<SalaryDeductionRequest> {
					new SalaryDeductionRequest { IsDeleted = false, DeductionId = deductionId },
					new SalaryDeductionRequest{IsDeleted = false, DeductionId = deductionId}
				}
			});
			var okResult = response as ObjectResult;

			Assert.Equal(400, okResult.StatusCode);
		}
		[Fact]
		public async Task Get_Test()
		{
			//Arrange
			var salaryId = Guid.NewGuid();
			_auditLogService.Setup(x => x.GetAccesedUser(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(Task.FromResult(new AplicationAuditLog
				{
					AccesedAt = DateTime.Now,
					AccesedBy = "JCR",
					IPAddress = "192.168.200.182",
					Action = "Create",
					Entity = "Salary"
				}));
			_service.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Salary { EmployeeId = EmployeeId, Annually = 1000000, CTC = 1000000 });
			//Act
			var response = await _controller.Get(salaryId);
			var okResult = response as OkObjectResult;
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as TranSmart.Domain.Models.Payroll.Response.SalaryModel).EmployeeId);
		}
		[Fact]
		public async Task Paginate_Test()
		{
			//Arrange
			var list = new List<Salary> {
				new Salary
				{
					EmployeeId = EmployeeId,
					CTC = 1200000,
					Monthly = 100000,
					Annually=1200000,
					Earnings = new List<SalaryEarning> { new SalaryEarning { IsDeleted = false, ComponentId = Guid.NewGuid() } },
					Deductions = new List<SalaryDeduction> { new SalaryDeduction { IsDeleted = false, DeductionId = Guid.NewGuid() } }
				},
				new Salary
				{
					EmployeeId = Guid.NewGuid(),
					CTC = 960000,
					Monthly = 80000,
					Annually=960000,
					Earnings = new List<SalaryEarning> { new SalaryEarning { IsDeleted = false, ComponentId = Guid.NewGuid() } },
					Deductions = new List<SalaryDeduction> { new SalaryDeduction { IsDeleted = false, DeductionId = Guid.NewGuid() } }
				}
			}.AsQueryable();
			_service.Setup(x => x.GetPaginate(It.IsAny<BaseSearch>())).ReturnsAsync(list.ToPaginate(0, 10));
			//Act
			var response = await _controller.Paginate(new BaseSearch());
			var okResult = response as OkObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}
		[Fact]
		public async Task GetTemplateComponents_Test()
		{
			//Arrange
			var templateId = Guid.NewGuid();
			var list = new List<TemplateEarning> {
				new TemplateEarning {
					Amount= 100000,Percentage = 10
				} };
			_service.Setup(x => x.GetTemplateComponents(It.IsAny<Guid>()))
				.ReturnsAsync(list);
			//Act
			var response = await _controller.GetTemplateComponents(templateId);
			var result = response as OkObjectResult;
			//Assert
			Assert.NotNull(result);
			Assert.Equal(200, result.StatusCode);
		}
		[Fact]
		public async Task GetSalary_Test()
		{
			//Arrange
			var empId = Guid.NewGuid();
			var salary = new Salary { EmployeeId = EmployeeId, Annually = 1200000 };
			_service.Setup(x => x.EmpSalary(It.IsAny<Guid>())).ReturnsAsync(salary);
			//Act
			var response = await _controller.GetSalary(empId);
			var result = response as OkObjectResult;
			//Assert
			Assert.NotNull(result);
			Assert.Equal(200, result.StatusCode);
			Assert.Equal(EmployeeId, (result.Value as TranSmart.Domain.Models.Payroll.Response.SalaryModel).EmployeeId);
		}
	}
}
