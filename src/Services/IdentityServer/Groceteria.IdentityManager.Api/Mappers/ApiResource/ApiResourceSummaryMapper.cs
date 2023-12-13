using AutoMapper;
using Groceteria.Identity.Shared.Entities;
using Groceteria.IdentityManager.Api.Models.Contracts;
using Groceteria.IdentityManager.Api.Models.Dtos.ApiResource;

namespace Groceteria.IdentityManager.Api.Mappers.ApiResource;

public class ApiResourceSummaryMapper : Profile
{
    public ApiResourceSummaryMapper()
    {
        CreateMap<ApiResourceDto, ApiResourceSummary>()
        .ForMember(d => d.ResourceId, o => o.MapFrom(s => s.Id))
        .ForMember(d => d.IsActive, o => o.MapFrom(s => s.Enabled))
        .ForMember(d => d.CreatedOn, o => o.MapFrom(s => DateTime.Parse(s.Metadata.CreatedOn)))
        .ForMember(d => d.UpdatedOn, o =>
        {
            o.PreCondition(s => !string.IsNullOrEmpty(s.Metadata.UpdatedOn));
            o.MapFrom(s => DateTime.Parse(s.Metadata.UpdatedOn));
        });

        CreateMap<ApiResourceExtended, ApiResourceSummary>()
        .ForMember(d => d.CreatedOn, o => o.MapFrom(s => s.Created))
        .ForMember(d => d.UpdatedOn, o => o.MapFrom(s => s.Updated));

    }
}
