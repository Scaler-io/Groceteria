using Groceteria.Identity.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Groceteria.Identity.Shared.Entities.Configurations
{
    public class UserAddressEntityConfigurations : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.HasOne(a => a.AppUser)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.AppUserId);

            builder.Property(a => a.AddressType)
                .HasConversion(
                    a => a.ToString(),
                    a => (AddressType)Enum.Parse(typeof(AddressType), a)
                );
        }
    }
}
