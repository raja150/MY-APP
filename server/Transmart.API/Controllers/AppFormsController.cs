using TranSmart.Data.Specifications;
using TranSmart.Domain.Entities;
using TranSmart.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TranSmart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppFormsController : BaseController
    {

        private readonly IBaseService<AppForm> _service;
        public AppFormsController(IBaseService<AppForm> service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            IEnumerable<AppForm> items = _service.GetBySpecification(new AppFormSpecification(id));
            if (items.Any())
            {
                return Ok(items.FirstOrDefault());
            }
            return BadRequest();
        }
    }
}
