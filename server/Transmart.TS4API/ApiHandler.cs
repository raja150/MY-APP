using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Transmart.TS4API
{
	public class ApiHandler : BaseHandler
	{
		public ApiHandler()
		{

		}
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			if (string.IsNullOrEmpty(ServiceConfig.TS4Token))
			{
				ServiceConfig.TS4Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJUeXAiOiItMSIsIlVJZCI6IjI3MTIiLCJFSWQiOiItMTAwIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiOTc2NjM4NDktMDY3Mi00N2Y2LWJhYjYtZGIyMTIyMDBlZDIyIiwiZXhwIjoxNjUwNTM5MzYzLCJpc3MiOiJhdm9udGl4IiwiYXVkIjoiYXZvbnRpeCJ9.zEOzSznArOGLLmoDS7MEcjqA-36sSmLC0i6GOiS1ltY";
			}
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ServiceConfig.TS4Token);
			HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
			if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			{
				//MedtekToken token = await _service.GetTranscription();
				//var info = await _login.GetUser(new Models.LoginModel { ClientID = token.ClientId, UserName = token.Username, Password = token.Password });
				//request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", info.UserToken);
				//token.Token = info.UserToken;
				//ServiceConfig.TranscriptionToken = info.UserToken;
				//_ = await _service.UpdateTranscription(token);
				response = await base.SendAsync(request, cancellationToken);
			}
			return response;
		}
	}
}
