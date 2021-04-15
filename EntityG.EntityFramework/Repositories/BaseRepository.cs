using EntityG.EntityFramework.Entities;
using EntityG.EntityFramework.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityG.EntityFramework.Repositories
{
    public class BaseRepository<T> : IRepository<T>, IDisposable where T : class
    {
        private readonly DbSet<T> _dbSet;
        private readonly DbContext _context;

        public BaseRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
        }

        public IQueryable<T> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<T> GetQueryable(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int? countSkip = null, int? countTake = null,
            bool disableTracking = false)
        {
            IQueryable<T> query = GetQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (countSkip.HasValue)
            {
                query = query.Skip(countSkip.Value);
            }

            if (countTake.HasValue)
            {
                query = query.Take(countTake.Value);
            }

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public T GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public List<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? countSkip = null,
            int? countTake = null,
            bool disableTracking = false)
        {
            return GetQueryable(filter, includes, orderBy, countSkip, countTake, disableTracking).ToList();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Remove(object id)
        {
            Remove(GetById(id));
        }

        public bool Any(Expression<Func<T, bool>> filter = null)
        {
            return GetQueryable().Any(filter!);
        }

        public int Count(Expression<Func<T, bool>> filter = null)
        {
            return GetQueryable(filter).Count();
        }

        public T FirstOrDefault(
            Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            bool disableTracking = false)
        {
            return GetQueryable(
                filter: filter,
                includes: includes,
                orderBy: orderBy,
                disableTracking: disableTracking).FirstOrDefault();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }


        public Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            int? countSkip = null, 
            int? countTake = null, 
            bool disableTracking = false)
        {
            return GetQueryable(filter, includes, orderBy, countSkip, countTake, disableTracking).ToListAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> filter = null)
        {
            return GetQueryable().AnyAsync(filter!);
        }

        public  Task<T> FirstOrDefaultAsync(
            Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            bool disableTracking = false)
        {
            return GetQueryable(
                filter:filter, 
                includes: includes, 
                orderBy: orderBy, 
                disableTracking:disableTracking).FirstOrDefaultAsync();
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> filter = null)
        {
            return GetQueryable(filter).CountAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
