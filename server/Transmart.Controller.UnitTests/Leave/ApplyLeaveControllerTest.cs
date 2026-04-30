using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Leave;
using TranSmart.API.Extensions;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave;
using TranSmart.Domain.Models.Leave.Approval;
using TranSmart.Domain.Models.Leave.List;
using TranSmart.Domain.Models.Leave.Model;
using TranSmart.Domain.Models.Leave.Request;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Domain.Models.SelfService;
using TranSmart.Service.Leave;
using Xunit;

namespace Transmart.Controller.UnitTests.Leave
{
	public class ApplyLeaveControllerTest : ControllerTestBase
	{
		private readonly Mock<IApplyLeaveService> _applyLeaveService;
		private readonly ApplyLeaveController _controller;
		public ApplyLeaveControllerTest() : base()
		{
			_applyLeaveService = new Mock<IApplyLeaveService>();
			_controller = new ApplyLeaveController(Mapper, _applyLeaveService.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};
		}
		[Fact]
		public async Task SelfService_Get_Test()
		{
			// Arrange   
			var id = Guid.NewGuid();
			_applyLeaveService.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new ApplyLeave { EmployeeId = EmployeeId, ID = id });

			// Act
			var attributes = _controller.GetType().GetMethod("Get").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await _controller.Get(id);
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Equal((int)TranSmart.Core.Permission.SS_ApplyLeaves, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Read, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}
		[Fact]
		public async Task UserApplyLeaveCheckEmployeeTest()
		{
			// Arrange  
			TranSmart.Core.Result.Result<ApplyLeave> result = new();
			_applyLeaveService.Setup(x => x.MaximumLeavesValidation(It.IsAny<ApplyLeave>())).Callback((ApplyLeave leave) =>
			{
				result.IsSuccess = true;
			}).ReturnsAsync(result);
			_applyLeaveService.Setup(x => x.AddAsync(It.IsAny<ApplyLeave>())).Callback((ApplyLeave applyLeave) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = applyLeave;

			}).ReturnsAsync(result);

			var controller = _controller;
			// Act
			var attributes = controller.GetType().GetMethod("SelfServicePost").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.SelfServicePost(new ApplyLeaveRequest());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Equal((int)TranSmart.Core.Permission.SS_ApplyLeaves, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Create, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as ApplyLeaveModel).EmployeeId);
		}
		[Fact]
		public async Task Self_Service_User_Update_Leave_Check_Employee_Test()
		{
			// Arrange  
			TranSmart.Core.Result.Result<ApplyLeave> result = new();
			_applyLeaveService.Setup(x => x.MaximumLeavesValidation(It.IsAny<ApplyLeave>())).Callback((ApplyLeave leave) =>
			{
				result.IsSuccess = true;
			}).ReturnsAsync(result);
			_applyLeaveService.Setup(x => x.UpdateAsync(It.IsAny<ApplyLeave>())).Callback((ApplyLeave applyLeave) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = applyLeave;

			}).ReturnsAsync(result);

			// Act
			var attributes = _controller.GetType().GetMethod("SelfServicePut").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await _controller.SelfServicePut(new ApplyLeaveRequest());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Equal((int)TranSmart.Core.Permission.SS_ApplyLeaves, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Update, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as ApplyLeaveModel).EmployeeId);
		}
		[Fact]
		public async Task LeaveManagement_Paginate_Test()
		{
			// Arrange   
			var list = new List<ApplyLeave> { new ApplyLeave { EmployeeId = EmployeeId }, new ApplyLeave { EmployeeId = Guid.NewGuid() } }.AsQueryable();
			_applyLeaveService.Setup(x => x.ApprovalPaginate(It.IsAny<ApplyLeaveSearch>())).ReturnsAsync(list.ToPaginate(0, 10));

			// Act
			var attributes = _controller.GetType().GetMethod("LMApprovalPaginate").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await _controller.LMApprovalPaginate(new ApplyLeaveSearch());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Equal((int)TranSmart.Core.Permission.LM_Leaves, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Read, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}
		[Fact]
		public async Task Leave_Management_Apply_Leave_Post_Test()
		{
			// Arrange 
			TranSmart.Core.Result.Result<ApplyLeave> result = new();

			_applyLeaveService.Setup(x => x.AddApprovedLeaveAsync(It.IsAny<ApplyLeave>())).Callback((ApplyLeave applyLeave) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = applyLeave;
				result.ReturnValue.EmployeeId = EmployeeId;
			}).ReturnsAsync(result);

			// Act
			var attributes = _controller.GetType().GetMethod("PostLeave").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await _controller.PostLeave(new ApplyLeaveRequest());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Equal((int)TranSmart.Core.Permission.LM_Leaves, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Create, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as ApplyLeaveModel).EmployeeId);
		}
		[Fact]
		public async Task Leave_Management_Apply_Leave_Post_BadRequest()
		{
			// Arrange 
			TranSmart.Core.Result.Result<ApplyLeave> result = new();

			_applyLeaveService.Setup(x => x.AddApprovedLeaveAsync(It.IsAny<ApplyLeave>())).Callback((ApplyLeave applyLeave) =>
			{
				result.IsSuccess = false;
			}).ReturnsAsync(result);

			// Act
			var attributes = _controller.GetType().GetMethod("PostLeave").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await _controller.PostLeave(new ApplyLeaveRequest());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Equal((int)TranSmart.Core.Permission.LM_Leaves, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Create, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);

			Assert.Null(okResult);
		}
		[Theory]
		[InlineData(2, true)]//approve
		[InlineData(3, false)]//reject 
		public async Task Leave_Management_Apply_Leave_Update_Test(byte status, bool isApproved)
		{
			//Arrange
			TranSmart.Core.Result.Result<ApplyLeave> result = new();

			_applyLeaveService.Setup(x => x.Approve(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>()))
				   .Callback((Guid Id, Guid approvedEmpId, bool isAdminRequest) =>
			 {
				 result.IsSuccess = true;
				 result.ReturnValue = new ApplyLeave { ApprovedById = EmployeeId, Status = status };
			 }).ReturnsAsync(result);
			_applyLeaveService.Setup(x => x.Reject(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>()))
				.Callback((Guid Id, string rejectReason, Guid rejectedEmpId) =>
				{
					result.IsSuccess = true;
					result.ReturnValue = new ApplyLeave { ApprovedById = EmployeeId, Status = status };
				}).ReturnsAsync(result);
			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("LeavePut").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.LeavePut(new ApproveRequest { IsApproved = isApproved });
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Equal((int)TranSmart.Core.Permission.LM_Leaves, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Update, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as ApplyLeavesModel).ApprovedById);
			Assert.Equal(status, (okResult.Value as ApplyLeavesModel).Status);
		}
		[Fact]
		public async Task Self_Service_Cancel_Leave_Test()
		{
			// Arrange  
			TranSmart.Core.Result.Result<ApplyLeave> result = new();
			_applyLeaveService.Setup(x => x.CancelAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
				.Callback((Guid id, Guid employeeId) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new ApplyLeave { EmployeeId = EmployeeId };

			}).ReturnsAsync(result);

			// Act
			var attributes = _controller.GetType().GetMethod("Cancel").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await _controller.Cancel(Guid.NewGuid());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Equal((int)TranSmart.Core.Permission.SS_ApplyLeaves, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Update, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as ApplyLeaveModel).EmployeeId);
		}

		[Fact]
		public async Task Self_Service_Cancel_BadRequest_Test()
		{
			// Arrange  
			TranSmart.Core.Result.Result<ApplyLeave> result = new();
			_applyLeaveService.Setup(x => x.CancelAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
				.Callback((Guid id, Guid employeeId) =>
				{
					result.IsSuccess = false;

				}).ReturnsAsync(result);

			// Act
			var attributes = _controller.GetType().GetMethod("Cancel").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await _controller.Cancel(Guid.NewGuid());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Equal((int)TranSmart.Core.Permission.SS_ApplyLeaves, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Update, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);

			Assert.Null(okResult);

		}
		[Fact]
		public async Task RejectLeaveAfterApprove_Leave_Test()
		{
			//Arrange
			var result = new TranSmart.Core.Result.Result<ApplyLeave>();
			_applyLeaveService.Setup(x => x.RejectAfterApprove(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>()))
				.Callback((Guid id, string rejectReason, Guid rejectedEmpId) =>
				{
					result.IsSuccess = true;
					result.ReturnValue = new ApplyLeave
					{
						EmployeeId = EmployeeId,
						Status = (int)ApplyLeaveSts.Rejected,
						RejectReason = "Production issue",
						ApprovedById = Guid.NewGuid()
					};
				}).ReturnsAsync(result);

			// Act
			var response = await _controller.RejectLeaveAfterApprove(new ApproveRequest());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal((int)ApplyLeaveSts.Rejected, (okResult.Value as ApplyLeavesModel).Status);
		}

		[Fact]
		public async Task Self_Service_List_Test()
		{
			// Arrange   
			var list = new List<ApplyLeave> { new ApplyLeave { EmployeeId = EmployeeId }, new ApplyLeave { EmployeeId = Guid.NewGuid() } }.AsQueryable();
			_applyLeaveService.Setup(x => x.SelfServiceSearch(It.IsAny<ApplyLeaveSearch>())).ReturnsAsync(list.ToPaginate(0, 10));

			// Act
			var attributes = _controller.GetType().GetMethod("SelfServicePaginate").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await _controller.SelfServicePaginate(new ApplyLeaveSearch());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Equal((int)TranSmart.Core.Permission.SS_ApplyLeaves, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Read, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(1, ((TranSmart.API.Models.Paginate<ApplyLeaveList>)okResult.Value).Items.Count(x => x.EmployeeId == EmployeeId));
		}
		[Fact]
		public async Task GetLeavesBetweenTwoDates_Test()
		{
			//Arrange
			var list = new List<ApplyLeave> { new ApplyLeave { EmployeeId = EmployeeId }, new ApplyLeave { EmployeeId = Guid.NewGuid() } };
			_applyLeaveService.Setup(x => x.GetLeavesBetweenTwoDates(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
				.ReturnsAsync(list);

			var response = await _controller.GetLeavesBetweenTwoDates(DateTime.Now.AddDays(-1), DateTime.Now.Date);
			var okResult = response as OkObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(1, ((List<LeaveInfoModel>)okResult.Value).Count(x => x.EmployeeId == EmployeeId));
		}
		[Fact]
		public async Task GetMonthLeaves()
		{
			//Arrange
			var month = DateTime.Now.Month;
			var leavesList = new List<ApplyLeave> {
				new ApplyLeave
				{
					EmployeeId = EmployeeId, FromDate = DateTime.Now,
					ToDate = DateTime.Now.AddDays(1),
					Status = (int)ApplyLeaveSts.Approved
				},
				new ApplyLeave
				{
					EmployeeId = Guid.NewGuid(),
					FromDate = DateTime.Now.AddDays(-35),
					ToDate= DateTime.Now.AddDays(-34),
					Status = (int)ApplyLeaveSts.Cancelled
				}
			};
			_applyLeaveService.Setup(x => x.GetLeaves(It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(leavesList);
			var response = await _controller.GetMonthLeaves(month);
			var okResult = response as OkObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(1, ((List<LeaveInfoModel>)okResult.Value).Count(x => x.FromDate.Month == month));
		}
		[Fact]
		public async Task GetPastFutureWeekLeaves_Test()
		{
			var fromDate = DateTime.Today; var toDate = DateTime.Today.AddDays(1);
			var leavesList = new List<ApplyLeave> { new ApplyLeave
			{
				EmployeeId = EmployeeId,
				FromDate = DateTime.Now.AddDays(-2),ToDate = DateTime.Now.AddDays(2)
			},
			new ApplyLeave
			{
				EmployeeId = EmployeeId,
				FromDate= DateTime.Now.AddDays(-10), ToDate= DateTime.Now.AddDays(-9)
			},
			new ApplyLeave
			{
				EmployeeId = EmployeeId,
				FromDate= DateTime.Now.AddDays(-4), ToDate= DateTime.Now.AddDays(4)
			}};
			_applyLeaveService.Setup(x => x.GetWeekLeaves(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(leavesList);
			var response = await _controller.GetPastFutureWeekLeaves(EmployeeId, fromDate, toDate);
			var okResult = response as OkObjectResult;
			Assert.NotNull(okResult);
			Assert.Equal(2, ((List<ApplyLeaveModel>)okResult.Value).Count(x => x.FromDate >= fromDate.AddDays(-7) && x.ToDate <= toDate.AddDays(7)));

		}
		[Fact]
		public async Task GetLeave_Test()
		{
			//Arrange
			var applyLeaveId = Guid.NewGuid();
			var list = new List<ApplyLeave> {
				new ApplyLeave
				{
					ID= applyLeaveId,
					EmployeeId = EmployeeId,
					FromDate= DateTime.Now,
					ToDate= DateTime.Now.AddDays(1)
				},
				new ApplyLeave
				{
					ID= Guid.NewGuid(),
					EmployeeId = EmployeeId,
					FromDate= DateTime.Now.AddDays(-1),
					ToDate= DateTime.Now
				} };
			_applyLeaveService.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(list.FirstOrDefault());
			var response = await _controller.GetLeave(EmployeeId);
			var okResult = response as OkObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(EmployeeId, (okResult.Value as ApplyLeaveModel).EmployeeId);
		}
		[Fact]
		public async Task GetEmployeeLeaveTypes()
		{
			var leaveTypeId = Guid.NewGuid();
			var list = new List<LeaveBalanceModel>
			{
				new LeaveBalanceModel
				{
					LeaveTypeId = leaveTypeId,
					LeaveTypeName = "Casual Leave",
					EmployeeId = EmployeeId,
					Leaves = 2
				}
			};
			_applyLeaveService.Setup(x => x.GetEmployeeLeaveTypes(It.IsAny<Guid>()))
				.ReturnsAsync(list);
			//Act
			var response = await _controller.GetEmployeeLeaveTypes();
			var okResult = response as OkObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}
		[Fact]
		public async Task Get_LeaveBalance_By_LeaveType()  //This test not required
		{
			// Arrange   
			_applyLeaveService.Setup(x => x.GetLeaveBalanceByLeaveType(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateTime>(),It.IsAny<DateTime>())).ReturnsAsync(2);

			var controller = _controller;
			// Act
			var attributes = controller.GetType().GetMethod("GetLeaveBalanceByLeaveType").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);

			var response = await controller.GetLeaveBalanceByLeaveType(Guid.NewGuid(),DateTime.Now.Date,DateTime.Now.Date);
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Equal((int)TranSmart.Core.Permission.SS_ApplyLeaves, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Read, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}

		[Fact]
		public async Task Get_Leave_By_Approval_EmployeeID_Test()
		{
			// Arrange  
			TranSmart.Core.Result.Result<ApplyLeave> result = new();
			_applyLeaveService.Setup(x => x.GetLeave(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(new ApplyLeave { EmployeeId = EmployeeId }))
			   .Callback((Guid id, Guid employeeId) =>
			   {
				   result.IsSuccess = true;
				   result.ReturnValue = new ApplyLeave { EmployeeId = EmployeeId };

			   });

			var controller = _controller;
			// Act
			var attributes = controller.GetType().GetMethod("GetApprovalById").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.GetApprovalById(Guid.NewGuid());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Equal((int)TranSmart.Core.Permission.SA_LeaveApplication, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Read, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as LeaveInfoModel).EmployeeId);
		}
		[Fact]
		public async Task GetLeaveBalanceByEmpLeaveType()
		{
			TranSmart.Core.Result.Result<LeaveBalance> result = new();
			_applyLeaveService.Setup(x => x.GetLeaveBalanceByLeaveType(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
				.Callback((Guid leaveTypeId, Guid empId,DateTime fromDate,DateTime toDate) =>
				{
					result.IsSuccess = true;
					result.ReturnValue = new LeaveBalance
					{
						Leaves = 0.5m
					};
				});
			// Act
			var response = await _controller.GetLeaveBalanceByEmpLeaveType(Guid.NewGuid(), Guid.NewGuid(),DateTime.Now.Date,DateTime.Now.Date);
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}

		[Fact]
		public async Task LeaveManagement_GetEmployeeLeaveTypes_Test()
		{
			var leaveTypeId = Guid.NewGuid();
			var list = new List<LeaveBalanceModel>
			{
				new LeaveBalanceModel
				{
					LeaveTypeId = leaveTypeId,
					LeaveTypeName = "Casual Leave",
					EmployeeId = EmployeeId,
					Leaves = 2
				}
			};
			_applyLeaveService.Setup(x => x.GetEmployeeLeaveTypes(It.IsAny<Guid>()))
				.ReturnsAsync(list);
			//Act
			var response = await _controller.GetEmployeeLeaveTypes(EmployeeId);
			var okResult = response as OkObjectResult;
			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}

		[Theory]
		[InlineData(2, true)]//approve
		[InlineData(3, false)]//reject 
		public async Task Approval_Update_Test(byte status, bool isApproved)
		{
			//Arrange
			TranSmart.Core.Result.Result<ApplyLeave> result = new();

			_applyLeaveService.Setup(x => x.Approve(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<bool>()))
				   .Callback((Guid Id, Guid approvedEmpId, bool isAdminRequest) =>
				   {
					   result.IsSuccess = true;
					   result.ReturnValue = new ApplyLeave { ApprovedById = EmployeeId, Status = status };
				   }).ReturnsAsync(result);
			_applyLeaveService.Setup(x => x.Reject(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<Guid>()))
				.Callback((Guid Id, string rejectReason, Guid rejectedEmpId) =>
				{
					result.IsSuccess = true;
					result.ReturnValue = new ApplyLeave { ApprovedById = EmployeeId, Status = status };
				}).ReturnsAsync(result);
			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("ApprovalPut").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.ApprovalPut(new ApproveRequest { IsApproved = isApproved });
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Equal((int)TranSmart.Core.Permission.SA_LeaveApplication, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Update, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as ApplyLeavesModel).ApprovedById);
			Assert.Equal(status, (okResult.Value as ApplyLeavesModel).Status);
		}

		[Fact]
		public async Task Approval_Paginate_Test()
		{
			// Arrange  
			var list = new List<ApplyLeave>
			{ new ApplyLeave {
				ApprovedById =  EmployeeId
				},
			  new ApplyLeave {
				  ApprovedById =  Guid.NewGuid()
			  } }.AsQueryable();
			_applyLeaveService.Setup(x => x.ApprovalPaginate(It.IsAny<BaseSearch>())).ReturnsAsync(list.ToPaginate(0, 10));

			// Act
			var attributes = _controller.GetType().GetMethod("ApprovalPaginate").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await _controller.ApprovalPaginate(new ApplyLeaveSearch());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Equal((int)TranSmart.Core.Permission.SA_LeaveApplication, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Read, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Single(((TranSmart.API.Models.Paginate<ApplyLeavesList>)okResult.Value).Items.Where(x => x.ApprovedById == EmployeeId));
		}

	}
}
