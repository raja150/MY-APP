
using AutoMapper;
using TranSmart.Domain.Models;
using TranSmart.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TranSmart.Core.Result;
using TranSmart.API.Extensions;
using System.Threading.Tasks;

namespace TranSmart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupValuesController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ILookupService _service;
        public LookupValuesController(IMapper mapper, ILookupService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpPost] 
        public async Task<IActionResult> Post(LookUpValues model)
        {
            Result<TranSmart.Domain.Entities.LookUpValues> result = 
				await _service.AddAsync(_mapper.Map<TranSmart.Domain.Entities.LookUpValues>(model));
            if (result.IsSuccess)
            {
                return Ok(_mapper.Map<LookUpValues>(result.ReturnValue));
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> Get(string code)
        {
            return Ok(_mapper.Map<List<LookUpValues>>(await _service.Search(code)));
        }
    }
}
