using Microsoft.AspNetCore.Identity;

namespace Groceteria.Identity.Shared.Entities
{
    public class AppRole: IdentityRole<Guid>
    {
        public IEnumerable<AppUserRole> UserRoles { get; set; }
    }
}
