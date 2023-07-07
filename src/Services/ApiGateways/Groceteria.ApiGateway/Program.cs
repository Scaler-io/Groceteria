using Groceteria.ApiGateway.DI;
using Groceteria.ApiGateway.Extensions.Logger;
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
        var logger = scope.ServiceProvider.GetRequiredService<Serilog.ILogger>();
        logger.Here().Information("logger is working");
        return "Hello world";
    }
});

try
{
    app.Run();
}
finally
{

}
