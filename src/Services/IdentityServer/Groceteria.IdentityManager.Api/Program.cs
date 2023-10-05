using Groceteria.IdentityManager.Api;
using Groceteria.IdentityManager.Api.DependencyInjections;
using Groceteria.IdentityManager.Api.Middlewares;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Logging;
using Serilog;
using System.Net;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var logger = Logging.GetLogger(configuration, builder.Environment);
var host = builder.Host.UseSerilog(logger);

services.AddApplicationServices(configuration)
    .AddDataAccessServices(configuration)
    .AddBusinessLogicServices();

var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

if (app.Environment.IsDevelopment())
{
    IdentityModelEventSource.ShowPII = true;
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.EnablePersistAuthorization();
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"Identity manager - {description.GroupName.ToUpperInvariant()}");
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors("GroceteriaCorsPolicy");

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<CorrelationHeaderEnricher>();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<TokenSizeValidationMiddleware>();

app.Run();
