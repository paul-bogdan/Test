namespace Discounts.Domain.GetAllDiscountCodes;

public class GetAllDiscountCodesResponse
{
   
    public string? Id { get; init ; }
    public string? Code { get; init; }
        
    public decimal Percentage { get; init; }
        
    public DateTime CreationDate { get; init; }
        
    public DateTime ExpirationDate { get; init; }
        
    public DateTime? UsedAt { get; set; }
       
    public bool Used { get; set; }
}