using Groceteria.Catalogue.Api.Entities;
using MongoDB.Driver;

namespace Groceteria.Catalogue.Api.DataAccess.Repositories
{
    public class MongoRepository<T> : IMongoRepository<T> where T : BaseEntity
    {
        private readonly ICatalogueContext _context;
        private readonly IMongoDatabase _database;

        public MongoRepository(ICatalogueContext context)
        {
            _context = context;
            _database = _context.GetMongoDatabaseInstance();
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync(string collectionName)
        {
            var collection = GetCollection<T>(collectionName);
            return await collection.Find(item => true).ToListAsync();
        }

        private IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
