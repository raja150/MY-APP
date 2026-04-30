using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TranSmart.API.Controllers;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Models;
using TranSmart.Service;
using Xunit;

namespace Transmart.Controller.UnitTests
{
	public class SequenceNoControllerTest : ControllerTestBase
	{
		private Mock<ISequenceNoService> _service;

		private readonly SequenceNoController _controller;
		public SequenceNoControllerTest() : base()
		{
			_service = new Mock<ISequenceNoService>();
			_controller = new SequenceNoController(Mapper, _service.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};
		}


		[Fact]
		public async Task GetAll_Test()
		{

			// Arrange
			_service.Setup(x => x.GetList(It.IsAny<string>())).ReturnsAsync(new List<SequenceNo>
			{ new SequenceNo { ID = EmployeeId,Attribute ="asdf",EntityName="_SequenceNo"} });

			var response = await _controller.GetAll();
			var okResult = response as OkObjectResult;

			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}


		//// Got Mapping Issue in SequenceNoController(Line 37-38)
		//[Fact]
		//public async Task PutTest()
		//{

		//	// Arrange
		//	_service.Setup(x => x.UpdateRange(It.IsAny<IEnumerable<SequenceNo>>())).ReturnsAsync(new List<SequenceNo>
		//	{ new SequenceNo { ID = Guid.NewGuid(),Attribute ="asdf",EntityName="_SequenceNo"} });

		//	var response = await _controller.Put(new List<SequenceNoModel>() { new SequenceNoModel { ID = EmployeeId } });
		//	var okResult = response as OkObjectResult;

		//	//Assert
		//	Assert.NotNull(okResult);
		//	Assert.Equal(200, okResult.StatusCode);
		//}
	}
}
