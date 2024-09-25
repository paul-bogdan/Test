using Cart.Contracts;
using Cart.Domain.AddDiscount;
using MassTransit;
using MediatR;


namespace Cart.Domain.Consumers;

public class AddDiscountConsumer : IConsumer<AddDiscountToCartRequest>
{
    private readonly IMediator _mediator;


    public AddDiscountConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async Task Consume(ConsumeContext<AddDiscountToCartRequest> context)
    {
        var message = context.Message;
        try
        {
            await _mediator.Send(new AddDiscountCommand
            {
                UserId = message.UserId,
                DiscountId = message.DiscountId,
                Percentage = message.Percentage,
                DiscountCode = message.DiscountCode
            }, context.CancellationToken);
            await  context.RespondAsync(new AddRequestToCartResponse
            {
                IsSuccess = true
            });
        }
        catch (Exception e)
        {
            await context.RespondAsync(new AddRequestToCartResponse
            {
                IsSuccess = false,
                Data = null,
                Error = e.Message
            });
        }
    }
}