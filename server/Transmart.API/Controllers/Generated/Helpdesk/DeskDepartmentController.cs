using AutoMapper;
using TranSmart.Core.Result;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Helpdesk;
using TranSmart.Service.Helpdesk;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TranSmart.API.Extensions;
using System.Threading.Tasks;

namespace TranSmart.API.Controllers.Helpdesk
{
    [Route("api/Helpdesk/[controller]")]
    [ApiController]
    public partial class DeskDepartmentController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IDeskDepartmentService _service;
        public DeskDepartmentController(IMapper mapper, IDeskDepartmentService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] string OrderBy)
        {
            return Ok(_mapper.Map<IEnumerable<DeskDepartmentList>>(await _service.GetList(OrderBy)));
        }

        [HttpGet("Paginate")]
        [ApiAuthorize(Core.Permission.HD_DeskDepartment, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<DeskDepartmentList>>(await _service.GetPaginate(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission.HD_DeskDepartment, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<DeskDepartmentModel>(await _service.GetById(id)));
        }

        [HttpPost]
        [ApiAuthorize(Core.Permission.HD_DeskDepartment, Core.Privilege.Create)]
        public async Task<IActionResult> Post(DeskDepartmentModel model)
        {
            Result<DeskDepartment> result = await _service.AddAsync(_mapper.Map<DeskDepartment>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<DeskDepartmentModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
        [ApiAuthorize(Core.Permission.HD_DeskDepartment, Core.Privilege.Update)]
        public async Task<IActionResult> Put(DeskDepartmentModel model)
        {
            Result<DeskDepartment> result = await _service.UpdateAsync(_mapper.Map<DeskDepartment>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<DeskDepartmentModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

    }
}
