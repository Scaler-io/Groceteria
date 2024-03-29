﻿using Groceteria.SaleseOrder.Api.Extensions;
using Serilog.Context;

namespace Groceteria.SaleseOrder.Api.Middlewares
{
    public class CorrelationHeaderEnricher
    {
        private const string CorrelationIdLogPropertyName = "CorrelationId";
        private readonly RequestDelegate _next;

        public CorrelationHeaderEnricher(RequestDelegate next)
        {
            _next = next;
        }

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

        private string GetOrGenerateCorrelationId(HttpContext context) => context?.Request.GetRequestHeaderOrDefault("CorrelationId", $"GEN-{Guid.NewGuid().ToString()}");
    }
}
