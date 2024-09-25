namespace Discounts.Domain.Database.Utils;

public class ServerConfig
{
    public MongoDBConfig MongoDB { get; set; } = new MongoDBConfig();
}