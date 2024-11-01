using System.Text.Json;
using Ecom.Core;
using Ecom.Core.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Ecom.Data;

public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly DbSet<TEntity> _dbSet;
    private readonly ApplicationDbContext _context;
    private readonly IDistributedCache _cache;
    private readonly IEventPublisher _eventPublisher;

    public BaseRepository(ApplicationDbContext context,
        IDistributedCache cache,
        IEventPublisher eventPublisher)
    {
        _context = context;
        _cache = cache;
        _dbSet = context.Set<TEntity>();
        _eventPublisher = eventPublisher;
    }

    private async Task<IList<TEntity>> GetEntitiesAsync(Func<Task<IList<TEntity>>> getAllAsync, string? cacheKey = null)
    {
        if (string.IsNullOrEmpty(cacheKey))
            return await getAllAsync();

        var cachedData = await _cache.GetStringAsync(cacheKey);

        if (cachedData != null)
            return JsonSerializer.Deserialize<IList<TEntity>>(cachedData)!;

        var items = await getAllAsync();

        await _cache.SetStringAsync(
            cacheKey,
            JsonSerializer.Serialize(items),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
            });

        return items;
    }


    public async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null, bool includeDeleted = false)
    {
        var query = AddDeletedFilter(_dbSet, includeDeleted);

        query = func?.Invoke(query) ?? query;

        return await query.ToListAsync();
    }

    public virtual async Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
           int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, bool includeDeleted = false)
    {
        var query = AddDeletedFilter(_dbSet, includeDeleted);

        query = func != null ? func(query) : query;

        return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
    }

    public async Task<TEntity?> GetByIdAsync(int id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null, bool includeDeleted = false)
    {
        var query = func != null ? func(_dbSet) : _dbSet;

        return await AddDeletedFilter(query, includeDeleted).FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public async Task<IList<TEntity>> GetByIdsAsync(IList<int> ids, Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null, bool includeDeleted = false)
    {
        var query = func != null ? func(_dbSet) : _dbSet;

        var entries = await AddDeletedFilter(query, includeDeleted).Where(entry => ids.Contains(entry.Id)).ToListAsync();

        return entries;
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
