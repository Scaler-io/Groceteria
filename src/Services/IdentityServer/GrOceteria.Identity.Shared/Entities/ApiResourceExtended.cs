using IdentityServer4.EntityFramework.Entities;

namespace Groceteria.Identity.Shared.Entities
{
    public class ApiResourceExtended : ApiResource
    {
        public bool IsDefault { get; set; }
    }
}
