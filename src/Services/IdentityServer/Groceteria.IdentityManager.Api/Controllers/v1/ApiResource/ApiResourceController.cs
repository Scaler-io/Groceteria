using FluentValidation;
using Groceteria.Identity.Shared.Data;
using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Dtos.ApiResource;
using Groceteria.IdentityManager.Api.Services;
using Groceteria.IdentityManager.Api.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Groceteria.IdentityManager.Api.Controllers.v1.ApiResource
{

    [ApiVersion("1")]
    [Authorize]
    public class ApiResourceController : BaseApiController
    {
        private readonly GroceteriaOauthDbContext _dbContext;
        private readonly IValidator<ApiResourceDto> _validator;
        public ApiResourceController(ILogger logger, IIdentityService identityService,
            GroceteriaOauthDbContext dbContext,
            IValidator<ApiResourceDto> validator)
            : base(logger, identityService)
        {
            _dbContext = dbContext;
            _validator = validator;
        }

        [HttpGet("api-resource")]
        [SwaggerHeader("CorrelationId", "expects unique correlation id")]
        [SwaggerOperation(OperationId = "GetApiResource", Description = "Fetches all api resource")]
        public async Task<IActionResult> GetApiResources([FromQuery] RequestQuery query)
        {
            Logger.Here().MethodEnterd();
            Logger.Here().MethodExited();
            var apiResources = await _dbContext.ApiResourcesExtended.ToListAsync();
            return Ok(apiResources);
        }

        [HttpGet("api-resource/{id}")]
        [SwaggerHeader("CorrelationId", "expects unique correlation id")]
        [SwaggerOperation(OperationId = "GetApiResource", Description = "Fetches api resource by id")]
        public async Task<IActionResult> GetApiResource([FromRoute] string id)
        {
            Logger.Here().MethodEnterd();
            Logger.Here().MethodExited();
            await Task.Yield();
            return Ok();
        }

        [HttpGet("api-resource/count")]
        [SwaggerHeader("CorrelationId", "expects unique correlation id")]
        [SwaggerOperation(OperationId = "GetApiResourceCount", Description = "Fetches total api resource count")]
        public async Task<IActionResult> GetApiResourceCount()
        {
            Logger.Here().MethodEnterd();
            Logger.Here().MethodExited();
            await Task.Yield();
            return Ok();
        }

        [HttpPost("api-resource/upsert")]
        [SwaggerHeader("CorrelationId", "expects unique correlation id")]
        [SwaggerOperation(OperationId = "UpsertApiResource", Description = "Creates or updates total api resource")]
        public async Task<IActionResult> UpsertApiResource([FromBody] ApiResourceDto apiResource)
        {
            Logger.Here().MethodEnterd();

            var validationResult = _validator.Validate(apiResource);
            if (!validationResult.IsValid)
            {
                return Failure(validationResult);
            }

            Logger.Here().MethodExited();
            await Task.Yield();
            return Ok();
        }

        [HttpDelete("api-resource/delete/{id}")]
        [SwaggerHeader("CorrelationId", "expects unique correlation id")]
        [SwaggerOperation(OperationId = "DeleteApiResource", Description = "Deletes api resource")]
        public async Task<IActionResult> DeleteApiResource()
        {
            Logger.Here().MethodEnterd();
            Logger.Here().MethodExited();
            await Task.Yield();
            return Ok();
        }
    }
}