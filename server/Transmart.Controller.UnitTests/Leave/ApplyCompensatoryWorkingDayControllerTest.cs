using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Leave;
using TranSmart.API.Extensions;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave;
using TranSmart.Service.Leave;
using Xunit;

namespace Transmart.Controller.UnitTests.Leave
{
    public class ApplyCompensatoryWorkingDayControllerTest : ControllerTestBase
    {
        private Mock<IApplyCompensatoryWorkingDayService> _applyCompensatoryWorkingDayService;
        private readonly ApplyCompensatoryWorkingDayController _controller;

        public ApplyCompensatoryWorkingDayControllerTest() : base()
        {
            _applyCompensatoryWorkingDayService = new Mock<IApplyCompensatoryWorkingDayService>();
            _controller = new ApplyCompensatoryWorkingDayController(Mapper, _applyCompensatoryWorkingDayService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = Claim }
                }
            };
        }

        [Fact]
        public async Task Apply_Compensatory_Working_Day_Post_Test()
        {
            // Arrange  
            TranSmart.Core.Result.Result<ApplyCompo> result = new();
            _applyCompensatoryWorkingDayService.Setup(x => x.AddAsync(It.IsAny<ApplyCompo>())).Callback((ApplyCompo applyCompo) =>
            {
                result.IsSuccess = true;
                result.ReturnValue = applyCompo;

            }).ReturnsAsync(result);

            var controller = _controller;

            // Act
            var attributes = controller.GetType().GetMethod("Post").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var response = await controller.Post(new ApplyCompensatoryWorkingDayModel());
            var okResult = response as OkObjectResult;

            //assert
            Assert.True(attributes.Any());
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SS_ApplyCompensatoryWorkingDay);
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Create);

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as ApplyCompensatoryWorkingDayModel).EmployeeId);
        }

        [Fact]
        public async Task Apply_Compensatory_Working_Day_Put_Test()
        {
            // Arrange  
            TranSmart.Core.Result.Result<ApplyCompo> result = new();
            _applyCompensatoryWorkingDayService.Setup(x => x.UpdateAsync(It.IsAny<ApplyCompo>())).Callback((ApplyCompo applyCompo) =>
            {
                result.IsSuccess = true;
                result.ReturnValue = applyCompo;

            }).ReturnsAsync(result);

            var controller = _controller;
            
            // Act
            var attributes = controller.GetType().GetMethod("Put").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var response = await controller.Put(new ApplyCompensatoryWorkingDayModel());
            var okResult = response as OkObjectResult;

            //assert
            Assert.True(attributes.Any());
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SS_ApplyCompensatoryWorkingDay);
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Update);

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as ApplyCompensatoryWorkingDayModel).EmployeeId);
        }

        [Theory]
        [InlineData(1)]
        public async Task Apply_Compensatory_Working_Day_Self_Test(int count)
        {
            // Arrange  
            var list = new List<ApplyCompo>
            {
                new ApplyCompo
                {
                    EmployeeId = EmployeeId
                },
                new ApplyCompo
                {
                    EmployeeId = Guid.NewGuid()
                }
            }.AsQueryable();

            TranSmart.Core.Result.Result<IPaginate<ApplyCompo>> result = new();
            _applyCompensatoryWorkingDayService.Setup(x => x.SelfServiceSearch(It.IsAny<BaseSearch>())).ReturnsAsync(list.ToPaginate(0, 10));

            var controller = _controller;

            // Act
            var attributes = controller.GetType().GetMethod("Paginate").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var response = await controller.SelfServiceSearch(new BaseSearch());
            var okResult = response as OkObjectResult;

            //assert
            Assert.True(attributes.Any());
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SS_ApplyCompensatoryWorkingDay);
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(count, ((TranSmart.API.Models.Paginate<ApplyCompensatoryWorkingDayList>)okResult.Value).Items.Where(x => x.EmployeeId == EmployeeId).Count());

        }
    }
}
