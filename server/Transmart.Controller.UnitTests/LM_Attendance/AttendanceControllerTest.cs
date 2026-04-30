using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transmart.TS4API;
using Transmart.TS4API.Models;
using TranSmart.API.Controllers.Attendance;
using TranSmart.API.Extensions;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.LM_Attendance;
using TranSmart.Domain.Models.LM_Attendance.List;
using TranSmart.Domain.Models.LM_Attendance.Response;
using TranSmart.Service.Leave;
using TranSmart.Service.Organization;
using Xunit;

namespace Transmart.Controller.UnitTests.LM_Attendance
{
	public class AttendanceControllerTest : ControllerTestBase
	{

		private readonly Mock<IAttendanceService> _attendanceService;
		private readonly AttendanceController _controller;
		private readonly Mock<IEmployeeService> _empService;
		private readonly Mock<ITs4ApiS> _apis;

		public AttendanceControllerTest() : base()
		{
			_attendanceService = new Mock<IAttendanceService>();
			_empService = new Mock<IEmployeeService>();
			_apis = new Mock<ITs4ApiS>();
			_controller = new AttendanceController(Mapper, _attendanceService.Object, _empService.Object, _apis.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};
		}
		[Fact]
		public async Task Attendance_Paginate_GetValidRecords()
		{
			// Arrange  
			var list = new List<Attendance>
			{
				new Attendance
				{
					EmployeeId = EmployeeId
				},
				new Attendance
				{
					EmployeeId = Guid.NewGuid(),
				},
				new Attendance
				{
					EmployeeId = Guid.NewGuid()
				}
			}.AsQueryable();

			_attendanceService.Setup(x => x.GetPaginate(It.IsAny<BaseSearch>())).ReturnsAsync(list.ToPaginate(0, 10));

			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("Paginate").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.Paginate(new BaseSearch());
			var okResult = response as OkObjectResult;

			//assert
			Assert.True(attributes.Any());
			Assert.Equal((int)TranSmart.Core.Permission.LM_Attendance, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Read, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Single(((TranSmart.API.Models.Paginate<AttendanceList>)okResult.Value).Items.Where(x => x.EmployeeId == EmployeeId));

		}

		[Fact]
		public async Task Attendance_GetById_GetValidRecords()
		{
			// Arrange  
			var id = Guid.NewGuid();

			Result<Attendance> result = new();
			_attendanceService.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(result.ReturnValue = new Attendance { ID = id, EmployeeId = EmployeeId }));

			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("Get").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.Get(id);

			//assert
			Assert.True(attributes.Any());
			Assert.Equal((int)TranSmart.Core.Permission.LM_Attendance, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Read, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);

			Assert.Equal(id, response.ID);
			Assert.Equal(EmployeeId, response.EmployeeId);

		}

		[Fact]
		public async Task Attendance_GetDetails_GetValidRecords()
		{
			// Arrange  

			Result<Attendance> result = new();
			_attendanceService.Setup(x => x.GetAttendanceReport(It.IsAny<Guid>(), It.IsAny<DateTime>()))
					  .Returns(Task.FromResult(result.ReturnValue = new Attendance { ID = Guid.NewGuid(), EmployeeId = EmployeeId, AttendanceDate = DateTime.Now.Date }));

			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("Get").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.GetDetails(EmployeeId, DateTime.Now);

			//assert
			Assert.True(attributes.Any());
			Assert.Equal((int)TranSmart.Core.Permission.LM_Attendance, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Read, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);

			Assert.Equal(EmployeeId, response.EmployeeId);
			Assert.Equal(DateTime.Now.Date, response.AttendanceDate);

		}


		[Fact]
		public async Task Attendance_GetDate_GetValidRecords()
		{
			// Arrange  

			Result<Attendance> result = new();
			_attendanceService.Setup(x => x.GetDate(It.IsAny<Guid>())).Returns(Task.FromResult(result.ReturnValue = new Attendance { ID = Guid.NewGuid(), EmployeeId = EmployeeId }));

			var controller = _controller;

			// Act
			var response = await controller.GetDate(EmployeeId);

			//assert
			Assert.Equal(EmployeeId, response.EmployeeId);

		}

		[Fact]
		public async Task Attendance_GetPunchIn_GetValidRecords()
		{
			// Arrange  
			Result<Attendance> result = new();
			_attendanceService.Setup(x => x.GetPunchIn(It.IsAny<Guid>()))
					  .Returns(Task.FromResult(result.ReturnValue = new Attendance { EmployeeId = EmployeeId, AttendanceDate = DateTime.Today }));

			var controller = _controller;

			// Act
			var response = await controller.GetPunchIn();
			var okResult = response as OkObjectResult;

			//assert
			Assert.Equal(EmployeeId, ((AttendanceModel)okResult.Value).EmployeeId);

		}

		[Fact]
		public async Task Attendance_GetPunchIn_NoContent()
		{
			// Arrange  
			Result<Attendance> result = new();
			_attendanceService.Setup(x => x.GetPunchIn(It.IsAny<Guid>()))
					  .Returns(Task.FromResult(result.ReturnValue = null));

			var controller = _controller;

			// Act
			var response = await controller.GetPunchIn();
			var okResult = response as OkObjectResult;

			//assert
			Assert.Null(okResult);

		}

		[Fact]
		public async Task Attendance_IsPunchedEmployee_True()
		{
			// Arrange  
			_attendanceService.Setup(x => x.IsPunchEmployee(It.IsAny<Guid>()))
					  .Returns(Task.FromResult(true));

			var controller = _controller;

			// Act
			var response = await controller.IsPunchedEmployee();
			var okResult = response as OkObjectResult;

			//assert
			Assert.True((bool)okResult.Value);

		}

		[Fact]
		public async Task Attendance_GetAttendanceData_GetValidRecords()
		{
			// Arrange  
			var list = new List<Employee>
			{
				new Employee
				{
					ID = EmployeeId
				},
				new Employee
				{
					ID = Guid.NewGuid(),
				},
			}.AsQueryable();
			_attendanceService.Setup(x => x.GetAttendanceData(It.IsAny<BaseSearch>())).ReturnsAsync(list.ToPaginate(0, 10));

			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("GetAttendanceData").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.GetAttendanceData(new AttendanceSearch());
			var okResult = response as OkObjectResult;

			//assert
			Assert.True(attributes.Any());
			Assert.Equal((int)TranSmart.Core.Permission.LM_Attendance, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Read, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Single(((TranSmart.API.Models.Paginate<EmployeeAttendance>)okResult.Value).Items.Where(x => x.ID == EmployeeId));

		}

		[Fact]
		public async Task Attendance_FinalizedAttendance_GetValidRecords()
		{
			// Arrange  
			var sum = new AttendanceSum
			{
				Present = 29,
				LOP = 0.5m,
				Unauthorized = 1.5m
			};
			Result<AttendanceSum> result = new();
			_attendanceService.Setup(x => x.Finalized(It.IsAny<byte>(), It.IsAny<short>())).
				Callback((byte month, short year) =>
				{
					result.IsSuccess = true;
					result.ReturnValue = sum;
				}).ReturnsAsync(result);


			var controller = _controller;

			// Act
			var response = await controller.Finalized(DateTime.Now);
			var okResult = response as OkObjectResult;

			//assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(0.5m, ((Result<AttendanceSum>)okResult.Value).ReturnValue.LOP);

		}

		[Theory]
		[InlineData(1, "2022-11-20", true)]//MonthStartDay is 1
		[InlineData(2, "2022-11-20", false)]//MonthStartDay is 2
		public async Task Attendance_DownloadAttendance_GetValidRecords(int monthStartDay, DateTime date, bool isHalfDay)
		{
			// Arrange  

			var list = new List<Attendance>
			{
				new Attendance
				{
					EmployeeId = EmployeeId,
					AttendanceDate=DateTime.Parse("2022-10-20"),
					AttendanceStatus= (int)AttendanceStatus.Present,
					IsHalfDay=isHalfDay,
					HalfDayType=1
				},
				new Attendance
				{
					EmployeeId = Guid.NewGuid(),
					AttendanceDate = DateTime.Now.AddDays(-3),
					AttendanceStatus= (int)AttendanceStatus.Present
				},
			}.AsQueryable();

			var employeeList = new List<Employee>
			{
				new Employee
				{
					ID = EmployeeId,
					Status = 1,
					Designation= new Designation(),
					Department = new Department(),
					DateOfJoining=DateTime.Parse("2021-11-22")
				},
				new Employee
				{
					ID = Guid.NewGuid(),
					Status = 2,
					Designation= new Designation(),
					Department = new Department(),
					DateOfJoining=DateTime.Parse("2021-11-22")
				},
			}.AsQueryable();


			_empService.Setup(x => x.GetEmps(It.IsAny<DateTime>())).ReturnsAsync(employeeList);

			_attendanceService.Setup(x => x.GetOrganizations()).ReturnsAsync(new Organizations { ID = Guid.NewGuid(), Name = "Avontix", MonthStartDay = monthStartDay });

			_attendanceService.Setup(x => x.GetAttendance(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(list);

			var controller = _controller;

			// Act
			var response = await controller.DownloadAttandeance(date);
			var okResult = response as OkObjectResult;

			//assert

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}


		[Fact]
		public async Task Attendance_ImportCodingLogs_Test()
		{
			// Arrange  
			var list = new List<AttendanceImportModel>();
			Result<ManualAttLogs> result = new();

			_apis.Setup(x => x.GetCodingAttLogs(It.IsAny<DateTime>())).ReturnsAsync(list);

			_attendanceService.Setup(x => x.ManualLogsImport(It.IsAny<List<ManualAttLogs>>())).Callback((List<ManualAttLogs> manualAttLogs) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new ManualAttLogs { EmployeeId = EmployeeId };

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var response = await controller.ImportCodingLogs();
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, ((Result<ManualAttLogs>)okResult.Value).ReturnValue.EmployeeId);
		}

		[Fact]
		public async Task Attendance_ImportTranscriptionLogs_Test()
		{
			// Arrange  
			var list = new List<AttendanceImportModel>();
			Result<ManualAttLogs> result = new();

			_apis.Setup(x => x.GetTranscriptionAttLogs(It.IsAny<DateTime>())).ReturnsAsync(list);

			_attendanceService.Setup(x => x.ManualLogsImport(It.IsAny<List<ManualAttLogs>>())).Callback((List<ManualAttLogs> manualAttLogs) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new ManualAttLogs { EmployeeId = EmployeeId };

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var response = await controller.ImportTranscriptionLogs();
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, ((Result<ManualAttLogs>)okResult.Value).ReturnValue.EmployeeId);
		}

		[Fact]
		public async Task Attendance_ImportTranscriptionLogsWithList_Test()
		{
			// Arrange  
			Result<ManualAttLogs> result = new();

			_attendanceService.Setup(x => x.ManualLogsImport(It.IsAny<List<ManualAttLogs>>())).Callback((List<ManualAttLogs> manualAttLogs) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new ManualAttLogs { EmployeeId = EmployeeId };

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var response = await controller.ImportTranscriptionLogs(new List<ManualAttLogs>());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, ((Result<ManualAttLogs>)okResult.Value).ReturnValue.EmployeeId);
		}

		[Fact]
		public async Task Attendance_ManualLogsImport_Test()
		{
			// Arrange  
			Result<BiometricAttLogs> result = new();

			_attendanceService.Setup(x => x.BiometricLogsImport(It.IsAny<List<BiometricAttLogs>>())).Callback((List<BiometricAttLogs> biometricAttLogs) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new BiometricAttLogs { EmpCode = "2063" };

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var response = await controller.ImportBiometricLogs(new List<BiometricAttLogsRequest>());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal("2063", ((Result<BiometricAttLogs>)okResult.Value).ReturnValue.EmpCode);
		}

		[Fact]
		public async Task Attendance_Post_Test()
		{
			// Arrange  
			Result<Attendance> result = new();
			_attendanceService.Setup(x => x.AddAsync(It.IsAny<Attendance>())).Callback((Attendance attendance) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = attendance;

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("Post").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.Post(new AttendanceModel());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.True(attributes.Any());
			Assert.Equal((int)TranSmart.Core.Permission.LM_Attendance, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Create, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as AttendanceModel).EmployeeId);
		}

		[Fact]
		public async Task Attendance_Post_BadRequest()
		{
			// Arrange  
			Result<Attendance> result = new();
			_attendanceService.Setup(x => x.AddAsync(It.IsAny<Attendance>())).Callback((Attendance attendance) =>
			{
				result.IsSuccess = false;
				result.ReturnValue = attendance;

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("Post").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.Post(new AttendanceModel());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.True(attributes.Any());
			Assert.Equal((int)TranSmart.Core.Permission.LM_Attendance, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Create, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);

			Assert.Null(okResult);
		}


		[Fact]
		public async Task Attendance_PostTimings_Test()
		{
			// Arrange  
			Result<Attendance> result = new();
			_attendanceService.Setup(x => x.AddNewTimings(It.IsAny<Attendance>())).Callback((Attendance attendance) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = attendance;

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("Post").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.PostTimings(new AttendanceModel());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.True(attributes.Any());
			Assert.Equal((int)TranSmart.Core.Permission.LM_Attendance, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Create, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);

			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as AttendanceModel).EmployeeId);
		}

		[Fact]
		public async Task Attendance_PostTimings_BadRequest()
		{
			// Arrange  
			Result<Attendance> result = new();
			_attendanceService.Setup(x => x.AddNewTimings(It.IsAny<Attendance>())).Callback((Attendance attendance) =>
			{
				result.IsSuccess = false;
				result.ReturnValue = attendance;

			}).ReturnsAsync(result);

			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("Post").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.PostTimings(new AttendanceModel());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.True(attributes.Any());
			Assert.Equal((int)TranSmart.Core.Permission.LM_Attendance, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Create, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);

			Assert.Null(okResult);
		}

		[Fact]
		public async Task Attendance_RunAttendance_Test()
		{
			//Arrange
			Result<Attendance> result = new();

			_attendanceService.Setup(x => x.CalculateAttendance(It.IsAny<DateTime>()))
				.Callback((DateTime date) =>
				{
					result.IsSuccess = true;
					result.ReturnValue = new Attendance { EmployeeId = EmployeeId };
				}).ReturnsAsync(result);
			var controller = _controller;

			// Act
			var response = await controller.RunAttendance(DateTime.Now);
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, ((Result<Attendance>)okResult.Value).ReturnValue.EmployeeId);
		}

		[Fact]
		public async Task Attendance_UpdateAttendance_Test()
		{
			//Arrange
			Result<Attendance> result = new();

			_attendanceService.Setup(x => x.AttendanceUpdate(It.IsAny<List<AttendanceDetails>>(),It.IsAny<Guid>()))
				.Callback((List<Attendance> attendances) =>
				{
					result.IsSuccess = true;
					result.ReturnValue = new Attendance { EmployeeId = EmployeeId };
				}).ReturnsAsync(result);
			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("Put").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.UpdateAttendance(new AttendanceDetails());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Equal((int)TranSmart.Core.Permission.LM_Attendance, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Update, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, ((Attendance)okResult.Value).EmployeeId);
		}

		[Fact]
		public async Task Attendance_PutTimings_Test()
		{
			//Arrange
			Result<Attendance> result = new();

			_attendanceService.Setup(x => x.UpdateTimings(It.IsAny<Guid>()))
				.Callback((Guid id) =>
				{
					result.IsSuccess = true;
					result.ReturnValue = new Attendance { EmployeeId = EmployeeId };
				}).ReturnsAsync(result);
			var controller = _controller;

			// Act
			var response = await controller.PutTimings();
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, ((AttendanceModel)okResult.Value).EmployeeId);
		}

		[Fact]
		public async Task Attendance_PutTimings_BadRequest()
		{
			//Arrange
			Result<Attendance> result = new();

			_attendanceService.Setup(x => x.UpdateTimings(It.IsAny<Guid>()))
				.Callback((Guid id) =>
				{
					result.IsSuccess = false;
					result.ReturnValue = new Attendance { EmployeeId = EmployeeId };
				}).ReturnsAsync(result);
			var controller = _controller;

			// Act
			var response = await controller.PutTimings();
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Null(okResult);
		}

		[Fact]
		public async Task Attendance_RePunchIn_Test()
		{
			//Arrange
			Result<Attendance> result = new();

			_attendanceService.Setup(x => x.RePunchIn(It.IsAny<Guid>()))
				.Callback((Guid id) =>
				{
					result.IsSuccess = true;
					result.ReturnValue = new Attendance { EmployeeId = EmployeeId };
				}).ReturnsAsync(result);
			var controller = _controller;

			// Act
			var response = await controller.RePunchIn();
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, ((AttendanceModel)okResult.Value).EmployeeId);
		}

		[Fact]
		public async Task Attendance_RePunchIn_BadRequest()
		{
			//Arrange
			Result<Attendance> result = new();

			_attendanceService.Setup(x => x.RePunchIn(It.IsAny<Guid>()))
				.Callback((Guid id) =>
				{
					result.IsSuccess = false;
					result.ReturnValue = new Attendance { EmployeeId = EmployeeId };
				}).ReturnsAsync(result);
			var controller = _controller;

			// Act
			var response = await controller.RePunchIn();
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Null(okResult);
		}

		[Fact]
		public async Task Attendance_Put_Test()
		{
			//Arrange
			Result<Attendance> result = new();

			_attendanceService.Setup(x => x.UpdateAsync(It.IsAny<Attendance>()))
				.Callback((Attendance attendance) =>
				{
					result.IsSuccess = true;
					result.ReturnValue = new Attendance { EmployeeId = EmployeeId };
				}).ReturnsAsync(result);
			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("Put").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.Put(new AttendanceModel());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Equal((int)TranSmart.Core.Permission.LM_Attendance, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Update, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, ((AttendanceModel)okResult.Value).EmployeeId);
		}

		[Fact]
		public async Task Attendance_Put_BadRequest()
		{
			//Arrange
			Result<Attendance> result = new();

			_attendanceService.Setup(x => x.UpdateAsync(It.IsAny<Attendance>()))
				.Callback((Attendance attendance) =>
				{
					result.IsSuccess = false;
					result.ReturnValue = new Attendance { EmployeeId = EmployeeId };
				}).ReturnsAsync(result);
			var controller = _controller;

			// Act
			var attributes = controller.GetType().GetMethod("Put").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var response = await controller.Put(new AttendanceModel());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.Equal((int)TranSmart.Core.Permission.LM_Attendance, (int)((ApiAuthorizeAttribute)attributes[0]).Permission);
			Assert.Equal((int)TranSmart.Core.Privilege.Update, (int)((ApiAuthorizeAttribute)attributes[0]).Privilege);
			Assert.Null(okResult);
		}
	}
}
