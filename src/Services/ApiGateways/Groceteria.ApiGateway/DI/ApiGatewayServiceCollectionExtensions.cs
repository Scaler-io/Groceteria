using Groceteria.ApiGateway.Infrastructures.Logger;
using Serilog;

namespace Groceteria.ApiGateway.DI
{
    public static class ApiGatewayServiceCollectionExtensions
    {
        public static IServiceCollection AddGatewayServices(this IServiceCollection services, IConfiguration configuration)
        {
            // serilog 
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var logIndexPattern = $"Groceteria.ApiGateway-{env?.ToLower().Replace(".", "-")}";
            var logger = LoggerConfig.Configure(configuration, logIndexPattern);
            services.AddSingleton(Log.Logger)
                    .AddSingleton(x => logger);

            return services;
        }
    }
}
