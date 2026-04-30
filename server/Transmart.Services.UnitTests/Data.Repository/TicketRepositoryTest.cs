using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Data.Repository.HelpDesk;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Services.UnitTests;
using Xunit;

namespace Transmart.Services.UnitTests.Data.Repository
{
	public class TicketRepositoryTest : IClassFixture<InMemoryFixture>
	{
		private readonly InMemoryFixture inMemory;
		private readonly TicketRepository _repository;
		private readonly Guid managerId = Guid.NewGuid();
		private readonly Guid helpTopicId = Guid.NewGuid();
		private readonly Guid subHelpTopicId = Guid.NewGuid();
		private readonly Guid empId = Guid.NewGuid();
		private readonly Guid ticketStatusId = Guid.NewGuid();

		public TicketRepositoryTest(InMemoryFixture fixture)
		{
			inMemory = fixture;
			_repository = new TicketRepository(inMemory.DbContext);
			inMemory.DbContext.SelfService_Ticket.AddRange(
				new TranSmart.Domain.Entities.SelfService.Ticket
				{
					ID = Guid.NewGuid(),
					DepartmentId = Guid.Parse("ba2270e1-e51e-4e70-86f8-90f7221eee17"),
					Department = new TranSmart.Domain.Entities.Helpdesk.DeskDepartment
					{
						ID = Guid.Parse("ba2270e1-e51e-4e70-86f8-90f7221eee17"),
						Department = "IT",
						ManagerId = managerId,
					},
					HelpTopicId = helpTopicId,
					HelpTopic = new TranSmart.Domain.Entities.Helpdesk.HelpTopic
					{
						DueHours = 2,
						Name = "HelpTopic",
						Status = true,
					},
					SubTopicId = subHelpTopicId,
					SubTopic = new TranSmart.Domain.Entities.Helpdesk.HelpTopicSub
					{
						SubTopic = "SubTopic",
						HelpTopicId = helpTopicId,
					},
					AssignedToId = Guid.Parse("1a83854b-f7f5-403e-8714-87c9f16eaeca"),
					RaiseById = Guid.Parse("1a83854b-f7f5-403e-8714-87c9f16eaeca"),
					RaiseBy = new Employee
					{
						Name = "Zill",
						AadhaarNumber = "123412341234",
						FirstName = "Alla",
						MobileNumber = "9696969696",
						No = "Avontix1826"
					},
					RaisedOn = DateTime.Now.AddDays(-2),
					Subject = "System format",
					Message = "Please check once",
					//Status = 1,
					ModifiedAt = DateTime.Now.AddDays(-1),
					ModifiedBy = "ABCD",
					No = "TC22",
					TicketStatusId = ticketStatusId,
					TicketStatus = new TranSmart.Domain.Entities.Helpdesk.TicketStatus 
					{ 
						ID = ticketStatusId,
						Name = "Ticket",
						IsClosed = false
					},
				});
			inMemory.DbContext.SaveChanges();

			inMemory.DbContext.DepartmentEmployee.AddRange(new TranSmart.Domain.Entities.HelpDesk.DepartmentEmployee
			{
				DeskDepartmentId = Guid.Parse("ba2270e1-e51e-4e70-86f8-90f7221eee17"),
				EmployeeId = empId,
			});
			inMemory.DbContext.SaveChanges();
			inMemory.DbContext.ChangeTracker.Clear();

			inMemory.DbContext.Helpdesk_DeskDepartment.AddRange(new TranSmart.Domain.Entities.Helpdesk.DeskDepartment
			{
				ID = Guid.Parse("ba2270e1-e51e-4e70-86f8-90f7221eee17"),
				Department = "Help desk department",
				ManagerId = Guid.NewGuid(),
				Status = true,
			});
			inMemory.DbContext.ChangeTracker.Clear();
			inMemory.DbContext.SaveChanges();

			inMemory.DbContext.Helpdesk_HelpTopic.AddRange(new TranSmart.Domain.Entities.Helpdesk.HelpTopic
			{
				ID = helpTopicId,
				Name = "New Help topic"
			});
			inMemory.DbContext.ChangeTracker.Clear();
			inMemory.DbContext.SaveChanges();

			inMemory.DbContext.Helpdesk_HelpTopicSub.AddRange(new TranSmart.Domain.Entities.Helpdesk.HelpTopicSub
			{
				ID = subHelpTopicId,
				HelpTopicId = helpTopicId,
			});
			inMemory.DbContext.ChangeTracker.Clear();
			inMemory.DbContext.SaveChanges();

			inMemory.DbContext.Organization_Employee.AddRange(new Employee
			{
				ID = Guid.Parse("1a83854b-f7f5-403e-8714-87c9f16eaeca"),
				Name = "Anudeep",
				FirstName = "Alla",
			});
			inMemory.DbContext.ChangeTracker.Clear();
			inMemory.DbContext.SaveChanges();
			inMemory.DbContext.Helpdesk_TicketStatus.AddRange(new TranSmart.Domain.Entities.Helpdesk.TicketStatus
			{
				ID = ticketStatusId,
				IsClosed = false,
			});
			inMemory.DbContext.ChangeTracker.Clear();
			inMemory.DbContext.SaveChanges();

		}
		[Fact]
		public async void GetTickets()
		{
			var ticketSearch = new TicketSearch()
			{
				Status = 1,
				Page = 1,
				Size = 5,
				IsDescend = true,
				RefId = managerId
			};
			var res = await _repository.GetTickets(ticketSearch);
			Assert.True(res.Count == 1);
		}
	}
}
