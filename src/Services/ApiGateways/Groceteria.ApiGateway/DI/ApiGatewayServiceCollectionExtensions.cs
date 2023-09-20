using Groceteria.ApiGateway.Configurations;
using Groceteria.ApiGateway.Infrastructures.Logger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Serilog;

namespace Groceteria.ApiGateway.DI
{
    public static class ApiGatewayServiceCollectionExtensions
    {
        public static IServiceCollection AddGatewayServices(this IServiceCollection services, IHostBuilder host, IConfiguration configuration)
        {

            // serilog 
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var logIndexPattern = $"Groceteria.ApiGateway-{env?.ToLower().Replace(".", "-")}";
            var logger = LoggerConfig.Configure(configuration, logIndexPattern);

            host.UseSerilog(logger);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = "https://localhost:7001";
                    options.Audience = "https://localhost:7001/resources";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                    };
                });

            // ocelot
            services.AddOcelot()
                .AddCacheManager(settings =>
                {
                    settings.WithDictionaryHandle();
                });

            // configurations
            services.Configure<ApiSubscriptions>(configuration.GetSection("ApiSubscriptions"));

            return services;
        }
    }
}
