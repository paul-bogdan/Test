using Discounts.Domain.Entities;
using MongoDB.Driver;

namespace Discounts.Domain.Database;

public interface IDiscountsDbContext
{
    IMongoCollection<Discount> Discounts { get; }
}