using AutoMapper;
using Groceteria.Identity.Shared.Entities;
using IdentityServer4.EntityFramework.Entities;

namespace Groceteria.IdentityProvider.Mapper
{
    public class ApiClientMapper: Profile
    {
        public ApiClientMapper()
        {
            CreateMap<Client, ApiClient>()
                .ForMember(c => c.IsDefault, o => o.MapFrom(d => true));
        }
    }
}
