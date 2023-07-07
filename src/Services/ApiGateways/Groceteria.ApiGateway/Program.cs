using Groceteria.ApiGateway.DI;
using Groceteria.ApiGateway.Extensions.Logger;
using Groceteria.ApiGateway.Middlewares;
using Ocelot.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var services = builder.Services;
var host = builder.Host;

host.UseSerilog();
services.AddGatewayServices(config);

var app = builder.Build();

app.MapGet("/", () =>
{
    var serviceProvider = app.Services;
    using (var scope = serviceProvider.CreateScope())
    {
        //throw new Exception("unhandled exception");
        var logger = scope.ServiceProvider.GetRequiredService<Serilog.ILogger>();
        logger.Here().Information("logger is working");
        return "Hello world";
    }
});

app.UseMiddleware<GlobalExceptionMiddleware>();

try
{
    await app.UseOcelot();
}
finally
{
    Log.CloseAndFlush();
}
