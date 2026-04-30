using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Domain.Entities.HelpDesk;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Entities.SelfService;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;
using TranSmart.Service;
using TranSmart.Service.SelfService;
using Xunit;

namespace Transmart.Services.UnitTests.Services.SelfService
{
	public class TicketServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly ISequenceNoService sequenceNoService;
		private readonly ITicketService _service;
		private readonly Mock<DbContext> _context;

		public TicketServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			sequenceNoService = new SequenceNoService(uow.Object);
			_service = new TicketService(uow.Object, sequenceNoService);
			_context = new Mock<DbContext>();
		}
		[Fact]
		public async Task GetPaginateTests()
		{
			// Arrange
			TicketSearch baseSearch = new()
			{
				SortBy = "Employee",
				Size = 10,
				Page = 0,
				RefId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6")
			};
			Employee employee = new()
			{
				ID = Guid.NewGuid(),
				AadhaarNumber = "12131231232",
				DateOfJoining = DateTime.Parse("2019-12-30"),
				Gender = 1,
				Name = "Raja",
				No = "Avontix1796",
				DesignationId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6")
			};
			IEnumerable<Ticket> data = new List<Ticket>
			{
				new Ticket
				{
					RaiseById = employee.ID,
					ID = Guid.NewGuid(),
					No = employee.No,
					File = "f1",
					Message = "m1",
					Subject ="s1",
					//Status = (byte)TicketSts.OverDue,
				},
				new Ticket
				{
					RaiseById = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					ID = Guid.NewGuid(),
					No = employee.No,
					File = "f2",
					Message = "m2",
					Subject ="s2",
					//Status = (byte)TicketSts.Open,
				},
				new Ticket
				{
					RaiseById = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					ID = Guid.NewGuid(),
					No = employee.No,
					File = "f3",
					Message = "m3",
					Subject ="s3",
					//Status = (byte)TicketSts.InProcess,
				},

			}.AsQueryable();

			//Mock
			var mockSet = data.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<Ticket>()).Returns(mockSet.Object);
			var repository = new RepositoryAsync<Ticket>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Ticket>()).Returns(repository);

			//Act
			var ticket = await _service.GetPaginate(baseSearch);
			Assert.True(ticket.Count == 2);
		}

		[Fact]
		public async Task AddAsync_Test()
		{
			//Arrange
			//Ticket
			IEnumerable<Ticket> data = new List<Ticket>
			{
				new Ticket
				{
					RaiseById = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					ID = Guid.NewGuid(),
					No = "2222",
					File = "ffff",
					Message = "mmmm",
					Subject ="ssss",
				},

			}.AsQueryable();

			//Sequence No
			var mockSequenceNoToDB = new List<SequenceNo>
			{
				new SequenceNo
				{
					EntityName="SelfService_Ticket",
					Attribute="No"
				 }
			};

			//HelpTopic
			IEnumerable<HelpTopic> hTopic = new List<HelpTopic>
			{
				new HelpTopic
				{
					DepartmentId=Guid.NewGuid(),
					Name="Open",
					TicketStatusId=Guid.Parse("f529e1f2-0583-4520-bcf5-9f17d83dbcb9"),
					ID=Guid.Parse("6048f68e-ac00-49dc-b0fd-18f3b0b6d815"),
					Status=true
				},

			}.AsQueryable();

			//Mock SequenceNo
			var mockSequenceNo = mockSequenceNoToDB.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<SequenceNo>()).Returns(mockSequenceNo.Object);
			var repository = new RepositoryAsync<SequenceNo>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<SequenceNo>()).Returns(repository);

			// Mock Ticket
			var mockSet = data.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<Ticket>()).Returns(mockSet.Object);
			var ticketRepository = new RepositoryAsync<Ticket>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Ticket>()).Returns(ticketRepository);


			// Mock Help Topic
			var mockHTopic = hTopic.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<HelpTopic>()).Returns(mockHTopic.Object);
			var htRepo = new RepositoryAsync<HelpTopic>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<HelpTopic>()).Returns(htRepo);

			//Act
			var outcome = await _service.AddAsync(new Ticket
			{
				ID = Guid.NewGuid(),
				RaiseById = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
				No = "No1",
				File = "f1",
				Message = "m1",
				Subject = "s1",
				HelpTopicId = Guid.Parse("6048f68e-ac00-49dc-b0fd-18f3b0b6d815"),
			});
			Assert.Equal("A00000", outcome.ReturnValue.No);
			Assert.Equal(DateTime.Now.Date, outcome.ReturnValue.RaisedOn.Date);
			uow.Verify(m => m.SaveChangesAsync());
		}

		[Fact]
		public async Task UpdateAsync_In_ValidTicket_Exception_Thrown()
		{
			//Arrange
			IEnumerable<Ticket> data = new List<Ticket>
			{
				new Ticket
				{
					ID = Guid.Parse("fee33674-e646-4e74-8bc0-7b1c667f4195"),
					No = "2222",
					File = "ffff",
					Message = "mmmm",
					Subject ="ssss",
					//Status = (byte)TicketSts.Open,
				},
				new Ticket
				{
					ID = Guid.NewGuid(),
					No = "4444",
					File = "ffff",
					Message = "mmmm",
					Subject ="ssss",
					//Status = (byte)TicketSts.Open,
				}

			}.AsQueryable();

			// Mock Ticket
			var mockSet = data.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<Ticket>()).Returns(mockSet.Object);
			var ticketRepository = new RepositoryAsync<Ticket>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Ticket>()).Returns(ticketRepository);

			//Act
			var outcome = await _service.UpdateAsync(new Ticket
			{
				ID = Guid.Parse("529ac94c-6b14-481f-9381-ea3e3f59e42a"),
				No = "4444",
				File = "ffff",
				Message = "mmmm",
				Subject = "ssss",
				//Status = (byte)TicketSts.Open,
			});
			Assert.True(outcome.HasError);
		}

		[Fact]
		public async Task UpdateAsync_Valid_Ticket_True()
		{
			//Arrange
			IEnumerable<Ticket> data = new List<Ticket>
			{
				new Ticket
				{
					ID = Guid.Parse("fee33674-e646-4e74-8bc0-7b1c667f4195"),
					No = "2222",
					File = "ffff",
					Message = "mmmm",
					Subject ="ssss",
					//Status = (byte)TicketSts.Open,
				},
				new Ticket
				{
					ID = Guid.NewGuid(),
					No = "4444",
					File = "ffff",
					Message = "mmmm",
					Subject ="ssss",
					//Status = (byte)TicketSts.Open,
				}

			}.AsQueryable();

			// Mock Ticket
			var mockSet = data.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<Ticket>()).Returns(mockSet.Object);
			var ticketRepository = new RepositoryAsync<Ticket>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<Ticket>()).Returns(ticketRepository);

			//Act
			var outcome = await _service.UpdateAsync(new Ticket
			{
				ID = Guid.Parse("fee33674-e646-4e74-8bc0-7b1c667f4195"),
				No = "N1",
				File = "f1",
				Message = "m1",
				Subject = "s1",
				//Status = (byte)TicketSts.Open,
				DepartmentId = Guid.Parse("1076db53-bd28-4be9-9a4f-775934b27353"),
				HelpTopicId = Guid.Parse("8d68b76e-68e8-4b86-bffc-3948164c2e00"),
				SubTopicId = Guid.Parse("14aa43ca-ff72-41c9-9385-49cdc7c014bc"),
			});
			Assert.Equal("s1", outcome.ReturnValue.Subject);
			Assert.Equal("m1", outcome.ReturnValue.Message);
			Assert.Equal(Guid.Parse("1076db53-bd28-4be9-9a4f-775934b27353"), outcome.ReturnValue.DepartmentId);
			Assert.Equal(Guid.Parse("8d68b76e-68e8-4b86-bffc-3948164c2e00"), outcome.ReturnValue.HelpTopicId);
			Assert.Equal(Guid.Parse("14aa43ca-ff72-41c9-9385-49cdc7c014bc"), outcome.ReturnValue.SubTopicId);
		}

		[Fact]
		public async Task Update_Response_Success()	
		{
			IEnumerable<TicketLog> data = new List<TicketLog>
			{
				new TicketLog
				{
					TicketId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					ID = Guid.NewGuid(),
				},

			}.AsQueryable();

			//Mock 
			var mockLog = data.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<TicketLog>()).Returns(mockLog.Object);
			var repository = new RepositoryAsync<TicketLog>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<TicketLog>()).Returns(repository);
			var result = await _service.UserResponse(Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"), "ADDED", Guid.NewGuid());
			uow.Verify(m => m.SaveChangesAsync());
			Assert.True(result.HasNoError);
		}
		[Fact]
		public async Task Update_Response_Fail()	
		{
			IEnumerable<TicketLog> data = new List<TicketLog>
			{
				new TicketLog
				{
					TicketId = Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"),
					ID = Guid.NewGuid(),
				},

			}.AsQueryable();

			//Mock 
			var mockLog = data.AsQueryable().BuildMockDbSet();
			_context.Setup(x => x.Set<TicketLog>()).Returns(mockLog.Object);
			var repository = new RepositoryAsync<TicketLog>(_context.Object);
			uow.Setup(m => m.GetRepositoryAsync<TicketLog>()).Returns(repository);
			uow.Setup(x => x.SaveChangesAsync()).Throws(new Exception("Exception occurred while executing method"));
			var result = await _service.UserResponse(Guid.Parse("bdd2fa1c-9310-4298-9d4a-0035b96edff6"), "ADDED", Guid.NewGuid());
			uow.Verify(m => m.SaveChangesAsync());
			Assert.True(result.HasError);
		}
	}
}
