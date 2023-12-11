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
            CreatedOn = s.Created.ToString("dd/MM/yyyy HH:mm:ss tt"),
            UpdatedOn = s.Updated.HasValue ? s.Updated.Value.ToString("dd/MM/yyyy HH:mm:ss tt") : string.Empty,
            LastAccesseOn = s.LastAccessed.HasValue ? s.LastAccessed.Value.ToString("dd/MM/yyyy HH:mm:ss tt") : string.Empty
        })).ReverseMap();

        CreateMap<ApiResourceSecret, ApiResourceSecretDto>().ReverseMap();
        CreateMap<ApiResourceScope, ApiResourceScopeDto>().ReverseMap();
        CreateMap<ApiResourceClaim, ClaimsDto>().ReverseMap();
    }
}
