using Ecom.Data;
using Ecom.Web.Services;
using Ecom.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
using Polly;

namespace Ecom.Web.Configuration;

public static class ServiceCollectionExtensions
{
    public static void ConfigureHttpApiClients(this IServiceCollection services)
    {
        services.AddHttpClient<ICatalogApiClient, CatalogApiClient>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
            client.BaseAddress = new Uri(settings.CatalogApiBaseAddress);
        })
        .AddCustomResilienceHandler();

        services.AddHttpClient<IRecommendationApiClient, CatalogApiClient>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
            client.BaseAddress = new Uri(settings.CatalogApiBaseAddress);
        })
        // Similar to the custom one we implemented!
        .AddStandardResilienceHandler();

        services.AddHttpClient<ICartApiClient, CartApiClient>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<ApiSettings>>().Value;
            client.BaseAddress = new Uri(settings.ShoppingCartApiBaseAddress);
        })
        .AddStandardResilienceHandler();
    }

    /// <summary>
    /// Http resilience policy. Adds Retry and Circut breaker features.
    /// </summary>
    private static void AddCustomResilienceHandler(this IHttpClientBuilder client)
    {
        client.AddResilienceHandler("eshop-apis", pipeline =>
        {
            // Overall timeout including retries
            pipeline.AddTimeout(TimeSpan.FromSeconds(10));

            pipeline.AddRetry(new HttpRetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                Delay = TimeSpan.FromMilliseconds(500)
            });

            pipeline.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
            {
                SamplingDuration = TimeSpan.FromSeconds(10),
                FailureRatio = 0.85,
                MinimumThroughput = 5,
                BreakDuration = TimeSpan.FromSeconds(30)
            });

            // Timeout per request
            pipeline.AddTimeout(TimeSpan.FromSeconds(3));
        });
    }

    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            var connectionString = configuration[configuration["AZURE_SQL_CONNECTION_STRING"] ?? "ConnectionStrings:MyEshop"];
            opt.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
        });
    }

    public static void ConfigureCache(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration[configuration["AZURE_REDIS_CONNECTION_STRING"] ?? "ConnectionStrings:Redis"];
        if (!string.IsNullOrWhiteSpace(connectionString))
            services.AddStackExchangeRedisCache(options => options.Configuration = connectionString);
        else
            services.AddDistributedMemoryCache();
    }
}