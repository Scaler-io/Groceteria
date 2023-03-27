using Groceteria.Catalogue.Api.Services.v2.Brands;
using Groceteria.Catalogue.Api.Services.v2.Categories;
using Groceteria.Catalogue.Api.Services.v2.Products;

namespace Groceteria.Catalogue.Api.DependencyInjections
{
    public static class ApplicationBusinessLogicExtensions
    {
        public static IServiceCollection AddBusinessLayerservices(this IServiceCollection services)
        {
            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IProductService, ProductService>();
            return services;
        }
    }
}
