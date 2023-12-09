using AutoMapper;
using Groceteria.Identity.Shared.Entities;
using IdentityServer4.EntityFramework.Entities;

namespace Groceteria.IdentityProvider.Mapper
{
    public class ApiScopeMapper: Profile  
    {
        public ApiScopeMapper()
        {
            CreateMap<ApiScope, ApiScopeExtended>()
                .ForMember(s => s.IsDefault, o => o.MapFrom(d => true))
                .ForMember(s => s.CreatedOn, o => o.MapFrom(d => DateTime.Now));
        }
    }
}
