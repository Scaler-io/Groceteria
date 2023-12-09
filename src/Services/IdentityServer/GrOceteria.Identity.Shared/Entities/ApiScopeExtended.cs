using IdentityServer4.EntityFramework.Entities;

namespace Groceteria.Identity.Shared.Entities
{
    public class ApiScopeExtended: ApiScope
    {
        public bool IsDefault { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
