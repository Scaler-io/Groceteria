using Groceteria.Identity.Shared.Entities;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace Groceteria.Identity.Shared.Data
{
    public class GroceteriaOauthDbContext: ConfigurationDbContext<GroceteriaOauthDbContext>
    {
        public GroceteriaOauthDbContext(DbContextOptions<GroceteriaOauthDbContext> options,
            ConfigurationStoreOptions storeOptions)
            : base(options, storeOptions)
        {

        }

        public DbSet<ApiScopeExtended> ApiScopesExtended { get; set; }
        public DbSet<ApiClient> ApiClients { get; set; }
        public DbSet<ApiResourceExtended> ApiResourcesExtended { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApiScopeExtended>().ToTable("ApiScopes");
            modelBuilder.Entity<ApiClient>().ToTable("Clients");
            modelBuilder.Entity<ApiResourceExtended>().ToTable("ApiResources");
        }
    }

    public class GroceteriaPersistedDbContext: PersistedGrantDbContext<GroceteriaPersistedDbContext>
    {
        public GroceteriaPersistedDbContext(DbContextOptions<GroceteriaPersistedDbContext> options,
                    OperationalStoreOptions storeOptions)
                    : base(options, storeOptions)
        {

        }
    }
}
