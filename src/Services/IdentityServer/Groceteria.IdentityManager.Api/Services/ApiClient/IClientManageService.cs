using Groceteria.IdentityManager.Api.Models.Core;
using Groceteria.IdentityManager.Api.Models.Dtos;
using IdentityServer4.EntityFramework.Entities;

namespace Groceteria.IdentityManager.Api.Services.ApiClient
{
    public interface IClientManageService
    {
        Task<Result<Pagination<ApiClientDto>>> GetApiClients(RequestQuery queryParams, RequestInformation requestInformation);
        Task<Result<bool>> UpsertApiClient(ApiClientDto clientEntity, RequestInformation requestInformation);
    }
}
