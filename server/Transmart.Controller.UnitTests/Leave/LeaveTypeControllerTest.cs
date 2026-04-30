using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Leave;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models.Leave;
using TranSmart.Domain.Models.Leave.Model;
using TranSmart.Service.Leave;
using Xunit;

namespace Transmart.Controller.UnitTests.Leave
{
	public class LeaveTypeControllerTest : ControllerTestBase
	{
		private Mock<ILeaveTypeService> _leaveTypeService;
		private readonly LeaveTypeController _controller;

		public LeaveTypeControllerTest() : base()
		{
			_leaveTypeService = new Mock<ILeaveTypeService>();
			_controller = new LeaveTypeController(Mapper, _leaveTypeService.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};
		}

		[Fact]
		public async Task LeaveTypeController_GetDefaultPayOffLeaveType_GetValidRecords()
		{
			// Arrange  
			_leaveTypeService.Setup(x => x.GetDefaultPayOffLeaveType()).ReturnsAsync(new LeaveType { ID= Guid.NewGuid(), DefaultPayoff=true});

			var controller = _controller;

			// Act
			var response = await controller.GetDefaultPayOffLeaveType();
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.True(((LeaveType)okResult.Value).DefaultPayoff);

		}

		[Fact]
		public async Task LeaveTypeController_GetPaidLeaveTypeList_GetValidRecords()
		{
			// Arrange  
			_leaveTypeService.Setup(x => x.GetDefaultPayOffLeaveType()).ReturnsAsync(new LeaveType
			{
				ID = Guid.NewGuid(),
				DefaultPayoff = true
			});

			var controller = _controller;

			// Act
			var response = await controller.GetPaidLeaveTypeList();
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);

		}
	}
}
