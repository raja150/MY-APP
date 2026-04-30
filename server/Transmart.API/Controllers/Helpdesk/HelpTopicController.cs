using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Domain.Models.Helpdesk;

namespace TranSmart.API.Controllers.Helpdesk
{
  
    public partial class HelpTopicController : BaseController
    {
        [HttpGet("Topics/{deptId}")]

        public async Task<IActionResult> GetHelpTopics(Guid deptId)
        {
            return Ok(_mapper.Map<IEnumerable<HelpTopicModel>>(await _service.GetHelpTopics(deptId)));
        }
    }
}
