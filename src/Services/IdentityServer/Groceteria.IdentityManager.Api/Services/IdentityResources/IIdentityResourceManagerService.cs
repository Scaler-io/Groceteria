using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Dtos.IdentityResource;

namespace Groceteria.IdentityManager.Api.Services.IdentityResources;

public interface IIdentityResourceManagerService : IIdentityManagerService
{
    Task<Result<IReadOnlyList<IdentityResourceDto>>> GetIdentityResources(string correlationId);
    Task<Result<IdentityResourceDto>> GetIdentityResource(string resourecId, string correlationId);
    Task<Result<bool>> UpsertIdentityResource(IdentityResourceDto idResource, string correlationId);
    Task<Result<bool>> DeleteIdentityResource(string resourceId, string correlationId);
}
