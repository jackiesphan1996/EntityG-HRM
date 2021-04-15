using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EntityG.EntityFramework.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetQueryable();

        T GetById(object id);

        List<T> GetAll(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? countSkip = null,
            int? countTake = null,
            bool disableTracking = false);

        void Add(T entity);

        void Update(T entity);

        void Remove(T entity);

        void Remove(object id);

        bool Any(Expression<Func<T, bool>> filter = null);

        int Count(Expression<Func<T, bool>> filter = null);

        T FirstOrDefault(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            bool disableTracking = false);

        Task<T> GetByIdAsync(object id);

        Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            int? countSkip = null, 
            int? countTake = null,
            bool disableTracking = false);
        
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter = null);
        
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            bool disableTracking = false);
        
        Task<int> CountAsync(Expression<Func<T, bool>> filter = null);
    }
}
