using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Organization.Setup;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Service.Organization;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Organization
{
	public class EmployeeResignationServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private IEmployeeResignationService _service;
		private readonly Mock<DbContext> _context;
		public EmployeeResignationServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new EmployeeResignationService(uow.Object);
			_context = new Mock<DbContext>();
		}



		[Fact]
		public void  CustomValidation_LastWorkingDateLesserThanApprovedOn_NoException()
		{
			//Arrange & Mock
			var id = Guid.Parse("1D73C8BB-4B04-4C34-5157-08DA64AB8559");
            var employee = new List<Employee>
			{
				new Employee
				{
					ID = id,
					LastWorkingDate = DateTime.Parse("2022-08-03")
				}
			};
			var mockEmployee = employee.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			var employeeResignationToDB = new List<EmployeeResignation>() { new EmployeeResignation(){} };

			var mockMockEmployeeResignation = employeeResignationToDB.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeResignation(uow, _context, mockMockEmployeeResignation);

			var test = new EmployeeResignation()
			{
				ID = Guid.NewGuid(),
				EmployeeId = id,
				ApprovedOn = DateTime.Parse("2022-10-10")
			};

			//Act
			var src = new EmployeeResignationService(uow.Object);
			var result = new Result<EmployeeResignation>();

			_ = src.CustomValidation(test, result);

			//Assert
			Assert.True(result.HasNoError);
		}



		[Fact]  
		public void  CustomValidation_LastWorkingDateGreaterThanApprovedOn_ThrowException()
		{
			//Arrange & Mock
			var employee = new List<Employee>
			{
				new Employee
				{
					LastWorkingDate = DateTime.Parse("2022-08-03")
				}
			};

			var mockEmployee = employee.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			var employeeResignationToDB = new List<EmployeeResignation>() { new EmployeeResignation() { } };

			var mockMockEmployeeResignation = employeeResignationToDB.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeResignation(uow, _context, mockMockEmployeeResignation);

			var test = new EmployeeResignation()
			{
				ApprovedOn = DateTime.Parse("2022-06-10")
			};

			//Act
			var src = new EmployeeResignationService(uow.Object);
			var result = new Result<EmployeeResignation>();

			_ = src.CustomValidation(test, result);

			//Assert
			Assert.True(result.HasError);
		}




		[Fact]    
		public void  CustomValidation_NewEmployee_ThrowException()
		{
			//Arrange & Mock
			var employee = new List<Employee>	{new Employee	{}	};

			var mockEmployee = employee.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			var employeeResignationToDB = new List<EmployeeResignation>() { new EmployeeResignation() { } };

			var mockMockEmployeeResignation = employeeResignationToDB.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeResignation(uow, _context, mockMockEmployeeResignation);

			var test = new EmployeeResignation() {	EmployeeId = Guid.NewGuid()	};

			//Act
			var src = new EmployeeResignationService(uow.Object);
			var result = new Result<EmployeeResignation>();

			_ = src.CustomValidation(test, result);

			//Assert
			Assert.True(result.HasError);
		}
	}
}
