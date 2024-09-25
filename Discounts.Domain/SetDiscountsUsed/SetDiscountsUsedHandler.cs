using Discounts.Contracts;
using Discounts.Domain.Database;
using Discounts.Domain.Entities;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Discounts.Domain.SetDiscountsUsed;

public class SetDiscountsUsedHandler : IRequestHandler<SetDiscountsUsedCommand, SetDiscountsUsedResponse>
{
    private readonly IDiscountsDbContext _dbContext;
    
    public SetDiscountsUsedHandler(IDiscountsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    
    public async Task<SetDiscountsUsedResponse> Handle(SetDiscountsUsedCommand request, CancellationToken cancellationToken)
    {
        var filter = Builders<Discount>.Filter.In(s => s.Id, request.DiscountIds);
        var discounts = await _dbContext.Discounts
            .Find(filter)
            .ToListAsync(cancellationToken);
        foreach (var discount in discounts)
        {
            discount.Used = true;
            discount.UsedAt = DateTime.UtcNow;
            await _dbContext.Discounts.ReplaceOneAsync(s => s.Id == discount.Id, discount, cancellationToken: cancellationToken);
        }
        return new SetDiscountsUsedResponse();
    }
}