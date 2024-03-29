﻿using AutoMapper;
using Groceteria.Identity.Shared.Entities;
using Groceteria.IdentityManager.Api.Models.Dtos;
using Groceteria.IdentityManager.Api.Models.Dtos.ApiScope;
using IdentityServer4.EntityFramework.Entities;

namespace Groceteria.IdentityManager.Api.Mappers.ApiScope
{
    public class ApiScopeDtoMapper: Profile
    {
        public ApiScopeDtoMapper()
        {
            CreateMap<ApiScopeExtended, ApiScopeDto>()
                .ForMember(s => s.IsActive, o => o.MapFrom(d => d.Enabled))
                .ForMember(s => s.IsRequired, o => o.MapFrom(d => d.Required))
                .ForMember(s => s.ShouldShowInDiscovery, o => o.MapFrom(d => d.ShowInDiscoveryDocument))
                .ForMember(s => s.UserClaims, o => o.MapFrom(d => d.UserClaims))
                .ReverseMap();

            CreateMap<ApiScopeClaim, ClaimsDto>()
                .ForMember(s => s.Type, o => o.MapFrom(d => d.Type))
                .ReverseMap();
                
        }
    }
}
