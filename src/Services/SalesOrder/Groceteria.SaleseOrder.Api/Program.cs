using Groceteria.SaleseOrder.Api.DependencyInjections;
using Groceteria.SalesOrder.Application.DependencyInjections;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var host = builder.Host;

host.UseSerilog();
builder.Services.AddApplicationServices(configuration)
                .AddBusinessLayerServices(configuration);

var app = builder.Build();
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.AddApplicationPipelines(apiVersionDescriptionProvider);

try
{
    await app.RunAsync();
}
finally
{
    Log.CloseAndFlush();
}