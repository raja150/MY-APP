using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Organization;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models.Organization;
using TranSmart.Service.Organization;
using Xunit;

namespace Transmart.Controller.UnitTests.Leave
{
    public class EmployeeControllerTest : ControllerTestBase
    {
        private Mock<IEmployeeService> _employeeService;
        private readonly EmployeeController _controller;
        public EmployeeControllerTest() : base()
        {
            _employeeService = new Mock<IEmployeeService>();
            _controller = new EmployeeController(Mapper, _employeeService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = Claim }
                }
            };
        }
        [Fact]
        public async Task Get_Employee_Details_Test()
        {
            TranSmart.Core.Result.Result<Employee> result = new();
            _employeeService.Setup(x => x.GetDetails(It.IsAny<Guid>())).Returns(Task.FromResult(result.ReturnValue = new Employee { ID = EmployeeId }));


            //_employeeService.Setup(x => x.GetDetails(It.IsAny<Guid>())).Callback((Guid EmpId) =>
            //{
            //    result.IsSuccess = true;
            //    result.ReturnValue = new Employee { ID = EmployeeId };
            //});
            var controller = _controller;
            // Act
            var resposne = await controller.GetEmployeeDetails();
            var okResult = resposne as OkObjectResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as EmployeeAddModel).ID);

        }
        [Fact]
        public async Task Search_Employee_Test()
        {
            TranSmart.Core.Result.Result<IEnumerable<Employee>> result = new();
            _employeeService.Setup(x => x.SearchEmp(It.IsAny<string>() , It.IsAny<Guid>() , It.IsAny<Guid>())).Returns(Task.FromResult(result.ReturnValue = new List<Employee> { new Employee { ID = Guid.NewGuid(), Name = "asdf", } }));
            
            var controller = new EmployeeController(Mapper, _employeeService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = Claim }
                }
            };
            // Act
            var resposne = await controller.SearchEmp("chiste" , Guid.NewGuid());
            var okResult = resposne as OkObjectResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotEqual(EmployeeId, ((List<EmployeeInfoModel>)okResult.Value)[0].ID);
        }
        [Fact]
        public async Task Employee_Resignation_Test() 
        {
            TranSmart.Core.Result.Result<EmployeeResignation> result = new();
            _employeeService.Setup(x => x.GetResignationDetails(It.IsAny<Guid>())).Returns(Task.FromResult(new EmployeeResignation { EmployeeId = EmployeeId }));
            //    .Callback((Guid EmpId) =>
            //{
            //    result.IsSuccess = true;
            //    result.ReturnValue = new EmployeeResignation { EmployeeId = EmployeeId };
            //});
            var controller = _controller;
            // Act
            var resposne = await controller.GetResignationDetails();
            var okResult = resposne as OkObjectResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as EmployeeResignationModel).EmployeeId);

        }
        [Fact]
        public async Task Get_Contact_Details_Test() 
        {
            TranSmart.Core.Result.Result<IEnumerable<EmployeeFamily>> result = new();
            _employeeService.Setup(x => x.GetContactDetails(It.IsAny<Guid>())).Returns(Task.FromResult(result.ReturnValue = new List<EmployeeFamily> { new EmployeeFamily { EmployeeId = EmployeeId} }));
            //    .Callback((Guid EmpId) =>
            //{
            //    result.IsSuccess = true;
            //    result.ReturnValue = new EmployeeResignation { EmployeeId = EmployeeId };
            //});
            var controller = _controller;
            // Act
            var resposne = await controller.GetContactDetails();
            var okResult = resposne as OkObjectResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, ((List<EmployeeFamilyModel>)okResult.Value)[0].EmployeeId);

        }
        [Fact]
        public async Task Get_Present_Address_Test()
        {
            //Arrange
            TranSmart.Core.Result.Result<EmployeePresentAd> result = new();
            _employeeService.Setup(x => x.GetPresentAddress(It.IsAny<Guid>())).Returns(Task.FromResult(new EmployeePresentAd { EmployeeId = EmployeeId }));
            var controller = _controller;
            //Act
            var response = await controller.GetPresentAddress();
            var okResult = response as OkObjectResult;
            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as EmployeePresentAdModel).EmployeeId);
        }
        [Fact]
        public async Task Get_Permanent_Address_Test()
        {
            //Arrange
            TranSmart.Core.Result.Result<EmployeePresentAd> result = new();
            _employeeService.Setup(x => x.GetPermanentAddress(It.IsAny<Guid>())).Returns(Task.FromResult(new EmployeePermanentAd { EmployeeId = EmployeeId }));
            var controller = _controller;
            //Act
            var response = await controller.GetPermanentAddress();
            var okResult = response as OkObjectResult;
            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as EmployeePermanentAdModel).EmployeeId);
        }
        [Fact]
        public async Task Get_Emergency_Address_Test()
        {
            //Arrange
            TranSmart.Core.Result.Result<EmployeeEmergencyAd> result = new();
            _employeeService.Setup(x => x.GetEmergencyAddress(It.IsAny<Guid>())).Returns(Task.FromResult(result.ReturnValue= new EmployeeEmergencyAd { EmployeeId = EmployeeId }));
            var controller = _controller;
            //Act
            var response = await controller.GetEmergencyAddress();
            var okResult = response as OkObjectResult;
            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as EmployeeEmergencyAdModel).EmployeeId);
        }
        [Fact]
        public async Task Get_Birthdays_Test()
        {
            //Arrange
            TranSmart.Core.Result.Result<IEnumerable<Employee>> result = new();
            _employeeService.Setup(x => x.GetBirthdays(It.IsAny<Guid>())).Returns(Task.FromResult(result.ReturnValue = new List<Employee> { new Employee { DepartmentId = DepartmentId } }));
            var controller = _controller;
            //Act
            var response = await controller.GetBirthdays();
            var okResult = response as OkObjectResult;
            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(DepartmentId,((List<EmployeeInfoModel>)okResult.Value)[0].DepartmentId);
        }
    }
}
