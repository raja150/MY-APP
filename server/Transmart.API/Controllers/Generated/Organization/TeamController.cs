using AutoMapper;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;
using TranSmart.Service.Organization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TranSmart.API.Extensions;
using System.Threading.Tasks;

namespace TranSmart.API.Controllers.Organization
{
    [Route("api/Organization/[controller]")]
    [ApiController]
    public partial class TeamController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ITeamService _service;
        public TeamController(IMapper mapper, ITeamService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] string OrderBy)
        {
            return Ok(_mapper.Map<IEnumerable<TeamList>>(await _service.GetList(OrderBy)));
        }

        [HttpGet("Paginate")]
        [ApiAuthorize(Core.Permission.Org_Team, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<TeamList>>(await _service.GetPaginate(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission.Org_Team, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<TeamModel>(await _service.GetById(id)));
        }

        [HttpPost]
        [ApiAuthorize(Core.Permission.Org_Team, Core.Privilege.Create)]
        public async Task<IActionResult> Post(TeamModel model)
        {
            Result<Team> result = await _service.AddAsync(_mapper.Map<Team>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<TeamModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
        [ApiAuthorize(Core.Permission.Org_Team, Core.Privilege.Update)]
        public async Task<IActionResult> Put(TeamModel model)
        {
            Result<Team> result = await _service.UpdateAsync(_mapper.Map<Team>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<TeamModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

    }
}
