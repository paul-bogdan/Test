using Cart.Domain.SaveOrder;
using Grpc.Core;
using MediatR;


namespace Cart.Microservice.Services
{
    public class OrderService : SaveOrderService.SaveOrderServiceBase
    {
        private readonly IMediator _mediator;

        public OrderService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<SaveOrderResponse> SaveOrder(SaveOrderRequest request, ServerCallContext context)
        {
            try
            {
                await _mediator.Send(new SaveOrderCommand()
                {
                    UserId = Guid.Parse(request.UserId)
                }, context.CancellationToken);
                return new SaveOrderResponse()
                {
                    IsSuccess = true,
                    Message = "Order saved successfully"
                };
            }
            catch (Exception e)
            {
                return new SaveOrderResponse()
                {
                    IsSuccess = false,
                    Message = e.Message
                };
            }
        }
    }
}