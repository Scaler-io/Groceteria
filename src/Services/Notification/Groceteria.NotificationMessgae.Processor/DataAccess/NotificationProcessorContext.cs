using Groceteria.Shared.SharedEntities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Groceteria.NotificationMessgae.Processor.DataAccess
{
    public class NotificationProcessorContext: DbContext
    {
        public NotificationProcessorContext(DbContextOptions option)
            :base(option)
        {
            
        }

        public DbSet<NotificationHistory> NotificationHistories { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach(var entry in ChangeTracker.Entries<NotificationHistory>())
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

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());  
        //}
    }
}
