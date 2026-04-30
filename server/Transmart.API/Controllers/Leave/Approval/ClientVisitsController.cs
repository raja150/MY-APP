
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TranSmart.API.Extensions;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Leave;
using TranSmart.Domain.Models.Leave;
using TranSmart.Domain.Models.Leave.Approval;
using TranSmart.Domain.Models.Leave.Search;
using TranSmart.Domain.Models.SelfService;
using TranSmart.Service.Leave.Approval;

namespace TranSmart.API.Controllers.Leave.Approval
{
    [Route("api/Leave/Approval/[controller]")]
    [ApiController]
    public class ClientVisitController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IApprovalClientVisitsService _service;
        public ClientVisitController(IMapper mapper, IApprovalClientVisitsService service)
        {
            _mapper = mapper;
            _service = service;
        }
        [HttpGet]
        [ApiAuthorize(Core.Permission.SA_ClientVisitApplications, Core.Privilege.Read)]
        public async Task<IActionResult> Approval([FromQuery] ApplyClientVisitSearch baseSearch)
        {
            baseSearch.RefId = LOGIN_USER_EMPId;
            return Ok(_mapper.Map<Models.Paginate<ApplyClientVisitsList>>(await _service.ClientVisit(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission.SA_ClientVisitApplications, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<ClientVisitInfoModel>(await _service.GetClientVisit(id, LOGIN_USER_EMPId)));
        }

        //[HttpPut]
        //[ApiAuthorize(Core.Permission.SA_ClientVisitApplications, Core.Privilege.Read)]
		//public async Task<IActionResult> Put(ApproveRequest model)
		//{
		//    var result = model.IsApproved? await _service.Approve(model.ID,LOGIN_USER_EMPId, model.IsApproved, model.RejectReason);
		//    return result.IsSuccess ? Ok(_mapper.Map<ApplyClientVisitsModel>(result.ReturnValue)) : BadRequest(result);
		//}

		[HttpPut]
		[ApiAuthorize(Core.Permission.SA_ClientVisitApplications, Core.Privilege.Read)]
		public async Task<IActionResult> ApprovalPut(ApproveRequest model)
		{
			Result<ApplyClientVisits> result;
			result = model.IsApproved ?
				await _service.Approve(model.ID, LOGIN_USER_EMPId, false) :
				await _service.Reject(model.ID, model.RejectReason, LOGIN_USER_EMPId);

			return result.IsSuccess ? Ok(_mapper.Map<ApplyClientVisitsModel>(result.ReturnValue)) : BadRequest(result);
		}
	}
}
