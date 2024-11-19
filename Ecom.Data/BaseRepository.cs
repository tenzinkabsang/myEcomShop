using Ecom.Core;
using Ecom.Core.Caching;
using Ecom.Core.Events;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Data;

public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _dbSet;
    private readonly ApplicationDbContext _context;
    private readonly IStaticCacheManager _staticCacheManager;
    private readonly IEventPublisher _eventPublisher;

    public BaseRepository(ApplicationDbContext context,
        IStaticCacheManager cache,
        IEventPublisher eventPublisher)
    {
        _context = context;
        _staticCacheManager = cache;
        _dbSet = context.Set<TEntity>();
        _eventPublisher = eventPublisher;
    }

    private async Task<IList<TEntity>> GetEntitiesAsync(Func<Task<IList<TEntity>>> getAllAsync, Func<ICacheKeyService, CacheKey>? getCacheKey = null)
    {
        if (getCacheKey is null)
            return await getAllAsync();

        var cacheKey = getCacheKey(_staticCacheManager) ?? _staticCacheManager.PrepareKeyForDefaultCache(EntityCacheDefaults<TEntity>.AllCacheKey);

        return await _staticCacheManager.GetAsync(cacheKey, getAllAsync);
    }

    public async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
        Func<ICacheKeyService, CacheKey>? getCacheKey = null,
        bool includeDeleted = false)
    {
        async Task<IList<TEntity>> getAllAsync()
        {
            var query = AddDeletedFilter(_dbSet, includeDeleted);
            query = func?.Invoke(query) ?? query;
            return await query.ToListAsync();
        }

        return await GetEntitiesAsync(getAllAsync, getCacheKey);
    }
    
    public async Task<IList<TEntity>> GetByIdsAsync(IList<int> ids, Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
       Func<ICacheKeyService, CacheKey>? getCacheKey = null,
       bool includeDeleted = false)
    {
        async Task<IList<TEntity>> getByIdsAsync()
        {
            var query = func != null ? func(_dbSet) : _dbSet;
            var entries = await AddDeletedFilter(query, includeDeleted).Where(entry => ids.Contains(entry.Id)).ToListAsync();
            return entries;
        }

        return await GetEntitiesAsync(getByIdsAsync, key =>
        {
            return getCacheKey != null 
                ? getCacheKey(_staticCacheManager) 
                : _staticCacheManager.PrepareKeyForDefaultCache(EntityCacheDefaults<TEntity>.ByIdsCacheKey, ids);
        });
    }

    public virtual async Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
           int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, bool includeDeleted = false)
    {
        var query = AddDeletedFilter(_dbSet, includeDeleted);

        query = func != null ? func(query) : query;

        return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
    }

    public async Task<TEntity?> GetByIdAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null, 
        Func<ICacheKeyService, CacheKey>? getCacheKey = null,
        bool includeDeleted = false)
    {
        async Task<TEntity?> getByIdAsync()
        {
            var query = func != null ? func(_dbSet) : _dbSet;
            return await AddDeletedFilter(query, includeDeleted).FirstOrDefaultAsync(entity => entity.Id == id);
        }

        if (getCacheKey is null)
            return await getByIdAsync();

        var cacheKey = getCacheKey(_staticCacheManager) ?? _staticCacheManager.PrepareKeyForDefaultCache(EntityCacheDefaults<TEntity>.ByIdCacheKey, id);

        return await _staticCacheManager.GetAsync(cacheKey, getByIdAsync);
    }


    public async Task InsertAsync(TEntity entity, bool publishEvent = true)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();

        if (publishEvent)
            await _eventPublisher.EntityInsertedAsync(entity);
    }

    public async Task UpdateAsync(TEntity entity, bool publishEvent = true)
    {
        ArgumentNullException.ThrowIfNull(entity);
        await _context.SaveChangesAsync();
        if (publishEvent)
            await _eventPublisher.EntityUpdatedAsync(entity);
    }

    public async Task DeleteAsync(TEntity entity, bool publishEvent = true)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (entity is ISoftDeletedEntity softDeleteEntity)
            softDeleteEntity.Deleted = true;
        else
            _dbSet.Remove(entity);

        await _context.SaveChangesAsync();

        if (publishEvent)
            await _eventPublisher.EntityDeletedAsync(entity);
    }

    public async Task DeleteByIdAsync(int id, bool publishEvent = true)
    {
        var entity = await GetByIdAsync(id);

        ArgumentNullException.ThrowIfNull(entity);

        await DeleteAsync(entity, publishEvent);
    }

    private IQueryable<TEntity> AddDeletedFilter(IQueryable<TEntity> query, bool includeDeleted)
    {
        if (includeDeleted)
            return query;

        if (typeof(TEntity).GetInterface(nameof(ISoftDeletedEntity)) == null)
            return query;

        return query.OfType<ISoftDeletedEntity>().Where(entry => !entry.Deleted).OfType<TEntity>();
    }
}
