namespace Cart.Contracts;

public class CartDto
{
    public Guid Id { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
    public decimal Total { get; set; }
    public decimal SubTotal { get; set; }
    public List<CartDiscountDto> Discounts { get; set; } = new();
}


