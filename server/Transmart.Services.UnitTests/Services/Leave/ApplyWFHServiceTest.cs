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
using TranSmart.Data.Repository.Attendance;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Service.Leave;
using TranSmart.Service.LM_Attendance;
using TranSmart.Service.Schedules;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Leave
{
	public class ApplyWFHServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IApplyWfhService _WFHservice;
		private readonly IAttendanceService _attService;
		private readonly ILeaveBalanceService _leaveBalanceService;
		private readonly IScheduleService _scheduleService;
		private readonly IShiftService _shiftService;
		private readonly EmployeeDataGenerator _employeeData;
		private readonly Mock<DbContext> _context;
		private readonly IAttendanceRepository _attendanceRepository;
		private readonly Guid wfhId = Guid.NewGuid();
		public ApplyWFHServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_leaveBalanceService = new LeaveBalanceService(uow.Object);
			_attendanceRepository = new AttendanceRepository(_dbContext.Object);
			_shiftService = new ShiftService(uow.Object);
			_scheduleService = new ScheduleService(uow.Object, _shiftService);
			_attService = new AttendanceService(uow.Object, _scheduleService, _leaveBalanceService, _attendanceRepository);
			_WFHservice = new ApplyWfhService(uow.Object, _attService);
			_employeeData = new EmployeeDataGenerator();
			_context = new Mock<DbContext>();
		}
		private void ApplyWFHMockData()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();

			IEnumerable<ApplyWfh> data = new List<ApplyWfh>
			{
				new ApplyWfh
				{
					Employee = employee,
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					AdminReason = "reason",
					FromDateC = DateTime.Now,
					ToDateC = DateTime.Now,
					ReasonForWFH = "WFH reason",
					Status = (int)WfhStatus.Rejected,
					ApprovedById=Guid.NewGuid()
				},
				new ApplyWfh
				{
					EmployeeId = employee.ID,
					Employee = employee,
					ID = wfhId,
					AdminReason = "reason",
					FromDateC = DateTime.Now.Date.AddDays(1),
					ToDateC = DateTime.Now.Date.AddDays(1),
					ReasonForWFH = "WFH reason",
					Status = (int)WfhStatus.Approved,
					ApprovedById=employee.ReportingToId
				}

			};
			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Employee=employee,
					FromDate = DateTime.Now.AddDays(2),
					ToDate = DateTime.Now.AddDays(4),
					Status = (int)WfhStatus.Approved,
				},
				new ApplyLeave
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Employee=employee,
					FromDate = DateTime.Now.AddDays(3),
					ToDate = DateTime.Now.AddDays(3),
					Status = 1,
				}

			}.AsQueryable();

			var appliedClientVisit = new List<ApplyClientVisits>
			{
				new ApplyClientVisits
				{
					EmployeeId = employee.ID,
					FromDate = DateTime.Now.Date.AddDays(-4),
					ToDate = DateTime.Now.Date.AddDays(-2),
					PlaceOfVisit = "USA",
					Status = (int)ClientVisitStatus.Applied,
				}
			}.AsQueryable();

			#endregion

			#region Mock

			//ApplyWFH 
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockSet);

			//ApplyLeave
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//ClientVisit
			var mockApplyClientVisits = appliedClientVisit.BuildMockDbSet();
			SetData.MockApplyClientVisits(uow, _context, mockApplyClientVisits);
			#endregion
		}

		private void ApplyWFHAproveMocData(int status)
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			var employees = _employeeData.GetAllEmployeesData();

			IEnumerable<Attendance> attendance = new List<Attendance>
			{
				new Attendance
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Employee=employee,
					AttendanceDate=DateTime.Now,
					AttendanceStatus=(int)AttendanceStatus.WFH,
					LeaveTypeID=Guid.NewGuid()
				},
				new Attendance
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Employee=employee,
					AttendanceDate=DateTime.Now.AddDays(1),
					AttendanceStatus=(int)AttendanceStatus.WFH,
					LeaveTypeID=Guid.NewGuid()
				}

			}.AsQueryable();
			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Employee=employee,
					FromDate = DateTime.Now.AddDays(2),
					ToDate = DateTime.Now.AddDays(2),
					Status = (int)WfhStatus.Approved,
				},
				new ApplyLeave
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Employee=employee,
					FromDate = DateTime.Now.AddDays(3),
					ToDate = DateTime.Now.AddDays(3),
					Status = 1,
				}

			}.AsQueryable();
			IEnumerable<ApplyWfh> data = new List<ApplyWfh>
			{
				new ApplyWfh
				{
					EmployeeId = employee.ID,
					ID = wfhId,
					Employee=employee,
					AdminReason = "hfgh",
					FromDateC = DateTime.Now.AddDays(4),
					ToDateC = DateTime.Now.AddDays(4),
					Status = status,
					ReasonForWFH = "dfgf",
				},
				new ApplyWfh
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					AdminReason = "hfgh",
					Employee=employee,
					FromDateC = DateTime.Now.AddDays(5),
					ToDateC = DateTime.Now.AddDays(5),
					ReasonForWFH = "dfgf",
					Status = 1,
				}

			}.AsQueryable();

			IEnumerable<AttendanceModifyLogs> attendanceModifyLogs = new List<AttendanceModifyLogs>
			{
				new AttendanceModifyLogs
				{
					ID = Guid.NewGuid(),
				},
				new AttendanceModifyLogs
				{
					ID = Guid.NewGuid(),
				}

			}.AsQueryable();
			var appliedClientVisit = new List<ApplyClientVisits>
			{
				new ApplyClientVisits
				{
					EmployeeId = employee.ID,
					FromDate = DateTime.Now.Date.AddDays(-4),
					ToDate = DateTime.Now.Date.AddDays(-2),
					PlaceOfVisit = "USA",
					Status = (int)ClientVisitStatus.Applied,
				}
			}.AsQueryable();
			#endregion

			#region Mock
			//ApplyWFH
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockSet);

			//ApplyLeave
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//Attendance
			var mockSetAttendance = attendance.AsQueryable().BuildMockDbSet();
			SetData.MockAttendance(uow, _context, mockSetAttendance);

			//AttendanceModifyLogs
			var mockSetAttendanceModifyLogs = attendanceModifyLogs.AsQueryable().BuildMockDbSet();
			SetData.MockAttendanceModifyLogs(uow, _context, mockSetAttendanceModifyLogs);

			//Employee
			var mockSetEmployees = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployees(uow, _context, mockSetEmployees);

			//ClientVisit
			var mockApplyClientVisits = appliedClientVisit.BuildMockDbSet();
			SetData.MockApplyClientVisits(uow, _context, mockApplyClientVisits);
			#endregion
		}

		private void ApplyWFHRejectMockData(int status)
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();

			IEnumerable<ApplyWfh> data = new List<ApplyWfh>
			{
				new ApplyWfh
				{
					EmployeeId = employee.ID,
					ID = wfhId,
					Employee=employee,
					AdminReason = "hfgh",
					FromDateC = DateTime.Now,
					ToDateC = DateTime.Now,
					Status = status,
					ReasonForWFH = "dfgf",
				},
				new ApplyWfh
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					AdminReason = "hfgh",
					Employee=employee,
					FromDateC = DateTime.Now.AddDays(1),
					ToDateC = DateTime.Now.AddDays(1),
					ReasonForWFH = "dfgf",
					Status = (int)WfhStatus.Approved,
				}

			};

			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Employee=employee,
					FromDate = DateTime.Now.AddDays(2),
					ToDate = DateTime.Now.AddDays(2),
					Status = (int)WfhStatus.Approved,
				},
				new ApplyLeave
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Employee=employee,
					FromDate = DateTime.Now.AddDays(3),
					ToDate = DateTime.Now.AddDays(3),
					Status = 1,
				}

			};

			#endregion

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockSet);

			//ApplyLeave
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

		}

		[Fact]
		public async Task SelfServiceSearch_SearchWithRefId_GetValidRecords()
		{
			// Arrange
			var employee = _employeeData.GetEmployeeData();
			ApplyWFHMockData();

			ApplyWfhSearch WFHSearch = new()
			{
				FromDate = DateTime.Now.AddDays(-3),
				ToDate = DateTime.Now.AddDays(3),
				Status = 1,
				SortBy = "Employee",
				Size = 10,
				Page = 0,
				RefId = employee.ID
			};

			//Act
			var wfh = await _WFHservice.SelfServiceSearch(WFHSearch);

			//Assert
			Assert.Equal(2, wfh.Items.Count(x => x.EmployeeId == employee.ID));

		}

		[Fact]
		public async Task Get_SearchWithId_GetValidRecords()
		{
			//Arrange
			ApplyWFHMockData();

			//Act
			var applyWFH = await _WFHservice.GetById(wfhId);

			//Assert
			Assert.Equal(wfhId, applyWFH.ID);
		}

		[Fact]
		public async Task GetLeave_SearchWithId_GetValidRecords()
		{
			// Arrange
			var employee = _employeeData.GetEmployeeData();
			ApplyWFHMockData();

			//Act
			var applyWFH = await _WFHservice.GetLeave(wfhId, (Guid)employee.ReportingToId);

			//Assert
			Assert.Equal(wfhId, applyWFH.ID);
		}

		[Fact]
		public async Task GetPaginate_SearchWithStatus_GetValidRecords()
		{
			//Arrange
			ApplyWfhSearch WFHSearch = new()
			{
				FromDate = DateTime.Now.AddDays(-3),
				ToDate = DateTime.Now.AddDays(3),
				Status = (int)WfhStatus.Rejected,
				SortBy = "Employee",
				Size = 10,
				Page = 0,
			};

			ApplyWFHMockData();

			//Act
			var wfh = await _WFHservice.GetPaginate(WFHSearch);

			//Assert
			Assert.Equal(1, wfh.Items.Count(x => x.Status == (int)WfhStatus.Rejected));

		}

		[Fact]
		public async Task CustomValidation_DataSaved_WithOutException()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var result = new Result<ApplyWfh>();
			ApplyWFHMockData();

			var WFH = new ApplyWfh()
			{
				EmployeeId = employee.ID,
				ID = Guid.NewGuid(),
				Employee = employee,
				AdminReason = "reason",
				FromDateC = DateTime.Now,
				ToDateC = DateTime.Now,
				Status = (int)WfhStatus.Applied,
				ReasonForWFH = "WFH reason",
			};


			//Act
			var src = new ApplyWfhService(uow.Object, _attService);
			await src.CustomValidation(WFH, result);

			//Assert
			Assert.True(result.HasNoError);
		}

		[Fact]
		public async Task CustomValidation_Cannot_ApplyWFHMoreThanSixDays_ThrowException()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var result = new Result<ApplyWfh>();
			ApplyWFHMockData();

			var WFH = new ApplyWfh()
			{
				EmployeeId = employee.ID,
				ID = Guid.NewGuid(),
				Employee = employee,
				AdminReason = "reason",
				FromDateC = DateTime.Now.AddDays(6),
				ToDateC = DateTime.Now.AddDays(14),
				Status = (int)WfhStatus.Applied,
				ReasonForWFH = "WFH reason",
			};


			//Act
			var src = new ApplyWfhService(uow.Object, _attService);
			await src.CustomValidation(WFH, result);

			//Assert
			Assert.Null(result.ReturnValue);
			Assert.True(result.HasError);
			Assert.True(result.Messages[0].Description == "Sorry! You can't apply WFH more than 6 days");
		}

		[Fact]
		public async Task CustomValidation_AlreadyApproved_ThrowException()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var result = new Result<ApplyWfh>();
			ApplyWFHMockData();

			ApplyWfh WFH = new()
			{
				EmployeeId = employee.ID,
				ID = wfhId,
				Employee = employee,
				AdminReason = "hfgh",
				FromDateC = DateTime.Now.AddDays(1),
				ToDateC = DateTime.Now.AddDays(1),
				Status = (int)WfhStatus.Applied,
				ReasonForWFH = "dfgf",
			};


			//Act
			var src = new ApplyWfhService(uow.Object, _attService);
			await src.CustomValidation(WFH, result);

			//Assert
			Assert.Null(result.ReturnValue);
			Assert.True(result.HasError);
			Assert.True(result.Messages[0].Description == "Already approved.");
		}

		[Fact]
		public async Task CustomValidation_FromDateLessThanToDate_ThrowException()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var result = new Result<ApplyWfh>();
			ApplyWFHMockData();

			ApplyWfh WFH = new()
			{
				EmployeeId = employee.ID,
				ID = Guid.NewGuid(),
				Employee = employee,
				AdminReason = "reason",
				FromDateC = DateTime.Now.AddDays(1),
				ToDateC = DateTime.Now.AddDays(-1),
				Status = (int)WfhStatus.Applied,
				ReasonForWFH = "WFH reason",
			};

			//Act
			var src = new ApplyWfhService(uow.Object, _attService);
			await src.CustomValidation(WFH, result);

			//Assert
			Assert.Null(result.ReturnValue);
			Assert.True(result.HasError);
			Assert.True(result.Messages[0].Description == "From date should be less than the to date.");
		}

		[Fact]
		public async Task CustomValidation_AlreadyAppliedForThisDates_ThrowException()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var result = new Result<ApplyWfh>();
			ApplyWFHMockData();

			ApplyWfh WFH = new()
			{
				EmployeeId = employee.ID,
				ID = Guid.NewGuid(),
				Employee = employee,
				AdminReason = "reason",
				FromDateC = DateTime.Now.Date.AddDays(1),
				ToDateC = DateTime.Now.Date.AddDays(1),
				Status = (int)WfhStatus.Applied,
				ReasonForWFH = "WFH reason",
			};

			//Act
			var src = new ApplyWfhService(uow.Object, _attService);
			await src.CustomValidation(WFH, result);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public async Task CustomValidation_AlreadyLeavesAppliedForTheseDates_ThrowException()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var result = new Result<ApplyWfh>();
			ApplyWFHMockData();

			ApplyWfh WFH = new()
			{
				EmployeeId = employee.ID,
				ID = Guid.NewGuid(),
				Employee = employee,
				AdminReason = "reason",
				FromDateC = DateTime.Now.Date.AddDays(2),
				ToDateC = DateTime.Now.Date.AddDays(3),
				Status = (int)WfhStatus.Applied,
				ReasonForWFH = "WFH reason",
			};

			//Act
			var src = new ApplyWfhService(uow.Object, _attService);
			await src.CustomValidation(WFH, result);

			//Assert
			Assert.True(result.HasError);
			Assert.True(result.Messages[0].Description == "Leaves applied for these dates.");
		}

		[Fact]
		public async Task CustomValidation_AlreadyLeavesAppliedForBetweenDates_ThrowException()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var result = new Result<ApplyWfh>();
			ApplyWFHMockData();

			ApplyWfh WFH = new()
			{
				EmployeeId = employee.ID,
				ID = Guid.NewGuid(),
				Employee = employee,
				AdminReason = "reason",
				FromDateC = DateTime.Now.Date.AddDays(3),
				ToDateC = DateTime.Now.Date.AddDays(3),
				Status = (int)WfhStatus.Applied,
				ReasonForWFH = "WFH reason",
			};

			//Act
			var src = new ApplyWfhService(uow.Object, _attService);
			await src.CustomValidation(WFH, result);

			//Assert
			Assert.True(result.HasError);
			Assert.True(result.Messages[0].Description == "Leaves applied for these dates.");
		}

		[Fact]
		public async Task CustomValidation_AlreadyLeavesAppliedForThisDates_ThrowException()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var result = new Result<ApplyWfh>();
			#region Arrange

			IEnumerable<ApplyWfh> data = new List<ApplyWfh>
			{
				new ApplyWfh
				{
					Employee = employee,
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					AdminReason = "reason",
					FromDateC = DateTime.Now,
					ToDateC = DateTime.Now,
					ReasonForWFH = "WFH reason",
					Status = (int)WfhStatus.Rejected,
					ApprovedById=Guid.NewGuid()
				},
				new ApplyWfh
				{
					EmployeeId = employee.ID,
					Employee = employee,
					ID = wfhId,
					AdminReason = "reason",
					FromDateC = DateTime.Now.Date.AddDays(1),
					ToDateC = DateTime.Now.Date.AddDays(1),
					ReasonForWFH = "WFH reason",
					Status = (int)WfhStatus.Approved,
					ApprovedById=employee.ReportingToId
				}

			};
			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Employee=employee,
					FromDate = DateTime.Now.Date.AddDays(4),
					ToDate = DateTime.Now.Date.AddDays(4),
					FromHalf = false,
					ToHalf = true,
					Status = (int)WfhStatus.Applied,
				},
				new ApplyLeave
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Employee=employee,
					FromDate = DateTime.Now.AddDays(3),
					ToDate = DateTime.Now.AddDays(3),
					Status = 1,
				}

			}.AsQueryable();

			var appliedClientVisit = new List<ApplyClientVisits>
			{
				new ApplyClientVisits
				{
					EmployeeId = employee.ID,
					FromDate = DateTime.Now.Date.AddDays(-4),
					ToDate = DateTime.Now.Date.AddDays(-2),
					PlaceOfVisit = "USA",
					Status = (int)ClientVisitStatus.Applied,
				}
			}.AsQueryable();

			#endregion

			#region Mock

			//ApplyWFH 
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockSet);

			//ApplyLeave
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//ClientVisit
			var mockApplyClientVisits = appliedClientVisit.BuildMockDbSet();
			SetData.MockApplyClientVisits(uow, _context, mockApplyClientVisits);
			#endregion

			ApplyWfh WFH = new()
			{
				EmployeeId = employee.ID,
				ID = Guid.NewGuid(),
				Employee = employee,
				AdminReason = "reason",
				FromDateC = DateTime.Now.Date.AddDays(4),
				ToDateC = DateTime.Now.Date.AddDays(4),
				Status = (int)WfhStatus.Applied,
				FromHalf = false,
				ToHalf = true,
				ReasonForWFH = "WFH reason",
			};

			//Act
			var src = new ApplyWfhService(uow.Object, _attService);
			await src.CustomValidation(WFH, result);

			//Assert
			Assert.True(result.HasError);
			Assert.True(result.Messages[0].Description == "Leaves applied for these dates.");
		}

		[Fact]
		public async Task CustomValidation_FirstHalfOrSecondHalfShouldSelect_ThrowException()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var result = new Result<ApplyWfh>();
			ApplyWFHMockData();

			ApplyWfh WFH = new()
			{
				EmployeeId = employee.ID,
				ID = Guid.NewGuid(),
				Employee = employee,
				AdminReason = "reason",
				FromDateC = DateTime.Now.Date.AddDays(1),
				ToDateC = DateTime.Now.Date.AddDays(1),
				Status = (int)WfhStatus.Applied,
				FromHalf = true,
				ToHalf = true,
				ReasonForWFH = "WFH reason",
			};

			//Act
			var src = new ApplyWfhService(uow.Object, _attService);
			await src.CustomValidation(WFH, result);

			//Assert
			Assert.True(result.HasError);
			Assert.True(result.Messages[0].Description == "for Oneday WFH , either First half or Second Half Should select");
		}

		[Fact]
		public async Task OnBeforeAdd_DataSaved_WithOutException()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			var result = new Result<ApplyWfh>();
			ApplyWfh WFH = new()
			{
				EmployeeId = employee.ID,
				ID = Guid.NewGuid(),
				Employee = employee,
				AdminReason = "hfgh",
				FromDateC = DateTime.Now.AddDays(-1),
				ToDateC = DateTime.Now.AddDays(3),
				Status = (int)WfhStatus.Approved,
				ReasonForWFH = "dfgf",
			};

			IEnumerable<ApplyWfh> data = new List<ApplyWfh>
			{
				new ApplyWfh
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Employee=employee,
					AdminReason = "hfgh",
					FromDateC = DateTime.Now.AddDays(6),
					ToDateC = DateTime.Now.AddDays(7),
					Status = (int)WfhStatus.Approved,
					ReasonForWFH = "dfgf",
				},
				new ApplyWfh
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					AdminReason = "hfgh",
					Employee=employee,
					FromDateC = DateTime.Now.AddDays(8),
					ToDateC = DateTime.Now.AddDays(9),
					ReasonForWFH = "dfgf",
					Status =(int)WfhStatus.Approved,
				}

			};
			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Employee=employee,
					FromDate = DateTime.Now.AddDays(-4),
					ToDate = DateTime.Now.AddDays(-3),
					Status = (int)WfhStatus.Approved,
				},
				new ApplyLeave
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Employee=employee,
					FromDate = DateTime.Now.AddDays(-5),
					ToDate = DateTime.Now.AddDays(-4),
					Status = 1,
				}

				};
			IEnumerable<Attendance> attendance = new List<Attendance>
			{
				new Attendance
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Employee=employee,
					AttendanceDate=DateTime.Now.AddDays(-1),
					AttendanceStatus=(int)AttendanceStatus.WFH,
					LeaveTypeID=Guid.NewGuid()
				},
				new Attendance
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Employee=employee,
					AttendanceDate=DateTime.Now.AddDays(-2),
					AttendanceStatus=(int)AttendanceStatus.WFH,
					LeaveTypeID=Guid.NewGuid()
				}

			};

			IEnumerable<AttendanceModifyLogs> attendanceModifyLogs = new List<AttendanceModifyLogs>
			{
				new AttendanceModifyLogs
				{
					ID = Guid.NewGuid(),
				},
				new AttendanceModifyLogs
				{
					ID = Guid.NewGuid(),
				}

			};
			var appliedClientVisit = new List<ApplyClientVisits>
			{
				new ApplyClientVisits
			{
				EmployeeId = employee.ID,
				FromDate = DateTime.Now.Date.AddDays(-4),
				ToDate = DateTime.Now.Date.AddDays(-2),
				PlaceOfVisit = "USA",
				Status = (int)ClientVisitStatus.Applied,
			}
			};

			#endregion

			#region Mock

			//ApplyWFH 
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockSet);

			//ApplyLeave
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//Attendance
			var mockSetAttendance = attendance.AsQueryable().BuildMockDbSet();
			SetData.MockAttendance(uow, _context, mockSetAttendance);

			//AttendanceModifyLogs
			var mockSetAttendanceModifyLogs = attendanceModifyLogs.AsQueryable().BuildMockDbSet();
			SetData.MockAttendanceModifyLogs(uow, _context, mockSetAttendanceModifyLogs);

			//ClientVisit
			var mockApplyClientVisits = appliedClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockApplyClientVisits(uow, _context, mockApplyClientVisits);


			#endregion

			//Act
			var src = new ApplyWfhService(uow.Object, _attService);
			await src.OnBeforeAdd(WFH, result);

			//Assert
			Assert.True(result.HasNoError);
		}

		[Fact]
		public async Task OnBeforeAdd_DataSavedWithIsHalfDayTrue_WithOutException()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			var result = new Result<ApplyWfh>();
			ApplyWfh WFH = new()
			{
				EmployeeId = employee.ID,
				ID = Guid.NewGuid(),
				Employee = employee,
				AdminReason = "hfgh",
				FromDateC = DateTime.Now.AddDays(-1),
				ToDateC = DateTime.Now.AddDays(3),
				FromHalf = true,
				ToHalf = true,
				Status = (int)WfhStatus.Approved,
				ReasonForWFH = "dfgf",
			};

			IEnumerable<ApplyWfh> data = new List<ApplyWfh>
			{
				new ApplyWfh
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Employee=employee,
					AdminReason = "hfgh",
					FromDateC = DateTime.Now.AddDays(6),
					ToDateC = DateTime.Now.AddDays(7),
					Status = (int)WfhStatus.Approved,
					ReasonForWFH = "dfgf",
				},
				new ApplyWfh
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					AdminReason = "hfgh",
					Employee=employee,
					FromDateC = DateTime.Now.AddDays(8),
					ToDateC = DateTime.Now.AddDays(9),
					ReasonForWFH = "dfgf",
					Status =(int)WfhStatus.Approved,
				}

			};
			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Employee=employee,
					FromDate = DateTime.Now.AddDays(-4),
					ToDate = DateTime.Now.AddDays(-3),
					Status = (int)WfhStatus.Approved,
				},
				new ApplyLeave
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Employee=employee,
					FromDate = DateTime.Now.AddDays(-5),
					ToDate = DateTime.Now.AddDays(-4),
					Status = 1,
				}

				};
			IEnumerable<Attendance> attendance = new List<Attendance>
			{
				new Attendance
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Employee=employee,
					AttendanceDate=DateTime.Now.AddDays(-1),
					AttendanceStatus=(int)AttendanceStatus.WFH,
					LeaveTypeID=Guid.NewGuid()
				},
				new Attendance
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Employee=employee,
					AttendanceDate=DateTime.Now.AddDays(-2),
					AttendanceStatus=(int)AttendanceStatus.WFH,
					LeaveTypeID=Guid.NewGuid()
				}

			};

			IEnumerable<AttendanceModifyLogs> attendanceModifyLogs = new List<AttendanceModifyLogs>
			{
				new AttendanceModifyLogs
				{
					ID = Guid.NewGuid(),
				},
				new AttendanceModifyLogs
				{
					ID = Guid.NewGuid(),
				}

			};
			var appliedClientVisit = new List<ApplyClientVisits>
			{
				new ApplyClientVisits
			{
				EmployeeId = employee.ID,
				FromDate = DateTime.Now.Date.AddDays(-4),
				ToDate = DateTime.Now.Date.AddDays(-2),
				PlaceOfVisit = "USA",
				Status = (int)ClientVisitStatus.Applied,
			}
			};

			#endregion

			#region Mock

			//ApplyWFH 
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockSet);

			//ApplyLeave
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//Attendance
			var mockSetAttendance = attendance.AsQueryable().BuildMockDbSet();
			SetData.MockAttendance(uow, _context, mockSetAttendance);

			//AttendanceModifyLogs
			var mockSetAttendanceModifyLogs = attendanceModifyLogs.AsQueryable().BuildMockDbSet();
			SetData.MockAttendanceModifyLogs(uow, _context, mockSetAttendanceModifyLogs);

			//ClientVisit
			var mockApplyClientVisits = appliedClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockApplyClientVisits(uow, _context, mockApplyClientVisits);


			#endregion

			//Act
			var src = new ApplyWfhService(uow.Object, _attService);
			await src.OnBeforeAdd(WFH, result);

			//Assert
			Assert.True(result.HasNoError);
		}

		[Fact]
		public async Task Approve_WFHAlreadyApproved_ThrowException()
		{
			//Arrange
			ApplyWFHAproveMocData(2);

			//Act
			var applywfh = await _WFHservice.Approve(wfhId, Guid.Parse("a52ed78f-4058-44bb-a089-c0cb35c043ac"), false);

			//Assert
			Assert.Null(applywfh.ReturnValue);
			Assert.True(applywfh.HasError);
			Assert.True(applywfh.Messages[0].Description == "WFH has been approved.");
		}

		[Fact]
		public async Task Approve_YouDoNotHavePermission_ThrowException()
		{
			ApplyWFHAproveMocData(2);

			//Act
			var applywfh = await _WFHservice.Approve(wfhId, Guid.NewGuid(), false);

			//Assert
			Assert.Null(applywfh.ReturnValue);
			Assert.True(applywfh.HasError);
			Assert.True(applywfh.Messages[0].Description == "Sorry! You do not have the permission.");
		}

		[Fact]
		public async Task Reject_WFHAlreadyApproved_ThrowException()
		{
			//Arrange
			ApplyWFHRejectMockData(2);

			//Act
			var applywfh = await _WFHservice.Reject(wfhId, "ergtestg", Guid.NewGuid());

			//Assert
			Assert.Null(applywfh.ReturnValue);
			Assert.True(applywfh.HasError);
			Assert.True(applywfh.Messages[0].Description == "WFH has been approved.");
		}

		[Fact]
		public async Task Reject_WFHRejected_WithoutException()
		{
			//Arrange
			ApplyWFHRejectMockData(1);
			//Act
			var applywfh = await _WFHservice.Reject(wfhId, "Reject", Guid.NewGuid());

			//Assert
			Assert.True(applywfh.IsSuccess);
			Assert.True(applywfh.ReturnValue.ID == wfhId);
			Assert.True(applywfh.ReturnValue.AdminReason == "Reject");
			Assert.True(applywfh.ReturnValue.Status == (int)WfhStatus.Rejected);
		}

		[Fact]
		public async Task Reject_SaveChangesAsync_ThrowException()
		{
			//Arrange
			ApplyWFHRejectMockData(1);

			uow.Setup(x => x.SaveChangesAsync()).Throws(new Exception("Exception occurred while saving"));
			//Act
			var applywfh = await _WFHservice.Reject(wfhId, "Reject", Guid.NewGuid());

			//Assert
			Assert.False(applywfh.IsSuccess);
		}

		[Fact]
		public void Cancel_Invalid_ThrowException()
		{
			// Arrange
			var employee = _employeeData.GetEmployeeData();

			ApplyWFHMockData();

			//Act
			var applywfh = _WFHservice.Cancel(Guid.NewGuid(), employee.ID).Result;

			//Assert
			Assert.Null(applywfh.ReturnValue);
			Assert.True(applywfh.HasError);
			Assert.True(applywfh.Messages[0].Description == "Invalid.");
		}

		[Fact]
		public async Task Cancel_WFHAlreadyRejected_ThrowException()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();

			var data = new List<ApplyWfh>
			{
				new ApplyWfh
				{
					EmployeeId = employee.ID,
					ID = wfhId,
					Employee=employee,
					AdminReason = "hfgh",
					FromDateC = DateTime.Now,
					ToDateC = DateTime.Now,
					Status = (int)WfhStatus.Rejected,
					ReasonForWFH = "dfgf",
				},
				new ApplyWfh
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					AdminReason = "hfgh",
					Employee=employee,
					FromDateC = DateTime.Now.AddDays(1),
					ToDateC = DateTime.Now.AddDays(1),
					ReasonForWFH = "dfgf",
					Status = (int)WfhStatus.Applied,
				}

			};

			#endregion

			#region Mock
			//ApplyWFH
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockSet);

			#endregion

			//Act
			var applywfh = await _WFHservice.Cancel(wfhId, employee.ID);

			//Assert
			Assert.Null(applywfh.ReturnValue);
			Assert.True(applywfh.HasError);
			Assert.True(applywfh.Messages[0].Description == "WFH has been rejected.");
		}

		[Fact]
		public async Task Cancel_AlreadyApproved_ThrowException()
		{
			// Arrange
			var employee = _employeeData.GetEmployeeData();
			ApplyWFHMockData();

			//Act
			var applywfh = await _WFHservice.Cancel(wfhId, employee.ID);

			//Assert
			Assert.Null(applywfh.ReturnValue);
			Assert.True(applywfh.HasError);
			Assert.True(applywfh.Messages[0].Description == "WFH has been approved.");
		}

		[Fact]
		public async Task Cancel_SaveData_WithoutException()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();

			IEnumerable<ApplyWfh> data = new List<ApplyWfh>
			{
				new ApplyWfh
				{
					Employee = employee,
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					AdminReason = "reason",
					FromDateC = DateTime.Now,
					ToDateC = DateTime.Now,
					ReasonForWFH = "WFH reason",
					Status = (int)WfhStatus.Rejected,
					ApprovedById=Guid.NewGuid()
				},
				new ApplyWfh
				{
					EmployeeId = employee.ID,
					Employee = employee,
					ID = wfhId,
					AdminReason = "reason",
					FromDateC = DateTime.Now.Date.AddDays(1),
					ToDateC = DateTime.Now.Date.AddDays(1),
					ReasonForWFH = "WFH reason",
					Status = (int)WfhStatus.Applied,
					ApprovedById=employee.ReportingToId
				}

			};
			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Employee=employee,
					FromDate = DateTime.Now.AddDays(2),
					ToDate = DateTime.Now.AddDays(4),
					Status = (int)WfhStatus.Approved,
				},
				new ApplyLeave
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Employee=employee,
					FromDate = DateTime.Now.AddDays(3),
					ToDate = DateTime.Now.AddDays(3),
					Status = 1,
				}

			}.AsQueryable();

			var appliedClientVisit = new List<ApplyClientVisits>
			{
				new ApplyClientVisits
				{
					EmployeeId = employee.ID,
					FromDate = DateTime.Now.Date.AddDays(-4),
					ToDate = DateTime.Now.Date.AddDays(-2),
					PlaceOfVisit = "USA",
					Status = (int)ClientVisitStatus.Applied,
				}
			}.AsQueryable();

			#endregion

			#region Mock

			//ApplyWFH 
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockSet);

			//ApplyLeave
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//ClientVisit
			var mockApplyClientVisits = appliedClientVisit.BuildMockDbSet();
			SetData.MockApplyClientVisits(uow, _context, mockApplyClientVisits);
			#endregion

			//Act
			var applywfh = await _WFHservice.Cancel(wfhId, employee.ID);

			//Assert
			Assert.True(applywfh.IsSuccess);
			Assert.True(applywfh.ReturnValue.Status == (int)WfhStatus.Cancelled);
		}

		[Fact]
		public async Task Cancel_SaveChangesAsync_ThrowException()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();

			IEnumerable<ApplyWfh> data = new List<ApplyWfh>
			{
				new ApplyWfh
				{
					Employee = employee,
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					AdminReason = "reason",
					FromDateC = DateTime.Now,
					ToDateC = DateTime.Now,
					ReasonForWFH = "WFH reason",
					Status = (int)WfhStatus.Rejected,
					ApprovedById=Guid.NewGuid()
				},
				new ApplyWfh
				{
					EmployeeId = employee.ID,
					Employee = employee,
					ID = wfhId,
					AdminReason = "reason",
					FromDateC = DateTime.Now.Date.AddDays(1),
					ToDateC = DateTime.Now.Date.AddDays(1),
					ReasonForWFH = "WFH reason",
					Status = (int)WfhStatus.Applied,
					ApprovedById=employee.ReportingToId
				}

			};
			IEnumerable<ApplyLeave> applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Employee=employee,
					FromDate = DateTime.Now.AddDays(2),
					ToDate = DateTime.Now.AddDays(4),
					Status = (int)WfhStatus.Approved,
				},
				new ApplyLeave
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					Employee=employee,
					FromDate = DateTime.Now.AddDays(3),
					ToDate = DateTime.Now.AddDays(3),
					Status = 1,
				}

			}.AsQueryable();

			var appliedClientVisit = new List<ApplyClientVisits>
			{
				new ApplyClientVisits
				{
					EmployeeId = employee.ID,
					FromDate = DateTime.Now.Date.AddDays(-4),
					ToDate = DateTime.Now.Date.AddDays(-2),
					PlaceOfVisit = "USA",
					Status = (int)ClientVisitStatus.Applied,
				}
			}.AsQueryable();

			#endregion

			#region Mock

			//ApplyWFH 
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockSet);

			//ApplyLeave
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//ClientVisit
			var mockApplyClientVisits = appliedClientVisit.BuildMockDbSet();
			SetData.MockApplyClientVisits(uow, _context, mockApplyClientVisits);
			#endregion

			uow.Setup(x => x.SaveChangesAsync()).Throws(new Exception("Exception occurred while saving"));

			//Act
			var applywfh = await _WFHservice.Cancel(wfhId, employee.ID);

			//Assert
			Assert.False(applywfh.IsSuccess);
			Assert.Null(applywfh.ReturnValue);
		}

		[Fact]
		public async Task RejectWFHAfterApprove_AttendanceAlreadyExecuted_ThrowException()
		{
			//Arrange
			var rejectEmpId = Guid.NewGuid();
			var employee = _employeeData.GetEmployeeData();
			ApplyWFHMockData();

			var attendance = new List<Attendance>
			{
				new Attendance
				{
					ID = Guid.NewGuid(),
					EmployeeId = employee.ID,
					AttendanceDate = DateTime.Now,
					AttendanceStatus = (int)AttendanceStatus.Present,
				},
				new Attendance
				{
					ID = Guid.NewGuid(),
					EmployeeId = employee.ID,
					AttendanceDate = DateTime.Now.AddDays(1),
					AttendanceStatus = (int)AttendanceStatus.Present,
				}
			};
			//Mock

			var mockAttendance = attendance.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<Attendance>()).Returns(mockAttendance.Object);
			var repository = new RepositoryAsync<Attendance>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Attendance>()).Returns(repository);
			//Act
			var result = await _WFHservice.RejectAfterApprove(wfhId, "Rejected after approve", rejectEmpId);
			//Assert
			Assert.Null(result.ReturnValue);
			Assert.True(result.HasError);
			Assert.True(result.Messages[0].Description == "For this days attendance already executed");
		}

		[Fact]
		public async Task RejectWFHAfterApprove_Status_Verify()
		{
			//Arrange
			var rejectEmpId = Guid.NewGuid();
			var employee = _employeeData.GetEmployeeData();
			ApplyWFHMockData();

			var attendance = new List<Attendance>
			{
				new Attendance
				{
					ID = Guid.NewGuid(),
					EmployeeId = employee.ID,
					AttendanceDate = DateTime.Now.AddDays(-3),
					AttendanceStatus = (int)AttendanceStatus.Present,
				},
			};
			//Mock

			var mockAttendance = attendance.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<Attendance>()).Returns(mockAttendance.Object);
			var repository = new RepositoryAsync<Attendance>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Attendance>()).Returns(repository);
			//Act
			var dd = await _WFHservice.RejectAfterApprove(wfhId, "Rejected after approve", rejectEmpId);
			//Assert
			Assert.Equal((int)WfhStatus.Rejected, dd.ReturnValue.Status);
			Assert.True(dd.IsSuccess);
		}

		[Fact]
		public async Task RejectWFHAfterApprove_SaveChangesAsync_ThrowException()
		{
			//Arrange
			var rejectEmpId = Guid.NewGuid();
			var employee = _employeeData.GetEmployeeData();
			ApplyWFHMockData();

			var attendance = new List<Attendance>
			{
				new Attendance
				{
					ID = Guid.NewGuid(),
					EmployeeId = employee.ID,
					AttendanceDate = DateTime.Now.AddDays(-3),
					AttendanceStatus = (int)AttendanceStatus.Present,
				},
			};
			//Mock

			var mockAttendance = attendance.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<Attendance>()).Returns(mockAttendance.Object);
			var repository = new RepositoryAsync<Attendance>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Attendance>()).Returns(repository);
			uow.Setup(x => x.SaveChangesAsync()).Throws(new Exception("Exception occurred while saving"));
			//Act
			var result = await _WFHservice.RejectAfterApprove(wfhId, "Rejected after approve", rejectEmpId);
			//Assert
			Assert.False(result.IsSuccess);
		}

		[Fact]
		public async Task GetPastFutureWFH_GetValidRecords()
		{
			// Arrange
			var employee = _employeeData.GetEmployeeData();
			ApplyWFHMockData();
			//Act
			var wfh = await _WFHservice.GetPastFutureWFH(employee.ID, DateTime.Now, DateTime.Now.AddDays(1));

			//Assert
			Assert.Equal(2, wfh.Count(x => x.EmployeeId == employee.ID));
		}

		[Theory]
		#region InlineData
		//WFH applied for one day with half day validations
		[InlineData(1, 1, true, false, false)]//Apply WFH with from half for leave approved first day getting no exception
		[InlineData(1, 1, false, true, true)]//Apply WFH with to half for leave approved first day getting exception
		[InlineData(3, 3, true, false, true)]//Apply WFH with from half for leave approved last day getting exception
		[InlineData(3, 3, false, true, false)]//Apply WFH with to half for leave approved last day getting no exception
		[InlineData(2, 2, false, true, true)]//Apply WFH between leave approved dates getting exception

		//WFH applied for more then one day without half day validations
		[InlineData(0, 1, false, false, true)]//Apply WFH with before day to first day of approved leaves dates
		[InlineData(0, 2, false, false, true)]//Apply WFH with before day to second day of approved leaves dates
		[InlineData(0, 3, false, false, true)]//Apply WFH with before day to third day of approved leaves dates
		[InlineData(0, 4, false, false, true)]//Apply WFH with before day to after one day of approved leaves dates
		[InlineData(2, 3, false, false, true)]//Apply WFH with second day to third day of approved leaves dates
		[InlineData(2, 4, false, false, true)]//Apply WFH with second day to after one day of approved leaves dates
		[InlineData(3, 4, false, false, true)]//Apply WFH with last day to after one day of approved leaves dates
		#endregion
		public async Task AddAsync_WFHAppliedForLeaveApprovedForMultipleDays_Test(int fromDate, int toDate, bool fromHalf, bool toHalf, bool isException)
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();

			//Leave approved for multiple days with fromHalf = true and toHalf = true
			var applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Employee=employee,
					FromDate = DateTime.Now.Date.AddDays(1),
					ToDate = DateTime.Now.Date.AddDays(3),
					FromHalf = true,
					ToHalf = true,
					Status = (int)WfhStatus.Approved,
				},
			};

			ApplyWfh WFH = new()
			{
				EmployeeId = employee.ID,
				ID = Guid.NewGuid(),
				Employee = employee,
				AdminReason = "hfgh",
				FromDateC = DateTime.Now.Date.AddDays(fromDate),
				ToDateC = DateTime.Now.Date.AddDays(toDate),
				FromHalf = fromHalf,
				ToHalf = toHalf,
				Status = (int)WfhStatus.Applied,
				ReasonForWFH = "dfgf",
			};

			#endregion

			#region Mock

			//ApplyWFH 
			var mockSet = new List<ApplyWfh>().AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockSet);

			//ApplyLeave
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//ClientVisit
			var mockApplyClientVisits = new List<ApplyClientVisits>().AsQueryable().BuildMockDbSet();
			SetData.MockApplyClientVisits(uow, _context, mockApplyClientVisits);


			#endregion

			//Act
			var src = new ApplyWfhService(uow.Object, _attService);
			var result = await src.AddAsync(WFH);

			//Assert
			if (isException)
			{
				Assert.True(result.HasError);
				Assert.True(result.Messages[0].Description == "Leaves applied for these dates.");
			}
			else
			{
				Assert.True(result.HasNoError);
				Assert.True(result.ReturnValue.FromHalf == fromHalf);
				Assert.True(result.ReturnValue.Status == (int)WfhStatus.Applied);
			}
		}

		[Theory]
		#region InlineData
		//WFH applied for one day with half day validations
		[InlineData(1, 1, false, true, false)]//Apply WFH with to half for leave approved date getting no exception
		[InlineData(1, 1, true, false, true)]//Apply WFH with from half for leave approved date getting exception

		//WFH applied for more then one day with half day validations
		//WFH applied for two days before approved leave date to approved leave date with all form half and to half scenarios
		[InlineData(-1, 1, true, true, true)]
		[InlineData(-1, 1, true, false, true)]
		[InlineData(-1, 1, false, true, true)]
		[InlineData(-1, 1, false, false, true)]

		//WFH applied for one day before approved leave date to after one day of approved leave date with all form half and to half scenarios
		[InlineData(0, 2, true, true, true)]
		[InlineData(0, 2, true, false, true)]
		[InlineData(0, 2, false, true, true)]
		[InlineData(0, 2, false, false, true)]

		//WFH applied for approved leave date to after one day of approved leave date with all form half and to half scenarios
		[InlineData(1, 2, true, true, true)]
		[InlineData(1, 2, true, false, true)]
		[InlineData(1, 2, false, true, true)]
		[InlineData(1, 2, false, false, true)]
		#endregion
		public async Task AddAsync_WFHAppliedForLeaveApprovedForOneDay_Test(int fromDate, int toDate, bool fromHalf, bool toHalf, bool isException)
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();

			//Leave approved for one day with fromHalf = true and toHalf = false
			var applyLeave = new List<ApplyLeave>
			{
				new ApplyLeave
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					Employee=employee,
					FromDate = DateTime.Now.Date.AddDays(1),
					ToDate = DateTime.Now.Date.AddDays(1),
					FromHalf = true,
					ToHalf = false,
					Status = (int)WfhStatus.Approved,
				},
			};

			ApplyWfh WFH = new()
			{
				EmployeeId = employee.ID,
				ID = Guid.NewGuid(),
				Employee = employee,
				AdminReason = "hfgh",
				FromDateC = DateTime.Now.Date.AddDays(fromDate),
				ToDateC = DateTime.Now.Date.AddDays(toDate),
				FromHalf = fromHalf,
				ToHalf = toHalf,
				Status = (int)WfhStatus.Applied,
				ReasonForWFH = "dfgf",
			};

			#endregion

			#region Mock

			//ApplyWFH 
			var mockSet = new List<ApplyWfh>().AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockSet);

			//ApplyLeave
			var mockSetApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSetApplyLeave);

			//ClientVisit
			var mockApplyClientVisits = new List<ApplyClientVisits>().AsQueryable().BuildMockDbSet();
			SetData.MockApplyClientVisits(uow, _context, mockApplyClientVisits);


			#endregion

			//Act
			var src = new ApplyWfhService(uow.Object, _attService);
			var result = await src.AddAsync(WFH);

			//Assert
			if (isException)
			{
				Assert.True(result.HasError);
				Assert.True(result.Messages[0].Description == "Leaves applied for these dates.");
			}
			else
			{
				Assert.True(result.HasNoError);
				Assert.True(result.ReturnValue.FromHalf == fromHalf);
				Assert.True(result.ReturnValue.Status == (int)WfhStatus.Applied);
			}
		}

	}

}
