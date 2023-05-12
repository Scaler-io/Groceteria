using Groceteria.Discount.Grpc.DependencyInjections;
using Groceteria.Discount.Grpc.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

builder.Host.UseSerilog();
services.AddApplicationServices(configuration);

var app = builder.Build();
app.AddApplicationPipelens();

try
{
    app.MigrateDatabase();
    await app.RunAsync();
}
finally
{
    Log.CloseAndFlush();
}