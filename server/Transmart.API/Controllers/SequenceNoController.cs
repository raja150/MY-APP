using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TranSmart.Data.Paging;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Models;
using TranSmart.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TranSmart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SequenceNoController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ISequenceNoService _service;
        public SequenceNoController(IMapper mapper, ISequenceNoService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            return Ok(_mapper.Map<IEnumerable<SequenceNoModel>>(await _service.GetList("")));
        }

        [HttpPut]
        public IActionResult Put(List<SequenceNoModel> model)
        {
            List<SequenceNo> sequenceNos = _mapper.Map<List<SequenceNo>>(model);
            return Ok(_mapper.Map<Task<IEnumerable<SequenceNo>>>(_service.UpdateRange(sequenceNos)));
        }
    }
}
