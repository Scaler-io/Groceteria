using Groceteria.Identity.Shared.Data;
using Groceteria.Identity.Shared.Data.Interfaces;
using Groceteria.Identity.Shared.Data.Repositories;
using Groceteria.IdentityManager.Api.Services;
using Groceteria.IdentityManager.Api.Services.ApiClient;
using Groceteria.IdentityManager.Api.Services.ApiScope;
using Groceteria.IdentityManager.Api.Services.PaginatedRequest;
using Groceteria.IdentityManager.Api.Services.Search;

namespace Groceteria.IdentityManager.Api.DependencyInjections
{
    public static class BusinessLogicServiceExtensions
    {
        public static IServiceCollection AddBusinessLogicServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IClientManageService, ClientManageService>();
            services.AddScoped<IApiScopeManagerService, ApiScopeManagerService>();  
            services.AddScoped<IServiceFactory, ServiceFactory>();
            services.AddScoped<IIdentityManagerService, ClientManageService>();
            services.AddScoped<IIdentityManagerService, ApiScopeManagerService>();
            services.AddScoped(typeof(ISearchService<>), typeof(SearchService<>));
            services.AddScoped(typeof(IPaginatedService<>), typeof(PaginatedService<>));
            return services;
        }
    }
}
