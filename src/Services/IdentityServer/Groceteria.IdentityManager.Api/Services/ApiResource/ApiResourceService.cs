using Groceteria.IdentityManager.Api.Models.Contracts;
using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Dtos.ApiResource;

namespace Groceteria.IdentityManager.Api.Services.ApiResource;

public class ApiResourceService : IApiResourceService
{
    public Task<Result<Pagination<ApiResourceSummary>>> GetallApiResources(RequestQuery query, string correlationId)
    {
        throw new NotImplementedException();
    }
    public Task<Result<ApiResourceDto>> GetApiResource(string resourceId, string correlationId)
    {
        throw new NotImplementedException();
    }
    public Task<Result<bool>> DeleteApiResource(string resourceId, string correlationId)
    {
        throw new NotImplementedException();
    }
    public Task<Result<bool>> UpsertApiResource(ApiResourceDto apiResource, string correlationId)
    {
        throw new NotImplementedException();
    }
}
