using MediatR;

namespace Cart.Domain.GetOrCreateCart;

public class GetOrCreateCartCommand :   IRequest<GetOrCreateCartResponse>
{
    public Guid UserId { get; set; }
    
}