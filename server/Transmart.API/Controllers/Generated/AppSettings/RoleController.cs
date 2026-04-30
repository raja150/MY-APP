using AutoMapper;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.AppSettings;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.AppSettings;
using TranSmart.Service.AppSettings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TranSmart.API.Extensions;
using System.Threading.Tasks;

namespace TranSmart.API.Controllers.AppSettings
{
    [Route("api/AppSettings/[controller]")]
    [ApiController]
    public partial class RoleController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IRoleService _service;
        public RoleController(IMapper mapper, IRoleService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] string OrderBy)
        {
            return Ok(_mapper.Map<IEnumerable<RoleList>>(await _service.GetList(OrderBy)));
        }

        [HttpGet("Paginate")]
        [ApiAuthorize(Core.Permission._Role, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<RoleList>>(await _service.GetPaginate(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission._Role, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<RoleModel>(await _service.GetById(id)));
        }

        [HttpPost]
        [ApiAuthorize(Core.Permission._Role, Core.Privilege.Create)]
        public async Task<IActionResult> Post(RoleModel model)
        {
            Result<Role> result = await _service.AddAsync(_mapper.Map<Role>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<RoleModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
        [ApiAuthorize(Core.Permission._Role, Core.Privilege.Update)]
        public async Task<IActionResult> Put(RoleModel model)
        {
            Result<Role> result = await _service.UpdateAsync(_mapper.Map<Role>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<RoleModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

    }
}
