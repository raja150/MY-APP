using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Helpdesk;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Domain.Models.Helpdesk;
using TranSmart.Service.Helpdesk;
using Xunit;

namespace Transmart.Controller.UnitTests.HelpDesk
{
	public class HelpTopicControllerTest : ControllerTestBase
	{
		private readonly Mock<IHelpTopicService> _helpTopicService;
		private readonly HelpTopicController _controller;
		public HelpTopicControllerTest() : base()
		{
			_helpTopicService = new Mock<IHelpTopicService>();
			_controller = new HelpTopicController(Mapper, _helpTopicService.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};
		}


		[Fact]
		public async Task GetHelpTopicsTest()
		{
			// Arrange
			_helpTopicService.Setup(x => x.GetHelpTopics(It.IsAny<Guid>()));

			// Act
			var response = await _controller.GetHelpTopics(It.IsAny<Guid>());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}
	}
}

