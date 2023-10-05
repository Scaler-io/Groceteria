using Groceteria.Identity.Shared.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Groceteria.Identity.Shared.Entities
{
    [Table("Addresses")]
    public class UserAddress: BaseEntity
    {
        public AddressType AddressType { get; set; }
        public string AddressLine1 { get; set; }
        public string Landmark { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
