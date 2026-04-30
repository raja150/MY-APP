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

namespace Transmart.Services.UnitTests.Services.Leave.ApplyLeaveData
{
    public static class SetData
    {
        public static void MockLeaveType(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSetLeavetype)
        {
            context.Setup(x => x.Set<LeaveType>()).Returns(mockSetLeavetype.Object);
            var repository = new RepositoryAsync<LeaveType>(context.Object);
            uow.Setup(m => m.GetRepositoryAsync<LeaveType>()).Returns(repository);
        }

        public static void MockApplyLeave(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSetApplyLeave)
        {
            context.Setup(x => x.Set<ApplyLeave>()).Returns(mockSetApplyLeave.Object);
            var repository = new RepositoryAsync<ApplyLeave>(context.Object);
            uow.Setup(m => m.GetRepositoryAsync<ApplyLeave>()).Returns(repository);
        }

		public static void MockApplyLeaveType(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSetApplyLeaveType)
		{
			context.Setup(x => x.Set<ApplyLeaveType>()).Returns(mockSetApplyLeaveType.Object);
			var repository = new RepositoryAsync<ApplyLeaveType>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ApplyLeaveType>()).Returns(repository);
		}

		public static void MockClientVisit(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSetApplyClientVisit)
		{
			context.Setup(x => x.Set<ApplyClientVisits>()).Returns(mockSetApplyClientVisit.Object);
			var repository = new RepositoryAsync<ApplyClientVisits>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ApplyClientVisits>()).Returns(repository);
		}

		public static void MockLeaveBalance(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSetLeaveBalance)
        {
            context.Setup(x => x.Set<LeaveBalance>()).Returns(mockSetLeaveBalance.Object);
            var repository = new RepositoryAsync<LeaveBalance>(context.Object);
            uow.Setup(m => m.GetRepositoryAsync<LeaveBalance>()).Returns(repository);
        }

        public static void MockEmployee(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSetEmployee)
        {
            context.Setup(x => x.Set<Employee>()).Returns(mockSetEmployee.Object);
            var repository = new RepositoryAsync<Employee>(context.Object);
            uow.Setup(m => m.GetRepositoryAsync<Employee>()).Returns(repository);
        }

        public static void MockApplyLeaveDetails(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSetApplyLeaveDetails)
        {
            context.Setup(x => x.Set<ApplyLeaveDetails>()).Returns(mockSetApplyLeaveDetails.Object);
            var repository = new RepositoryAsync<ApplyLeaveDetails>(context.Object);
            uow.Setup(m => m.GetRepositoryAsync<ApplyLeaveDetails>()).Returns(repository);
        }
		public static void MockAttendanceSum(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSetAttendanceSum)
		{
			context.Setup(x => x.Set<AttendanceSum>()).Returns(mockSetAttendanceSum.Object);
			var repository = new RepositoryAsync<AttendanceSum>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<AttendanceSum>()).Returns(repository);
		}
		public static void MockAttendance(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSetAttendance)
        {
            context.Setup(x => x.Set<Attendance>()).Returns(mockSetAttendance.Object);
            var repository = new RepositoryAsync<Attendance>(context.Object);
            uow.Setup(m => m.GetRepositoryAsync<Attendance>()).Returns(repository);
        }
		public static void MockSetAttendance(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSetAttendance)
		{
			context.Setup(x => x.Set<Attendance>()).Returns(mockSetAttendance.Object);
			var repository = new Repository<Attendance>(context.Object);
			uow.Setup(m => m.GetRepository<Attendance>()).Returns(repository);
		}
		public static void MockManualAttLogs(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSetAttendance)
		{
			context.Setup(x => x.Set<ManualAttLogs>()).Returns(mockSetAttendance.Object);
			var repository = new RepositoryAsync<ManualAttLogs>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ManualAttLogs>()).Returns(repository);
		}

		public static void MockOrganizations(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSetAttendance)
		{
			context.Setup(x => x.Set<Organizations>()).Returns(mockSetAttendance.Object);
			var repository = new RepositoryAsync<Organizations>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Organizations>()).Returns(repository);
		}
		public static void MockAttendanceModifyLogs(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<AttendanceModifyLogs>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<AttendanceModifyLogs>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<AttendanceModifyLogs>()).Returns(repository);
		}

		public static void MockAllocation(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Allocation>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Allocation>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Allocation>()).Returns(repository);
		}

		public static void MockShift(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Shift>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Shift>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Shift>()).Returns(repository);
		}

		public static void MockBiometricAttLogs(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<BiometricAttLogs>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<BiometricAttLogs>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<BiometricAttLogs>()).Returns(repository);
		}

		public static void MockHolidays(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Holidays>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Holidays>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Holidays>()).Returns(repository);
		}

		public static void MockExemptions(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Exemptions>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Exemptions>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Exemptions>()).Returns(repository);
		}

		public static void MockWeekOffDays(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<WeekOffDays>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<WeekOffDays>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<WeekOffDays>()).Returns(repository);
		}

		public static void MockApplyWFH(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<ApplyWfh>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<ApplyWfh>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ApplyWfh>()).Returns(repository);
		}
	}
}
