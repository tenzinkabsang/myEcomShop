using Ecom.Core.Caching;

namespace Ecom.Core.Events;

/// <summary>
/// Consumer interface
/// </summary>
/// <typeparam name="T">Type</typeparam>
public interface IConsumer<T>
{
    Task HandleEventAsync(T eventMessage);
}


public class CacheEventConsumer<TEntity> : 
    IConsumer<EntityDeletedEvent<TEntity>>, 
    IConsumer<EntityUpdatedEvent<TEntity>>
    where TEntity : BaseEntity
{
    private readonly IStaticCacheManager _staticCacheManager;

    public CacheEventConsumer(IStaticCacheManager staticCacheManager) 
        => _staticCacheManager = staticCacheManager;

    public async Task HandleEventAsync(EntityDeletedEvent<TEntity> eventMessage)
    {
        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(EntityCacheDefaults<TEntity>.ByIdCacheKey, eventMessage.Entity.Id);
        await _staticCacheManager.RemoveAsync(cacheKey);
    }

    public async Task HandleEventAsync(EntityUpdatedEvent<TEntity> eventMessage)
    {
        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(EntityCacheDefaults<TEntity>.ByIdCacheKey, eventMessage.Entity.Id);
        await _staticCacheManager.RemoveAsync(cacheKey);
    }
}