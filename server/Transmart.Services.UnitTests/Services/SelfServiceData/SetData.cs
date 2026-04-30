using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;

namespace Transmart.Services.UnitTests.Services.SelfServiceData
{
	public static class SetData
	{
		public static void MockApplyClientVisits(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<ApplyClientVisits>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<ApplyClientVisits>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ApplyClientVisits>()).Returns(repository);
		}
		public static void MockApplyCompensatoryWorkingDay(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<ApplyCompo>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<ApplyCompo>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ApplyCompo>()).Returns(repository);
		}
		public static void MockLeaveBalance(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<LeaveBalance>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<LeaveBalance>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<LeaveBalance>()).Returns(repository);
		}
		public static void MockLeaveSettings(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<LeaveSettings>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<LeaveSettings>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<LeaveSettings>()).Returns(repository);
		}
		public static void MockApplyWFH(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<ApplyWfh>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<ApplyWfh>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ApplyWfh>()).Returns(repository);
		}
		public static void MockApplyLeave(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<ApplyLeave>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<ApplyLeave>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ApplyLeave>()).Returns(repository);
		}
		public static void MockAttendance(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Attendance>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Attendance>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Attendance>()).Returns(repository);
		}
		public static void MockHolidays(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Holidays>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Holidays>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Holidays>()).Returns(repository);
		}
		public static void MockApprovedLeaves(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<ApprovedLeaves>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<ApprovedLeaves>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ApprovedLeaves>()).Returns(repository);
		}
		public static void MockShift(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Shift>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Shift>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Shift>()).Returns(repository);
		}
		public static void MockAllocation(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Allocation>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Allocation>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Allocation>()).Returns(repository);
		}
		public static void MockWeekOffDays(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<WeekOffDays>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<WeekOffDays>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<WeekOffDays>()).Returns(repository);
		}
		public static void MockAdjustLeave(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<AdjustLeave>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<AdjustLeave>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<AdjustLeave>()).Returns(repository);
		}
		public static void MockLeaveType(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<LeaveType>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<LeaveType>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<LeaveType>()).Returns(repository);
		}

		public static void MockAttendanceModifyLogs(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<AttendanceModifyLogs>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<AttendanceModifyLogs>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<AttendanceModifyLogs>()).Returns(repository);
		}

		public static void MockEmployees(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Employee>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Employee>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Employee>()).Returns(repository);
		}
		public static void MockEmployeeLeaveBalanceQry(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<EmployeeLeaveBalanceQry>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<EmployeeLeaveBalanceQry>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EmployeeLeaveBalanceQry>()).Returns(repository);
		}
	}
}
