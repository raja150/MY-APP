using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transmart.TS4API
{
    public static class ServiceConfig
    {
        /// <summary>
        /// Register all the Refit API interfaces and handlers of specific interface. 
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the service</param>
        /// <param name="apiUrl">Base API URL</param>
        public static void RefitConfig(this IServiceCollection services, string apiUrl)
        {
            _ = services.AddTransient<ApiHandler>();

            _ = services.AddRefitClient<ILogin>()
              .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiUrl));

            _ = services.AddRefitClient<ITs4ApiS>()
               .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiUrl))
               .AddHttpMessageHandler<ApiHandler>();

          
        }

        public static string TS4Token { get; set; }
    }
}
