using Microsoft.AspNetCore.Mvc;
using TranSmart.API.Extensions;
using TranSmart.API.Services;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Models;
using TranSmart.Service;

namespace TranSmart.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IUserService _service;
		private readonly ISsoService _ssoService;

		public UserController(IMapper mapper, IUserService service, ISsoService SsoService)
		{
			_mapper = mapper;
			_service = service;
			_ssoService = SsoService;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			return Ok(_mapper.Map<UserModel>(await _service.GetById(id)));
		}

		[HttpPut]
		[ApiAuthorize(Core.Permission.Org_Users, Core.Privilege.Update)]
		public async Task<IActionResult> Put(UserModel model)
		{
			var result = new Result<User>();
			if (model.EmployeeId == LOGIN_USER_EMPId)
			{
				result.AddMessageItem(new MessageItem("Self changes are not allowed"));
				return BadRequest(result);
			}
			result = await _service.UpdateAsync(_mapper.Map<User>(model));
			if (result.HasError)
			{
				return BadRequest(result);
			}
			if (!string.IsNullOrEmpty(model.Password))
			{
				var response = await _ssoService.AdminUpdate(new SsoUserModel
				{
					ExpireOn = DateTime.UtcNow.AddDays(-1),
					EmployeeId = model.EmployeeId,
					Name = model.Name,
					Password = model.Password,
				});
				if (response.HasError)
				{
					return BadRequest(response);
				}
			}
			return Ok(_mapper.Map<UserModel>(result.ReturnValue));
		}

		[HttpPut("reset")]
		[ApiAuthorize(Core.Permission.Org_Users, Core.Privilege.Update)]
		public async Task<IActionResult> Reset(UserModel model)
		{
			var result = new Result<User>();
			if (model.EmployeeId == LOGIN_USER_EMPId)
			{
				result.AddMessageItem(new MessageItem("Self changes are not allowed"));
				return BadRequest(result);
			}
			model.Password = "Welcome!2023";
			result = await _service.UpdateAsync(_mapper.Map<User>(model));
			if (result.HasError)
			{
				return BadRequest(result);
			}

			var response = await _ssoService.AdminUpdate(new SsoUserModel
			{
				ExpireOn = DateTime.UtcNow.AddDays(-1),
				EmployeeId = model.EmployeeId,
				Name = model.Name,
				Password = model.Password,
			});
			if (response.HasError)
			{
				return BadRequest(response);
			}

			return Ok(_mapper.Map<UserModel>(result.ReturnValue));
		}

		[HttpGet("Paginate")]
		public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
		{
			return Ok(_mapper.Map<Models.Paginate<UsersList>>(await _service.GetPaginate(baseSearch)));
		}
		[HttpPost]
		[ApiAuthorize(Core.Permission.Org_Users, Core.Privilege.Create)]
		public async Task<IActionResult> Post(UserModel model)
		{
			var entity = _mapper.Map<User>(model);
			Result<User> result = await _service.AddAsync(entity);
			if (result.HasError)
			{
				return BadRequest(result);
			}
			var response = await _ssoService.SSOAddUser(model);
			if (response.HasError)
			{
				return BadRequest(response);
			}
			return Ok(_mapper.Map<UserModel>(result.ReturnValue));
		}

		[HttpGet("History/{id}")]
		public async Task<IActionResult> GetUserAudit(Guid id)
		{
			return Ok(_mapper.Map<List<UserAuditModel>>(await _service.GetAuditHistory(id)));
		}
	}
}
