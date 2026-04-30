
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using TranSmart.API.Extensions;
using TranSmart.API.Services;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Service.Organization;

namespace TranSmart.API.Controllers.SelfService
{
	[Route("api/SelfService/[controller]")]
	[ApiController]
	public class AnnualSalaryController : BaseController
	{
		private readonly IEmployeeService _employeeService;
		private readonly IFileServerService _fileServerService;
		public AnnualSalaryController(IFileServerService fileServerService, IEmployeeService employeeService)
		{
			_fileServerService = fileServerService;
			_employeeService = employeeService;
		}

		[HttpGet("HasFormSixteen")]
		public async Task<IActionResult> HasFormSixteen()
		{
			var employee = await _employeeService.GetById(LOGIN_USER_EMPId);
			if (!_fileServerService.IsFileExists(@"images\Forms", "FY 2022 - 2023", $"{employee.PanNumber}_2023-24.pdf"))
			{
				return NotFound();
			}

			return Ok();
		}

		//[HttpGet("FormSixteen")]
		//public async Task<IActionResult> FormSixteen(Guid id)
		//{
		//	var employee = await _employeeService.GetById(LOGIN_USER_EMPId);

		//	var file1 = _fileServerService.PhysicalPath(@"images\Forms", "FY 2022 - 2023", $"{employee.PanNumber}_2023-24.pdf");
		//	var file2 = _fileServerService.PhysicalPath(@"images\Forms", "FY 2022 - 2023", $"{employee.PanNumber}_PARTB_2023-24.pdf");

		//	using var memoryStream = new MemoryStream();
		//	using ICSharpCode.SharpZipLib.Zip.ZipFile zip = new(memoryStream);
		//	zip.BeginUpdate();
		//	zip.Password = employee.PanNumber.ToUpper();
		//	ICSharpCode.SharpZipLib.Zip.ZipEntry zipEntry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(file1);

		//	zip.Add(file1, Path.GetFileName(file1).Replace(employee.PanNumber, "Form 16"));
		//	zip.Add(file2, Path.GetFileName(file2).Replace(employee.PanNumber, "Form 16"));
		//	zip.CommitUpdate();
		//	zip.Close();
		//	return File(memoryStream.ToArray(), "application/zip", "Form16.zip");
		//}
	}
}
