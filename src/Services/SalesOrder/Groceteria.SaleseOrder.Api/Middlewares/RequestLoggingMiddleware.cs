using Groceteria.Shared.Extensions;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog.Context;
using ILogger = Serilog.ILogger;

namespace Groceteria.SaleseOrder.Api.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var headers = context.Request.Headers.ToDictionary(x => x.Key, x => x.Value);
            var url = context.Request.GetDisplayUrl();
            var verb = context.Request.Method;

            using (LogContext.PushProperty("Url", url))
            {
                LogContext.PushProperty("HttpMethod", verb);
                _logger.Here()
                        .Information("Http request starting. Headers: {@headers}", headers);
                await _next(context);
                _logger.Here()
                        .Information("Http request completed. Response code {@code}", context.Response.StatusCode);
            }
        }
    }
}
