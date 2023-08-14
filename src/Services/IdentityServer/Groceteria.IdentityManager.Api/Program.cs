using Groceteria.IdentityManager.Api;
using Groceteria.IdentityManager.Api.DependencyInjections;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var logger = Logging.GetLogger(configuration, builder.Environment);
var host = builder.Host.UseSerilog(logger);

services.AddApplicationServices(configuration)
    .AddDataAccessServices(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
