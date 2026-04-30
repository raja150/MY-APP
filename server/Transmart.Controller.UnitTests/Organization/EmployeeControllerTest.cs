using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Organization;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models.Organization;
using TranSmart.Service.Organization;
using Xunit;

namespace Transmart.Controller.UnitTests.Organization
{
	public class EmployeeControllerTest : ControllerTestBase
	{
		private readonly Mock<IEmployeeService> _service;

		private readonly EmployeeController _controller;
		public EmployeeControllerTest() : base()
		{
			_service = new Mock<IEmployeeService>();
			_controller = new EmployeeController(Mapper, _service.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};
		}


		[Fact]
		public async Task GetEmpTest()
		{
			var id = Guid.NewGuid();
			_ = _service.Setup(x => x.GetEmp(It.IsAny<Guid>())).Returns(Task.FromResult(new Employee { ID = EmployeeId }));

			// Act
			var response = await _controller.GetEmp(id);
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}

		[Fact]
		public async Task GetEmpNullTest()
		{
			var id = Guid.NewGuid();
			_ = _service.Setup(x => x.GetEmp(It.IsAny<Guid>())).Returns(Task.FromResult<Employee>(null));

			// Act
			var response = await _controller.GetEmp(id);
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Null(okResult);
		}


		[Fact]
		public async Task SearchEmpTest()
		{
			IEnumerable<Employee> employee = new List<Employee>().AsQueryable();
			_ = _service.Setup(x => x.SearchEmp(It.IsAny<string>())).Returns(Task.FromResult(employee));

			// Act
			var response = await _controller.SearchEmp("Name");
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}





		[Fact]
		public async Task LeavesApprovedEmployeesTest()
		{
			IEnumerable<ApplyLeave> leaves = new List<ApplyLeave>()
			{
				new ApplyLeave
				{
					ID = Guid.NewGuid(),
					Employee = new Employee
					{
						ID = Guid.NewGuid(),
						DepartmentId = DepartmentId
					}
				},
				new ApplyLeave
				{
					ID = Guid.NewGuid(),
					Employee = new Employee
					{
						ID = Guid.NewGuid(),
						DepartmentId = Guid.NewGuid()
					}
				}
			}.AsQueryable();
			 
			_service.Setup(x => x.LeavesApprovedEmployees(It.IsAny<Guid?>())).ReturnsAsync(
				(Guid? departmentId) =>
				{
					return leaves.Where(x => x.Employee.DepartmentId == departmentId);
				});


			// Act 
			var response = await _controller.LeavesApprovedEmployees();
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.True((okResult.Value as List<EmployeeModel>).Count == 1);
		}


		[Fact]
		public async Task ApprovedPendingEmployeesTest()
		{
			IEnumerable<ApplyLeave> leaves = new List<ApplyLeave>()
			{
				new ApplyLeave
				{
					ID = Guid.NewGuid(),
					Employee = new Employee
					{
						ID = Guid.NewGuid(),
						ReportingToId = EmployeeId
					}
				},
				new ApplyLeave
				{
					ID = Guid.NewGuid(),
					Employee = new Employee
					{
						ID = Guid.NewGuid(),
						ReportingToId = Guid.NewGuid()

					}
				}
			}.AsQueryable();

			_service.Setup(x => x.ApprovedPendingEmployees(It.IsAny<Guid>())).ReturnsAsync(
				(Guid? employeeId) =>
				{
					return leaves.Where(x => x.Employee.ReportingToId == employeeId);
				});


			// Act 
			var response = await _controller.ApprovedPendingEmployees();
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.True((okResult.Value as List<EmployeeModel>).Count == 1);
		}


		[Fact]
		public async Task ApprovedPendingWFHEmployeesTest()
		{
			IEnumerable<ApplyWfh> applyWFH = new List<ApplyWfh>()
			{
				new ApplyWfh
				{
					ID = Guid.NewGuid(),
					Employee = new Employee
					{
						ID = Guid.NewGuid(),
						ReportingToId = EmployeeId,
						Status = 1
					}
				},
				new ApplyWfh
				{
					ID = Guid.NewGuid(),
					Employee = new Employee
					{
						ID = Guid.NewGuid(),
						ReportingToId = Guid.NewGuid(),
						Status = 1
					}
				}
			}.AsQueryable();

			_service.Setup(x => x.ApprovedPendingWFHEmployees(It.IsAny<Guid>())).ReturnsAsync(
				() =>
				{
					return applyWFH.Where(x => x.Employee.ReportingToId == EmployeeId);
				});


			// Act 
			var response = await _controller.ApprovedPendingWFHEmployees();
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.True((okResult.Value as List<EmployeeModel>).Count == 1);
		}
	}
}
