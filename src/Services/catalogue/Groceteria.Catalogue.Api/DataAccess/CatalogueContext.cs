using Groceteria.Catalogue.Api.Configurations;
using Groceteria.Catalogue.Api.Entities;
using Groceteria.Catalogue.Api.Models.Constants;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Groceteria.Catalogue.Api.DataAccess
{
    public class CatalogueContext : ICatalogueContext
    {
        private readonly MongodbSettingOptions _mongodbOptions;
        private readonly IMongoDatabase _database;
        public CatalogueContext(IOptionsSnapshot<MongodbSettingOptions> options)
        {
            _mongodbOptions= options.Value;
            var client = new MongoClient(_mongodbOptions.ConnectionString);
            _database = client.GetDatabase(_mongodbOptions.Database);
            
            CatalogueContextSeed.SeedBrands(_database.GetCollection<Brand>(MongodbCollectionNames.Brands));
            CatalogueContextSeed.SeedCategories(_database.GetCollection<Category>(MongodbCollectionNames.Categories));
            CatalogueContextSeed.SeedProducts(_database.GetCollection<Product>(MongodbCollectionNames.Products));
        }

        public IMongoDatabase GetMongoDatabaseInstance()
        {
            return _database;
        }
    }
}
