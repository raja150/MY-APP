using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Data;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Service.Leave;

namespace TranSmart.API.Controllers.Leave
{
    [Route("api/Leave/[controller]")]
    [ApiController]
    public class LeaveAccumulationController : BaseController
    {
        private readonly ILeaveAccumulationService _service;
        public LeaveAccumulationController(ILeaveAccumulationService service)
        {
            _service = service;
        }

        [HttpPost("LeaveAccumulation")]
        public async Task<IActionResult> UpdateLeaves(DateTime ScheduleDate)
        {
            var result = await _service.LeavesAccumulationSchedule(ScheduleDate);
            return result.HasError ? BadRequest(result) : Ok(result);
        }

        [HttpGet("EmpLeaves/{empId}/{leaveTypeId}/{attDate}")]
        public async Task<IActionResult> GetEmpLeaves(Guid empId, Guid leaveTypeId,DateTime attDate)
        {
            return Ok(await _service.GetEmpLeaves(empId, leaveTypeId,attDate));
        }
    }
}
