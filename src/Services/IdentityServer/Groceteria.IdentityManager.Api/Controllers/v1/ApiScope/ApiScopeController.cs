using FluentValidation;
using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Filters;
using Groceteria.IdentityManager.Api.Models.Contracts;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Dtos.ApiScope;
using Groceteria.IdentityManager.Api.Models.Enums;
using Groceteria.IdentityManager.Api.Services;
using Groceteria.IdentityManager.Api.Services.ApiScope;
using Groceteria.IdentityManager.Api.Services.PaginatedRequest;
using Groceteria.IdentityManager.Api.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Groceteria.IdentityManager.Api.Controllers.v1.ApiScope
{
    [ApiVersion("1")]
    [Authorize]
    public class ApiScopeController : BaseApiController
    {
        private readonly IPaginatedService<ApiScopeSummary> _paginatedScopeService;
        private readonly IApiScopeManagerService _apiScopeService;
        private readonly IValidator<ApiScopeDto> _validator;

        public ApiScopeController(ILogger logger,
            IIdentityService identityService,
            IPaginatedService<ApiScopeSummary> paginatedScopeService,
            IApiScopeManagerService apiScopeService,
            IValidator<ApiScopeDto> validator)
            : base(logger, identityService)
        {
            _paginatedScopeService = paginatedScopeService;
            _apiScopeService = apiScopeService;
            _validator = validator;
        }

        [HttpGet("api-scopes")]
        [SwaggerHeader("CorrelationId", "expects unique correlation id")]
        [SwaggerOperation(OperationId = "GetApiScopes", Description = "Fetches all api scopes")]
        [EnsureOwnership(Roles.All)]
        public async Task<IActionResult> GetApiScopes([FromQuery] RequestQuery query)
        {
            Logger.Here().MethodEnterd();
            query.SortField = "scopeId";
            var result = await _paginatedScopeService.GetPaginatedData(query, RequestInformation.CorrelationId, SearchIndex.ApiScope);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }

        [HttpGet("api-scopes/{id}")]
        [SwaggerHeader("CorrelationId", "expects unique correlation id")]
        [SwaggerOperation(OperationId = "GetApiScope", Description = "Fetches api scope details using id")]
        [EnsureOwnership(Roles.All)]
        public async Task<IActionResult> GetApiScope([FromRoute] string id)
        {
            Logger.Here().MethodEnterd();
            var result = await _apiScopeService.GetApiScope(System.Int32.Parse(id), RequestInformation.CorrelationId);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }

        [HttpPost("api-scope/upsert")]
        [SwaggerHeader("CorrelationId", "expects unique correlation id")]
        [SwaggerOperation(OperationId = "UpsertApiScope", Description = "Creates or update api scope details using id")]
        public async Task<IActionResult> UpsertApiScope([FromBody] ApiScopeDto scope)
        {
            Logger.Here().MethodEnterd();

            var validationResult = _validator.Validate(scope);

            if (!validationResult.IsValid)
            {
                return Failure(validationResult);
            }

            var result = await _apiScopeService.UpsertApiScope(scope, RequestInformation.CorrelationId);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }

        [HttpDelete("api-scope/delete/{id}")]
        [SwaggerHeader("CorrelationId", "expects unique correlation id")]
        [SwaggerOperation(OperationId = "UpsertApiScope", Description = "Deletes api scope details using id")]
        public async Task<IActionResult> DeleteApiScope([FromRoute] string id)
        {
            Logger.Here().MethodEnterd();
            var result = await _apiScopeService.DeleteApiScope(id, RequestInformation.CorrelationId);
            Logger.Here().MethodExited();
            return OkOrFailure(result);
        }
    }
}
