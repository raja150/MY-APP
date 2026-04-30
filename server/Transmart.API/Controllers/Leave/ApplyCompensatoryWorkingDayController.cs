using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.API.Extensions;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave;
using TranSmart.Service.Leave;

namespace TranSmart.API.Controllers.Leave
{
    [Route("api/Leave/[controller]")]
    [ApiController]
    public class ApplyCompensatoryWorkingDayController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IApplyCompensatoryWorkingDayService _service;
        public ApplyCompensatoryWorkingDayController(IMapper mapper, IApplyCompensatoryWorkingDayService service)
        {
            _mapper = mapper;
            _service = service;
        }
        [HttpGet("Self")]
        public async Task<IActionResult> SelfServiceSearch([FromQuery] BaseSearch baseSearch)
        {
            baseSearch.RefId = LOGIN_USER_EMPId;
            return Ok(_mapper.Map<Models.Paginate<ApplyCompensatoryWorkingDayList>>(await _service.SelfServiceSearch(baseSearch)));

        }
        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] string OrderBy)
        {
            return Ok(_mapper.Map<IEnumerable<ApplyCompensatoryWorkingDayModel>>(await _service.GetList(OrderBy)));
        }

        [HttpGet("Paginate")]
        [ApiAuthorize(Core.Permission.SS_ApplyCompensatoryWorkingDay, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<ApplyCompensatoryWorkingDayList>>(await _service.GetPaginate(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission.SS_ApplyCompensatoryWorkingDay, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<ApplyCompensatoryWorkingDayModel>(await _service.GetById(id)));
        }

        [HttpPost]
        [ApiAuthorize(Core.Permission.SS_ApplyCompensatoryWorkingDay, Core.Privilege.Create)]
        public async Task<IActionResult> Post(ApplyCompensatoryWorkingDayModel model)
        {
            model.EmployeeId = LOGIN_USER_EMPId;
            Result<ApplyCompo> result = await _service.AddAsync(_mapper.Map<ApplyCompo>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<ApplyCompensatoryWorkingDayModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
        [ApiAuthorize(Core.Permission.SS_ApplyCompensatoryWorkingDay, Core.Privilege.Update)]
        public async Task<IActionResult> Put(ApplyCompensatoryWorkingDayModel model)
        {
            model.EmployeeId = LOGIN_USER_EMPId;
            Result<ApplyCompo> result = await _service.UpdateAsync(_mapper.Map<ApplyCompo>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<ApplyCompensatoryWorkingDayModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string name)
        {
            return Ok(_mapper.Map<IEnumerable<ApplyCompensatoryWorkingDayModel>>(await _service.Search(name)));
        }
    }
}
