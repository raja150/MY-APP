using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using Transmart.Services.UnitTests.Services.Payroll.PayRollData;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.PayRoll;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Payroll
{
	public class EmpBonusServiceTest
	{

		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IEmpBonusService _empBonusService;
		private readonly Mock<DbContext> _context;
		private readonly EmployeeDataGenerator _employeeData;


		public EmpBonusServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_empBonusService = new EmpBonusService(uow.Object);
			_context = new Mock<DbContext>();
			_employeeData = new EmployeeDataGenerator();
		}

		//[Fact]
		//public async Task AddBulk_AddUpdateAndDeleteData_IsSuccess()
		//{
		//	var employeeId = Guid.NewGuid();
		//	var updateId = Guid.NewGuid();
		//	var deleteId = Guid.NewGuid();
		//	var addId = Guid.NewGuid();
		//	#region Arrange
		//	var empBonusList = new List<EmpBonus>
		//	{
		//		new EmpBonus
		//		{
		//			ID = updateId,
		//			EmployeeId=employeeId,
		//			Amount=120
		//		},
		//		new EmpBonus
		//		{
		//			ID = deleteId,
		//			//EmployeeId=Guid.NewGuid(),
		//			Amount=120
		//		}
		//	};
		//	var empBonus = new List<EmpBonus>
		//	{
		//		new EmpBonus
		//		{
		//			ID = updateId,
		//			EmployeeId=employeeId,
		//			Amount=100
		//		},
		//		new EmpBonus
		//		{
		//			ID = addId,
		//			EmployeeId=Guid.NewGuid(),
		//			Amount=10
		//		}
		//	};
		//	#endregion

		//	//Mock
		//	var _repository = _context.GetRepositoryAsyncDbSet(uow, empBonusList);
		//	_repository.Setup(x => x.SingleAsync(It.IsAny<Expression<Func<EmpBonus, bool>>>(),
		//	 It.IsAny<Func<IQueryable<EmpBonus>, IOrderedQueryable<EmpBonus>>>(),
		//	 It.IsAny<Func<IQueryable<EmpBonus>, IIncludableQueryable<EmpBonus, object>>>(), true)).ReturnsAsync(
		//		 empBonusList.FirstOrDefault(x => x.ID == deleteId));

		//	var dd = await _empBonusService.AddBulk(empBonus);
		//	var list = await uow.Object.GetRepositoryAsync<EmpBonus>().GetAsync();
		//	var updatedItem = list.FirstOrDefault(x => x.ID == updateId);
		//	//Assert
		//	Assert.True(dd.HasNoError);
		//	Assert.Equal(100, updatedItem.Amount);
		//	Assert.Equal(1, list.Count(x => x.ID == addId));
		//	Assert.Equal(0, list.Count(x => x.ID == deleteId));
		//	uow.Verify(m => m.SaveChangesAsync());
		//}
		[Fact]
		public async Task AddBulk_AddUpdateAndDeleteData_IsSuccess()
		{
			var employeeId = Guid.NewGuid();
			var updateId = Guid.NewGuid();
			var deleteId = Guid.NewGuid();
			var addId = Guid.NewGuid();
			#region Arrange
			var empBonusList = new List<EmpBonus>
			{
				new EmpBonus
				{
					ID = updateId,
					EmployeeId=employeeId,
					Amount=120
				},
				new EmpBonus
				{
					ID = deleteId,
					EmployeeId=Guid.NewGuid(),
					Amount=120
				}
			};
			var empBonus = new List<EmpBonus>
			{
				new EmpBonus
				{
					ID = updateId,
					EmployeeId=employeeId,
					Amount=100
				},
				new EmpBonus
				{
					ID = addId,
					EmployeeId=Guid.NewGuid(),
					Amount=10
				}
			};
			#endregion

			//Mock
			//_ = _context.GetRepositoryAsyncDbSet(uow, empBonusList);
			var mockSet = empBonusList.AsQueryable().BuildMockDbSet();
			SetData.MockEmpBonus(uow, _context, mockSet);

			var dd = await _empBonusService.AddBulk(empBonus);
			var list = await uow.Object.GetRepositoryAsync<EmpBonus>().GetAsync();
			var updatedItem = list.FirstOrDefault(x => x.ID == updateId);
			//Assert
			Assert.True(dd.HasNoError);
			Assert.Equal(100, updatedItem.Amount);
			Assert.Equal(1, list.Count(x => x.ID == addId));
			Assert.Equal(0, list.Count(x => x.ID == deleteId));
			uow.Verify(m => m.SaveChangesAsync());
		}


		[Fact]
		public void CustomValidation_ReleaseDataBeforeEmployeeJoinData_ThrowError()
		{
			#region Arrange

			var employeeList = _employeeData.GetAllEmployeesData();
			var employee = _employeeData.GetEmployeeData();
			var empBonusList = new List<EmpBonus>
			{
				new EmpBonus
				{
				ID = Guid.Parse("6D73C8BB-4B07-4C34-5157-08DA64AB8559"),
				EmployeeId=Guid.Parse("6D23C8BB-4B07-4C34-5157-08DA64AB8559"),
				Amount=120,
				Employee=employee,
				ReleasedOn= new DateTime(2021 , 08 , 12),
				},
				new EmpBonus
				{
				ID = Guid.Parse("6D13C8BB-4B07-4C34-5157-08DA64AB8559"),
				EmployeeId=Guid.Parse("6D43C8BB-4B07-4C34-5157-08DA64AB8559"),
				Amount=150,
				ReleasedOn= new DateTime(2021 , 08 , 12),
				}
			};
			var empBonus = new EmpBonus
			{
				ID = Guid.NewGuid(),
				Employee = employee,
				ReleasedOn = DateTime.Parse("2019-08-12")
			};
			#endregion

			#region Mock

			var mockSet = empBonusList.AsQueryable().BuildMockDbSet();
			SetData.MockEmpBonus(uow, _context, mockSet);

			var employeeDataMockSet = employeeList.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDataGenerator(uow, _context, employeeDataMockSet);

			#endregion

			//Asset
			var excutionResult = new Result<EmpBonus>();
			_ = _empBonusService.CustomValidation(
			  empBonus, excutionResult);
			var list = employeeList.Where(x => x.DateOfJoining <= empBonus.ReleasedOn);
			//Assert
			Assert.True(excutionResult.HasError);
			Assert.True(!list.Any());
		}

		[Fact]
		public void CustomValidation_ReleaseDataAfterEmployeeJoinData_NoError()
		{
			#region Arrange

			var employeeList = _employeeData.GetAllEmployeesData();
			var employee = _employeeData.GetEmployeeData();
			var empBonusList = new List<EmpBonus>
			{
				new EmpBonus
				{
				ID = Guid.Parse("6D73C8BB-4B07-4C34-5157-08DA64AB8559"),
				EmployeeId=Guid.Parse("6D23C8BB-4B07-4C34-5157-08DA64AB8559"),
				Amount=120,
				Employee=employee,
				ReleasedOn= new DateTime(2021 , 08 , 12),
				},
				new EmpBonus
				{
				ID = Guid.Parse("6D13C8BB-4B07-4C34-5157-08DA64AB8559"),
				EmployeeId=Guid.Parse("6D43C8BB-4B07-4C34-5157-08DA64AB8559"),
				Amount=150,
				ReleasedOn= new DateTime(2021 , 08 , 12),
				}
			};
			var empBonus = new EmpBonus
			{
				ID = Guid.NewGuid(),
				Employee = employee,
				ReleasedOn = DateTime.Parse("2021-08-12")
			};
			#endregion

			#region Mock

			var mockSet = empBonusList.AsQueryable().BuildMockDbSet();
			SetData.MockEmpBonus(uow, _context, mockSet);

			var employeeDataMockSet = employeeList.AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeDataGenerator(uow, _context, employeeDataMockSet);

			#endregion

			//Asset
			var excutionResult = new Result<EmpBonus>();
			_ = _empBonusService.CustomValidation(empBonus, excutionResult);
			var list = employeeList.Where(x => x.DateOfJoining <= empBonus.ReleasedOn);
			//Assert
			Assert.True(excutionResult.HasNoError);
			Assert.NotNull(list);
		}
	}
}
