using Groceteria.IdentityProvider;
using Groceteria.IdentityProvider.DependencyInjections;
using Groceteria.IdentityProvider.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var host = builder.Host;
var environment = builder.Environment;

var logger = Logging.GetLogger(configuration, environment);
var loger = host.UseSerilog(logger);

services.AddApplicationServices(configuration)
    .AddIdentityServices(configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseIdentityServer();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

//app.MapGet("/", () =>
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
//        logger.Here().WithCorrelationId(Guid.NewGuid().ToString()).Information("Enriching correlation id");
//    }
//    return "Hello World!";
//});

await app.MigrateAsycn(configuration);

app.Run();
