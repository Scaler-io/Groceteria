using Groceteria.ApiGateway.Infrastructures.Logger;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
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

            Log.Logger = logger;
            services.AddSingleton(Log.Logger);

            // ocelot
            services.AddOcelot()
                .AddCacheManager(settings =>
                {
                    settings.WithDictionaryHandle();
                });

            return services;
        }
    }
}
