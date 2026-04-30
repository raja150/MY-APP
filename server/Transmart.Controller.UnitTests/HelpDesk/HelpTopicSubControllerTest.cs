using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Helpdesk;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Service.Helpdesk;
using Xunit;

namespace Transmart.Controller.UnitTests.HelpDesk
{
	public class HelpTopicSubControllerTest : ControllerTestBase
	{
		private Mock<IHelpTopicSubService> _helpTopicSubservice;
		private readonly HelpTopicSubController _controller;

		public HelpTopicSubControllerTest() : base()
		{
			_helpTopicSubservice = new Mock<IHelpTopicSubService>();
			_controller = new HelpTopicSubController(Mapper, _helpTopicSubservice.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};

		}

		[Fact]
		public async Task GetHelpTopicSubsTest()
		{
			// Arrange  
			_helpTopicSubservice.Setup(x => x.GetHelpTopicSubs(It.IsAny<Guid>()));
			
			// Act
			var response = _controller.GetHelpTopicSubs(It.IsAny<Guid>());
			var okResult = await response as OkObjectResult;

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}
	}
}


