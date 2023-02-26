namespace Groceteria.Catalogue.Api.DataAccess.Repositories
{
    public interface IMongoRepository<T>
    {
        Task<IReadOnlyCollection<T>> GetAllAsync(string collectionName);
    }
}
