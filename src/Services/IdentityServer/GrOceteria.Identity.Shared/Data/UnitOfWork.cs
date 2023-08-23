using Groceteria.Identity.Shared.Data.Interfaces;
using Groceteria.Identity.Shared.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Groceteria.Identity.Shared.Data
{
    public class UnitOfWork: IUnitOfWork
    {
        private Hashtable _repositories;

        public IBaseRepository<TEntity> Repository<TEntity>(DbContext context) where TEntity : class
        {
            if (_repositories == null) _repositories = new Hashtable();
            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(BaseRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IBaseRepository<TEntity>)_repositories[type];
        }
        public async Task<int> Complete(DbContext context)
        {
            return await context.SaveChangesAsync();
        }
    }
}
