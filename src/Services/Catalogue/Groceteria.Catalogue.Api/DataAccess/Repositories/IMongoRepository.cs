using System.Linq.Expressions;

namespace Groceteria.Catalogue.Api.DataAccess.Repositories
{
    public interface IMongoRepository<T>
    {
        Task<IReadOnlyCollection<T>> GetAllAsync(string collectionName, int pageSize, int pageIndex);
        Task<IReadOnlyList<T>> GetListByPredicateAsync(Expression<Func<T, bool>> predicate, string collectionName);
        Task<T> GetByIdAsync(string id, string collectionName);
        Task<T> GetByPredicateAsync(Expression<Func<T, bool>> predicate, string collectionName);
        Task UpsertAsync(T entity, string collectionName);
        Task DeleteAsync(string id, string collectionName); 
    }
}
