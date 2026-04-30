using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Domain.Models.Helpdesk;

namespace TranSmart.API.Controllers.Helpdesk
{
    public partial class HelpTopicSubController
    {
        [HttpGet("SubTopics/{helptopicId}")]

        public async Task<IActionResult> GetHelpTopicSubs(Guid helptopicId)
        {
            return Ok(_mapper.Map<IEnumerable<HelpTopicSubModel>>(await _service.GetHelpTopicSubs(helptopicId)));
        }
    }
}
