using Groceteria.IdentityManager.Api.Models.Contracts;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Dtos.ApiResource;

namespace Groceteria.IdentityManager.Api.Services.ApiResource;

public interface IApiResourceService
{
    Task<Result<Pagination<ApiResourceSummary>>> GetallApiResources(RequestQuery query, string correlationId);
    Task<Result<ApiResourceDto>> GetApiResource(string resourceId, string correlationId);
    Task<Result<bool>> UpsertApiResource(ApiResourceDto apiResource, string correlationId);
    Task<Result<bool>> DeleteApiResource(string resourceId, string correlationId);
}
