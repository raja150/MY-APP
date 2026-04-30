using Microsoft.EntityFrameworkCore;
using Moq;
using TranSmart.Data;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;

namespace Transmart.Services.UnitTests.Services.Organization.Setup
{
	public static class SetData
	{
		public static void MockAllocation(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Allocation>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Allocation>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Allocation>()).Returns(repository);
		}

		public static void MockDepartment(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Department>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Department>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Department>()).Returns(repository);
		}

		public static void MockDesignation(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Designation>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Designation>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Designation>()).Returns(repository);
		}

		public static void MockEmployeeEmergencyAd(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<EmployeeEmergencyAd>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<EmployeeEmergencyAd>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EmployeeEmergencyAd>()).Returns(repository);
		}



		public static void MockEmployeeFamily(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<EmployeeFamily>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<EmployeeFamily>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EmployeeFamily>()).Returns(repository);
		}


		public static void MockEmployee(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Employee>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Employee>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Employee>()).Returns(repository);
		}


		public static void MockEmployeeResignation(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<EmployeeResignation>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<EmployeeResignation>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EmployeeResignation>()).Returns(repository);
		}


		public static void MockEmployeePresentAd(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<EmployeePresentAd>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<EmployeePresentAd>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EmployeePresentAd>()).Returns(repository);
		}


		public static void MockEmployeePermanentAd(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<EmployeePermanentAd>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<EmployeePermanentAd>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EmployeePermanentAd>()).Returns(repository);
		}

		public static void MockApplyLeave(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<ApplyLeave>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<ApplyLeave>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<ApplyLeave>()).Returns(repository);
		}

		public static void MockLeaveBalance(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<LeaveBalance>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<LeaveBalance>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<LeaveBalance>()).Returns(repository);
		}


		public static void MockLocation(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Location>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Location>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Location>()).Returns(repository);
		}


		public static void MockOrganizations(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Organizations>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Organizations>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Organizations>()).Returns(repository);
		}


		public static void MockTeam(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Team>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Team>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Team>()).Returns(repository);
		}
		public static void MockReplication(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Replication>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Replication>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Replication>()).Returns(repository);
		}
		public static void MockEmployeeDevice(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<EmployeeDevice>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<EmployeeDevice>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EmployeeDevice>()).Returns(repository);
		}
	}
}
