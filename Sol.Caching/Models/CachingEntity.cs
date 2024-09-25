namespace Sol.Caching.Models;

public class CachingEntity<T> where T : class
{
    public string Key { get; set; } = null!;
    public T Data { get; set; } = null!;
}