using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Dtos;

namespace Groceteria.IdentityManager.Api.Services.ApiClient
{
    public interface IClientManageService : IIdentityManagerService
    {
        Task<Result<Pagination<ApiClientDto>>> GetApiClients(RequestQuery queryParams, RequestInformation requestInformation);
        Task<Result<ApiClientDto>> GetApiClient(string clientId);
        Task<Result<bool>> UpsertApiClient(ApiClientDto clientEntity, RequestInformation requestInformation);
    }
}
