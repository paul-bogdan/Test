namespace Cart.Contracts;

public class AddDiscountToCartRequest
{
    public Guid UserId { get; set; }
    public string? DiscountId { get; set; }
    public string? DiscountCode { get; set; }
    public decimal Percentage { get; set; }
}