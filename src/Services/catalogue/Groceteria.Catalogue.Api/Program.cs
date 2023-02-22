using Groceteria.Catalogue.Api.DependencyInjections;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddApplicationServices(config);

var app = builder.Build();

app.AddApplicationPipelines();

await app.RunAsync();

