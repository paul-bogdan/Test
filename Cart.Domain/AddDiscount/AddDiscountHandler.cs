using Cart.Contracts;
using Cart.Domain.Consumers;
using Cart.Domain.Entities;
using Cart.Domain.Mappings;
using MassTransit;
using MediatR;
using Sol.Caching;

namespace Cart.Domain.AddDiscount;

public class AddDiscountHandler : IRequestHandler<AddDiscountCommand,AddDiscountResponse>
{
    private readonly ICachingService _cachingService;
    
    private readonly IPublishEndpoint _publishEndpoint;
    
    public AddDiscountHandler(ICachingService cachingService, IPublishEndpoint publishEndpoint)
    {
        _cachingService = cachingService;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task<AddDiscountResponse> Handle(AddDiscountCommand request, CancellationToken cancellationToken)
    {
        
        var cart = await _cachingService.GetData<Entities.Cart>(_cachingService.GenerateKey(Constants.Keys.Cart,request.UserId),cancellationToken);
        
        if (cart == null)
        {
            ArgumentNullException.ThrowIfNull(cart);
        }
        cart.AddDiscount(new CartDiscount()
        {
            DiscountId = request.DiscountId,
            Percentage = request.Percentage,
            DiscountCode = request.DiscountCode
        });
        
        await _cachingService.SetData(_cachingService.GenerateKey(Constants.Keys.Cart,request.UserId),cart,CachingTimes.Daily,cancellationToken);
        
        // notify all instances of signalr that a discount has been added
        // Publish the CartUpdated message
        await _publishEndpoint.Publish(new CartUpdatedRequest
        {
            UserId = request.UserId.ToString()
        
        }, cancellationToken);
        
        return new AddDiscountResponse();
    }
}