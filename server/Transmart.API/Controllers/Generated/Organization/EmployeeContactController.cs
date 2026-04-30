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
    public partial class EmployeeContactController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeContactService _service;
        public EmployeeContactController(IMapper mapper, IEmployeeContactService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] string OrderBy)
        {
            return Ok(_mapper.Map<IEnumerable<EmployeeContactModel>>(await _service.GetList(OrderBy)));
        }

        [HttpGet("Paginate")]
        [ApiAuthorize(Core.Permission.Org_Employee, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<EmployeeContactList>>(await _service.GetPaginate(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission.Org_Employee, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<EmployeeContactModel>(await _service.GetById(id)));
        }

        [HttpPost]
        [ApiAuthorize(Core.Permission.Org_Employee, Core.Privilege.Create)]
        public async Task<IActionResult> Post(EmployeeContactModel model)
        {
            Result<EmployeeContact> result = await  _service.AddAsync(_mapper.Map<EmployeeContact>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<EmployeeContactModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
        [ApiAuthorize(Core.Permission.Org_Employee, Core.Privilege.Update)]
        public async Task<IActionResult> Put(EmployeeContactModel model)
        {
            Result<EmployeeContact> result = await _service.UpdateAsync(_mapper.Map<EmployeeContact>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<EmployeeContactModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string name)
        {
            return Ok(_mapper.Map<IEnumerable<EmployeeContactModel>>(_service.Search(name)));
        }
    }
}
