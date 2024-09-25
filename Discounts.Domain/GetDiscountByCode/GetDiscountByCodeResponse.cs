namespace Discounts.Domain.GetDiscountByCode;

public class GetDiscountByCodeResponse
{
    public string? Id { get; init ; }
    public string? Code { get; init; }
        
    public decimal Percentage { get; init; }
        
    public DateTime CreationDate { get; init; }
        
    public DateTime ExpiryDate { get; init; }
        
    public DateTime? UsedAt { get; set; }
       
    public bool Used { get; set; }
}