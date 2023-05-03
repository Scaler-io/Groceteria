using Groceteria.Catalogue.Api.Extensions;
using Serilog;
using Serilog.Context;
using ILogger = Serilog.ILogger;

namespace Groceteria.Catalogue.Api.Middlewares
{
    public class CorrelationHeaderEnricher
    {
        private const string CorrelationIdLogPropertyName = "CorrelationId";
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public CorrelationHeaderEnricher(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var correlationId = GetOrGenerateCorrelationId(context);
            using (LogContext.PushProperty("ThreadId", Environment.CurrentManagedThreadId))
            {
                LogContext.PushProperty(CorrelationIdLogPropertyName, correlationId);
                context.Response.Headers.Add("CorrelationId", correlationId);
                await _next(context);
            }
        }

        private string GetOrGenerateCorrelationId(HttpContext context) => context?.Request.GetRequestHeaderOrdefault("CorrelationId", $"GEN-{Guid.NewGuid().ToString()}"); 
    }
}
