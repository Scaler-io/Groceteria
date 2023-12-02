using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Dtos.ApiScope;

namespace Groceteria.IdentityManager.Api.Services.ApiScope
{
    public interface IApiScopeManagerService
    {
        Task<Result<Pagination<ApiScopeDto>>> GetApiScopes(RequestQuery query, string correaltionId);
        Task<Result<ApiScopeDto>> GetApiScope(int id, string correlationId);
        Task<Result<bool>> UpsertApiScope(ApiScopeDto scopeEntity, string correlationId);
    }
}
