using AutoMapper;
using Groceteria.IdentityManager.Api.Models.Contracts;
using Groceteria.IdentityManager.Api.Models.Dtos.ApiScope;

namespace Groceteria.IdentityManager.Api.Mappers.ApiScope
{
    public class ApiScopeSummaryMapper: Profile
    {
        public ApiScopeSummaryMapper()
        {
            CreateMap<IdentityServer4.EntityFramework.Entities.ApiScope, ApiScopeSummary>()
                .ForMember(s => s.IsActive, o => o.MapFrom(d => d.Enabled))
                .ReverseMap();

            CreateMap<ApiScopeDto, ApiScopeSummary>().ReverseMap();
        }
    }
}
