using Groceteria.SalesOrder.Domain.Common;
using Groceteria.SalesOrder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Groceteria.SalesOrder.Infrastructure.Persistance
{
    public class SalesOrderContext: DbContext
    {
        public SalesOrderContext(DbContextOptions option)
            : base(option)
        {
            
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<BillingAddress> Addresses { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
        public DbSet<NotificationEmailHistory> NotificationEmailHistories { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach(var entry in ChangeTracker.Entries<EntityBase>())
            {
                switch(entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = "default";
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = "default";
                        entry.Entity.LastModifiedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = "default";
                        entry.Entity.LastModifiedAt = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
