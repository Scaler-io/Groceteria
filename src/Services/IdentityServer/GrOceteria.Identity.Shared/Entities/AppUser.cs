using Microsoft.AspNetCore.Identity;

namespace Groceteria.Identity.Shared.Entities
{
    public class AppUser: IdentityUser<Guid>
    {
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public ICollection<UserAddress> Addresses { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
