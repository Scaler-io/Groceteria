using Groceteria.ApiGateway.DI;
using Groceteria.ApiGateway.Middlewares;
using Ocelot.Configuration;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var services = builder.Services;
var host = builder.Host;

host.ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile($"ocelot.{context.HostingEnvironment.EnvironmentName}.json", true, true);
    });

services.AddGatewayServices(host, config);
var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/", () =>  "Hello world");

app.UseCors("GroceteriaCorsPolicy");

app.UseMiddleware<ValidateSubscriptionKeyMiddleware>();

app.UseMiddleware<GlobalExceptionMiddleware>();

try
{
    await app.UseOcelot();
    app.Run();
}
finally
{
    Log.CloseAndFlush();
}
