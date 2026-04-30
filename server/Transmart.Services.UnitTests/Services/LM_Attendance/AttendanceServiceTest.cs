using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
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
using TranSmart.Domain.Models.Schedules;
using TranSmart.Service.Leave;
using TranSmart.Service.LM_Attendance;
using TranSmart.Service.Schedules;
using Xunit;

namespace Transmart.Services.UnitTests.Services.LM_Attendance
{
	public class AttendanceServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly AttendanceService attendanceService;
		private readonly Mock<ILeaveBalanceService> _leaveBalanceService;
		private readonly EmployeeDataGenerator _employeeData;
		private readonly Mock<IScheduleService> _schService;
		private readonly Mock<DbContext> _context;
		private readonly IAttendanceRepository _attendanceRepository;
		public AttendanceServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: "InMemoryDatabase");
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_employeeData = new EmployeeDataGenerator();
			_schService = new Mock<IScheduleService>();
			_leaveBalanceService = new Mock<ILeaveBalanceService>();
			_context = new Mock<DbContext>();
			_attendanceRepository = new AttendanceRepository(_dbContext.Object);
			attendanceService = new AttendanceService(uow.Object, _schService.Object, _leaveBalanceService.Object, _attendanceRepository)
			{

			};
		}
		[Theory]
		[InlineData("d1e53b8b-7316-4951-a1bd-985fb14bdf6c")]
		public void GetAttWithSchedule_Test(Guid id)
		{
			//Arrange
			var employee = _employeeData.GetEmployeeData();
			var employees = _employeeData.GetAllEmployeesData();
			var attendance = new Attendance
			{
				ID = Guid.Parse("d1e53b8b-7316-4951-a1bd-985fb14bdf6c"),
				EmployeeId = employee.ID,
				Employee = employee,
				AttendanceDate = DateTime.Today.Date,
				AttendanceStatus = (int)AttendanceStatus.Present,
			};
			//Employee Mock
			var mockSetEmployee = employees.AsQueryable().BuildMockDbSet();
			SetData.MockEmployee(uow, _context, mockSetEmployee);
			//Mock GetEmployeeSchedule
			_schService.Setup(x => x.GetEmployeeSchedule(It.IsAny<Employee>()))
				.Returns(Task.FromResult(new ScheduleDetails { BreakTime = 1, NoOfBreaks = 1, StartAt = 9, EndsAt = 18, LoginGraceTime = 9 }));

			var dd = attendanceService.GetAttWithSchedule(attendance);
			Assert.Equal(id, dd.Result.ID);

		}


		[Theory]
		[InlineData((int)AttendanceStatus.Present, (int)AttendanceStatus.Present, (int)AttendanceStatus.HalfDayLeave, 0, 1, (int)AttendanceStatus.Present, false)]
		[InlineData((int)AttendanceStatus.Present, (int)AttendanceStatus.Absent, (int)AttendanceStatus.Absent, 1, 0, (int)AttendanceStatus.Absent, false)]
		[InlineData((int)AttendanceStatus.HalfDayLeave, (int)AttendanceStatus.Absent, (int)AttendanceStatus.Absent, 1, 0, (int)AttendanceStatus.Absent, false)]//AttendanceStatus is HalfDayLeave
		[InlineData((int)AttendanceStatus.HalfDayLeave, (int)AttendanceStatus.Absent, (int)AttendanceStatus.Absent, 1, 0, (int)AttendanceStatus.Absent, true)]//AttendanceStatus is HalfDayLeave and isHalfDay True
		[InlineData((int)AttendanceStatus.HalfDayLeave, (int)AttendanceStatus.Absent, (int)AttendanceStatus.HalfDayLeave, 1, 0, (int)AttendanceStatus.Absent, true)]//AttendanceStatus is HalfDayLeave, isHalfDay True and halfDay is HalfDayLeave

		public async Task ChangeAttStatus_Test(int attendanceStatus, int status, int halfDay, int absentVal, int leaveVal, int attRes, bool isHalfDay)
		{
			//Arrange 
			Guid leaveTypeId = Guid.NewGuid();
			var employee = _employeeData.GetEmployeeData();
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
			List<Attendance> attendance = new()
			{
				new Attendance
				{
					EmployeeId = employee.ID,
					LeaveTypeID = leaveTypeId,
					AttendanceDate = DateTime.Parse("2022-08-20"),
					IsHalfDay = true,
					AttendanceStatus = 8,
					HalfDayType = 8
				},
				new Attendance
				{
					EmployeeId = employee.ID,
					LeaveTypeID = leaveTypeId,
					AttendanceDate = DateTime.Parse("2022-08-20"),
					IsHalfDay = true,
					AttendanceStatus = 8,
					HalfDayType = 8
				}

			};

			var attendanceParam = new Attendance
			{
				ID = Guid.Parse("8a9dcd4e-722a-4014-aedd-3c6819dfce67"),
				AttendanceStatus = attendanceStatus,
				EmployeeId = employee.ID
			};
			//Attendance Mock
			var mockSetAttendanc = attendance.AsQueryable().BuildMockDbSet();
			SetData.MockAttendance(uow, _context, mockSetAttendanc);
			//AttendanceModifyLogs
			var mockSetAttendanceModifyLogs = attendanceModifyLogs.AsQueryable().BuildMockDbSet();
			SetData.MockAttendanceModifyLogs(uow, _context, mockSetAttendanceModifyLogs);
			//Act
			var result = await attendanceService.ChangeAttStatus(attendanceParam, status, 4m, absentVal, leaveVal,1m, isHalfDay,
																	halfDay, true, leaveTypeId, (int)AttendanceStatus.Unautherized);
			//Assert
			Assert.Equal(leaveTypeId, result.LeaveTypeID);
			Assert.Equal(result.AttendanceStatus, attRes);
		}

		[Fact]

		public async Task GetWeekOffDays_Test()
		{
			//Arrange 
			var weekOffSetup = new WeekOffSetup()
			{
				Name = "weekOffSetup",
				Status = true,
			};
			var weekOffDays = new List<WeekOffDays>
				{
					new WeekOffDays
					{
						ID=Guid.NewGuid(),
						Type=3,
						WeekDate=DateTime.Today,
						WeekOffSetup=weekOffSetup,
					},
					new WeekOffDays
					{
						ID=Guid.NewGuid(),
						Type=3,
						WeekDate=DateTime.Today.AddDays(1),
						WeekOffSetup=weekOffSetup,
					},
					 new WeekOffDays
					{
						ID=Guid.NewGuid(),
						Type=1,
						WeekOffSetup=weekOffSetup,
					},
					 new WeekOffDays
					{
						ID=Guid.NewGuid(),
						Type=2,
						WeekOffSetup=weekOffSetup,
					}
				};
			//Mock
			var mockSetWeekOffDays = weekOffDays.AsQueryable().BuildMockDbSet();
			SetData.MockWeekOffDays(uow, _context, mockSetWeekOffDays);

			//Act
			var result = await attendanceService.GetWeekOffDays(DateTime.Today);
			//Assert
			Assert.Equal(1, result.Count(x => x.Type == 3));
			Assert.Equal(3, result.Count);
		}
	}
}
