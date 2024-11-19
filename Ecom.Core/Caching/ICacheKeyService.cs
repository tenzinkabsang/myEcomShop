namespace Ecom.Core.Caching;

public interface ICacheKeyService
{
    /// <summary>
    /// Create a copy of cache key using the default cache time and fill it by passed parameters
    /// </summary>
    /// <param name="cacheKey">Initial cache key</param>
    /// <param name="cacheKeyParameters">Parameters to create cache key</param>
    /// <returns>Cache key</returns>
    CacheKey PrepareKeyForDefaultCache(CacheKey cacheKey, params object[] cacheKeyParameters);
}
