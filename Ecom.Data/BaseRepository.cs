using Ecom.Core;
using Ecom.Core.Events;
using Microsoft.EntityFrameworkCore;

namespace Ecom.Data;

public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity: BaseEntity
{
    private readonly DbSet<TEntity> _dbSet;
    private readonly ApplicationDbContext _context;
    private readonly IEventPublisher _eventPublisher;

    public BaseRepository(ApplicationDbContext context, IEventPublisher eventPublisher)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
        _eventPublisher = eventPublisher;
    }

    public async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null, bool includeDeleted = true)
    {
        var query = AddDeletedFilter(_dbSet, includeDeleted);

        //query = func != null ? func(query) : query;

        query = func?.Invoke(query) ?? query;

        return await query.ToListAsync();
    }

    public virtual async Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
           int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, bool includeDeleted = true)
    {
        var query = AddDeletedFilter(_dbSet, includeDeleted);

        query = func != null ? func(query) : query;

        return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
    }

    public async Task<TEntity?> GetByIdAsync(int id, bool includeDeleted = true)
    {
        var query = AddDeletedFilter(_dbSet, includeDeleted);
        return await query.FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public async Task<IList<TEntity>> GetByIdsAsync(IList<int> ids, bool includeDeleted = true)
    {
        var query = AddDeletedFilter(_dbSet, includeDeleted);

        var entries = await query.Where(entry => ids.Contains(entry.Id)).ToListAsync();

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
