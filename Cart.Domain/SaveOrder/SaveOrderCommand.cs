using MediatR;

namespace Cart.Domain.SaveOrder;

public class SaveOrderCommand : IRequest<SaveOrderResponse>
{
    public Guid UserId { get; set; }
}