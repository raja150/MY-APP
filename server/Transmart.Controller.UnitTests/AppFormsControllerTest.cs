//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using TranSmart.API.Controllers;
//using TranSmart.Data.Specifications;
//using TranSmart.Domain.Entities;
//using TranSmart.Service;
//using Xunit;

//namespace Transmart.Controller.UnitTests
//{
//	public class AppFormsControllerTest : ControllerTestBase
//	{
//		private readonly Mock<IBaseService<AppForm>> _service;
//		private readonly AppFormsController _controller;
//		public AppFormsControllerTest()
//		{
//			_service = new Mock<IBaseService<AppForm>>();
//			_controller = new AppFormsController(_service.Object)
//			{
//				ControllerContext = new ControllerContext()
//				{
//					HttpContext = new DefaultHttpContext() { User = Claim }
//				}
//			};
//		}


//		[Theory]
//		[InlineData(false)]
//		[InlineData(true)]
//		public void GET_BY_TEST(bool isNull)
//		{
//			var appForms = new List<AppForm> { new AppForm { DisplayName = "Carrier", Name = "CarrierNames", Header = "Carrier" } };
//			var app = new List<AppForm> {};

//			if (isNull)
//			{
//				_ = _service.Setup(x => x.GetBySpecification(It.IsAny<AppFormSpecification>())).Returns(app);
//			}
//			else
//			{
//				_ = _service.Setup(x => x.GetBySpecification(It.IsAny<AppFormSpecification>())).Returns(appForms);
//			}

//			_ = _controller.GetType().GetMethod("Get");
//			var response = _controller.Get(Guid.NewGuid());
//			var okResult = response as OkObjectResult;

//			if (isNull)
//			{
//				Assert.Null(okResult);
//				Assert.False(false);
//			}
//			else
//			{
//				Assert.NotNull(okResult);
//				Assert.Equal(200, okResult.StatusCode);
//			}
//		}
//	}
//}
