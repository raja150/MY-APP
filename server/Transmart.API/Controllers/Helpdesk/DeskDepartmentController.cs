using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranSmart.Domain.Entities.Helpdesk;
using TranSmart.Domain.Models.HelpDesk.Responce;

namespace TranSmart.API.Controllers.Helpdesk
{
   
    public partial class DeskDepartmentController : BaseController
    {
        [HttpGet("GetDepList")] 
        public async Task<IActionResult> GetDepLists()
        {
            return Ok(_mapper.Map<IEnumerable<DeskDepartmentModel>>(await _service.GetDepartments()));
        }
    }
}
