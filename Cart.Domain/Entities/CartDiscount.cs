namespace Cart.Domain.Entities;

public class CartDiscount
{
    public string? DiscountCode { get; set; }
    public string? DiscountId { get; set; }
    
    public decimal Value { get; set; }
    
    public decimal Percentage { get; set; }
}