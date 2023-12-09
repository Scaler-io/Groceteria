using AutoMapper;
using Groceteria.Identity.Shared.Entities;
using Groceteria.IdentityManager.Api.Models.Dtos;
using Groceteria.IdentityManager.Api.Models.Dtos.ApiResource;
using IdentityServer4.EntityFramework.Entities;

namespace Groceteria.IdentityManager.Api.Mappers.ApiResource;

public class ApiResourceDtoMapper : Profile
{
    public ApiResourceDtoMapper()
    {
        CreateMap<ApiResourceExtended, ApiResourceDto>()
        .ForMember(d => d.Metadata, o => o.MapFrom(s => new MetaDataDto
        {
            CreatedOn = s.Created.ToString("dd/MM/YYYY HH:mm:ssZ"),
            UpdatedOn = s.Updated.Value.ToString("dd/MM/YYYY HH:mm:ssZ"),
            LastAccesseOn = s.LastAccessed.Value.ToString("dd/MM/YYYY HH:mm:ssZ")
        }));

        CreateMap<ApiResourceSecret, ApiResourceSecretDto>();
        CreateMap<ApiResourceScope, ApiResourceScopeDto>();
        CreateMap<ApiResourceClaim, ClaimsDto>();
    }
}
