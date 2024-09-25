namespace Cart.Domain.Hub;

public interface IConnectionMapping
{
    int Count { get; }
    void Add(string key, string connectionId);
    IEnumerable<string> GetConnections(string key);
    void Remove(string connectionId);
    Task<string?> GetKeyByConnectionId(string connectionId);

}