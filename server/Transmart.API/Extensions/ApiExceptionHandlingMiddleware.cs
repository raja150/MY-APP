using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using TranSmart.API.Extensions.DI;
using TranSmart.Core.Exceptions;

namespace TranSmart.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseApiExceptionHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ApiExceptionHandlingMiddleware>();
        }
    }
    public class ApiExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionHandlingMiddleware> _logger;

        public ApiExceptionHandlingMiddleware(RequestDelegate next, ILogger<ApiExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
                //if (context.Response.StatusCode == StatusCodes.Status400BadRequest)
                //{
                //    _ = context.Response.Body.Seek(0, SeekOrigin.Begin);
                //    string text = await new StreamReader(context.Response.Body).ReadToEndAsync();
                //    _ = context.Response.Body.Seek(0, SeekOrigin.Begin); 
                //}
            }
			catch (TranSmartException ex) { await HandleExceptionAsync(context, ex, ex.Code); }
			catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, "EX00000");
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, string code)
        {
            //https://dev.to/moesmp/what-every-asp-net-core-web-api-project-needs-part-3-exception-handling-middleware-3nif
            //http://codingsonata.com/exception-handling-and-logging-in-asp-net-core-web-api/
            _logger.LogError(ex, $"An unhandled exception has occurred, {ex.Message}");

			var problemDetails = new AnnotatedProblemDetails()
			{
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
				Code = code,
                Title = "Internal Server Error",
                Status = (int)HttpStatusCode.InternalServerError,
                Instance = context.Request.Path,
#if DEBUG
                Detail = ex.Message
#else
                Detail = "Internal server error occurred!"
#endif
            };

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            string result = JsonSerializer.Serialize(problemDetails);

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(result);
        }
    }
}
