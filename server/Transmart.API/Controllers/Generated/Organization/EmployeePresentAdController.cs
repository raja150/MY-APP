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
    public partial class EmployeePresentAdController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IEmployeePresentAdService _service;
        public EmployeePresentAdController(IMapper mapper, IEmployeePresentAdService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] string OrderBy)
        {
            return Ok(_mapper.Map<IEnumerable<EmployeePresentAdList>>(await _service.GetList(OrderBy)));
        }

        [HttpGet("Paginate")]
        [ApiAuthorize(Core.Permission.Org_Employee, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<EmployeePresentAdList>>(await _service.GetPaginate(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission.Org_Employee, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<EmployeePresentAdModel>(await _service.GetById(id)));
        }

        [HttpPost]
        [ApiAuthorize(Core.Permission.Org_Employee, Core.Privilege.Create)]
        public async Task<IActionResult> Post(EmployeePresentAdModel model)
        {
            Result<EmployeePresentAd> result = await _service.AddAsync(_mapper.Map<EmployeePresentAd>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<EmployeePresentAdModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
        [ApiAuthorize(Core.Permission.Org_Employee, Core.Privilege.Update)]
        public async Task<IActionResult> Put(EmployeePresentAdModel model)
        {
            Result<EmployeePresentAd> result = await _service.UpdateAsync(_mapper.Map<EmployeePresentAd>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<EmployeePresentAdModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

    }
}
