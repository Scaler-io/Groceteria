using AutoMapper;
using Groceteria.IdentityManager.Api.Models.Dtos;
using Groceteria.IdentityManager.Api.Models.Dtos.IdentityResource;
using IdentityServer4.EntityFramework.Entities;

namespace Groceteria.IdentityManager.Api.Mappers.IdentityResource;

public class IdentityResourceDtoMapper : Profile
{
    public IdentityResourceDtoMapper()
    {
        CreateMap<IdentityServer4.EntityFramework.Entities.IdentityResource, IdentityResourceDto>()
        .ForMember(d => d.MetaData, o => o.MapFrom(s => new MetaDataDto
        {
            CreatedOn = s.Created.ToString("dd/MM/yyyy HH:mm:ss tt"),
            UpdatedOn = s.Updated.HasValue ? s.Updated.Value.ToString("dd/MM/yyyy HH:mm:ss tt") : string.Empty
        })).ReverseMap();

        CreateMap<IdentityResourceClaim, ClaimsDto>().ReverseMap();
    }
}
