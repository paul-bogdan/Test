using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sol.Caching.Redis.Services;
using StackExchange.Redis;

namespace Sol.Caching.Redis;

public static class RedisCachingModule
{
    public static void AddRedisCaching(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Redis");
        ArgumentNullException.ThrowIfNull(services);
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(connectionString));
        services.AddSingleton<ICachingService, RedisCachingService>();
    }
}