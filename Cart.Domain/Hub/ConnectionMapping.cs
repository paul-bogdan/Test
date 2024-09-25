using Cart.Domain.Hub;

namespace Cart.Microservice.Hub;

public class ConnectionMapping : IConnectionMapping
{
    private  List<Connections> _connections = new();
    
    public int Count
    {
        get
        {
            lock (_connections)
            {
                return _connections.Count;
            }
        }
    }

    public void Add(string key, string connectionId)
    {
        lock (_connections)
        {
            if(_connections.Any(x=> x.Key == key))
            {
                _connections.Remove(_connections.FirstOrDefault(x => x.Key == key));
            }
            if(!_connections.Any(x=> x.Key == key && x.ConnectionId == connectionId))
            {
                _connections.Add(new Connections
                {
                    Key = key,
                    ConnectionId = connectionId
                });
                
            }
        }
    }

    public IEnumerable<string> GetConnections(string key)
    {
        lock (_connections)
        {
            return _connections.Where(x => x.Key == key).Select(x => x.ConnectionId);
        }
    }
    
    public async Task <string?> GetKeyByConnectionId(string connectionId)
    {
        lock (_connections)
        {
            var connections= _connections.FirstOrDefault(x => x.ConnectionId == connectionId);
            if (connections is not null)
            {
                return connections.Key;
            }

            return null;
        }
    }

    public void Remove(string connectionId)
    {
        lock (_connections)
        {
            var connections = new List<Connections>();
            lock (connections)
            {
                connections.AddRange(_connections.Where(item => item.ConnectionId != connectionId));
                _connections = connections;
            }
        }
    }

}