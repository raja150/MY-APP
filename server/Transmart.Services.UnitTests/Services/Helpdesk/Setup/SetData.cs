using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Data;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Domain.Entities.HelpDesk;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.SelfService;

namespace Transmart.Services.UnitTests.Services.Helpdesk.Setup
{
	public static class SetData
	{
		public static void MockDepartmentGroup(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<DepartmentGroup>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<DepartmentGroup>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<DepartmentGroup>()).Returns(repository);
		}


		public static void MockDeskDepartment(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<DeskDepartment>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<DeskDepartment>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<DeskDepartment>()).Returns(repository);
		}


		public static void MockDeskGroupEmployeeService(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<DeskGroupEmployee>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<DeskGroupEmployee>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<DeskGroupEmployee>()).Returns(repository);
		}

		public static void MockHelpTopicService(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<HelpTopic>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<HelpTopic>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<HelpTopic>()).Returns(repository);
		}

		public static void MockHelpTopicSubService(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<HelpTopicSub>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<HelpTopicSub>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<HelpTopicSub>()).Returns(repository);
		}

		public static void MockTicketLogService(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<TicketLog>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<TicketLog>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<TicketLog>()).Returns(repository);
		}

		public static void MockTicketService(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Ticket>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Ticket>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Ticket>()).Returns(repository);
		}

		public static void MockTicketStatusService(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<TicketStatus>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<TicketStatus>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<TicketStatus>()).Returns(repository);
		}

		public static void MockTicketLogRecipientsService(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<TicketLogRecipients>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<TicketLogRecipients>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<TicketLogRecipients>()).Returns(repository);
		}

		public static void MockEmployeeService(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<Employee>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Employee>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Employee>()).Returns(repository);
		}
		public static void MockDeskGroup(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<DeskGroup>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<DeskGroup>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<DeskGroup>()).Returns(repository);
		}
		public static void MockDepartmentEmployee(Mock<UnitOfWork<TranSmartContext>> uow, Mock<DbContext> context, dynamic mockSet)
		{
			context.Setup(x => x.Set<DepartmentEmployee>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<DepartmentEmployee>(context.Object);
			uow.Setup(m => m.GetRepositoryAsync<DepartmentEmployee>()).Returns(repository);
		}
	}
}
