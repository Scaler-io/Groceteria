using Groceteria.SalesOrder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Groceteria.SalesOrder.Infrastructure.Persistance.Configurations
{
    public class PaymentDetailsEntityConfiguration : IEntityTypeConfiguration<PaymentDetails>
    {
        public void Configure(EntityTypeBuilder<PaymentDetails> builder)
        {
            builder.HasKey(pd => pd.Id);
            builder.HasIndex(pd => pd.Id)
                .IsUnique();

            builder.Property(pd => pd.Id)
                 .IsRequired()
                 .ValueGeneratedOnAdd()
                 .HasDefaultValueSql("NEWID()");

            builder.HasMany(pd => pd.Orders)
                .WithOne(pd => pd.PaymentDetails)
                .HasForeignKey(fk => fk.PaymentId);
        }
    }
}
