using Groceteria.SalesOrder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Groceteria.SalesOrder.Infrastructure.Persistance.Configurations
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasIndex(o => o.Id)
                .IsUnique();

            builder.Property(o => o.Id)
                 .IsRequired()
                 .ValueGeneratedOnAdd()
                 .HasDefaultValueSql("NEWID()");

            builder.HasOne(o => o.BillingAddress)
                .WithMany(ba => ba.Orders)
                .HasForeignKey(fk => fk.BillingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.PaymentDetails)
                .WithMany(pd => pd.Orders)
                .HasForeignKey(fk => fk.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
