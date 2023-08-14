using Groceteria.IdentityManager.Api.Configurations.Identity;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using System.Reflection;

namespace Groceteria.IdentityManager.Api.DependencyInjections
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.UseCamelCasing(true);
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = ApiVersion.Default;
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            }).AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            var identityGroupAccess = configuration
                .GetSection("IdentityGroupAccess")
                .Get<IdentityGroupAccess>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Audience = identityGroupAccess.Audience;
                    options.Authority = identityGroupAccess.Authority;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true
                    };
                });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = HandleFrameworkValidationFailure();
            });

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }

        private static Func<ActionContext, IActionResult> HandleFrameworkValidationFailure()
        {
            return context =>
            {
                var errors = context.ModelState
                                    .Where(err => err.Value.Errors.Count > 0).ToList();

                var validationError = new ApiValidationResponse
                {
                    Errors = new List<FieldLevelError>()
                };
                foreach (var error in errors)
                {
                    var fieldLevelError = new FieldLevelError()
                    {
                        Code = ErrorCodes.BadRequest.ToString(),
                        Field = error.Key,
                        Message = error.Value.Errors?.First().ErrorMessage,
                    };

                    validationError.Errors.Add(fieldLevelError);
                }
                return new UnprocessableEntityObjectResult(validationError);
            };
        }
    }
}
