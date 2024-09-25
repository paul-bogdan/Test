using Discounts.Domain.Database;
using MediatR;
using MongoDB.Driver;

namespace Discounts.Domain.GetDiscountByCode;

public class GetDiscountByCodeHandler : IRequestHandler<GetDiscountByCodeQuery, GetDiscountByCodeResponse?>
{
    private readonly IDiscountsDbContext _context;
    
    public GetDiscountByCodeHandler(IDiscountsDbContext context)
    {
        _context = context;
    }
    
    public async Task<GetDiscountByCodeResponse?> Handle(GetDiscountByCodeQuery request, CancellationToken cancellationToken)
    { 
        var discount = await _context.Discounts.Find(x => x.Code == request.Code).FirstOrDefaultAsync(cancellationToken);
        if (discount == null)
        {
            return null;
        }
        return new GetDiscountByCodeResponse
        {
            Id = discount.Id.ToString(),
            Code = discount.Code,
            Percentage = discount.Percentage,
            CreationDate = discount.CreationDate,
            ExpiryDate = discount.ExpiryDate,
            UsedAt = discount.UsedAt,
            Used = discount.Used
        };
    }
}