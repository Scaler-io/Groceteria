using Groceteria.Discount.Grpc.Extensions;
using Serilog.Context;
using ILogger = Serilog.ILogger;

namespace Groceteria.Discount.Grpc.Middlewares
{
    public class CorrelationHeaderEnricher
    {
        private const string CorrelationIdLogPropertyName = "CorrelationId";
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public async Task InvokeAsync(HttpContext context)
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