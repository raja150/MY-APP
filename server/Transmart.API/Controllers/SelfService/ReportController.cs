using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TranSmart.API.Extensions;
using TranSmart.Data;
 
using TranSmart.Service.SelfService;

namespace TranSmart.API.Controllers.SelfService
{
    [Route("api/SelfService/[controller]")]
    [ApiController]
    public class ReportController : BaseController
    {
        private readonly IReportService _service; 
        private readonly TranSmart.Service.Reports.ISelfService _selfService;

        public ReportController(IReportService service,
            TranSmart.Service.Reports.ISelfService selfService)
        {
            _service = service;
            _selfService = selfService;
        }

       
        [HttpGet("LeaveBalance")]
        [ApiAuthorize(Core.Permission.SS_Leavebalance, Core.Privilege.Read)]

        public async Task<IActionResult> LeaveBalance()
        {
            return Ok(await _selfService.LeaveBalance(LOGIN_USER_EMPId));
        }

        [HttpGet("EmployeeLeaves")]
        [ApiAuthorize(Core.Permission.SS_Leavebalance, Core.Privilege.Read)]

        public IActionResult GetEmployeeLeaves()
        {
            return Ok(_service.EmployeeLeaveBalance(LOGIN_USER_EMPId));
        }
        [HttpGet("EmployeeProfile")]
        [ApiAuthorize(Core.Permission.SS_Leavebalance, Core.Privilege.Read)]

        public IActionResult EmployeeProfile()
        {
            return Ok(_service.EmployeeProfile(LOGIN_USER_EMPId));
        }

    }
}