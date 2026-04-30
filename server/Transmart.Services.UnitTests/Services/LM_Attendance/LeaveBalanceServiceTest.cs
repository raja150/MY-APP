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
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models.LM_Attendance.List;
using TranSmart.Service.LM_Attendance;
using Xunit;

namespace Transmart.Services.UnitTests.Services.LM_Attendance
{
	public class LeaveBalanceServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly ILeaveBalanceService _service;
		private readonly EmployeeDataGenerator _employeeData;
		private readonly Mock<DbContext> _context;

		public LeaveBalanceServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_service = new LeaveBalanceService(uow.Object);
			_employeeData = new EmployeeDataGenerator();
			_context = new Mock<DbContext>();
		}
		private void LeaveBalanceMockData(int leaves, byte status)
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();

			IEnumerable<LeaveBalance> data = new List<LeaveBalance>
			{
				new LeaveBalance
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					LeaveTypeId=Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					Leaves=leaves,
					ApplyLeaveId= Guid.NewGuid()
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					ApplyLeaveId = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
					LeaveTypeId=Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					Leaves=leaves,
				}

			};

			IEnumerable<ApplyLeaveDetails> applyLeaveDetails = new List<ApplyLeaveDetails>
			{
				new ApplyLeaveDetails
				{
					ID = Guid.NewGuid(),
				},
				new ApplyLeaveDetails
				{
					ID = Guid.NewGuid(),
				}

			};
			IEnumerable<ApplyLeave> applyLeaves = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Status=(int)ApplyLeaveSts.Cancelled,
					FromDate=DateTime.Parse("2022-08-25"),
					ToDate=DateTime.Parse("2022-08-26")
				},
				new ApplyLeave
				{
					EmployeeId = employee.ID,
					ID = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
					Status=status,
					FromDate=DateTime.Parse("2022-08-17"),
					ToDate=DateTime.Parse("2022-08-21")
				}

			};

			#endregion

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSet);

			//ApplyLeave
			var mockSetApplyLeave = applyLeaves.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//ApplyLeaveDetails
			var mockSetApplyLeaveDetails = applyLeaveDetails.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveDetails(uow, _context, mockSetApplyLeaveDetails);
		}

		private void LeaveBalanceLeaveToLeaveMockData(int leaves, Guid presentLTypeID)
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();

			IEnumerable<LeaveBalance> data = new List<LeaveBalance>
			{
				new LeaveBalance
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					LeaveTypeId=presentLTypeID,
					Leaves=leaves,
					ApplyLeaveId= Guid.NewGuid()
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					ApplyLeaveId = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
					LeaveTypeId=presentLTypeID,
					Leaves=leaves,
				}

			}.AsQueryable();

			IEnumerable<ApplyLeaveDetails> applyLeaveDetails = new List<ApplyLeaveDetails>
			{
				new ApplyLeaveDetails
				{
					ID = Guid.NewGuid(),
					LeaveTypeId = presentLTypeID,
					ApplyLeaveId = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
					ApplyLeave = new ApplyLeave { EmployeeId = employee.ID, Status = (int)ApplyLeaveSts.Approved },
					LeaveDate = DateTime.Parse("2022-08-20")
				},
				new ApplyLeaveDetails
				{
					ID = Guid.NewGuid(),
				}

			}.AsQueryable();
			IEnumerable<ApplyLeave> applyLeaves = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Status=(int)ApplyLeaveSts.Cancelled,
					FromDate=DateTime.Parse("2022-08-25"),
					ToDate=DateTime.Parse("2022-08-26")
				},
				new ApplyLeave
				{
					EmployeeId = employee.ID,
					ID = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
					Status=2,
					FromDate=DateTime.Parse("2022-08-17"),
					ToDate=DateTime.Parse("2022-08-21")
				}

			}.AsQueryable();


			#endregion

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSet);

			//ApplyLeave
			var mockSetApplyLeave = applyLeaves.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//ApplyLeaveDetails
			var mockSetApplyLeaveDetails = applyLeaveDetails.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveDetails(uow, _context, mockSetApplyLeaveDetails);
		}

		private void LeaveBalanceLeavesModifyBasedOnAttendanceMOckData(int leaves)
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			IEnumerable<LeaveBalance> data = new List<LeaveBalance>
			{
				new LeaveBalance
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					LeaveTypeId=Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
					Leaves=leaves,
					ApplyLeaveId= Guid.NewGuid()
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					ApplyLeaveId = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
					LeaveTypeId=Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
					Leaves=leaves,
				}

			}.AsQueryable();

			IEnumerable<ApplyLeaveDetails> applyLeaveDetails = new List<ApplyLeaveDetails>
			{
				new ApplyLeaveDetails
				{
					ID = Guid.NewGuid(),
					LeaveTypeId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					ApplyLeaveId = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
					ApplyLeave = new ApplyLeave { EmployeeId = employee.ID, Status = (int)ApplyLeaveSts.Approved },
					LeaveDate = DateTime.Parse("2022-08-20")
				},
				new ApplyLeaveDetails
				{
					ID = Guid.NewGuid(),
				}

			}.AsQueryable();
			IEnumerable<ApplyLeave> applyLeaves = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Status=(int)ApplyLeaveSts.Cancelled,
					FromDate=DateTime.Parse("2022-08-25"),
					ToDate=DateTime.Parse("2022-08-26")
				},
				new ApplyLeave
				{
					EmployeeId = employee.ID,
					ID = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
					Status=2,
					FromDate=DateTime.Parse("2022-08-17"),
					ToDate=DateTime.Parse("2022-08-21")
				}

			}.AsQueryable();


			#endregion

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSet);

			//ApplyLeave
			var mockSetApplyLeave = applyLeaves.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//ApplyLeaveDetails
			var mockSetApplyLeaveDetails = applyLeaveDetails.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveDetails(uow, _context, mockSetApplyLeaveDetails);
		}

		[Fact]
		public async Task GetEmpLeaveBalance_GetWithEmployeeUdAndLeaveTypeId_GetValidRecords()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			var fromDate = DateTime.Today;
			var leaveTypeId = Guid.NewGuid();

			IEnumerable<LeaveBalance> data = new List<LeaveBalance>
			{
				new LeaveBalance
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					LeaveTypeId=leaveTypeId,
					EffectiveFrom = fromDate,
					EffectiveTo = fromDate.AddDays(30),
					Leaves = 0.5m,
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					LeaveTypeId=leaveTypeId,
					EffectiveFrom = fromDate.AddDays(10),
					EffectiveTo = fromDate.AddDays(30),
					Leaves=2,
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					LeaveTypeId=Guid.NewGuid(),
					EffectiveFrom = fromDate,
					EffectiveTo = fromDate.AddDays(30),
				}

			}.AsQueryable();


			#endregion

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSet);

			//Act
			var leaveBalance = await _service.GetEmpLeaveBalance(employee.ID, leaveTypeId,fromDate.AddDays(11));

			//Assert
			Assert.Equal(2, leaveBalance[0].Leaves);
			Assert.Single(leaveBalance);
		}

		[Fact]
		public async Task AddLeave_DataUpdate_WithOutException()
		{
			//Arrange
			int leaves = 19;
			byte status = 2;
			var employee = _employeeData.GetEmployeeData();
			var applyLeaveId = Guid.NewGuid();
			var leaveTypeId = Guid.NewGuid();
			var result = new Result<LeaveBalance>();

			#region Arrange

			IEnumerable<LeaveBalance> leaveBalanceData = new List<LeaveBalance>
			{
				new LeaveBalance
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					LeaveTypeId=leaveTypeId,
					Leaves=leaves,
					ApplyLeaveId= Guid.NewGuid()
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					ApplyLeaveId = applyLeaveId,
					LeaveTypeId=leaveTypeId,
					Leaves=leaves,
				}

			};

			IEnumerable<ApplyLeave> applyLeaves = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Status=(int)ApplyLeaveSts.Cancelled,
					FromDate=DateTime.Parse("2022-08-25"),
					ToDate=DateTime.Parse("2022-08-26")
				},
				new ApplyLeave
				{
					EmployeeId = employee.ID,
					ID = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
					Status=status,
					FromDate=DateTime.Parse("2022-08-17"),
					ToDate=DateTime.Parse("2022-08-21")
				}

			};

			var applyLeaveDetails = new List<ApplyLeaveDetails>
			{
				new ApplyLeaveDetails
				{
					IsHalfDay= true,
					IsFirstHalf = true,
					LeaveDate = DateTime.Now.AddDays(-2),
					LeaveTypeId = leaveTypeId,
					ApplyLeaveId = applyLeaveId,
					ApplyLeave = new ApplyLeave
					{
						ID = applyLeaveId,
						EmployeeId = employee.ID,
						Status = (int)ApplyLeaveSts.Approved,
					}
				},
				new ApplyLeaveDetails
				{
					IsHalfDay= true,
					IsFirstHalf = true,
					LeaveDate = DateTime.Now.AddDays(-1),
					LeaveTypeId = Guid.NewGuid(),
					ApplyLeaveId = Guid.NewGuid(),
					ApplyLeave = new ApplyLeave
					{
						EmployeeId = Guid.NewGuid(),
						Status = (int)ApplyLeaveSts.Approved,
					}
				}

			};

			#endregion

			//Mock
			var mockSet = leaveBalanceData.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSet);

			//ApplyLeave
			var mockSetApplyLeave = applyLeaves.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//ApplyLeaveDetails
			var mockSetApplyLeaveDetails = applyLeaveDetails.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveDetails(uow, _context, mockSetApplyLeaveDetails);

			//Act
			result = await _service.AddLeave(employee.ID, leaveTypeId, DateTime.Now.AddDays(-2), true, true, Guid.NewGuid());

			//Assert
			Assert.True(result.HasNoError);
			Assert.True(leaveBalanceData.LastOrDefault().Leaves == 18.5m);
		}

		[Fact]
		public async Task AddLeaveDataAdd_WithOutException()
		{
			//Arrange
			int leaves = 19;
			var employee = _employeeData.GetEmployeeData();
			var applyLeaveId = Guid.NewGuid();
			var leaveTypeId = Guid.NewGuid();
			var leaveBalanceLvTypeId = Guid.NewGuid();

			#region Arrange

			IEnumerable<LeaveBalance> data = new List<LeaveBalance>
			{
				new LeaveBalance
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					LeaveTypeId=leaveTypeId,
					Leaves=leaves,
					ApplyLeaveId= Guid.NewGuid()
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					ApplyLeaveId = applyLeaveId,
					LeaveTypeId=leaveBalanceLvTypeId,
					Leaves=leaves,
				}

			};

			//IEnumerable<ApplyLeave> applyLeaves = new List<ApplyLeave>
			//{
			//	new ApplyLeave
			//	{
			//		EmployeeId = Guid.NewGuid(),
			//		ID = Guid.NewGuid(),
			//		LeaveTypeId=Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
			//		Status=(int)ApplyLeaveSts.Cancelled,
			//		FromDate=DateTime.Parse("2022-08-25"),
			//		ToDate=DateTime.Parse("2022-08-26")
			//	},
			//	new ApplyLeave
			//	{
			//		EmployeeId = employee.ID,
			//		ID = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
			//		LeaveTypeId=Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
			//		Status=status,
			//		FromDate=DateTime.Parse("2022-08-17"),
			//		ToDate=DateTime.Parse("2022-08-21")
			//	}

			//};
			var applyLeaves = new List<ApplyLeave>();

			var applyLeaveDetails = new List<ApplyLeaveDetails>
			{
				new ApplyLeaveDetails
				{
					IsHalfDay= true,
					IsFirstHalf = true,
					LeaveDate = DateTime.Now.AddDays(-2),
					LeaveTypeId = leaveTypeId,
					ApplyLeaveId = applyLeaveId,
					ApplyLeave = new ApplyLeave
					{
						ID = applyLeaveId,
						EmployeeId = employee.ID,
						Status = (int)ApplyLeaveSts.Approved,
					}
				},
				new ApplyLeaveDetails
				{
					IsHalfDay= true,
					IsFirstHalf = true,
					LeaveDate = DateTime.Now.AddDays(-1),
					LeaveTypeId = Guid.NewGuid(),
					ApplyLeaveId = Guid.NewGuid(),
					ApplyLeave = new ApplyLeave
					{
						EmployeeId = Guid.NewGuid(),
						Status = (int)ApplyLeaveSts.Approved,
					}
				}

			};

			#endregion

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSet);

			//ApplyLeave
			var mockSetApplyLeave = applyLeaves.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//ApplyLeaveDetails
			var mockSetApplyLeaveDetails = applyLeaveDetails.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveDetails(uow, _context, mockSetApplyLeaveDetails);


			//Act
			var leaveBalance = await _service.AddLeave(employee.ID, leaveBalanceLvTypeId, DateTime.Now.AddDays(-2), true, false, Guid.NewGuid());

			//Assert
			Assert.True(leaveBalance.HasNoError);
		}

		[Fact]
		public async Task AddLeave_EmployeeDoesNotHaveEnoughLeaves_ThrowException()
		{
			//Arrange
			int leaves = 0;
			byte status = 0;
			var employee = _employeeData.GetEmployeeData();

			LeaveBalanceMockData(leaves, status);

			//Act
			var leaveBalance = await _service.AddLeave(employee.ID, Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"), DateTime.Parse("2022-08-20"), true, false, Guid.NewGuid());

			//Assert
			Assert.False(leaveBalance.HasNoError);
		}

		[Fact]
		public async Task UpdateLeave_DataUpdate_WithOutException()
		{
			//Arrange

			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			var applyLeaveId = Guid.NewGuid();
			var leaveTypeId = Guid.NewGuid();

			IEnumerable<LeaveBalance> LeaveBalanceData = new List<LeaveBalance>
			{
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					ApplyLeaveId = applyLeaveId,
					LeaveTypeId=leaveTypeId,
					Leaves=2,
				}

			};

			var applyLeaveDetails = new List<ApplyLeaveDetails>
			{
				new ApplyLeaveDetails
				{
					IsHalfDay= true,
					IsFirstHalf = true,
					LeaveDate = DateTime.Now.AddDays(-2),
					LeaveTypeId = leaveTypeId,
					ApplyLeaveId = applyLeaveId,
					ApplyLeave = new ApplyLeave
					{
						ID = applyLeaveId,
						EmployeeId = employee.ID,
						Status = (int)ApplyLeaveSts.Approved,
					}
				},
				new ApplyLeaveDetails
				{
					IsHalfDay= true,
					IsFirstHalf = true,
					LeaveDate = DateTime.Now.AddDays(-1),
					LeaveTypeId = Guid.NewGuid(),
					ApplyLeaveId = Guid.NewGuid(),
					ApplyLeave = new ApplyLeave
					{
						EmployeeId = Guid.NewGuid(),
						Status = (int)ApplyLeaveSts.Approved,
					}
				}

			};

			IEnumerable<ApplyLeave> applyLeaves = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Status=(int)ApplyLeaveSts.Cancelled,
					FromDate=DateTime.Parse("2022-08-25"),
					ToDate=DateTime.Parse("2022-08-26")
				},
				new ApplyLeave
				{
					EmployeeId = employee.ID,
					ID = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
					Status=(int)ApplyLeaveSts.Approved,
					FromDate=DateTime.Parse("2022-08-17"),
					ToDate=DateTime.Parse("2022-08-21")
				}

			};

			#endregion

			//Mock
			var mockSet = LeaveBalanceData.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSet);

			//ApplyLeave
			var mockSetApplyLeave = applyLeaves.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//ApplyLeaveDetails
			var mockSetApplyLeaveDetails = applyLeaveDetails.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveDetails(uow, _context, mockSetApplyLeaveDetails);

			//Act
			var leaveBalance = await _service.UpdateLeave(employee.ID, leaveTypeId, DateTime.Now.AddDays(-2), true);

			//Assert
			Assert.True(leaveBalance.HasNoError);
			Assert.True(LeaveBalanceData.FirstOrDefault().Leaves == 2.5m);
		}

		[Fact]
		public async Task UpdateLeave_PreviouslyDidNotHaveAppliedLeave_ThrowException()
		{
			#region Arrange
			Result<LeaveBalance> result = new();
			var employee = _employeeData.GetEmployeeData();
			var applyLeaveId = Guid.NewGuid();
			var leaveTypeId = Guid.NewGuid();

			IEnumerable<LeaveBalance> data = new List<LeaveBalance>
			{
				new LeaveBalance
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					LeaveTypeId=Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					Leaves=2,
					ApplyLeaveId= Guid.NewGuid()
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					ApplyLeaveId = applyLeaveId,
					LeaveTypeId=leaveTypeId,
					Leaves=0,
				}

			};

			var applyLeaveDetails = new List<ApplyLeaveDetails>
			{
				new ApplyLeaveDetails
				{
					IsHalfDay= true,
					IsFirstHalf = true,
					LeaveDate = DateTime.Now.AddDays(-2),
					LeaveTypeId = leaveTypeId,
					ApplyLeaveId = applyLeaveId,
					ApplyLeave = new ApplyLeave
					{
						ID = applyLeaveId,
						EmployeeId = employee.ID,
						Status = (int)ApplyLeaveSts.Approved,
					}
				},
				new ApplyLeaveDetails
				{
					IsHalfDay= true,
					IsFirstHalf = true,
					LeaveDate = DateTime.Now.AddDays(-1),
					LeaveTypeId = Guid.NewGuid(),
					ApplyLeaveId = Guid.NewGuid(),
					ApplyLeave = new ApplyLeave
					{
						EmployeeId = Guid.NewGuid(),
						Status = (int)ApplyLeaveSts.Approved,
					}
				}

			};

			IEnumerable<ApplyLeave> applyLeaves = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Status=(int)ApplyLeaveSts.Cancelled,
					FromDate=DateTime.Parse("2022-08-25"),
					ToDate=DateTime.Parse("2022-08-26")
				},
				new ApplyLeave
				{
					EmployeeId = employee.ID,
					ID = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
					Status=(int)ApplyLeaveSts.Approved,
					FromDate=DateTime.Parse("2022-08-17"),
					ToDate=DateTime.Parse("2022-08-21")
				}

			};

			#endregion

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSet);

			//ApplyLeave
			var mockSetApplyLeave = applyLeaves.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//ApplyLeaveDetails
			var mockSetApplyLeaveDetails = applyLeaveDetails.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveDetails(uow, _context, mockSetApplyLeaveDetails);

			//Act
			result = await _service.UpdateLeave(employee.ID, leaveTypeId, DateTime.Now.AddDays(-2), true);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public async Task UpdateLeave_EmployeeDoesNotHaveEnoughLeaves_throwException()
		{
			#region Arrange
			Result<LeaveBalance> result = new();
			var employee = _employeeData.GetEmployeeData();
			var applyLeaveId = Guid.NewGuid();
			var leaveTypeId = Guid.NewGuid();

			IEnumerable<LeaveBalance> data = new List<LeaveBalance>
			{
				new LeaveBalance
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					LeaveTypeId=Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					Leaves=2,
					ApplyLeaveId= Guid.NewGuid()
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					ApplyLeaveId = applyLeaveId,
					LeaveTypeId=leaveTypeId,
					Leaves=-2,
				}

			};

			var applyLeaveDetails = new List<ApplyLeaveDetails>
			{
				new ApplyLeaveDetails
				{
					IsHalfDay= true,
					IsFirstHalf = true,
					LeaveDate = DateTime.Now.AddDays(-2),
					LeaveTypeId = leaveTypeId,
					ApplyLeaveId = applyLeaveId,
					ApplyLeave = new ApplyLeave
					{
						ID = applyLeaveId,
						EmployeeId = employee.ID,
						Status = (int)ApplyLeaveSts.Approved,
					}
				},
				new ApplyLeaveDetails
				{
					IsHalfDay= true,
					IsFirstHalf = true,
					LeaveDate = DateTime.Now.AddDays(-1),
					LeaveTypeId = Guid.NewGuid(),
					ApplyLeaveId = Guid.NewGuid(),
					ApplyLeave = new ApplyLeave
					{
						EmployeeId = Guid.NewGuid(),
						Status = (int)ApplyLeaveSts.Approved,
					}
				}

			};

			IEnumerable<ApplyLeave> applyLeaves = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Status=(int)ApplyLeaveSts.Cancelled,
					FromDate=DateTime.Parse("2022-08-25"),
					ToDate=DateTime.Parse("2022-08-26")
				},
				new ApplyLeave
				{
					EmployeeId = employee.ID,
					ID = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
					Status=(int)ApplyLeaveSts.Approved,
					FromDate=DateTime.Parse("2022-08-17"),
					ToDate=DateTime.Parse("2022-08-21")
				}

			};

			#endregion

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSet);

			//ApplyLeave
			var mockSetApplyLeave = applyLeaves.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//ApplyLeaveDetails
			var mockSetApplyLeaveDetails = applyLeaveDetails.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveDetails(uow, _context, mockSetApplyLeaveDetails);

			//Act
			result = await _service.UpdateLeave(employee.ID, leaveTypeId, DateTime.Now.AddDays(-2), true);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public async Task UpdateLeave_PreviouslyDidNotHaveAnyAppliedLeave_ThrowException()
		{
			#region Arrange
			Result<LeaveBalance> result = new();
			var employee = _employeeData.GetEmployeeData();
			var applyLeaveId = Guid.NewGuid();
			var leaveTypeId = Guid.NewGuid();

			IEnumerable<LeaveBalance> data = new List<LeaveBalance>
			{
				new LeaveBalance
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					LeaveTypeId=Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					Leaves=2,
					ApplyLeaveId= Guid.NewGuid()
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					ApplyLeaveId = applyLeaveId,
					LeaveTypeId=leaveTypeId,
					Leaves=-2,
				}

			};

			var applyLeaveDetails = new List<ApplyLeaveDetails>
			{
				new ApplyLeaveDetails
				{
					IsHalfDay= true,
					IsFirstHalf = true,
					LeaveDate = DateTime.Now.AddDays(-2),
					LeaveTypeId = leaveTypeId,
					ApplyLeaveId = applyLeaveId,
					ApplyLeave = new ApplyLeave
					{
						ID = applyLeaveId,
						EmployeeId = employee.ID,
						Status = (int)ApplyLeaveSts.Approved,
					}
				},
				new ApplyLeaveDetails
				{
					IsHalfDay= true,
					IsFirstHalf = true,
					LeaveDate = DateTime.Now.AddDays(-1),
					LeaveTypeId = Guid.NewGuid(),
					ApplyLeaveId = Guid.NewGuid(),
					ApplyLeave = new ApplyLeave
					{
						EmployeeId = Guid.NewGuid(),
						Status = (int)ApplyLeaveSts.Approved,
					}
				}

			};

			IEnumerable<ApplyLeave> applyLeaves = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Status=(int)ApplyLeaveSts.Cancelled,
					FromDate=DateTime.Parse("2022-08-25"),
					ToDate=DateTime.Parse("2022-08-26")
				},
				new ApplyLeave
				{
					EmployeeId = employee.ID,
					ID = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
					Status=(int)ApplyLeaveSts.Approved,
					FromDate=DateTime.Parse("2022-08-17"),
					ToDate=DateTime.Parse("2022-08-21")
				}

			};

			#endregion

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSet);

			//ApplyLeave
			var mockSetApplyLeave = applyLeaves.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//ApplyLeaveDetails
			var mockSetApplyLeaveDetails = applyLeaveDetails.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveDetails(uow, _context, mockSetApplyLeaveDetails);

			//Act

			result = await _service.UpdateLeave(employee.ID, Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"), DateTime.Parse("2022-08-20"), true);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public async Task LeaveToLeave_DataSaved_WithOutException()
		{
			//Arrange
			int leaves = 19;
			Guid presentLTypeID = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6");
			var employee = _employeeData.GetEmployeeData();

			LeaveBalanceLeaveToLeaveMockData(leaves, presentLTypeID);

			//Act
			var leaveBalance = await _service.LeaveToLeave(employee.ID, presentLTypeID, Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"), DateTime.Parse("2022-08-20"), true, false, true, Guid.NewGuid());

			//Assert
			Assert.True(leaveBalance.HasNoError);
		}

		[Fact]
		public async Task LeaveToLeave_DataUpdated_WithOutException()
		{
			//Arrange
			int leaves = 19;
			Guid presentLTypeID = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6");
			var employee = _employeeData.GetEmployeeData();

			LeaveBalanceLeaveToLeaveMockData(leaves, presentLTypeID);

			//Act
			var leaveBalance = await _service.LeaveToLeave(employee.ID, presentLTypeID, Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"), DateTime.Parse("2022-08-20"), false, true, true, Guid.NewGuid());

			//Assert
			Assert.True(leaveBalance.HasNoError);
		}

		[Fact]
		public async Task LeaveToLeave_DataAdded_WithOutException()
		{
			//Arrange
			int leaves = 19;
			Guid presentLTypeID = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6");
			var employee = _employeeData.GetEmployeeData();

			LeaveBalanceLeaveToLeaveMockData(leaves, presentLTypeID);

			//Act
			var leaveBalance = await _service.LeaveToLeave(employee.ID, presentLTypeID, Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"), DateTime.Parse("2022-08-20"), true, false, true, Guid.NewGuid());

			//Assert
			Assert.True(leaveBalance.HasNoError);
		}

		[Fact]
		public async Task LeaveToLeave_PreviouslyDidNotHaveAppliedLeave_ThrowException()
		{
			//Arrange
			int leaves = 0;
			Guid presentLTypeID = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6");
			var employee = _employeeData.GetEmployeeData();

			LeaveBalanceLeaveToLeaveMockData(leaves, presentLTypeID);

			//Act
			var leaveBalance = await _service.LeaveToLeave(employee.ID, presentLTypeID, Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"), DateTime.Parse("2022-08-20"), true, false, false, Guid.NewGuid());

			//Assert
			Assert.False(leaveBalance.HasNoError);
		}

		[Fact]
		public async Task LeavesModifyBasedOnAttendance_LeaveToLeaveDataSaved_WithOutException()
		{
			//Arrange
			int leaves = 19;
			var employee = _employeeData.GetEmployeeData();
			Attendance presentAttendance = new()
			{
				EmployeeId = employee.ID,
				LeaveTypeID = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
				AttendanceDate = DateTime.Parse("2022-08-20"),
				IsHalfDay = true,
				AttendanceStatus = 8,
				HalfDayType = 8
			};
			AttendanceDetails requiredAttendance = new()
			{
				EmployeeId = employee.ID,
				LeaveTypeId = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
				AttendanceStatus = "8",
				IsHalfDay = false,
				IsFirstOff = true,
				HalfDayType = 3
			};

			LeaveBalanceLeavesModifyBasedOnAttendanceMOckData(leaves);

			//Act
			var leaveBalance = await _service.LeavesModifyBasedOnAttendance(presentAttendance, requiredAttendance, Guid.NewGuid());

			//Assert
			Assert.True(leaveBalance.HasNoError);
		}

		[Fact]
		public async Task LeavesModifyBasedOnAttendance_UpdateLeaveDataUpdated_WithOutException()
		{
			//Arrange
			int leaves = 19;
			var employee = _employeeData.GetEmployeeData();
			Attendance presentAttendance = new()
			{
				EmployeeId = employee.ID,
				LeaveTypeID = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
				AttendanceDate = DateTime.Parse("2022-08-20"),
				IsHalfDay = true,
				AttendanceStatus = 3,
				HalfDayType = 2
			};
			AttendanceDetails requiredAttendance = new()
			{
				EmployeeId = employee.ID,
				LeaveTypeId = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
				AttendanceStatus = "5",
				IsHalfDay = false,
				IsFirstOff = true,
				HalfDayType = 1
			};

			LeaveBalanceLeavesModifyBasedOnAttendanceMOckData(leaves);

			//Act
			var leaveBalance = await _service.LeavesModifyBasedOnAttendance(presentAttendance, requiredAttendance, Guid.NewGuid());

			//Assert
			Assert.True(leaveBalance.HasNoError);
		}

		[Fact]
		public async Task LeavesModifyBasedOnAttendance_PreviouslyDidNotHaveAppliedLeave_ThrowException()
		{
			//Arrange
			int leaves = 0;
			var employee = _employeeData.GetEmployeeData();
			Attendance presentAttendance = new()
			{
				EmployeeId = employee.ID,
				LeaveTypeID = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
				AttendanceDate = DateTime.Parse("2022-08-20"),
				IsHalfDay = true,
				AttendanceStatus = 3,
				HalfDayType = 2
			};
			AttendanceDetails requiredAttendance = new()
			{
				EmployeeId = employee.ID,
				LeaveTypeId = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
				AttendanceStatus = "5",
				IsHalfDay = false,
				IsFirstOff = true,
				HalfDayType = 2
			};
			LeaveBalanceLeavesModifyBasedOnAttendanceMOckData(leaves);

			//Act
			var leaveBalance = await _service.LeavesModifyBasedOnAttendance(presentAttendance, requiredAttendance, Guid.NewGuid());

			//Assert
			Assert.False(leaveBalance.HasNoError);
		}

		[Fact]
		public async Task LeavesModifyBasedOnAttendance_DataSaved_WithOutException()
		{
			//Arrange
			int leaves = 19;
			var employee = _employeeData.GetEmployeeData();
			Attendance presentAttendance = new()
			{
				EmployeeId = employee.ID,
				LeaveTypeID = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
				AttendanceDate = DateTime.Parse("2022-08-20"),
				IsHalfDay = true,
				AttendanceStatus = 2,
				HalfDayType = 1
			};
			AttendanceDetails requiredAttendance = new()
			{
				EmployeeId = employee.ID,
				LeaveTypeId = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
				AttendanceStatus = "1",
				IsHalfDay = false,
				IsFirstOff = true,
				HalfDayType = 2
			};

			LeaveBalanceLeavesModifyBasedOnAttendanceMOckData(leaves);

			//Act
			var leaveBalance = await _service.LeavesModifyBasedOnAttendance(presentAttendance, requiredAttendance, Guid.NewGuid());

			//Assert
			Assert.True(leaveBalance.HasNoError);
		}

		[Fact]
		public async Task LeavesModifyBasedOnAttendance_DataAdded_WithOutException()
		{
			//Arrange
			int leaves = 19;
			var employee = _employeeData.GetEmployeeData();
			Attendance presentAttendance = new()
			{
				EmployeeId = employee.ID,
				LeaveTypeID = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
				AttendanceDate = DateTime.Parse("2022-08-20"),
				IsHalfDay = true,
				AttendanceStatus = 2,
				HalfDayType = 1
			};
			AttendanceDetails requiredAttendance = new()
			{
				EmployeeId = employee.ID,
				LeaveTypeId = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
				AttendanceStatus = "1",
				IsHalfDay = false,
				IsFirstOff = true,
				HalfDayType = 8
			};
			LeaveBalanceLeavesModifyBasedOnAttendanceMOckData(leaves);

			//Act
			var leaveBalance = await _service.LeavesModifyBasedOnAttendance(presentAttendance, requiredAttendance, Guid.NewGuid());

			//Assert
			Assert.True(leaveBalance.HasNoError);
		}

		[Fact]
		public async Task LeavesModifyBasedOnAttendance_EmployeeDoesNotHaveEnoughLeaves_ThrowException()
		{
			//Arrange
			int leaves = 0;
			var employee = _employeeData.GetEmployeeData();
			Attendance presentAttendance = new()
			{
				EmployeeId = employee.ID,
				LeaveTypeID = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
				AttendanceDate = DateTime.Parse("2022-08-20"),
				IsHalfDay = true,
				AttendanceStatus = 2,
				HalfDayType = 1
			};
			AttendanceDetails requiredAttendance = new()
			{
				EmployeeId = employee.ID,
				LeaveTypeId = Guid.Parse("add2fa1c-9310-4298-9d4a-0035b96edff6"),
				AttendanceStatus = "1",
				IsHalfDay = false,
				IsFirstOff = true,
				HalfDayType = 8
			};
			LeaveBalanceLeavesModifyBasedOnAttendanceMOckData(leaves);

			//Act
			var leaveBalance = await _service.LeavesModifyBasedOnAttendance(presentAttendance, requiredAttendance, Guid.NewGuid());

			//Assert
			Assert.False(leaveBalance.HasNoError);
		}
	}
}
