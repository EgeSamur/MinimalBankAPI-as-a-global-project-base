using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MinimalBankAPI.DataAccess.Repositories.Abstract.Base;
using MinimalBankAPI.Domain.Common;
using System.Linq.Expressions;

namespace MinimalBankAPI.DataAccess.Repositories.Concrete.Base
{
    public class BaseRepository<T> : IRepository<T> where T : class, IBaseEntity, new()
    {
        private readonly DbContext _dbContext;

        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private DbSet<T> __table { get => _dbContext.Set<T>(); }
        public async Task AddAsync(T entity)
        {
            await __table.AddAsync(entity);
        }

        public async Task AddRangeAsync(IList<T> entities)
        {
            await __table.AddRangeAsync(entities);

        }
        public async Task<T> UpdateAsync(T entity)
        {
            await Task.Run(() => { __table.Update(entity); });
            return entity;
        }

        public async Task<IList<T>> UpdateRangeAsync(IList<T> entity)
        {
            await Task.Run(() =>
            {
                __table.UpdateRange(entity);
            });
            return entity;
        }


        public async Task HardDeleteRangeAsync(IList<T> entity)
        {
            await Task.Run(() => { __table.RemoveRange(entity); });
        }
        public async Task HardDeleteAsync(T entity)
        {
            await Task.Run(() => { __table.Remove(entity); });
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool enableTracking = false)
        {
            IQueryable<T> queryable = __table;
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include is not null) queryable = include(queryable);

            return await queryable.FirstOrDefaultAsync(predicate);
        }
        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            __table.AsNoTracking();
            if (predicate is not null) __table.Where(predicate);
            return await __table.CountAsync();
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate, bool enableTracking)
        {
            if (!enableTracking) __table.AsNoTracking();
            return __table.Where(predicate);
        }

        public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool enableTracking = false)
        {
            IQueryable<T> queryable = __table;
            if (!enableTracking)
                queryable = queryable.AsNoTracking();
            if (include is not null) queryable = include(queryable);
            if (predicate is not null) queryable = queryable.Where(predicate);
            if (orderBy is not null)
                return await orderBy(queryable).ToListAsync();

            return await queryable.ToListAsync();
        }

        //public async Task<IPaginate<T>> GetAllByPagingAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool enableTracking = false, int currentPage = 1, int pageSize = 10)
        //{
        //    IQueryable<T> queryable = __table;
        //    if (!enableTracking)
        //        queryable = queryable.AsNoTracking();
        //    if (include is not null) queryable = include(queryable);
        //    if (predicate is not null) queryable = queryable.Where(predicate);
        //    if (orderBy is not null)
        //    {
        //        var x = orderBy(queryable).Skip((currentPage - 1) * pageSize).Take(pageSize);
        //        return x.ToPaginate<T>(index: currentPage, size: pageSize);

        //    }
        //    var result = queryable.Skip((currentPage - 1) * pageSize).Take(pageSize);
        //    return result.ToPaginate<T>(index: currentPage, size: pageSize);
        //}

        public async Task<IList<T>> GetWithFiltersAsync(Expression<Func<T, bool>>? predicate = null,
                                                 Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
                                                 Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                                                 bool enableTracking = false,
                                                 Dictionary<string, object>? filters = null)
        {
            IQueryable<T> queryable = __table;

            if (!enableTracking)
                queryable = queryable.AsNoTracking();

            if (include is not null)
                queryable = include(queryable);

            if (predicate is not null)
                queryable = queryable.Where(predicate);

            if (filters != null)
            {
                // {{"isbestseller","true"}{"CategoryId","1"}} gibi gelecek.
                foreach (var filter in filters)
                {
                    var parameter = Expression.Parameter(typeof(T), "x");
                    var property = Expression.Property(parameter, filter.Key);
                    var constant = Expression.Constant(filter.Value);
                    var equal = Expression.Equal(property, constant);
                    var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);
                    queryable = queryable.Where(lambda);
                }
            }

            if (orderBy is not null)
                return await orderBy(queryable).ToListAsync();

            return await queryable.ToListAsync();
        }

        public async Task<IList<T>> GetWithFiltersByPagingAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool enableTracking = false, Dictionary<string, object>? filters = null, int currentPage = 1, int pageSize = 10)
        {
            IQueryable<T> queryable = __table;

            if (!enableTracking)
                queryable = queryable.AsNoTracking();

            if (include is not null)
                queryable = include(queryable);

            if (predicate is not null)
                queryable = queryable.Where(predicate);

            if (filters != null)
            {
                // {{"isbestseller","true"}{"CategoryId","1"}} gibi gelecek.
                foreach (var filter in filters)
                {
                    var parameter = Expression.Parameter(typeof(T), "x");
                    var property = Expression.Property(parameter, filter.Key);
                    var constant = Expression.Constant(filter.Value);
                    var equal = Expression.Equal(property, constant);
                    var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);
                    queryable = queryable.Where(lambda);
                }
            }

            if (orderBy is not null)
            {
                return await orderBy(queryable).Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
            }

            return await queryable.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync(); ;
        }

        //public async Task<IPaginate<T>> GetListAsync(Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, int index = 0, int size = 10, bool isAll = true, bool withDeleted = false, bool enableTracking = false, CancellationToken cancellationToken = default)
        //{
        //    IQueryable<T> queryable = __table;
        //    if (index == 0 && size == 0)
        //        size = int.MaxValue;
        //    if (isAll)
        //        size = int.MaxValue;
        //    if (!enableTracking)
        //        queryable = queryable.AsNoTracking();
        //    if (include != null)
        //        queryable = include(queryable);
        //    if (withDeleted)
        //        queryable = queryable.IgnoreQueryFilters();
        //    if (predicate != null)
        //        queryable = queryable.Where(predicate);
        //    if (orderBy != null)
        //        return await orderBy(queryable).ToPaginateAsync(index, size, from: 0, cancellationToken);
        //    return await queryable.ToPaginateAsync(index, size, from: 0, cancellationToken);
        //}
    }
}
