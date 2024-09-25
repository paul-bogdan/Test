using Cart.Microservice;
using Grpc.Net.Client;

namespace Web.Services;

public interface IOrderService
{
    Task<(bool, string)> SaveOrder(string userId, CancellationToken cancellationToken);
}

public class OrderService :IOrderService
{
    private readonly string CartServiceUrl;

    public OrderService(IConfiguration configuration)
    {
        CartServiceUrl = configuration["CartServiceGrpcUrl"];
    }
    
    
    public async Task<(bool,string)> SaveOrder(string userId,CancellationToken cancellationToken)
    {
        var channel = GrpcChannel.ForAddress(CartServiceUrl);
        var client = new SaveOrderService.SaveOrderServiceClient(channel);
        var data=  await client.SaveOrderAsync(new (){UserId = userId},cancellationToken:cancellationToken);
        return new(data.IsSuccess, data.Message);
    }

}