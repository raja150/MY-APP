using DocumentFormat.OpenXml.Bibliography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Data.Repository.Attendance;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Enums;
using TranSmart.Service;
using TranSmart.Service.Leave;
using TranSmart.Service.LM_Attendance;
using TranSmart.Service.Schedules;
using TranSmart.Services.UnitTests;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Leave
{
	[CollectionDefinition("Collection #1")]
	[Xunit.TestCaseOrderer("Transmart.Services.UnitTests.PriorityOrderer", "Transmart.Services.UnitTests")]
	public class ApplyLeaveIntegrationTest : IClassFixture<InMemoryFixture>
	{
		private readonly UnitOfWork<TranSmartContext> _uow;
		private readonly TranSmartContext dbContext;
		private readonly SequenceNoService seqNo;
		private readonly IApplyLeaveService _applyLeaveService;
		private readonly AttendanceService _attendanceService;
		private readonly ScheduleService _scheduleService;
		private readonly LeaveBalanceService _leaveBalanceService;
		private readonly ShiftService _shiftService;
		private readonly AttendanceRepository _attendanceRepository;
		private readonly Guid applyLeaveId = Guid.NewGuid();
		private readonly Guid reportingToId = Guid.NewGuid();
		private readonly Guid _employeeId = Guid.Parse("d1e53b8b-7316-4951-a1bd-985fb14bdf6c");
		private readonly Guid clLeaveTypeId = Guid.NewGuid();
		private readonly Guid elLeaveTypeId = Guid.NewGuid();
		private readonly Guid newApplyLeaveId = Guid.NewGuid();
		public ApplyLeaveIntegrationTest()
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

			dbContext.Organization_Employee.AddRange(
				new Employee
				{
					ID = _employeeId,
					No = "AVONTIX1827",
					Name = "Anudeep",
					Gender = 1,
					Status = 1,
					MaritalStatus = 2,
					MobileNumber = "9639639632",
					DateOfBirth = new DateTime(1989, 02, 02),
					DateOfJoining = new DateTime(2020, 08, 12),
					AadhaarNumber = "561250752388",
					PanNumber = "BLMPJ2797L",
					WorkLocationId = Guid.NewGuid(),
					WorkLocation = new Location { ID = Guid.NewGuid(), Name = "Jubilee Hills" },
					TeamId = Guid.NewGuid(),
					Team = new Team
					{
						ID = Guid.NewGuid(),
						Name = "Regular",
					},
					ReportingToId = reportingToId
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
					WorkLocationId = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbe"),
					WorkLocation = new Location { ID = Guid.Parse("8cc4fd65-b7de-4acf-8c4b-53c76a620cbe"), Name = "Jubilee Hills" },
					TeamId = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b5e"),
					Team = new Team
					{
						ID = Guid.Parse("1524b506-a8c0-4bda-a085-ea2811d82b5e"),
						Name = "Regular",
					},
					WorkFromHome = 1
				});
			dbContext.SaveChangesAsync();


			dbContext.Leave_ApplyLeaveDetails.AddRange(
				new ApplyLeaveDetails
				{
					ID = Guid.Parse("2b604b40-96e3-4718-b9de-7731147c2c4a"),
					ApplyLeaveId = Guid.Parse("0aaac7a7-a7cf-4e2e-9b36-3b9d7c2f3851"),
					LeaveTypeId = clLeaveTypeId,
					IsFirstHalf = true,
				}
				);
			dbContext.SaveChangesAsync();

			dbContext.HR_Attendance.AddRange(
				new Attendance
				{
					ID = Guid.NewGuid(),
					EmployeeId = _employeeId,
					AttendanceDate = DateTime.Now.AddDays(-1),
					AttendanceStatus = 1
				},
				new Attendance
				{
					ID = Guid.NewGuid(),
					EmployeeId = _employeeId,
					AttendanceDate = DateTime.Now.AddDays(-2),
					AttendanceStatus = 1
				},
				new Attendance
				{
					ID = Guid.NewGuid(),
					EmployeeId = _employeeId,
					AttendanceDate = DateTime.Now.AddDays(-3),
					AttendanceStatus = 1
				},
				new Attendance
				{
					ID = Guid.NewGuid(),
					EmployeeId = _employeeId,
					AttendanceDate = DateTime.Now.AddDays(-4),
					AttendanceStatus = 1
				});
			dbContext.SaveChangesAsync();
			_uow.Context.ChangeTracker.Clear();
		}
		[Fact]
		public async Task AddApprovedLeaveAsync_Without_Exception()
		{
			var effectiveFrom = DateTime.Now.AddMonths(-1).AddDays(1);
			var newApplyLeave = new ApplyLeave
			{
				ID = newApplyLeaveId,
				EmployeeId = _employeeId,
				FromDate = DateTime.Now.AddDays(-4),
				ToDate = DateTime.Now,
				Reason = "Corona",
				EmergencyContNo = "9381742192",
				Status = (int)ApplyLeaveSts.Applied,
				LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":5}]",
				ApplyLeaveType = new List<ApplyLeaveType> {
				new ApplyLeaveType
				{
					ApplyLeaveId = newApplyLeaveId,
					ApplyLeave= new ApplyLeave{ID = newApplyLeaveId,EmployeeId = _employeeId,Status = (int)ApplyLeaveSts.Applied},
					LeaveTypeId = Guid.Parse("6ff2209e-8187-4cfd-84f9-621a0ae9e52f"),
					NoOfLeaves = 5,
				},
				}
			};
			dbContext.Leave_LeaveType.AddRange(
				new LeaveType
				{
					ID = Guid.Parse("6ff2209e-8187-4cfd-84f9-621a0ae9e52f"),
					Code = "CL",
					Name = "Casual leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 8,
					FutureDate = 8,
					MaxApplications = 2,
					MinLeaves = 1,
					MaxLeaves = 12,
					specifiedperio = 1,
				},
				new LeaveType
				{
					ID = elLeaveTypeId,
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
					PastDate = 8,
					FutureDate = 8,
					MaxApplications = 2,
					MinLeaves = 1,
					MaxLeaves = 12,
					specifiedperio = 1,
				});

			dbContext.Leave_LeaveBalance.AddRange(
				new LeaveBalance
				{
					ID = Guid.NewGuid(),
					EmployeeId = _employeeId,
					LeaveTypeId = Guid.Parse("6ff2209e-8187-4cfd-84f9-621a0ae9e52f"),
					Leaves = 8,
					EffectiveFrom = effectiveFrom,
					EffectiveTo = effectiveFrom.AddMonths(2).AddDays(-1)
				});
			await dbContext.SaveChangesAsync();
			_uow.Context.ChangeTracker.Clear();
			var dd = await _applyLeaveService.AddApprovedLeaveAsync(newApplyLeave);
			var leaveBalanceCount = dbContext.Leave_LeaveBalance.Count();
			Assert.True(leaveBalanceCount == 2);
			Assert.Equal(-5, dbContext.Leave_LeaveBalance.LastOrDefault().Leaves);
			Assert.True(dd.IsSuccess);
		}
		[Fact]
		public async Task Approve_By_ApprovalManager()
		{
			var effectiveFrom = DateTime.Now.AddMonths(-1).AddDays(1);
			_uow.Context.ChangeTracker.Clear();
			dbContext.Leave_ApplyLeave.Add(new ApplyLeave
			{
				ID = applyLeaveId,
				EmployeeId = _employeeId,
				FromDate = DateTime.Now.AddDays(-4),
				ToDate = DateTime.Now,
				Reason = "Corona",
				EmergencyContNo = "9381742222",
				Status = (int)ApplyLeaveSts.Applied,
				ApplyLeaveType = new List<ApplyLeaveType> {
				new ApplyLeaveType
				{
					ApplyLeaveId = applyLeaveId,
					ApplyLeave= new ApplyLeave{ID = newApplyLeaveId,EmployeeId = _employeeId,Status = (int)ApplyLeaveSts.Applied},
					LeaveType = new LeaveType{ID = Guid.Parse("6ff2209e-8187-4cfd-84f9-621a0ae9e52f"),Name = "casual leave",Code="CL"},
					NoOfLeaves = 1,
				},
				}
			});
			dbContext.Leave_LeaveBalance.AddRange(new LeaveBalance
			{
				EmployeeId = _employeeId,
				LeaveTypeId = Guid.Parse("6ff2209e-8187-4cfd-84f9-621a0ae9e52f"),
				Leaves = 5,
				LeavesAddedOn = DateTime.Now.Date,
				EffectiveFrom = effectiveFrom,
				EffectiveTo = effectiveFrom.AddMonths(2).AddDays(-1)
			});
			await dbContext.SaveChangesAsync();
			_uow.Context.ChangeTracker.Clear();
			var dd = await _applyLeaveService.Approve(applyLeaveId, reportingToId, true);
			var remainingBalance = dbContext.Leave_LeaveBalance.Sum(x => x.Leaves);
			var approvedLeave = dbContext.Leave_ApplyLeave.FirstOrDefault();
			Assert.True(remainingBalance == 4);
			Assert.True(approvedLeave.Status == (int)ApplyLeaveSts.Approved);
			Assert.True(dd.IsSuccess);
		}
		[Fact]
		public async Task SpecifiedPeriod_Validation_True_Case()
		{
			dbContext.Leave_LeaveType.AddRange(
				new LeaveType
				{
					ID = Guid.Parse("6ff2209e-8187-4cfd-84f9-621a0ae9e52f"),
					Code = "CL",
					Name = "Earned Leave",
					EffectiveAfter = 1,
					EffectiveType = 2,
					EffectiveBy = 1,
					ProrateByT = 1,
					RoundOff = 1,
					RoundOffTo = 1,
					Gender = 4,
					MaritalStatus = 3,
					PastDate = 8,
					FutureDate = 8,
					MaxApplications = 2,
					MinLeaves = 1,
					MaxLeaves = 12,
					specifiedperio = 1,
				},
				new LeaveType
				{
					ID = elLeaveTypeId,
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
					PastDate = 8,
					FutureDate = 8,
					MaxApplications = 2,
					MinLeaves = 1,
					MaxLeaves = 12,
					specifiedperio = 1,
				});
			dbContext.Leave_LeaveBalance.AddRange(new LeaveBalance
			{
				ID = Guid.NewGuid(),
				LeaveTypeId = Guid.Parse("6ff2209e-8187-4cfd-84f9-621a0ae9e52f"),
				EmployeeId = _employeeId,
				Leaves = 5
			});
			await dbContext.SaveChangesAsync();
			var newApplyLeave = new ApplyLeave
			{
				ID = newApplyLeaveId,
				EmployeeId = _employeeId,
				FromDate = DateTime.Now.AddDays(-4),
				ToDate = DateTime.Now,
				Reason = "Corona",
				EmergencyContNo = "9381742192",
				Status = (int)ApplyLeaveSts.Applied,
				LeaveTypes = "[{\"name\":\"Casual Leave\",\"days\":3}]",
				ApplyLeaveType = new List<ApplyLeaveType> {
				new ApplyLeaveType
				{
					ApplyLeaveId = newApplyLeaveId,
					ApplyLeave= new ApplyLeave{ID = newApplyLeaveId,EmployeeId = _employeeId,Status = (int)ApplyLeaveSts.Applied},
					LeaveTypeId = Guid.Parse("6ff2209e-8187-4cfd-84f9-621a0ae9e52f"),
					NoOfLeaves = 3,
				},
				}
			};
			var selfServiceValidations = await _applyLeaveService.SelfServiceLeaveValidation(newApplyLeave);
			Assert.True(selfServiceValidations.HasNoError);
		}
		[Fact]
		public async Task UpdateLeaveBalanceAfterReject()
		{
			var applyLeaveTypeId = Guid.NewGuid();

			dbContext.Leave_ApplyLeaveType.AddRange(new ApplyLeaveType
			{
				ID = applyLeaveTypeId,
				LeaveTypeId = Guid.Parse("6ff2209e-8187-4cfd-84f9-621a0ae9e52f"),
				ApplyLeaveId = applyLeaveId,
				NoOfLeaves = 2.5m,
			});
			dbContext.Leave_LeaveBalance.AddRange(new LeaveBalance
			{
				ID = Guid.NewGuid(),
				ApplyLeaveId = applyLeaveId,
				LeaveTypeId = Guid.Parse("6ff2209e-8187-4cfd-84f9-621a0ae9e52f"),
				EmployeeId = _employeeId,
				Leaves = 5,
				LeavesAddedOn= DateTime.Now.Date,
			});
			await dbContext.SaveChangesAsync();
			_uow.Context.ChangeTracker.Clear();
			var applyLeave = new ApplyLeave
			{
				ID = applyLeaveId,
				EmployeeId = _employeeId,
				FromDate = DateTime.Now.AddDays(-4),
				ToDate = DateTime.Now,
				Reason = "Corona",
				EmergencyContNo = "9381742192",
				Status = (int)ApplyLeaveSts.Applied,
				ApplyLeaveType = new List<ApplyLeaveType> {
				new ApplyLeaveType
				{
					ApplyLeaveId = applyLeaveId,
					ApplyLeave= new ApplyLeave{ID = newApplyLeaveId,EmployeeId = _employeeId,Status = (int)ApplyLeaveSts.Applied},
					LeaveTypeId = Guid.Parse("6ff2209e-8187-4cfd-84f9-621a0ae9e52f"),
					NoOfLeaves = 3,
				}}
			};
			//Act
			await _applyLeaveService.UpdateLeaveBalanceAfterReject(applyLeave);
			//Assert
			var leaveBalanceCount = dbContext.Leave_LeaveBalance.FirstOrDefault();
			Assert.True(leaveBalanceCount.Leaves == 0);

		}
	}
}
