using Microsoft.AspNetCore.Identity;

namespace Groceteria.Identity.Shared.Entities
{
    public class AppUserRole: IdentityUserRole<Guid>
    {
        public AppUser User { get; set; }
        public AppRole Role { get; set; }
    }
}
