﻿using Groceteria.SalesOrder.Domain.Common;
using Groceteria.Shared.Core;
using System.Linq.Expressions;

namespace Groceteria.SalesOrder.Application.Contracts.Persistance
{
    public interface IBaseRepository<T> where T: EntityBase
    {
        Task<IReadOnlyList<T>> GetAllAsync(RequestQuery query);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                        string includeString = null,
                                        bool disableTracking = true);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                        List<Expression<Func<T, object>>> includse = null,
                                        bool disableTracking = true);
        Task<T> GetByIdAsync(object id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}