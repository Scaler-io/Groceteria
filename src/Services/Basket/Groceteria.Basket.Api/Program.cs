using Groceteria.Basket.Api.DependencyInjections;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddApplicationServices(configuration);


var app = builder.Build();

var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.AddApplicationPipelines(apiVersionDescriptionProvider);
