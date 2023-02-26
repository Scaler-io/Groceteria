using Groceteria.Catalogue.Api.Entities;
using Groceteria.Shared.Helpers;
using MongoDB.Driver;

namespace Groceteria.Catalogue.Api.DataAccess
{
    public class CatalogueContextSeed
    {
        public static void SeedBrands(IMongoCollection<Brand> brandsCollections)
        {
            bool existBrand = brandsCollections.Find(brand => true).Any();
            if(!existBrand)
            {
                brandsCollections.InsertMany(GetPreconfiguredBrands());
            }
        }
         
        public static void SeedCategories(IMongoCollection<Category> categoriesCollections)
        {
            bool existCategory = categoriesCollections.Find(category => true).Any();
            if(!existCategory)
            {
                categoriesCollections.InsertMany(GetPreconfiguredCategory());
            }
        }

        public static void SeedProducts(IMongoCollection<Product> productsCollections)
        {
            bool existProduct = productsCollections.Find(product => true).Any();
            if(!existProduct)
            {
                productsCollections.InsertMany(GetPreconfiguredProducts());
            }
        }

        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            var products = FileReaderHelper<Product>.SeederFileReader("Products");
            return products;
        }

        private static IEnumerable<Category> GetPreconfiguredCategory()
        {
            var categories = FileReaderHelper<Category>.SeederFileReader("Categories");
            return categories;
        }

        private static IEnumerable<Brand> GetPreconfiguredBrands()
        {
            var brands = FileReaderHelper<Brand>.SeederFileReader("Brands");
            return brands;
        }
    }
}
