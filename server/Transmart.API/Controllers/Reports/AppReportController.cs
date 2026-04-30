using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Services; 
using TranSmart.Service.Reports;

namespace TranSmart.API.Controllers.Reports
{
	[Route("api/[controller]")]
	[ApiController]
	public class AppReportController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IAppReportService _service;
		private readonly ICacheService _cacheService;
		public AppReportController(IMapper mapper, IAppReportService service, ICacheService cacheService)
		{
			_mapper = mapper;
			_service = service;
			_cacheService = cacheService;
		}

		[HttpGet("Modules")]
		public async Task<IActionResult> Modules()
		{
			return Ok(await _service.GetModules());
		}

		[HttpGet("Reports/{id:Guid}")]
		public async Task<IActionResult> GetReport([FromRoute] Guid id)
		{
			return Ok(_mapper.Map<List<TranSmart.Domain.Models.Report>>(await _service.GetReports(id)));
		}

		[HttpGet("RoleReports/{moduleId:Guid}/{roleId:Guid}")]
		public async Task<IActionResult> GetRoleReports([FromRoute] Guid moduleId, [FromRoute] Guid roleId)
		{
			return Ok(await _cacheService.GetReportsMenu(moduleId, roleId));
		}


		[HttpGet("RoleReportModule/{roleId:Guid}")]
		public async Task<IActionResult> RoleReportModule([FromRoute] Guid roleId)
		{
			return Ok(await _cacheService.GetRoleReportModule( roleId));
		}
	}
}
