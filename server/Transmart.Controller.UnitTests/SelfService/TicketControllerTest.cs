using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Leave.Approval;
using TranSmart.API.Controllers.Payroll;
using TranSmart.API.Extensions;
using TranSmart.API.Services;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.SelfService;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.SelfService.List;
using TranSmart.Domain.Models.SelfService.Request;
using TranSmart.Domain.Models.SelfService.Response;
using TranSmart.Service.SelfService;
using Xunit;

namespace Transmart.Controller.UnitTests
{
    public class TicketControllerTest : ControllerTestBase
    {
        private Mock<ITicketService> _ticketService;
		private Mock<IFileServerService> _fileServerService;
        public TicketControllerTest() : base()
        {
            _ticketService = new Mock<ITicketService>();
			_fileServerService = new Mock<IFileServerService>();
        }

        //[Fact]
        [Theory]
        [InlineData(1)]
        public async Task Ticket_GetPaginate_Test(int count)
        {
            // Arrange  
           Result<Ticket> result = new();

            // Act
            var list = new List<Ticket> { new Ticket { ID = EmployeeId }, new Ticket { ID = Guid.NewGuid() } }.AsQueryable();
            _ticketService.Setup(x => x.GetPaginate(It.IsAny<BaseSearch>())).ReturnsAsync(list.ToPaginate(0, 10));
            var controller = new TranSmart.API.Controllers.SelfService.TicketController(Mapper, _ticketService.Object, _fileServerService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = Claim }
                }
            };

            var attributes = controller.GetType().GetMethod("Paginate").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var resposne = await controller.Paginate(new TicketSearch());
            var okResult = resposne as OkObjectResult;

            // Assert
            Assert.True(attributes.Any());
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SS_RaiseTicket);
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(count, ((TranSmart.API.Models.Paginate<TicketList>)okResult.Value).Items.Where(x => x.ID == EmployeeId).Count());
        }

        [Fact]
        public async Task Ticket_Post_Test()
        {
            // Arrange  
            TranSmart.Core.Result.Result<Ticket> result = new();
            _ticketService.Setup(x => x.AddAsync(It.IsAny<Ticket>())).Callback((Ticket ticket) =>
            {
                result.IsSuccess = true;
                result.ReturnValue = ticket;
            }).ReturnsAsync(result);

            var controller = new TranSmart.API.Controllers.SelfService.TicketController(Mapper, _ticketService.Object,_fileServerService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = Claim }
                }
            };
            // Act
            var attributes = controller.GetType().GetMethod("Post").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var resposne = await controller.Post(new TicketRequest());
            var okResult = resposne as OkObjectResult;

            Assert.True(attributes.Any());
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SS_RaiseTicket);
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Create);

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as Ticket).RaiseById);
        }

        [Fact]
        public async Task Ticket_Put_Test()
        {
            // Arrange  
            TranSmart.Core.Result.Result<Ticket> result = new();
            _ticketService.Setup(x => x.UpdateAsync(It.IsAny<Ticket>())).Callback((Ticket ticket) =>
            {
                result.IsSuccess = true;
                result.ReturnValue = ticket;
            }).ReturnsAsync(result);

            var controller = new TranSmart.API.Controllers.SelfService.TicketController(Mapper, _ticketService.Object, _fileServerService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = Claim }
                }
            };
            // Act
            var attributes = controller.GetType().GetMethod("Put").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var resposne = await controller.Put(new TicketRequest());
            var okResult = resposne as OkObjectResult;

            Assert.True(attributes.Any());
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SS_RaiseTicket);
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Update);

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as Ticket).RaiseById);
        }
    }
}
