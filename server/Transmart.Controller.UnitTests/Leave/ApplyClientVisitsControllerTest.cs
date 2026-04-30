using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Leave;
using TranSmart.API.Extensions;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models.Leave.Model;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Domain.Models.SelfService;
using TranSmart.Service.Leave;
using Xunit;

namespace Transmart.Controller.UnitTests.Leave
{
    public class ApplyClientVisitsControllerTest : ControllerTestBase
    {

        private Mock<IApplyClientVisitsService> _applyclientVisitService;
        private readonly ApplyClientVisitsController _controller;

        public ApplyClientVisitsControllerTest() : base()
        {
            _applyclientVisitService = new Mock<IApplyClientVisitsService>();
            _controller = new ApplyClientVisitsController(Mapper, _applyclientVisitService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = Claim }
                }
            };
        }

		[Fact]
		public async Task ApplyClientVisits_GetList_GetValidRecords()
		{
			// Arrange  
			var id = Guid.NewGuid();

			Result<ApplyClientVisits> result = new();
			_applyclientVisitService.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(result.ReturnValue = new ApplyClientVisits { ID = id, EmployeeId = EmployeeId, PlaceOfVisit="HYD" }));

			var controller = _controller;

			// Act
			var response = await controller.GetList("HYD");
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}

		[Fact]
		public async Task ApplyClientVisits_GetById_GetValidRecords()
		{
			// Arrange  
			var id = Guid.NewGuid();

			Result<ApplyClientVisits> result = new();
			_applyclientVisitService.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(result.ReturnValue = new ApplyClientVisits { ID = id, EmployeeId = EmployeeId }));

			var controller = _controller;

			// Act
			var response = await controller.Get(id);
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, ((ApplyClientVisitModel)okResult.Value).EmployeeId);

		}

		[Fact]
        public async Task Apply_Client_Visits_Post_Test()
        {
            // Arrange  
            TranSmart.Core.Result.Result<ApplyClientVisits> result = new();
            _applyclientVisitService.Setup(x => x.AddAsync(It.IsAny<ApplyClientVisits>())).Callback((ApplyClientVisits applyClientVisits) =>
            {
                result.IsSuccess = true;
                result.ReturnValue = applyClientVisits;

            }).ReturnsAsync(result);

            var controller = _controller;

            // Act
            var attributes = controller.GetType().GetMethod("Post").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var response = await controller.Post(new ApplyClientVisitsModel());
            var okResult = response as OkObjectResult;

            // Assert
            Assert.True(attributes.Any());
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SS_ApplyClientVisits);
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Create);

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as ApplyClientVisitsModel).EmployeeId);
        }

		[Fact]
		public async Task Apply_Client_Visits_Post_BadRequest()
		{
			// Arrange  
			TranSmart.Core.Result.Result<ApplyClientVisits> result = new();
			_applyclientVisitService.Setup(x => x.AddAsync(It.IsAny<ApplyClientVisits>())).Callback((ApplyClientVisits applyClientVisits) =>
			{
				result.IsSuccess = false;
				result.ReturnValue = applyClientVisits;

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("Post").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.Post(new ApplyClientVisitsModel());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.True(attributes.Any());
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SS_ApplyClientVisits);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Create);

			Assert.Null(okResult);
		}


		[Fact]
        public async Task Apply_Client_Visits_Put_Test()
        {
            // Arrange  
            TranSmart.Core.Result.Result<ApplyClientVisits> result = new();
            _applyclientVisitService.Setup(x => x.UpdateAsync(It.IsAny<ApplyClientVisits>())).Callback((ApplyClientVisits applyClientVisits) =>
            {
                result.IsSuccess = true;
                result.ReturnValue = applyClientVisits;

            }).ReturnsAsync(result);

            var controller = _controller;

            // Act
            var attributes = controller.GetType().GetMethod("Put").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var response = await controller.Put(new ApplyClientVisitsModel());
            var okResult = response as OkObjectResult;

            // Assert
            Assert.True(attributes.Any());
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SS_ApplyClientVisits);
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Update);

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as ApplyClientVisitsModel).EmployeeId);
        }

		[Fact]
		public async Task Apply_Client_Visits_Put_BadRequest()
		{
			// Arrange  
			TranSmart.Core.Result.Result<ApplyClientVisits> result = new();
			_applyclientVisitService.Setup(x => x.UpdateAsync(It.IsAny<ApplyClientVisits>())).Callback((ApplyClientVisits applyClientVisits) =>
			{
				result.IsSuccess = false;
				result.ReturnValue = applyClientVisits;

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("Put").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.Put(new ApplyClientVisitsModel());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.True(attributes.Any());
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SS_ApplyClientVisits);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Update);

			Assert.Null(okResult);
		}

		[Theory]
        [InlineData(1)]
        public async Task Apply_Client_Visits_Paginate_Test(int count)
        {
            // Arrange  
            var list = new List<ApplyClientVisits> 
            {
                new ApplyClientVisits
                { 
                    EmployeeId = EmployeeId
                },
                new ApplyClientVisits 
                { 
                    EmployeeId = Guid.NewGuid() 
                }
            }.AsQueryable();
           
            TranSmart.Core.Result.Result<ApplyClientVisits> result = new();
            _applyclientVisitService.Setup(x => x.GetPaginate(It.IsAny<ApplyClientVisitSearch>())).ReturnsAsync(list.ToPaginate(0, 10));

            var controller = _controller;

            // Act
            var attributes = controller.GetType().GetMethod("Paginate").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var response = await controller.Paginate(new ApplyClientVisitSearch());
            var okResult = response as OkObjectResult;

            //assert
            Assert.True(attributes.Any());
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SS_ApplyClientVisits);
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(count, ((TranSmart.API.Models.Paginate<ApplyClientVisitsList>)okResult.Value).Items.Where(x => x.EmployeeId == EmployeeId).Count());

        }

		[Fact]
		public async Task ApplyClientVisits_Search_GetValidRecords()
		{
			// Arrange  
			Result<ApplyClientVisits> result = new();
			_applyclientVisitService.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(result.ReturnValue = new ApplyClientVisits {EmployeeId = EmployeeId }));

			var controller = _controller;

			// Act
			var response = await controller.Search("HYD");
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);

		}
	}
}
