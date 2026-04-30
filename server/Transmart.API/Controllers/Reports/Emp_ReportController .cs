using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TranSmart.Data;
using TranSmart.Service.Reports;
    
namespace TranSmart.API.Controllers.Reports 
{
    [Route("api/Organization/[controller]")]
    [ApiController]
    public class EmpReportController : BaseController
    {
       
        private readonly IEmpReportService _service;
        public EmpReportController(IEmpReportService service)
        {
            _service = service; 

        }
        [HttpGet("EmployeeProfile")]
        public async Task<IActionResult> EmployeeProfile()
        {
            return Ok(await _service.EmployeeProfile());
        }
        [HttpGet("LeaveBalance")]
        public async Task<IActionResult> EmployeeLeaveBalance()
        {
            return Ok(await _service.EmployeeLeaveBalance());
        }
    }
}