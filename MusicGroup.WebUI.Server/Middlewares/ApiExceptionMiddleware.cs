using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MusicGroup.Common;
using MusicGroup.WebUI.Server.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MusicGroup.WebUI.Server.Middlewares
{
    public sealed class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionMiddleware> _logger;

        public ApiExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory?.CreateLogger<ApiExceptionMiddleware>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(ApiExceptionMiddleware)}. {ex.Message}");
                
                bool handled = await HandleExceptionAsync(context, ex);

                if (!handled)
                {
                    throw;
                }
            }
        }

        private async Task<bool> HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpRequest request = context.Request;
            
            bool isAjax = (request.ContentType == Constants.Http.ApiContextType);

            if (!isAjax)
            {
                return false;
            }
            
            string requestId = context.TraceIdentifier;

            string url = request.Path.ToString();

            string traceMessage = $"{nameof(ApiExceptionMiddleware)}. Unhandled exception.\r\nUrl: \"{url}\"\r\n{exception.Message}";

            _logger.LogError(exception, $"Error occured. RequestId=\"{requestId}\". Url=\"{url}\". Message=\"{exception.Message}\".");

            var error = new ServerError
            {
                RequestId = requestId,
                DebugDetails = traceMessage
            };
                    
            var response = new ResponseContainer
            {
                Error = error
            };
            
            HttpResponse httpResponse = context.Response;
            
            httpResponse.ContentType = Constants.Http.ApiContextType;
            
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            
            string responseJson = JsonConvert.SerializeObject(response, jsonSerializerSettings);
            
            await httpResponse.WriteAsync(responseJson);

            return true;
        }
    }

    public static class ApiExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiExceptionMiddleware>();
        }
    }
}