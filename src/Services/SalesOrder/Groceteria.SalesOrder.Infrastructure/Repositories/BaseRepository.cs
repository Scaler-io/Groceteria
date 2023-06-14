using Groceteria.SalesOrder.Application.Contracts.Persistance;
using Groceteria.SalesOrder.Domain.Common;
using Groceteria.SalesOrder.Infrastructure.Persistance;
using Groceteria.Shared.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Groceteria.SalesOrder.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : EntityBase
    {
        public SalesOrderContext _context;

        public BaseRepository(SalesOrderContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(RequestQuery querySpec)
        {
            return await _context.Set<T>()
                .Skip((querySpec.PageIndex - 1) * querySpec.PageSize)
                .Take(querySpec.PageSize)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, 
            RequestQuery querySpec)
        {
            return await _context.Set<T>()
                .Where(predicate)
                .Skip(querySpec.PageIndex-1 * querySpec.PageSize)
                .Take(querySpec.PageSize)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(RequestQuery querySpec,
            Expression<Func<T, bool>> predicate, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            string includeString = null, 
            bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();
            if (disableTracking) query.AsNoTracking();
            if (!string.IsNullOrEmpty(includeString)) query = query.Include(includeString);
            if (predicate != null) query = query.Where(predicate).Skip(querySpec.PageIndex - 1 * querySpec.PageSize).Take(querySpec.PageSize);
            //if (orderBy != null) return await orderBy(query).ToListAsync();
            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(RequestQuery querySpec, 
            Expression<Func<T, bool>> predicate, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            List<Expression<Func<T, object>>> includse = null, 
            bool disableTracking = true)
        {
            IQueryable<T> query = _context.Set<T>();
            if(disableTracking) query.AsNoTracking();
            if(includse != null) query = includse.Aggregate(query, (current, include) => current.Include(include));
            if (predicate != null) query = query.Where(predicate)
                    .Skip((querySpec.PageIndex - 1) * querySpec.PageSize)
                    .Take(querySpec.PageSize);
            if (orderBy != null) return await orderBy(query).ToListAsync();
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task<T> AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();         
        }

        public async Task<int> Completed()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
