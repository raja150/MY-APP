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
    public partial class LocationController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ILocationService _service;
        public LocationController(IMapper mapper, ILocationService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] string OrderBy)
        {
            return Ok(_mapper.Map<IEnumerable<LocationList>>(await _service.GetList(OrderBy)));
        }

        [HttpGet("Paginate")]
        [ApiAuthorize(Core.Permission.Org_Location, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<LocationList>>(await _service.GetPaginate(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission.Org_Location, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<LocationModel>(await _service.GetById(id)));
        }

        [HttpPost]
        [ApiAuthorize(Core.Permission.Org_Location, Core.Privilege.Create)]
        public async Task<IActionResult> Post(LocationModel model)
        {
            Result<Location> result = await _service.AddAsync(_mapper.Map<Location>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<LocationModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
        [ApiAuthorize(Core.Permission.Org_Location, Core.Privilege.Update)]
        public async Task<IActionResult> Put(LocationModel model)
        {
            Result<Location> result = await _service.UpdateAsync(_mapper.Map<Location>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<LocationModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

    }
}
