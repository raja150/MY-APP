using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Transmart.TS4API
{
    public class BaseHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Content == null) request.Content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
