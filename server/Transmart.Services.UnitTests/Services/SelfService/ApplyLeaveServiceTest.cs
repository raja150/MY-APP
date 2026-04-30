using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Leave.ApplyLeaveData;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Data.Repository.Attendance;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Service.Leave;
using TranSmart.Service.LM_Attendance;
using TranSmart.Service.Schedules;
using Xunit;

namespace Transmart.Services.UnitTests.Services.SelfService
{
	public class ApplyLeaveServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly EmployeeDataGenerator _employeeData;
		private readonly IApplyLeaveService _service;
		private readonly IScheduleService _scheduleService;
		private readonly IShiftService _shiftService;
		private readonly ILeaveBalanceService _leaveBalanceService;
		private readonly IAttendanceRepository _attRepository;
		private readonly IAttendanceService _attService;
		private readonly Mock<DbContext> _context;
		public ApplyLeaveServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_leaveBalanceService = new LeaveBalanceService(uow.Object);
			_attRepository = new AttendanceRepository(_dbContext.Object);
			_shiftService = new ShiftService(uow.Object);
			_scheduleService = new ScheduleService(uow.Object, _shiftService);
			_attService = new AttendanceService(uow.Object, _scheduleService, _leaveBalanceService, _attRepository);
			_employeeData = new EmployeeDataGenerator();
			_service = new ApplyLeaveService(uow.Object, _attService, _leaveBalanceService);
			_context = new Mock<DbContext>();
		}
		[Fact]
		public async Task SelfServiceSearch_Pagination_ReturnList_Approved()
		{
			var approverId = Guid.NewGuid();
			var applyLeaveSearch = new ApplyLeaveSearch
			{
				Name = "chishtheshwar",
				FromDate = DateTime.Parse("2022-07-1"),
				ToDate = DateTime.Parse("2022-07-30"),
				Status = (int)ApplyLeaveSts.Cancelled,
				SortBy = "Employee",
				Size = 10,
				Page = 0,
				RefId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6")
			};
			var employee = new Employee
			{
				ID = Guid.NewGuid(),
				AadhaarNumber = "12131231232",
				DateOfJoining = DateTime.Parse("2019-12-30"),
				Gender = 1,
				Name = "chishtheshwar",
				No = "Avontix1796",
				DesignationId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6")
			};

			IEnumerable<ApplyLeave> applyLeaveData = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					EmployeeId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					Employee = new Employee{Name = "chishtheshwar"},
					ID = Guid.NewGuid(),
					FromDate = DateTime.Parse("2022-07-13"),
					ToDate = DateTime.Parse("2022-07-15"),
					Status = 1,
					ApprovedById = approverId,
					ApprovedBy = new Employee
					{
						ID = Guid.NewGuid(),
						Name = "Chishtheshwar",
						No = "Avontix1827"
					}
				},
				new ApplyLeave
				{
					EmployeeId = employee.ID,
					Employee = new Employee{Name = "chishtheshwar"},
					ID = Guid.NewGuid(),
					FromDate = DateTime.Parse("2022-07-15"),
					ToDate = DateTime.Parse("2022-07-15"),
					Status = 1,
				}

			}.AsQueryable();

			//ApplyLeave Mock
			var applyLeaveSet = applyLeaveData.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<ApplyLeave>()).Returns(applyLeaveSet.Object);
			var applyLeaveRepository = new RepositoryAsync<ApplyLeave>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ApplyLeave>()).Returns(applyLeaveRepository);

			// Act
			var applyLeave = await _service.SelfServiceSearch(applyLeaveSearch);
			//Assert.Equal(applyLeave)
			Assert.Equal(approverId, applyLeave.Items[0].ApprovedById);
			Assert.Equal("Chishtheshwar", applyLeave.Items[0].ApprovedBy.Name);
			Assert.True(applyLeave.Count == 1);
		}

		[Fact]
		public async Task SelfServiceSearch_Pagination_ReturnList_Rejected()
		{
			var approverId = Guid.NewGuid();
			var applyLeaveSearch = new ApplyLeaveSearch
			{
				Name = "chishtheshwar",
				FromDate = DateTime.Parse("2022-07-1"),
				ToDate = DateTime.Parse("2022-07-30"),
				Status = (int)ApplyLeaveSts.Cancelled,
				SortBy = "Employee",
				Size = 10,
				Page = 0,
				RefId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6")
			};
			var employee = new Employee
			{
				ID = Guid.NewGuid(),
				AadhaarNumber = "12131231232",
				DateOfJoining = DateTime.Parse("2019-12-30"),
				Gender = 1,
				Name = "chishtheshwar",
				No = "Avontix1796",
				DesignationId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6")
			};

			IEnumerable<ApplyLeave> applyLeaveData = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					EmployeeId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					Employee = new Employee{Name = "chishtheshwar"},
					ID = Guid.NewGuid(),
					FromDate = DateTime.Parse("2022-07-13"),
					ToDate = DateTime.Parse("2022-07-15"),
					Status = (int)ApplyLeaveSts.Rejected,
					Reason = "Rejected due to deployment",
					ApprovedById = approverId,
					ApprovedBy = new Employee
					{
						ID = Guid.NewGuid(),
						Name = "Chishtheshwar",
						No = "Avontix1827"
					}
				},
				new ApplyLeave
				{
					EmployeeId = employee.ID,
					Employee = new Employee{Name = "chishtheshwar"},
					ID = Guid.NewGuid(),
					FromDate = DateTime.Parse("2022-07-15"),
					ToDate = DateTime.Parse("2022-07-15"),
					Status = 1,
				}

			}.AsQueryable();

			//ApplyLeave Mock
			var applyLeaveSet = applyLeaveData.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<ApplyLeave>()).Returns(applyLeaveSet.Object);
			var applyLeaveRepository = new RepositoryAsync<ApplyLeave>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ApplyLeave>()).Returns(applyLeaveRepository);

			// Act
			var applyLeave = await _service.SelfServiceSearch(applyLeaveSearch);
			//Assert.Equal(applyLeave)
			Assert.Equal((int)ApplyLeaveSts.Rejected, applyLeave.Items[0].Status);
			Assert.Equal(approverId, applyLeave.Items[0].ApprovedById);
			Assert.Equal("Chishtheshwar", applyLeave.Items[0].ApprovedBy.Name);
			Assert.True(applyLeave.Count == 1);
		}

		[Fact]
		public void ApplyLeaves_Dates_Validation_Test() //Apply leave fromDate More then the ToDate Test
		{
			#region Arrange
			DateTime fromDate = DateTime.Now.AddDays(3);
			DateTime toDate = DateTime.Now.AddDays(1);
			var employee = _employeeData.GetEmployeeData();
			var lvTypeId = Guid.NewGuid();

			var leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = lvTypeId,
					Code = "EL",
					Name = "Earned Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 12,
					specifiedperio = 1,
				}
			}.AsQueryable();

			#endregion

			#region Mock
			//LeaveType Mock 
			var mockSetLeaveType = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeaveType);

			//Employee Mock
			var mockSetEmployee = _employeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			#endregion
			//Act
			var dd = _service.AddAsync(new ApplyLeave
			{
				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				FromDate = fromDate,
				ToDate = toDate,
				Reason = "Corona",
				EmergencyContNo = "9392440499",
				LeaveTypes = "[{\"name\":\"Earned Leave\",\"days\":2}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					LeaveTypeId = lvTypeId,
					NoOfLeaves= 2,
				} }
			});
			//Assert
			Assert.False(dd.Result.IsSuccess);
		}

		[Fact]
		public async Task MobileNumber_Validation_Test()
		{
			// Arrange
			DateTime fromDate = DateTime.Now.AddDays(1);
			DateTime toDate = DateTime.Now.AddDays(2);
			var mobileNum = "939244049";
			var employee = _employeeData.GetEmployeeData();

			var lvTypeId = Guid.NewGuid();
			var leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = lvTypeId,
					Code = "EL",
					Name = "Earned Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 12,
					specifiedperio = 1,
				}
			}.AsQueryable();

			#region Mock
			//LeaveType Mock 
			var mockSetLeaveType = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeaveType);

			//Employee Mock
			var mockSetEmployee = _employeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			#endregion
			//Act
			var dd = await _service.AddAsync(new ApplyLeave
			{
				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				FromDate = fromDate,
				ToDate = toDate,
				Reason = "Corona",
				EmergencyContNo = mobileNum,
				LeaveTypes = "[{\"name\":\"Earned Leave\",\"days\":2}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					LeaveTypeId = lvTypeId,
					NoOfLeaves= 2,
				} }
			});
			//Assert
			Assert.False(dd.IsSuccess);
		}

		[Fact]
		public async Task HalfDay_Leave_HalfDay_WFH_Valid_Test()
		{
			#region Arrange
			DateTime fromDate = DateTime.Now.AddDays(1);
			DateTime toDate = DateTime.Now.AddDays(2);
			var employee = _employeeData.GetEmployeeData();
			var lvTypeId = Guid.NewGuid();
			var applyLeaveId = Guid.NewGuid();

			var leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = lvTypeId,
					Code = "EL",
					Name = "Earned Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 12,
					specifiedperio = 1,
				}
			}.AsQueryable();

			var applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.ID,
				  FromDate = DateTime.Now.Date,
				  ToDate = DateTime.Now.Date,
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Approved
				},
			}.AsQueryable();

			var applyLeaveType = new List<ApplyLeaveType>
			{
				new ApplyLeaveType
				{
					ID = Guid.NewGuid(),
					LeaveTypeId = lvTypeId,
					ApplyLeave = new ApplyLeave
					{
						ID = applyLeaveId,
						EmployeeId = employee.ID,
						FromDate = DateTime.Now.Date,
						ToDate = DateTime.Now.Date,
						Reason = "Corona",
						EmergencyContNo = "9381742192"
					}
				}
			};

			var wfh = new List<ApplyWfh> { new ApplyWfh
			{
				EmployeeId = employee.ID,
				FromDateC = fromDate,
				FromHalf = false,
				ToDateC = toDate,
				ToHalf = true,
				ReasonForWFH = "due to health",
				Status = (int)WfhStatus.Applied
			} };

			var applyClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				EmployeeId= employee.ID,
				FromDate = DateTime.Now.AddDays(3),
				ToDate = DateTime.Now.AddDays(4),
				Status = (int)ClientVisitStatus.Rejected
			} };

			DateTime today = DateTime.Today;
			DateTime start = new DateTime(today.Year, today.Month, 26);
			DateTime end = start.AddMonths(1).AddDays(-1);
			var leaveBalances = new List<LeaveBalance>
				{
					new LeaveBalance
					{
						ID=Guid.NewGuid(),
						Leaves=5,
						EmployeeId=employee.ID,
						LeaveTypeId=lvTypeId,
						EffectiveFrom = start,
						EffectiveTo = end,
					},
				};

			#endregion

			#region Mock
			//LeaveType Mock 
			var mockSetLeaveType = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeaveType);

			//Apply Leave Mock 
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var MockApplyLeaveType = applyLeaveType.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveType(uow, _context, MockApplyLeaveType);

			var mockClientVisit = applyClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockClientVisit(uow, _context, mockClientVisit);

			var mockWFH = wfh.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWFH);

			//LeaveBalance Mock
			var mockSetLeaveBalance = leaveBalances.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockSetLeaveBalance);

			//Employee Mock
			var mockSetEmployee = _employeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			#endregion
			//Act
			var dd = await _service.AddAsync(new ApplyLeave
			{
				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				FromDate = fromDate,
				FromHalf = true,
				ToDate = toDate,
				ToHalf = false,
				Reason = "Corona",
				EmergencyContNo = "9392440422",
				LeaveTypes = "[{\"name\":\"Earned Leave\",\"days\":2}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					LeaveTypeId = lvTypeId,
					NoOfLeaves= 2,
				} }
			});
			//Assert
			Assert.Equal((int)ApplyLeaveSts.Applied, dd.ReturnValue.Status);
			Assert.True(dd.IsSuccess);
		}

		[Fact]
		public async Task HalfDay_Leave_Same_HalfDay_WFH_Throw_Exception_Test()
		{
			#region Arrange
			DateTime fromDate = DateTime.Now.AddDays(1);
			DateTime toDate = DateTime.Now.AddDays(2);
			var employee = _employeeData.GetEmployeeData();
			var lvTypeId = Guid.NewGuid();
			var applyLeaveId = Guid.NewGuid();

			var leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = lvTypeId,
					Code = "EL",
					Name = "Earned Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 2,
					FutureDate = 5,
					MaxApplications = 1,
					MinLeaves = 1,
					MaxLeaves = 12,
					specifiedperio = 1,
				}
			}.AsQueryable();

			var applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
				  ID = applyLeaveId,
				  EmployeeId = employee.ID,
				  FromDate = DateTime.Now.Date,
				  ToDate = DateTime.Now.Date,
				  Reason = "Corona",
				  EmergencyContNo = "9381742192",
				  Status = (int)ApplyLeaveSts.Approved
				},
			}.AsQueryable();

			var applyLeaveType = new List<ApplyLeaveType>
			{
				new ApplyLeaveType
				{
					ID = Guid.NewGuid(),
					LeaveTypeId = lvTypeId,
					ApplyLeave = new ApplyLeave
					{
						ID = applyLeaveId,
						EmployeeId = employee.ID,
						FromDate = DateTime.Now.Date,
						ToDate = DateTime.Now.Date,
						Reason = "Corona",
						EmergencyContNo = "9381742192"
					}
				}
			};

			var wfh = new List<ApplyWfh> { new ApplyWfh
			{
				EmployeeId = employee.ID,
				FromDateC = fromDate,
				FromHalf = true,
				ToDateC = toDate,
				ToHalf = false,
				ReasonForWFH = "due to health",
				Status = (int)WfhStatus.Applied
			} };

			#endregion

			#region Mock
			//LeaveType Mock 
			var mockSetLeaveType = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, mockSetLeaveType);

			//Apply Leave Mock 
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			var MockApplyLeaveType = applyLeaveType.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveType(uow, _context, MockApplyLeaveType);

			var mockWFH = wfh.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWFH);

			//Employee Mock
			var mockSetEmployee = _employeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);

			#endregion
			//Act
			var dd = await _service.AddAsync(new ApplyLeave
			{
				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				FromDate = fromDate,
				FromHalf = true,
				ToDate = toDate,
				ToHalf = false,
				Reason = "Corona",
				EmergencyContNo = "9392440422",
				LeaveTypes = "[{\"name\":\"Earned Leave\",\"days\":2}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					LeaveTypeId = lvTypeId,
					NoOfLeaves= 2,
				} }
			});
			//Assert
			Assert.False(dd.IsSuccess);
		}
		[Fact]
		public async Task GetEmployeeLeaveTypes_Test()
		{
			//Arrange
			DateTime start = DateTime.Today;
			DateTime end = start.AddMonths(1).AddDays(-1);
			var employee = _employeeData.GetEmployeeData();
			var clLeaveTypeId = Guid.NewGuid();
			var slLeaveTypeId = Guid.NewGuid();
			var leaveBalance = new List<LeaveBalance>
			{
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					LeaveTypeId = clLeaveTypeId,
					LeaveType = new LeaveType{Name = "casual Lave"},
					Leaves = 2,
					CustomizedBalId = Guid.NewGuid(),
					EffectiveFrom = start,
					EffectiveTo = end,
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					LeaveTypeId= slLeaveTypeId,
					LeaveType = new LeaveType{Name = "sick Leave"},
					Leaves = 4,
					CustomizedBalId = Guid.NewGuid(),
					EffectiveFrom = start,
					EffectiveTo = end,
				},
				new LeaveBalance
				{
					EmployeeId = Guid.NewGuid(),
					LeaveTypeId= slLeaveTypeId,
					LeaveType = new LeaveType{Name = "Earned Leave"},
					Leaves = 4,
					CustomizedBalId = Guid.NewGuid(),
					EffectiveFrom = start,
					EffectiveTo = end,
				}
			};
			//Mock
			var mockLeaveBalance = leaveBalance.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockLeaveBalance);

			//Act
			var dd = await _service.GetEmployeeLeaveTypes(employee.ID);
			//assert
			Assert.Equal(2, dd.Count());
			Assert.Equal(dd, dd);
		}

		[Fact]
		public async Task UpdateLBIfLeaveApproved_Sandwich_Policy_Verification()
		{
			//Arrange
			DateTime start = DateTime.Today.AddDays(-10);
			DateTime end = start.AddMonths(1).AddDays(-1);
			var employee = _employeeData.GetEmployeeData();
			var clLeaveTypeId = Guid.NewGuid();
			var slLeaveTypeId = Guid.NewGuid();
			var applyLeaveId = Guid.NewGuid();
			var leaveBalance = new List<LeaveBalance>
			{
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					LeaveTypeId = clLeaveTypeId,
					LeaveType = new LeaveType{Name = "casulal Lave"},
					Leaves = 2,
					CustomizedBalId = Guid.NewGuid(),
					EffectiveFrom = start,
					EffectiveTo = end,
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					LeaveTypeId= slLeaveTypeId,
					LeaveType = new LeaveType{Name = "sick Leave"},
					Leaves = 4,
					CustomizedBalId = Guid.NewGuid(),
					EffectiveFrom = start,
					EffectiveTo = end,
				},
				new LeaveBalance
				{
					EmployeeId = Guid.NewGuid(),
					LeaveTypeId= slLeaveTypeId,
					LeaveType = new LeaveType{Name = "Earned Leave"},
					Leaves = 4,
					CustomizedBalId = Guid.NewGuid(),
					EffectiveFrom = start,
					EffectiveTo = end,
				}
			};
			var applyLeave = new ApplyLeave
			{
				ID = applyLeaveId,
				EmployeeId = employee.ID,
				FromDate = start.AddDays(2),
				FromHalf = false,
				ToDate = start.AddDays(5),
				ToHalf = false,
				Reason = "Corona",
				EmergencyContNo = "9392440422",
				LeaveTypes = "[{\"name\":\"Earned Leave\",\"days\":2}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					LeaveTypeId = clLeaveTypeId,
					NoOfLeaves= 4,
				} }
			};

			var applyLeaveDetails = new List<ApplyLeaveDetails>
			{
				new ApplyLeaveDetails
				{
					ID = Guid.NewGuid(),
					ApplyLeaveId= applyLeaveId,
					ApplyLeave = new ApplyLeave
					{
						ID = applyLeaveId,
						EmployeeId = employee.ID,
						FromDate = end.AddDays(-2),
						ToDate = end.AddDays(-2),
						Reason = "Corona",
						EmergencyContNo = "9381742192",
						Status = (int)ApplyLeaveSts.Applied,
						FromHalf = true,
						ToHalf = false
					},
					IsFirstHalf = false,
					IsHalfDay = false
				}
			};

			var attendance = new List<Attendance>
			{
				new Attendance
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  AttendanceDate = start.AddDays(2),
				  AttendanceStatus = (int)AttendanceStatus.Absent
				},
				new Attendance
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  AttendanceDate = start.AddDays(3),
				  AttendanceStatus = (int)AttendanceStatus.WeekOff
				},
				new Attendance
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  AttendanceDate = start.AddDays(4),
				  AttendanceStatus = (int)Enum.Parse(AttendanceStatus.WeekOff.GetType(),AttendanceStatus.WeekOff.ToString()),
				},
				new Attendance
				{
				  ID = Guid.NewGuid(),
				  EmployeeId = employee.ID,
				  AttendanceDate = start.AddDays(5),
				  AttendanceStatus = (int)AttendanceStatus.Absent
				},
			};

			var modifyLogs = new List<AttendanceModifyLogs>
			{
				new AttendanceModifyLogs
				{
					ID = Guid.NewGuid(),
					EmployeeID = employee.ID,
					IsFirstOff = true,
					IsHalfDay = true,
					AttendanceID = Guid.Parse("3c39d877-0f11-4ee4-9401-89d804390efe"),
				}
			};

			var leaveType = new List<LeaveType> { new LeaveType { ID = clLeaveTypeId, Name = "Casual leave", DefaultPayoff = false } };

			//Mock
			var mockLeaveBalance = leaveBalance.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, mockLeaveBalance);

			var leaveDetailsMock = applyLeaveDetails.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveDetails(uow, _context, leaveDetailsMock);

			var attendanceMock = attendance.AsQueryable().BuildMockDbSet();
			SetData.MockAttendance(uow, _context, attendanceMock);
			//_ = _context.GetRepositoryAsyncDbSet(uow, attendance);
			_ = _context.GetRepositoryAsyncDbSet(uow, modifyLogs);

			var leaveTypeMock = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow, _context, leaveTypeMock);
			//Act
			await _service.UpdateLBIfLeaveApproved(applyLeave);
			var list = uow.Object.GetRepositoryAsync<Attendance>().GetAsync().Result.ToList();
			Assert.Equal((int)AttendanceStatus.Leave, list[0].AttendanceStatus);
			Assert.Equal((int)AttendanceStatus.Leave, list[1].AttendanceStatus);
			Assert.Equal((int)AttendanceStatus.Leave, list[2].AttendanceStatus);
			Assert.Equal((int)AttendanceStatus.Leave, list[3].AttendanceStatus);
		}

		[Fact] // leave applied for halfDay,and fromHalf is true,i.e first half is leave
			   // attendance status represents first half and halfDay type represents second half
		public async Task UpdateLBIfLeaveApproved_First_HalfDay_Verification_OneDayLeave()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			var fromDate = DateTime.Now.Date;
			var applyLeaveId = Guid.NewGuid();
			var attendanceId = Guid.NewGuid();
			var leaveType = new List<LeaveType> { new LeaveType
			{
				ID = leaveTypeId,
				DefaultPayoff = false,
				Name = "Casual Leave",
				Status = true
			} };
			var alreadyAppliedLeave = new List<ApplyLeave> { new ApplyLeave
			{
				FromDate = fromDate.AddDays(1),
				ToDate = fromDate.AddDays(1),
				FromHalf = false,
				ToHalf = false,
				EmployeeId = employee.ID,
				ApplyLeaveType = new List<ApplyLeaveType>
				{
					new ApplyLeaveType
					{
						LeaveTypeId = leaveTypeId,
						NoOfLeaves = 1
					}
				}
			} };
			var applyLvDetails = new List<ApplyLeaveDetails> { new ApplyLeaveDetails
			{
				ApplyLeaveId = applyLeaveId,
				LeaveTypeId = leaveTypeId,
				LeaveCount = 1,
				LeaveDate = fromDate,
			} };
			var lvBalance = new List<LeaveBalance> { new LeaveBalance
			{
				ApplyLeaveId = applyLeaveId,
				EffectiveFrom = fromDate,
				EffectiveTo = fromDate.AddMonths(1).AddDays(-1),
				EmployeeId = employee.ID,
				LeaveTypeId = leaveTypeId,
				Leaves = 1
			} };
			var attendance = new List<Attendance> { new Attendance
			{
				EmployeeId = employee.ID,
				AttendanceDate = fromDate.AddDays(-1),
				AttendanceStatus = (int)AttendanceStatus.Present,
				LoginType = (int)LoginType.Biometric
			} };
			var modifyAttLogs = new List<AttendanceModifyLogs> { new AttendanceModifyLogs
			{
				ID = Guid.NewGuid(),
				PresentAttStatus = (int)AttendanceStatus.Present,
				AttendanceID = attendanceId,
				IsHalfDay = true,
			} };
			var applyLeave = new ApplyLeave
			{
				FromDate = fromDate.AddDays(-1),
				ToDate = fromDate.AddDays(-1),
				FromHalf = true,
				ToHalf = false,
				EmployeeId = employee.ID,
				ApplyLeaveType = new List<ApplyLeaveType>
				{
					new ApplyLeaveType
					{
						LeaveTypeId = leaveTypeId,
						NoOfLeaves = 0.5m
					}
				}
			};
			//Mock
			_ = _context.GetRepositoryAsyncDbSet(uow, leaveType);
			_ = _context.GetRepositoryAsyncDbSet(uow, alreadyAppliedLeave);
			_ = _context.GetRepositoryAsyncDbSet(uow, applyLvDetails);
			_ = _context.GetRepositoryAsyncDbSet(uow, lvBalance);
			var attendanceMock = attendance.AsQueryable().BuildMockDbSet();
			SetData.MockAttendance(uow, _context, attendanceMock);
			_ = _context.GetRepositoryAsyncDbSet(uow, modifyAttLogs);
			//Act
			await _service.UpdateLBIfLeaveApproved(applyLeave);
			var details = await uow.Object.GetRepositoryAsync<ApplyLeaveDetails>().GetAsync();
			var att = await uow.Object.GetRepositoryAsync<Attendance>().GetAsync();
			//Assert
			Assert.Equal((int)AttendanceStatus.HalfDayLeave, att.FirstOrDefault().AttendanceStatus);
			Assert.Equal((int)AttendanceStatus.HalfDayPresent, att.FirstOrDefault().HalfDayType);
			Assert.True(details.LastOrDefault().IsFirstHalf);
		}

		[Fact] // leave applied for halfDay,and toHalf is true, i.e second half is leave
			   // (attendance status represents firstHalf, halfDayType represents secondHalf)
		public async Task UpdateLBIfLeaveApproved_Second_HalfDay_Verification_OneDayLeave()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			var fromDate = DateTime.Now.Date;
			var applyLeaveId = Guid.NewGuid();
			var attendanceId = Guid.NewGuid();
			var leaveType = new List<LeaveType> { new LeaveType
			{
				ID = leaveTypeId,
				DefaultPayoff = false,
				Name = "Casual Leave",
				Status = true
			} };
			var alreadyAppliedLeave = new List<ApplyLeave> { new ApplyLeave
			{
				FromDate = fromDate.AddDays(1),
				ToDate = fromDate.AddDays(1),
				FromHalf = false,
				ToHalf = false,
				EmployeeId = employee.ID,
				ApplyLeaveType = new List<ApplyLeaveType>
				{
					new ApplyLeaveType
					{
						LeaveTypeId = leaveTypeId,
						NoOfLeaves = 1
					}
				}
			} };
			var applyLvDetails = new List<ApplyLeaveDetails> { new ApplyLeaveDetails
			{
				ApplyLeaveId = applyLeaveId,
				LeaveTypeId = leaveTypeId,
				LeaveCount = 1,
				LeaveDate = fromDate,
			} };
			var lvBalance = new List<LeaveBalance> { new LeaveBalance
			{
				ApplyLeaveId = applyLeaveId,
				EffectiveFrom = fromDate,
				EffectiveTo = fromDate.AddMonths(1).AddDays(-1),
				EmployeeId = employee.ID,
				LeaveTypeId = leaveTypeId,
				Leaves = 1
			} };
			var attendance = new List<Attendance> { new Attendance
			{
				EmployeeId = employee.ID,
				AttendanceDate = fromDate.AddDays(-1),
				AttendanceStatus = (int)AttendanceStatus.Present,
				LoginType = (int)LoginType.Biometric
			} };
			var modifyAttLogs = new List<AttendanceModifyLogs> { new AttendanceModifyLogs
			{
				ID = Guid.NewGuid(),
				PresentAttStatus = (int)AttendanceStatus.Present,
				AttendanceID = attendanceId,
				IsHalfDay = true,
			} };
			var applyLeave = new ApplyLeave
			{
				FromDate = fromDate.AddDays(-1),
				ToDate = fromDate.AddDays(-1),
				FromHalf = false,
				ToHalf = true,
				EmployeeId = employee.ID,
				ApplyLeaveType = new List<ApplyLeaveType>
				{
					new ApplyLeaveType
					{
						LeaveTypeId = leaveTypeId,
						NoOfLeaves = 0.5m
					}
				}
			};
			//Mock
			_ = _context.GetRepositoryAsyncDbSet(uow, leaveType);
			_ = _context.GetRepositoryAsyncDbSet(uow, alreadyAppliedLeave);
			_ = _context.GetRepositoryAsyncDbSet(uow, applyLvDetails);
			_ = _context.GetRepositoryAsyncDbSet(uow, lvBalance);
			var attendanceMock = attendance.AsQueryable().BuildMockDbSet();
			SetData.MockAttendance(uow, _context, attendanceMock);
			_ = _context.GetRepositoryAsyncDbSet(uow, modifyAttLogs);
			//Act
			await _service.UpdateLBIfLeaveApproved(applyLeave);
			var details = await uow.Object.GetRepositoryAsync<ApplyLeaveDetails>().GetAsync();
			var att = await uow.Object.GetRepositoryAsync<Attendance>().GetAsync();
			//Assert
			Assert.Equal((int)AttendanceStatus.HalfDayPresent, att.FirstOrDefault().AttendanceStatus);
			Assert.Equal((int)AttendanceStatus.HalfDayLeave, att.FirstOrDefault().HalfDayType);
			Assert.False(details.LastOrDefault().IsFirstHalf);
		}

		[Fact]//if we apply 2 days leave with first and second half true,
			  //then first HalfDay checkbox refers secondHalf leave in Day 1
			  //second halfDay checkbox refers firstHalf leave in Day 2
		public async Task UpdateLBIfLeaveApproved_HalfDay_Verification_DifferentDays()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			var fromDate = DateTime.Now.Date;
			var applyLeaveId = Guid.NewGuid();
			var attendanceId = Guid.NewGuid();
			var leaveType = new List<LeaveType> { new LeaveType
			{
				ID = leaveTypeId,
				DefaultPayoff = false,
				Name = "Casual Leave",
				Status = true
			} };
			var alreadyAppliedLeave = new List<ApplyLeave> { new ApplyLeave
			{
				FromDate = fromDate.AddDays(1),
				ToDate = fromDate.AddDays(1),
				FromHalf = false,
				ToHalf = false,
				EmployeeId = employee.ID,
				LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":1}]",
				ApplyLeaveType = new List<ApplyLeaveType>
				{
					new ApplyLeaveType
					{
						LeaveTypeId = leaveTypeId,
						NoOfLeaves = 1
					}
				}
			} };
			var applyLvDetails = new List<ApplyLeaveDetails> { new ApplyLeaveDetails
			{
				ApplyLeaveId = applyLeaveId,
				LeaveTypeId = leaveTypeId,
				LeaveCount = 1,
				LeaveDate = fromDate,
			} };
			var lvBalance = new List<LeaveBalance> { new LeaveBalance
			{
				ApplyLeaveId = applyLeaveId,
				EffectiveFrom = fromDate,
				EffectiveTo = fromDate.AddMonths(1).AddDays(-1),
				EmployeeId = employee.ID,
				LeaveTypeId = leaveTypeId,
				Leaves = 1
			} };
			var attendance = new List<Attendance> { new Attendance
			{
				ID = attendanceId,
				EmployeeId = employee.ID,
				AttendanceDate = fromDate.AddDays(-2),
				AttendanceStatus = (int)AttendanceStatus.WFH,
				LoginType = (int)LoginType.Biometric
			},
			new Attendance{
				ID = attendanceId,
				EmployeeId = employee.ID,
				AttendanceDate = fromDate.AddDays(-1),
				AttendanceStatus = (int)AttendanceStatus.Present,
				LoginType = (int)LoginType.Biometric
			} };
			var modifyAttLogs = new List<AttendanceModifyLogs> { new AttendanceModifyLogs
			{
				ID = Guid.NewGuid(),
				PresentAttStatus = (int)AttendanceStatus.Present,
				AttendanceID = attendanceId,
				IsHalfDay = true,
			} };
			var applyLeave = new ApplyLeave
			{
				FromDate = fromDate.AddDays(-2),
				ToDate = fromDate.AddDays(-1),
				FromHalf = true,
				ToHalf = true,
				EmployeeId = employee.ID,
				ApplyLeaveType = new List<ApplyLeaveType>
				{
					new ApplyLeaveType
					{
						LeaveTypeId = leaveTypeId,
						NoOfLeaves = 1
					}
				}
			};
			//Mock
			_ = _context.GetRepositoryAsyncDbSet(uow, leaveType);
			_ = _context.GetRepositoryAsyncDbSet(uow, alreadyAppliedLeave);
			_ = _context.GetRepositoryAsyncDbSet(uow, applyLvDetails);
			_ = _context.GetRepositoryAsyncDbSet(uow, lvBalance);
			var attendanceMock = attendance.AsQueryable().BuildMockDbSet();
			SetData.MockAttendance(uow, _context, attendanceMock);
			_ = _context.GetRepositoryAsyncDbSet(uow, modifyAttLogs);
			//Act
			await _service.UpdateLBIfLeaveApproved(applyLeave);
			var details = await uow.Object.GetRepositoryAsync<ApplyLeaveDetails>().GetAsync();
			var att = await uow.Object.GetRepositoryAsync<Attendance>().GetAsync();
			var list = details.ToList();
			//Assert
			Assert.Equal((int)AttendanceStatus.HalfDayWFH, att.FirstOrDefault().HalfDayType);
			Assert.Equal((int)AttendanceStatus.HalfDayLeave, att.FirstOrDefault().AttendanceStatus);
			Assert.Equal((int)AttendanceStatus.HalfDayLeave, att.LastOrDefault().HalfDayType);
			Assert.Equal((int)AttendanceStatus.HalfDayPresent, att.LastOrDefault().AttendanceStatus);
			Assert.True(list[1].IsFirstHalf);
			Assert.False(list[2].IsFirstHalf);
		}

		[Fact]
		public async Task RejectAfterApprove_Verifying_When_TwoRecords_In_LeaveBalance_For_OneDay_Leave()
		{
			var employee = _employeeData.GetEmployeeData();
			var applyLeaveId = Guid.NewGuid();
			var leaveTypeId = Guid.NewGuid();
			var fromDate = DateTime.Now.Date;
			var attendanceId = Guid.NewGuid();

			var appliedLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					ID = applyLeaveId,
					EmployeeId = employee.ID,
					FromDate = fromDate.AddMonths(1).AddDays(-18),
					ToDate = fromDate.AddMonths(1).AddDays(-18),
					LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":1}]",
					Status = (int)ApplyLeaveSts.Approved,
					ApplyLeaveType = new List<ApplyLeaveType>
					{
						new ApplyLeaveType
						{
							LeaveTypeId = leaveTypeId,
							NoOfLeaves = 1,
						}
					}
				}
			};
			var applyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
			{
				ApplyLeaveId = applyLeaveId,
				LeaveTypeId = leaveTypeId,
				NoOfLeaves = 1,
			} };
			var attendance = new List<Attendance> { new Attendance
			{
				ID = attendanceId,
				EmployeeId = employee.ID,
				AttendanceDate = fromDate.AddDays(-1),
				AttendanceStatus = (int)AttendanceStatus.Present
			} };
			var leaveBalance = new List<LeaveBalance>
			{
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					LeaveTypeId = leaveTypeId,
					ApplyCompensatoryId = Guid.NewGuid(),
					EffectiveFrom = fromDate,
					EffectiveTo = fromDate.AddMonths(1).AddDays(-1),
					Leaves = 0.5m
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					LeaveTypeId = leaveTypeId,
					ApplyCompensatoryId = Guid.NewGuid(),
					EffectiveFrom = fromDate.AddMonths(1).AddDays(-20),
					EffectiveTo = fromDate.AddMonths(1).AddDays(-1),
					Leaves = 1.5m
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					LeaveTypeId = leaveTypeId,
					ApplyLeaveId = applyLeaveId,
					EffectiveFrom = fromDate,
					EffectiveTo = fromDate.AddMonths(1).AddDays(-1),
					Leaves = -0.5m
				},
				new LeaveBalance
				{
					EmployeeId = employee.ID,
					LeaveTypeId = leaveTypeId,
					ApplyLeaveId = applyLeaveId,
					EffectiveFrom = fromDate.AddMonths(1).AddDays(-20),
					EffectiveTo = fromDate.AddMonths(1).AddDays(-1),
					Leaves = -0.5m
				}
			};
			//Mock
			_ = _context.GetRepositoryAsyncDbSet(uow, appliedLeave);
			_ = _context.GetRepositoryAsyncDbSet(uow, applyLeaveType);
			_ = _context.GetRepositoryAsyncDbSet(uow, attendance);

			var leaveBalMock = leaveBalance.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveBalance(uow, _context, leaveBalMock);

			var leaveBal = await uow.Object.GetRepositoryAsync<LeaveBalance>().GetAsync();
			var rejectedLeaveBal = await uow.Object.GetRepositoryAsync<LeaveBalance>().GetAsync(x => x.LeaveTypeId == leaveTypeId && x.ApplyLeaveId == applyLeaveId);
			Assert.Equal(2, leaveBal.Sum(x => x.Leaves));
			Assert.Equal(2, rejectedLeaveBal.Count());
			Assert.Equal(0, rejectedLeaveBal.Sum(x => x.Leaves));
			Assert.Equal((int)ApplyLeaveSts.Rejected, appliedLeave.First().Status);
		}

		[Theory]
		[InlineData(2, 2)]//in LeaveBalance Q1 having 0.5 Leaves and Q2 having 0.5 leaves ; leave applied for 1 day, then leave balance rejected from both half's
						  //first rejected from Q1 and then from Q2
		public async Task UpdateLBIfLeaveApproved_Verify_LeaveBalance(int from, int to)
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			var fromDate = DateTime.Now.Date;
			var applyLeaveId = Guid.NewGuid();
			var attendanceId = Guid.NewGuid();
			var leaveType = new List<LeaveType> { new LeaveType
			{
				ID = leaveTypeId,
				DefaultPayoff = false,
				Name = "Casual Leave",
				Status = true
			},
			new LeaveType{
				ID = Guid.NewGuid(),
				DefaultPayoff = true,
				Name = "Loss Of Pay",
				Status = true

			} };
			var alreadyAppliedLeave = new List<ApplyLeave> { new ApplyLeave
			{
				FromDate = fromDate.AddDays(1),
				ToDate = fromDate.AddDays(1),
				FromHalf = false,
				ToHalf = false,
				EmployeeId = employee.ID,
				ApplyLeaveType = new List<ApplyLeaveType>
				{
					new ApplyLeaveType
					{
						LeaveTypeId = leaveTypeId,
						NoOfLeaves = 1
					}
				}
			} };
			var applyLvDetails = new List<ApplyLeaveDetails> { new ApplyLeaveDetails
			{
				ApplyLeaveId = applyLeaveId,
				LeaveTypeId = leaveTypeId,
				LeaveCount = 1,
				LeaveDate = fromDate,
			} };
			var lvBalance = new List<LeaveBalance> {
			new LeaveBalance{
				ApplyLeaveId = applyLeaveId,
				EffectiveFrom = fromDate,
				EffectiveTo = fromDate.AddMonths(1).AddDays(-1),
				EmployeeId = employee.ID,
				LeaveTypeId = leaveTypeId,
				Leaves= 1
			},
			new LeaveBalance{
				ApplyLeaveId = applyLeaveId,
				EffectiveFrom = fromDate,
				EffectiveTo = fromDate.AddMonths(1).AddDays(-1),
				EmployeeId = employee.ID,
				LeaveTypeId = leaveTypeId,
				Leaves= -1
			},
			new LeaveBalance
			{
				ApplyLeaveId = applyLeaveId,
				EffectiveFrom = fromDate,
				EffectiveTo = fromDate.AddMonths(1).AddDays(-1),
				EmployeeId = employee.ID,
				LeaveTypeId = leaveTypeId,
				Leaves = 0.5m
			},
			new LeaveBalance{
				ApplyLeaveId = applyLeaveId,
				EffectiveFrom = fromDate.AddMonths(1).AddDays(-20),
				EffectiveTo = fromDate.AddMonths(1).AddDays(-1),
				EmployeeId = employee.ID,
				LeaveTypeId = leaveTypeId,
				Leaves= 1
			} };
			var attendance = new List<Attendance> { new Attendance
			{
				ID = attendanceId,
				EmployeeId = employee.ID,
				AttendanceDate = fromDate.AddDays(-2),
				AttendanceStatus = (int)AttendanceStatus.WFH,
				LoginType = (int)LoginType.Biometric
			},
			new Attendance{
				ID = attendanceId,
				EmployeeId = employee.ID,
				AttendanceDate = fromDate.AddDays(-1),
				AttendanceStatus = (int)AttendanceStatus.Present,
				LoginType = (int)LoginType.Biometric
			} };
			var modifyAttLogs = new List<AttendanceModifyLogs> { new AttendanceModifyLogs
			{
				ID = Guid.NewGuid(),
				PresentAttStatus = (int)AttendanceStatus.Present,
				AttendanceID = attendanceId,
				IsHalfDay = true,
			} };
			var applyLeave = new ApplyLeave
			{
				FromDate = fromDate.AddDays(from),
				ToDate = fromDate.AddDays(to),
				FromHalf = false,
				ToHalf = false,
				EmployeeId = employee.ID,
				ApplyLeaveType = new List<ApplyLeaveType>
				{
					new ApplyLeaveType
					{
						LeaveTypeId = leaveTypeId,
						NoOfLeaves = 1
					}
				}
			};
			//Mock
			_ = _context.GetRepositoryAsyncDbSet(uow, leaveType);
			_ = _context.GetRepositoryAsyncDbSet(uow, alreadyAppliedLeave);
			_ = _context.GetRepositoryAsyncDbSet(uow, applyLvDetails);
			_ = _context.GetRepositoryAsyncDbSet(uow, lvBalance);
			var attendanceMock = attendance.AsQueryable().BuildMockDbSet();
			SetData.MockAttendance(uow, _context, attendanceMock);
			_ = _context.GetRepositoryAsyncDbSet(uow, modifyAttLogs);

			var details = await uow.Object.GetRepositoryAsync<ApplyLeaveDetails>().GetAsync();
			var leaveBalance = await uow.Object.GetRepositoryAsync<LeaveBalance>().GetAsync();
			//Act
			await _service.UpdateLBIfLeaveApproved(applyLeave);

			Assert.Equal(fromDate.AddDays(from), details.LastOrDefault().LeaveDate);
			Assert.Equal(1, details.LastOrDefault().LeaveCount);
			Assert.Equal(-0.5m, leaveBalance.ElementAt(4).Leaves);
			Assert.Equal(fromDate, leaveBalance.ElementAt(4).EffectiveFrom);
			Assert.Equal(-0.5m, leaveBalance.LastOrDefault().Leaves);
			Assert.Equal(fromDate.AddMonths(1).AddDays(-20), leaveBalance.LastOrDefault().EffectiveFrom);

		}

		private void AlreadyApplied_LeaveData_WFhData(Guid employeeId, Guid leaveTypeId)
		{
			var applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					FromDate = DateTime.Now.AddDays(-5),
					ToDate = DateTime.Now.AddDays(-4),
					EmployeeId = employeeId,
					NoOfLeaves = 2,
					LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":2}]",
					ApplyLeaveType = new List<ApplyLeaveType>{new ApplyLeaveType
					{
						LeaveTypeId = leaveTypeId,
						NoOfLeaves= 2,
					} }
				}
			};
			var wfh = new List<ApplyWfh> { new ApplyWfh
			{
				EmployeeId = employeeId,
				FromDateC = DateTime.Now,
				ToDateC = DateTime.Now.AddDays(3),
				FromHalf = true,
				ToHalf = true,
				Status = (int)WfhStatus.Approved
			} };

			var mockApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockApplyLeave);

			var mockWFH = wfh.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWFH);
		}

		[Theory] //Ex -> WFH dates 20-06-2023 to 23-06-23 and fromHalf, toHalf are true; applying leave on 20-06-2023 and fromHalf false,toHalf true
		[InlineData(0, 0, false, true)]// Leave applied on 20-06-23
		[InlineData(3, 3, true, false)]// Leave applied on 23-06-23
		public async Task SelfServiceLeaveValidation_FromHalf_ToHalf_Matches_With_WFH_Dates(int fromDate, int toDate, bool fromHalf, bool toHalf)
		{
			var employee = _employeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			AlreadyApplied_LeaveData_WFhData(employee.ID, leaveTypeId);

			var newApplyLeave = new ApplyLeave
			{
				EmployeeId = employee.ID,
				FromDate = DateTime.Now.AddDays(fromDate),
				ToDate = DateTime.Now.AddDays(toDate),
				FromHalf = fromHalf,
				ToHalf = toHalf,
				EmergencyContNo = "9392440499",
				NoOfLeaves = 0.5m,
				LeaveTypes = "[{\"name\":\"Earned Leave\",\"days\":0.5}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
						LeaveTypeId = leaveTypeId,
						NoOfLeaves= 0.5m,
				} }
			};

			var dd = await _service.SelfServiceLeaveValidation(newApplyLeave);
			Assert.False(dd.IsSuccess);
		}

		[Theory]
		[InlineData(true, true)]   // Leave applied from 19-06-23 to 23-06-23
		[InlineData(true, false)]  // Leave applied from 19-06-23 to 23-06-23
		[InlineData(false, true)]  // Leave applied from 19-06-23 to 23-06-23
		[InlineData(false, false)] // Leave applied from 19-06-23 to 23-06-23
		public async Task SelfServiceLeaveValidation_ToDate_Matches_With_WFH_ToDate(bool fromHalf, bool toHalf)
		{
			var employee = _employeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			AlreadyApplied_LeaveData_WFhData(employee.ID, leaveTypeId);

			var newApplyLeave = new ApplyLeave
			{
				EmployeeId = employee.ID,
				FromDate = DateTime.Now.AddDays(-1),
				ToDate = DateTime.Now.AddDays(3),
				FromHalf = fromHalf,
				ToHalf = toHalf,
				EmergencyContNo = "9392440499",
				NoOfLeaves = 5,
				LeaveTypes = "[{\"name\":\"Earned Leave\",\"days\":5}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
						LeaveTypeId = leaveTypeId,
						NoOfLeaves= 5,
				} }
			};

			var dd = await _service.SelfServiceLeaveValidation(newApplyLeave);
			Assert.False(dd.IsSuccess);
		}

		[Theory]
		[InlineData(true, true)]   // Leave applied from 19-06-23 to 22-06-23
		[InlineData(true, false)]  // Leave applied from 19-06-23 to 22-06-23
		[InlineData(false, true)]  // Leave applied from 19-06-23 to 22-06-23
		[InlineData(false, false)] // Leave applied from 19-06-23 to 22-06-23
		public async Task SelfServiceLeaveValidation_ToDate_BetWeen_WFH_Dates(bool fromHalf, bool toHalf)
		{
			var employee = _employeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			AlreadyApplied_LeaveData_WFhData(employee.ID, leaveTypeId);

			var newApplyLeave = new ApplyLeave
			{
				EmployeeId = employee.ID,
				FromDate = DateTime.Now.AddDays(-1),
				ToDate = DateTime.Now.AddDays(2),
				FromHalf = fromHalf,
				ToHalf = toHalf,
				EmergencyContNo = "9392440499",
				NoOfLeaves = 4,
				LeaveTypes = "[{\"name\":\"Earned Leave\",\"days\":4}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
						LeaveTypeId = leaveTypeId,
						NoOfLeaves= 4,
				} }
			};

			var dd = await _service.SelfServiceLeaveValidation(newApplyLeave);
			Assert.False(dd.IsSuccess);
		}

		[Theory]
		[InlineData(true, true)]   // Leave applied from 20-06-23 to 23-06-23
		[InlineData(true, false)]  // Leave applied from 20-06-23 to 23-06-23
		[InlineData(false, true)]  // Leave applied from 20-06-23 to 23-06-23
		[InlineData(false, false)] // Leave applied from 20-06-23 to 23-06-23
		public async Task SelfServiceLeaveValidation_FromDate_And_ToDate_SameAs_WFH_Dates(bool fromHalf, bool toHalf)
		{
			var employee = _employeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			AlreadyApplied_LeaveData_WFhData(employee.ID, leaveTypeId);

			var newApplyLeave = new ApplyLeave
			{
				EmployeeId = employee.ID,
				FromDate = DateTime.Now.AddDays(0),
				ToDate = DateTime.Now.AddDays(3),
				FromHalf = fromHalf,
				ToHalf = toHalf,
				EmergencyContNo = "9392440499",
				NoOfLeaves = 4,
				LeaveTypes = "[{\"name\":\"Earned Leave\",\"days\":4}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
						LeaveTypeId = leaveTypeId,
						NoOfLeaves= 4,
				} }
			};

			var dd = await _service.SelfServiceLeaveValidation(newApplyLeave);
			Assert.False(dd.IsSuccess);
		}

		[Theory]
		[InlineData(true, true)]   // Leave applied from 21-06-21 to 22-06-22
		[InlineData(true, false)]  // Leave applied from 21-06-21 to 22-06-22
		[InlineData(false, true)]  // Leave applied from 21-06-21 to 22-06-22
		[InlineData(false, false)] // Leave applied from 21-06-21 to 22-06-22
		public async Task SelfServiceLeaveValidation_FromDate_And_ToDate_InBetween_WFH_Dates(bool fromHalf, bool toHalf)
		{
			var employee = _employeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			AlreadyApplied_LeaveData_WFhData(employee.ID, leaveTypeId);

			var newApplyLeave = new ApplyLeave
			{
				EmployeeId = employee.ID,
				FromDate = DateTime.Now.AddDays(1),
				ToDate = DateTime.Now.AddDays(2),
				FromHalf = fromHalf,
				ToHalf = toHalf,
				EmergencyContNo = "9392440499",
				NoOfLeaves = 4,
				LeaveTypes = "[{\"name\":\"Earned Leave\",\"days\":2}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
						LeaveTypeId = leaveTypeId,
						NoOfLeaves= 4,
				} }
			};

			var dd = await _service.SelfServiceLeaveValidation(newApplyLeave);
			Assert.False(dd.IsSuccess);
		}

		[Fact]
		public async Task SelfServiceLeaveValidation_OneDay_Leave_InBetween_WFH_Dates()
		{
			var employee = _employeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			AlreadyApplied_LeaveData_WFhData(employee.ID, leaveTypeId);

			var newApplyLeave = new ApplyLeave
			{
				EmployeeId = employee.ID,
				FromDate = DateTime.Now.AddDays(2),
				ToDate = DateTime.Now.AddDays(2),
				FromHalf = true,
				ToHalf = false,
				EmergencyContNo = "9392440499",
				NoOfLeaves = 3,
				LeaveTypes = "[{\"name\":\"Earned Leave\",\"days\":3}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
						LeaveTypeId = leaveTypeId,
						NoOfLeaves= 3,
				} }
			};

			var dd = await _service.SelfServiceLeaveValidation(newApplyLeave);
			Assert.False(dd.IsSuccess);
		}

		[Theory]
		[InlineData(true, true)]   // Leave applied from 20-06-23 to 22-06-23
		[InlineData(true, false)]  // Leave applied from 20-06-23 to 22-06-23
		[InlineData(false, true)]  // Leave applied from 20-06-23 to 22-06-23
		[InlineData(false, false)] // Leave applied from 20-06-23 to 22-06-23

		public async Task SelfServiceLeaveValidation_Leave_FromDate_MatchesWith_WFH_FromDate(bool fromHalf, bool toHalf)
		{
			var employee = _employeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			AlreadyApplied_LeaveData_WFhData(employee.ID, leaveTypeId);

			var newApplyLeave = new ApplyLeave
			{
				EmployeeId = employee.ID,
				FromDate = DateTime.Now.AddDays(0),
				ToDate = DateTime.Now.AddDays(2),
				FromHalf = fromHalf,
				ToHalf = toHalf,
				EmergencyContNo = "9392440499",
				NoOfLeaves = 3,
				LeaveTypes = "[{\"name\":\"Earned Leave\",\"days\":3}]",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
						LeaveTypeId = leaveTypeId,
						NoOfLeaves= 3,
				} }
			};

			var dd = await _service.SelfServiceLeaveValidation(newApplyLeave);
			Assert.False(dd.IsSuccess);
		}

		[Fact]
		public async Task SelfServiceLeaveValidation__LeaveDate_And_HalfDays_MatchesWith_WFH_Date_And_HalfDays()
		{
			var employee = _employeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			var alreadyApplyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					FromDate = DateTime.Now.AddDays(-5),
					ToDate = DateTime.Now.AddDays(-4),
					EmployeeId = employee.ID,
					NoOfLeaves = 2,
					LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":2}]",
					ApplyLeaveType = new List<ApplyLeaveType>{new ApplyLeaveType
					{
						LeaveTypeId = leaveTypeId,
						NoOfLeaves= 2,
					} }
				}
			};
			var WFH = new List<ApplyWfh>
			{
				new ApplyWfh
				{
					ID = Guid.NewGuid(),
					EmployeeId=employee.ID,
					FromDateC = DateTime.Now.Date,
					ToDateC = DateTime.Now.Date,
					FromHalf = true,
					ToHalf = false,
					Status = (int)WfhStatus.Approved,
				}
			};
			var applyLeave = new ApplyLeave
			{
				EmployeeId = employee.ID,
				FromDate = DateTime.Now.Date,
				ToDate = DateTime.Now.Date,
				FromHalf = true,
				ToHalf = false,
				NoOfLeaves = 0.5m,
				LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":0.5}]",
				EmergencyContNo = "9392440499",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					NoOfLeaves = 0.5m,
					LeaveTypeId= leaveTypeId,
				} }
			};
			var mockApplyLeave = alreadyApplyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockApplyLeave);

			var mockWFH = WFH.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWFH);

			var dd = await _service.SelfServiceLeaveValidation(applyLeave);
			Assert.False(dd.IsSuccess);
		}
		[Fact]
		public async Task SelfServiceLeaveValidation_PastDaysValidation_Throw_Exception()
		{
			var employee = _employeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			AlreadyApplied_LeaveData_WFhData(employee.ID, leaveTypeId);
			var clientVisits = new List<ApplyClientVisits>
			{
				new ApplyClientVisits
				{
					EmployeeId = employee.ID,
					FromDate = DateTime.Now.AddDays(4),
					ToDate= DateTime.Now.AddDays(5),
					Status =(int)ClientVisitStatus.Applied,
					PlaceOfVisit = "USA",
				}
			};
			var leaveType = new List<LeaveType>
			{
				new LeaveType
				{
					ID = leaveTypeId,
					Code = "CL",
					Name="Casual Leave",
					DefaultPayoff = false,
					PastDate = 2,
					FutureDate = 10,
				}
			};
			var applyLeaveType = new List<ApplyLeaveType>
			{
				new ApplyLeaveType
				{
					LeaveTypeId = leaveTypeId,
					NoOfLeaves= 2,
					ApplyLeave = new ApplyLeave
					{
						EmployeeId = employee.ID,
						Status = (int)ApplyLeaveSts.Approved
					}
				}
			};
			var applyLeave = new ApplyLeave
			{
				EmployeeId = employee.ID,
				FromDate = DateTime.Today.AddDays(-3),
				ToDate = DateTime.Today.AddDays(-1),
				FromHalf = true,
				ToHalf = false,
				NoOfLeaves = 3,
				LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":3}]",
				EmergencyContNo = "9392440499",
				ApplyLeaveType = new List<ApplyLeaveType> { new ApplyLeaveType
				{
					NoOfLeaves = 3,
					LeaveTypeId= leaveTypeId,
				} }
			};
			var mockClientVisits = clientVisits.AsQueryable().BuildMockDbSet();
			SetData.MockClientVisit(uow, _context, mockClientVisits);
			var mockLeaveType = leaveType.AsQueryable().BuildMockDbSet();
			SetData.MockLeaveType(uow,_context, mockLeaveType);
			var mockApplyLeaveType = applyLeaveType.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeaveType(uow,_context, mockApplyLeaveType);

			var dd = await _service.SelfServiceLeaveValidation(applyLeave);
			Assert.False(dd.IsSuccess);
		}
		[Fact]
		public async Task SelfServiceLeaveValidation_JSONConvert_Deserialization_ReturnNull_Throw_Exception()
		{
			var employee = _employeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			var applyLeave = new ApplyLeave
			{
				EmployeeId = employee.ID,
				NoOfLeaves = 2,
				FromDate = DateTime.Now,
				ToDate = DateTime.Now.AddDays(2),
				FromHalf = true,
				ToHalf = true,
				LeaveTypes = "[]",
				ApplyLeaveType = new List<ApplyLeaveType>
				{
					new ApplyLeaveType
					{
						NoOfLeaves = 2,
						LeaveTypeId = leaveTypeId,
					}
				}
			};
			var dd = await _service.SelfServiceLeaveValidation(applyLeave);
			Assert.False(dd.IsSuccess);
		}
		[Fact]
		public async Task SelfServiceLeaveValidation_JSONConvert_Deserialization_Executing_Catch_Throw_Exception()
		{
			var employee = _employeeData.GetEmployeeData();
			var leaveTypeId = Guid.NewGuid();
			var applyLeave = new ApplyLeave
			{
				EmployeeId = employee.ID,
				NoOfLeaves = 2,
				FromDate = DateTime.Now,
				ToDate = DateTime.Now.AddDays(2),
				FromHalf = true,
				ToHalf = true,
				LeaveTypes = "",
				ApplyLeaveType = new List<ApplyLeaveType>
				{
					new ApplyLeaveType
					{
						NoOfLeaves = 2,
						LeaveTypeId = leaveTypeId,
					}
				}
			};
			var dd = await _service.SelfServiceLeaveValidation(applyLeave);
			Assert.False(dd.IsSuccess);
		}
	}
}
