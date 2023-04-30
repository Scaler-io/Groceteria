using Groceteria.Discount.Grpc.DependencyInjections;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddApplicationServices(configuration);

var app = builder.Build();
app.AddApplicationPipelens();

// Configure the HTTP request pipeline.
app.Run();