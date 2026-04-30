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
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.OnlineTest;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Service.Leave;
using TranSmart.Service.LM_Attendance;
using TranSmart.Service.Schedules;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Leave
{
	public class ApplyClientVisitsServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly IApplyClientVisitsService _service;
		private readonly EmployeeDataGenerator _employeeData;
		private readonly Mock<DbContext> _context;
		private readonly IAttendanceRepository _attRepository;
		private readonly IShiftService _shiftService;
		private readonly IAttendanceService _attService;
		private readonly IScheduleService _scheduleService;
		private readonly ILeaveBalanceService _leaveBalanceService;
		public ApplyClientVisitsServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_leaveBalanceService = new LeaveBalanceService(uow.Object);
			_attRepository = new AttendanceRepository(_dbContext.Object);
			_shiftService = new ShiftService(uow.Object);
			_scheduleService = new ScheduleService(uow.Object, _shiftService);
			_attService = new AttendanceService(uow.Object, _scheduleService, _leaveBalanceService, _attRepository);
			_service = new ApplyClientVisitsService(uow.Object, _attService);
			_employeeData = new EmployeeDataGenerator();
			_context = new Mock<DbContext>();
		}

		private void ApplyClientVisitsMockData()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			IEnumerable<ApplyClientVisits> data = new List<ApplyClientVisits>
			{
				new ApplyClientVisits
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.NewGuid(),
					AdminReason = "Reason",
					PlaceOfVisit="HYD",
					PurposeOfVisit ="Client Requirement",
					FromDate = DateTime.Now.AddDays(-6),
					ToDate = DateTime.Now.AddDays(-4),
					Status = (int)ClientVisitStatus.Approved,
				},
				new ApplyClientVisits
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					AdminReason = "Reason",
					PlaceOfVisit="HYD",
					PurposeOfVisit ="Client Requirement",
					FromDate = DateTime.Now.AddDays(-2),
					ToDate = DateTime.Now.AddDays(-2),
					Status = (int)ClientVisitStatus.Applied,
				}
			};
			var applyLeave = new List<ApplyLeave> { new ApplyLeave
			{
				EmployeeId = employee.ID,
				FromDate = DateTime.Now.AddDays(-8),
				ToDate = DateTime.Now.AddDays(-8),
				NoOfLeaves = 1,
				ApplyLeaveType = new List<ApplyLeaveType>{new ApplyLeaveType { NoOfLeaves = 1,LeaveTypeId = Guid.NewGuid()} },
				Status = (int)ApplyLeaveSts.Approved,
			} };
			var wfh = new List<ApplyWfh> { new ApplyWfh
			{
				EmployeeId = employee.ID,
				FromDateC = DateTime.Now.AddDays(-1),
				ToDateC = DateTime.Now.AddDays(-1),
				Status = (int)WfhStatus.Approved,
			} };
			#endregion

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockApplyClientVisits(uow, _context, mockSet);

			var mockLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockLeave);

			var mockWfh = wfh.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWfh);
		}

		[Fact]
		public async Task Get_SearchWithId_GetValidRecords()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();

			IEnumerable<ApplyClientVisits> data = new List<ApplyClientVisits>
			{
				new ApplyClientVisits
				{
					EmployeeId = Guid.NewGuid(),
					ID = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					AdminReason = "Reason",
					PlaceOfVisit="HYD",
					PurposeOfVisit ="Client Requirement",
					FromDate = DateTime.Parse("2022-07-15"),
					ToDate = DateTime.Parse("2022-07-15"),
					Status = 1,
				},
				new ApplyClientVisits
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					AdminReason = "Reason",
					PlaceOfVisit="HYD",
					PurposeOfVisit ="Client Requirement",
					FromDate = DateTime.Parse("2022-07-15"),
					ToDate = DateTime.Parse("2022-07-15"),
					Status = 1,
				}

			}.AsQueryable();


			#endregion

			//Mock
			var mockSetApplyClientVisits = data.AsQueryable().BuildMockDbSet();
			SetData.MockApplyClientVisits(uow, _context, mockSetApplyClientVisits);

			//Act
			var applyClient = await _service.GetById(Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"));

			//Assert
			Assert.Equal(Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"), applyClient.ID);
		}

		[Fact]
		public async Task GetPaginate_SearchWithReferenceAndFromDateToToDate_GetValidRecords()
		{
			#region Arrange
			var employee = _employeeData.GetEmployeeData();
			ApplyClientVisitSearch baseSearch = new()
			{
				FromDate = DateTime.Parse("2022-07-1"),
				ToDate = DateTime.Parse("2022-07-30"),
				Status = 0,
				SortBy = "Employee",
				Size = 10,
				Page = 0,
				RefId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6")
			};
			IEnumerable<ApplyClientVisits> data = new List<ApplyClientVisits>
			{
				new ApplyClientVisits
				{
					EmployeeId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					ID = Guid.NewGuid(),
					AdminReason = "Reason",
					PlaceOfVisit="HYD",
					PurposeOfVisit ="Client Requirement",
					FromDate = DateTime.Parse("2022-07-15"),
					ToDate = DateTime.Parse("2022-07-15"),
					Status = 1,
				},
				new ApplyClientVisits
				{
					EmployeeId = employee.ID,
					ID = Guid.NewGuid(),
					AdminReason = "Reason",
					PlaceOfVisit="HYD",
					PurposeOfVisit ="Client Requirement",
					FromDate = DateTime.Parse("2022-07-15"),
					ToDate = DateTime.Parse("2022-07-15"),
					Status = 1,
				}

			};


			#endregion

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			SetData.MockApplyClientVisits(uow, _context, mockSet);

			//Act
			var applyClient = await _service.GetPaginate(baseSearch);

			//Assert
			Assert.Equal(1, applyClient.Items.Count(x => x.EmployeeId == Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6")));
		}

		[Fact]
		public async Task CustomValidation_DataAdded_WithOutException()
		{
			var employee = _employeeData.GetEmployeeData();
			Result<ApplyClientVisits> result = new();
			ApplyClientVisitsMockData();

			ApplyClientVisits applyClientVisits = new()
			{
				EmployeeId = employee.ID,
				ID = Guid.NewGuid(),
				AdminReason = "Reason",
				PlaceOfVisit = "KNR",
				PurposeOfVisit = "Work",
				FromDate = DateTime.Parse("2022-07-17"),
				ToDate = DateTime.Parse("2022-07-18"),
				Status = 1,
			};
			var applyLeave = new List<ApplyLeave>();

			//Mock
			var mockSet = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSet);

			//Act
			var service = new ApplyClientVisitsService(uow.Object, _attService);
			await service.CustomValidation(applyClientVisits, result);

			//Assert
			Assert.True(result.HasNoError);
		}

		[Fact]
		public async Task CustomValidation_FromDateLessThenToDate_ThrowException()
		{
			var employee = _employeeData.GetEmployeeData();
			Result<ApplyClientVisits> result = new();
			ApplyClientVisitsMockData();

			ApplyClientVisits applyClientVisits = new()
			{
				EmployeeId = employee.ID,
				ID = Guid.NewGuid(),
				AdminReason = "Reason",
				PlaceOfVisit = "KNR",
				PurposeOfVisit = "Work",
				FromDate = DateTime.Parse("2022-07-17"),
				ToDate = DateTime.Parse("2022-07-16"),
				Status = 1,
			};
			var applyLeave = new List<ApplyLeave>();

			//Mock
			var mockSet = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSet);

			//Act
			var service = new ApplyClientVisitsService(uow.Object, _attService);
			await service.CustomValidation(applyClientVisits, result);

			//Assert
			Assert.True(result.HasError);
		}

		[Fact]
		public async Task CustomValidation_AlreadyApplied_ThrowException()
		{
			var employee = _employeeData.GetEmployeeData();
			Result<ApplyClientVisits> result = new();
			ApplyClientVisitsMockData();

			ApplyClientVisits applyClientVisits = new()
			{
				EmployeeId = employee.ID,
				ID = Guid.NewGuid(),
				AdminReason = "Reason",
				PlaceOfVisit = "KNR",
				PurposeOfVisit = "Work",
				FromDate = DateTime.Now.AddDays(-3),
				ToDate = DateTime.Now.AddDays(-1),
				Status = 1,
			};

			var applyLeave = new List<ApplyLeave>();

			//Mock
			var mockSet = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockSet);

			//Act
			var service = new ApplyClientVisitsService(uow.Object, _attService);
			await service.CustomValidation(applyClientVisits, result);

			//Assert
			Assert.True(result.HasError);
		}
		[Fact]
		public async Task CustomValidation_LeaveAlreadyApplied_Throw_Exception()
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			Result<ApplyClientVisits> result = new();
			ApplyClientVisitsMockData();

			var appliedClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				EmployeeId = employee.ID,
				FromDate = DateTime.Now.Date.AddDays(-4),
				ToDate = DateTime.Now.Date.AddDays(-2),
				PlaceOfVisit = "USA",
				Status = (int)ClientVisitStatus.Applied,
			} };
			var clientVisit = new ApplyClientVisits
			{
				EmployeeId = employee.ID,
				FromDate = DateTime.Now.Date.AddDays(-9),
				ToDate = DateTime.Now.Date.AddDays(-7),
				PlaceOfVisit = "USA",
				Status = (int)ClientVisitStatus.Applied,
			};
			//mock

			var mockApplyClientVisits = appliedClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockApplyClientVisits(uow, _context, mockApplyClientVisits);
			//act
			var service = new ApplyClientVisitsService(uow.Object, _attService);
			await service.CustomValidation(clientVisit, result);
			//assert
			Assert.True(result.HasError);

		}
		[Fact]
		public async Task Cancel_Apply_ClientVisits()
		{
			//Arrange
			var clientVisitId = Guid.NewGuid();
			var applyLeaveId = Guid.NewGuid();
			var wfhId = Guid.NewGuid();
			var employee = _employeeData.GetEmployeeData();
			var appliedClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				ID = clientVisitId,
				EmployeeId = employee.ID,
				FromDate = DateTime.Now.Date.AddDays(-4),
				ToDate = DateTime.Now.Date.AddDays(-2),
				PlaceOfVisit = "USA",
				Status = (int)ClientVisitStatus.Applied,
			} };
			var applyLeave = new List<ApplyLeave> { new ApplyLeave
			{
				ID = applyLeaveId,
				EmployeeId = employee.ID,
				FromDate = DateTime.Now.Date,
				ToDate = DateTime.Now.Date.AddDays(2),
				Status = (int)ApplyLeaveSts.Applied

			} };
			var applyWfh = new List<ApplyWfh> { new ApplyWfh
			{
				ID = wfhId,
				FromDateC= DateTime.Now.AddDays(2),
				ToDateC = DateTime.Now.AddDays(2),
			} };
			//Mock
			var mockApplyClientVisits = appliedClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockApplyClientVisits(uow, _context, mockApplyClientVisits);

			var mockApplyLeave = applyLeave.AsQueryable().BuildMockDbSet();
			SetData.MockApplyLeave(uow, _context, mockApplyLeave);

			var mockWFH = applyWfh.AsQueryable().BuildMockDbSet();
			SetData.MockApplyWFH(uow, _context, mockWFH);
			//Act
			var service = new ApplyClientVisitsService(uow.Object, _attService);
			var dd = await service.Cancel(clientVisitId, employee.ID);
			//Assert
			Assert.Equal((int)ClientVisitStatus.Cancelled, dd.ReturnValue.Status);
		}

		[Fact]
		public async Task Cancel_Apply_ClientVisits_Throw_Invalid_Exception()
		{
			//Arrange
			var clientVisitId = Guid.NewGuid();
			var employee = _employeeData.GetEmployeeData();
			var appliedClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				ID = Guid.NewGuid(),
				EmployeeId = employee.ID,
				FromDate = DateTime.Now.Date.AddDays(-4),
				ToDate = DateTime.Now.Date.AddDays(-2),
				PlaceOfVisit = "USA",
				Status = (int)ClientVisitStatus.Applied,
			} };
			//Mock
			var mockApplyClientVisits = appliedClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockApplyClientVisits(uow, _context, mockApplyClientVisits);
			//Act
			var service = new ApplyClientVisitsService(uow.Object, _attService);
			var dd = await service.Cancel(clientVisitId, employee.ID);
			//Assert
			Assert.True(dd.HasError);
		}

		[Theory]
		[InlineData((int)ClientVisitStatus.Approved)]
		[InlineData((int)ClientVisitStatus.Rejected)]
		public async Task Cancel_Apply_ClientVisits_Throw_Exception_AlreadyApproved(int status)
		{
			//Arrange
			var clientVisitId = Guid.NewGuid();
			var employee = _employeeData.GetEmployeeData();
			var appliedClientVisit = new List<ApplyClientVisits> { new ApplyClientVisits
			{
				ID = clientVisitId,
				EmployeeId = employee.ID,
				FromDate = DateTime.Now.Date.AddDays(-4),
				ToDate = DateTime.Now.Date.AddDays(-2),
				PlaceOfVisit = "USA",
				Status = status,
			} };
			//Mock
			var mockApplyClientVisits = appliedClientVisit.AsQueryable().BuildMockDbSet();
			SetData.MockApplyClientVisits(uow, _context, mockApplyClientVisits);
			//Act
			var service = new ApplyClientVisitsService(uow.Object, _attService);
			var dd = await service.Cancel(clientVisitId, employee.ID);
			//Assert
			Assert.True(dd.HasError);
		}
	}
}
