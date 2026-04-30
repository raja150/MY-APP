using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TranSmart.API.Extensions;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Leave;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Service.Approval;

namespace TranSmart.API.Controllers.Leave.Approval
{
    [Route("api/Leave/Approval/[controller]")]
    [ApiController]
    public class CompensatoryDayController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IApprovalCompensatoryWorkingDayService _service;
        public CompensatoryDayController(IMapper mapper, IApprovalCompensatoryWorkingDayService service)
        {
            _mapper = mapper;
            _service = service;
        }
        [HttpGet]
        [ApiAuthorize(Core.Permission.SA_CompApplications, Core.Privilege.Read)]
        public async  Task<IActionResult> PaginateEmployee([FromQuery] StatusSearch baseSearch)
        {
            baseSearch.RefId = LOGIN_USER_EMPId;
            return Ok(_mapper.Map<Models.Paginate<ApplyCompensatoryWorkingDayList>>(await _service.GetAllList(baseSearch)));
        }
        [HttpPut]
        [ApiAuthorize(Core.Permission.SA_CompApplications, Core.Privilege.Read)]
        public async Task<IActionResult> Put(ApplyCompensatoryWorkingDayModel model)
        {
            Result<ApplyCompo> result =await _service.UpdateAsync(_mapper.Map<ApplyCompo>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<ApplyCompensatoryWorkingDayModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }
    }
}
