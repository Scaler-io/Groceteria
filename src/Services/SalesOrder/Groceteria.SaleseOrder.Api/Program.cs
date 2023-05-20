using Groceteria.SaleseOrder.Api.DependencyInjections;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var host = builder.Host;

host.UseSerilog();
builder.Services.AddApplicationServices(configuration);

var app = builder.Build();
app.AddApplicationPipelines();

try
{
    await app.RunAsync();
}
finally
{
    Log.CloseAndFlush();
}