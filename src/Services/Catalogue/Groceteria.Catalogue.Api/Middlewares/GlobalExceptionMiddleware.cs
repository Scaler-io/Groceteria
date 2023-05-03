using Groceteria.Shared.Core;
using Groceteria.Shared.Extensions;
using System.Net.Mime;
using System.Net;
using ILogger = Serilog.ILogger;
using Groceteria.Shared.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Groceteria.Catalogue.Api.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        public readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                HandleGeneralException(context, ex);
            }
        }

        private async Task HandleGeneralException(HttpContext context, Exception ex)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = _env.IsDevelopment()
                             ? new ApiExceptionResponse(ex.Message, ex.StackTrace)
                             : new ApiExceptionResponse(ex.Message);

            var jsonSettings = new JsonSerializerSettings { 
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter()
                }
            };
            var jsonResponse = JsonConvert.SerializeObject(response, jsonSettings);
            _logger.Here().Error("{@InternalServerError} - {@response}", ErrorCode.InternalServerError, jsonResponse);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
