using Discounts.Domain.AddDiscountToCart;
using Discounts.Domain.GetAllDiscountCodes;
using Discounts.Domain.GetDiscountByCode;
using Discounts.Domain.ValidateDiscountCode;
using Grpc.Core;
using MediatR;

namespace Discounts.Service.Services;

public class AddToCartDiscountService : AddDiscountToCartService.AddDiscountToCartServiceBase
{
    private readonly IMediator _mediator;
    
    private readonly AddDiscountToCartMqCommand _addDiscountToCartMqCommand;

    
    public AddToCartDiscountService(IMediator mediator, AddDiscountToCartMqCommand addDiscountToCartMqCommand)
    {
        _mediator = mediator;
        _addDiscountToCartMqCommand = addDiscountToCartMqCommand;
    }

    public override async Task<AddDiscountToCartResponse> AddDiscountToCart(AddDiscountToCartRequest request, ServerCallContext context)
    {
       
        var discount = await _mediator.Send(new GetDiscountByCodeQuery()
        {
            Code   =request.DiscountCode
        }, context.CancellationToken);

        if (discount is null)
        {
            return new AddDiscountToCartResponse()
            {
                Success = false,
                Message = "Code is not found"
            };
        }
        
        if (!discount.Used)
        {
              var response=  await _addDiscountToCartMqCommand.ExecuteAsync(new Cart.Contracts.AddDiscountToCartRequest
              {
                  UserId = Guid.Parse( request.UserId),
                  DiscountId = discount.Id,
                  Percentage = discount.Percentage,
                    DiscountCode = discount.Code
              });
                return new AddDiscountToCartResponse()
                {
                    Success = response.IsSuccess,
                    Message = response.Error?? string.Empty
                };
          
        }

        return new AddDiscountToCartResponse()
        {
            Success = false,
            Message = "Code is not valid"
        };
    }
}