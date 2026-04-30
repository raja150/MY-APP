
//using AutoMapper;
//using TranSmart.Core.Result;
//using TranSmart.Domain.Entities.AppSettings;
//using TranSmart.Domain.Models;
//using TranSmart.Domain.Models.AppSettings;
//using TranSmart.Service.AppSettings;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using TranSmart.Service.Reports;
//using System.Threading.Tasks;
//using TranSmart.Domain.Models.Settings;
//using TranSmart.API.Extensions;
//using TranSmart.API.Services;
//using Microsoft.AspNetCore.Http;
//using System.Linq;

//namespace TranSmart.API.Controllers
//{
//	[Route("api/[controller]")]
//	[ApiController]
//	public class RoleController : BaseController
//	{
//		private readonly IMapper _mapper;
//		private readonly IRolesService _service;
//		private readonly IAppReportService _reportService;
//		private readonly ICacheService _cacheService;

//		public RoleController(IMapper mapper, IRolesService service,
//			ICacheService cacheService,
//			IAppReportService reportService)
//		{
//			_mapper = mapper;
//			_service = service;
//			_cacheService = cacheService;
//			_reportService = reportService;
//		}

//		[HttpGet("Paginate")]
//		public async Task<TranSmart.API.Models.Paginate<RoleModel>> Paginate([FromQuery] BaseSearch baseSearch)
//		{
//			return (_mapper.Map<TranSmart.API.Models.Paginate<RoleModel>>(await _service.GetPaginate(baseSearch)));
//		}

//		[HttpGet("{id}")]
//		public async Task<IActionResult> Get(Guid id)
//		{
//			Role role = await _service.GetById(id);
//			RoleModel roleModel = _mapper.Map<RoleModel>(role);
//			roleModel.Pages = _mapper.Map<List<Domain.Models.RolePrivilegeModel>>(role.Pages);
//			roleModel.Reports = _mapper.Map<List<RoleReportPrivilegeModel>>(role.Reports);
//			return Ok(roleModel);
//		}

//		[HttpPost]
//		public async Task<IActionResult> Post(RoleModel model)
//		{
//			Result<Role> result = new();
//			if (!model.Pages.Any(x => x.Privilege > 0) && !model.Reports.Any(x => x.Privilege))
//			{
//				result.AddMessageItem(new MessageItem(ErrMessages.Please_Select_Any_One_Page));
//				return BadRequest(result);
//			}
//			Role role = _mapper.Map<Role>(model);
//			role.Pages = _mapper.Map<List<RolePrivilege>>(model.Pages);
//			role.Reports = _mapper.Map<List<RoleReportPrivilege>>(model.Reports);
//			result = await _service.AddAsync(role);
//			if (result.HasError)
//			{
//				return BadRequest(result);
//			}
//			_ = await _cacheService.UpdateRolePrivileges();
//			return Ok(_mapper.Map<RoleModel>(result.ReturnValue));
//		}

//		[HttpPut]
//		[ApiAuthorize(Core.Permission._Role, Core.Privilege.Update)]
//		[ProducesResponseType(StatusCodes.Status200OK)]
//		[ProducesResponseType(typeof(Result<Role>), StatusCodes.Status400BadRequest)]
//		[ProducesDefaultResponseType]
//		public async Task<IActionResult> Put(RoleModel model)
//		{
//			Result<Role> result = new();
//			if (!model.Pages.Any(x => x.Privilege > 0) && !model.Reports.Any(x => x.Privilege))
//			{
//				result.AddMessageItem(new MessageItem(ErrMessages.Please_Select_Any_One_Page));
//				return BadRequest(result);
//			}
//			Role role = _mapper.Map<Role>(model);
//			role.Pages = _mapper.Map<List<RolePrivilege>>(model.Pages);
//			role.Reports = _mapper.Map<List<RoleReportPrivilege>>(model.Reports);
//			result = await _service.UpdateAsync(role);
//			if (result.HasError)
//			{
//				return BadRequest(result);
//			}
//			_ = await _cacheService.UpdateRolePrivileges();
//			return Ok(result);
//		}

//		[HttpGet("Pages")]
//		[ApiAuthorize(Core.Permission._Role, Core.Privilege.Create)]
//		public async Task<IActionResult> GetPages()
//		{
//			return Ok(_mapper.Map<IEnumerable<TranSmart.Domain.Models.Page>>(await _service.Pages()));
//		}

//		[HttpGet("Reports")]
//		[ApiAuthorize(Core.Permission._Role, Core.Privilege.Create)]
//		public async Task<IActionResult> GetReports()
//		{
//			return Ok(_mapper.Map<IEnumerable<RoleReportPrivilegeModel>>(await _reportService.GetReports()));
//		}

//		[HttpGet("GetMenu/{roleId:guid}")]
//		public async Task<IActionResult> GetMenu(Guid roleId)
//		{
//			return Ok(await _cacheService.GetRoleMenu(roleId));
//		}

//		[HttpGet("GetMenuContent/{RoleId}")]
//		public async Task<IActionResult> GetMenuContent(Guid RoleId)
//		{
//			return Ok((await _service.GetById(RoleId)).Menu);
//		}

//		[HttpGet("Modules")]
//		public async Task<IActionResult> GetModules()
//		{
//			return Ok(_mapper.Map<List<TranSmart.Domain.Models.Group>>(await _service.GetModules()));
//		}

//		[HttpGet("GetReport/{GroupId}")]
//		public async Task<IActionResult> GetReport(Guid GroupId)
//		{
//			return Ok(_mapper.Map<List<TranSmart.Domain.Models.Report>>(await _service.GetReport(GroupId)));
//		}
//		[HttpGet("All")]
//		public async Task<IActionResult> GetAllRolles()
//		{
//			return Ok(_mapper.Map<List<RoleModel>>(await _service.Roles()));
//		}
//	}
//}
