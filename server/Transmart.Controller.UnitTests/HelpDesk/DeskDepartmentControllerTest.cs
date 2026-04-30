using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Helpdesk;
using TranSmart.API.Extensions;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Domain.Models.Helpdesk;
using TranSmart.Service.Helpdesk;
using Xunit;

namespace Transmart.Controller.UnitTests.HelpDesk
{
	public class DeskDepartmentControllerTest : ControllerTestBase
	{
		private readonly Mock<IDeskDepartmentService> _deskDepartmentService;
		private readonly DeskDepartmentController _controller;
		public DeskDepartmentControllerTest() : base()
		{
			_deskDepartmentService = new Mock<IDeskDepartmentService>();
			_controller = new DeskDepartmentController(Mapper, _deskDepartmentService.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};
		}


		[Fact]
		public async Task GetDepListsTest()
		{
			// Arrange  
			_deskDepartmentService.Setup(x => x.GetDepartments());

			// Act
			var response = await _controller.GetDepLists();
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}
	}
}
