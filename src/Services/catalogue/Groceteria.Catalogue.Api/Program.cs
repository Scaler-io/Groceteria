using Groceteria.Catalogue.Api.DependencyInjections;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var host = builder.Host;

host.UseSerilog();

builder.Services.AddApplicationServices(config);

var app = builder.Build();
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.AddApplicationPipelines(apiVersionDescriptionProvider);

await app.RunAsync();

