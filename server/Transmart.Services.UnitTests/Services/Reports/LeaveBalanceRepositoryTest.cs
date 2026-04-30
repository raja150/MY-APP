using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.ApplyLeaveData;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Data.Repository.Leave;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Reports
{
	public class LeaveBalanceRepositoryTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly ILeaveBalanceRepository _repository;
		private EmployeeDataGenerator _employeeData;
		private Mock<DbContext> _context;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		public LeaveBalanceRepositoryTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			_repository = new LeaveBalanceRepository(_dbContext.Object);
			_employeeData = new EmployeeDataGenerator();
			_context = new Mock<DbContext>();
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
		}

		//[Theory]
		//[InlineData("a9cc5e1b-e24f-4939-b47d-1b86e583afc7", "05adb896-81cf-4323-9898-a8db16ca0a20", "1524b506-a8c0-4bda-a085-ea2811d82b50",
		//			"80ccbc50-ebf9-4654-9160-c36201d1783c", "9fdd7ce4-62dd-45a5-93ef-594a279e80bb", 10)]
		//public async Task BalanceReport_Test(Guid deptId, Guid designId, Guid teamId, Guid employeeId, Guid leaveTypeId, decimal balance)
		//{
		//	//Arrange
		//	var employee = _employeeData.GetEmployeeData();
		//	IEnumerable<LeaveBalance> leaveBalances = new List<LeaveBalance>
		//		{
		//			new LeaveBalance
		//			{
		//				ID=Guid.NewGuid(),
		//				Leaves=5,
		//				EmployeeId=employee.ID,
		//				Employee = employee,
		//				LeaveTypeId=Guid.Parse("9fdd7ce4-62dd-45a5-93ef-594a279e80bb"),
		//			},
		//			new LeaveBalance
		//			{
		//				ID=Guid.NewGuid(),
		//				Leaves=5,
		//				EmployeeId=employee.ID,
		//				Employee = employee,
		//				LeaveTypeId=Guid.Parse("9fdd7ce4-62dd-45a5-93ef-594a279e80bb"),
		//			},
		//			new LeaveBalance
		//			{
		//				ID=Guid.NewGuid(),
		//				Leaves=5,
		//				EmployeeId=employee.ID,
		//				Employee = employee,
		//				LeaveTypeId=Guid.Parse("a84b662a-d517-4fe6-92bf-238c7c63f630"),
		//			},
		//		}.AsQueryable();
		//	IEnumerable<LeaveType> leaveType = new List<LeaveType>
		//	{
		//		new LeaveType
		//		{
		//			ID = Guid.Parse("9fdd7ce4-62dd-45a5-93ef-594a279e80bb"),
		//			Code = "EL",
		//			Name = "Earned Leave",
		//			EffectiveAfter = 1,
		//			EffectiveType = 2,
		//			EffectiveBy = 1,
		//			ProrateByT = 1,
		//			RoundOff = 1,
		//			RoundOffTo = 1,
		//			Gender = 4,
		//			MaritalStatus = 3,
		//			PastDate = 2,
		//			FutureDate = 5,
		//			MaxApplications = 1,
		//			MinLeaves = 1,
		//			MaxLeaves = 12,
		//			specifiedperio = 1,
		//		}
		//	}.AsQueryable();

		//	//Mock 
		//	var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
		//	SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);
		//	_dbContext.Setup(x => x.Set<LeaveBalance>()).Returns(mockSetLeaveBalance.Object);

		//	//_dbContext.SetupProperty(x => x.Leave_LeaveBalance);

		//	//var obj = _dbContext.Object;
		//	//obj.Leave_LeaveBalance = (DbSet<LeaveBalance>)leaveBalances.AsQueryable();

		//	var repository = new RepositoryAsync<LeaveBalance>(_dbContext.Object);
		//	uow.Setup(m => m.GetRepositoryAsync<LeaveBalance>()).Returns(repository);

		//	var mockSetLeavetype = leaveType.AsQueryable().BuildMockDbSet();
		//	///SetData.MockLeaveType(uow, _context, mockSetLeavetype);
		//	_dbContext.Setup(x => x.Set<LeaveType>()).Returns(mockSetLeavetype.Object);
		//	var leaveTypeRepository = new RepositoryAsync<LeaveType>(_dbContext.Object);
		//	uow.Setup(m => m.GetRepositoryAsync<LeaveType>()).Returns(leaveTypeRepository);

		//	var mockSetEmployee = _employeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
		//	///SetData.MockEmployee(uow, _context, mockSetEmployee);
		//	_dbContext.Setup(x => x.Set<Employee>()).Returns(mockSetEmployee.Object);
		//	var empRepository = new RepositoryAsync<Employee>(_dbContext.Object);
		//	uow.Setup(m => m.GetRepositoryAsync<Employee>()).Returns(empRepository);

		//	//Act
		//	var dd = await _repository.BalanceReport(deptId, designId, teamId, employeeId, leaveTypeId);
		//	//Assert
		//	//Assert.Equal(balance, dd.);
		//}
	}
}
