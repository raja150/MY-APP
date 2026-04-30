using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities;
using TranSmart.Service;
using Xunit;

namespace Transmart.Controller.UnitTests
{
	public class LookupValuesControllerTest : ControllerTestBase
	{

		private Mock<ILookupService> _service;

		private readonly LookupValuesController _controller;

		public LookupValuesControllerTest() : base()
		{
			_service = new Mock<ILookupService>();
			_controller = new LookupValuesController(Mapper, _service.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};
		}


		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public async void POST_TEST(bool hasError)
		{
			var result = new Result<LookUpValues>();

			_ = !hasError
				? _service.Setup(x => x.AddAsync(It.IsAny<LookUpValues>()))
			 .Callback(() =>
			 {
				 result.IsSuccess = true;
				 result.ReturnValue = new LookUpValues { ID = EmployeeId };
			 }).ReturnsAsync(result)
				:
				_service.Setup(x => x.AddAsync(It.IsAny<LookUpValues>()))
			   .Callback(() =>
			   {
				   result.IsSuccess = false;
				   result.ReturnValue = null;
			   }).ReturnsAsync(result);

			// Act
			var resposne = await _controller.Post(new TranSmart.Domain.Models.LookUpValues());
			var okResult = resposne as OkObjectResult;
			if (hasError)
			{
				Assert.Null(okResult);
			}
			else
			{
				Assert.NotNull(okResult);
				Assert.Equal(200, okResult.StatusCode);
			}
		}



		[Theory]
		[InlineData(1, "T1")]
		[InlineData(0, "T5")]
		public async void GET_BY_CODE(int count, string name)
		{
			var lookUps = new List<LookUpValues>
			{
				new LookUpValues {Text ="Test1" ,Code ="T1"},
				new LookUpValues {Text ="Test2" ,Code ="T2"},
				new LookUpValues {Text ="Test3" ,Code ="T3"},
				new LookUpValues {Text ="Test4" ,Code ="T4"},
			};
			_ = _service.Setup(x => x.Search(It.IsAny<string>())).ReturnsAsync(lookUps.Where(x => x.Code == name));
			_ = _controller.GetType().GetMethod("Get");

			var response = await _controller.Get(name);
			var okResult = response as OkObjectResult;
		 
			Assert.Equal(count, ((List<TranSmart.Domain.Models.LookUpValues>)okResult.Value).Count);
		}
	}
}
