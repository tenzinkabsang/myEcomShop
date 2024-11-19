namespace Ecom.Core.Caching;

public record EntityCacheDefaults<TEntity> where TEntity: BaseEntity
{
    public static string EntityTypeName => typeof(TEntity).Name.ToLowerInvariant();

    public static CacheKey ByIdCacheKey => new($"Ecom.{EntityTypeName}.byid.{{0}}", ByIdTag);

    public static CacheKey ByIdsCacheKey => new($"Ecom.{EntityTypeName}.byids.{{0}}", ByIdsTag);

    public static CacheKey AllCacheKey => new($"Ecom.{EntityTypeName}.all.", ByEntityTag);

    public static string ByIdTag => $"Ecom.{EntityTypeName}.byid.";
    
    public static string ByIdsTag => $"Ecom.{EntityTypeName}.byids.";

    public static string ByEntityTag => $"Ecom.{EntityTypeName}.";
}
