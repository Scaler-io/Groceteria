using Groceteria.SalesOrder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Groceteria.SalesOrder.Infrastructure.Persistance.Configurations
{
    public class BillingAddressEntityConfiguration : IEntityTypeConfiguration<BillingAddress>
    {
        public void Configure(EntityTypeBuilder<BillingAddress> builder)
        {
            builder.HasKey(ba => ba.Id);
            builder.HasIndex(ba => ba.Id)
                .IsUnique();

            builder.Property(ba => ba.AlternateMobile)
                .IsRequired(false);

            builder.Property(ba => ba.Id)
                 .IsRequired()
                 .ValueGeneratedOnAdd()
                 .HasDefaultValueSql("NEWID()");

            builder.HasMany(ba => ba.Orders)
                .WithOne(o => o.BillingAddress)
                .HasForeignKey(fk => fk.BillingId);
        }
    }
}
