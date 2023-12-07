using AutoMapper;
using Groceteria.Identity.Shared.Entities;
using IdentityServer4.EntityFramework.Entities;

namespace Groceteria.IdentityProvider.Mapper
{
    public class ApiResourceMapper : Profile
    {
        public ApiResourceMapper()
        {
            CreateMap<ApiResource, ApiResourceExtended>()
                .ForMember(s => s.IsDefault, o => o.MapFrom(d => true));
        }
    }
}
