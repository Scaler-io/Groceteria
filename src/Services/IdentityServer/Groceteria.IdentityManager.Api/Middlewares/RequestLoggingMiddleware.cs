using Groceteria.IdentityManager.Api.Extensions;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog.Context;
using System.Diagnostics;

namespace Groceteria.IdentityManager.Api.Middlewares
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
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var headers = context.Request.Headers.ToDictionary(x => x.Key, x => x.Value);
            var url = context.Request.GetDisplayUrl();
            var verb = context.Request.Method;          

            using (LogContext.PushProperty("Url", url))
            {
                LogContext.PushProperty("HttpMethod", verb);
                _logger.Here()
                        .Information("Http request starting");
                await _next(context);

                stopwatch.Stop();
                _logger.Here().Debug("Elapsed time {elapsedTime}", stopwatch.Elapsed);
                _logger.Here()
                        .Information("Http request completed. Response code {@code}", context.Response.StatusCode);
            }
        }
    }
}
