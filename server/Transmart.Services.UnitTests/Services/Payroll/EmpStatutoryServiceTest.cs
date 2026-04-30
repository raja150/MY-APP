using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using Transmart.Services.UnitTests.Services.Payroll.PayRollData;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Payroll
{
	public class EmpStatutoryServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IEmpStatutoryService _empStatutoryService;
		private readonly Mock<DbContext> _context;
		private readonly EmployeeDataGenerator _employeeDataGenerator;
		public EmpStatutoryServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_empStatutoryService = new EmpStatutoryService(uow.Object);
			_context = new Mock<DbContext>();
			_employeeDataGenerator = new EmployeeDataGenerator();
		}

		[Fact]
		public async Task AddBulk_AddUpdateAndDelete_IsSuccess()
		{
			var existingId = Guid.NewGuid();
			var newId = Guid.NewGuid();
			var deleteId = Guid.NewGuid();
			var employeeId = Guid.NewGuid();
			#region Arrange

			var employee = _employeeDataGenerator.GetEmployeeData();
			var employeeList = _employeeDataGenerator.GetAllEmployeesData();
			var empStatutoryList = new List<EmpStatutory>()
			{
				new EmpStatutory
				{
					ID=deleteId,
					EmpId=Guid.NewGuid(),
					//Emp=employee,
					UAN="ABCDEFG",
					ESINo="1234",
					EnableESI=1,
					EmployeeContrib=1,
					EmployeesProvid="Test",
					EnablePF=1
				},
				  new EmpStatutory
				{
					ID=existingId,
					EmpId=employeeId,
					Emp=employee,
					UAN="ABCDEFG",
					ESINo="1254",
					EmployeeContrib=1,
					EmployeesProvid="Test",
					EnablePF=1,
					EnableESI=1
				}
			};
			var empStatutories = new List<EmpStatutory>()
			{
				new EmpStatutory
				{
					ID=existingId,
					EmpId=employeeId,
					Emp=employee,
					ESINo="123456"
				},
				 new EmpStatutory
				{
					 ID = newId,
					 EmpId=Guid.Empty,
				},
				 new EmpStatutory{}
			};
			#endregion

			#region Mock
			var _repository = _context.GetRepositoryAsyncDbSet(uow, empStatutoryList);
			_repository.Setup(x => x.SingleAsync(It.IsAny<Expression<Func<EmpStatutory, bool>>>(),
			 It.IsAny<Func<IQueryable<EmpStatutory>, IOrderedQueryable<EmpStatutory>>>(),
			 It.IsAny<Func<IQueryable<EmpStatutory>, IIncludableQueryable<EmpStatutory, object>>>(), true)).ReturnsAsync(
				 empStatutoryList.FirstOrDefault(x => x.ID == deleteId));
			var employeeMockSet = employeeList.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDataGenerator(uow, _context, employeeMockSet);
			#endregion

			//Act
			var dd =await _empStatutoryService.AddBulk(empStatutories);
			var list = await uow.Object.GetRepositoryAsync<EmpStatutory>().GetAsync();
			var updateEmpStatutory = list.FirstOrDefault(x => x.EmpId == employeeId);
			//Assert
			Assert.True(dd.HasNoError);
			Assert.Equal(0, list.Count(x => x.ID == deleteId));//delete
			Assert.Equal("123456", updateEmpStatutory.ESINo);//update
			Assert.Equal(1, list.Count(x => x.ID == newId));//add
			uow.Verify(m => m.SaveChangesAsync());
		}

		[Fact]
		public async Task AddBulk_IsCatch_ThrowException()
		{
			#region Arrange
			var employee = _employeeDataGenerator.GetEmployeeData();
			var employeeList = _employeeDataGenerator.GetAllEmployeesData();
			var empStatutoryList = new List<EmpStatutory>()
			{
				  new EmpStatutory
				{
					ID=Guid.NewGuid(),
					EmpId=Guid.NewGuid(),
					Emp=employee
				}
			};
			var empStatutories = new List<EmpStatutory>()
			{
				new EmpStatutory
				{
					ID=Guid.NewGuid(),
					Emp=employee
				}
			};
			#endregion

			#region Mock

			var mockSet = empStatutoryList.AsQueryable().BuildMockDbSet();
			SetData.MockEmpStatutory(uow, _context, mockSet);
			var employeeMockSet = employeeList.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDataGenerator(uow, _context, employeeMockSet);
			uow.Setup(x => x.SaveChangesAsync()).Throws(new InvalidOperationException());
			#endregion

			//Asset
			var dd = await _empStatutoryService.AddBulk(empStatutories);

			//Act
			Assert.True(dd.HasError);
		}

		[Fact]
		public async Task CustomValidation_EmployeeExists_ThrowError()
		{
			var empStatutoryId = Guid.NewGuid();
			var employeeId = Guid.NewGuid();
			#region Arrange

			var employee = _employeeDataGenerator.GetEmployeeData();
			var employeeList = _employeeDataGenerator.GetAllEmployeesData();
			var empStatutoryList = new List<EmpStatutory>()
			{
				new EmpStatutory
				{
					ID=empStatutoryId,
					EmpId=employeeId,
					Emp=employee,
					UAN="ABCDEFG",
					EnablePF=0,
					EmployeesProvid="123"
				},
				  new EmpStatutory
				{
					ID=Guid.NewGuid(),
					EmpId=employeeId,
					Emp=employee,
					UAN="ABCDEFG",
					EnablePF=0,
					EmployeesProvid="123"
				}
			};

			#endregion

			#region Mock

			var mockSet = empStatutoryList.AsQueryable().BuildMockDbSet();
			SetData.MockEmpStatutory(uow, _context, mockSet);

			var employeeMockSet = employeeList.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDataGenerator(uow, _context, employeeMockSet);

			#endregion

			//Asset
			var excutionResult = new Result<EmpStatutory>();
			await _empStatutoryService.CustomValidation(
				new EmpStatutory
				{
					ID = empStatutoryId,
					EmpId = employeeId,
					EnablePF = 0,
					EmployeesProvid = "1123",
					UAN = "GDXJJS"
				}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasError);
		}

		[Fact]
		public async Task CustomValidation_UANExists_ThrowError()
		{
			var empStatutoryId = Guid.NewGuid();
			var employeeId = Guid.NewGuid();
			#region Arrange

			var employee = _employeeDataGenerator.GetEmployeeData();
			var employeeList = _employeeDataGenerator.GetAllEmployeesData();
			var empStatutoryList = new List<EmpStatutory>()
			{
				new EmpStatutory
				{
					ID=empStatutoryId,
					EmpId=employeeId,
					Emp=employee,
					UAN="ABCDEFG",
					EnablePF=1,
					EmployeesProvid="123"
				},
				  new EmpStatutory
				{
					ID=Guid.NewGuid(),
					EmpId=employeeId,
					Emp=employee,
					UAN="ABCDEFG",
					EnablePF=1,
					EmployeesProvid="123"
				}
			};

			#endregion

			#region Mock

			var mockSet = empStatutoryList.AsQueryable().BuildMockDbSet();
			SetData.MockEmpStatutory(uow, _context, mockSet);

			var employeeMockSet = employeeList.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDataGenerator(uow, _context, employeeMockSet);

			#endregion

			//Asset
			var excutionResult = new Result<EmpStatutory>();
			await  _empStatutoryService.CustomValidation(
				new EmpStatutory
				{
					ID = Guid.NewGuid(),
					EmpId = Guid.NewGuid(),
					EnablePF = 1,
					EmployeesProvid = "1223",
					UAN = "ABCDEFG"
				}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasError);
		}

		[Fact]
		public async Task CustomValidation_AccountNumberExists_ThrowError()
		{
			var empStatutoryId = Guid.NewGuid();
			var employeeId = Guid.NewGuid();
			#region Arrange

			var employee = _employeeDataGenerator.GetEmployeeData();
			var employeeList = _employeeDataGenerator.GetAllEmployeesData();
			var empStatutoryList = new List<EmpStatutory>()
			{
				new EmpStatutory
				{
					ID=empStatutoryId,
					EmpId=employeeId,
					Emp=employee,
					UAN="ABCDEFG",
					EnablePF=1,
					EmployeesProvid="123"
				}
			};

			#endregion

			#region Mock

			var mockSet = empStatutoryList.AsQueryable().BuildMockDbSet();
			SetData.MockEmpStatutory(uow, _context, mockSet);

			var employeeMockSet = employeeList.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDataGenerator(uow, _context, employeeMockSet);

			#endregion

			//Asset
			var excutionResult = new Result<EmpStatutory>();
			await _empStatutoryService.CustomValidation(
				new EmpStatutory
				{
					ID = Guid.NewGuid(),
					EmpId = Guid.NewGuid(),
					EnablePF = 1,
					EmployeesProvid = "123",
					UAN = "DSS"
				}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasError);
		}

		[Fact]
		public async Task CustomValidation_NoError_IsSuccess()
		{
			var empStatutoryId = Guid.NewGuid();
			var employeeId = Guid.NewGuid();
			#region Arrange

			var employee = _employeeDataGenerator.GetEmployeeData();
			var employeeList = _employeeDataGenerator.GetAllEmployeesData();
			var empStatutoryList = new List<EmpStatutory>()
			{
				new EmpStatutory
				{
					ID=empStatutoryId,
					EmpId=employeeId,
					Emp=employee,
					UAN="ABCDEFG",
					EnablePF=1,
					EmployeesProvid="123"
				}
			};

			#endregion

			#region Mock

			var mockSet = empStatutoryList.AsQueryable().BuildMockDbSet();
			SetData.MockEmpStatutory(uow, _context, mockSet);

			var employeeMockSet = employeeList.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDataGenerator(uow, _context, employeeMockSet);

			#endregion

			//Asset
			var excutionResult = new Result<EmpStatutory>();
			await _empStatutoryService.CustomValidation(
				new EmpStatutory
				{
					ID = Guid.NewGuid(),
					EmpId = Guid.NewGuid(),
					EnablePF = 1,
					EmployeesProvid = "1253",
					UAN = "DSS"
				}, excutionResult);

			//Assert
			Assert.True(excutionResult.HasNoError);
		}
	}
}
