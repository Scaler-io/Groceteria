using Groceteria.Infrastructure.Logger;
using Groceteria.SaleseOrder.Api.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Newtonsoft.Json.Converters;
using Serilog;
using Swagger.Configurations;
using Swagger.Examples.Errors;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

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

            // swgger
            services.AddSwaggerGen(option =>
            {
                option.EnableAnnotations();
                option.OperationFilter<SwaggerHeaderFilters>();
                option.ExampleFilters();
            });
            services.AddSwaggerExamplesFromAssemblies(typeof(BadRequestApiResponseExample).Assembly);
            services.ConfigureOptions<ConfigureSwaggerOptions>();

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

            services.Configure<ApiBehaviorOptions>(option =>
            {
                option.InvalidModelStateResponseFactory = SalesOrderApiConfigurations.HandleFrameworkValidationFailure();
            });

            return services;
        }
    
        public static WebApplication AddApplicationPipelines(this WebApplication app, IApiVersionDescriptionProvider provider)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
