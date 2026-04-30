using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers.Organization;
using TranSmart.API.Extensions;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models.Organization;
using TranSmart.Service;
using TranSmart.Service.Organization;
using Xunit;

namespace Transmart.Controller.UnitTests.Organization
{
    public class UpdateProfileControllerTest : ControllerTestBase
    {
        private Mock<ISequenceNoService> _sequence;
        private Mock<IUpdateProfileService> _updateservice;
        private Mock<IEmployeePresentAdService> _emppresentadservice;
        private Mock<IEmployeeEmergencyAdService> _empemergencyadservice;
        private Mock<IEmployeePermanentAdService> _emppermanentadservice;
        private Mock<IEmployeeFamilyService> _empfamilyservice;
        private readonly UpdateProfileController _controller;

        public UpdateProfileControllerTest() : base()
        {
            _sequence = new Mock<ISequenceNoService>();
            _updateservice = new Mock<IUpdateProfileService>();
            _emppresentadservice = new Mock<IEmployeePresentAdService>();
            _empemergencyadservice = new Mock<IEmployeeEmergencyAdService>();
            _emppermanentadservice = new Mock<IEmployeePermanentAdService>();
            _empfamilyservice = new Mock<IEmployeeFamilyService>();
            _controller = new UpdateProfileController(Mapper, _updateservice.Object, _emppresentadservice.Object, _emppermanentadservice.Object, _empemergencyadservice.Object, _empfamilyservice.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = Claim }
                }
            };
        }

        [Fact]
        public async Task UpdateProfile_EmergencyAdd_Test()
        {
            // Arrange  
            TranSmart.Core.Result.Result<EmployeeEmergencyAd> result = new();
            TranSmart.Core.Result.Result<Employee> result1 = new();
            _updateservice.Setup(x => x.Verification(It.IsAny<Guid>())).Callback((Guid empId) =>
            {
                result1.IsSuccess = true;
                result1.ReturnValue = new Employee { ID = EmployeeId };
            }).ReturnsAsync(result1);
            _empemergencyadservice.Setup(x => x.UpdateAsync(It.IsAny<EmployeeEmergencyAd>())).Callback((EmployeeEmergencyAd empemrAdd) =>
            {
                result.IsSuccess = true;
                result.ReturnValue = empemrAdd;
            }).ReturnsAsync(result);

            var controller = _controller;
            // Act
            //var attributes = controller.GetType().GetMethod("Put").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var resposne = await controller.Put(new EmployeeEmergencyAdModel());
            var okResult = resposne as OkObjectResult;

            //Assert.True(attributes.Any());
            //Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Permission, (int)TranSmart.Core.Permission.SS_Payslips);
            //Assert.Equal((int)((ApiAuthorizeAttribute)attributes[0]).Privilege, (int)TranSmart.Core.Privilege.Read);
            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as EmployeeEmergencyAdModel).EmployeeId);
        }

        [Fact]
        public async Task UpdateProfile_PresentAdd_Test()
        {
            // Arrange  
            TranSmart.Core.Result.Result<EmployeePresentAd> result = new();
            TranSmart.Core.Result.Result<Employee> result1 = new();
            _updateservice.Setup(x => x.Verification(It.IsAny<Guid>())).Callback((Guid empId) =>
            {
                result1.IsSuccess = true;
                result1.ReturnValue = new Employee { ID = EmployeeId };
            }).ReturnsAsync(result1);
            _emppresentadservice.Setup(x => x.UpdateAsync(It.IsAny<EmployeePresentAd>())).Callback((EmployeePresentAd emppreAdd) =>
            {
                result.IsSuccess = true;
                result.ReturnValue = emppreAdd;
            }).ReturnsAsync(result);

            var controller = _controller;
            // Act
            //var attributes = controller.GetType().GetMethod("Put").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var response = await controller.Put(new EmployeePresentAdModel());
            var okResult = response as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as EmployeePresentAdModel).EmployeeId);
        }

        [Fact]
        public async Task UpdateProfile_PermanentAdd_Test()
        {
            // Arrange  
            TranSmart.Core.Result.Result<EmployeePermanentAd> result = new();
            TranSmart.Core.Result.Result<Employee> result1 = new();
            _updateservice.Setup(x => x.Verification(It.IsAny<Guid>())).Callback((Guid empId) =>
            {
                result1.IsSuccess = true;
                result1.ReturnValue = new Employee { ID = EmployeeId };
            }).ReturnsAsync(result1);
            _emppermanentadservice.Setup(x => x.UpdateAsync(It.IsAny<EmployeePermanentAd>())).Callback((EmployeePermanentAd empperAdd) =>
            {
                result.IsSuccess = true;
                result.ReturnValue = empperAdd;
            }).ReturnsAsync(result);

            var controller = _controller;
            // Act
            //var attributes = controller.GetType().GetMethod("Put").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var response = await controller.Put(new EmployeePermanentAdModel());
            var okResult = response as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as EmployeePermanentAdModel).EmployeeId);
        }

        [Fact]
        public async Task UpdateProfile_Profile_Test()
        {
            // Arrange  
            TranSmart.Core.Result.Result<Employee> result = new();
            _updateservice.Setup(x => x.UpdateFromProfile(It.IsAny<Guid>(), It.IsAny<EmpProfileModel>()))//.Returns(result.ReturnValue =  Task.FromResult(result.ReturnValue = new Employee { ID = EmployeeId }))
           .Callback((Guid empId, EmpProfileModel emp) =>
           {
               result.IsSuccess = true;
               result.ReturnValue = new Employee { ID = EmployeeId };
           }).ReturnsAsync(result);

            var controller = _controller;

            // Act
            var attributes = controller.GetType().GetMethod("UpdateFromProfile").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var response = await controller.UpdateFromProfile(new EmpProfileModel());
            var okResult = response as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as EmpProfileModel).ID);
        }


        [Fact]
        public async Task UpdateProfile_PofileContact_Test()
        {
            // Arrange  
            TranSmart.Core.Result.Result<EmployeeFamily> result = new();
            _empfamilyservice.Setup(x => x.UpdateProfileContact(It.IsAny<EmployeeFamilyModel>())).Callback((EmployeeFamilyModel empfamily) =>
            {
                result.IsSuccess = true;
                result.ReturnValue = new EmployeeFamily { EmployeeId = EmployeeId };
            }).ReturnsAsync(result);

            var controller = _controller;
            // Act
            var attributes = controller.GetType().GetMethod("LocWeekOffSetup").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
            var response = await controller.LocWeekOffSetup(new EmployeeFamilyModel());
            var okResult = response as OkObjectResult;

            // Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal(EmployeeId, (okResult.Value as EmployeeFamilyModel).EmployeeId);
        }





		[Fact]
		public async Task DeleteContactTest()
		{
			// Arrange  
			TranSmart.Core.Result.Result<EmployeeFamily> result = new();
			_empfamilyservice.Setup(x => x.DeleteProfileContact(It.IsAny<Guid>())).Callback((Guid id) =>
			{
				result.IsSuccess = true;
				result.ReturnValue = new EmployeeFamily { EmployeeId = EmployeeId };
			}).ReturnsAsync(result);

		
			// Act
			var response = await _controller.DeleteContact(It.IsAny<Guid>());
			var okResult = response as OkObjectResult;

			// Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
			Assert.Equal(EmployeeId, (okResult.Value as EmployeeFamilyModel).EmployeeId);
		}
	}
}
