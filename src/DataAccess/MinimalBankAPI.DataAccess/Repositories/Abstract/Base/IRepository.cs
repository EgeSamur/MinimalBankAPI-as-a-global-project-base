using Microsoft.EntityFrameworkCore.Query;
using MinimalBankAPI.Domain.Common;
using System.Linq.Expressions;

namespace MinimalBankAPI.DataAccess.Repositories.Abstract.Base
{
    public interface IRepository<T> where T : class, IBaseEntity, new()
    {

        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool enableTracking = false);

        Task<T> GetAsync(Expression<Func<T, bool>> predicate,
          Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
          bool enableTracking = false);

        IQueryable<T> Find(Expression<Func<T, bool>> predicate, bool enableTracking = false);

        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);

        public Task<IList<T>> GetWithFiltersAsync(Expression<Func<T, bool>>? predicate = null,
                                                 Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
                                                 Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                                 bool enableTracking = false,
                                                 Dictionary<string, object>? filters = null);

        Task<IList<T>> GetAllByPagingAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool enableTracking = false, int currentPage = 1, int pageSize = 10);
        Task AddAsync(T entity);
        Task AddRangeAsync(IList<T> entities);
        Task<T> UpdateAsync(T entity);
        Task<IList<T>> UpdateRangeAsync(IList<T> entity);
        Task HardDeleteRangeAsync(IList<T> entity);
        Task HardDeleteAsync(T entity);
    }
}
