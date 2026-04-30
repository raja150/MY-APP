using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace TranSmart.API.IntegrationTest
{
    public class TestFixture<TStartup> : IDisposable
    {
        private readonly TestServer _server;
        public TestFixture()
               : this(Path.Combine(""))
        {
        }

        public HttpClient Client { get; }

        public static string GetProjectPath(string projectRelativePath, Assembly startupAssembly)
        {
            var projectName = startupAssembly.GetName().Name;

            var applicationBasePath = AppContext.BaseDirectory;

            var directoryInfo = new DirectoryInfo(@"D:\hari\HIMS\trunk\server\Transmart.API");

            do
            {
                directoryInfo = directoryInfo.Parent;

                var projectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, projectRelativePath));

                if (projectDirectoryInfo.Exists
                    && new FileInfo(Path.Combine(projectDirectoryInfo.FullName,
                        projectName, $"{ projectName}.csproj")).Exists)
                {
                    return Path.Combine(projectDirectoryInfo.FullName, projectName);
                }
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
        } 

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
			GC.SuppressFinalize(this);
        }

        protected virtual void InitializeServices(IServiceCollection services)
        {
            var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;

            var manager = new ApplicationPartManager
            {
                ApplicationParts =
                {
                    new AssemblyPart(startupAssembly)
                },
                FeatureProviders =
                {
                    new ControllerFeatureProvider(),
                    new ViewComponentFeatureProvider()
                }
            };

            services.AddSingleton(manager);
        }

        protected TestFixture(string relativeTargetProjectParentDir)
        {
            var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;
            var contentRoot = GetProjectPath(relativeTargetProjectParentDir, startupAssembly);

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(contentRoot)
                .AddJsonFile("appsettings.json");

            var webHostBuilder = new WebHostBuilder()
                .UseContentRoot(contentRoot)
                .ConfigureServices(InitializeServices)
                .UseConfiguration(configurationBuilder.Build())
                .UseEnvironment("Development")
                .UseStartup(typeof(TStartup));

            // Create instance of test server
            _server = new TestServer(webHostBuilder);

            // Add configuration for client
            Client = _server.CreateClient();
            Client.BaseAddress = new Uri("http://localhost:5000");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (Client.DefaultRequestHeaders.Authorization == null)
            {
                GetToken();
            }
            else
            {
                var response = Client.GetAsync(@"/Api/Setup/Bed");
                var value = response.GetAwaiter().GetResult();
                if (value.ToString().Contains("Unauthorized"))
                {
                    Client.DefaultRequestHeaders.Authorization = null;
                    GetToken();
                }
            } 
        }

        public void GetToken()
        {
            var request = new
            {
                Url = "/api/Auth",
                Body = new
                {
                    Username = "administrator",
                    Password = "password"
                }
            };

            // Act
            var response = Client.PostAsync(request.Url, ContentHelper.GetStringContent(request.Body));
            var value = response.GetAwaiter().GetResult();
            Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(value.Content.ReadAsStringAsync().Result);
            string Jwt = values["jwtToken"].ToString();
            Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Jwt);

        }
    }
}
