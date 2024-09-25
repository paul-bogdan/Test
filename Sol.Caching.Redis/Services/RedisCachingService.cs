using Newtonsoft.Json;
using Sol.Caching.Redis.Utils;
using StackExchange.Redis;

namespace Sol.Caching.Redis.Services;

internal class RedisCachingService : ICachingService
{
    //in order to extend this and query multiple columns we need to extend the class to index the columns
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisCachingService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<T?> GetOrSetData<T>(string key, Func<Task<T?>> value,CachingTimes expryDate,CancellationToken cancellationToken = default)
    {
        key = GenerateKey(key);
        var valueFromCache = await GetData<T>(key,cancellationToken);
        if (valueFromCache != null)
            return valueFromCache;
        var newValue = await value();
        await SetData(key, newValue, expryDate);
        return newValue;
    }
    
    public async Task<T?> GetData<T>(string key,CancellationToken cancellationToken= default)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var value = await db.StringGetAsync(key);

        if (!string.IsNullOrWhiteSpace(value) && value.HasValue)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T?>(value.ToString(), new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterContractResolver(),
                Formatting = Formatting.Indented
            });
  
        }

        return default;
    }

    public async Task<List<T?>> GetData<T>(List<string> keys,CancellationToken cancellationToken = default)
    {
        var db = _connectionMultiplexer.GetDatabase();

        var values = await db.StringGetAsync(keys.Select(x => (RedisKey)x).ToArray());
        var result = new List<T?>();
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value) && value.HasValue)
                result.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<T?>(value.ToString(),  new JsonSerializerSettings
                {
                    ContractResolver = new PrivateSetterContractResolver()
                }));
            else
                result.Add(default);
        }
        return result;
    }
    

    public async Task<bool> SetData<T>(string key, T value,  CachingTimes expiryDate,CancellationToken cancellationToken = default)
    {
        var db = _connectionMultiplexer.GetDatabase();
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(value, new JsonSerializerSettings
        {
            ContractResolver = new PrivateSetterContractResolver(),
            Formatting = Formatting.Indented
        });
        return await db.StringSetAsync(key,json ,new TimeSpan((int)expiryDate,0,0) );
    }
    
    
    public async Task<object> RemoveData(string key,CancellationToken cancellationToken = default)
    {

        var db = _connectionMultiplexer.GetDatabase();
        return await db.KeyDeleteAsync(key);
    }


    public async Task RemoveData(CancellationToken cancellationToken)
    {
        var db = _connectionMultiplexer.GetDatabase();
        try
        {
            // Ensure cancellation is checked before executing the command
            cancellationToken.ThrowIfCancellationRequested();

            // Execute the command to clear all data from the current database
            await db.ExecuteAsync("FLUSHALL");

            // Check again if cancellation was requested
            cancellationToken.ThrowIfCancellationRequested();
        }
        catch (OperationCanceledException)
        {
            // Handle the case where the operation was canceled
            Console.WriteLine("The operation was canceled.");
            throw; // Rethrow the exception if you want to handle it further up the call chain
        }
        catch (Exception ex)
        {
            // Handle other exceptions that may occur
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }
    }

    
  
    
    
    public string GenerateKey(string key, params object[] args)
    {
        return key.ToLower() + string.Join("_", args);
    }
}


