using AutoMapper;
using Groceteria.Identity.Shared.Entities;
using Groceteria.IdentityManager.Api.Models.Contracts;
using Groceteria.IdentityManager.Api.Models.Dtos.ApiScope;

namespace Groceteria.IdentityManager.Api.Mappers.ApiScope
{
    public class ApiScopeSummaryMapper: Profile
    {
        public ApiScopeSummaryMapper()
        {
            CreateMap<ApiScopeExtended, ApiScopeSummary>()
                .ForMember(s => s.ScopeId, o => o.MapFrom(d => d.Id))
                .ForMember(s => s.IsActive, o => o.MapFrom(d => d.Enabled))
                .ReverseMap();

            CreateMap<ApiScopeDto, ApiScopeSummary>()
                .ForMember(s => s.ScopeId, o => o.MapFrom(d => d.Id))
                .ReverseMap();
        }
    }
}
