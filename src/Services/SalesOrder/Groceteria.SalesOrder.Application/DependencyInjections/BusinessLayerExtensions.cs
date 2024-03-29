﻿using FluentValidation;
using Groceteria.SalesOrder.Application.Configurations;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Groceteria.SalesOrder.Application.DependencyInjections
{
    public static class BusinessLayerExtensions
    {
        public static IServiceCollection AddBusinessLayerServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.Configure<EmailTemplates>(configuration.GetSection(EmailTemplates.EmailTemplatesConfiguration));
            return services;
        }
    }
}
