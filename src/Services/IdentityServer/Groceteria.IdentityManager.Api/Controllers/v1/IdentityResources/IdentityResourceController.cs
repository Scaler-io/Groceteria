using Groceteria.IdentityManager.Api.Extensions;
using Groceteria.IdentityManager.Api.Filters;
using Groceteria.IdentityManager.Api.Models.Contracts;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Dtos.IdentityResource;
using Groceteria.IdentityManager.Api.Models.Enums;
using Groceteria.IdentityManager.Api.Services;
using Groceteria.IdentityManager.Api.Services.IdentityResources;
using Groceteria.IdentityManager.Api.Services.PaginatedRequest;
using Groceteria.IdentityManager.Api.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Groceteria.IdentityManager.Api.Controllers.v1.IdentityResources;

[ApiVersion("1")]
[Authorize]
public class IdentityResourceController : BaseApiController
{
    private readonly IIdentityResourceManagerService _idResourceService;
    private readonly IPaginatedService<IdentityResourceSummary> _paginatedService;
    public IdentityResourceController(ILogger logger, IIdentityService identityService,
    IIdentityResourceManagerService idResourceService,
    IPaginatedService<IdentityResourceSummary> paginatedService)
        : base(logger, identityService)
    {
        _idResourceService = idResourceService;
        _paginatedService = paginatedService;
    }

    [HttpGet("idresource")]
    [SwaggerHeader("CorrelationId", "expects unique correlation id")]
    [SwaggerOperation(OperationId = "GetAllIdResources", Description = "Fetches all identity resources")]
    [EnsureOwnership(Roles.All)]
    public async Task<IActionResult> GetAllIdResources([FromQuery] RequestQuery query)
    {
        Logger.Here().MethodEnterd();
        query.SortField = "resourceId";
        var result = await _paginatedService.GetPaginatedData(query, correlationId: RequestInformation.CorrelationId, SearchIndex.IdentityResource);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpGet("idresource/{id}")]
    [SwaggerHeader("CorrelationId", "expects unique correlation id")]
    [SwaggerOperation(OperationId = "GetIdResource", Description = "Fetches identity resource by id")]
    [EnsureOwnership(Roles.All)]
    public async Task<IActionResult> GetIdResource([FromRoute] string id)
    {
        Logger.Here().MethodEnterd();
        var result = await _idResourceService.GetIdentityResource(id, RequestInformation.CorrelationId);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpGet("idresource/count")]
    [SwaggerHeader("CorrelationId", "expects unique correlation id")]
    [SwaggerOperation(OperationId = "GetIdResourceCount", Description = "Fetches total identity resource count")]
    [EnsureOwnership(Roles.All)]
    public async Task<IActionResult> GetIdResourceCount()
    {
        Logger.Here().MethodEnterd();
        var result = await _paginatedService.GetCount(RequestInformation.CorrelationId, SearchIndex.IdentityResource);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpPost("idresource/upsert")]
    [SwaggerHeader("CorrelationId", "expects unique correlation id")]
    [SwaggerOperation(OperationId = "GetAllIdResources", Description = "Fetches all identity resources")]
    [EnsureOwnership(Roles.SuperAdmin, Roles.SystemAdmin)]
    public async Task<IActionResult> UpsertIdResources([FromBody] IdentityResourceDto idResource)
    {
        Logger.Here().MethodEnterd();
        var result = await _idResourceService.UpsertIdentityResource(idResource, RequestInformation.CorrelationId);
        Logger.Here().MethodExited();
        return OkOrFailure(result);
    }

    [HttpDelete("idresource/delete")]
    [SwaggerHeader("CorrelationId", "expects unique correlation id")]
    [SwaggerOperation(OperationId = "GetAllIdResources", Description = "Fetches all identity resources")]
    [EnsureOwnership(Roles.SuperAdmin)]
    public async Task<IActionResult> DeleteIdResource([FromQuery] string resourceId)
    {
        Logger.Here().MethodEnterd();
        await Task.Yield();
        Logger.Here().MethodExited();
        return Ok();
    }
}
