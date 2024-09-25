using Discounts.Domain.Database.Utils;
using Discounts.Domain.Entities;
using MongoDB.Driver;

namespace Discounts.Domain.Database;

public class DiscountsDbContext : IDiscountsDbContext
{
    private readonly IMongoDatabase _db;
    public DiscountsDbContext(MongoDBConfig config)
    {
        var client = new MongoClient(config.ConnectionString);
        _db = client.GetDatabase(config.Database);
    }
    public IMongoCollection<Discount> Discounts => _db.GetCollection<Discount>("Discount");
}