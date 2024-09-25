namespace Sol.Caching;

public interface ICachingService
{
    /// <summary>
    ///     Get Data using key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<T?> GetData<T>(string key,CancellationToken cancellationToken = default);


    /// <summary>
    /// get data for multiple keys
    /// </summary>
    /// <param name="keys"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<List<T?>> GetData<T>(List<string> keys,CancellationToken cancellationToken = default);

    
    /// <summary>
    ///     Set Data with Value and Expiration Time of Key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expiryDate"></param>
    /// <returns></returns>

    Task<bool>  SetData<T>(string key, T value, CachingTimes expiryDate,CancellationToken cancellationToken = default);

    /// <summary>
    ///     Remove Data
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<object> RemoveData(string key,CancellationToken cancellationToken = default);

    /// <summary>
    ///  flush all the data in the cache
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>

    Task RemoveData(CancellationToken cancellationToken);
    
    /// <summary>
    /// generate key for caching
    /// </summary>
    /// <param name="key"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    string GenerateKey(string key, params object[] args);
    
    
    /// <summary>
    /// get or upsert data in the cache, and return the datas
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="expiryDate"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T?> GetOrSetData<T>(string key, Func<Task<T?>> value, CachingTimes expiryDate,CancellationToken cancellationToken = default);
   
}

public enum  CachingTimes
{
    Hourly = 1,
    Daily = 24,
    TenMinutes = 10,
    ThirtyMinutes = 30,
    SevenDays = 24*7
    
}