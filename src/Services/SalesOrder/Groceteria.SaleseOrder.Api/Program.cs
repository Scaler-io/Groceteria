using Groceteria.SaleseOrder.Api.DependencyInjections;
using Groceteria.SaleseOrder.Api.Extensions;
using Groceteria.SalesOrder.Application.DependencyInjections;
using Groceteria.SalesOrder.Infrastructure.DependencyInjections;
using Groceteria.SalesOrder.Infrastructure.Persistance;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var host = builder.Host;

host.UseSerilog();
builder.Services.AddApplicationServices(configuration)
                .AddBusinessLayerServices(configuration)
                .AddInfrastructureLayerServices(configuration);

var app = builder.Build();
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.AddApplicationPipelines(apiVersionDescriptionProvider);

try
{
    app.MigrateDbAsync<SalesOrderContext>((context, service) =>
    {
        var logger = service.GetRequiredService<Serilog.ILogger>();
        SalesOrderContextSeed.SeedAsync(context, logger);
    });

    await app.RunAsync();
}
finally
{
    Log.CloseAndFlush();
}