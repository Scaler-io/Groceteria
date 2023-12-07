using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Groceteria.Identity.Shared.Entities.Configurations.OAuth
{
    internal class ApiClientEntityConfiguration : IEntityTypeConfiguration<ApiClient>
    {
        public void Configure(EntityTypeBuilder<ApiClient> builder)
        {
            builder.Property(client => client.IsDefault)
                .HasDefaultValue(false);
        }
    }
}
