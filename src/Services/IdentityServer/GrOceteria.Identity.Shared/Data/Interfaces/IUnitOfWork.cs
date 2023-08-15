namespace Groceteria.Identity.Shared.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IBaseRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<int> Complete();
    }
}
