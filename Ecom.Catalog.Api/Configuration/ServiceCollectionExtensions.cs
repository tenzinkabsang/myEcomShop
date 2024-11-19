using Microsoft.Extensions.Caching.Hybrid;

namespace Ecom.Catalog.Api.Configuration;

public static class ServiceCollectionExtensions
{
    public static void ConfigureCache(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration[configuration["AZURE_REDIS_CONNECTION_STRING"] ?? "ConnectionStrings:Redis"];

        if (!string.IsNullOrWhiteSpace(connectionString))
            services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);
        //else
        //    services.AddDistributedMemoryCache();


        int defaultCacheTime = int.TryParse(configuration["CacheSettings:DefaultCacheTime"], out var time) ? time : 30;

#pragma warning disable EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        services.AddHybridCache(options =>
        {
            options.MaximumPayloadBytes = 1024 * 1024;
            options.MaximumKeyLength = 1024;
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(defaultCacheTime),
                LocalCacheExpiration = TimeSpan.FromMinutes(defaultCacheTime)
            };
        });
#pragma warning restore EXTEXP0018 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    }
}
