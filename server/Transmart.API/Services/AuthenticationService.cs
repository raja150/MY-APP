using System.Net;
using System.Security.Claims;
using System.Text;
using TranSmart.API.Models;
using TranSmart.Core.Result;
using TranSmart.Core.Util;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.AppSettings;
using TranSmart.Service;
#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace TranSmart.API.Services
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
	public interface IAuthenticationService
	{
		Task<object> Login(LoginModel login);
		Task<Result<TokenApiModel>> RefreshToken(TokenApiModel tokenApiModel);
		Task<Result<User>> ChangePassword(PasswordExpModel model);
		Task<Result<User>> ResetPassword(ResetPwd model);
		Task<Result<User>> UpdatePassword(ChangePassword model, Guid userId);

	}
#pragma warning disable S3881 // "IDisposable" should be implemented correctly
	public class AuthenticationService : IAuthenticationService, IDisposable
#pragma warning restore S3881 // "IDisposable" should be implemented correctly
	{
		private readonly IConfiguration _configuration;
		private readonly IUserService _service;
		private readonly ITokenService _tokenService;
		private readonly HttpClient client;
		private readonly ISsoService _ssoService;
		public AuthenticationService(IConfiguration configuration,
			IUserService service, ITokenService tokenService, ISsoService ssoService)
		{
			_configuration = configuration;
			_service = service;
			_tokenService = tokenService;
			client = new HttpClient();
			client.DefaultRequestHeaders.Add(StringUtil.APIKey, _configuration["ApiKey"]);
			_ssoService = ssoService;
		}

		public async Task<object> Login(LoginModel login)
		{
			HttpResponseMessage response = await client.PostAsJsonAsync(_configuration["SSOApi"] + "/Auth/Login", login);

			if (response.StatusCode == HttpStatusCode.OK)
			{
				var data = await SsoData.GetUserInfo(response);
				//Invalid User
				if (data.ErrorCode == ErrorCodes.UserNotFound)
				{
					return null;
				}
				//Wrong Attempts greater than 5
				else if (data.ErrorCode == ErrorCodes.UserLocked)
				{
					return new
					{
						data.ErrorCode,
					};
				}
				User entity;
				//Exist is SSO, but Employee Id is Guid.Empty then check
				//that user exist or not in Local
				if (data.EmployeeId == Guid.Empty && (data.ErrorCode == ErrorCodes.UserValid
					|| data.ErrorCode == ErrorCodes.PasswordExpired))
				{
					entity = await _service.CheckUser(login.Username);
				}
				else
				{
					entity = await _service.GetUserById(data.EmployeeId);
				}
				if (entity == null)
				{
					return null;
				}
				//Password expire
				else if (data.ErrorCode == ErrorCodes.PasswordExpired)
				{
					return new { jwtToken = _tokenService.GenerateAccessToken(GetClaims(entity)) };
				}
				await _service.UpdateLastLogin(entity.ID);
				await _service.UpdateWebPunchIn(entity.EmployeeID);
				return new
				{
					jwtToken = _tokenService.GenerateAccessToken(GetClaims(entity)),
					refreshToken = data.RefreshToken,
					roleId = entity.RoleId,
					name = entity.Employee != null ? entity.Employee.Name : entity.Name,
					userId = entity.EmployeeID.HasValue ? entity.EmployeeID.Value.ToString() : string.Empty,
				};
			}
			return null;
		}

		public async Task<Result<TokenApiModel>> RefreshToken(TokenApiModel tokenApiModel)
		{
			var result = new Result<TokenApiModel>();
			Result<ClaimsPrincipal> resultToken = _tokenService.GetPrincipalFromExpiredToken(tokenApiModel.AccessToken);
			if (!resultToken.HasError)
			{
				tokenApiModel.Name = resultToken.ReturnValue.Claims.FirstOrDefault(x => x.Type == "uid").Value;

				HttpResponseMessage response = await client.PostAsJsonAsync(_configuration["SSOApi"] + "/Auth/Refresh", tokenApiModel);
				if (response.StatusCode == HttpStatusCode.OK)
				{
					var dd = new TokenApiModel()
					{
						AccessToken = _tokenService.GenerateAccessToken(resultToken.ReturnValue.Claims),
						RefreshToken = await SsoData.GetRefreshToken(response),
					};
					result.ReturnValue = dd;
					return result;
				}
				return null;
			}
			return null;
		}
		private static IEnumerable<Claim> GetClaims(User entity)
		{
			var claims = new[]{
						new Claim("id", entity.ID.ToString()),
						new Claim("uid", entity.Name),
						new Claim("eid", entity.EmployeeID.HasValue ? entity.EmployeeID.Value.ToString() : string.Empty),
						new Claim(ClaimTypes.Role, entity.RoleId.ToString()),
						new Claim("did",  entity.Employee == null ? Guid.Empty.ToString() : entity.Employee.DepartmentId.ToString()),
						new Claim("desgId",  entity.Employee == null ? Guid.Empty.ToString() : entity.Employee.DesignationId.ToString()),

					};
			return claims;
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
		public async Task<Result<User>> ChangePassword(PasswordExpModel model)
		{
			var result = new Result<User>();
			if (string.IsNullOrEmpty(EncodedText(model.NewPassword).Trim()))
			{
				result.AddMessageItem(new MessageItem(ErrMessages.Password_Required));
			}
			User user = null;
			Result<ClaimsPrincipal> resultToken = _tokenService.GetPrincipalFromExpiredToken(model.Token);

			var data = resultToken.ReturnValue.Claims.FirstOrDefault(x => x.Type == "eid" || x.Type == "EGuid");
			var name = resultToken.ReturnValue.Claims.FirstOrDefault(x => x.Type == "uid");

			if (string.IsNullOrEmpty(data?.Value))
			{
				user = await _service.CheckUser(name?.Value);
			}
			else
			{
				user = await _service.GetUserById(Guid.Parse(data?.Value));
			}

			if (user == null)
			{
				result.AddMessageItem(new MessageItem(ErrMessages.Invalid_User));
				return result;
			}
			user.Password = EncodedText(model.NewPassword);
			user.ExpireOn = DateTime.Now.AddDays(30);
			result = await _service.UpdateUserPwd(user);
			if (!result.HasError)
			{
				return await SsoUpdateUser(result.ReturnValue, model.NewPassword);
			}
			return result;
		}
		public async Task<Result<User>> ResetPassword(ResetPwd model)
		{
			var result = new Result<User>();
			if (string.IsNullOrEmpty(model.NewPassword.Trim()))
			{
				result.AddMessageItem(new MessageItem(ErrMessages.Password_Required));
				return result;
			}
			User user = await _service.GetById(model.UserId);
			if (user == null)
			{
				result.AddMessageItem(new MessageItem(ErrMessages.Invalid_User));
				return result;
			}
			//Here we are checking that Token is expired or not
			bool validToken = _tokenService.IsTokenExpired(model.Token, user.Password + user.AddedAt.Ticks.ToString());
			if (!validToken)
			{
				result.AddMessageItem(new MessageItem(ErrMessages.Reset_Password_Link_Expired));
				return result;
			}
			user.Password = EncodedText(model.NewPassword);
			user.ExpireOn = DateTime.Now.AddMonths(1);
			result = await _service.UpdateUserPwd(user);
			if (!result.HasError)
			{
				return await SsoUpdateUser(result.ReturnValue, model.NewPassword);
			}
			return result;
		}
		private static string EncodedText(string text)
		{
			return Encoding.UTF8.GetString(Convert.FromBase64String(text));
		}

		private async Task<Result<User>> SsoUpdateUser(User user, string NewPassword)
		{
			return await _ssoService.SSOUpdateUser(new SsoUserModel
			{
				EmployeeId = user.EmployeeID ?? Guid.Empty,
				Name = user.Name,
				Password = EncodedText(NewPassword),
				ExpireOn = DateTime.UtcNow.AddMonths(1),
			});
		}

		public async Task<Result<User>> UpdatePassword(ChangePassword model, Guid userId)
		{
			var result = new Result<User>();
			if (string.IsNullOrEmpty(EncodedText(model.NewPassword).Trim()))
			{
				result.AddMessageItem(new MessageItem("Invalid password"));
				return result;
			}
			result = await _service.UpdatePassword(userId, EncodedText(model.OldPassword), EncodedText(model.NewPassword));
			if (result.HasError)
			{
				return result;
			}
			//Updating SSO 
			return await SsoUpdateUser(result.ReturnValue, model.NewPassword);
		}

	}
}
