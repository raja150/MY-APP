using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Payroll;
using TranSmart.API.Extensions;
using TranSmart.Domain.Entities.Payroll;
using TranSmart.Domain.Models.Payroll;
using TranSmart.Domain.Models.Payroll.Response;
using TranSmart.Service;
using TranSmart.Service.Payroll;
using Xunit;

namespace Transmart.Controller.UnitTests.PayRoll
{
    public class PayMonthControllerTest : ControllerTestBase
    {
        private Mock<SequenceNoService> _sequnce;
        private Mock<IPayMonthService> _paymonthservice;
        private Mock<EarningComponentService> _earningService;
        private Mock<TranSmart.Service.Payroll.DeductionComponentService> _deductionService;
        private Mock<EmpStatutoryService> _empstatutoryservice;
        private Mock<TranSmart.Service.Organization.OrganizationsService> _orgService;
        private Mock<ApplicationAuditLogService> _auditLogService;
        private readonly PayMonthController _controller;


        public PayMonthControllerTest() : base()
        {
            _sequnce = new Mock<SequenceNoService>(UOW.Object);
            _paymonthservice = new Mock<IPayMonthService>();
            _earningService = new Mock<EarningComponentService>(UOW.Object);
            _deductionService = new Mock<TranSmart.Service.Payroll.DeductionComponentService>(UOW.Object);
            _empstatutoryservice = new Mock<EmpStatutoryService>(UOW.Object);
            _orgService = new Mock<TranSmart.Service.Organization.OrganizationsService>(UOW.Object);
            _auditLogService = new Mock<ApplicationAuditLogService>(UOW.Object);
            _controller = new PayMonthController(Mapper, _paymonthservice.Object, _earningService.Object, _orgService.Object, _deductionService.Object,
                _empstatutoryservice.Object, _auditLogService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = Claim }
                }
            };
        }


        [Fact]
        public async Task PayMonth_GetMyPaySlip_Test()
        {
            // Arrange  
            TranSmart.Core.Result.Result<PaySheet> result = new();
            _paymonthservice.Setup(x => x.GetEmployeePay(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(result.ReturnValue = new PaySheet { EmployeeID = EmployeeId } ));

            var controller = _controller;
            // Act
            var attributes = controller.GetType().GetMethod("GetMyPaySlip").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var resposne = await controller.GetMyPaySlip(Guid.NewGuid());
            var okResult = resposne as OkObjectResult;

            // Assert
            Assert.True(attributes.Any());
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SS_SalaryDetails);
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as PaySlipModel).EmployeeID);
        }

        [Fact]
        public async Task PayMonth_GetAnnualSalary_Test()
        {
            // Arrange  
            TranSmart.Core.Result.Result<IEnumerable<PaySheet>> result = new();
            _paymonthservice.Setup(x => x.GetEmployeeAnnualSalary(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(result.ReturnValue = new List<PaySheet> { new PaySheet { EmployeeID=EmployeeId} }));

            var controller = _controller;
            // Act
            //var attributes = controller.GetType().GetMethod("GetAnnualSalary").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var resposne = await controller.GetAnnualSalary(Guid.NewGuid());
            var okResult = resposne as OkObjectResult;

            // Assert
            //Assert.True(attributes.Any());
            //Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.PS_Payrun);
            //Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, ((List<SalarySummaryModel>)okResult.Value)[0].EmployeeId);
        }

        [Fact]
        public async Task PayMonth_GetMyPayMonths_Test()
        {
            // Arrange  
            TranSmart.Core.Result.Result<IEnumerable<PaySheet>> result = new();
            _paymonthservice.Setup(x => x.GetSalarySlips(It.IsAny<Guid>())).Returns(Task.FromResult(result.ReturnValue = new List<PaySheet> { new PaySheet { EmployeeID = EmployeeId } }));

            var controller = _controller;
            // Act

            var attributes = controller.GetType().GetMethod("GetMyPayMonths").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var resposne = await controller.GetMyPayMonths();
            var okResult = resposne as OkObjectResult;

            // Assert
            Assert.True(attributes.Any());
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SS_SalaryDetails);
            Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }
    }
}  




   
