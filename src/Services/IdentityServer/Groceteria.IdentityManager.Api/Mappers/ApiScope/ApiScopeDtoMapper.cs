using AutoMapper;
using Groceteria.IdentityManager.Api.Models.Dtos.ApiScope;
using IdentityServer4.EntityFramework.Entities;

namespace Groceteria.IdentityManager.Api.Mappers.ApiScope
{
    public class ApiScopeDtoMapper: Profile
    {
        public ApiScopeDtoMapper()
        {
            CreateMap<IdentityServer4.EntityFramework.Entities.ApiScope, ApiScopeDto>()
                .ForMember(s => s.IsActive, o => o.MapFrom(d => d.Enabled))
                .ForMember(s => s.IsRequired, o => o.MapFrom(d => d.Required))
                .ForMember(s => s.ShouldShowInDiscovery, o => o.MapFrom(d => d.ShowInDiscoveryDocument))
                .ForMember(s => s.UserClaims, o => o.MapFrom(d => d.UserClaims))
                .ReverseMap();

            CreateMap<ApiScopeClaim, ApiScopeClaimsDto>()
                .ForMember(s => s.Type, o => o.MapFrom(d => d.Type))
                .ReverseMap();
                
        }
    }
}
