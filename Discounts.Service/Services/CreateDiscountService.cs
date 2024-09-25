
using Discounts.Domain.CreateDiscountCodes;
using Grpc.Core;
using MediatR;

namespace Discounts.Service.Services;

public class CreateDiscountService : CreateDiscounts.CreateDiscountsBase
{
    private readonly ILogger<CreateDiscountService> _logger;
    private readonly IMediator _mediator;
    
    public CreateDiscountService(ILogger<CreateDiscountService> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    public override async Task<CreateDiscountReply> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        try
        {
            await _mediator.Send(new CreateDiscountCodesCommand()
            {
                CodesCount = request.CodesCount,
            },context.CancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating discount");
            Console.WriteLine(e);
            throw;
        }
        return new CreateDiscountReply();
    }
}