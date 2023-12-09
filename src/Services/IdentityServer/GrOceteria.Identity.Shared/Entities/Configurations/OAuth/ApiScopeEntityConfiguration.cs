using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Groceteria.Identity.Shared.Entities.Configurations.OAuth
{
    public class ApiScopeEntityConfiguration : IEntityTypeConfiguration<ApiScopeExtended>
    {
        public void Configure(EntityTypeBuilder<ApiScopeExtended> builder)
        {
            builder.ToTable("ApiScopes");

            builder.Property(scope => scope.IsDefault)
                .IsRequired()
                .HasDefaultValue(false);
            builder.Property(scope => scope.CreatedOn)
                .HasDefaultValue(DateTime.Now);
        }
    }
}
