using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrOceteria.Identity.Shared.Entities.Configurations.OAuth;

public class IdentityResourceEntityConfiguration : IEntityTypeConfiguration<IdResource>
{
    public void Configure(EntityTypeBuilder<IdResource> builder)
    {
        builder.Property(i => i.IsDefault)
        .IsRequired()
        .HasDefaultValue(false);
    }
}
