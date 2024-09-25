
using Discounts.Domain.CreateDiscountCodes;
using Discounts.Domain.ValidateDiscountCode;
using Grpc.Core;
using MediatR;

namespace Discounts.Service.Services;

public class ValidateDiscountService : ValidateDiscountCode.ValidateDiscountCodeBase
{

    private readonly IMediator _mediator;
    
    public ValidateDiscountService(IMediator mediator)
    {
     
        _mediator = mediator;
    }
    
    public override async Task<ValidateDiscountCodeReply> ValidateCode(ValidateDiscountCodeRequest request, ServerCallContext context)
    {
        var response=  await _mediator.Send(new ValidateDiscountCodeCommand()
        {
            Code = request.Code,
        },context.CancellationToken);
    
        return new ()
        {
            IsValid = response.IsValid
        };
    }
}