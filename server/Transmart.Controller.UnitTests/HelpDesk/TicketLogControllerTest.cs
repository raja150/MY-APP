using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Helpdesk;
using TranSmart.API.Extensions;
using TranSmart.API.Services;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.HelpDesk;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Entities.SelfService;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Helpdesk.Request;
using TranSmart.Domain.Models.Organization;
using TranSmart.Domain.Models.SelfService.List;
using TranSmart.Service.Helpdesk;
using TranSmart.Service.Organization;
using TranSmart.Service.SelfService;
using Xunit;

namespace Transmart.Controller.UnitTests.Leave
{
	public class TicketLogControllerTest : ControllerTestBase
	{
		private readonly Mock<ITicketLogService> _ticketLogService;
		private readonly Mock<IEmployeeService> _employeeService;
		private readonly Mock<IFileServerService> _fService;
		private readonly Mock<ITicketService> _tService;
		private readonly TicketLogController _controller;
		public TicketLogControllerTest() : base()
		{
			_ticketLogService = new Mock<ITicketLogService>();
			_employeeService = new Mock<IEmployeeService>();
			_tService = new Mock<ITicketService>();
			_fService = new Mock<IFileServerService>();

			_controller = new TicketLogController(Mapper, _ticketLogService.Object,
				_employeeService.Object, _tService.Object, _fService.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};
		}

		[Fact]
		public async Task Ticket_Log_Post_Test()
		{
			//Arrange
			TranSmart.Core.Result.Result<TicketLog> result = new();
			TranSmart.Core.Result.Result<Task<EmployeesMailModel>> result1 = new();
			TranSmart.Core.Result.Result<Task<Employee>> result2 = new();
			_ticketLogService.Setup(x => x.AddAsync(It.IsAny<TicketLog>())).Callback((TicketLog ticketLog) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = ticketLog;
				result.ReturnValue.TicketId = EmployeeId;

			}).ReturnsAsync(result);

			_employeeService.Setup(x => x.GetLoginEmpMail(It.IsAny<Guid>()))
			.Returns(result2.ReturnValue = Task.FromResult(new Employee
			{
				ID = EmployeeId,
				PersonalEmail = "avontix@gmail.com",
				WorkEmail = "work@gamail.com"
			}));

			var controller = _controller;

			//Act
			var attributes = controller.GetType().GetMethod("PostReply").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.PostReply(new TicketLogModel { Response = "sdf", Recipients = new List<TicketLogRecipientsModel>() });
			var okResult = response as OkObjectResult;

			// Assert
			Assert.True(attributes.Any());
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.HD_Tickets);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Create);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as TicketLogModel).TicketId);
		}

		[Fact]
		public async Task TicketLog_DeptTransfer_Test()
		{
			//Arrange
			TranSmart.Core.Result.Result<Ticket> result = new();

			_ticketLogService.Setup(x => x.DeptTransfer(It.IsAny<Ticket>(), It.IsAny<Guid>()))
				.Callback(() =>
				{
					result.IsSuccess = true;
					result.ReturnValue = new Ticket();

				}).ReturnsAsync(result);

			var controller = _controller;

			//Act
			var response = await controller.DeptTransfer(new DeptTransferModel());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}


		[Fact]
		public async Task Ticket_Log_ReAssign_Test()
		{
			//Arrange

			TranSmart.Core.Result.Result<TicketLog> result = new();
			TranSmart.Core.Result.Result<Employee> result1 = new();

			_ticketLogService.Setup(x => x.ReAssign(It.IsAny<TicketLog>()))
				.Callback((TicketLog item) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new TicketLog { AssignedToId = EmployeeId };

			}).ReturnsAsync(result);

			_employeeService.Setup(x => x.GetLoginEmpMail(It.IsAny<Guid>())).Returns(Task.FromResult(result1.ReturnValue = new Employee { ID = EmployeeId, PersonalEmail = "avontix@gmail.com" }));


			var controller = _controller;

			//Act
			var attributes = controller.GetType().GetMethod("ReAssign").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.ReAssign(new TicketLogModel { AssignedToId = EmployeeId });
			var okResult = response as OkObjectResult;

			// Assert
			Assert.True(attributes.Any());
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.HD_Tickets);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Update);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as TicketLog).AssignedToId);
		}

		[Theory]
		[InlineData(1)]
		public async Task Ticket_Log_GetTickets_Test(int count)
		{
			//Arrange
			var list = new List<Ticket>
			{
				new Ticket
				{
					ID = EmployeeId
				},
				new Ticket
				{
					ID = Guid.NewGuid()
				}
			}.AsQueryable();

			TranSmart.Core.Result.Result<Task<EmployeesMailModel>> result1 = new();

			_ticketLogService.Setup(x => x.GetTicekts(It.IsAny<TicketSearch>())).ReturnsAsync(list.ToPaginate(0, 10));

			var controller = _controller;

			//Act
			var attributes = controller.GetType().GetMethod("GetTickets").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.GetTickets(new TicketSearch());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.True(attributes.Any());
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.HD_Tickets);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);


			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(count, ((TranSmart.API.Models.Paginate<TicketList>)okResult.Value).Items.Where(x => x.ID == EmployeeId).Count());
		}
	}
}
