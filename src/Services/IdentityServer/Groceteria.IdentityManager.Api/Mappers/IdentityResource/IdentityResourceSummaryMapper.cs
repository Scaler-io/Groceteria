using AutoMapper;
using Groceteria.IdentityManager.Api.Models.Contracts;
using Groceteria.IdentityManager.Api.Models.Dtos.IdentityResource;

namespace Groceteria.IdentityManager.Api.Mappers.IdentityResource;

public class IdentityResourceSummaryMapper : Profile
{
    public IdentityResourceSummaryMapper()
    {
        CreateMap<IdentityServer4.EntityFramework.Entities.IdentityResource, IdentityResourceSummary>()
        .ForMember(d => d.ResourceId, o => o.MapFrom(s => s.Id))
        .ForMember(d => d.CreatedOn, o => o.MapFrom(s => s.Created))
        .ForMember(d => d.UpdatedOn, o => o.MapFrom(s => s.Updated))
        .ReverseMap();

        CreateMap<IdentityResourceDto, IdentityResourceSummary>()
        .ForMember(d => d.ResourceId, o => o.MapFrom(s => s.Id))
        .ForMember(d => d.CreatedOn, o => o.MapFrom(s => DateTime.Parse(s.MetaData.CreatedOn)))
        .ForMember(d => d.UpdatedOn, o =>
        {
            o.PreCondition(s => !string.IsNullOrEmpty(s.MetaData.UpdatedOn));
            o.MapFrom(s => DateTime.Parse(s.MetaData.UpdatedOn));
        }).ReverseMap();
    }
}
