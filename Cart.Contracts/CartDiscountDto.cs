namespace Cart.Contracts;

public class CartDiscountDto
{
    public string? DiscountCode { get; set; }
    public string? DiscountId { get; set; }
    public decimal Value { get; set; }
}