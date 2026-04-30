using System.Security.Claims;
using System.Text;
using TranSmart.API.Models;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Enums;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.AppSettings;
using TranSmart.Service;
using TranSmart.Service.Organization;

namespace TranSmart.API.Services
{
	public class LocalAuthenticationService : IAuthenticationService
	{
		private readonly IUserService _service;
		private readonly ITokenService _tokenService;
		private readonly IEmployeeService _employeeService;
		public LocalAuthenticationService(IUserService service, ITokenService tokenService, IEmployeeService employeeService)
		{
			_service = service;
			_tokenService = tokenService;
			_employeeService = employeeService;
		}
		public async Task<object> Login(LoginModel login)
		{

			var entity = await _service.Validate(login.Username, login.Password);
			if (entity != null)
			{
				await _service.UpdateWebPunchIn(entity.EmployeeID);
				var employee = entity.EmployeeID.HasValue ? await _employeeService.GetById(entity.EmployeeID.Value) : null;
				var claims = new[]{
					new Claim("uid", entity.Name),
					new Claim("id", entity.ID.ToString()),
					new Claim("eid", entity.EmployeeID.HasValue ? entity.EmployeeID.Value.ToString():string.Empty),
					new Claim("did",  employee == null ? Guid.Empty.ToString() : employee.DepartmentId.ToString()),
					new Claim("desgId",  employee == null ? Guid.Empty.ToString() : employee.DesignationId.ToString()),
					new Claim(ClaimTypes.Role, entity.RoleId.ToString()),
				};
				if (entity.Type == (int)UserSts.Locked)
				{
					return new
					{
						ErrorCode = ErrorCodes.UserLocked
					};
				}
				//now verify the password expire or not
				if (entity.ExpireOn.Date < DateTime.Now.Date)
				{
					return new
					{
						jwtToken = _tokenService.GenerateAccessToken(claims)
					};
				}
				var success = new UserLoginLog { UserId = entity.ID, IPAddress = "123123144", LoginAt = DateTime.Now };
				_ = await _service.AddUserLoginLog(success);
				return new
				{
					jwtToken = _tokenService.GenerateAccessToken(claims),
					refreshToken = entity.RefreshToken,
					roleId = entity.RoleId,
					name = employee == null ? entity.Name : employee.Name,
					userId = entity.EmployeeID.HasValue ? entity.EmployeeID.Value.ToString() : string.Empty,

				};
			}
			var fail = new UserLoginFail { UserName = login.Username, IPAddress = "123123144", LoginAt = DateTime.Now };
			await _service.AddUserLoginFail(fail);
			
			return null;
		}
		public async Task<Result<TokenApiModel>> RefreshToken(TokenApiModel tokenApiModel)
		{
			var tokenResult = new Result<TokenApiModel>();
			Result<ClaimsPrincipal> resultToken = _tokenService.GetPrincipalFromExpiredToken(tokenApiModel.AccessToken);
			if (!resultToken.HasError)
			{
				_ = Guid.TryParse(resultToken.ReturnValue.Claims.FirstOrDefault(x => x.Type == "id").Value
					?? Guid.Empty.ToString(), out Guid userId);

				Result<User> result = await _service.Refresh(userId, tokenApiModel.RefreshToken);
				if (result.HasError)
				{
					tokenResult.AddMessageItem(new MessageItem(ErrMessages.Invalid_User_Or_Invalid_Refresh_Token));
					return tokenResult;
				}
				var refreshToken = new TokenApiModel()
				{
					AccessToken = _tokenService.GenerateAccessToken(resultToken.ReturnValue.Claims),
					RefreshToken = result.ReturnValue.RefreshToken
				};
				tokenResult.ReturnValue = refreshToken;
				return tokenResult;
			}
			else
			{
				return null;
			}
		}
		private static string EncodedText(string text)
		{
			return Encoding.UTF8.GetString(Convert.FromBase64String(text));
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
			return result;
		}

		public async Task<Result<User>> UpdatePassword(ChangePassword model, Guid userId)
		{
			var result = new Result<User>();
			if (string.IsNullOrEmpty(EncodedText(model.NewPassword).Trim()))
			{
				result.AddMessageItem(new MessageItem("Invalid password"));
				return result;
			}
			return await _service.UpdatePassword(userId, EncodedText(model.OldPassword), EncodedText(model.NewPassword));
		}
	}
}
