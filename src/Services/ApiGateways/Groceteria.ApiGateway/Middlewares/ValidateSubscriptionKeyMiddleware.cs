using Groceteria.ApiGateway.Configurations;
using Groceteria.ApiGateway.Extensions.Logger;
using Groceteria.ApiGateway.Utils;
using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Groceteria.ApiGateway.Middlewares
{
    public class ValidateSubscriptionKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly ApiSubscriptions _apiSubscriptions;

        public ValidateSubscriptionKeyMiddleware(RequestDelegate next, ILogger logger,
            IOptions<ApiSubscriptions> apiSubscriptions    
        )
        {
            _next = next;
            _logger = logger;
            _apiSubscriptions = apiSubscriptions.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;
            var segment = path.Split('/');

            if (segment.Length > 2)
            {
                var apiName = segment[1];
                var subscriptionKey = GetSubscriptionKeyForAPi(apiName);
                if (subscriptionKey != null)
                {
                    if (!context.Request.Headers.ContainsKey("Subscription-Key"))
                    {
                        _logger.Here().Warning("Request header did not have Subscription-Key");
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(new ApiResponse(ErrorCode.Unauthorized, "Request header did not have Subscription-Key"),
                        new JsonSerializerOptions
                        {
                           Converters = {new EnumToStringConverter<ErrorCode>()}
                        });;
                        return;
                    }

                    var requestSubscriptionKey = context.Request.Headers["Subscription-Key"];

                    if (!string.Equals(requestSubscriptionKey, subscriptionKey, StringComparison.OrdinalIgnoreCase))
                    {
                        _logger.Here().Warning("Invalid Subscription-Key provided");
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(new ApiResponse(ErrorCode.Unauthorized, "Invalid Subscription-Key provided"),
                        new JsonSerializerOptions
                        {
                            Converters = { new EnumToStringConverter<ErrorCode>() }
                        });
                        return;
                    }
                }
            }

            await _next.Invoke(context);
        }

        private string GetSubscriptionKeyForAPi(string apiName)
        {
            var capitalizeApiName = apiName[0].ToString().ToUpper() + apiName.Substring(1); 
            var propertyInfo = typeof(ApiSubscriptions).GetProperty(capitalizeApiName);
            return propertyInfo.GetValue(_apiSubscriptions)?.ToString();
        }
    }
}
