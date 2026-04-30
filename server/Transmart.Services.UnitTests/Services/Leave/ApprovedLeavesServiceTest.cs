using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using Transmart.Services.UnitTests.Services.SelfServiceData;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Service.Leave;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Leave
{
	public class ApprovedLeavesServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IApprovedLeavesService _service;
		private readonly EmployeeDataGenerator _employeeData;
		private readonly Mock<DbContext> _context;

		public ApprovedLeavesServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new ApprovedLeavesService(uow.Object);
			_employeeData = new EmployeeDataGenerator();
			_context = new Mock<DbContext>();
		}

		private void ApprovedLeavesMockData()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();

			IEnumerable<LeaveBalance> leaveBalance = new List<LeaveBalance>
			{
				new LeaveBalance
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					Employee=employee,
					LeaveTypeId=Guid.NewGuid()
				},
				new LeaveBalance
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Employee=employee,
					LeaveTypeId=Guid.NewGuid()
				}

			};
			IEnumerable<ApprovedLeaves> data = new List<ApprovedLeaves>
			{
				new ApprovedLeaves
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					Employee=employee
				},
				new ApprovedLeaves
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Reason="xddf",
					Employee=employee
				}

			};
			#endregion

			#region Mock

			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockApprovedLeaves(uow, _context, mockSet);

			var mockSetLeaveBalance = leaveBalance.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			#endregion
		}
		[Fact]
		public async Task OnBeforeUpdate_Update_WithOutException()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			Result<ApprovedLeaves> result = new();
			var leaveBalance = new List<LeaveBalance>
			{
				new LeaveBalance
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					Employee=employee,
					LeaveTypeId=Guid.NewGuid()
				},
				new LeaveBalance
				{
					EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783d"),
					ID = Guid.NewGuid(),
					Employee=employee,
					LeaveTypeId=Guid.Parse("80ccba50-ebf9-4654-9160-c36201d1783c")
				}

			};
			IEnumerable<ApprovedLeaves> data = new List<ApprovedLeaves>
			{
				new ApprovedLeaves
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					Employee=employee,
					LeaveTypeId=Guid.NewGuid()
				},
				new ApprovedLeaves
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Reason="xddf",
					Employee=employee,
					LeaveTypeId=Guid.Parse("80ccba50-ebf9-4654-9160-c36201d1783c")
				}

			};

			ApprovedLeaves leaves = new()
			{
				EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783a"),
				ID = Guid.NewGuid(),
				Reason = "HHH",
				Employee = employee,
				LeaveTypeId = Guid.Parse("80ccba50-ebf9-4654-9160-c36201d1783c")
			};
			#endregion

			#region Mock

			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockApprovedLeaves(uow, _context, mockSet);

			var mockSetLeaveBalance = leaveBalance.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			#endregion

			//Act
			var service = new ApprovedLeavesService(uow.Object);
			await service.OnBeforeUpdate(leaves, result);

			//Assert
			Assert.True(result.HasNoError);
			Assert.Equal(leaves.EmployeeId, leaveBalance[1].EmployeeId);
		}

		[Fact]
		public async Task OnBeforeUpdate_AlreadyApplied_ThrowException()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			Result<ApprovedLeaves> result = new();
			IEnumerable<LeaveBalance> leaveBalance = new List<LeaveBalance>
			{
				new LeaveBalance
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					Employee=employee,
					LeaveTypeId=Guid.NewGuid()
				},
				new LeaveBalance
				{
					EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
					ID = Guid.NewGuid(),
					Employee=employee,
					LeaveTypeId=Guid.Parse("80ccbb50-ebf9-4654-9160-c36201d1783c")
				}

			}.AsQueryable();
			IEnumerable<ApprovedLeaves> data = new List<ApprovedLeaves>
			{
				new ApprovedLeaves
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					Employee=employee,
					LeaveTypeId=Guid.NewGuid()
				},
				new ApprovedLeaves
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Reason="xddf",
					Employee=employee,
					LeaveTypeId=Guid.Parse("80ccbb50-ebf9-4654-9160-c36201d1783c")
				}

			}.AsQueryable();

			ApprovedLeaves leaves = new()
			{
				EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
				ID = Guid.NewGuid(),
				Reason = "HHH",
				Employee = employee,
				LeaveTypeId = Guid.Parse("80ccbb50-ebf9-4654-9160-c36201d1783c")
			};
			#endregion

			#region Mock

			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockApprovedLeaves(uow, _context, mockSet);

			var mockSetLeaveBalance = leaveBalance.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			#endregion

			//Act
			var service = new ApprovedLeavesService(uow.Object);
			await service.OnBeforeUpdate(leaves, result);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public async Task OnBeforeAdd_DataAdded_WithOutException()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			Result<ApprovedLeaves> result = new();
			ApprovedLeavesMockData();

			ApprovedLeaves leaves = new()
			{
				EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783d"),
				ID = Guid.NewGuid(),
				Reason = "HHH",
				Employee = employee
			};
			#endregion

			//Act
			var service = new ApprovedLeavesService(uow.Object);
			await service.OnBeforeAdd(leaves, result);

			//Assert
			Assert.True(result.HasNoError);
		}

		[Fact]
		public async Task OnBeforeAdd_AlreadyApplied_ThrowException()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			Result<ApprovedLeaves> result = new();
			ApprovedLeavesMockData();

			ApprovedLeaves leaves = new()
			{
				EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
				ID = Guid.NewGuid(),
				Reason = "HHH",
				Employee = employee
			};
			#endregion

			//Act
			var service = new ApprovedLeavesService(uow.Object);
			await service.OnBeforeAdd(leaves, result);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public async Task CustomValidation_ApprovedLeavesNull_WithOutException()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			Result<ApprovedLeaves> result = new();
			ApprovedLeavesMockData();

			ApprovedLeaves leaves = new()
			{
				EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783d"),
				ID = Guid.NewGuid(),
				Reason = "HHH",
				Employee = employee
			};
			#endregion

			//Act
			var service = new ApprovedLeavesService(uow.Object);
			await service.CustomValidation(leaves, result);

			//Assert
			Assert.True(result.HasNoError);
		}

		[Fact]
		public async Task CustomValidation_AlreadyApplied_ThrowException()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			Result<ApprovedLeaves> result = new();
			ApprovedLeavesMockData();

			ApprovedLeaves leaves = new()
			{
				EmployeeId = Guid.Parse("80ccbc50-ebf9-4654-9160-c36201d1783c"),
				ID = Guid.NewGuid(),
				Reason = "HHH",
				Employee = employee
			};
			#endregion

			//Act
			var service = new ApprovedLeavesService(uow.Object);
			await service.CustomValidation(leaves, result);

			//Assert
			Assert.True(result.HasError);
		}

	}
}

