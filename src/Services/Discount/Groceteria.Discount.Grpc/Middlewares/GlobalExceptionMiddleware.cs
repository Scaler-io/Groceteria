using Groceteria.Shared.Core;
using Groceteria.Shared.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using ILogger = Serilog.ILogger;
using Groceteria.Shared.Extensions;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Groceteria.Discount.Grpc.Middlewares
{
    public class GlobalExceptionMiddleware: Interceptor
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionMiddleware(ILogger logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async Task<TResponse> HandleAsync<TRequest, TResponse>(TRequest request,
            ServerCallContext context,
            UnaryServerMethod<TRequest, TResponse> next)
            where TRequest : class
            where TResponse : class
        {
            try
            {
               return await next(request, context);
            }
            catch (Exception exception)
            {
                HandleGeneralException(exception);
                throw new RpcException(new Status(StatusCode.Internal, exception.Message));
            }
        }

        private void HandleGeneralException(Exception exception)
        {
            var response = _env.IsDevelopment()
                             ? new ApiExceptionResponse(exception.Message, exception.StackTrace)
                             : new ApiExceptionResponse(exception.Message);

            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter>
                {
                    new StringEnumConverter()
                }
            };
            var jsonResponse = JsonConvert.SerializeObject(response, jsonSettings);
            _logger.Here().Error("{@InternalServerError} - {@response}", ErrorCode.InternalServerError, jsonResponse);
          
        }
    }
}