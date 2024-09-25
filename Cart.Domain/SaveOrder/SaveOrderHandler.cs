using Cart.Domain.SetDiscountsUsed;
using Discounts.Contracts;
using MediatR;
using Sol.Caching;

namespace Cart.Domain.SaveOrder;

public class SaveOrderHandler : IRequestHandler<SaveOrderCommand,SaveOrderResponse>
{
    private readonly ICachingService _cachingService;
    private readonly SetDiscountsUsedMqCommand _setDiscountsUsedMqCommand;
    
    public SaveOrderHandler(ICachingService cachingService,SetDiscountsUsedMqCommand setDiscountsUsedMqCommand)
    {
        _cachingService = cachingService;
        _setDiscountsUsedMqCommand = setDiscountsUsedMqCommand;
    }
    
    public async Task<SaveOrderResponse> Handle(SaveOrderCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cachingService.GetData<Entities.Cart>(_cachingService.GenerateKey(Constants.Keys.Cart,request.UserId),cancellationToken);
        if(cart == null)
        {
            return new SaveOrderResponse()
            {
                IsSuccess = false,
                ErrorMessage = "Cart not found"
            };
        }
        
        // save the order
        // here send event to mark discounts used
        var resp = await _setDiscountsUsedMqCommand.ExecuteAsync(new SetDiscountUsedRequest()
        {
            DiscountIds = cart.Discounts.Select(s => s.DiscountId).ToList()
        });
        
        if(resp.IsSuccess == false)
        {
            return new SaveOrderResponse()
            {
                IsSuccess = false,
                ErrorMessage = "Error while marking discounts used"
            };
        }
        
        await _cachingService.RemoveData(_cachingService.GenerateKey(Constants.Keys.Cart,request.UserId),cancellationToken);
        return new SaveOrderResponse()
        {
            IsSuccess = true
        };
        
    }
    
        
    
}