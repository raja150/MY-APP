using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using Transmart.Services.UnitTests.Services.Organization.Setup;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models.Organization;
using TranSmart.Service.Organization;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Organization
{
	public class UpdateProfileServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IUpdateProfileService _service;
		private readonly Mock<DbContext> _context;
		private readonly EmployeeDataGenerator _employee;
		public UpdateProfileServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new UpdateProfileService(uow.Object);
			_context = new Mock<DbContext>();
			_employee = new EmployeeDataGenerator();
		}


		[Fact]
		public async Task Verification_ExistEmployee_NoException()
		{
			// Arrange & Mock			
			var id = Guid.Parse("5032288a-7205-4e4d-8f34-a343aa59cb2c");
			var mockEmployeeToDB = new List<Employee>()
			{
				new Employee
				{
					ID = id
				},
				new Employee
				{
					ID = Guid.NewGuid()
				}

			};
			var mockEmployee = mockEmployeeToDB.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			//Act
			var result = await _service.Verification(id);

			//Assert
			Assert.True(result.HasNoError);
		}



		[Fact]
		public async Task Verification_NewEmployee_ThrowException()
		{
			// Arrange & Mock			
			var id = Guid.Parse("5032288a-7205-4e4d-8f34-a343aa59cb2c");
			var mockEmployeeToDB = new List<Employee>()
			{
				new Employee
				{
					ID = Guid.NewGuid()
				},
				new Employee
				{
					ID = Guid.NewGuid()
				}

			};
			var mockEmployee = mockEmployeeToDB.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			//Act
			var result = await _service.Verification(id);

			//Assert
			Assert.True(result.HasError);
		}




		[Fact]    
		public async Task UpdateFromProfile_SpecificEmployeeWithName_NoException()
		{
			// Arrange & Mock
			var employeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c");
			var employees = _employee.GetAllEmployeesData();
			var mockEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			var item = new EmpProfileModel()
			{
				ID = employeeId,
				Name = "Shiva",
				MobileNumber = "9866682334"
			};

			//Assert
			var result = await _service.UpdateFromProfile(employeeId, item);

			//Act
			Assert.True(result.IsSuccess);
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
		}




		[Fact] 
		public async Task UpdateFromProfile_SpecificEmployeeWithMbl_NoException()
		{
			// Arrange & Mock
			var employeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c");
			var employees = _employee.GetAllEmployeesData();
			var mockEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			var item = new EmpProfileModel()
			{
				ID = employeeId,
				DateOfBirth = DateTime.Parse("2000-08-08"),
				MobileNumber = "9866682334",

			};

			//Assert
			var result = await _service.UpdateFromProfile(employeeId, item);

			//Act
			Assert.True(result.IsSuccess);
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
		}




		[Fact]    
		public async Task UpdateFromProfile_NewEmployee_ThrowException()
		{
			// Arrange & Mock
			var employeeId = Guid.Parse("4D73C8BB-4B04-4C34-5157-08DA64AB8559");
			var employees = _employee.GetAllEmployeesData();
			var mockEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			var item = new EmpProfileModel()
			{
				ID = employeeId,
				Name = "Arjun",
				MobileNumber = "9866682334",

			};

			//Assert
			var result = await _service.UpdateFromProfile(employeeId, item);

			//Act
			Assert.True(result.HasError);
		}



		[Fact]         
		public async Task UpdateFromProfile_InvalidMobile_ThrowException()
		{
			// Arrange & Mock
			var employeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c");
			var employees = _employee.GetAllEmployeesData();
			var mockEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockEmployee);

			var item = new EmpProfileModel()
			{
				ID = employeeId,
				DateOfBirth = DateTime.Parse("2000-08-08"),
				MobileNumber = "55",

			};

			//Assert
			var result = await _service.UpdateFromProfile(employeeId, item);

			//Act
			Assert.True(result.HasError);
		}
	}
}


