using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities.Organization;
using TranSmart.Domain.Models.Organization;
using TranSmart.Service.Organization;

namespace TranSmart.API.Controllers.Organization
{
    [Route("api/Organization/[controller]")]
    public partial class UpdateProfileController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IUpdateProfileService _service;
        private readonly IEmployeePresentAdService _presentService;
        private readonly IEmployeeEmergencyAdService _emergencyService;
        private readonly IEmployeePermanentAdService _permanentService;
        private readonly IEmployeeFamilyService _serviceFamily;

        public UpdateProfileController(IMapper mapper, IUpdateProfileService service, IEmployeePresentAdService presentService, IEmployeePermanentAdService permanentService, IEmployeeEmergencyAdService emergencyService, IEmployeeFamilyService serviceFamily)
        {
            _mapper = mapper;
            _service = service;
            _emergencyService = emergencyService;
            _serviceFamily = serviceFamily;
            _presentService = presentService;
            _permanentService = permanentService;
        }
        [HttpPut("UpdateEmergencyAddress")]
        public async Task<IActionResult> Put(EmployeeEmergencyAdModel model)
        {
			model.EmployeeId = LOGIN_USER_EMPId;
			Result<Employee> result1 = await _service.Verification(LOGIN_USER_EMPId);
			if (result1.HasError) return BadRequest(result1);
			Result<EmployeeEmergencyAd> result = await _emergencyService.UpdateAsync(_mapper.Map<EmployeeEmergencyAd>(model));
			if (result.IsSuccess)
            {
                return Ok(_mapper.Map<EmployeeEmergencyAdModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }
        [HttpPut("UpdatePresentAddress")]
        public async Task<IActionResult> Put(EmployeePresentAdModel model)
        {
			model.EmployeeId = LOGIN_USER_EMPId;
			Result<Employee> result1 = await _service.Verification(LOGIN_USER_EMPId);
			if (result1.HasNoError)
            {
                Result<EmployeePresentAd> result = await _presentService.UpdateAsync(_mapper.Map<EmployeePresentAd>(model));
                if (result.IsSuccess)
                {
                    return Ok(_mapper.Map<EmployeePresentAdModel>(result.ReturnValue));
                }

            }
            return BadRequest(result1);
        }
        [HttpPut("UpdatePermanentAddress")]
        public async Task<IActionResult> Put(EmployeePermanentAdModel model)
        {
			model.EmployeeId = LOGIN_USER_EMPId;
			Result<Employee> result1 = await _service.Verification(LOGIN_USER_EMPId);
			if (result1.HasNoError)
            {
                Result<EmployeePermanentAd> result = await _permanentService.UpdateAsync(_mapper.Map<EmployeePermanentAd>(model));
                if (result.IsSuccess)
                {
                    return Ok(_mapper.Map<EmployeePermanentAdModel>(result.ReturnValue));
                }
            }

            return BadRequest(result1);
        }
        [HttpPut("UpdateFromProfile")]
        public async Task<IActionResult> UpdateFromProfile(EmpProfileModel model)
        {
            var empId = LOGIN_USER_EMPId;
            Result<Employee> result = await _service.UpdateFromProfile(empId, model);
            if (!result.HasError)
            {
                return Ok(_mapper.Map<EmpProfileModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }
        [HttpPut("UpdateProfileContact")]
        public async Task<IActionResult> LocWeekOffSetup(EmployeeFamilyModel model)
        {
            model.EmployeeId = LOGIN_USER_EMPId;
            Result<EmployeeFamily> result = await _serviceFamily.UpdateProfileContact(model);
            if (!result.HasError)
            {
                return Ok(_mapper.Map<EmployeeFamilyModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }

        [HttpPut("DeleteProfileContact/{id}")]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            Result<EmployeeFamily> result = await _serviceFamily.DeleteProfileContact(id);
            if (!result.HasError)
            {
                return Ok(_mapper.Map<EmployeeFamilyModel>(result.ReturnValue));
            }
            return BadRequest(result);
        }
    }
}
