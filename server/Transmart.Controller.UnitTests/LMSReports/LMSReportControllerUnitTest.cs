using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Reports;
using TranSmart.API.Extensions;
using TranSmart.Domain.Models.Reports;
using TranSmart.Domain.Models.Reports.LMS;
using TranSmart.Service.Reports;
using TranSmart.Service.Reports.LMS;
using Xunit;

namespace Transmart.Controller.UnitTests.LMSReports
{
    public class LMSReportControllerUnitTest : ControllerTestBase
    {
        private readonly Mock<ILmsReportService> _LMSreportService;
        private readonly Mock<ILmsService> _reportService;
        private readonly LmsReportController _controller;
        public LMSReportControllerUnitTest() : base()
        {
            _LMSreportService = new Mock<ILmsReportService>();
            _reportService = new Mock<ILmsService>();
            _controller = new LmsReportController(_LMSreportService.Object, _reportService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = Claim }
                }
            };
        }
        [Fact]
        public async Task My_Team_Attendance_Test()
        {
            // Arrange  
            var result = new TranSmart.Core.Result.Result<IEnumerable<AttendanceModel>>();
            _LMSreportService.Setup(x => x.MyTeamAttendance(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(result.ReturnValue = new List<AttendanceModel> { new AttendanceModel { EmployeeId = EmployeeId } }));

            var controller = _controller;
            // Act
            var attributes = controller.GetType().GetMethod("MyTeamAttendance").GetCustomAttributes(typeof(ApiReportAuthorizeAttribute), true);
            var resposne = await controller.MyTeamAttendance(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now.Date, DateTime.Now.Date, 1);
            var okResult = resposne as OkObjectResult;

            // Assert
            Assert.Equal((int)((ApiReportAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.ReportPermission.LMS_MyTeamAttendance);

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, ((List<AttendanceModel>)okResult.Value)[0].EmployeeId);
        }

        [Fact]
        public async Task My_Team_Leave_Balance_Test()
        {
            // Arrange  
            var result = new TranSmart.Core.Result.Result<IEnumerable<LeaveBalancesModel>>();
            _LMSreportService.Setup(x => x.MyTeamLeaveBalances(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(result.ReturnValue = new List<LeaveBalancesModel> { new LeaveBalancesModel { ReportingToId = EmployeeId } }));

            var controller = _controller;
            // Act
            var attributes = controller.GetType().GetMethod("MyTeamLeaveBalances").GetCustomAttributes(typeof(ApiReportAuthorizeAttribute), true);
            var resposne = await controller.MyTeamLeaveBalances(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            var okResult = resposne as OkObjectResult;

            // Assert
            Assert.Equal((int)((ApiReportAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.ReportPermission.LMS_MyTeamLeaveBalance);

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, ((List<LeaveBalancesModel>)okResult.Value)[0].ReportingToId);
        }
        [Fact]
        public async Task Self_Attendance_Test()
        {
			// Arrange  
			var result = new TranSmart.Core.Result.Result<IEnumerable<AttendanceReportModel>>();
            _LMSreportService.Setup(x => x.SelfAttendance(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(result.ReturnValue = new List<AttendanceReportModel> { new AttendanceReportModel { EmployeeId = EmployeeId } }));

            var controller = _controller;
            // Act
            var attributes = controller.GetType().GetMethod("SelfAttendance").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var resposne = await controller.SelfAttendance(DateTime.Today.Date, DateTime.Today.Date, 1);
            var okResult = resposne as OkObjectResult;

            // Assert
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SS_Attendance);
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, ((List<AttendanceReportModel>)okResult.Value)[0].EmployeeId);
        }
    }
}
