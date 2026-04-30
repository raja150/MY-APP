using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Data.Repository.Attendance;
using TranSmart.Services.UnitTests;
using Xunit;

namespace Transmart.Services.UnitTests.Data.Repository
{
	public class AttendanceRepositoryTest : IClassFixture<InMemoryFixture>
	{
		private readonly InMemoryFixture inMemory;
		private readonly AttendanceRepository _repository;
		public AttendanceRepositoryTest(InMemoryFixture fixture)
		{
			var employeeId = Guid.NewGuid();
			var leaveTypeId = Guid.NewGuid();
			inMemory = fixture;
			_repository = new AttendanceRepository(inMemory.DbContext);
			inMemory.DbContext.HR_Attendance.AddRange(
				new TranSmart.Domain.Entities.Leave.Attendance
				{
					ID = Guid.NewGuid(),
					AttendanceStatus = (int)AttendanceStatus.Late,
					Present = 28,
					UADays = 1,
					Absent = 1,
					AttendanceDate = DateTime.Now.Date,
					EmployeeId = employeeId,
					LeaveTypeID = leaveTypeId,
					IsFirstHalf = true
				},
				new TranSmart.Domain.Entities.Leave.Attendance
				{
					ID = Guid.NewGuid(),
					AttendanceStatus = (int)AttendanceStatus.Late,
					Present = 30,
					UADays = 1,
					Absent = 0,
					AttendanceDate = DateTime.Now.Date,
					EmployeeId = Guid.NewGuid(),
					LeaveTypeID = leaveTypeId,
					IsFirstHalf = true
				},
				new TranSmart.Domain.Entities.Leave.Attendance
				{
					ID = Guid.NewGuid(),
					AttendanceStatus = (int)AttendanceStatus.Late,
					Present = 26,
					UADays = 2,
					Absent = 2,
					AttendanceDate = DateTime.Now.Date,
					EmployeeId = employeeId,
					LeaveTypeID = leaveTypeId,
					IsFirstHalf = true
				});
			inMemory.DbContext.SaveChanges();
		}
		[Fact]
		public void FinalizedAttendance_Test()
		{
			var dd = _repository.Summary(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(2),1,2023);
			Assert.True(dd.Count() == 2);
			Assert.True(dd.FirstOrDefault().Present == 54);
			Assert.True(dd.FirstOrDefault().Unauthorized == 3);
			Assert.True(dd.FirstOrDefault().LOP == 3);
		}
	}
}
