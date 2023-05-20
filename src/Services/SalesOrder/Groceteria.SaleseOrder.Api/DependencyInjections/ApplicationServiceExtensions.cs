using Groceteria.Infrastructure.Logger;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Converters;
using Serilog;

namespace Groceteria.SaleseOrder.Api.DependencyInjections
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    });

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            // http
            services.AddHttpContextAccessor();

            // serilog
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var logIndexPattern = $"Groceteria.SalesOrder.Api-{env?.ToLower().Replace(".", "-")}";
            var logger = LoggerConfig.Configure(configuration, logIndexPattern);
            services.AddSingleton(Log.Logger)
                    .AddSingleton(x => logger);

            // api versioning
            services.AddApiVersioning(option =>
            {
                option.DefaultApiVersion = ApiVersion.Default;
                option.ReportApiVersions = true;
                option.AssumeDefaultVersionWhenUnspecified = true;
            })
            .AddVersionedApiExplorer(option =>
            {
                option.GroupNameFormat = "'v'VVV";
                option.SubstituteApiVersionInUrl = true;
            });
            return services;
        }
    
        public static WebApplication AddApplicationPipelines(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
