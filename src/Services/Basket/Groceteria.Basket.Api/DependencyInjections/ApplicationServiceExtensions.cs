using Groceteria.Basket.Api.Configurations;
using Groceteria.Basket.Api.Middlewares;
using Groceteria.Basket.Api.Swagger;
using Groceteria.Discount.Grpc.Protos;
using Groceteria.Infrastructure.Logger;
using Grpc.Core;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Newtonsoft.Json.Converters;
using RabbitMQ.Client;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace Groceteria.Basket.Api.DependencyInjections
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
                    
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.OperationFilter<SwaggerHeaderFilter>();
                options.ExampleFilters();
            });

            services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());

            // serilog
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var logIndexPattern = $"Groceteria.Basket.Api-{env?.ToLower().Replace(".", "-")}";
            var logger = LoggerConfig.Configure(configuration, logIndexPattern);
            services.AddSingleton(Log.Logger)
                    .AddSingleton(x => logger);

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            // grpc client
            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
            {
                options.Address = new Uri(configuration["GrpcSettings:DiscountUrl"]);
                options.ChannelOptionsActions.Add(channelOptions => channelOptions.Credentials = ChannelCredentials.Insecure);
            });

            // masstransit-Rabbitmq
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(configuration["RabbitMq:ConnectionString"]);
                });
            });

            // http
            services.AddHttpContextAccessor();

            // versioning
            services.AddApiVersioning(option =>
            {
                option.DefaultApiVersion = new ApiVersion(1, 0);
                option.ReportApiVersions = true;
                option.AssumeDefaultVersionWhenUnspecified = true;
            })
            .AddVersionedApiExplorer(option =>
            {
                option.GroupNameFormat = "'v'VVV";
                option.SubstituteApiVersionInUrl = true;
            });

            // mapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // swagger
            services.ConfigureOptions<ConfigureSwaggerOptions>();

            // api response behaviour
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = BasketApiConfigurations.HandleFrameworkValidationFailure();
            });

            services.Configure<CatalogueApiSettings>(configuration.GetSection(CatalogueApiSettings.catalogueApiSettings));

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

            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<CorrelationHeaderEnricher>();
            app.UseMiddleware<GlobalExceptionMiddleware>();

            app.Run();

            return app;
        }
    }
}
