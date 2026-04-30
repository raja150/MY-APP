using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TranSmart.API.Extensions;
using TranSmart.API.Services;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.SelfService;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.SelfService.Request;
using TranSmart.Service.SelfService;

namespace TranSmart.API.Controllers.SelfService
{
	[Route("api/SelfService/[controller]")]
	[ApiController]
	public class ComplianceController : BaseController
	{
		private readonly ICompilanceService _service;
		private readonly IFileServerService _fileService;
		public ComplianceController(ICompilanceService service, IFileServerService fileServer)
		{
			_service = service;
			_fileService = fileServer;
		}

		[HttpGet]
		[ApiAuthorize(Core.Permission.SS_Compliances, Core.Privilege.Read)]
		public async Task<IActionResult> Get()
		{
			var res = await _service.GetById(LOGIN_USER_EMPId);
			if (res != null)
			{
				var file = await _fileService.DownloadFile("Images", "Compliances", LOGIN_USER_EMPId.ToString());
				if (file != null)
				{
					return new DownloadFile(file, "Compliances.pdf");
				}
				return Ok();
			}
			return NoContent();
		}

		[HttpPost]
		[ApiAuthorize(Core.Permission.SS_Compliances, Core.Privilege.Create)]
		public async Task<IActionResult> Post([FromForm] IFormFile file)
		{
			var result = new Result<Compliance>();
			if (file == null || Path.GetExtension(file.FileName).ToLower() != ".pdf")
			{
				result.AddMessageItem(new MessageItem("Only PDF format is acceptable."));
				return BadRequest(result);
			}
			var entity = new Compliance
			{
				EmployeeId = LOGIN_USER_EMPId,
				FileName = Path.GetExtension(file.FileName),
			};
			result = await _service.AddOrUpdate(entity);
			if (result.HasError) return BadRequest(result);
			bool isUploaded = await _fileService.UploadFile(file, LOGIN_USER_EMPId.ToString(), "Images", "Compliances");
			if (!isUploaded)
			{
				result.AddMessageItem(new MessageItem("saving file in our path is failed"));
				return BadRequest(result);
			}
			return Ok(result);
		}
	}
}
