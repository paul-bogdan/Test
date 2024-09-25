using Cart.Contracts;

namespace Cart.Domain.Hub;

public class CartMessage
{
    public CartDto? Cart { get; set; }
    
}