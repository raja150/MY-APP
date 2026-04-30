using Microsoft.AspNetCore.Mvc;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models;
using TranSmart.Domain.Models.Organization;
using TranSmart.Service.Organization;

namespace TranSmart.API.Controllers.Organization
{
	[Route("api/Organization/[controller]")]
	[ApiController]
	public class EmployeeDeviceController : BaseController
	{
		private readonly IMapper _mapper;
		private readonly IEmployeeDeviceService _service;
		public EmployeeDeviceController(IMapper mapper, IEmployeeDeviceService service)
		{
			_mapper = mapper;
			_service = service;
		}
		[HttpGet("Paginate")]
		public async Task<IActionResult> Paginate([FromQuery] BaseSearch baseSearch)
		{
			return Ok(_mapper.Map<Models.Paginate<EmployeeDeviceModel>>(await _service.GetPaginate(baseSearch)));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			return Ok(_mapper.Map<EmployeeDeviceModel>(await _service.GetById(id)));
		}

		[HttpPost]
		public async Task<IActionResult> Post(EmployeeDeviceModel model)
		{
			var entity = _mapper.Map<EmployeeDevice>(model);
			entity.InstalledById = LOGIN_USER_EMPId;
			Result<EmployeeDevice> result;
			if (!model.addDuplicate)
			{
				result = await _service.AddAsync(entity);
			}
			else
			{
				result = await _service.AddOnlyAsync(entity);
			}
			if (result.HasNoError)
			{
				return Ok(_mapper.Map<EmployeeDeviceModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}

		[HttpPut]
		public async Task<IActionResult> Put(EmployeeDeviceModel model)
		{
			var entity = _mapper.Map<EmployeeDevice>(model);
			entity.InstalledById = LOGIN_USER_EMPId;
			Result<EmployeeDevice> result;
			if (entity.IsUninstalled)
			{
				entity.UninstalledById = LOGIN_USER_EMPId;
			}
			if (!model.addDuplicate)
			{
				result = await _service.UpdateAsync(entity);
			}
			else
			{
				result = await _service.UpdateOnlyAsync(entity);
			}
			if (result.HasNoError)
			{
				return Ok(_mapper.Map<EmployeeDeviceModel>(result.ReturnValue));
			}
			return BadRequest(result);
		}
	}
}
