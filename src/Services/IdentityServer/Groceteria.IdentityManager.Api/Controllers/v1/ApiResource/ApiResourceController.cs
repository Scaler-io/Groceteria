using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Services;
using Groceteria.IdentityManager.Api.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Groceteria.IdentityManager.Api.Controllers.v1.ApiResource
{

    [ApiVersion("1")]
    [Authorize]
    public class ApiResourceController : BaseApiController
    {
        public ApiResourceController(ILogger logger, IIdentityService identityService)
            : base(logger, identityService)
        {
        }

        [HttpGet("api-resource")]
        [SwaggerHeader("CorrelationId", "expects unique correlation id")]
        [SwaggerOperation(OperationId = "GetApiResource", Description = "Fetches all api resource")]
        public async Task<IActionResult> GetApiResources([FromQuery] RequestQuery query)
        {
            Logger.Here().MethodEnterd();
            Logger.Here().MethodExited();
            await Task.Yield();
            return Ok();
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
        public async Task<IActionResult> UpsertApiResource()
        {
            Logger.Here().MethodEnterd();
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