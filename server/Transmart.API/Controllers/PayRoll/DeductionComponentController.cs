using Microsoft.AspNetCore.Mvc;
using TranSmart.Domain.Models.Payroll;

namespace TranSmart.API.Controllers.Payroll
{
	public partial class DeductionComponentController : BaseController
	{

		[HttpGet("GetDeductions")]
		public async Task<IActionResult> DeductionComponents([FromQuery] string OrderBy)
		{
			return Ok(_mapper.Map<IEnumerable<DeductionComponentList>>(await _service.DeductionComponents(OrderBy)));
		}
	}
}
