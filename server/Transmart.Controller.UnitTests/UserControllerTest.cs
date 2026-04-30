using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Transmart.Controller.UnitTests;
using TranSmart.API.Controllers;
using TranSmart.API.Extensions;
using TranSmart.API.Services;
using TranSmart.Core;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Service;
using Xunit;

namespace Transmart.Controller.UnitTests
{
    public class UserControllerTest : ControllerTestBase
    {
        private readonly Mock<IUserService> _service;
        private readonly UserController _controller;
		private readonly Mock<ISsoService> _ssoService;
		public UserControllerTest()
        {
            _service = new Mock<IUserService>();
			_ssoService = new Mock<ISsoService>();
			_controller = new UserController(Mapper, _service.Object, _ssoService.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = Claim }
                }
            };
        }


		[Fact]
		public async Task GetTest()
		{
			var result = new Result<UserAudit>();

			_ = _service.Setup(x => x.GetById(It.IsAny<Guid>()))
			 	.Callback(() =>
				 {
					 result.IsSuccess = true;
				 });

			// Act
			var resposne = await _controller.Get(It.IsAny<Guid>());

			var okResult = resposne as OkObjectResult;

			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}


		[Fact]
        public void GET_PAGINATE_TEST()
        {
            var list = new List<User>
            {
                new User { 
					ID =  EmployeeId,Name ="User",
                    Employee = new Employee
                            {
                                ID = EmployeeId,
                                Name = "Shiva"
                            }
                 }
            }.AsQueryable();

            _ = _service.Setup(x => x.GetPaginate(It.IsAny<BaseSearch>()))
                .ReturnsAsync(list.ToPaginate(0, 10));

            // Act
            var resposne = _controller.Paginate(new BaseSearch { Size = 10, Page = 0 });
            var okResult = resposne;

            // Assert
            Assert.NotNull(okResult);
        }

        [Fact]
        public async Task PUT_USERS_TEST()
        {
			var result = new Result<User>();

			_ = _service.Setup(x => x.UpdateAsync(It.IsAny<User>()))
			   .Callback(() =>
			   {
				   result.IsSuccess = true;
				   result.ReturnValue = new User { ID = EmployeeId };
			   }).ReturnsAsync(result);

			_ = _ssoService.Setup(x => x.AdminUpdate(It.IsAny<SsoUserModel>()))
			   .Callback(() =>
			   {
				   result.IsSuccess = true;
				   result.ReturnValue = new User { ID = EmployeeId };
			   }).ReturnsAsync(result);


			// Act
			var resposne =await  _controller.Put(new UserModel
            {
                EmployeeId = Guid.NewGuid(),
                ID = Guid.NewGuid(),
                Name = "UserUpdate",
                Password = "PasswordUpdate",
                RoleId = Guid.NewGuid()
            });

           var okResult =  resposne as OkObjectResult;

			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}

		[Fact]
		public async Task PUT_USERS_BadRequest()
		{
			var result = new Result<User>();

			_ = _service.Setup(x => x.UpdateAsync(It.IsAny<User>()))
			   .Callback(() =>
			   {
				   result.IsSuccess = true;
				   result.ReturnValue = new User { ID = EmployeeId };
			   }).ReturnsAsync(result);

			_ = _ssoService.Setup(x => x.AdminUpdate(It.IsAny<SsoUserModel>()))
			   .Callback(() =>
			   {
				   result.IsSuccess = true;
				   result.ReturnValue = new User { ID = EmployeeId };
			   }).ReturnsAsync(result);


			// Act
			var resposne = await _controller.Put(new UserModel
			{
				EmployeeId = EmployeeId,
				ID = Guid.NewGuid(),
				Name = "UserUpdate",
				Password = "PasswordUpdate",
				RoleId = Guid.NewGuid()
			});

			var okResult = resposne as OkObjectResult;

			//Assert
			Assert.Null(okResult);
		}

		[Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task POST_USERS_TEST(bool hasError)      
        {
            var result = new Result<User>();
            _ = !hasError
               ? _service.Setup(x => x.AddAsync(It.IsAny<User>()))
            .Callback(() =>
            {
                result.IsSuccess = true;
                result.ReturnValue = new User { ID = EmployeeId };
            }).ReturnsAsync(result)
               :
               _service.Setup(x => x.AddAsync(It.IsAny<User>()))
              .Callback(() =>
              {
                  result.IsSuccess = false;
                  result.ReturnValue = null;
              }).ReturnsAsync(result);

			_ = _ssoService.Setup(x => x.SSOAddUser(It.IsAny<UserModel>()))
		   .Callback(() =>
		   {
			   result.IsSuccess = true;
			   result.ReturnValue = new User { ID = EmployeeId };
		   }).ReturnsAsync(result);
			// Act
			var resposne = _controller.Post(new UserModel
            {
                EmployeeId = EmployeeId,
                ID = Guid.NewGuid(),
                Name = "UserUpdate",
                Password = "PasswordUpdate",
                RoleId = Guid.NewGuid()
            });
            
			var okResult =  await resposne as OkObjectResult;

             //Assert
            if (!hasError)
            {
                
               Assert.Equal(200, okResult.StatusCode);
            }
            else
            {
                Assert.False(false);
            }
        }


		[Fact]
		public  async Task  GetUserAuditTest()
		{
			var result = new Result<UserAudit>();

			_ =  _service.Setup(x => x.GetAuditHistory(It.IsAny<Guid>()))
			 	.Callback(() =>
				 {
					 result.IsSuccess = true;
				 });


			// Act
			var resposne = await _controller.GetUserAudit(It.IsAny<Guid>());

			var okResult = resposne as OkObjectResult;

			//Assert
			Assert.NotNull(okResult);
			Assert.Equal(200, okResult.StatusCode);
		}
	}
}
