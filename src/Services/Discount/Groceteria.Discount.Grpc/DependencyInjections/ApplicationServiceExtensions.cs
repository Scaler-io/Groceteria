﻿using Groceteria.Discount.Grpc.Middlewares;
using Groceteria.Discount.Grpc.Services;
using Groceteria.Infrastructure.Logger;
using Grpc.AspNetCore.Server;
using Serilog;
using System.Reflection;

namespace Groceteria.Discount.Grpc.DependencyInjections
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var logIndexPattern = $"Groceteria.Dicount.Grpc-{env?.ToLower().Replace(".", "-")}";
            var logger = LoggerConfig.Configure(configuration, logIndexPattern);
            services.AddSingleton(Log.Logger)
                    .AddSingleton(x => logger);
            services.AddHttpContextAccessor();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddGrpc();
            services.Configure<GrpcServiceOptions>(options =>
            {
                options.EnableDetailedErrors = true;
            });
            return services;
        }

        public static WebApplication AddApplicationPipelens(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.MapGrpcService<DiscountService>();

            app.MapGet("/", async context =>
            {
                await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            });

            return app;
        }
    }
}
