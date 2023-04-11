using Groceteria.Basket.Api.Configurations;
using Groceteria.Shared.Constants;

namespace Groceteria.Basket.Api.DependencyInjections
{
    public static class HttpServiceExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration config)
        {
            var catalogueApi = new CatalogueApiSettings();
            config.GetSection("CatalogueApiSettings").Bind(catalogueApi);

            services.AddHttpClient();

            services.AddHttpClient(HttpClientNames.CatalogueApi, c =>
            {
                c.BaseAddress = new Uri(catalogueApi.BaseAddress);
                c.Timeout = new TimeSpan(0, 0, 30);
            });

            return services;
        }
    }
}
