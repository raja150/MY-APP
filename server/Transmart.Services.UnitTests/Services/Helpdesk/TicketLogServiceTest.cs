using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transmart.Services.UnitTests.Services.Helpdesk.Setup;
using Transmart.Services.UnitTests.Services.Leave.EmployeeData;
using TranSmart.Core;
using TranSmart.Data;
using TranSmart.Data.Repository.HelpDesk;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Domain.Entities.HelpDesk;
using TranSmart.Domain.Entities.SelfService;
using TranSmart.Service.Helpdesk;
using Xunit;

namespace Transmart.Services.UnitTests.Services.Helpdesk
{
	public class TicketLogServiceTest
	{
		private readonly Mock<TranSmartContext> _dbContext;
		private readonly Mock<UnitOfWork<TranSmartContext>> uow;
		private readonly ITicketLogService _service;
		private readonly Mock<DbContext> _context;
		private readonly EmployeeDataGenerator _employeeData;
		private readonly ITicketRepository _repository;

		public TicketLogServiceTest()
		{
			var builder = new DbContextOptionsBuilder<TranSmartContext>();
			var app = new Mock<IApplicationUser>();
			var dbContextOptions = builder.Options;
			_dbContext = new Mock<TranSmartContext>(dbContextOptions, app.Object);
			uow = new Mock<UnitOfWork<TranSmartContext>>(_dbContext.Object);
			_employeeData = new EmployeeDataGenerator();
			_repository = new TicketRepository(_dbContext.Object);
			_service = new TicketLogService(uow.Object, _repository);
			_context = new Mock<DbContext>();
		}

		[Fact]
		public async Task AddAsync_Success()
		{
			#region Arrange
			var ticketId = Guid.NewGuid();
			var ticketStatusId = Guid.NewGuid();
			var empId = Guid.NewGuid();
			var departmentId = Guid.NewGuid();
			var groupId = Guid.NewGuid();

			var mockTicketLogToDB = new List<TicketLog>
			{
				new TicketLog
				{
					 TicketId = Guid.NewGuid(),
					 Ticket =  new Ticket()
					 {
						//Status = 0
					 },
				}
			};
			var tickets = new List<Ticket>
			{
			   new Ticket
			   {
				   ID = ticketId,
				   TicketStatusId = ticketStatusId,
				   DepartmentId = departmentId,
				   TicketStatus = new TicketStatus
				   {
					   IsClosed = false
				   }
			   }
			};
			var departmentGrp = new List<DepartmentGroup>
			{
				new DepartmentGroup
				{
					DeskDepartmentId = departmentId,
					GroupsId = Guid.NewGuid(),
				}
			};
			var mockTicketStatusToDB = new List<TicketStatus>
			{
			   new TicketStatus
			   {
				   ID = ticketStatusId,
				   IsClosed = false
			   }
			};
			var deskDepartment = new List<DeskDepartment>
			{
				new DeskDepartment
				{
					ManagerId   = empId,
					Department ="Department"
				}
			};
			var deskGroup = new List<DeskGroup>
			{
				new DeskGroup
				{
					ID =groupId,
					Status = true,
					CanAssignTicket = 1,
					CanCloseTicket = 1,
					CanDeleteTicket = 1,
					CanEditTicket = 1,
					CanPostReply = 1,
				}
			};
			var departmentEmployees = new List<DepartmentEmployee>
			{
				new DepartmentEmployee
				{
					EmployeeId = empId,
					GroupId= groupId,
				}
			};
			#endregion
			#region Mock
			//Ticket Log
			var mockTicketLog = mockTicketLogToDB.AsQueryable().BuildMockDbSet();
			SetData.MockTicketLogService(uow, _context, mockTicketLog);

			//Tickets
			var mockTicket = tickets.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);

			//Department group
			var mockDepartmentGrp = departmentGrp.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentGroup(uow, _context, mockDepartmentGrp);

			//Ticket Status
			var mockTicketStatus = mockTicketStatusToDB.AsQueryable().BuildMockDbSet();
			SetData.MockTicketStatusService(uow, _context, mockTicketStatus);

			//Employee Mock
			var mockSetEmployee = _employeeData.GetAllEmployeesData().AsQueryable().BuildMockDbSet();
			SetData.MockEmployeeService(uow, _context, mockSetEmployee);

			//DeskDepartment
			var mockDeskDepartment = deskDepartment.AsQueryable().BuildMockDbSet();
			SetData.MockDeskDepartment(uow, _context, mockDeskDepartment);

			//DeskGroup
			var mockDeskGroup = deskGroup.AsQueryable().BuildMockDbSet();
			SetData.MockDeskGroup(uow, _context, mockDeskGroup);

			//Department Employee
			var mockDepartmentEmployee = departmentEmployees.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentEmployee(uow, _context, mockDepartmentEmployee);

			#endregion
			var ticketLog = new TicketLog()
			{
				TicketId = ticketId,
				TicketStatusId = ticketStatusId,
				RepliedById = empId
			};

			//Act
			var dd = await _service.AddAsync(ticketLog);

			//Assert
			Assert.True(dd.IsSuccess);
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
		}

		[Fact]
		public async Task AddAsync_InvalidTicket_ThrowException()
		{
			//Arrange & Mock
			var mockTicketToDB = new List<Ticket>
			{
			   new Ticket
			   {
				   ID = Guid.NewGuid(),
				  // Status = 0
			   }
			};
			var mockTicket = mockTicketToDB.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);

			var depp = new TicketLog()
			{
				TicketId = Guid.NewGuid()
			};

			//Act
			var dd = await _service.AddAsync(depp);

			//Assert
			Assert.True(dd.HasError);
		}

		[Fact]
		public async Task AddAsync_InvalidUser_Exception()
		{
			#region Arrange
			var ticketId = Guid.NewGuid();
			var ticketStatusId = Guid.NewGuid();
			var empId = Guid.NewGuid();
			var departmentId = Guid.NewGuid();
			var groupId = Guid.NewGuid();

			var tickets = new List<Ticket>
			{
			   new Ticket
			   {
				   ID = ticketId,
				   TicketStatus=new TicketStatus{ ID = ticketStatusId, IsClosed=false },
				   DepartmentId = departmentId,
				   Department =new DeskDepartment{ ID = departmentId }
			   }
			};

			var departmentGroups = new List<DepartmentGroup>
			{
				new DepartmentGroup
			   {
				   ID = Guid.NewGuid(),
				   DeskDepartmentId = Guid.NewGuid(),
				   DeskDepartment =new DeskDepartment()
			   }
			};
			var ticketStatus = new List<TicketStatus>
			{
			   new TicketStatus
			   {
				   ID = ticketStatusId,
				   IsClosed = false
			   }
			};

			var deskDepartments = new List<DeskDepartment>
			{
				new DeskDepartment
				{
					ManagerId   = Guid.NewGuid(),
					Department ="Department"
				}
			};
			var deskGroup = new List<DeskGroup>
			{
				new DeskGroup
				{
					ID =groupId,
					Status = true,
					CanAssignTicket = 1,
					CanCloseTicket = 1,
					CanDeleteTicket = 1,
					CanEditTicket = 1,
					CanPostReply = 1,
				}
			};
			var departmentEmployees = new List<DepartmentEmployee>
			{
				new DepartmentEmployee
				{
					EmployeeId = Guid.NewGuid(),
					GroupId= groupId,
				}
			};
			#endregion
			#region Mock
			//Ticket
			var mockTicket = tickets.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);

			//DepartmentGroup
			var mockDepartmentGroup = departmentGroups.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentGroup(uow, _context, mockDepartmentGroup);

			//TicketStatus
			var mockTicketStatus = ticketStatus.AsQueryable().BuildMockDbSet();
			SetData.MockTicketStatusService(uow, _context, mockTicketStatus);

			//DeskDepartment
			var mockDeskDepartment = deskDepartments.AsQueryable().BuildMockDbSet();
			SetData.MockDeskDepartment(uow, _context, mockDeskDepartment);

			//DeskGroup
			var mockDeskGroup = deskGroup.AsQueryable().BuildMockDbSet();
			SetData.MockDeskGroup(uow, _context, mockDeskGroup);

			//Department Employee
			var mockDepartmentEmployee = departmentEmployees.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentEmployee(uow, _context, mockDepartmentEmployee);
			#endregion
			var ticketLog = new TicketLog()
			{
				TicketId = ticketId,
				TicketStatusId = ticketStatusId,
				RepliedById = empId
			};

			//Act
			var result = await _service.AddAsync(ticketLog);

			//Assert
			Assert.True(result.HasError);
			Assert.Single(result.Messages);
		}

		[Fact]
		public async Task AddAsync_ManagerLogin_NoException()
		{
			#region Arrange
			var ticketId = Guid.NewGuid();
			var ticketStatusId = Guid.NewGuid();
			var empId = Guid.NewGuid();
			var departmentId = Guid.NewGuid();
			var groupId = Guid.NewGuid();

			var tickets = new List<Ticket>
			{
			   new Ticket
			   {
				   ID = ticketId,
				   TicketStatus=new TicketStatus{ ID = ticketStatusId, IsClosed=false },
				   DepartmentId = departmentId,
				   Department =new DeskDepartment{ ID = departmentId, ManagerId = empId }
			   }
			};

			var departmentGroups = new List<DepartmentGroup>
			{
				new DepartmentGroup
			   {
				   ID = Guid.NewGuid(),
				   DeskDepartmentId = Guid.NewGuid(),
				   DeskDepartment =new DeskDepartment()
			   }
			};
			var ticketStatus = new List<TicketStatus>
			{
			   new TicketStatus
			   {
				   ID = ticketStatusId,
				   IsClosed = false
			   }
			};

			var deskDepartments = new List<DeskDepartment>
			{
				new DeskDepartment
				{
					ID = departmentId,
					ManagerId   = empId,
					Department ="Department"
				}
			};
			var ticketLogs = new List<TicketLog>
			{
				new TicketLog
				{
					 TicketId = Guid.NewGuid(),
					 Ticket =  new Ticket(),
				}
			};

			var deskGroup = new List<DeskGroup>
			{
				new DeskGroup
				{
					ID =groupId,
					Status = true,
					CanAssignTicket = 1,
					CanCloseTicket = 1,
					CanDeleteTicket = 1,
					CanEditTicket = 1,
					CanPostReply = 1,
				}
			};
			var departmentEmployees = new List<DepartmentEmployee>
			{
				new DepartmentEmployee
				{
					EmployeeId = Guid.NewGuid(),
					GroupId= groupId,
				}
			};
			#endregion
			#region Mock
			//Ticket
			var mockTicket = tickets.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);

			//DepartmentGroup
			var mockDepartmentGroup = departmentGroups.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentGroup(uow, _context, mockDepartmentGroup);

			//TicketStatus
			var mockTicketStatus = ticketStatus.AsQueryable().BuildMockDbSet();
			SetData.MockTicketStatusService(uow, _context, mockTicketStatus);

			//DeskDepartment
			var mockDeskDepartment = deskDepartments.AsQueryable().BuildMockDbSet();
			SetData.MockDeskDepartment(uow, _context, mockDeskDepartment);

			//TicketLOg
			var mockTicketLog = ticketLogs.AsQueryable().BuildMockDbSet();
			SetData.MockTicketLogService(uow, _context, mockTicketLog);

			//DeskGroup
			var mockDeskGroup = deskGroup.AsQueryable().BuildMockDbSet();
			SetData.MockDeskGroup(uow, _context, mockDeskGroup);

			//Department Employee
			var mockDepartmentEmployee = departmentEmployees.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentEmployee(uow, _context, mockDepartmentEmployee);
			#endregion
			var ticketLog = new TicketLog()
			{
				TicketId = ticketId,
				TicketStatusId = ticketStatusId,
				RepliedById = empId,
				RepliedOn = DateTime.Now,
				Response = "Response",
				Recipients = new List<TicketLogRecipients>(),
				ModifiedBy = "ASDF",
			};

			//Act
			var result = await _service.AddAsync(ticketLog);

			//Assert
			Assert.True(result.HasNoError);
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
		}

		[Fact]
		public async Task AddAsync_GroupEmployeeCanPostReplayNo_ThrowException()
		{
			#region Arrange
			var ticketId = Guid.NewGuid();
			var ticketStatusId = Guid.NewGuid();
			var empId = Guid.NewGuid();
			var departmentId = Guid.NewGuid();
			var DeskGroupId = Guid.NewGuid();
			var groupId = Guid.NewGuid();

			var tickets = new List<Ticket>
			{
			   new Ticket
			   {
				   ID = ticketId,
				   TicketStatus=new TicketStatus{ ID = ticketStatusId, IsClosed=false },
				   DepartmentId = departmentId,
				   Department =new DeskDepartment{ ID = departmentId, ManagerId = empId }
			   }
			};

			var departmentGroups = new List<DepartmentGroup>
			{
				new DepartmentGroup
			   {
				   ID = Guid.NewGuid(),
				   GroupsId = DeskGroupId,
				   DeskDepartmentId = departmentId,
				   DeskDepartment =new DeskDepartment()
			   }
			};
			var ticketStatus = new List<TicketStatus>
			{
			   new TicketStatus
			   {
				   ID = ticketStatusId,
				   IsClosed = false
			   }
			};

			var deskDepartments = new List<DeskDepartment>
			{
				new DeskDepartment
				{
					ID = Guid.NewGuid(),
					ManagerId   = empId,
					Department ="Department"
				}
			};
			var ticketLogs = new List<TicketLog>
			{
				new TicketLog
				{
					 TicketId = Guid.NewGuid(),
					 Ticket =  new Ticket(),
				}
			};

			var deskGroup = new List<DeskGroup>
			{
				new DeskGroup
				{
					ID =groupId,
					Status = true,
					CanAssignTicket = 1,
					CanCloseTicket = 1,
					CanDeleteTicket = 1,
					CanEditTicket = 1,
					CanPostReply = 0,
				}
			};
			var departmentEmployees = new List<DepartmentEmployee>
			{
				new DepartmentEmployee
				{
					EmployeeId = empId,
					GroupId= groupId,
				}
			};

			#endregion
			#region Mock
			//Ticket
			var mockTicket = tickets.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);

			//Department Group
			var mockDepartmentGroup = departmentGroups.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentGroup(uow, _context, mockDepartmentGroup);

			//Ticket Status
			var mockTicketStatus = ticketStatus.AsQueryable().BuildMockDbSet();
			SetData.MockTicketStatusService(uow, _context, mockTicketStatus);

			//Desk Department
			var mockDeskDepartment = deskDepartments.AsQueryable().BuildMockDbSet();
			SetData.MockDeskDepartment(uow, _context, mockDeskDepartment);

			//Ticket Log
			var mockTicketLog = ticketLogs.AsQueryable().BuildMockDbSet();
			SetData.MockTicketLogService(uow, _context, mockTicketLog);

			//DeskGroup
			var mockDeskGroup = deskGroup.AsQueryable().BuildMockDbSet();
			SetData.MockDeskGroup(uow, _context, mockDeskGroup);

			//Department Employee
			var mockDepartmentEmployee = departmentEmployees.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentEmployee(uow, _context, mockDepartmentEmployee);
			#endregion
			var ticketLog = new TicketLog()
			{
				TicketId = ticketId,
				TicketStatusId = ticketStatusId,
				RepliedById = empId,
				RepliedOn = DateTime.Now,
				Response = "Response",
				Recipients = new List<TicketLogRecipients>(),
			};

			//Act
			var result = await _service.AddAsync(ticketLog);

			//Assert
			Assert.True(result.HasError);
			Assert.Single(result.Messages);
		}

		[Fact]
		public async Task AddAsync_GroupEmployeeCanCloseTicketNo_ThrowException()
		{
			#region Arrange
			var ticketId = Guid.NewGuid();
			var ticketStatusId = Guid.NewGuid();
			var empId = Guid.NewGuid();
			var departmentId = Guid.NewGuid();
			var deskGroupId = Guid.NewGuid();
			var groupId = Guid.NewGuid();

			var tickets = new List<Ticket>
			{
			   new Ticket
			   {
				   ID = ticketId,
				   TicketStatus=new TicketStatus{ ID = ticketStatusId, IsClosed=false },
				   DepartmentId = departmentId,
				   Department =new DeskDepartment{ ID = departmentId, ManagerId = empId }
			   }
			};

			var departmentGroups = new List<DepartmentGroup>
			{
				new DepartmentGroup
			   {
				   ID = Guid.NewGuid(),
				   GroupsId = deskGroupId,
				   DeskDepartmentId = departmentId,
				   DeskDepartment =new DeskDepartment()
			   }
			};
			var ticketStatus = new List<TicketStatus>
			{
			   new TicketStatus
			   {
				   ID = ticketStatusId,
				   IsClosed = true
			   }
			};

			var deskDepartments = new List<DeskDepartment>
			{
				new DeskDepartment
				{
					ID = Guid.NewGuid(),
					ManagerId   = empId,
					Department ="Department"
				}
			};
			var ticketLogs = new List<TicketLog>
			{
				new TicketLog
				{
					 TicketId = Guid.NewGuid(),
					 Ticket =  new Ticket(),
				}
			};

			var deskGroup = new List<DeskGroup>
			{
				new DeskGroup
				{
					ID =groupId,
					Status = true,
					CanAssignTicket = 1,
					CanCloseTicket = 0,
					CanDeleteTicket = 1,
					CanEditTicket = 1,
					CanPostReply = 1,
				}
			};
			var departmentEmployees = new List<DepartmentEmployee>
			{
				new DepartmentEmployee
				{
					EmployeeId = empId,
					GroupId= groupId,
				}
			};

			#endregion
			#region Mock
			//Ticket
			var mockTicket = tickets.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);

			//Department Group
			var mockDepartmentGroup = departmentGroups.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentGroup(uow, _context, mockDepartmentGroup);

			//Ticket Status
			var mockTicketStatus = ticketStatus.AsQueryable().BuildMockDbSet();
			SetData.MockTicketStatusService(uow, _context, mockTicketStatus);

			//Desk Department
			var mockDeskDepartment = deskDepartments.AsQueryable().BuildMockDbSet();
			SetData.MockDeskDepartment(uow, _context, mockDeskDepartment);

			//Ticket Log
			var mockTicketLog = ticketLogs.AsQueryable().BuildMockDbSet();
			SetData.MockTicketLogService(uow, _context, mockTicketLog);

			//DeskGroup
			var mockDeskGroup = deskGroup.AsQueryable().BuildMockDbSet();
			SetData.MockDeskGroup(uow, _context, mockDeskGroup);

			//Department Employee
			var mockDepartmentEmployee = departmentEmployees.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentEmployee(uow, _context, mockDepartmentEmployee);
			#endregion
			var ticketLog = new TicketLog()
			{
				TicketId = ticketId,
				TicketStatusId = ticketStatusId,
				RepliedById = empId,
				RepliedOn = DateTime.Now,
				Response = "Response",
				Recipients = new List<TicketLogRecipients>(),
			};

			//Act
			var result = await _service.AddAsync(ticketLog);

			//Assert
			Assert.True(result.HasError);
			Assert.Single(result.Messages);
		}

		[Fact]
		public void DepartmentTransfer_InvalidTicket_ThrowException()
		{
			// Arrange  &  Mock
			var id = Guid.Parse("8A6A545B-BA1C-495B-32FA-08DA64C93928");
			var mockTicketLogToDB = new List<TicketLog>
			{
				new TicketLog
				{
					Ticket=new Ticket
					{
						ID = id,
						//Status = 0
					}
				},
			};

			var tickets = new List<Ticket>
			{
				  new Ticket
				  {
					 ID = id,
					 //Status = 0
				  }
			};

			var mockTicketLog = mockTicketLogToDB.AsQueryable().BuildMockDbSet();
			var mockTicket = tickets.AsQueryable().BuildMockDbSet();

			SetData.MockTicketLogService(uow, _context, mockTicketLog);
			SetData.MockTicketService(uow, _context, mockTicket);

			var ticket = new Ticket()
			{
				ID = Guid.NewGuid(),
				DepartmentId = Guid.NewGuid(),
				Message = "Hi"
			};

			//Act
			var dd = _service.DeptTransfer(ticket, Guid.Empty).Result;

			//Assert
			Assert.True(dd.HasError);
		}

		[Fact]
		public async Task DepartmentTransfer_InvalidUser_ThrowException()
		{
			#region Arrange
			var ticketId = Guid.NewGuid();
			var ticketStatusId = Guid.NewGuid();
			var departmentId = Guid.NewGuid();
			var groupId = Guid.NewGuid();

			var tickets = new List<Ticket>
			{
			   new Ticket
			   {
				   ID = ticketId,
				   TicketStatus=new TicketStatus{ ID = ticketStatusId, IsClosed=false },
				   DepartmentId = departmentId,
				   Department =new DeskDepartment{ ID = departmentId }
			   }
			};

			var departmentGroups = new List<DepartmentGroup>
			{
				new DepartmentGroup
			   {
				   ID = Guid.NewGuid(),
				   DeskDepartmentId = Guid.NewGuid(),
				   DeskDepartment =new DeskDepartment()
			   }
			};

			var deskDepartments = new List<DeskDepartment>
			{
				new DeskDepartment
				{
					ManagerId   = Guid.NewGuid(),
					Department ="Department"
				}
			};
			var deskGroup = new List<DeskGroup>
			{
				new DeskGroup
				{
					ID =groupId,
					Status = true,
					CanAssignTicket = 1,
					CanCloseTicket = 1,
					CanDeleteTicket = 1,
					CanEditTicket = 1,
					CanPostReply = 1,
				}
			};
			var departmentEmployees = new List<DepartmentEmployee>
			{
				new DepartmentEmployee
				{
					EmployeeId = Guid.NewGuid(),
					GroupId= groupId,
				}
			};
			#endregion
			#region Mock
			//Ticket
			var mockTicket = tickets.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);

			//Department Group
			var mockDepartmentGroup = departmentGroups.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentGroup(uow, _context, mockDepartmentGroup);

			//Desk Department
			var mockDeskDepartment = deskDepartments.AsQueryable().BuildMockDbSet();
			SetData.MockDeskDepartment(uow, _context, mockDeskDepartment);

			//DeskGroup
			var mockDeskGroup = deskGroup.AsQueryable().BuildMockDbSet();
			SetData.MockDeskGroup(uow, _context, mockDeskGroup);

			//Department Employee
			var mockDepartmentEmployee = departmentEmployees.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentEmployee(uow, _context, mockDepartmentEmployee);
			#endregion

			var ticket = new Ticket()
			{
				ID = ticketId,
				DepartmentId = Guid.NewGuid(),
				Message = "Hello"
			};

			//Act
			var result = await _service.DeptTransfer(ticket, Guid.Empty);

			//Assert
			Assert.True(result.HasError);
			Assert.Single(result.Messages);

		}

		[Fact]
		public async Task DepartmentTransfer_ManagerLogin_NoException()
		{
			#region Arrange
			var ticketId = Guid.NewGuid();
			var ticketStatusId = Guid.NewGuid();
			var empId = Guid.NewGuid();
			var departmentId = Guid.NewGuid();
			var groupId = Guid.NewGuid();

			var tickets = new List<Ticket>
			{
			   new Ticket
			   {
				   ID = ticketId,
				   TicketStatus=new TicketStatus{ ID = ticketStatusId, IsClosed=false },
				   DepartmentId = departmentId,
				   Department =new DeskDepartment{ ID = departmentId, ManagerId = empId }
			   }
			};

			var departmentGroups = new List<DepartmentGroup>
			{
				new DepartmentGroup
			   {
				   ID = Guid.NewGuid(),
				   DeskDepartmentId = Guid.NewGuid(),
				   DeskDepartment =new DeskDepartment()
			   }
			};

			var deskDepartments = new List<DeskDepartment>
			{
				new DeskDepartment
				{
					ID = departmentId,
					ManagerId   = empId,
					Department ="Department"
				}
			};
			var ticketLogs = new List<TicketLog>
			{
				new TicketLog
				{
					 TicketId = Guid.NewGuid(),
					 Ticket =  new Ticket(),
				}
			};
			var deskGroup = new List<DeskGroup>
			{
				new DeskGroup
				{
					ID =groupId,
					Status = true,
					CanAssignTicket = 1,
					CanCloseTicket = 1,
					CanDeleteTicket = 1,
					CanEditTicket = 1,
					CanPostReply = 1,
				}
			};
			var departmentEmployees = new List<DepartmentEmployee>
			{
				new DepartmentEmployee
				{
					EmployeeId = Guid.NewGuid(),
					GroupId= groupId,
				}
			};
			#endregion
			#region Mock
			//Ticket
			var mockTicket = tickets.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);

			//DepartmentGroup
			var mockDepartmentGroup = departmentGroups.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentGroup(uow, _context, mockDepartmentGroup);

			//DeskDepartment
			var mockDeskDepartment = deskDepartments.AsQueryable().BuildMockDbSet();
			SetData.MockDeskDepartment(uow, _context, mockDeskDepartment);

			//Ticket Log
			var mockTicketLog = ticketLogs.AsQueryable().BuildMockDbSet();
			SetData.MockTicketLogService(uow, _context, mockTicketLog);

			//DeskGroup
			var mockDeskGroup = deskGroup.AsQueryable().BuildMockDbSet();
			SetData.MockDeskGroup(uow, _context, mockDeskGroup);

			//Department Employee
			var mockDepartmentEmployee = departmentEmployees.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentEmployee(uow, _context, mockDepartmentEmployee);
			#endregion
			var ticket = new Ticket()
			{
				ID = ticketId,
				DepartmentId = Guid.NewGuid(),
				Message = "Hello"
			};
			//Act
			var result = await _service.DeptTransfer(ticket, empId);

			//Assert
			Assert.True(result.HasNoError);
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
		}

		[Fact]
		public async Task DepartmentTransfer_GroupEmployeeCanTransferTicketNo_ThrowException()
		{
			#region Arrange
			var ticketId = Guid.NewGuid();
			var ticketStatusId = Guid.NewGuid();
			var empId = Guid.NewGuid();
			var departmentId = Guid.NewGuid();
			var DeskGroupId = Guid.NewGuid();
			var groupId = Guid.NewGuid();

			var tickets = new List<Ticket>
			{
			   new Ticket
			   {
				   ID = ticketId,
				   TicketStatus=new TicketStatus{ ID = ticketStatusId, IsClosed=false },
				   DepartmentId = departmentId,
				   Department =new DeskDepartment{ ID = departmentId, ManagerId = empId }
			   }
			};

			var departmentGroups = new List<DepartmentGroup>
			{
				new DepartmentGroup
			   {
				   ID = Guid.NewGuid(),
				   GroupsId = DeskGroupId,
				   DeskDepartmentId = departmentId,
				   DeskDepartment =new DeskDepartment()
			   }
			};

			var deskDepartments = new List<DeskDepartment>
			{
				new DeskDepartment
				{
					ID = Guid.NewGuid(),
					ManagerId   = empId,
					Department ="Department"
				}
			};
			var ticketLogs = new List<TicketLog>
			{
				new TicketLog
				{
					 TicketId = Guid.NewGuid(),
					 Ticket =  new Ticket(),
				}
			};

			var deskGroup = new List<DeskGroup>
			{
				new DeskGroup
				{
					ID =groupId,
					Status = true,
					CanAssignTicket = 1,
					CanCloseTicket = 1,
					CanDeleteTicket = 1,
					CanEditTicket = 1,
					CanPostReply = 1,
					CanTransferTicket = 0,
				}
			};
			var departmentEmployees = new List<DepartmentEmployee>
			{
				new DepartmentEmployee
				{
					EmployeeId = empId,
					GroupId= groupId,
				}
			};
			#endregion
			#region Mock
			//Ticket
			var mockTicket = tickets.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);

			//Department Group
			var mockDepartmentGroup = departmentGroups.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentGroup(uow, _context, mockDepartmentGroup);

			//Desk Department
			var mockDeskDepartment = deskDepartments.AsQueryable().BuildMockDbSet();
			SetData.MockDeskDepartment(uow, _context, mockDeskDepartment);

			//Ticket Log
			var mockTicketLog = ticketLogs.AsQueryable().BuildMockDbSet();
			SetData.MockTicketLogService(uow, _context, mockTicketLog);

			//DeskGroup
			var mockDeskGroup = deskGroup.AsQueryable().BuildMockDbSet();
			SetData.MockDeskGroup(uow, _context, mockDeskGroup);

			//Department Employee
			var mockDepartmentEmployee = departmentEmployees.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentEmployee(uow, _context, mockDepartmentEmployee);
			#endregion
			var ticket = new Ticket()
			{
				ID = ticketId,
				DepartmentId = Guid.NewGuid(),
				Message = "Hello"
			};
			//Act
			var result = await _service.DeptTransfer(ticket, empId);

			//Assert
			Assert.True(result.HasError);
			Assert.Single(result.Messages);
		}

		[Fact]
		public async Task DepartmentTransfer_SaveChanges_ThrowException()
		{
			#region Arrange
			var ticketId = Guid.NewGuid();
			var ticketStatusId = Guid.NewGuid();
			var empId = Guid.NewGuid();
			var departmentId = Guid.NewGuid();
			var DeskGroupId = Guid.NewGuid();
			var groupId = Guid.NewGuid();

			var tickets = new List<Ticket>
			{
			   new Ticket
			   {
				   ID = ticketId,
				   TicketStatus=new TicketStatus{ ID = ticketStatusId, IsClosed=false },
				   DepartmentId = departmentId,
				   Department =new DeskDepartment{ ID = departmentId, ManagerId = empId }
			   }
			};

			var departmentGroups = new List<DepartmentGroup>
			{
				new DepartmentGroup
			   {
				   ID = Guid.NewGuid(),
				   GroupsId = DeskGroupId,
				   DeskDepartmentId = departmentId,
				   DeskDepartment =new DeskDepartment()
			   }
			};

			var deskDepartments = new List<DeskDepartment>
			{
				new DeskDepartment
				{
					ID = Guid.NewGuid(),
					ManagerId   = empId,
					Department ="Department"
				}
			};
			var ticketLogs = new List<TicketLog>
			{
				new TicketLog
				{
					 TicketId = Guid.NewGuid(),
					 Ticket =  new Ticket(),
				}
			};

			var deskGroup = new List<DeskGroup>
			{
				new DeskGroup
				{
					ID =groupId,
					Status = true,
					CanAssignTicket = 1,
					CanCloseTicket = 1,
					CanDeleteTicket = 1,
					CanEditTicket = 1,
					CanPostReply = 1,
					CanTransferTicket = 1,
				}
			};
			var departmentEmployees = new List<DepartmentEmployee>
			{
				new DepartmentEmployee
				{
					EmployeeId = empId,
					GroupId= groupId,
				}
			};

			#endregion
			#region Mock
			//Ticket
			var mockTicket = tickets.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);

			//Department Group
			var mockDepartmentGroup = departmentGroups.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentGroup(uow, _context, mockDepartmentGroup);

			//Desk Department
			var mockDeskDepartment = deskDepartments.AsQueryable().BuildMockDbSet();
			SetData.MockDeskDepartment(uow, _context, mockDeskDepartment);

			//Ticket Log
			var mockTicketLog = ticketLogs.AsQueryable().BuildMockDbSet();
			SetData.MockTicketLogService(uow, _context, mockTicketLog);

			//DeskGroup
			var mockDeskGroup = deskGroup.AsQueryable().BuildMockDbSet();
			SetData.MockDeskGroup(uow, _context, mockDeskGroup);

			//Department Employee
			var mockDepartmentEmployee = departmentEmployees.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentEmployee(uow, _context, mockDepartmentEmployee);
			#endregion
			var ticket = new Ticket()
			{
				ID = ticketId,
				DepartmentId = Guid.NewGuid(),
				Message = "Hello"
			};
			uow.Setup(x => x.SaveChangesAsync()).Throws(new InvalidOperationException());
			//Act
			var result = await _service.DeptTransfer(ticket, empId);

			//Assert
			Assert.True(result.HasError);
			Assert.Single(result.Messages);
		}

		[Fact]
		public async Task ReAssign_InvalidTicket_ThrowException()
		{
			//Arrange & Mock
			var mockTicketLogToDB = new List<TicketLog>
			{
			   new TicketLog {}
			};

			var mockTicketToDB = new List<Ticket>
			{
			   new Ticket {}
			};

			var mockTicket = mockTicketToDB.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);
			var mockTicketLog = mockTicketLogToDB.AsQueryable().BuildMockDbSet();
			SetData.MockTicketLogService(uow, _context, mockTicketLog);

			var ticketLog = new TicketLog()
			{
				ID = Guid.NewGuid(),
				Ticket = new Ticket() { },
				TicketId = Guid.NewGuid()
			};

			//Act
			var dd = await _service.ReAssign(ticketLog);

			//Assert
			Assert.True(dd.HasError);
		}

		[Fact]
		public async Task ReAssign_InvalidUser_ThrowException()
		{
			#region Arrange
			var ticketId = Guid.NewGuid();
			var ticketStatusId = Guid.NewGuid();
			var departmentId = Guid.NewGuid();
			var groupId = Guid.NewGuid();

			var tickets = new List<Ticket>
			{
			   new Ticket
			   {
				   ID = ticketId,
				   TicketStatus=new TicketStatus{ ID = ticketStatusId, IsClosed=false },
				   DepartmentId = departmentId,
				   Department =new DeskDepartment{ ID = departmentId }
			   }
			};

			var departmentGroups = new List<DepartmentGroup>
			{
				new DepartmentGroup
			   {
				   ID = Guid.NewGuid(),
				   DeskDepartmentId = Guid.NewGuid(),
				   DeskDepartment =new DeskDepartment()
			   }
			};

			var deskDepartments = new List<DeskDepartment>
			{
				new DeskDepartment
				{
					ManagerId   = Guid.NewGuid(),
					Department ="Department"
				}
			};
			var deskGroup = new List<DeskGroup>
			{
				new DeskGroup
				{
					ID =groupId,
					Status = true,
					CanAssignTicket = 1,
					CanCloseTicket = 1,
					CanDeleteTicket = 1,
					CanEditTicket = 1,
					CanPostReply = 1,
				}
			};
			var departmentEmployees = new List<DepartmentEmployee>
			{
				new DepartmentEmployee
				{
					EmployeeId = Guid.NewGuid(),
					GroupId= groupId,
				}
			};
			#endregion
			#region Mock
			//Ticket
			var mockTicket = tickets.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);

			//Department Group
			var mockDepartmentGroup = departmentGroups.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentGroup(uow, _context, mockDepartmentGroup);

			//Desk Department
			var mockDeskDepartment = deskDepartments.AsQueryable().BuildMockDbSet();
			SetData.MockDeskDepartment(uow, _context, mockDeskDepartment);

			//DeskGroup
			var mockDeskGroup = deskGroup.AsQueryable().BuildMockDbSet();
			SetData.MockDeskGroup(uow, _context, mockDeskGroup);

			//Department Employee
			var mockDepartmentEmployee = departmentEmployees.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentEmployee(uow, _context, mockDepartmentEmployee);
			#endregion
			var ticketLog = new TicketLog()
			{
				TicketId = ticketId,
				TicketStatusId = ticketStatusId,
				RepliedById = Guid.NewGuid(),
				RepliedOn = DateTime.Now,
				Response = "Response",
				Recipients = new List<TicketLogRecipients>(),
			};

			//Act
			var result = await _service.ReAssign(ticketLog);

			//Assert
			Assert.True(result.HasError);
			Assert.Single(result.Messages);

		}

		[Fact]
		public async Task ReAssign_ManagerLogin_NoException()
		{
			#region Arrange
			var ticketId = Guid.NewGuid();
			var ticketStatusId = Guid.NewGuid();
			var empId = Guid.NewGuid();
			var departmentId = Guid.NewGuid();
			var groupId = Guid.NewGuid();

			var tickets = new List<Ticket>
			{
			   new Ticket
			   {
				   ID = ticketId,
				   TicketStatus=new TicketStatus{ ID = ticketStatusId, IsClosed=false },
				   DepartmentId = departmentId,
				   Department =new DeskDepartment{ ID = departmentId, ManagerId = empId }
			   }
			};

			var departmentGroups = new List<DepartmentGroup>
			{
				new DepartmentGroup
			   {
				   ID = Guid.NewGuid(),
				   DeskDepartmentId = Guid.NewGuid(),
				   DeskDepartment =new DeskDepartment()
			   }
			};

			var deskDepartments = new List<DeskDepartment>
			{
				new DeskDepartment
				{
					ID = departmentId,
					ManagerId   = empId,
					Department ="Department"
				}
			};
			var ticketLogs = new List<TicketLog>
			{
				new TicketLog
				{
					 TicketId = Guid.NewGuid(),
					 Ticket =  new Ticket(),
				}
			};
			var deskGroup = new List<DeskGroup>
			{
				new DeskGroup
				{
					ID =groupId,
					Status = true,
					CanAssignTicket = 1,
					CanCloseTicket = 1,
					CanDeleteTicket = 1,
					CanEditTicket = 1,
					CanPostReply = 1,
				}
			};
			var departmentEmployees = new List<DepartmentEmployee>
			{
				new DepartmentEmployee
				{
					EmployeeId = Guid.NewGuid(),
					GroupId= groupId,
				}
			};
			#endregion
			#region Mock
			//Ticket
			var mockTicket = tickets.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);

			//DepartmentGroup
			var mockDepartmentGroup = departmentGroups.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentGroup(uow, _context, mockDepartmentGroup);

			//DeskDepartment
			var mockDeskDepartment = deskDepartments.AsQueryable().BuildMockDbSet();
			SetData.MockDeskDepartment(uow, _context, mockDeskDepartment);

			//Ticket Log
			var mockTicketLog = ticketLogs.AsQueryable().BuildMockDbSet();
			SetData.MockTicketLogService(uow, _context, mockTicketLog);

			//DeskGroup
			var mockDeskGroup = deskGroup.AsQueryable().BuildMockDbSet();
			SetData.MockDeskGroup(uow, _context, mockDeskGroup);

			//Department Employee
			var mockDepartmentEmployee = departmentEmployees.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentEmployee(uow, _context, mockDepartmentEmployee);

			#endregion
			var ticketLog = new TicketLog()
			{
				TicketId = ticketId,
				TicketStatusId = ticketStatusId,
				RepliedById = empId,
				RepliedOn = DateTime.Now,
				Response = "Response",
				Recipients = new List<TicketLogRecipients>(),
			};
			//Act
			var result = await _service.ReAssign(ticketLog);

			//Assert
			Assert.True(result.HasNoError);
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
		}

		[Fact]
		public async Task ReAssign_GroupEmployeeCanAssignTicket_ThrowException()
		{
			#region Arrange
			var ticketId = Guid.NewGuid();
			var ticketStatusId = Guid.NewGuid();
			var empId = Guid.NewGuid();
			var departmentId = Guid.NewGuid();
			var DeskGroupId = Guid.NewGuid();
			var groupId = Guid.NewGuid();

			var tickets = new List<Ticket>
			{
			   new Ticket
			   {
				   ID = ticketId,
				   TicketStatus=new TicketStatus{ ID = ticketStatusId, IsClosed=false },
				   DepartmentId = departmentId,
				   Department =new DeskDepartment{ ID = departmentId, ManagerId = empId }
			   }
			};

			var departmentGroups = new List<DepartmentGroup>
			{
				new DepartmentGroup
			   {
				   ID = Guid.NewGuid(),
				   GroupsId = DeskGroupId,
				   DeskDepartmentId = departmentId,
				   DeskDepartment =new DeskDepartment()
			   }
			};

			var deskDepartments = new List<DeskDepartment>
			{
				new DeskDepartment
				{
					ID = Guid.NewGuid(),
					ManagerId   = empId,
					Department ="Department"
				}
			};
			var ticketLogs = new List<TicketLog>
			{
				new TicketLog
				{
					 TicketId = Guid.NewGuid(),
					 Ticket =  new Ticket(),
				}
			};

			var deskGroup = new List<DeskGroup>
			{
				new DeskGroup
				{
					ID =groupId,
					Status = true,
					CanAssignTicket = 0,
					CanCloseTicket = 1,
					CanDeleteTicket = 1,
					CanEditTicket = 1,
					CanPostReply = 1,
					CanTransferTicket = 1,
				}
			};
			var departmentEmployees = new List<DepartmentEmployee>
			{
				new DepartmentEmployee
				{
					EmployeeId = empId,
					GroupId= groupId,
				}
			};

			#endregion
			#region Mock
			//Ticket
			var mockTicket = tickets.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);

			//Department Group
			var mockDepartmentGroup = departmentGroups.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentGroup(uow, _context, mockDepartmentGroup);

			//Desk Department
			var mockDeskDepartment = deskDepartments.AsQueryable().BuildMockDbSet();
			SetData.MockDeskDepartment(uow, _context, mockDeskDepartment);

			//Ticket Log
			var mockTicketLog = ticketLogs.AsQueryable().BuildMockDbSet();
			SetData.MockTicketLogService(uow, _context, mockTicketLog);

			//DeskGroup
			var mockDeskGroup = deskGroup.AsQueryable().BuildMockDbSet();
			SetData.MockDeskGroup(uow, _context, mockDeskGroup);

			//Department Employee
			var mockDepartmentEmployee = departmentEmployees.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentEmployee(uow, _context, mockDepartmentEmployee);
			#endregion
			var ticketLog = new TicketLog()
			{
				TicketId = ticketId,
				TicketStatusId = ticketStatusId,
				RepliedById = empId,
				RepliedOn = DateTime.Now,
				Response = "Response",
				Recipients = new List<TicketLogRecipients>(),
			};
			//Act
			var result = await _service.ReAssign(ticketLog);

			//Assert
			Assert.True(result.HasError);
			Assert.Single(result.Messages);
		}

		[Fact]
		public async Task ReAssign_GroupEmployeeCanAssignTicket_WithOutException()
		{
			#region Arrange
			var ticketId = Guid.NewGuid();
			var ticketStatusId = Guid.NewGuid();
			var empId = Guid.NewGuid();
			var departmentId = Guid.NewGuid();
			var DeskGroupId = Guid.NewGuid();
			var groupId = Guid.NewGuid();

			var tickets = new List<Ticket>
			{
			   new Ticket
			   {
				   ID = ticketId,
				   TicketStatus=new TicketStatus{ ID = ticketStatusId, IsClosed=false },
				   DepartmentId = departmentId,
				   Department =new DeskDepartment{ ID = departmentId, ManagerId = empId }
			   }
			};

			var departmentGroups = new List<DepartmentGroup>
			{
				new DepartmentGroup
			   {
				   ID = Guid.NewGuid(),
				   GroupsId = DeskGroupId,
				   DeskDepartmentId = departmentId,
				   DeskDepartment =new DeskDepartment()
			   }
			};

			var deskDepartments = new List<DeskDepartment>
			{
				new DeskDepartment
				{
					ID = Guid.NewGuid(),
					ManagerId   = empId,
					Department ="Department"
				}
			};
			var ticketLogs = new List<TicketLog>
			{
				new TicketLog
				{
					 TicketId = Guid.NewGuid(),
					 Ticket =  new Ticket(),
				}
			};

			var deskGroup = new List<DeskGroup>
			{
				new DeskGroup
				{
					ID =groupId,
					Status = true,
					CanAssignTicket = 1,
					CanCloseTicket = 1,
					CanDeleteTicket = 1,
					CanEditTicket = 1,
					CanPostReply = 1,
					CanTransferTicket = 1,
				}
			};
			var departmentEmployees = new List<DepartmentEmployee>
			{
				new DepartmentEmployee
				{
					EmployeeId = empId,
					GroupId= groupId,
				}
			};

			#endregion
			#region Mock
			//Ticket
			var mockTicket = tickets.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);

			//Department Group
			var mockDepartmentGroup = departmentGroups.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentGroup(uow, _context, mockDepartmentGroup);

			//Desk Department
			var mockDeskDepartment = deskDepartments.AsQueryable().BuildMockDbSet();
			SetData.MockDeskDepartment(uow, _context, mockDeskDepartment);

			//Ticket Log
			var mockTicketLog = ticketLogs.AsQueryable().BuildMockDbSet();
			SetData.MockTicketLogService(uow, _context, mockTicketLog);

			//DeskGroup
			var mockDeskGroup = deskGroup.AsQueryable().BuildMockDbSet();
			SetData.MockDeskGroup(uow, _context, mockDeskGroup);

			//Department Employee
			var mockDepartmentEmployee = departmentEmployees.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentEmployee(uow, _context, mockDepartmentEmployee);
			#endregion
			var ticketLog = new TicketLog()
			{
				TicketId = ticketId,
				TicketStatusId = ticketStatusId,
				RepliedById = empId,
				RepliedOn = DateTime.Now,
				Response = "Response",
				Recipients = new List<TicketLogRecipients>(),
			};
			//Act
			var result = await _service.ReAssign(ticketLog);

			//Assert
			Assert.True(result.HasNoError);
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());
		}

		[Fact]
		public async Task GetTicketLog_GetValidTicketLogs()
		{
			var ticketId = Guid.NewGuid();
			//Arrange
			var ticketLogs = new List<TicketLog>
			{
				new TicketLog
				{
					 ID = Guid.NewGuid(),
					 TicketId = ticketId,
					 Ticket =  new Ticket(),
					 Response = "OK",
				},
				new TicketLog
				{
					 ID = Guid.NewGuid(),
					 TicketId = Guid.NewGuid(),
					 Ticket =  new Ticket(),
					 Response = "OK",
				},
				new TicketLog
				{
					 ID = Guid.NewGuid(),
					 TicketId = ticketId,
					 Ticket =  new Ticket(),
					 Response = "OK",
				},
			};

			//Mock
			var mockSet = ticketLogs.AsQueryable().BuildMockDbSet();
			SetData.MockTicketLogService(uow, _context, mockSet);
			//Act
			var result = await _service.GetTicketLog(ticketId);

			//Assert
			Assert.Equal(2, result.Count());
		}

		[Fact]
		public async Task TicketResponse_ReturnsSingleTicketLog()
		{
			var ticketLogId = Guid.NewGuid();
			//Arrange
			var ticketLogs = new List<TicketLog>
			{
				new TicketLog
				{
					 ID = ticketLogId,
					 TicketId = Guid.NewGuid(),
					 Ticket =  new Ticket(),
					 Response = "OK",
				},
				new TicketLog
				{
					 ID = Guid.NewGuid(),
					 TicketId = Guid.NewGuid(),
					 Ticket =  new Ticket(),
					 Response = "OK",
				},
				new TicketLog
				{
					 ID = Guid.NewGuid(),
					 TicketId = Guid.NewGuid(),
					 Ticket =  new Ticket(),
					 Response = "OK",
				},
			};

			//Mock
			var mockSet = ticketLogs.AsQueryable().BuildMockDbSet();
			SetData.MockTicketLogService(uow, _context, mockSet);
			//Act
			var result = await _service.TicketResponse(ticketLogId);

			//Assert
			Assert.Equal(ticketLogId, result.ID);
		}

		[Fact]
		public async Task UpdateResponse_InvalidLogResponse_ThrowException()
		{
			// Arrange
			var ticketLogs = new List<TicketLog>
			{
				new TicketLog
				{
					ID = Guid.NewGuid(),
					Ticket=new Ticket(),
					Response = "OK",
				},
			};

			//mock
			var mockTicketLog = ticketLogs.AsQueryable().BuildMockDbSet();
			SetData.MockTicketLogService(uow, _context, mockTicketLog);


			//Act
			var dd = await _service.UpdateResponse(Guid.NewGuid(), "Response", Guid.NewGuid());

			//Assert
			Assert.True(dd.HasError);
		}

		[Fact]
		public async Task UpdateResponse_TicketIsClosed_ThrowException()
		{
			// Arrange  &  Mock
			var id = Guid.NewGuid();
			var ticketLogs = new List<TicketLog>
			{
				new TicketLog
				{
					ID = id,
					Ticket=new Ticket(),
					Response = "OK",
				},
			};
			var tickets = new List<Ticket>
			{
			   new Ticket
			   {
				   ID = Guid.NewGuid(),
				   TicketStatus =new TicketStatus() { ID = Guid.NewGuid(), IsClosed = true }
			   }
			};

			//mock
			//Ticket Log
			var mockTicketLog = ticketLogs.AsQueryable().BuildMockDbSet();
			SetData.MockTicketLogService(uow, _context, mockTicketLog);
			//Ticket
			var mockTicket = tickets.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);


			//Act
			var dd = await _service.UpdateResponse(id, "Response", Guid.NewGuid());

			//Assert
			Assert.True(dd.HasError);
		}

		[Fact]
		public async Task UpdateResponse_CannotBeEditable_ThrowException()
		{
			#region Arrange
			var id = Guid.NewGuid();
			var ticketId = Guid.NewGuid();
			var empId = Guid.NewGuid();
			var groupId = Guid.NewGuid();

			var ticketLogs = new List<TicketLog>
			{
				new TicketLog
				{
					ID = id,
					TicketId = ticketId,
					Ticket=new Ticket(),
					Response = "OK",
					RepliedById = empId
				},
			};
			var tickets = new List<Ticket>
			{
			   new Ticket
			   {
				   ID = ticketId,
				   TicketStatus =new TicketStatus() { ID = Guid.NewGuid(), IsClosed = false }
			   }
			};
			var deskGroup = new List<DeskGroup>
			{
				new DeskGroup
				{
					ID =groupId,
					Status = true,
					CanAssignTicket = 1,
					CanCloseTicket = 1,
					CanDeleteTicket = 1,
					CanEditTicket = 0,
					CanPostReply = 1,
					CanTransferTicket =1,

				}
			};
			var departmentEmployees = new List<DepartmentEmployee>
			{
				new DepartmentEmployee
				{
					EmployeeId = empId,
					GroupId= groupId,
				}
			};
			#endregion
			#region Mock
			//Ticket Log
			var mockTicketLog = ticketLogs.AsQueryable().BuildMockDbSet();
			SetData.MockTicketLogService(uow, _context, mockTicketLog);

			//Ticket
			var mockTicket = tickets.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);

			//DeskGroup
			var mockDeskGroup = deskGroup.AsQueryable().BuildMockDbSet();
			SetData.MockDeskGroup(uow, _context, mockDeskGroup);

			//Department Employee
			var mockDepartmentEmployee = departmentEmployees.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentEmployee(uow, _context, mockDepartmentEmployee);
			#endregion
			//Act
			var dd = await _service.UpdateResponse(id, "Response", empId);

			//Assert
			Assert.True(dd.HasError);
		}

		[Fact]
		public async Task UpdateResponse_InvalidUser_ThrowException()
		{
			#region Arrange
			var id = Guid.NewGuid();
			var ticketId = Guid.NewGuid();
			var empId = Guid.NewGuid();
			var groupId = Guid.NewGuid();

			var ticketLogs = new List<TicketLog>
			{
				new TicketLog
				{
					ID = id,
					TicketId = ticketId,
					Ticket=new Ticket(),
					Response = "OK",
					RepliedById = empId
				},
			};
			var tickets = new List<Ticket>
			{
			   new Ticket
			   {
				   ID = ticketId,
				   TicketStatus =new TicketStatus() { ID = Guid.NewGuid(), IsClosed = false }
			   }
			};
			var departmentGroups = new List<DepartmentGroup>
			{
				new DepartmentGroup
			   {
				   ID = Guid.NewGuid(),
				   DeskDepartmentId = Guid.NewGuid(),
				   DeskDepartment =new DeskDepartment()
			   }
			};

			var deskDepartments = new List<DeskDepartment>
			{
				new DeskDepartment
				{
					ManagerId   = Guid.NewGuid(),
					Department ="Department"
				}
			};
			var deskGroup = new List<DeskGroup>
			{
				new DeskGroup
				{
					ID =groupId,
					Status = true,
					CanAssignTicket = 1,
					CanCloseTicket = 1,
					CanDeleteTicket = 1,
					CanEditTicket = 1,
					CanPostReply = 1,
					CanTransferTicket = 0,
				}
			};
			var departmentEmployees = new List<DepartmentEmployee>
			{
				new DepartmentEmployee
				{
					EmployeeId = empId,
					GroupId= groupId,
				}
			};
			#endregion
			#region Mock
			//Ticket Log
			var mockTicketLog = ticketLogs.AsQueryable().BuildMockDbSet();
			SetData.MockTicketLogService(uow, _context, mockTicketLog);

			//Ticket
			var mockTicket = tickets.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);

			//Department Group
			var mockDepartmentGroup = departmentGroups.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentGroup(uow, _context, mockDepartmentGroup);

			//Desk Department
			var mockDeskDepartment = deskDepartments.AsQueryable().BuildMockDbSet();
			SetData.MockDeskDepartment(uow, _context, mockDeskDepartment);

			//DeskGroup
			var mockDeskGroup = deskGroup.AsQueryable().BuildMockDbSet();
			SetData.MockDeskGroup(uow, _context, mockDeskGroup);

			//Department Employee
			var mockDepartmentEmployee = departmentEmployees.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentEmployee(uow, _context, mockDepartmentEmployee);
			#endregion

			//Act
			var dd = await _service.UpdateResponse(id, "Response", Guid.NewGuid());

			//Assert
			Assert.True(dd.HasError);
		}

		[Fact]
		public async Task UpdateResponse_ManagerLogin_NoException()
		{
			#region Arrange
			var id = Guid.NewGuid();
			var ticketId = Guid.NewGuid();
			var empId = Guid.NewGuid();
			var managerId = Guid.NewGuid();
			var departmentId = Guid.NewGuid();
			var groupId = Guid.NewGuid();

			var ticketLogs = new List<TicketLog>
			{
				new TicketLog
				{
					ID = id,
					TicketId = ticketId,
					Ticket=new Ticket(),
					Response = "OK",
					RepliedById = empId
				},
			};
			var tickets = new List<Ticket>
			{
			   new Ticket
			   {
				   ID = ticketId,
				   DepartmentId = departmentId,
				   TicketStatus =new TicketStatus() { ID = Guid.NewGuid(), IsClosed = false }
			   }
			};
			var departmentGroups = new List<DepartmentGroup>
			{
				new DepartmentGroup
			   {
				   ID = Guid.NewGuid(),
				   DeskDepartmentId = Guid.NewGuid(),
				   DeskDepartment =new DeskDepartment()
			   }
			};

			var deskDepartments = new List<DeskDepartment>
			{
				new DeskDepartment
				{
					ID = departmentId,
					ManagerId   = managerId,
					Department ="Department"
				}
			};
			var deskGroup = new List<DeskGroup>
			{
				new DeskGroup
				{
					ID =groupId,
					Status = true,
					CanAssignTicket = 1,
					CanCloseTicket = 1,
					CanDeleteTicket = 1,
					CanEditTicket = 1,
					CanPostReply = 1,
					CanTransferTicket = 0,
				}
			};
			var departmentEmployees = new List<DepartmentEmployee>
			{
				new DepartmentEmployee
				{
					EmployeeId = empId,
					GroupId= groupId,
				}
			};
			#endregion
			#region Mock
			//Ticket Log
			var mockTicketLog = ticketLogs.AsQueryable().BuildMockDbSet();
			SetData.MockTicketLogService(uow, _context, mockTicketLog);

			//Ticket
			var mockTicket = tickets.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);

			//Department Group
			var mockDepartmentGroup = departmentGroups.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentGroup(uow, _context, mockDepartmentGroup);

			//Desk Department
			var mockDeskDepartment = deskDepartments.AsQueryable().BuildMockDbSet();
			SetData.MockDeskDepartment(uow, _context, mockDeskDepartment);

			//DeskGroup
			var mockDeskGroup = deskGroup.AsQueryable().BuildMockDbSet();
			SetData.MockDeskGroup(uow, _context, mockDeskGroup);

			//Department Employee
			var mockDepartmentEmployee = departmentEmployees.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentEmployee(uow, _context, mockDepartmentEmployee);
			#endregion
			//Act
			var dd = await _service.UpdateResponse(id, "Response", managerId);

			//Assert
			Assert.True(dd.HasNoError);
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());

		}

		[Fact]
		public async Task UpdateResponse_GroupEmployeeCanEditTicket_WithOutException()
		{
			#region Arrange
			var id = Guid.NewGuid();
			var ticketId = Guid.NewGuid();
			var DeskGroupId = Guid.NewGuid();
			var empId = Guid.NewGuid();
			var departmentId = Guid.NewGuid();
			var groupId = Guid.NewGuid();

			var ticketLogs = new List<TicketLog>
			{
				new TicketLog
				{
					ID = id,
					TicketId = ticketId,
					Ticket=new Ticket(),
					Response = "OK",
					RepliedById = empId
				},
			};
			var tickets = new List<Ticket>
			{
			   new Ticket
			   {
				   ID = ticketId,
				   DepartmentId = departmentId,
				   TicketStatus =new TicketStatus() { ID = Guid.NewGuid(), IsClosed = false }
			   }
			};
			var departmentGroups = new List<DepartmentGroup>
			{
				new DepartmentGroup
			   {
				   ID = Guid.NewGuid(),
				   GroupsId = DeskGroupId,
				   DeskDepartmentId = departmentId,
				   DeskDepartment =new DeskDepartment()
			   }
			};

			var deskDepartments = new List<DeskDepartment>
			{
				new DeskDepartment
				{
					ID = departmentId,
					ManagerId   = empId,
					Department ="Department"
				}
			};

			var deskGroup = new List<DeskGroup>
			{
				new DeskGroup
				{
					ID =groupId,
					Status = true,
					CanAssignTicket = 1,
					CanCloseTicket = 1,
					CanDeleteTicket = 1,
					CanEditTicket = 1,
					CanPostReply = 1,
					CanTransferTicket = 0,
				}
			};
			var departmentEmployees = new List<DepartmentEmployee>
			{
				new DepartmentEmployee
				{
					EmployeeId = empId,
					GroupId= groupId,
				}
			};
			#endregion
			#region Mock
			//Ticket
			var mockTicket = tickets.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);

			//Department Group
			var mockDepartmentGroup = departmentGroups.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentGroup(uow, _context, mockDepartmentGroup);

			//Desk Department
			var mockDeskDepartment = deskDepartments.AsQueryable().BuildMockDbSet();
			SetData.MockDeskDepartment(uow, _context, mockDeskDepartment);

			//Ticket Log
			var mockTicketLog = ticketLogs.AsQueryable().BuildMockDbSet();
			SetData.MockTicketLogService(uow, _context, mockTicketLog);

			//DeskGroup
			var mockDeskGroup = deskGroup.AsQueryable().BuildMockDbSet();
			SetData.MockDeskGroup(uow, _context, mockDeskGroup);

			//Department Employee
			var mockDepartmentEmployee = departmentEmployees.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentEmployee(uow, _context, mockDepartmentEmployee);
			#endregion

			//Act
			var result = await _service.UpdateResponse(id, "Response", empId);

			//Assert
			Assert.True(result.HasNoError);
			uow.Verify(m => m.SaveChangesAsync(), Times.AtLeastOnce());

		}

		[Fact]
		public async Task UpdateResponse_SaveChanges_ThrowException()
		{
			#region Arrange
			var id = Guid.NewGuid();
			var ticketId = Guid.NewGuid();
			var DeskGroupId = Guid.NewGuid();
			var empId = Guid.NewGuid();
			var departmentId = Guid.NewGuid();
			var groupId = Guid.NewGuid();

			var ticketLogs = new List<TicketLog>
			{
				new TicketLog
				{
					ID = id,
					TicketId = ticketId,
					Ticket=new Ticket(),
					Response = "OK",
					RepliedById = empId
				},
			};
			var tickets = new List<Ticket>
			{
			   new Ticket
			   {
				   ID = ticketId,
				   DepartmentId = departmentId,
				   TicketStatus =new TicketStatus() { ID = Guid.NewGuid(), IsClosed = false }
			   }
			};
			var departmentGroups = new List<DepartmentGroup>
			{
				new DepartmentGroup
			   {
				   ID = Guid.NewGuid(),
				   GroupsId = DeskGroupId,
				   DeskDepartmentId = departmentId,
				   DeskDepartment =new DeskDepartment()
			   }
			};

			var deskDepartments = new List<DeskDepartment>
			{
				new DeskDepartment
				{
					ID = departmentId,
					ManagerId   = empId,
					Department ="Department"
				}
			};

			var deskGroup = new List<DeskGroup>
			{
				new DeskGroup
				{
					ID =groupId,
					Status = true,
					CanAssignTicket = 1,
					CanCloseTicket = 1,
					CanDeleteTicket = 1,
					CanEditTicket = 1,
					CanPostReply = 1,
					CanTransferTicket = 0,
				}
			};
			var departmentEmployees = new List<DepartmentEmployee>
			{
				new DepartmentEmployee
				{
					EmployeeId = empId,
					GroupId= groupId,
				}
			};

			#endregion
			#region Mock
			//Ticket
			var mockTicket = tickets.AsQueryable().BuildMockDbSet();
			SetData.MockTicketService(uow, _context, mockTicket);

			//Department Group
			var mockDepartmentGroup = departmentGroups.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentGroup(uow, _context, mockDepartmentGroup);

			//Desk Department
			var mockDeskDepartment = deskDepartments.AsQueryable().BuildMockDbSet();
			SetData.MockDeskDepartment(uow, _context, mockDeskDepartment);

			//Ticket Log
			var mockTicketLog = ticketLogs.AsQueryable().BuildMockDbSet();
			SetData.MockTicketLogService(uow, _context, mockTicketLog);

			//DeskGroup
			var mockDeskGroup = deskGroup.AsQueryable().BuildMockDbSet();
			SetData.MockDeskGroup(uow, _context, mockDeskGroup);

			//Department Employee
			var mockDepartmentEmployee = departmentEmployees.AsQueryable().BuildMockDbSet();
			SetData.MockDepartmentEmployee(uow, _context, mockDepartmentEmployee);
			#endregion

			uow.Setup(x => x.SaveChangesAsync()).Throws(new InvalidOperationException());

			//Act
			var result = await _service.UpdateResponse(id, "Response", empId);

			//Assert
			Assert.True(result.HasError);
			Assert.Single(result.Messages);
		}

	}
}
