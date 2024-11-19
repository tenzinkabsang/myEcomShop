namespace Ecom.Core.Caching;

public interface IStaticCacheManager : ICacheKeyService
{
    Task<T> GetAsync<T>(CacheKey key, Func<Task<T>> acquire);
    Task<T> GetAsync<T>(CacheKey key, Func<T> acquire);
    Task RemoveAsync(CacheKey key);
    Task RemoveAsync(IEnumerable<CacheKey> keys);
    Task RemoveByTagsAsync(IEnumerable<string> tags);
}
