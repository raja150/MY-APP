using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using TranSmart.API.Models;
using TranSmart.API.Services;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.AppSettings;
using TranSmart.Service;

namespace TranSmart.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly ILogger _logger;
		private readonly IUserService _service;
		private readonly ITokenService _tokenService;
		private readonly IAuthenticationService _authService;

		public IConfiguration Configuration { get; }
		public AuthController(ILogger<AuthController> logger, IUserService service,
			IConfiguration configuration, ITokenService tokenService,
			IAuthenticationService authService)
		{
			_logger = logger;
			_service = service ?? throw new ArgumentNullException(nameof(service));
			Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			_tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
			_authService = authService;
		}

		[HttpGet]
		public IActionResult VersionNo()
		{
			return Ok(new { No = "1.0.1428" });
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginModel login)
		{
			_logger.LogDebug("AuthController entered");
			login.Password = EncodedText(login.Password);
			var result = await _authService.Login(login);
			if (result != null)
			{
				return Ok(result);
			}
			_logger.LogDebug("AuthController left");
			return BadRequest(result);
		}


		[HttpPost]
		[Route("Refresh")]
		public async Task<IActionResult> Refresh(TokenApiModel tokenApiModel)
		{
			if (tokenApiModel is null)
			{
				return BadRequest();
			}
			var result = await _authService.RefreshToken(tokenApiModel);
			return result != null && result.HasNoError ? Ok(result.ReturnValue) : BadRequest(result);
		}

		[HttpPut("SendMail")]
		public async Task<IActionResult> MailCheck(ForgetPwdModel model)
		{
			var item = await _service.CheckUser(model.UserName);
			if (item != null)
			{
				var claims = new[] { new Claim("id", item.ID.ToString()) };
				string token = _tokenService.GenerateAccessToken(claims, item.Password + item.AddedAt.Ticks.ToString());
				await MailService.MailSending(item.ID.ToString(), token, item.Employee.WorkEmail, model.AppUrl);
				return Ok(item);
			}
			else
			{
				return BadRequest("Invalid user");
			}
		}

		[HttpPut]
		[Authorize] //User box  popup
		public async Task<IActionResult> UpdatePassword(ChangePassword model)
		{
			var result = new Result<User>();
			if (string.IsNullOrEmpty(EncodedText(model.NewPassword).Trim()))
			{
				result.AddMessageItem(new MessageItem("Invalid password"));
				return BadRequest(result);
			}

			var userId = User.Claims.FirstOrDefault(x => x.Type == "id").Value ?? Guid.Empty.ToString();
			result = await _authService.UpdatePassword(model, Guid.Parse(userId));
			if (result.HasError)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}


		[HttpPut("ChangePwd")]//Expire Password
		public async Task<IActionResult> ChangePassword(PasswordExpModel model)
		{
			var result = await _authService.ChangePassword(model);
			if (result.HasError)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

		[HttpPut("ResetPwd")]//from Mail link
		public async Task<IActionResult> ResetPwd(ResetPwd model)
		{
			var result = await _authService.ResetPassword(model);
			if (result.HasError)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

		private static string EncodedText(string text)
		{
			return Encoding.UTF8.GetString(Convert.FromBase64String(text));
		}
	}
}
