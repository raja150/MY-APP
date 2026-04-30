using DocumentFormat.OpenXml.Drawing.Charts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TranSmart.API.Models;
using TranSmart.Core.Result;
using TranSmart.Domain.Entities;

namespace TranSmart.API.Services
{
    public static class SsoData
    {
        public static async Task SsoErrors(HttpResponseMessage response, Result<User> result)
        {
            var errors = new List<SsoErrorMessages>();
            foreach (var e in await Json(response))
            {
                if (e.Key == "messages")
                {
                    errors = JsonConvert.DeserializeObject<List<SsoErrorMessages>>(e.Value.ToString());
                    break;
                }
            }

            foreach (var item in from item in errors
                                 where !result.Messages.Any(x => x.Description == item.Description)
                                 select item)
            {
                result.AddMessageItem(new MessageItem(item.Description));
            }
        }
        private static async Task<JObject> Json(HttpResponseMessage response)
        {
            var data = await response.Content.ReadAsStringAsync();
            return JObject.Parse(data);
        }
		public static async Task<SsoEmpData> GetUserInfo(HttpResponseMessage response)
		{
			return JsonConvert.DeserializeObject<SsoEmpData>(await response.Content.ReadAsStringAsync());
		}
		public static async Task<string> GetRefreshToken(HttpResponseMessage response)
		{
			string token = string.Empty;
			foreach (var e in await Json(response))
			{
				if (e.Key == "refreshToken")
				{
					token = e.Value.ToString();
					break;
				}
			}
			return token;
		}

	}
	public class SsoErrorMessages
    {
        public string Description { get; set; }
    }
}
