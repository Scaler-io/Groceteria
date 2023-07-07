using Groceteria.ApiGateway.Extensions.Logger;
using Groceteria.ApiGateway.Models.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Groceteria.ApiGateway.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _environment;

        public GlobalExceptionMiddleware(ILogger logger, 
            RequestDelegate next, 
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _next = next;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }catch(Exception e)
            {
                _logger.Here().Error("Unhandled excpetion occured");
                HandleExcpetion(e, context);
            }
        }

        private async void HandleExcpetion(Exception e, HttpContext context)
        {
            var code = (int)HttpStatusCode.InternalServerError;
            var message = e.Message;
            var stackTrace = e.StackTrace;

            var response = _environment.IsDevelopment() 
                            ? new ErrorResponse(code, message, stackTrace)
                            : new ErrorResponse(code, message);

            var serializeSettings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter>() { new StringEnumConverter() },
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

            var jsonResponse = JsonConvert.SerializeObject(response, serializeSettings);
            _logger.Here().Error("Exception handled - {code} {message} {stackTrace}",
                code, message, stackTrace);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
