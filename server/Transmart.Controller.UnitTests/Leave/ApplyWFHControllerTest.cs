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
using TranSmart.Domain.Models.Leave;
using TranSmart.Domain.Models.Leave.Approval;
using TranSmart.Domain.Models.Leave.Model;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Service;
using TranSmart.Service.Leave;
using TranSmart.Service.Schedules;
using Xunit;

namespace Transmart.Controller.UnitTests.Leave
{
    public class ApplyWFHControllerTest : ControllerTestBase
    {

        private readonly Mock<IApplyWfhService> _applyWFHService;
        private readonly ApplyWfhController _controller;

        public ApplyWFHControllerTest() : base()
        {
            _applyWFHService = new Mock<IApplyWfhService>();

            _controller = new ApplyWfhController(Mapper, _applyWFHService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = Claim }
                }
            };
        }

		[Fact]
		public async Task ApplyWFH_GetList_GetValidRecords()
		{
			// Arrange  
			var id = Guid.NewGuid();

			Result<ApplyWfh> result = new();
			_applyWFHService.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(result.ReturnValue = new ApplyWfh { ID = id, EmployeeId = EmployeeId}));

			var controller = _controller;

			// Act
			var response = await controller.GetList("employee");
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}

		[Theory]
		[InlineData(1)]
		public async Task ApplyWFH_Paginate_Test(int count)
		{
			// Arrange  
			var list = new List<ApplyWfh>
			{
				new ApplyWfh
				{
					EmployeeId = EmployeeId
				},
				new ApplyWfh
				{
					EmployeeId = Guid.NewGuid()
				}
			}.AsQueryable();

			_applyWFHService.Setup(x => x.GetPaginate(It.IsAny<ApplyWfhSearch>())).ReturnsAsync(list.ToPaginate(0, 10));

			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("Paginate").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.Paginate(new ApplyWfhSearch());
			var okResult = response as OkObjectResult;

			//assert
			//assert
			Assert.True(attributes.Any());
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.LM_WFH);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(count, ((TranSmart.API.Models.Paginate<ApplyWfhList>)okResult.Value).Items.Where(x => x.EmployeeId == EmployeeId).Count());

		}

		[Fact]
		public async Task ApplyWFH_Get_GetValidRecords()
		{
			// Arrange  
			var id = Guid.NewGuid();

			Result<ApplyWfh> result = new();
			_applyWFHService.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(result.ReturnValue = new ApplyWfh { ID = id, EmployeeId = EmployeeId }));

			var controller = _controller;

			// Act
			var response = await controller.Get(id);
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, ((ApplyWfhModel)okResult.Value).EmployeeId);

		}

		[Fact]
		public async Task ApplyWFH_GEtPastFutureWFH_GetValidRecords()
		{
			// Arrange  
			var list = new List<ApplyWfh>
			{
				new ApplyWfh
				{
					EmployeeId = EmployeeId,
					FromDateC=DateTime.Now,
					ToDateC=DateTime.Now,
				},
				new ApplyWfh
				{
					EmployeeId = Guid.NewGuid(),
					FromDateC=DateTime.Now,
					ToDateC=DateTime.Now,
				}
			}.AsQueryable();

			Result<ApplyWfh> result = new();
			_applyWFHService.Setup(x => x.GetPastFutureWFH(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(list);

			var controller = _controller;

			// Act
			var response = await controller.GEtPastFutureWFH(EmployeeId,DateTime.Now,DateTime.Now);
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(2, ((IEnumerable<ApplyWfhModel>)okResult.Value).Count());
		}

		[Fact]
        public async Task Apply_WFH_Post_Test()
        {
            // Arrange  
            TranSmart.Core.Result.Result<ApplyWfh> result = new();
            _applyWFHService.Setup(x => x.AddAsync(It.IsAny<ApplyWfh>())).Callback((ApplyWfh applyWFH) =>
            {
                result.IsSuccess = true;
                result.ReturnValue = applyWFH;

            }).ReturnsAsync(result);

            var controller = _controller;

            // Act
            var attributes = controller.GetType().GetMethod("Post").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var response = await controller.Post(new ApplyWfhModel());
            var okResult = response as OkObjectResult;

            //assert
            Assert.True(attributes.Any());
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SS_ApplyWFH);
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Create);

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as ApplyWfhModel).EmployeeId);
        }
		[Fact]
		public async Task Apply_WFH_Post_BadRequest()
		{
			// Arrange  
			TranSmart.Core.Result.Result<ApplyWfh> result = new();
			_applyWFHService.Setup(x => x.AddAsync(It.IsAny<ApplyWfh>())).Callback((ApplyWfh applyWFH) =>
			{
				result.IsSuccess = false;
				result.ReturnValue = applyWFH;

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("Post").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.Post(new ApplyWfhModel());
			var okResult = response as OkObjectResult;

			//assert
			Assert.True(attributes.Any());
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SS_ApplyWFH);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Create);

			Assert.Null(okResult);

		}

		[Fact]
		public async Task ApplyWFH_PostLM_Test()
		{
			// Arrange  
			Result<ApplyWfh> result = new();
			_applyWFHService.Setup(x => x.AddAsync(It.IsAny<ApplyWfh>())).Callback((ApplyWfh applyWFH) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new ApplyWfh { EmployeeId = EmployeeId };

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var response = await controller.PostLM(new ApplyWfhModel());
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as ApplyWfhModel).EmployeeId);
		}

		[Fact]
		public async Task ApplyWFH_PostLM_BadRequest()
		{
			// Arrange  
			Result<ApplyWfh> result = new();
			_applyWFHService.Setup(x => x.AddAsync(It.IsAny<ApplyWfh>())).Callback((ApplyWfh applyWFH) =>
			{
				result.IsSuccess = false;
				result.ReturnValue = new ApplyWfh { EmployeeId = EmployeeId };

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var response = await controller.PostLM(new ApplyWfhModel());
			var okResult = response as OkObjectResult;

			//assert
			Assert.Null(okResult);
		}

		[Fact]
        public async Task Apply_WFH_Put_Test()
        {
            // Arrange

            TranSmart.Core.Result.Result<ApplyWfh> result = new();
            _applyWFHService.Setup(x => x.UpdateAsync(It.IsAny<ApplyWfh>())).Callback((ApplyWfh applyWFH) =>
            {
                result.IsSuccess = true;
                result.ReturnValue = applyWFH;

            }).ReturnsAsync(result);

            var controller = _controller;

            // Act
            var attributes = controller.GetType().GetMethod("Put").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var response = await controller.Put(new ApplyWfhModel());
            var okResult = response as OkObjectResult;

            //assert
            Assert.True(attributes.Any());
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SS_ApplyWFH);
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Update);

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as ApplyWfhModel).EmployeeId);
        }

		[Fact]
		public async Task Apply_WFH_Put_BadRequest()
		{
			// Arrange

			TranSmart.Core.Result.Result<ApplyWfh> result = new();
			_applyWFHService.Setup(x => x.UpdateAsync(It.IsAny<ApplyWfh>())).Callback((ApplyWfh applyWFH) =>
			{
				result.IsSuccess = false;
				result.ReturnValue = applyWFH;

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("Put").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.Put(new ApplyWfhModel());
			var okResult = response as OkObjectResult;

			//assert
			Assert.True(attributes.Any());
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SS_ApplyWFH);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Update);

			Assert.Null(okResult);
		}

		[Fact]
		public async Task ApplyWFH_RejectWfhAfterApprove_Test()
		{
			// Arrange

			Result<ApplyWfh> result = new();
			_applyWFHService.Setup(x => x.RejectAfterApprove(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>())).Callback((Guid id, string rejectReason, Guid rejectedEmpId) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new ApplyWfh { EmployeeId = EmployeeId };

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var response = await controller.RejectWfhAfterApprove(new ApproveRequest());
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as ApplyWfhModel).EmployeeId);
		}


		[Fact]
        public async Task Apply_WFH_Cancel_Test()
        {
            // Arrange  
            TranSmart.Core.Result.Result<ApplyWfh> result = new();
            _applyWFHService.Setup(x => x.Cancel(It.IsAny<Guid>(), It.IsAny<Guid>())).Callback((Guid id, Guid employeeId) =>
            {
                result.IsSuccess = true;
                result.ReturnValue = new ApplyWfh { EmployeeId = EmployeeId };

            }).ReturnsAsync(result);

            var controller = _controller;

            // Act
            var response = await controller.Cancel(Guid.NewGuid());
            var okResult = response as OkObjectResult;

            //assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as ApplyWfhModel).EmployeeId);
        }

		[Fact]
		public async Task Apply_WFH_Cancel_BadRequest()
		{
			// Arrange  
			TranSmart.Core.Result.Result<ApplyWfh> result = new();
			_applyWFHService.Setup(x => x.Cancel(It.IsAny<Guid>(), It.IsAny<Guid>())).Callback((Guid id, Guid employeeId) =>
			{
				result.IsSuccess = false;
				result.ReturnValue = new ApplyWfh { EmployeeId = EmployeeId };

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var response = await controller.Cancel(Guid.NewGuid());
			var okResult = response as OkObjectResult;

			//assert
			Assert.Null(okResult);
		}

		[Fact]
        public async Task Apply_WFH_Get_Approval_By_Id_Test()
        {
            // Arrange  
            TranSmart.Core.Result.Result<ApplyWfh> result = new();
            _applyWFHService.Setup(x => x.GetLeave(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(result.ReturnValue = new ApplyWfh { EmployeeId = EmployeeId }));


            var controller = _controller;

            // Act
            var attributes = controller.GetType().GetMethod("GetApprovalById").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var response = await controller.GetApprovalById(Guid.NewGuid());
            var okResult = response as OkObjectResult;

            //assert
            Assert.True(attributes.Any());
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SA_WFHApplicationApprove);
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as ApplyWfhModel).EmployeeId);
        }

        [Theory]
        [InlineData(2, true)]//approve
        [InlineData(3, false)]//reject 
        public async Task Apply_WFH_Approval_Put_Test(byte status, bool isApproved)
        {
            //Arrange
            TranSmart.Core.Result.Result<ApplyWfh> result = new();

            _applyWFHService.Setup(x => x.Approve(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>()))
                   .Callback((Guid Id, Guid approvedEmpId, bool isAdminRequest) =>
                   {
                       result.IsSuccess = true;
                       result.ReturnValue = new ApplyWfh { ApprovedById = EmployeeId, Status = status };
                   }).ReturnsAsync(result);

            _applyWFHService.Setup(x => x.Reject(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>()))
                   .Callback((Guid Id, string rejectReson, Guid rejectedEmpId) =>
                   {
                       result.IsSuccess = true;
                       result.ReturnValue = new ApplyWfh { AdminReason = rejectReson, Status = status, EmployeeId = rejectedEmpId, ApprovedById = EmployeeId };
                   }).ReturnsAsync(result);

            var controller = _controller;

            // Act
            var attributes = controller.GetType().GetMethod("ApprovalPut").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var response = await controller.ApprovalPut(new ApproveRequest { IsApproved = isApproved });
            var okResult = response as OkObjectResult;

            //assert
            Assert.True(attributes.Any());
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SA_WFHApplicationApprove);
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Update);

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as ApplyWfhModel).ApprovedById);
            Assert.Equal(status, (okResult.Value as ApplyWfhModel).Status);
        }

        [Theory]
        [InlineData(2, true)]//approve
        [InlineData(3, false)]//reject 
        public async Task Apply_WFH_Leave_Put_Test(byte status, bool isApproved)
        {
            //Arrange
            TranSmart.Core.Result.Result<ApplyWfh> result = new();

            _applyWFHService.Setup(x => x.Approve(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>()))
                   .Callback((Guid Id, Guid approvedEmpId, bool isAdminRequest) =>
                   {
                       result.IsSuccess = true;
                       result.ReturnValue = new ApplyWfh { ApprovedById = EmployeeId, Status = status };
                   }).ReturnsAsync(result);

            _applyWFHService.Setup(x => x.Reject(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>()))
                   .Callback((Guid Id, string rejectReson, Guid rejectedEmpId) =>
                   {
                       result.IsSuccess = true;
                       result.ReturnValue = new ApplyWfh { AdminReason = rejectReson, Status = status, EmployeeId = rejectedEmpId, ApprovedById = EmployeeId };
                   }).ReturnsAsync(result);

            var controller = _controller;

            // Act
			var response = await controller.LeavePut(new ApproveRequest { IsApproved = isApproved });
            var okResult = response as OkObjectResult;

            //assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as ApplyWfhModel).ApprovedById);
            Assert.Equal(status, (okResult.Value as ApplyWfhModel).Status);
        }

        [Theory]
        [InlineData(1)]
        public async Task Apply_WFH_Get_Approval_Test(int count)
        {
            // Arrange
            var list = new List<ApplyWfh>
            {
                new ApplyWfh
                {
                    EmployeeId = EmployeeId
                },
                new ApplyWfh
                {
                    EmployeeId = Guid.NewGuid()
                }
            }.AsQueryable();

            TranSmart.Core.Result.Result<ApplyWfh> result = new();
            _applyWFHService.Setup(x => x.GetPaginate(It.IsAny<ApplyWfhSearch>())).ReturnsAsync(list.ToPaginate(0, 10));

            var controller = _controller;

            // Act
            var attributes = controller.GetType().GetMethod("Approvals").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var response = await controller.Approvals(new ApplyWfhSearch());
            var okResult = response as OkObjectResult;

            //assert
            Assert.True(attributes.Any());
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SA_WFHApplicationApprove);
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(count, ((TranSmart.API.Models.Paginate<ApplyWfhList>)okResult.Value).Items.Where(x => x.EmployeeId == EmployeeId).Count());


        }

        [Theory]
        [InlineData(1)]
        public async Task Apply_WFH_Self_Service_Search_Test(int count)
        {
            // Arrange
            var list = new List<ApplyWfh>
            {
                new ApplyWfh
                {
                    EmployeeId = EmployeeId
                },
                new ApplyWfh
                {
                    EmployeeId = Guid.NewGuid()
                }
            }.AsQueryable();

            TranSmart.Core.Result.Result<ApplyWfhSearch> result = new();
            _applyWFHService.Setup(x => x.SelfServiceSearch(It.IsAny<ApplyWfhSearch>())).ReturnsAsync(list.ToPaginate(0, 10));

            var controller = _controller;

            // Act
            var attributes = controller.GetType().GetMethod("Paginate").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var response = await controller.SelfServiceSearch(new ApplyWfhSearch());
            var okResult = response as OkObjectResult;

            //assert
            Assert.True(attributes.Any());
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.LM_WFH);
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(count, ((TranSmart.API.Models.Paginate<ApplyWfhList>)okResult.Value).Items.Where(x => x.EmployeeId == EmployeeId).Count());


        }
		[Fact]
		public async Task ApplyWFH_Search_GetValidRecords()
		{
			// Arrange  
			Result<ApplyWfh> result = new();
			_applyWFHService.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(result.ReturnValue = new ApplyWfh { EmployeeId = EmployeeId }));

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
