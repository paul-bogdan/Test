using MediatR;

namespace Cart.Domain.DeleteCart;

public class DeleteCartCommand : IRequest<DeleteCartResponse>
{
    public Guid UserId { get; set; }
}