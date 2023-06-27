using Groceteria.SalesOrder.Domain.Common;
using Groceteria.Shared.Core;
using System.Linq.Expressions;

namespace Groceteria.SalesOrder.Application.Contracts.Persistance
{
    public interface IBaseRepository<T> where T: EntityBase
    {
        Task<IReadOnlyList<T>> GetAllAsync(RequestQuery querySpec);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, RequestQuery querySpec);
        Task<IReadOnlyList<T>> GetAsync(RequestQuery querySpec, Expression<Func<T, bool>> predicate,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                        string includeString = null,
                                        bool disableTracking = true);
        Task<IReadOnlyList<T>> GetAsync(RequestQuery querySpec, Expression<Func<T, bool>> predicate,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                        List<Expression<Func<T, object>>> includse = null,
                                        bool disableTracking = true);
        Task<long> GetCount(Expression<Func<T, bool>> predicate);
        Task<T> GetByIdAsync(object id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> Completed();
    }
}
