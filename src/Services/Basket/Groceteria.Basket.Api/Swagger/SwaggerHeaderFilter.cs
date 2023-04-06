﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Groceteria.Basket.Api.Swagger
{
    public class SwaggerHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var headers = context.MethodInfo?.GetCustomAttributes(true).OfType<SwaggerHeaderAttribute>();
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }
            foreach (var header in headers)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = header.Name,
                    In = ParameterLocation.Header,
                    Description = header.Description,
                    Required = header.Required,
                });
            }
        }
    }
}
