using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TranSmart.Core.Result;
using TranSmart.Core.Util;
using TranSmart.Data;
using TranSmart.Domain.Entities;
using TranSmart.Domain.Models;
using TranSmart.Service;

namespace TranSmart.API.Services
{
	public interface ISsoService
	{
		Task<Result<User>> SSOAddUser(UserModel model);
		Task<Result<User>> SSOUpdateUser(SsoUserModel model);
		Task<Result<User>> AdminUpdate(SsoUserModel model);
		Task<bool> UpdateEmpSts(SsoUserModel model);
	}
	public class SsoService : ISsoService
	{
		readonly HttpClient client;
		private readonly IConfiguration _configuration;
		private readonly IUserService _service;
		public SsoService(IConfiguration configuration, IUserService service)
		{
			_configuration = configuration;
			client = new HttpClient();
			client.DefaultRequestHeaders.Add(StringUtil.APIKey, _configuration["ApiKey"]);
			_service = service;
		}

		public async Task<Result<User>> AdminUpdate(SsoUserModel model)
		{
			model.Name = model.Name.ToUpper();
			model.Type = (int)UserSts.Active;
			HttpResponseMessage response = await client.PutAsJsonAsync(_configuration["SSOApi"] + "/User/AdminUpdate", model);
			var result = new Result<User>();
			if (response.StatusCode == HttpStatusCode.OK)
			{
				return result;
			}
			await SsoData.SsoErrors(response, result);
			return result;
		}
		public async Task<Result<User>> SSOAddUser(UserModel model)
		{
			model.Name = model.Name.ToUpper();
			HttpResponseMessage response = await client.PostAsJsonAsync(_configuration["SSOApi"] + "/User", model);
			var result = new Result<User>();
			if (response.StatusCode == HttpStatusCode.OK)
			{
				return result;
			}
			await SsoData.SsoErrors(response, result);
			return result;
		}
		public async Task<Result<User>> SSOUpdateUser(SsoUserModel model)
		{
			var emp = await _service.CheckUser(model.Name);
			if (emp.Employee == null)
			{
				model.Type = (int)UserSts.Active;
			}
			else
			{
				model.Type = emp.Employee.Status == 1 ? (int)UserSts.Active : (int)UserSts.InActive;
			}
			HttpResponseMessage response = await client.PutAsJsonAsync(_configuration["SSOApi"] + "/User", model);
			var result = new Result<User>();
			if (response.StatusCode == HttpStatusCode.OK)
			{
				return result;
			}
			await SsoData.SsoErrors(response, result);
			return result;
		}

		public async Task<bool> UpdateEmpSts(SsoUserModel model)
		{
			HttpResponseMessage response = await client.PutAsJsonAsync(_configuration["SSOApi"] + "/User/EmpSts", model);
			if (response.StatusCode == HttpStatusCode.OK)
			{
				return true;
			}
			return false;
		}

	}
}
