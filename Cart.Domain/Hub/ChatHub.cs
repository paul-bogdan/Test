using Cart.Domain.DeleteCart;
using Cart.Domain.GetOrCreateCart;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Cart.Domain.Hub;

public class CartHub : Hub<ICartHub>
{
    private readonly IConnectionMapping Connections;
    private readonly IMediator _mediator;
   
    public CartHub(IConnectionMapping connections, IMediator mediator)
    {
        Connections = connections;
        _mediator = mediator;
    }
    
    public override async Task OnConnectedAsync()
    {
        var httpContext= Context.GetHttpContext();
        var userGuid= httpContext?.Request.Query["guid"].ToString();
        if (userGuid != null)
        {
            
            Connections.Add(userGuid, Context.ConnectionId);
            await SendCartToUser(userGuid);
        }

    }
    
    
    private async Task SendCartToUser(string userId)
    {
        var cart = await _mediator.Send(new GetOrCreateCartCommand()
        {
            UserId = Guid.Parse(userId)
        });
        
        var connectionId = Connections.GetConnections(userId).FirstOrDefault();
        if (!string.IsNullOrEmpty(connectionId))
        {
            await Clients.Client(connectionId).ReceiveUpdates(new CartMessage()
            {
                Cart = cart.Cart
            }, CancellationToken.None);
        }
    }
    
    
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var key = await Connections.GetKeyByConnectionId(Context.ConnectionId);
        if (!string.IsNullOrWhiteSpace(key))
        {
            
            Connections.Remove(Context.ConnectionId);
            await _mediator.Send(new DeleteCartCommand()
            {
                UserId = Guid.Parse(key)
            });
        }
        await base.OnDisconnectedAsync(exception);
    
        
    }
    
    
}