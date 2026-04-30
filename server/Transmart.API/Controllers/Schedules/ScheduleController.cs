using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Transmart.TS4API;
using Transmart.TS4API.Models;
using TranSmart.Domain.Models.Schedules;
using TranSmart.Service.Schedules;

namespace TranSmart.API.Controllers.Schedules
{
    [Route("api/Schedule/[controller]")]
    [ApiController]
    public class ScheduleController : BaseController
    {
        private readonly IScheduleService _service;
        private readonly ITs4ApiS _apis;
        public ScheduleController(IScheduleService service, ITs4ApiS apis)
        {
            _service = service;
            _apis = apis;
        }

        [HttpGet("GetSchedules")]
        public async Task<List<ScheduleDetails>> GetSchedules(Guid BranchID,bool importToTS4)
        {

            var schedulelist = new List<ScheduleModel>();
			List<ScheduleDetails> list = await _service.GetSchedules(BranchID);
			foreach (var item in list)
            {
                schedulelist.Add(new ScheduleModel()
                {
                    TS4ID = item.EmployeeID,
                    ScheduleFrom = item.FromDate,
                    ScheduleTo = DateTime.Now,
                    InTime = item.StartAt,
                    OutTime = item.EndsAt,
                    OutNextDay = item.NextDayOut,
                    BreakTime = item.BreakTime,
                    NoOfBreaks = item.NoOfBreaks
                });
            }
			if (importToTS4)
			{
				await _apis.PostSchedules(schedulelist);
			}
            return list;
        }
    }
}
