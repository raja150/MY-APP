using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using Transmart.Services.UnitTests.Services.SelfServiceData;
using TranSmart.API.Models.Import;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Service.Leave;
using Xunit;
using LeaveBalance = TranSmart.Domain.Entities.Leave.LeaveBalance;

namespace Transmart.Services.UnitTests.Services.Leave
{
	public class AdjustLeaveServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IAdjustLeaveService _service;
		private readonly EmployeeDataGenerator _employeeData;
		private readonly Mock<DbContext> _context;

		public AdjustLeaveServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new AdjustLeaveService(uow.Object);
			_employeeData = new EmployeeDataGenerator();
			_context = new Mock<DbContext>();
		}
		private void AdjustLeaveMockData()
		{
			var employee = _employeeData.GetEmployeeData();
			LeaveType leaveType = new()
			{
				Name = "EL"
			};
			IEnumerable<AdjustLeave> data = new List<AdjustLeave>
			{
				new AdjustLeave
				{
					EmployeeId = Guid.NewGuid(),
					ID = employee.ID,
					Employee=employee,
					LeaveType=leaveType
				},
				new AdjustLeave
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Employee=employee,
					LeaveType=leaveType
				}

			};
			IEnumerable<TranSmart.Domain.Entities.Leave.LeaveBalance> leaveBalances = new List<TranSmart.Domain.Entities.Leave.LeaveBalance>
			{
				new LeaveBalance
				{
					EmployeeId = Guid.NewGuid(),
					ID = employee.ID,
					Employee=employee,
					LeaveType=leaveType
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Employee=employee,
					LeaveType=leaveType
				}

			};

			#region Mock

			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockAdjustLeave(uow, _context, mockSet);

			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			#endregion
		}

		[Fact]
		public async Task AddBulk_SaveChanges_WithOutException()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			AdjustLeaveMockData();
			List<AdjustLeave> leave = new()
			{
				new AdjustLeave
				{
					EmployeeId = employee.ID,
					LeaveTypeId = leaveTypeId,
				},
				new AdjustLeave
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid()
				},
			};
			#endregion

			//Act
			var adjustLeave = await _service.AddBulk(leave);

			//Assert
			Assert.True(adjustLeave.HasNoError);
		}

		[Fact]
		public async Task OnBeforeAdd_DataSaved_WithOutException()
		{
			//Arrange
			AdjustLeaveMockData();
			var adjustLeaveId = Guid.NewGuid();
			var employee = _employeeData.GetEmployeeData();
			var elId = Guid.NewGuid();
			var clId = Guid.NewGuid();

			var leaveBalance = new List<LeaveBalance>
			{ new LeaveBalance
				{
					LeaveTypeId = clId,
					Leaves = 6,
				},
				new LeaveBalance
				{
					LeaveTypeId = clId,
					Leaves = -2,//Approved Leaves
				},
				new LeaveBalance
				{
					LeaveTypeId= elId,
					Leaves = 6,
				}
			};
			AdjustLeave leave = new()
			{
				ID = adjustLeaveId,
				EmployeeId = employee.ID,
				LeaveTypeId = clId
			};
			//Mock
			var mockSetLeaveBalance = leaveBalance.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);
			//Act
			var adjustLeave = await _service.AddAsync(leave);

			//Assert
			Assert.True(adjustLeave.HasNoError);
		}

		[Fact]
		public async Task OnBeforeUpdate_DataUpdated_WithOutException()
		{
			//Arrange
			AdjustLeaveMockData();
			var adjustLeaveId = Guid.NewGuid();
			var employee = _employeeData.GetEmployeeData();
			var elId = Guid.NewGuid();
			var clId = Guid.NewGuid();

			var leaveBalance = new List<LeaveBalance>
			{ new LeaveBalance
				{
					LeaveTypeId = clId,
					Leaves = 6,
					EmployeeId= employee.ID,
					CustomizedBalId = adjustLeaveId,
				},
				new LeaveBalance
				{
					LeaveTypeId = clId,
					Leaves = -2,//Approved Leaves
					EmployeeId= employee.ID,
				},
				new LeaveBalance
				{
					LeaveTypeId= elId,
					Leaves = 6,
					EmployeeId= employee.ID,
				}
			};

			AdjustLeave leave = new()
			{
				ID = adjustLeaveId,
				EmployeeId = employee.ID,
				LeaveTypeId = clId,
				NewBalance = 5
			};
			//Mock
			var mockSetLeaveBalance = leaveBalance.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			//Act
			var adjustLeave = await _service.UpdateAsync(leave);

			//Assert
			Assert.True(leaveBalance[0].Leaves == leave.NewBalance);
			Assert.True(adjustLeave.HasNoError);
		}
		[Theory]
		[InlineData(1, true)]  //Throw exception Since Already 2 Leaves Booked
		[InlineData(-2, true)] //Throw Exception Since Already 2 Leaves Booked
		[InlineData(4, false)] //Initial Leave Balance = 6, No exception because Available is 4 booked is 2
		[InlineData(8, false)] //Initial Leave Balance = 6, No exception because Available is 8 booked is 2
		[InlineData(18, false)]//Initial Leave Balance = 6, No exception because Available is 18 booked is 2
		public async Task OnBeforeUpdate_WithException(int newBalance, bool isValid)
		{
			//Arrange
			Result<AdjustLeave> result = new();

			var employee = _employeeData.GetEmployeeData();
			var elId = Guid.NewGuid();
			var clId = Guid.NewGuid();
			var adjustLeaveId = Guid.NewGuid();

			var leaveBalance = new List<LeaveBalance>
			{ new LeaveBalance
				{
					EmployeeId = employee.ID,
					LeaveTypeId = clId,
					Leaves = 6,
					CustomizedBalId = adjustLeaveId,
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					LeaveTypeId = clId,
					Leaves = -2, //Approved Leaves
					ApplyLeaveId = Guid.NewGuid(),
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					LeaveTypeId= elId,
					Leaves = 6,
				}
			};

			var adjustLeaves = new List<AdjustLeave> { new AdjustLeave
			{
				EmployeeId = employee.ID,
				ID=adjustLeaveId,
				LeaveTypeId = clId,
				NewBalance = 6
			}
			};
			AdjustLeave adjustLeaveRequest = new()
			{
				ID = adjustLeaveId,
				EmployeeId = employee.ID,
				LeaveTypeId = clId,
				NewBalance = newBalance
			};
			#region Mock

			var mockSet = adjustLeaves.AsQueryable().BuildMockDbSet();
			SetData.MockAdjustLeave(uow, _context, mockSet);

			var mockSetLeaveBalance = leaveBalance.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			#endregion
			//Act
			var src = new AdjustLeaveService(uow.Object);
			await src.OnBeforeUpdate(adjustLeaveRequest, result);

			//Assert
			Assert.Equal(result.HasError, isValid);
		}


		[Theory]
		[InlineData(-5, true)]  //Remainning Leave Balance 4 ,Now we are Reducing to -1,Throws exception
		[InlineData(-2, false)] //Remainning Leave Balance 4 ,Now we are Reducing to 2, No exception
		[InlineData(-4, false)] //Initial Leave Balance = 6, Now we are Reducing to 0, No exception
		[InlineData(8, false)] //Initial Leave Balance = 6, Used 2, Now we are incrasing to 12, No exception
		public async Task OnBeforeAdd_WithException(int newBalance, bool isValid)
		{
			//Arrange
			Result<AdjustLeave> result = new();

			var employee = _employeeData.GetEmployeeData();
			var elId = Guid.NewGuid();
			var clId = Guid.NewGuid();
			var adjustLeaveId = Guid.NewGuid();

			var leaveBalance = new List<LeaveBalance>
			{ new LeaveBalance
				{
					EmployeeId = employee.ID,
					LeaveTypeId = clId,
					Leaves = 6,
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					LeaveTypeId = clId,
					Leaves = -2 //Approved Leaves
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					LeaveTypeId= elId,
					Leaves = 6,
				}
			};

			var adjustLeaves = new List<AdjustLeave> { new AdjustLeave
			{
				EmployeeId = employee.ID,
				ID=adjustLeaveId,
				LeaveTypeId = clId,
				NewBalance = 6
			}
			};
			AdjustLeave adjustLeaveRequest = new()
			{
				EmployeeId = employee.ID,
				LeaveTypeId = clId,
				NewBalance = newBalance
			};
			#region Mock

			var mockSet = adjustLeaves.AsQueryable().BuildMockDbSet();
			SetData.MockAdjustLeave(uow, _context, mockSet);

			var mockSetLeaveBalance = leaveBalance.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			#endregion
			//Act
			var src = new AdjustLeaveService(uow.Object);
			await src.OnBeforeAdd(adjustLeaveRequest, result);

			//Assert
			Assert.Equal(result.HasError, isValid);
		}
		[Theory]
		[InlineData(4, false)] //Initial 8 Leaves Adjusted,4 Leaves Approved,and -4 Again Adjusted Updated As +4, No exception(Sum of leaves are positive(8))
		[InlineData(-5, true)] //Initial 8 Leaves Adjusted,4 Leaves Approved,and -4 Again Adjusted Updated As -5, Throws exception(Sum of leaves are Negative(-1))
		[InlineData(3, false)] //Initial 8 Leaves Adjusted,4 Leaves Approved,and -4 Again Adjusted Updated As +3, No exception(Sum of leaves are positive(7))
		[InlineData(-3, false)]//Initial 8 Leaves Adjusted,4 Leaves Approved,and -4 Again Adjusted Updated As -3, No exception(Sum of leaves are positive(1))
		[InlineData(-6, true)] //Initial 8 Leaves Adjusted,4 Leaves Approved,and -4 Again Adjusted Updated As -6, Throw exception(Sum of leaves are Negative(-2))
		public async Task OnBeforeAdd(int newBalance, bool isValid)
		{
			Result<AdjustLeave> result = new();
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var elId = Guid.NewGuid();
			var clId = Guid.NewGuid();
			var adjustLeaveId = Guid.NewGuid();

			var leaveBalance = new List<LeaveBalance>
			{ new LeaveBalance
				{
					EmployeeId = employee.ID,
					LeaveTypeId = clId,
					Leaves = 8,
					CustomizedBalId = Guid.NewGuid(),
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					LeaveTypeId = clId,
					Leaves = -2, //Approved Leaves
					ApplyLeaveId = Guid.NewGuid(),
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					LeaveTypeId = clId,
					Leaves = -2, //Approved Leaves
					ApplyLeaveId = Guid.NewGuid(),
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					LeaveTypeId= clId,
					Leaves = -4,
					CustomizedBalId = adjustLeaveId,
				}
			};

			var adjustLeaves = new List<AdjustLeave> { new AdjustLeave
			{
				EmployeeId = employee.ID,
				ID=adjustLeaveId,
				LeaveTypeId = clId,
				NewBalance = 6
			},new AdjustLeave
			{
				EmployeeId = employee.ID,
				ID=adjustLeaveId,
				LeaveTypeId = clId,
				NewBalance = -4
			}
			};
			AdjustLeave adjustLeaveRequest = new()
			{
				ID = adjustLeaveId,
				EmployeeId = employee.ID,
				LeaveTypeId = clId,
				NewBalance = newBalance
			};
			#region Mock

			var mockSet = adjustLeaves.AsQueryable().BuildMockDbSet();
			SetData.MockAdjustLeave(uow, _context, mockSet);

			var mockSetLeaveBalance = leaveBalance.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			#endregion
			//Act
			var src = new AdjustLeaveService(uow.Object);
			await src.OnBeforeUpdate(adjustLeaveRequest, result);

			//Assert
			Assert.Equal(result.HasError, isValid);
		}
	}
}
