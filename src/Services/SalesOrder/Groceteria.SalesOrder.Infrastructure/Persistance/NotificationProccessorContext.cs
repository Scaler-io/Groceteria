using Groceteria.Shared.SharedEntities;
using Microsoft.EntityFrameworkCore;

namespace Groceteria.SalesOrder.Infrastructure.Persistance
{
    public class NotificationProccessorContext: DbContext
    {
        public NotificationProccessorContext(DbContextOptions<NotificationProccessorContext> options)
            :base(options)
        {
            
        }

        public DbSet<NotificationHistory> NotificationHistories { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<NotificationHistory>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
