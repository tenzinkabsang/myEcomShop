using Ecom.Core.Caching;

namespace Ecom.Catalog.Api;

public static class EcomCatalogCacheDefaults
{
    public static CacheKey CategoriesKey => new CacheKey("Ecom.Catalog.Categories");
}
