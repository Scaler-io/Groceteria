using Groceteria.Identity.Shared.Data;
using Groceteria.Identity.Shared.Data.Interfaces;
using Groceteria.Identity.Shared.Data.Repositories;
using Groceteria.IdentityManager.Api.Services.ApiClient;

namespace Groceteria.IdentityManager.Api.DependencyInjections
{
    public static class BusinessLogicServiceExtensions
    {
        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IClientManageService, ClientManageService>();
            return services;
        }
    }
}
