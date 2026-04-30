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
using TranSmart.Service.Helpdesk;
using TranSmart.Service.Organization;
using TranSmart.Service.SelfService;

namespace TranSmart.API.Controllers.Helpdesk
{
	[Route("api/HelpDesk/[controller]")]
	[ApiController]
	public class TicketLogController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly ITicketLogService _service;
		private readonly IEmployeeService _eService;
		private readonly ITicketService _selfTicketService;
		private readonly IFileServerService _fileServerService;

		public TicketLogController(IMapper mapper, ITicketLogService service, IEmployeeService eService, ITicketService ticketService, IFileServerService fileServerService)
		{
			_mapper = mapper;
			_service = service;
			_eService = eService;
			_selfTicketService = ticketService;
			_fileServerService = fileServerService;
		}

		[HttpPost]
		[ApiAuthorize(Core.Permission.HD_Tickets, Core.Privilege.Create)]
		public async Task<IActionResult> PostReply(TicketLogModel model)
		{
			TicketLog entity = _mapper.Map<TicketLog>(model);

			entity.RepliedById = LOGIN_USER_EMPId;
			entity.RepliedOn = DateTime.Now;
			entity.ModifiedBy = UserId;

			Result<TicketLog> result = await _service.AddAsync(entity);
			if (result.HasError)
			{
				return BadRequest(result);
			}
			var loginMail = await _eService.GetLoginEmpMail(LOGIN_USER_EMPId);

			//Raised Person Mail Id
			if (!string.IsNullOrEmpty(model.RaisedByEmpMail) && !string.IsNullOrEmpty(loginMail.WorkEmail))
			{
				await MailService.SendMailFromHelpDesk(loginMail.WorkEmail, model.RaisedByEmpMail, entity.Response, string.Concat("Ticket No: ", model.TicketNo));
			}
			//Sending mail  for Recipients
			foreach (var item in model.Recipients.Where(item => !string.IsNullOrEmpty(item.WorkMail)))
			{
				await MailService.SendMailFromHelpDesk(loginMail.WorkEmail, item.WorkMail, entity.Response, string.Concat("Ticket No: ", model.TicketNo));
			}

			return Ok(_mapper.Map<TicketLogModel>(result.ReturnValue));
		}


		[HttpGet("{id}")]
		[ApiAuthorize(Core.Permission.HD_Tickets, Core.Privilege.Read)]
		public async Task<IActionResult> Get(Guid id)
		{
			var item = await _selfTicketService.GetById(id);
			if (item != null) return Ok(_mapper.Map<TicketModel>(item));
			return NoContent();
		}

		[HttpPost("New")]
		[ApiAuthorize(Core.Permission.HD_Tickets, Core.Privilege.Create)]
		public async Task<IActionResult> Post([FromForm] TicketRequest model)
		{
			Ticket entity = _mapper.Map<Ticket>(model);
			entity.RaiseById = LOGIN_USER_EMPId;
			entity.File = model.File != null ? model.File.FileName : "";
			Result<Ticket> result = await _selfTicketService.AddAsync(entity);
			if (result.HasError) return BadRequest(result);
			await _fileServerService.UploadFile(model.File, result.ReturnValue.ID.ToString(), "Attachments", "Tickets");
			return Ok(result.ReturnValue);
		}

		#region Get

		[HttpGet("Paginate")]
		[ApiAuthorize(Core.Permission.HD_Tickets, Core.Privilege.Read)]
		public async Task<IActionResult> GetTickets([FromQuery] TicketSearch search)
		{
			search.RefId = LOGIN_USER_EMPId;
			return Ok(_mapper.Map<Models.Paginate<TicketList>>(await _service.GetTicekts(search)));
		}

		[HttpGet("Response/{id}")]
		public async Task<IActionResult> SearchEmp(Guid id)
		{
			return Ok(_mapper.Map<List<TicketResponseModel>>(await _service.GetTicketLog(id)));
		}

		[HttpGet("ResponseInfo/{id}")]
		public async Task<IActionResult> GetTicketResponseInfo(Guid id)
		{
			return Ok(_mapper.Map<TicketResponseModel>(await _service.TicketResponse(id)));
		}

		#endregion

		#region Put

		[HttpPut("Transfer")]
		[ApiAuthorize(Core.Permission.HD_Tickets, Core.Privilege.Update)]
		public async Task<IActionResult> DeptTransfer(DeptTransferModel model)
		{
			var entity = _mapper.Map<Ticket>(model);
			entity.ModifiedBy = UserId;
			Result<Ticket> result = await _service.DeptTransfer(entity, LOGIN_USER_EMPId);
			if (result.HasError) return BadRequest(result);
			return Ok(result.ReturnValue);
		}

		[HttpPut("ReAssign")]
		[ApiAuthorize(Core.Permission.HD_Tickets, Core.Privilege.Update)]
		public async Task<IActionResult> ReAssign(TicketLogModel model)
		{
			TicketLog entity = _mapper.Map<TicketLog>(model);
			entity.RepliedOn = DateTime.Now;
			entity.RepliedById = LOGIN_USER_EMPId;
			entity.ModifiedBy = UserId;
			Result<TicketLog> result = await _service.ReAssign(entity);
			if (result.HasError) return BadRequest(result);

			var loginMail = await _eService.GetLoginEmpMail(LOGIN_USER_EMPId);

			//Sending  Mail to assigned person
			if (!string.IsNullOrEmpty(loginMail.WorkEmail) && !string.IsNullOrEmpty(model.ToWorkMail))
			{
				await MailService.SendMailFromHelpDesk(loginMail.WorkEmail, model.ToWorkMail, "New Ticket",
								string.Concat("Ticket No: ", model.TicketNo));
			}

			return Ok(result.ReturnValue);
		}

		[HttpPut("Response")]
		[ApiAuthorize(Core.Permission.HD_Tickets, Core.Privilege.Update)]
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
			return Ok(_mapper.Map<TicketResponseModel>(result.ReturnValue));
		}


		#endregion

	}
}
