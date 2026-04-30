using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Leave.Approval;
using TranSmart.API.Extensions;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models.Leave.Approval;
using TranSmart.Domain.Models.Leave.Model;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Domain.Models.SelfService;
using TranSmart.Service;
using TranSmart.Service.Leave.Approval;
using Xunit;

namespace Transmart.Controller.UnitTests.Leave.Approve
{
	public class ClientVisitControllerTest : ControllerTestBase
	{
		private Mock<IApprovalClientVisitsService> _clientvisitsservice;
		private Mock<ISequenceNoService> _sequnce;
		private readonly ClientVisitController _controller;

		public ClientVisitControllerTest() : base()
		{
			_sequnce = new Mock<ISequenceNoService>();
			_clientvisitsservice = new Mock<IApprovalClientVisitsService>();
			_controller = new ClientVisitController(Mapper, _clientvisitsservice.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};

		}

		[Fact]
		public async Task ClientVisit_Paginate_Test()
		{
			// Arrange  
			 
			var list = new List<ApplyClientVisits> {
				new ApplyClientVisits { EmployeeId = EmployeeId },
				new ApplyClientVisits { EmployeeId = Guid.NewGuid() } };
			_clientvisitsservice.Setup(x => x.ClientVisit(It.IsAny<ApplyClientVisitSearch>()))
				.ReturnsAsync((ApplyClientVisitSearch search) =>
				{
					return list.Where(x => x.EmployeeId == search.RefId).AsQueryable().ToPaginate(0, 10);
				});


			// Act
			var attributes = _controller.GetType().GetMethod("Approval").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);

			var resposne = await _controller.Approval(new ApplyClientVisitSearch());
			var okResult = resposne as OkObjectResult;

			// Assert
			Assert.True(attributes.Any());
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SA_ClientVisitApplications);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.True(((TranSmart.API.Models.Paginate<ApplyClientVisitsList>)okResult.Value)
				.Items.Count(x => x.EmployeeId == EmployeeId) == 1);
		}


		[Fact]
		public async Task ClientVisit_GetId_Test()
		{
			// Arrange  
			Result<ApplyClientVisits> result = new();
			_clientvisitsservice.Setup(x => x.GetClientVisit(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(result.ReturnValue = new ApplyClientVisits { EmployeeId = EmployeeId }));

			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("Get").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var resposne = controller.Get(Guid.NewGuid());
			var okResult = await resposne as OkObjectResult;

			// Assert
			Assert.True(attributes.Any());
			Assert.Equal((int)TranSmart.Core.Permission.SA_ClientVisitApplications, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Read, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as ClientVisitInfoModel).EmployeeId);
		}

		[Fact]
		public async Task ClientVisit_Put_Test()
		{
			// Arrange  
			var id= Guid.NewGuid();
			Result<ApplyClientVisits> result = new();
			_clientvisitsservice.Setup(x => x.Approve(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>()))
				.Callback((Guid id, bool isApproved, string RejectReason) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new ApplyClientVisits { ID=id , EmployeeId=EmployeeId,};

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("Put").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);

			var response = await controller.ApprovalPut(new ApproveRequest { ID= id, IsApproved=true, RejectReason="ds"});
			var okResult = response as OkObjectResult;

			// Assert
			Assert.True(attributes.Any());
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SA_ClientVisitApplications);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as ApplyClientVisitsModel).EmployeeId);
		}
		[Fact]
		public async Task ClientVisit_Put_BadRequest()
		{
			// Arrange  
			var id = Guid.NewGuid();
			Result<ApplyClientVisits> result = new();
			_clientvisitsservice.Setup(x => x.Approve(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>()))
				.Callback((Guid id, bool isApproved, string RejectReason) =>
				{
					result.IsSuccess = false;
					result.ReturnValue = new ApplyClientVisits { ID = id, EmployeeId = EmployeeId, };

				}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("Put").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);

			var response = await controller.ApprovalPut(new ApproveRequest { ID = id, IsApproved = true, RejectReason = "ds" });
			var okResult = response as OkObjectResult;

			// Assert
			Assert.True(attributes.Any());
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SA_ClientVisitApplications);
			Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

			Assert.Null(okResult);
		}


	}
}
