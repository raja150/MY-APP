using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.AppSettings;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;

namespace Transmart.Services.UnitTests.Services.Setup
{
	public static class SetData
	{
		public static void MockAplicationAuditLog(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<AplicationAuditLog>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<AplicationAuditLog>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<AplicationAuditLog>()).Returns(repository);
		}
		
		public static void MockEmployee(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Employee>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Employee>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Employee>()).Returns(repository);
		} 

		public static void MockSequenceNoNonAsync(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<SequenceNo>()).Returns(mockSet.Object);
			var repository = new Repository<SequenceNo>(context.Object);
			uow.Setup(m => m.GetRepository<SequenceNo>()).Returns(repository);
		} 

		public static void MockEmpImageNo(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<EmpImage>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<EmpImage>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EmpImage>()).Returns(repository);
		}

		public static void MockEmpImage(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<EmpImage>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<EmpImage>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<EmpImage>()).Returns(repository);
		}


		public static void MockLookUpValuesNo(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<LookUpValues>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<LookUpValues>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<LookUpValues>()).Returns(repository);
		}


		public static void MockRole(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Role>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Role>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Role>()).Returns(repository);
		}

		public static void MockRolePrivilege(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<RolePrivilege>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<RolePrivilege>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<RolePrivilege>()).Returns(repository);
		} 


		public static void MockRoleReportPrivilege(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<RoleReportPrivilege>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<RoleReportPrivilege>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<RoleReportPrivilege>()).Returns(repository);
		}

		public static void MockPageNo(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Page>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Page>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Page>()).Returns(repository);
		}



		public static void MockGroup(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Group>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Group>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Group>()).Returns(repository);
		}
		public static void MockReport(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Report>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Report>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Report>()).Returns(repository);
		} 

		public static void MockPayMonth(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<PayMonth>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<PayMonth>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<PayMonth>()).Returns(repository);
		}


		public static void MockPaySheet(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<PaySheet>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<PaySheet>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<PaySheet>()).Returns(repository);
		}
		public static void MockDesignation(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Designation>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Designation>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Designation>()).Returns(repository);
		}


		public static void MockSequenceNo(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<SequenceNo>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<SequenceNo>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<SequenceNo>()).Returns(repository);
		}


		public static void MockUserLoginLog(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<UserLoginLog>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<UserLoginLog>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<UserLoginLog>()).Returns(repository);
		}


		public static void MockUserLoginFail(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<UserLoginFail>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<UserLoginFail>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<UserLoginFail>()).Returns(repository);
		}



		public static void MockUser(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<User>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<User>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<User>()).Returns(repository);
		}


		public static void MockUserAudit(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<UserAudit>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<UserAudit>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<UserAudit>()).Returns(repository);
		}

		public static void MockUnAuthorizedLeaves(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<UnAuthorizedLeaves>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<UnAuthorizedLeaves>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<UnAuthorizedLeaves>()).Returns(repository);
		}
		public static void MockUserPassword(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<UserPasswords>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<UserPasswords>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<UserPasswords>()).Returns(repository);
		}
	}
}
