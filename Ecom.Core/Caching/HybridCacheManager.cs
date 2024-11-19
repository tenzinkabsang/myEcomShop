using System.Globalization;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;

namespace Ecom.Core.Caching;

public class HybridCacheManager(HybridCache cache, IConfiguration configuration) : IStaticCacheManager
{
    public async Task<T> GetAsync<T>(CacheKey key, Func<Task<T>> acquire)
    {
        var result = await cache.GetOrCreateAsync(
            key.Key,
            async _ => await acquire(),
            new HybridCacheEntryOptions { Expiration = key.CacheTime, LocalCacheExpiration = key.CacheTime },
            key.Tags);

        return result;
    }

    public Task<T> GetAsync<T>(CacheKey key, Func<T> acquire)
    {
        return GetAsync(key, () => Task.FromResult(acquire()));
    }

    public async Task RemoveAsync(CacheKey key)
    {
        await cache.RemoveAsync(key.Key);
    }

    public async Task RemoveAsync(IEnumerable<CacheKey> cacheKeys)
    {
        await cache.RemoveAsync(cacheKeys.Select(k => k.Key));
    }

    public async Task RemoveByTagsAsync(IEnumerable<string> tags)
    {
        await cache.RemoveByTagAsync(tags);
    }

    public CacheKey PrepareKeyForDefaultCache(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        var key = cacheKey.Create(CreateCacheKeyParameters, cacheKeyParameters);
        if (int.TryParse(configuration["CacheSettings:DefaultCacheTime"], out var time))
            key.CacheTime = TimeSpan.FromMinutes(time);

        return key;
    }

    protected virtual string CreateIdsHash(IEnumerable<int> ids)
    {
        if (!ids.Any())
            return string.Empty;

        var identifiersString = string.Join(", ", ids.OrderBy(id => id));
        return HashHelper.CreateHash(identifiersString);
    }

    /// <summary>
    /// Converts an object to cache parameter
    /// </summary>
    /// <param name="parameter">Object to convert</param>
    /// <returns>Cache parameter</returns>
    protected virtual object CreateCacheKeyParameters(object parameter)
    {
        return parameter switch
        {
            null => "null",
            IEnumerable<int> ids => CreateIdsHash(ids),
            IEnumerable<BaseEntity> entities => CreateIdsHash(entities.Select(entity => entity.Id)),
            BaseEntity entity => entity.Id,
            decimal param => param.ToString(CultureInfo.InvariantCulture),
            _ => parameter
        };
    }
}
