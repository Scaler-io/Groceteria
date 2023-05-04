using Groceteria.Catalogue.Api.Entities;
using MongoDB.Driver;
using System.Linq.Expressions;

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

        public async Task DeleteAsync(string id, string collectionName)
        {
            var collection = GetCollection<T>(collectionName);
            FilterDefinition<T> filter = Builders<T>.Filter.Where(x => x.Id == id);
            await collection.DeleteOneAsync(filter);
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync(string collectionName, int pageSize, int pageIndex)
        {
            var collection = GetCollection<T>(collectionName);
            return await collection.Find(item => true).Skip((pageIndex - 1) * pageSize).Limit(pageSize).ToListAsync();
        }

        public async Task<T> GetByIdAsync(string id, string collectionName)
        {
            var collection = GetCollection<T>(collectionName);
            return await collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<T> GetByPredicateAsync(Expression<Func<T, bool>> predicate, string collectionName)
        {
            var collection = GetCollection<T>(collectionName);
            FilterDefinition<T> filter = Builders<T>.Filter.Where(predicate);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> GetListByPredicateAsync(Expression<Func<T, bool>> predicate, string collectionName)
        {
            var collection = GetCollection<T>(collectionName);
            FilterDefinition<T> filter = Builders<T>.Filter.Where(predicate);
            return await collection.Find(filter).ToListAsync();
        }

        public async Task UpsertAsync(T entity, string collectionName)
        {
            var collection = GetCollection<T>(collectionName);
            if (!string.IsNullOrEmpty(entity.Id) && await IsDocumentUpdateRequest(entity.Id, collection))
            {
                await collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
            }
            else
            {
                await collection.InsertOneAsync(entity);
            }
        }

        private async Task<bool> IsDocumentUpdateRequest(string id, IMongoCollection<T> collection)
        {
            var entity = await collection.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == null) return false;
            return true;
        }

        private IMongoCollection<T> GetCollection<TCollection>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
