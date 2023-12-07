using IdentityServer4.EntityFramework.Entities;

namespace Groceteria.Identity.Shared.Entities
{
    public class ApiClient : Client
    {
        public bool IsDefault { get; set; }
    }
}
