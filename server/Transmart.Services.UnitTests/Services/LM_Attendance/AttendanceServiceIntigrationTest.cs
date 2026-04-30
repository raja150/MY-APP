using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Data.Repository.Attendance;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models.LM_Attendance;
using TranSmart.Domain.Models.LM_Attendance.List;
using TranSmart.Service;
using TranSmart.Service.Leave;
using TranSmart.Service.LM_Attendance;
using TranSmart.Service.Schedules;
using Xunit;

namespace Transmart.Services.UnitTests.Services.LM_Attendance
{
	[CollectionDefinition("Collection #1")]
	[Xunit.TestCaseOrderer("Transmart.Services.UnitTests.PriorityOrderer", "Transmart.Services.UnitTests")]
	public class AttendanceServiceIntigrationTest // : IClassFixture<InMemoryFixture>
	{
		private readonly SequenceNoService seqNo;
		private readonly ShiftService _shiftService;
		private readonly AttendanceService _attendanceService;
		private readonly LeaveBalanceService _leaveBalanceService;
		private readonly IApplyLeaveService _applyLeaveService;
		private readonly IApplyWfhService _applyWFHService;
		private readonly ScheduleService _scheduleService;
		private readonly Guid departmentId = Guid.NewGuid();
		private readonly Guid designationId = Guid.NewGuid();
		private readonly Guid workTypeId = Guid.NewGuid();
		private readonly Guid teamId = Guid.NewGuid();
		private readonly Guid workLocationId = Guid.NewGuid();
		private readonly Guid holidayId = Guid.NewGuid();
		private readonly Guid _employeeId = Guid.NewGuid();
		private readonly Guid weekOffSetupId = Guid.NewGuid();
		private readonly Guid leaveTypeId = Guid.NewGuid();
		private readonly string _employeeCode = "1832";
		private readonly UnitOfWork<TranSmartContext> _uow;
		private readonly TranSmartContext dbContext;
		private readonly AttendanceRepository _attendanceRepository;

		public AttendanceServiceIntigrationTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			builder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
			var dbContextOptions = builder.Options;
			var services = new ServiceCollection();
			services.AddScoped<IApplicationUser, TranSmart.Services.UnitTests.ApplicationUser>();
			dbContext = new TranSmartContext(dbContextOptions, new TranSmart.Services.UnitTests.ApplicationUser());
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
			_uow = new UnitOfWork<TranSmartContext>(dbContext);
			seqNo = new SequenceNoService(_uow);
			_shiftService = new ShiftService(_uow);
			_scheduleService = new ScheduleService(_uow, _shiftService);
			_leaveBalanceService = new LeaveBalanceService(_uow);
			_attendanceRepository = new AttendanceRepository(dbContext);
			_attendanceService = new AttendanceService(_uow, _scheduleService, _leaveBalanceService, _attendanceRepository);
			_applyLeaveService = new ApplyLeaveService(_uow, _attendanceService, _leaveBalanceService);
			_applyWFHService = new ApplyWfhService(_uow, _attendanceService);

			dbContext.Organization_Department.Add(new Department
			{
				ID = departmentId,
				Name = "IT Development",
			});
			dbContext.SaveChangesAsync();

			dbContext.Organization_Designation.Add(new Designation
			{
				ID = designationId,
				Name = "Jr Software Developer",
			});
			dbContext.SaveChangesAsync();
			dbContext.Organization_Location.Add(new Location
			{
				ID = workLocationId,
				Name = "HYD",
			});
			dbContext.SaveChangesAsync();
			dbContext.Organization_Team.Add(new Team
			{
				ID = teamId,
				Name = "IT",
			});
			dbContext.SaveChangesAsync();
			dbContext.Organization_WorkType.Add(new WorkType
			{
				ID = workTypeId,
				Name = "Full time",
				CalculateAtt = true
			});
			dbContext.SaveChangesAsync();

			dbContext.Organization_Employee.AddRange(
				new Employee
				{
					ID = _employeeId,
					No = "AVONTIX" + _employeeCode,
					Name = "Anudeep",
					Gender = 1,
					Status = 1,
					MaritalStatus = 2,
					MobileNumber = "9639639632",
					DateOfBirth = new DateTime(1989, 02, 02),
					DateOfJoining = new DateTime(2020, 08, 12),
					AadhaarNumber = "561250752388",
					PanNumber = "BLMPJ2797L",
					DepartmentId = departmentId,
					DesignationId = designationId,
					WorkLocationId = workLocationId,
					WorkLocation = new Location { ID = Guid.NewGuid(), Name = "Jubilee Hills" },
					TeamId = teamId,
					Team = new Team
					{
						ID = Guid.NewGuid(),
						Name = "Regular",
					},
					WorkTypeId = workTypeId,
					LastWorkingDate = DateTime.Today.AddDays(-2),
					AllowWebPunch = true,
					FirstName = "Anudeep",
					DOBC = new DateTime(1989, 02, 02),
				},
				new Employee
				{
					ID = Guid.NewGuid(),
					No = "AVONTIX1833",
					Name = "Vamshi",
					Gender = 1,
					Status = 1,
					MaritalStatus = 2,
					MobileNumber = "9639639632",
					DateOfBirth = new DateTime(1989, 02, 02),
					DateOfJoining = new DateTime(2020, 08, 12),
					AadhaarNumber = "561250752388",
					PanNumber = "BLMPJ2797L",
					DepartmentId = departmentId,
					DesignationId = designationId,
					WorkLocationId = workLocationId,
					WorkLocation = new Location { ID = Guid.NewGuid(), Name = "Jubilee Hills" },
					TeamId = teamId,
					Team = new Team
					{
						ID = Guid.NewGuid(),
						Name = "Regular",
					},
					WorkTypeId = workTypeId,
					WorkFromHome = 1,
					FirstName = "Vamshi"
				});
			dbContext.SaveChangesAsync();

			dbContext.Leave_LeaveType.Add(new LeaveType
			{
				ID = leaveTypeId,
				Name = "Casual Leave",
				Code = "CL",
				MaxLeaves = 99,
				FutureDate = 5,
				Gender = 4,
				MaritalStatus = 3
			});
			dbContext.SaveChangesAsync();

			dbContext.Leave_LeaveBalance.Add(new LeaveBalance
			{
				LeaveTypeId = leaveTypeId,
				Leaves = 56,
				EmployeeId = _employeeId
			});
			dbContext.SaveChangesAsync();

			dbContext.Leave_ApplyLeaveDetails.Add(new ApplyLeaveDetails
			{
				LeaveDate = DateTime.Today,
				ApplyLeave = new ApplyLeave
				{
					ID = Guid.Parse("fe9f3c14-31af-4da2-ac72-a9c067296254"),
					EmployeeId = _employeeId,
					Status = 1,
				},

			});
			dbContext.SaveChangesAsync();
		}

		[Fact]
		public async Task Calculate_Attendance_Present()
		{
			var attendanceDate = new DateTime(2022, 9, 1);
			var manualAttLogs = new List<ManualAttLogs>
			{
				new ManualAttLogs
				{
					ID = Guid.NewGuid(),
					EmployeeId = _employeeId,
					InTime = attendanceDate.AddHours(9),
					OutTime = attendanceDate.AddHours(18),
					AttendanceDate = attendanceDate,
				}
			};

			_uow.Context.ChangeTracker.Clear();
			_ = await _attendanceService.ManualLogsImport(manualAttLogs);

			var addResult = await _attendanceService.CalculateAttendance(attendanceDate);
			var attendance = dbContext.HR_Attendance.FirstOrDefault(x => x.AttendanceDate == attendanceDate
								&& x.EmployeeId == _employeeId);

			Assert.True(addResult.HasNoError);
			Assert.True(attendance.AttendanceStatus == (int)AttendanceStatus.Present);
			Assert.True(attendance.InTime == attendanceDate.AddHours(9));
			Assert.True(attendance.OutTime == attendanceDate.AddHours(18));
		}
		[Fact]
		public async Task Calculate_BiometricAttendance_Present()
		{
			var attendanceDate = DateTime.Today;
			var biometricAttLogs = new List<BiometricAttLogs>
			{
				new BiometricAttLogs
				{
					ID = Guid.NewGuid(),
					MovementTime = attendanceDate.AddHours(9),
					EmpCode = _employeeCode,
					MovementType = 0
				},
				new BiometricAttLogs
				{
					ID = Guid.NewGuid(),
					MovementTime = attendanceDate.AddHours(18),
					EmpCode = _employeeCode,
					MovementType = 1
				}
			};
			dbContext.ChangeTracker.Clear();

			_ = await _attendanceService.BiometricLogsImport(biometricAttLogs);
			var addResult = await _attendanceService.CalculateAttendance(attendanceDate);
			var attendance = dbContext.HR_Attendance.FirstOrDefault(x => x.AttendanceDate == attendanceDate
								&& x.EmployeeId == _employeeId);

			Assert.True(addResult.HasNoError);
			Assert.True(attendance.AttendanceStatus == (int)AttendanceStatus.Present);
			Assert.True(attendance.InTime == attendanceDate.AddHours(9));
			Assert.True(attendance.OutTime == attendanceDate.AddHours(18));
		}
		[Fact]
		public async Task Calculate_Attendance_Absent()
		{
			dbContext.ChangeTracker.Clear();
			var addResult = await _attendanceService.CalculateAttendance(DateTime.Now);
			var attendance = dbContext.HR_Attendance.FirstOrDefault(x => x.AttendanceDate == DateTime.Today
								&& x.EmployeeId == _employeeId);

			Assert.True(addResult.HasNoError);
			Assert.True(addResult.Messages.Count == 0);
			Assert.True(attendance.AttendanceStatus == (int)AttendanceStatus.Absent);
		}
		[Fact]
		public async Task Calculate_Attendance_WorkFromHome()
		{
			var attendanceDate = DateTime.Today;
			dbContext.ChangeTracker.Clear();
			dbContext.LM_ApplyWFH.Add(new ApplyWfh
			{
				ID = Guid.NewGuid(),
				EmployeeId = _employeeId,
				FromDateC = attendanceDate,
				ToDateC = attendanceDate,
				Status = 2,
				ReasonForWFH = "Health issue"
			});
			dbContext.SaveChanges();
			_uow.Context.ChangeTracker.Clear();

			var addResult = await _attendanceService.CalculateAttendance(attendanceDate);
			var employeeAttendance = await _uow.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == _employeeId
			&& x.AttendanceDate == attendanceDate);

			Assert.True(addResult.HasNoError);
			Assert.True(addResult.Messages.Count == 0);
			Assert.True(employeeAttendance.AttendanceStatus == (int)AttendanceStatus.WFH);

		}
		[Fact]
		public async Task UpdateAttendanceStatus_WhenEmployeeWFHApproved_AfterAttendanceCalcTest()
		{
			//Arrange
			var attendanceDate = DateTime.Today.AddDays(-1);
			dbContext.ChangeTracker.Clear();
			_ = await _attendanceService.CalculateAttendance(attendanceDate);

			//Raise new WFH request
			var applyWFH = new ApplyWfh
			{
				ID = _employeeId,
				EmployeeId = _employeeId,
				FromDateC = attendanceDate,
				ToDateC = attendanceDate,
				Status = (int)WfhStatus.Applied,
				ReasonForWFH = "Corona"
			};

			//Apply WFH
			await _applyWFHService.AddAsync(applyWFH);
			dbContext.ChangeTracker.Clear();
			await _applyWFHService.Approve(_employeeId, Guid.NewGuid(), true);

			//Get attendance data  
			var attendance = await _uow.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == _employeeId
		   && x.AttendanceDate == attendanceDate);

			//Assert attendance date status should be WFH
			Assert.True(attendance.AttendanceStatus == (int)AttendanceStatus.WFH);

		}
		[Fact]
		public async Task UpdateAttendanceStatus_WhenEmployeeWFHApplied_BeforeAttendanceCalcTest()
		{
			//Arrange 
			var attendanceDate = DateTime.Today.AddDays(-1);

			// Raise new WFH request
			var applyWFH = new ApplyWfh
			{
				ID = _employeeId,
				EmployeeId = _employeeId,
				FromDateC = attendanceDate,
				ToDateC = attendanceDate,
				Status = (int)WfhStatus.Applied,
				ReasonForWFH = "Corona"
			};
			_uow.Context.ChangeTracker.Clear();
			_ = await _attendanceService.CalculateAttendance(attendanceDate);

			//Get attendance before approve data  
			var attendanceBefore = await _uow.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == _employeeId
							&& x.AttendanceDate == attendanceDate);

			//Apply WFH
			await _applyWFHService.AddAsync(applyWFH);
			_uow.Context.ChangeTracker.Clear();
			await _applyWFHService.Approve(_employeeId, Guid.NewGuid(), true);

			//Get attendance approve data  
			var attendance = await _uow.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == _employeeId
							&& x.AttendanceDate == attendanceDate);

			//Assert attendance before approve date status should be Absent
			Assert.True(attendanceBefore.AttendanceStatus == (int)AttendanceStatus.Absent);

			//Assert attendance After approve date status should be Absent
			Assert.True(attendance.AttendanceStatus == (int)AttendanceStatus.WFH);

		}
		[Fact]
		public async Task Calculate_Attendance_Leave()
		{
			var attendanceDate = DateTime.Today;
			dbContext.ChangeTracker.Clear();
			dbContext.Leave_ApplyLeave.Add(new ApplyLeave
			{
				EmployeeId = _employeeId,
				FromDate = attendanceDate,
				ToDate = attendanceDate,
				Status = (int)ApplyLeaveSts.Approved,
			});

			dbContext.Leave_ApplyLeaveDetails.AddRange(new ApplyLeaveDetails
			{
				LeaveDate = attendanceDate,
				ApplyLeave = new ApplyLeave { EmployeeId = _employeeId, Status = 2 },
				LeaveCount = 1
			});
			var calculateAttendance = new CalculateAttendance(_uow, _scheduleService)
			{
				Leaves = new List<ApplyLeaveDetails>
				{
					new ApplyLeaveDetails
					{
						LeaveDate = attendanceDate,
						ApplyLeave = new ApplyLeave{ EmployeeId = _employeeId, Status = 2 },
						LeaveCount= 1.5m
					}
				}
			};
			dbContext.SaveChanges();
			dbContext.ChangeTracker.Clear();

			var addResult = await _attendanceService.CalculateAttendance(attendanceDate);
			var employeeAttendance = await _uow.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == _employeeId
			&& x.AttendanceDate == attendanceDate);


			Assert.True(addResult.HasNoError);
			Assert.True(addResult.Messages.Count == 0);
			Assert.True(employeeAttendance.AttendanceStatus == (int)AttendanceStatus.Leave);
		}
		[Fact]
		public async Task UpdateAttendanceStatus_WhenEmployeeLeaveApproved_AfterAttendanceCalcTest()
		{
			//Arrange
			var attendanceDate = DateTime.Today;
			_uow.Context.ChangeTracker.Clear();
			_ = await _attendanceService.CalculateAttendance(attendanceDate);


			//Raise new leave request 
			var leaveRequest = new ApplyLeave
			{
				ID = _employeeId,
				EmployeeId = _employeeId,
				FromDate = attendanceDate,
				ToDate = attendanceDate,
				EmergencyContNo = "9876543212",
				Status = (int)ApplyLeaveSts.Applied,
				NoOfLeaves = 2,
				LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":2}]",
				ApplyLeaveType = new List<ApplyLeaveType>
				{
					new ApplyLeaveType
					{
						NoOfLeaves = 2,
						LeaveTypeId = leaveTypeId,
					}
				}
			};
			await _applyLeaveService.AddAsync(leaveRequest);

			//Approve it (call respective service )
			await _applyLeaveService.Approve(_employeeId, Guid.NewGuid(), true);
			//Get attendance data  
			var attendance = await _uow.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == _employeeId
			&& x.AttendanceDate == attendanceDate);

			//Assert attendance date status should be leave
			Assert.True(attendance.AttendanceStatus == (int)AttendanceStatus.Absent);

		}
		[Fact]
		public async Task UpdateAttendanceStatus_WhenEmployeeLeaveApproved_BeforeAttendanceCalcTest()
		{
			//Arrange
			var attendanceDate = DateTime.Today;

			//Raise new leave request 
			var leaveRequest = new ApplyLeave
			{
				ID = _employeeId,
				EmployeeId = _employeeId,
				FromDate = attendanceDate,
				ToDate = attendanceDate,
				EmergencyContNo = "9876543212",
				Status = (int)ApplyLeaveSts.Applied,
				LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":2}]",
				ApplyLeaveType = new List<ApplyLeaveType>
				{
					new ApplyLeaveType
					{
						NoOfLeaves = 2,
						LeaveTypeId = leaveTypeId,
					}
				}
			};
			await _applyLeaveService.AddAsync(leaveRequest);
			_uow.Context.ChangeTracker.Clear();
			_ = await _attendanceService.CalculateAttendance(attendanceDate);


			//Get attendance before approve data  
			var attendanceBefore = await _uow.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == _employeeId
							&& x.AttendanceDate == attendanceDate);
			_uow.Context.ChangeTracker.Clear();

			//Approve it (call respective service )
			await _applyLeaveService.Approve(_employeeId, Guid.NewGuid(), true);

			//Get attendance data  
			var attendance = await _uow.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == _employeeId
			&& x.AttendanceDate == attendanceDate);

			//Assert attendance date status should be Absent
			Assert.True(attendanceBefore.AttendanceStatus == (int)AttendanceStatus.Absent);
			Assert.True(attendance.AttendanceStatus == (int)AttendanceStatus.Absent);

		}
		[Fact]
		public async Task Calculate_Attendance_WeekOff()
		{
			dbContext.ChangeTracker.Clear();
			dbContext.Leave_WeekOffDays.Add(new WeekOffDays
			{
				ID = Guid.NewGuid(),
				WeekOffSetup = new WeekOffSetup { ID = weekOffSetupId, Name = "WeekOffSetup", Status = true },
				Type = 3,
				WeekDate = DateTime.Today,
				Status = 2,
			});

			dbContext.Organization_Allocation.Add(new Allocation
			{
				EmployeeId = _employeeId,
				WeekOffSetupId = weekOffSetupId,

			});
			dbContext.SaveChanges();
			dbContext.ChangeTracker.Clear();


			var addResult = await _attendanceService.CalculateAttendance(DateTime.Today);
			var employeeAttendance = await _uow.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == _employeeId
			&& x.AttendanceDate == DateTime.Today);

			Assert.True(addResult.HasNoError);
			Assert.True(addResult.Messages.Count == 0);
			Assert.True(employeeAttendance.AttendanceStatus == (int)AttendanceStatus.WeekOff);
		}
		[Fact]
		public async Task Calculate_Attendance_Holiday()
		{
			var holidayDate = new DateTime(2022, 11, 27);
			dbContext.ChangeTracker.Clear();
			dbContext.Leave_Holidays.AddRange(new Holidays
			{
				Date = holidayDate,
				Name = "Dassehra",
				ID = holidayId,
				Type = 2,
				Description = "Dassehra",
				ReprApplication = "Dassehra"

			});

			dbContext.Leave_Exemptions.AddRange(new Exemptions
			{
				ID = Guid.NewGuid(),
				HolidaysId = holidayId,
				Employees = Guid.NewGuid().ToString() + "," + Guid.NewGuid(),
			});
			dbContext.SaveChanges();
			dbContext.ChangeTracker.Clear();

			var addResult = await _attendanceService.CalculateAttendance(holidayDate);
			var employeeAttendance = await _uow.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == _employeeId
			&& x.AttendanceDate == holidayDate);

			Assert.True(addResult.HasNoError);
			Assert.True(addResult.Messages.Count == 0);
			Assert.True(employeeAttendance.AttendanceStatus == (int)AttendanceStatus.Holiday);
		}
		[Fact]
		public async Task Calculate_Attendance_HalfDayAbsent()
		{
			var attendanceDate = new DateTime(2022, 9, 1);
			dbContext.ChangeTracker.Clear();
			var attendances = new List<AttendanceDetails>
			{
				new AttendanceDetails
				{
					ID = Guid.NewGuid(),
					EmployeeId = _employeeId,
					InTime = attendanceDate.AddHours(9),
					OutTime = attendanceDate.AddHours(18),
					AttendanceDate = DateTime.Today,
					AttendanceStatus="10",

				}
			};

			dbContext.HR_Attendance.Add(new Attendance
			{
				ID = Guid.NewGuid(),
				EmployeeId = _employeeId,
				InTime = attendanceDate.AddHours(9),
				OutTime = attendanceDate.AddHours(14),
				AttendanceDate = DateTime.Today,
				AttendanceStatus = 10
			});
			dbContext.SaveChanges();
			dbContext.ChangeTracker.Clear();

			var addResult = await _attendanceService.AttendanceUpdate(attendances,Guid.NewGuid());
			var employeeAttendance = await _uow.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == _employeeId
			&& x.AttendanceDate == DateTime.Today);

			Assert.True(addResult.HasNoError);
			Assert.True(addResult.Messages.Count == 0);
			Assert.True(employeeAttendance.AttendanceStatus == (int)AttendanceStatus.HalfDayAbsent);
		}
		[Fact]
		public async Task Calculate_Attendance_HalfDayPresent()
		{
			var attendanceDate = new DateTime(2022, 9, 1);
			dbContext.ChangeTracker.Clear();
			var attendances = new List<AttendanceDetails>
			{
				new AttendanceDetails
				{
					ID = Guid.NewGuid(),
					EmployeeId = _employeeId,
					InTime = attendanceDate.AddHours(9),
					OutTime = attendanceDate.AddHours(14),
					AttendanceDate = DateTime.Today,
					AttendanceStatus="7"
				}
			};

			dbContext.HR_Attendance.Add(new Attendance
			{
				ID = Guid.NewGuid(),
				EmployeeId = _employeeId,
				InTime = attendanceDate.AddHours(9),
				OutTime = attendanceDate.AddHours(14),
				AttendanceDate = DateTime.Today,
				AttendanceStatus = 7
			});
			dbContext.SaveChanges();
			dbContext.ChangeTracker.Clear();

			var addResult = await _attendanceService.AttendanceUpdate(attendances, Guid.NewGuid());
			var employeeAttendance = await _uow.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == _employeeId
			&& x.AttendanceDate == DateTime.Today);

			Assert.True(addResult.HasNoError);
			Assert.True(addResult.Messages.Count == 0);
			Assert.True(employeeAttendance.AttendanceStatus == (int)AttendanceStatus.HalfDayPresent);
		}
		[Fact]
		public async Task Calculate_Attendance_HalfDayLeave()
		{
			var attendanceDate = new DateTime(2022, 9, 1);
			dbContext.ChangeTracker.Clear();
			var attendances = new List<AttendanceDetails>
			{
				new AttendanceDetails
				{
					ID = Guid.NewGuid(),
					EmployeeId = _employeeId,
					InTime = attendanceDate.AddHours(9),
					OutTime = attendanceDate.AddHours(14),
					AttendanceDate = DateTime.Today,
					AttendanceStatus="8",
					IsHalfDay=true,
					LeaveTypeId=_employeeId

				}
			};

			dbContext.HR_Attendance.Add(new Attendance
			{
				ID = Guid.NewGuid(),
				EmployeeId = _employeeId,
				InTime = attendanceDate.AddHours(9),
				OutTime = attendanceDate.AddHours(14),
				AttendanceDate = DateTime.Today,
				AttendanceStatus = 8,
				IsHalfDay = true,
				LeaveTypeID = _employeeId
			});

			dbContext.Leave_ApplyLeave.AddRange(new ApplyLeave
			{
				EmployeeId = _employeeId,
				FromDate = attendanceDate,
				ToDate = attendanceDate,
				Status = (int)ApplyLeaveSts.Approved,

			});
			dbContext.SaveChanges();
			dbContext.ChangeTracker.Clear();

			var addResult = await _attendanceService.AttendanceUpdate(attendances, Guid.NewGuid());
			var employeeAttendance = await _uow.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == _employeeId
			&& x.AttendanceDate == DateTime.Today);

			Assert.True(addResult.HasNoError);
			Assert.True(addResult.Messages.Count == 0);
			Assert.True(employeeAttendance.AttendanceStatus == (int)AttendanceStatus.HalfDayLeave);
		}
		[Fact]
		public async Task Calculate_Attendance_HalfDayWFH()
		{
			var attendanceDate = new DateTime(2022, 9, 1);
			dbContext.ChangeTracker.Clear();
			var attendances = new List<AttendanceDetails>
			{
				new AttendanceDetails
				{
					ID = Guid.NewGuid(),
					EmployeeId = _employeeId,
					InTime = attendanceDate.AddHours(9),
					OutTime = attendanceDate.AddHours(14),
					AttendanceDate = DateTime.Today,
					AttendanceStatus="9",
					IsHalfDay=true,
					LeaveTypeId=_employeeId

				}
			};

			dbContext.HR_Attendance.Add(new Attendance
			{
				ID = Guid.NewGuid(),
				EmployeeId = _employeeId,
				InTime = attendanceDate.AddHours(9),
				OutTime = attendanceDate.AddHours(14),
				AttendanceDate = DateTime.Today,
				AttendanceStatus = 9,
				IsHalfDay = true,
				LeaveTypeID = _employeeId
			});
			dbContext.SaveChanges();
			dbContext.ChangeTracker.Clear();

			var addResult = await _attendanceService.AttendanceUpdate(attendances, Guid.NewGuid());
			var employeeAttendance = await _uow.GetRepositoryAsync<Attendance>().SingleAsync(x => x.EmployeeId == _employeeId
			&& x.AttendanceDate == DateTime.Today);

			Assert.True(addResult.HasNoError);
			Assert.True(addResult.Messages.Count == 0);
			Assert.True(employeeAttendance.AttendanceStatus == (int)AttendanceStatus.HalfDayWFH);
		}
		[Fact]
		public async Task AttendanceUpdate_AttendanceStatus_Present()
		{
			var attendanceDate = new DateTime(2022, 9, 1);
			dbContext.ChangeTracker.Clear();
			var attendances = new List<AttendanceDetails>
			{
				new AttendanceDetails
				{
					ID = Guid.NewGuid(),
					EmployeeId = _employeeId,
					InTime = attendanceDate.AddHours(9),
					OutTime = attendanceDate.AddHours(18),
					AttendanceDate = DateTime.Today,
					AttendanceStatus="1",
					IsHalfDay=true,
					LeaveTypeId=_employeeId

				}
			};
			dbContext.HR_Attendance.Add(new Attendance
			{
				ID = Guid.NewGuid(),
				EmployeeId = _employeeId,
				InTime = attendanceDate.AddHours(9),
				OutTime = attendanceDate.AddHours(18),
				AttendanceDate = DateTime.Today,
				AttendanceStatus = 2,
				IsHalfDay = true,
				LeaveTypeID = _employeeId
			});
			dbContext.SaveChanges();
			dbContext.ChangeTracker.Clear();

			var addResult = await _attendanceService.AttendanceUpdate(attendances, Guid.NewGuid());
			var attendance = dbContext.HR_Attendance.FirstOrDefault(x => x.AttendanceDate == DateTime.Today
								&& x.EmployeeId == _employeeId);

			Assert.True(addResult.HasNoError);
			Assert.True(addResult.Messages.Count == 0);
			Assert.True(addResult.ReturnValue.AttendanceStatus == (int)AttendanceStatus.Present);
			Assert.True(attendance.AttendanceStatus == (int)AttendanceStatus.Present);
		}
		[Fact]
		public async Task AttendanceUpdate_AttendanceStatus_Absent()
		{
			dbContext.ChangeTracker.Clear();
			var attendances = new List<AttendanceDetails>
			{
				new AttendanceDetails
				{
					ID = Guid.NewGuid(),
					EmployeeId = _employeeId,
					AttendanceDate = DateTime.Today,
					AttendanceStatus="2",
					IsHalfDay=true,
					LeaveTypeId=_employeeId
				}
			};
			dbContext.HR_Attendance.Add(new Attendance
			{
				ID = Guid.NewGuid(),
				EmployeeId = _employeeId,
				AttendanceDate = DateTime.Today,
				AttendanceStatus = 4,
				IsHalfDay = true,
				LeaveTypeID = _employeeId,
			});
			dbContext.SaveChanges();
			dbContext.ChangeTracker.Clear();

			var addResult = await _attendanceService.AttendanceUpdate(attendances, Guid.NewGuid());
			var attendance = dbContext.HR_Attendance.FirstOrDefault(x => x.AttendanceDate == DateTime.Today
								&& x.EmployeeId == _employeeId);

			Assert.True(addResult.HasNoError);
			Assert.True(addResult.Messages.Count == 0);
			Assert.True(addResult.ReturnValue.AttendanceStatus == (int)AttendanceStatus.Absent);
			Assert.True(attendance.AttendanceStatus == (int)AttendanceStatus.Absent);
		}
		[Fact]
		public async Task AttendanceUpdate_AttendanceStatus_Leave()
		{
			var attendanceDate = new DateTime(2022, 9, 1);
			dbContext.ChangeTracker.Clear();
			var attendances = new List<AttendanceDetails>
			{
				new AttendanceDetails
				{
					ID = Guid.NewGuid(),
					EmployeeId = _employeeId,
					AttendanceDate = DateTime.Today,
					AttendanceStatus="3",
					IsHalfDay=true,
					LeaveTypeId=leaveTypeId

				}
			};
			dbContext.HR_Attendance.Add(new Attendance
			{
				ID = Guid.NewGuid(),
				EmployeeId = _employeeId,
				AttendanceDate = DateTime.Today,
				AttendanceStatus = 1,
				IsHalfDay = true,
			});
			dbContext.SaveChanges();
			dbContext.ChangeTracker.Clear();


			var addResult = await _attendanceService.AttendanceUpdate(attendances, Guid.NewGuid());
			var attendance = dbContext.HR_Attendance.FirstOrDefault(x => x.AttendanceDate == DateTime.Today
								&& x.EmployeeId == _employeeId);

			Assert.True(addResult.HasNoError);
			Assert.True(addResult.Messages.Count == 0);
			Assert.True(addResult.ReturnValue.AttendanceStatus == (int)AttendanceStatus.Leave);
			Assert.True(attendance.AttendanceStatus == (int)AttendanceStatus.Leave);
		}
		[Fact]
		public async Task AttendanceUpdate_AttendanceStatus_WFH()
		{
			var attendanceDate = new DateTime(2022, 9, 1);
			dbContext.ChangeTracker.Clear();
			var attendances = new List<AttendanceDetails>
			{
				new AttendanceDetails
				{
					ID = Guid.NewGuid(),
					EmployeeId = _employeeId,
					AttendanceDate = DateTime.Today,
					AttendanceStatus="5",
					IsHalfDay=true,
					LeaveTypeId=_employeeId

				}
			};
			dbContext.HR_Attendance.Add(new Attendance
			{
				ID = Guid.NewGuid(),
				EmployeeId = _employeeId,
				AttendanceDate = DateTime.Today,
				AttendanceStatus = 6,
				IsHalfDay = true,
				LeaveTypeID = _employeeId
			});
			dbContext.SaveChanges();
			dbContext.ChangeTracker.Clear();

			var addResult = await _attendanceService.AttendanceUpdate(attendances, Guid.NewGuid());
			var attendance = dbContext.HR_Attendance.FirstOrDefault(x => x.AttendanceDate == DateTime.Today
								&& x.EmployeeId == _employeeId);

			Assert.True(addResult.HasNoError);
			Assert.True(addResult.Messages.Count == 0);
			Assert.True(addResult.ReturnValue.AttendanceStatus == (int)AttendanceStatus.WFH);
			Assert.True(attendance.AttendanceStatus == (int)AttendanceStatus.WFH);
		}
		[Fact]
		public async Task AttendanceUpdate_AttendanceStatus_WeekOff()
		{
			var attendanceDate = new DateTime(2022, 9, 1);
			dbContext.ChangeTracker.Clear();
			var attendances = new List<AttendanceDetails>
			{
				new AttendanceDetails
				{
					ID = Guid.NewGuid(),
					EmployeeId = _employeeId,
					AttendanceDate = DateTime.Today,
					AttendanceStatus="4",
					IsHalfDay=true,
					LeaveTypeId=_employeeId

				}
			};
			dbContext.HR_Attendance.Add(new Attendance
			{
				ID = Guid.NewGuid(),
				EmployeeId = _employeeId,
				AttendanceDate = DateTime.Today,
				AttendanceStatus = 2,
				IsHalfDay = true,
				LeaveTypeID = _employeeId
			});
			dbContext.SaveChanges();
			dbContext.ChangeTracker.Clear();

			var addResult = await _attendanceService.AttendanceUpdate(attendances, Guid.NewGuid());
			var attendance = dbContext.HR_Attendance.FirstOrDefault(x => x.AttendanceDate == DateTime.Today
								&& x.EmployeeId == _employeeId);

			Assert.True(addResult.HasNoError);
			Assert.True(addResult.Messages.Count == 0);
			Assert.True(addResult.ReturnValue.AttendanceStatus == (int)AttendanceStatus.WeekOff);
			Assert.True(attendance.AttendanceStatus == (int)AttendanceStatus.WeekOff);
		}
		[Fact]
		public async Task AttendanceUpdate_AttendanceStatus_Holiday()
		{
			var attendanceDate = new DateTime(2022, 9, 1);
			dbContext.ChangeTracker.Clear();
			var attendances = new List<AttendanceDetails>
			{
				new AttendanceDetails
				{
					ID = Guid.NewGuid(),
					EmployeeId = _employeeId,
					AttendanceDate = DateTime.Today,
					AttendanceStatus="6",
					IsHalfDay=true,
					LeaveTypeId=_employeeId

				}
			};
			dbContext.HR_Attendance.Add(new Attendance
			{
				ID = Guid.NewGuid(),
				EmployeeId = _employeeId,
				AttendanceDate = DateTime.Today,
				AttendanceStatus = 1,
				IsHalfDay = true,
				LeaveTypeID = _employeeId
			});
			dbContext.SaveChanges();
			dbContext.ChangeTracker.Clear();

			var addResult = await _attendanceService.AttendanceUpdate(attendances, Guid.NewGuid());
			var attendance = dbContext.HR_Attendance.FirstOrDefault(x => x.AttendanceDate == DateTime.Today
								&& x.EmployeeId == _employeeId);

			Assert.True(addResult.HasNoError);
			Assert.True(addResult.Messages.Count == 0);
			Assert.True(addResult.ReturnValue.AttendanceStatus == (int)AttendanceStatus.Holiday);
			Assert.True(attendance.AttendanceStatus == (int)AttendanceStatus.Holiday);
		}
		[Fact]
		public async Task AttendanceUpdate_AttendanceStatus_Unauthorized()
		{
			var attendanceDate = new DateTime(2022, 9, 1);
			dbContext.ChangeTracker.Clear();
			var attendances = new List<AttendanceDetails>
			{
				new AttendanceDetails
				{
					ID = Guid.NewGuid(),
					EmployeeId = _employeeId,
					AttendanceDate = DateTime.Today,
					AttendanceStatus="20",
					IsHalfDay=true,
					LeaveTypeId=_employeeId,
					Unauthorized = 20,
					IsFirstOff=true,
					HalfDayType=1,
				}
			};
			dbContext.HR_Attendance.Add(new Attendance
			{
				ID = Guid.NewGuid(),
				EmployeeId = _employeeId,
				AttendanceDate = DateTime.Today,
				AttendanceStatus = 20,
				IsHalfDay = true,
				LeaveTypeID = _employeeId
			});
			dbContext.SaveChanges();
			dbContext.ChangeTracker.Clear();

			var addResult = await _attendanceService.AttendanceUpdate(attendances, Guid.NewGuid());
			var attendance = dbContext.HR_Attendance.FirstOrDefault(x => x.AttendanceDate == DateTime.Today
								&& x.EmployeeId == _employeeId);

			Assert.True(addResult.HasNoError);
			Assert.True(addResult.Messages.Count == 0);
			Assert.True(addResult.ReturnValue.AttendanceStatus == (int)AttendanceStatus.Unautherized);
			Assert.True(attendance.AttendanceStatus == (int)AttendanceStatus.Unautherized);
		}
		[Fact]
		public async Task AttendanceUpdate_AttendanceStatus_InvalidStatus()
		{
			var attendanceDate = new DateTime(2022, 9, 1);
			dbContext.ChangeTracker.Clear();
			var attendances = new List<AttendanceDetails>
			{
				new AttendanceDetails
				{
					ID = Guid.NewGuid(),
					EmployeeId = _employeeId,
					AttendanceDate = DateTime.Today,
					AttendanceStatus="0",
					IsHalfDay=true,
					LeaveTypeId=_employeeId

				}
			};
			dbContext.HR_Attendance.Add(new Attendance
			{
				ID = Guid.NewGuid(),
				EmployeeId = _employeeId,
				AttendanceDate = DateTime.Today,
				AttendanceStatus = 0,
				IsHalfDay = true,
				LeaveTypeID = _employeeId
			});
			dbContext.SaveChanges();
			dbContext.ChangeTracker.Clear();

			var addResult = await _attendanceService.AttendanceUpdate(attendances, Guid.NewGuid());
			var attendance = dbContext.HR_Attendance.FirstOrDefault(x => x.AttendanceDate == DateTime.Today
								&& x.EmployeeId == _employeeId);

			Assert.True(addResult.HasError);
			Assert.True(addResult.Messages.Count == 1);
		}
		[Fact]
		public async Task RePunchIn_UpdateAttendance_Test_WithoutException()
		{
			//Arrange
			dbContext.ChangeTracker.Clear();
			dbContext.HR_Attendance.Add(new Attendance
			{
				ID = Guid.NewGuid(),
				EmployeeId = _employeeId,
				AttendanceDate = DateTime.Today,
				AttendanceStatus = 0,
				IsHalfDay = true,
				LeaveTypeID = _employeeId
			});
			await _uow.Context.SaveChangesAsync();
			dbContext.ChangeTracker.Clear();

			var result = await _attendanceService.RePunchIn(_employeeId);
			Assert.Equal(result.ReturnValue.WorkTime, 0);
			Assert.Equal(result.ReturnValue.InTime.Value.Date, DateTime.Now.Date);
		}
		[Fact]
		public async Task RePunchIn_UpdateAttendance_Test_WithException()
		{
			//Arrange
			dbContext.ChangeTracker.Clear();
			dbContext.HR_Attendance.Add(new Attendance
			{
				ID = Guid.NewGuid(),
				EmployeeId = _employeeId,
				AttendanceDate = DateTime.Now.AddDays(-1),
				AttendanceStatus = 0,
				IsHalfDay = true,
				LeaveTypeID = _employeeId
			});
			dbContext.SaveChanges();
			dbContext.ChangeTracker.Clear();

			var result = await _attendanceService.RePunchIn(_employeeId);
			Assert.False(result.IsSuccess);
		}
		[Fact]
		public async Task Finalized_AddingNewRecord_WithoutException()
		{
			var employeeId = Guid.NewGuid();
			dbContext.ChangeTracker.Clear();
			dbContext.HR_Attendance.AddRange(
				new Attendance
				{
					AttendanceDate = DateTime.Parse("2022-02-27"),
					EmployeeId = employeeId,
					Present = 2,
					Absent = 1.5m,
					UADays = 0
				},
				new Attendance
				{
					AttendanceDate = DateTime.Parse("2022-02-28"),
					EmployeeId = employeeId,
					Present = 4,
					Absent = 0,
					UADays = 1
				},
				new Attendance
				{
					AttendanceDate = DateTime.Parse("2022-04-04"),
					EmployeeId = employeeId,
					Present = 4,
					Absent = 2,
					UADays = 0.5m
				});
			dbContext.Payroll_PayMonth.Add(new TranSmart.Domain.Entities.Payroll.PayMonth
			{
				Month = DateTime.Today.Month,
				Year = DateTime.Today.Year,
				Start = DateTime.Parse("2022-02-26"),
				End = DateTime.Parse("2022-03-25"),
				Status = (int)PayMonthStatus.InProcess,
			});
			dbContext.SaveChanges();
			//var _attendanceRepo = new AttendanceRepository(dbContext);
			var result = await _attendanceService.Finalized((byte)DateTime.Today.Month, (short)DateTime.Today.Year);
			Assert.True(result.HasNoError);
		}
		[Fact]
		public async Task Finalized_Updating_ExistingRecord_WithoutException()
		{
			var employeeId = Guid.NewGuid();
			dbContext.ChangeTracker.Clear();
			dbContext.HR_Attendance.AddRange(
				new Attendance
				{
					AttendanceDate = DateTime.Parse("2022-02-27"),
					EmployeeId = employeeId,
					Present = 2.5m,
					Absent = 1.5m,
					UADays = 0
				},
				new Attendance
				{
					AttendanceDate = DateTime.Parse("2022-02-28"),
					EmployeeId = employeeId,
					Present = 4,
					Absent = 0,
					UADays = 1
				},
				new Attendance
				{
					AttendanceDate = DateTime.Parse("2022-04-04"),
					EmployeeId = employeeId,
					Present = 4,
					Absent = 2,
					UADays = 0.5m
				});
			dbContext.HR_AttendanceSum.AddRange(
				new AttendanceSum
				{
					EmployeeId = employeeId,
					Month = (byte)DateTime.Today.Month,
					Year = (short)DateTime.Today.Year,
				});

			dbContext.Payroll_PayMonth.Add(new TranSmart.Domain.Entities.Payroll.PayMonth
			{
				Month = DateTime.Today.Month,
				Year = DateTime.Today.Year,
				Start = DateTime.Parse("2022-02-26"),
				End = DateTime.Parse("2022-03-25"),
				Status = (int)PayMonthStatus.InProcess,
			});
			dbContext.SaveChanges();
			dbContext.ChangeTracker.Clear();
			//var _attendanceRepo = new AttendanceRepository(dbContext);
			var result = await _attendanceService.Finalized((byte)DateTime.Today.Month, (short)DateTime.Today.Year);
			Assert.True(result.HasNoError);
			Assert.True(dbContext.HR_AttendanceSum.FirstOrDefault().Present == 6.5m);
		}
		[Fact]
		public async Task Finalized_With_Exception()
		{
			dbContext.Payroll_PayMonth.Add(new TranSmart.Domain.Entities.Payroll.PayMonth
			{
				Month = DateTime.Today.Month,
				Year = DateTime.Today.Year,
				Start = DateTime.Parse("2022-02-26"),
				End = DateTime.Parse("2022-03-25"),
				Status = (int)PayMonthStatus.Released,
			});
			var result = await _attendanceService.Finalized((byte)DateTime.Today.Month, (short)DateTime.Today.Year);
			Assert.True(result.HasError);
		}
		[Fact]
		public async Task UpdateTimings_WithoutException_Test()
		{
			var employeeId = Guid.NewGuid();
			dbContext.ChangeTracker.Clear();
			dbContext.HR_Attendance.AddRange(
				new Attendance
				{
					AttendanceDate = DateTime.Today,
					EmployeeId = employeeId,
					Present = 2,
					Absent = 2.5m,
					UADays = 0,
					InTime = DateTime.Now.AddHours(-1),
					WorkTime = (int)0.5
				}
				);
			dbContext.SaveChanges();
			dbContext.ChangeTracker.Clear();
			var result = await _attendanceService.UpdateTimings(employeeId);
			var attendance = dbContext.HR_Attendance.FirstOrDefault();
			Assert.True(attendance.WorkTime == 1);
			Assert.True(result.IsSuccess);
		}
		[Fact]
		public async Task UpdateTimings_WithException_Test()
		{
			dbContext.HR_Attendance.AddRange(
				new Attendance
				{
					AttendanceDate = DateTime.Today,
					EmployeeId = Guid.NewGuid(),
					Present = 2,
					Absent = 2.5m,
					UADays = 0,
					InTime = DateTime.Now.AddHours(-1),
					WorkTime = (int)0.5
				});
			_uow.Context.ChangeTracker.Clear();
			await dbContext.SaveChangesAsync();
			var result = await _attendanceService.UpdateTimings(_employeeId);
			Assert.False(result.IsSuccess);
		}
		[Fact]
		public async Task GetAttendanceData_List()
		{
			var attendanceSearch = new AttendanceSearch
			{
				FromDate = DateTime.Now.AddDays(-10),
				ToDate = DateTime.Today,
				EmpId = _employeeId,
				SortBy = "Name",
				Size = 10,
				Page = 0,
			};
			var listData = await _attendanceService.GetAttendanceData(attendanceSearch);
			Assert.True(listData.Count == 1);
		}
		[Fact]
		public async Task IsPunchEmployee_Return_True()
		{
			var result = await _attendanceService.IsPunchEmployee(_employeeId);
			Assert.True(result);
		}
		[Fact]
		public async Task IsPunchEmployee_Return_False()
		{
			var result = await _attendanceService.IsPunchEmployee(Guid.NewGuid());
			Assert.False(result);
		}
		[Theory]
		[InlineData("")]
		[InlineData("1524b506-a8c0-4bda-a085-ea2811d82b5e")]
		public async Task AttendanceFromImport_Test(string id)
		{
			var attendanceDate = DateTime.Today;
			dbContext.ChangeTracker.Clear();
			var attendances = new List<Attendance> { new Attendance
			{
				ID = id==""?Guid.Empty:Guid.Parse(id),
				EmployeeId = _employeeId,
				AttendanceDate = DateTime.Today,
			}
			};
			dbContext.Leave_ApplyLeave.AddRange(new ApplyLeave
			{
				EmployeeId = Guid.NewGuid(),
				FromDate = attendanceDate,
				ToDate = attendanceDate,
				Status = (int)ApplyLeaveSts.Applied,
			});
			dbContext.HR_Attendance.AddRange(new Attendance
			{
				EmployeeId = Guid.NewGuid(),
				AttendanceDate = attendanceDate,
			});
			dbContext.SaveChanges();
			dbContext.ChangeTracker.Clear();

			var result = await _attendanceService.AttendanceFromImport(attendances);
			var att = dbContext.HR_Attendance.Count();
			Assert.True(id == "" ? att == 2 : att == 1);
		}
	}
}
