using FluentValidation;
using Groceteria.Identity.Shared.Data;
using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Models.Contracts;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Dtos.ApiResource;
using Groceteria.IdentityManager.Api.Models.Enums;
using Groceteria.IdentityManager.Api.Services;
using Groceteria.IdentityManager.Api.Services.ApiResource;
using Groceteria.IdentityManager.Api.Services.PaginatedRequest;
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
        private readonly IValidator<ApiResourceDto> _validator;
        private readonly IPaginatedService<ApiResourceSummary> _paginatedService;
        private readonly IApiResourceManagerService _apiResourceService;

        public ApiResourceController(ILogger logger, IIdentityService identityService,
            IValidator<ApiResourceDto> validator,
            IPaginatedService<ApiResourceSummary> paginatedService,
            IApiResourceManagerService apiResourceService)
            : base(logger, identityService)
        {
            _validator = validator;
            _paginatedService = paginatedService;
            _apiResourceService = apiResourceService;
        }

        [HttpGet("api-resource")]
        [SwaggerHeader("CorrelationId", "expects unique correlation id")]
        [SwaggerOperation(OperationId = "GetApiResource", Description = "Fetches all api resource")]
        public async Task<IActionResult> GetApiResources([FromQuery] RequestQuery query)
        {
            Logger.Here().MethodEnterd();
            query.SortField = "resourceId";
            var result = await _paginatedService.GetPaginatedData(query,
                                RequestInformation.CorrelationId,
                                SearchIndex.ApiResource);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }

        [HttpGet("api-resource/{id}")]
        [SwaggerHeader("CorrelationId", "expects unique correlation id")]
        [SwaggerOperation(OperationId = "GetApiResource", Description = "Fetches api resource by id")]
        public async Task<IActionResult> GetApiResource([FromRoute] string id)
        {
            Logger.Here().MethodEnterd();
            var result = await _apiResourceService.GetApiResource(id, RequestInformation.CorrelationId);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }

        [HttpGet("api-resource/count")]
        [SwaggerHeader("CorrelationId", "expects unique correlation id")]
        [SwaggerOperation(OperationId = "GetApiResourceCount", Description = "Fetches total api resource count")]
        public async Task<IActionResult> GetApiResourceCount()
        {
            Logger.Here().MethodEnterd();
            var result = await _paginatedService.GetCount(RequestInformation.CorrelationId, SearchIndex.ApiResource);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
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
            var result = await _apiResourceService.UpsertApiResource(apiResource, RequestInformation.CorrelationId);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }

        [HttpDelete("api-resource/delete")]
        [SwaggerHeader("CorrelationId", "expects unique correlation id")]
        [SwaggerOperation(OperationId = "DeleteApiResource", Description = "Deletes api resource")]
        public async Task<IActionResult> DeleteApiResource([FromQuery] string resourceId)
        {
            Logger.Here().MethodEnterd();
            var result = await _apiResourceService.DeleteApiResource(resourceId, RequestInformation.CorrelationId);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }
    }
}