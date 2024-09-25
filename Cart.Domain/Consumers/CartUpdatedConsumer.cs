using Cart.Contracts;
using Cart.Domain.Hub;
using Cart.Domain.Mappings;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Sol.Caching;

namespace Cart.Domain.Consumers
{
    public class CartUpdatedConsumer : IConsumer<CartUpdatedRequest>
    {
        private readonly IHubContext<CartHub, ICartHub> _chatHub;
        private readonly ICachingService _cachingService;
        private readonly IConnectionMapping _connections;

        public CartUpdatedConsumer(IHubContext<CartHub, ICartHub> chatHub, ICachingService cachingService, IConnectionMapping connections)
        {
            _chatHub = chatHub;
            _cachingService = cachingService;
            _connections = connections;
        }

        public async Task Consume(ConsumeContext<CartUpdatedRequest> context)
        {
            var cart = await _cachingService.GetData<Entities.Cart>(_cachingService.GenerateKey(Constants.Keys.Cart, context.Message.UserId), context.CancellationToken);
            var connections = _connections.GetConnections(context.Message.UserId.ToString());
            foreach (var connection in connections)
            {
                await _chatHub.Clients.Client(connection).ReceiveUpdates(new CartMessage()
                {
                    Cart = cart.ToDto()
                }, context.CancellationToken);
            }
        }
    }
}