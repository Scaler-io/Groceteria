using Groceteria.Basket.Api.DependencyInjections;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
var configuration = builder.Configuration;

builder.Services.AddApplicationServices(configuration);


var app = builder.Build();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.AddApplicationPipelines(apiVersionDescriptionProvider);
