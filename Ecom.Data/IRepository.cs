using Ecom.Core;

namespace Ecom.Data;

public interface IRepository<T> where T : BaseEntity
{
    Task<IList<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? func = null, bool includeDeleted = false);

    Task<IPagedList<T>> GetAllPagedAsync(Func<IQueryable<T>, IQueryable<T>>? func = null,
           int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, bool includeDeleted = false);

    Task<T?> GetByIdAsync(int id, Func<IQueryable<T>, IQueryable<T>>? func = null, bool includeDeleted = false);

    Task<IList<T>> GetByIdsAsync(IList<int> ids, Func<IQueryable<T>, IQueryable<T>>? func = null, bool includeDeleted = false);

    Task InsertAsync(T entity, bool publishEvent = true);

    Task UpdateAsync(T entity, bool publishEvent = true);

    Task DeleteAsync(T entity, bool publishEvent = true);
    
    Task DeleteByIdAsync(int id, bool publishEvent = true);
}
