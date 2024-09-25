using Discounts.Contracts;
using MassTransit;

namespace Cart.Domain.SetDiscountsUsed;

public class SetDiscountsUsedMqCommand
{
    private readonly IRequestClient<SetDiscountUsedRequest> _requestClient;
    
    public SetDiscountsUsedMqCommand(IRequestClient<SetDiscountUsedRequest> requestClient)
    {
        _requestClient = requestClient;
    }
    
    public async Task<SetDiscountUsedResponse> ExecuteAsync(SetDiscountUsedRequest request)
    {
        var response = await _requestClient.GetResponse<SetDiscountUsedResponse>(request);
        return response.Message;
    }

}