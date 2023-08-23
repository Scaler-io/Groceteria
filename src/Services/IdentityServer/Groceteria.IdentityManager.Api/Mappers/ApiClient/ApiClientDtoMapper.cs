using AutoMapper;
using Groceteria.IdentityManager.Api.Models.Dtos;
using IdentityServer4.EntityFramework.Entities;

namespace Groceteria.IdentityManager.Api.Mappers.ApiClient
{
    public class ApiClientDtoMapper: Profile
    {
        public ApiClientDtoMapper()
        {
            CreateMap<Client, ApiClientDto>()
                .ForMember(d => d.AllowedScopes, o => o.MapFrom(s => s.AllowedScopes.Select(i => i.Scope)))
                .ForMember(d => d.AllowedGrantTypes, o => o.MapFrom(s => s.AllowedGrantTypes.Select(i => i.GrantType)))
                .ForMember(d => d.ClientSecrets, o => o.MapFrom(s => s.ClientSecrets.Select(i => new ClientSecretDto
                {
                    Value = i.Value,
                    Description = i.Description,
                    Expiration = i.Expiration.HasValue ? i.Expiration.Value.ToString("dd/MM/yyyy") : string.Empty
                })))
                .ForMember(d => d.MetaData, o => o.MapFrom(s => new MetaDataDto
                {
                    CreatedOn = s.Created.ToString("dd/MM/yyyy"),
                    UpdatedOn = s.Updated.HasValue ? s.Updated.Value.ToString("dd/MM/yyyy") : string.Empty,
                    LastAccesseOn = s.LastAccessed.HasValue ? s.LastAccessed.Value.ToString("dd/MM/yyyy") : string.Empty 
                }))
                .ForMember(d => d.RedirectUris, o => o.MapFrom(s => s.RedirectUris.Select(i => i.RedirectUri )))
                .ForMember(d => d.PostLogoutRedirectUris, o => o.MapFrom(s => s.PostLogoutRedirectUris.Select(i => i.PostLogoutRedirectUri)))
                .ReverseMap(); 
        }
    }
}
