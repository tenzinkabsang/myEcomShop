namespace Ecom.Core.Caching;

public sealed class CacheKey
{
    public string Key { get; private set; }
    public List<string> Tags { get; init; } = [];
    public TimeSpan CacheTime { get; set; } = TimeSpan.FromMinutes(5);

    public CacheKey(string key, params string[] tags)
    {
        Key = key;
        Tags.AddRange(tags.Where(t => !string.IsNullOrEmpty(t)));
    }

    /// <summary>
    /// Create a new instance from the current one with updated Keys and Tags
    /// </summary>
    /// <param name="createCacheKeyParameters">Function to create parameters</param>
    /// <param name="keyObjects">Objects to create parameters</param>
    /// <returns>Cache key</returns>
    public CacheKey Create(Func<object, object> createCacheKeyParameters, params object[] keyObjects)
    {
        var cacheKey = new CacheKey(Key, [.. Tags]);

        if (keyObjects.Length == 0)
            return cacheKey;

        cacheKey.Key = string.Format(cacheKey.Key, keyObjects.Select(createCacheKeyParameters).ToArray());

        for (var i = 0; i < cacheKey.Tags.Count; i++)
            cacheKey.Tags[i] = string.Format(cacheKey.Tags[i], keyObjects.Select(createCacheKeyParameters).ToArray());

        return cacheKey;
    }
}
