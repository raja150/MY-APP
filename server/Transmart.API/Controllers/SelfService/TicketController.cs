using Microsoft.AspNetCore.Mvc;
using TranSmart.API.Extensions;
using TranSmart.API.Services;
using TranSmart.Core.Result;
using TranSmart.Core.Util;
using TranSmart.Domain.Entities.HelpDesk;
using TranSmart.Domain.Entities.SelfService;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Helpdesk.Request;
using TranSmart.Domain.Models.SelfService.List;
using TranSmart.Domain.Models.SelfService.Request;
using TranSmart.Domain.Models.SelfService.Response;
using TranSmart.Service.SelfService;

namespace TranSmart.API.Controllers.SelfService
{
	[Route("api/SelfService/[controller]")]
	[ApiController]
	public partial class TicketController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly ITicketService _service;
		private readonly IFileServerService _fileServerService;
		public TicketController(IMapper mapper, ITicketService service, IFileServerService fileServerService)
		{
			_mapper = mapper;
			_service = service;
			_fileServerService = fileServerService;
		}

		[HttpGet("Paginate")]
		[ApiAuthorize(Core.Permission.SS_RaiseTicket, Core.Privilege.Read)]
		public async Task<IActionResult> Paginate([FromQuery] TicketSearch search)
		{
			search.RefId = LOGIN_USER_EMPId;
			return Ok(_mapper.Map<Models.Paginate<TicketList>>(await _service.GetPaginate(search)));
		}
		[HttpGet("{id}")]
		[ApiAuthorize(Core.Permission.SS_RaiseTicket, Core.Privilege.Read)]	
		public async Task<IActionResult> Get(Guid id)
		{
			var item = await _service.GetById(id);
			if (item != null) return Ok(_mapper.Map<TicketModel>(item));
			return NoContent();
		}
		[HttpPost]
		[ApiAuthorize(Core.Permission.SS_RaiseTicket, Core.Privilege.Create)]
		public async Task<IActionResult> Post([FromForm] TicketRequest model)
		{
			Ticket entity = _mapper.Map<Ticket>(model);
			entity.RaiseById = LOGIN_USER_EMPId;
			entity.File = model.File != null ? model.File.FileName : "";
			Result<Ticket> result = await _service.AddAsync(entity);
			if (result.HasError) return BadRequest(result);
			await _fileServerService.UploadFile(model.File, result.ReturnValue.ID.ToString(), "Attachments", "Tickets");
			return Ok(result.ReturnValue);
		}
		[HttpPut]
		[ApiAuthorize(Core.Permission.SS_RaiseTicket, Core.Privilege.Update)]
		public async Task<IActionResult> Put([FromForm] TicketRequest model)
		{
			model.RaiseById = LOGIN_USER_EMPId;
			Result<Ticket> result = await _service.UpdateAsync(_mapper.Map<Ticket>(model));
			if (result.HasError) return BadRequest(result);
			return Ok(result.ReturnValue);
		}

		[HttpGet("Download/{id}/{fileName}")]
		public async Task<IActionResult> Download(Guid id, string fileName)
		{
			var res = await _fileServerService.DownloadFile(id, "Attachments", "Tickets");
			if (res != null)
			{
				return new DownloadFile(res, fileName);
			}
			return null;
		}
		[HttpPost("UserResponse")]
		public async Task<IActionResult> UserResponse(UserRespnoseModel model)
		{
			var result = new Result<TicketLog>();
			if (string.IsNullOrWhiteSpace(model.Response)|| StringUtil.HmtlStringIsEmpty(model.Response))
			{
				result.AddMessageItem(new MessageItem("Response is required"));
				return BadRequest(result);
			}
			result = await _service.UserResponse(model.TicketId, model.Response, LOGIN_USER_EMPId);
			if (result.HasError) return BadRequest(result);
			return Ok(result);
		}

		[HttpPut("Response")]
		[ApiAuthorize(Core.Permission.SS_RaiseTicket, Core.Privilege.Update)]
		public async Task<IActionResult> UpdateResponse(LogResponseModel model)	
		{
			var result = new Result<TicketLog>();
			if (string.IsNullOrWhiteSpace(model.Response) || StringUtil.HmtlStringIsEmpty(model.Response))
			{
				result.AddMessageItem(new MessageItem("Response is required"));
				return BadRequest(result);
			}
			result = await _service.UpdateResponse(model.ID, model.Response, LOGIN_USER_EMPId);
			if (result.HasError) return BadRequest(result);
			return Ok(result);
		}
	}

}
