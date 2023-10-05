using Microsoft.EntityFrameworkCore;

namespace Groceteria.Identity.Shared.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IBaseRepository<TEntity> Repository<TEntity>(DbContext context) where TEntity : class;
        Task<int> Complete(DbContext context);
    }
}
