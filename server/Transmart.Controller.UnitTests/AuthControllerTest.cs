using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TranSmart.API.Controllers;
using TranSmart.API.Extensions;
using TranSmart.API.Models;
using TranSmart.API.Services;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.AppSettings;
using TranSmart.Service;
using TranSmart.Service.Organization;
using Xunit;

namespace Transmart.Controller.UnitTests
{
	public class AuthControllerTest : ControllerTestBase
	{
		private readonly Mock<IUserService> _service;
		private readonly Mock<ITokenService> _tokenService;
		private readonly Mock<IEmployeeService> _empService;
		private readonly AuthController _controller;
		private readonly Mock<ILogger<AuthController>> _logger;
		private readonly Mock<IConfiguration> _configuration;
		private readonly Mock<IAuthenticationService> _authService;
		private readonly Mock<ISsoService> _SsoService;

		public AuthControllerTest() : base()
		{
			_service = new Mock<IUserService>();
			_tokenService = new Mock<ITokenService>();
			_empService = new Mock<IEmployeeService>();
			_logger = new Mock<ILogger<AuthController>>();
			_configuration = new Mock<IConfiguration>();
			_authService = new Mock<IAuthenticationService>();
			_SsoService = new Mock<ISsoService>();

			_controller = new AuthController(_logger.Object, _service.Object, _configuration.Object, _tokenService.Object, _authService.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext() { User = Claim }
				}
			};
		}
		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public async Task LOGIN(bool hasBadRequest)
		{
			if (hasBadRequest)
			{
				_ = _service.Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>()))
				 .ReturnsAsync((User)null);
			}
			else
			{
				_authService.Setup(x => x.Login(It.IsAny<LoginModel>())).ReturnsAsync(new object());
				_ = _service.Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync(new User { Name = "Avontix@1", EmployeeID = EmployeeId, ID = EmployeeId, RoleId = EmployeeId, Employee = new Employee { DepartmentId = EmployeeId, Department = new Department { ID = EmployeeId } } });
			}

			_ = _service.Setup(x => x.AddUserLoginLog(It.IsAny<UserLoginLog>()));


			_ = _empService.Setup(x => x.GetById(It.IsAny<Guid>())).Returns(Task.FromResult(new Employee { DepartmentId = EmployeeId }));

			_ = _controller.GetType().GetMethod("Login").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var resposne = await _controller.Login(new LoginModel
			{
				Username = "Avontix@1",
				Password = Base64StringEncode("Password@1")
			});

			_ = _empService.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Employee());
			_service.Setup(x => x.AddUserLoginFail(It.IsAny<UserLoginFail>()));
			POST_COMMON_CODE2(resposne, hasBadRequest);

		}


		[Theory]
		[InlineData(true, true, false, "", "")]
		[InlineData(true, false, false, "Active", "InActive")]
		[InlineData(false, false, false, "Active", "InActive")]
		public void REFRESH(bool hasBadRequest, bool tokenService, bool userService, string refreshToken, string accessToken)
		{
			var result = new Result<ClaimsPrincipal>();
			var user = new Result<User>();
			var token = new Result<TokenApiModel>();
			var claims = new List<Claim>()
			{
				new Claim("uid", EmployeeId.ToString()),
				new Claim("Username", "test"),
				new Claim("Role"," ")
			};
			var identity = new ClaimsIdentity(claims, "TestAuthType");
			var claimsPrincipal = new ClaimsPrincipal(identity);
			var users = new User()
			{
				ID = EmployeeId
			};

			if (hasBadRequest)
			{
				_ = _tokenService.Setup(x => x.GetPrincipalFromExpiredToken(It.IsAny<string>())).Callback(() =>
				{
					result.IsSuccess = tokenService;
					bool success = false;
					result.ReturnValue = claimsPrincipal;
					if (tokenService == success)
					{
						result.AddMessageItem(new MessageItem("Fails"));
					}

				}).Returns(result);
				_ = _service.Setup(x => x.Refresh(It.IsAny<Guid>(), It.IsAny<string>()))
				  .Callback(() =>
				  {
					  user.IsSuccess = userService;
					  user.ReturnValue = users;
				  }).ReturnsAsync(user);
			}
			else
			{
				_ = _tokenService.Setup(x => x.GetPrincipalFromExpiredToken(It.IsAny<string>()))
				   .Callback(() =>
				   {
					   result.IsSuccess = true;
					   result.ReturnValue = claimsPrincipal;
				   })
				   .Returns(result);
				_ = _service.Setup(x => x.Refresh(It.IsAny<Guid>(), It.IsAny<string>()))
					.Callback(() =>
					{
						user.IsSuccess = true;
						user.ReturnValue = users;
					}).ReturnsAsync(user);
				_authService.Setup(x => x.RefreshToken(It.IsAny<TokenApiModel>())).Callback((TokenApiModel token1) =>
				{
					token.IsSuccess = true;
					token.ReturnValue = token1;

				}).ReturnsAsync(token);
			}
			var model = new TokenApiModel { RefreshToken = refreshToken, AccessToken = accessToken };

			_ = _controller.GetType().GetMethod("Refresh").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var resposne = _controller.Refresh(model.AccessToken == "" ? null : model);

			POST_COMMON_CODE(resposne, hasBadRequest);

		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void UpdatePassword(bool hasBadRequest)
		{
			var result = new Result<User>();
			if (hasBadRequest)
			{
				_authService.Setup(x => x.UpdatePassword(It.IsAny<ChangePassword>(), It.IsAny<Guid>())).Callback(() =>
				{
					result.IsSuccess = false;
					result.AddMessageItem(new MessageItem("Invalid password"));
				}).ReturnsAsync(result);
			}
			else
			{
				_authService.Setup(x => x.UpdatePassword(It.IsAny<ChangePassword>(), It.IsAny<Guid>())).Callback(() =>
				{
					result.IsSuccess = true;
					result.ReturnValue = new User { EmployeeID = EmployeeId, Name = "AVONTIX2063" };
				}).ReturnsAsync(result);
			}
			_ = _controller.GetType().GetMethod("UpdatePassword").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var resposne = _controller.UpdatePassword(new ChangePassword
			{
				Name = "Avontix@1",
				OldPassword = Base64StringEncode("Shiva@1"),
				NewPassword = Base64StringEncode("Mahesh@123")

			});

			POST_COMMON_CODE(resposne, hasBadRequest);
		}

		//ChangePwd
		[Theory]
		[InlineData(true, "Mahesh@123")]
		[InlineData(false, "Mahesh@123")]
		[InlineData(false, "")]
		public async Task ChangePassword(bool hasBadRequest, string password)
		{
			var result = new Result<ClaimsPrincipal>();
			var user = new Result<User>();

			if (hasBadRequest)
			{
				_authService.Setup(x => x.ChangePassword(It.IsAny<PasswordExpModel>())).Callback(() =>
				{
					user.IsSuccess = false;
					user.AddMessageItem(new MessageItem("Invalid password"));

				}).ReturnsAsync(user);
			}
			else
			{
				_authService.Setup(x => x.ChangePassword(It.IsAny<PasswordExpModel>())).Callback(() =>
				{
					user.IsSuccess = true;
					user.ReturnValue = new User { Name = "AVONTIX2063", EmployeeID = EmployeeId };

				}).ReturnsAsync(user);
			}
			_ = _controller.GetType().GetMethod("UpdatePassword").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var resposne = _controller.ChangePassword(new PasswordExpModel
			{
				UserName = "Avontix@1",
				NewPassword = Base64StringEncode(password)
			});

			var okResult = await resposne as OkObjectResult;

			if (hasBadRequest)
			{
				Assert.Null(okResult);
			}
			else
			{
				Assert.NotNull(okResult);
			}
		}


		//ResetPwd
		[Theory]
		[InlineData(true, "Mahesh@123")]      //Invalid user
		[InlineData(false, "Mahesh@123")]
		public async Task ResetPwd(bool hasBadRequest, string password)
		{
			var result = new Result<ClaimsPrincipal>();
			var user = new Result<User>();

			if (hasBadRequest)
			{
				_authService.Setup(x => x.ResetPassword(It.IsAny<ResetPwd>())).Callback(() =>
				{
					user.IsSuccess = false;
					user.AddMessageItem(new MessageItem("Invalid password"));
				}).ReturnsAsync(user);
			}
			else
			{
				_authService.Setup(x => x.ResetPassword(It.IsAny<ResetPwd>())).Callback(() =>
				{
					user.IsSuccess = true;
					user.ReturnValue = new User { EmployeeID = EmployeeId, Name = "AVONTIX2063" };
				}).ReturnsAsync(user);
			}
			_ = _controller.GetType().GetMethod("ResetPwd").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var resposne = _controller.ResetPwd(new ResetPwd
			{
				UserId = EmployeeId,
				NewPassword = Base64StringEncode(password),
				Token = "iOQ4Il2omMR/n6xltLX5N9PPO7m/fcWQE3ffxbu6Cds=",
				ID = EmployeeId
			});

			var okResult = await resposne as OkObjectResult;

			if (hasBadRequest)
			{
				Assert.Null(okResult);
			}
			else
			{
				Assert.NotNull(okResult);
				Assert.Equal(200, okResult.StatusCode);
			}
		}

		//Forget
		[Theory]
		[InlineData(true, "Mahesh@123")]
		[InlineData(false, "Mahesh@123")]
		[InlineData(false, "")]
		public async Task MailCheck(bool hasBadRequest, string password)
		{
			var result = new Result<ClaimsPrincipal>();

			if (hasBadRequest)
			{
				_ = _service.Setup(x => x.GetById(It.IsAny<Guid>())).Callback(() =>
				{
					result.IsSuccess = false;
					result.AddMessageItem(new MessageItem("Invalid password"));
				});
			}
			else
			{
				_ = _service.Setup(x => x.CheckUser(It.IsAny<string>())).Callback(() =>
				{
					result.IsSuccess = true;
				}).ReturnsAsync(new User { ID = EmployeeId, Password = Base64StringEncode(password), AddedAt = DateTime.Now, Employee = new Employee { WorkEmail = "shiva123@gmail.com" } });
			}

			var claims = new List<Claim>()
			{
				new Claim("id", EmployeeId.ToString()),
				new Claim("Username", "test"),
				new Claim("Role"," ")
			};
			var identity = new ClaimsIdentity(claims, "TestAuthType");
			var claimsPrincipal = new ClaimsPrincipal(identity);

			_ = _tokenService.Setup(x => x.GenerateAccessToken(It.IsAny<IEnumerable<Claim>>(), It.IsAny<string>()))
				   .Callback(() =>
				   {
					   result.IsSuccess = true;
					   result.ReturnValue = claimsPrincipal;
				   }).Returns("iOQ4Il2omMR/n6xltLX5N9PPO7m/fcWQE3ffxbu6Cds=");

			_ = _controller.GetType().GetMethod("ResetPwd").GetCustomAttributes(typeof(ApiAuthorizeAttribute), true);
			var resposne = _controller.MailCheck(new ForgetPwdModel
			{
				UserName = "Avontix@1",
				AppUrl = ""
			});

			var okResult = await resposne as OkObjectResult;

			if (hasBadRequest)
			{
				Assert.Null(okResult);

			}
			else
			{
				Assert.NotNull(okResult);
				Assert.Equal(200, okResult.StatusCode);
			}
		}


		public static string Base64StringEncode(string originalString)
		{
			var bytes = Encoding.UTF8.GetBytes(originalString);

			var encodedString = Convert.ToBase64String(bytes);

			return encodedString;
		}

		private static void POST_COMMON_CODE(dynamic resposne, bool hasBadRequest)
		{
			var okResult = resposne as OkObjectResult;
			if (hasBadRequest)
			{
				Assert.Null(okResult);
				Assert.Equal(400, resposne.Result.StatusCode);
			}
			else
			{
				Assert.NotNull(resposne.Result);
				Assert.Equal(200, resposne.Result.StatusCode);
			}
		}



		private static void POST_COMMON_CODE2(dynamic resposne, bool hasBadRequest)
		{
			var okResult = resposne as OkObjectResult;
			if (hasBadRequest)
			{
				Assert.Null(okResult);
				Assert.Equal(400, resposne.StatusCode);
			}
			else
			{
				Assert.NotNull(resposne);
				Assert.Equal(200, resposne.StatusCode);
			}
		}
	}
}
