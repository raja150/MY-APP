using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Leave;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave.List;
using TranSmart.Service.Leave;
using Xunit;

namespace Transmart.Controller.UnitTests.Leave
{
	public class WeekOffDaysControllerTest : ControllerTestBase 
	{
		private Mock<IWeekOffDaysService> _weekOffDaysService;
		private readonly WeekOffDaysController _controller;

		public WeekOffDaysControllerTest() : base()
		{
			_weekOffDaysService = new Mock<IWeekOffDaysService>();
			_controller = new WeekOffDaysController(Mapper, _weekOffDaysService.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};
		}

		[Fact]
		public async Task WeekOffDaysController_GetAllWeekOffDays_GetValidRecords()
		{
			// Arrange  
			var list = new List<WeekOffDays>
			{
				new WeekOffDays
				{
					ID = EmployeeId,
					WeekDate=DateTime.Now,
				},
				new WeekOffDays
				{
					ID = Guid.NewGuid(),
					WeekDate=DateTime.Now,
				}
			}.AsQueryable();
			_weekOffDaysService.Setup(x => x.GetAllWeekOffDays(It.IsAny<BaseSearch>())).ReturnsAsync(list.ToPaginate(0, 10));

			// Act
			var response = await _controller.GetAllWeekOffDays(new BaseSearch());
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(2, ((TranSmart.API.Models.Paginate<WeekOffDaysList>)okResult.Value).Items.Count());

		}

		[Fact]
		public async Task WeekOffDaysController_DeleteWeekOffDays_GetValidRecords()
		{
			// Arrange  
			Result<WeekOffDays> result = new();
			_weekOffDaysService.Setup(x => x.DeleteWeekOffDays(It.IsAny<Guid>())).Callback((Guid id)=>
			{
				result.IsSuccess = true;
				result.ReturnValue = new WeekOffDays();
			}
			).ReturnsAsync(result);

			// Act
			var response = await _controller.DeleteWeekOffDays(EmployeeId);
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}
	}
}
