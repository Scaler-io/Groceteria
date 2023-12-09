using AutoMapper;
using Groceteria.IdentityManager.Api.Models.Dtos;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using static IdentityServer4.IdentityServerConstants;

namespace Groceteria.IdentityManager.Api.Mappers.ApiClient
{
    public class ApiClientDtoMapper: Profile
    {
        public ApiClientDtoMapper()
        {
            CreateMap<Identity.Shared.Entities.ApiClient, ApiClientDto>()
                .ForMember(d => d.ClientDescription, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.AllowedScopes, o => o.MapFrom(s => s.AllowedScopes.Select(i => i.Scope)))
                .ForMember(d => d.AllowedGrantTypes, o => o.MapFrom(s => s.AllowedGrantTypes.Select(i => i.GrantType)))
                .ForMember(d => d.ClientSecrets, o => o.MapFrom(s => s.ClientSecrets))
                .ForMember(d => d.MetaData, o => o.MapFrom(s => new MetaDataDto
                {
                    CreatedOn = s.Created.ToString("dd/MM/yyyy hh:mm:ss tt"),
                    UpdatedOn = s.Updated.HasValue ? s.Updated.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : string.Empty,
                    LastAccesseOn = s.LastAccessed.HasValue ? s.LastAccessed.Value.ToString("dd/MM/yyyy hh:mm:ss tt") : string.Empty
                }))
                .ForMember(d => d.RedirectUris, o => o.MapFrom(s => s.RedirectUris.Select(i => i.RedirectUri)))
                .ForMember(d => d.PostLogoutRedirectUris, o => o.MapFrom(s => s.PostLogoutRedirectUris.Select(i => i.PostLogoutRedirectUri)))
                .ReverseMap();


            CreateMap<ClientSecret, ClientSecretDto>()
                .ForMember(d => d.Expiration, o => o.MapFrom(s => s.Expiration.HasValue ? s.Expiration.Value.ToString("dd/MM/yyyy") : string.Empty));
            CreateMap<ClientSecretDto, ClientSecret>()
                .ForMember(d => d.Value, o => o.MapFrom(s => s.Value.Sha256()))
                .ForMember(d => d.Type, o => o.MapFrom(s => SecretTypes.SharedSecret));

            CreateMap<ApiClientDto, Identity.Shared.Entities.ApiClient>()
                .ForMember(d => d.Description, o => o.MapFrom(s => s.ClientDescription))
                .ForMember(d => d.AllowedScopes, o => o.MapFrom(s => s.AllowedScopes.Select(i => new ClientScope
                {
                    Scope = i
                })))
                .ForMember(d => d.AllowedGrantTypes, o => o.MapFrom(s => s.AllowedGrantTypes.Select(i => new ClientGrantType
                {
                    GrantType = i
                })))
                .ForMember(d => d.RedirectUris, o => o.MapFrom(s => s.RedirectUris.Select(i => new ClientRedirectUri
                {
                    RedirectUri = i
                })))
                .ForMember(d => d.PostLogoutRedirectUris, o => o.MapFrom(s => s.PostLogoutRedirectUris.Select(i => new ClientPostLogoutRedirectUri
                {
                    PostLogoutRedirectUri = i
                })));
        }
    }
}
