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
    public partial class UserSettingsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IUserSettingsService _service;
        public UserSettingsController(IMapper mapper, IUserSettingsService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] string OrderBy)
        {
            return Ok(_mapper.Map<IEnumerable<UserSettingsList>>(await _service.GetList(OrderBy)));
        }

        [HttpGet("Paginate")]
        [ApiAuthorize(Core.Permission._UserSettings, Core.Privilege.Read)]
        public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
        {
            return Ok(_mapper.Map<Models.Paginate<UserSettingsList>>(await _service.GetPaginate(baseSearch)));
        }

        [HttpGet("{id}")]
        [ApiAuthorize(Core.Permission._UserSettings, Core.Privilege.Read)]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(_mapper.Map<UserSettingsModel>(await _service.GetById(id)));
        }

        [HttpPost]
        [ApiAuthorize(Core.Permission._UserSettings, Core.Privilege.Create)]
        public async Task<IActionResult> Post(UserSettingsModel model)
        {
            Result<UserSettings> result = await _service.AddAsync(_mapper.Map<UserSettings>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<UserSettingsModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut]
        [ApiAuthorize(Core.Permission._UserSettings, Core.Privilege.Update)]
        public async Task<IActionResult> Put(UserSettingsModel model)
        {
            Result<UserSettings> result = await _service.UpdateAsync(_mapper.Map<UserSettings>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<UserSettingsModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

    }
}
