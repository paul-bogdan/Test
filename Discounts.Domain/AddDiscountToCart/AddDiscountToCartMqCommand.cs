using Cart.Contracts;
using MassTransit;

namespace Discounts.Domain.AddDiscountToCart;

public class AddDiscountToCartMqCommand
{
    private readonly IRequestClient<AddDiscountToCartRequest> _requestClient;
    
    public AddDiscountToCartMqCommand(IRequestClient<AddDiscountToCartRequest> requestClient)
    {
        _requestClient = requestClient;
    }
    
    public async Task<AddRequestToCartResponse> ExecuteAsync(AddDiscountToCartRequest request)
    {
        var response = await _requestClient.GetResponse<AddRequestToCartResponse>(request);
        return response.Message;
    }
    
}