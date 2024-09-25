using Discounts.Domain.Database;
using Discounts.Domain.Entities;
using MediatR;
using MongoDB.Driver;

namespace Discounts.Domain.ValidateDiscountCode;

public class ValidateDiscountCodeHandler : IRequestHandler<ValidateDiscountCodeCommand,ValidateDiscountCodeResponse>
{
    private readonly IDiscountsDbContext _context; 
    
    public ValidateDiscountCodeHandler(IDiscountsDbContext context)
    {
        _context = context;
    }
    
    
    public async Task<ValidateDiscountCodeResponse> Handle(ValidateDiscountCodeCommand request, CancellationToken cancellationToken)
    {
        
        if(string.IsNullOrEmpty(request.Code)) return new ValidateDiscountCodeResponse(){ IsValid = false};
        
        var filter = Builders<Discount>.Filter.Eq(d => d.Code, request.Code);
        var discount = await _context.Discounts.Find(filter).FirstOrDefaultAsync(cancellationToken);
        if(discount == null) return new ValidateDiscountCodeResponse(){ IsValid = false};  

        return new ValidateDiscountCodeResponse()
        {
            IsValid = !discount.Used && discount.ExpiryDate > DateTime.UtcNow
        };
    }
}