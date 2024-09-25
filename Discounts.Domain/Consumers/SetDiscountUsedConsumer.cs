using Discounts.Contracts;
using Discounts.Domain.Database;
using Discounts.Domain.SetDiscountsUsed;
using MassTransit;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Discounts.Domain.Consumers;

public class SetDiscountUsedConsumer : IConsumer<SetDiscountUsedRequest>
{
    
    private readonly IMediator _mediator;
    
 
    public SetDiscountUsedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Consume(ConsumeContext<SetDiscountUsedRequest> context)
    {
        var bsonIds = context.Message.DiscountIds.Select(s => new ObjectId(s)).ToList();
        try
        {
            await _mediator.Send(new SetDiscountsUsedCommand()
            {
                DiscountIds = bsonIds
            },context.CancellationToken);
            await context.RespondAsync(new SetDiscountUsedResponse
            {
                IsSuccess = true
            });
        }
        catch (Exception e)
        {
            await context.RespondAsync(new SetDiscountUsedResponse
            {
                IsSuccess = false,
                Error = e.Message
            });
        }
      
   
    }
}