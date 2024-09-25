namespace Cart.Domain.Hub;

public interface ICartHub
{
    Task ReceiveUpdates(CartMessage message, CancellationToken cancellationToken);
    
}