using Groceteria.NotificationMessgae.Processor.DataAccess.Repositories;
using Groceteria.Shared.SharedEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Groceteria.NotificationMessgae.Processor.DataAccess.Configurations
{
    public class NotificationEntityConfiguration : IEntityTypeConfiguration<Shared.SharedEntities.NotificationHistory>
    {
        public void Configure(EntityTypeBuilder<Shared.SharedEntities.NotificationHistory> builder)
        {
            builder.HasKey(notification => notification.Id);
            builder.HasIndex(notification => notification.Id)
                .IsUnique();

            builder.Property(notification => notification.Id)
                 .IsRequired()
                 .ValueGeneratedOnAdd()
                 .HasDefaultValueSql("NEWID()");
        }
    }
}
