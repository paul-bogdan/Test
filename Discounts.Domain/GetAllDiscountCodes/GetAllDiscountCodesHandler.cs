using Discounts.Domain.Database;
using MediatR;
using MongoDB.Driver;

namespace Discounts.Domain.GetAllDiscountCodes;

public class GetAllDiscountCodesHandler : IRequestHandler<GetAllDiscountCodesQuery, List<GetAllDiscountCodesResponse>>
{
    private readonly IDiscountsDbContext _context;

    public GetAllDiscountCodesHandler(IDiscountsDbContext context)
    {
        _context = context;
    }

    public async Task<List<GetAllDiscountCodesResponse>> Handle(GetAllDiscountCodesQuery request,  CancellationToken cancellationToken)
    {
        var discounts = await _context.Discounts.Find(_ => true).ToListAsync(cancellationToken);
        return discounts.Select(x => new GetAllDiscountCodesResponse { Code = x.Code, Used = x.Used,Percentage = x.Percentage}).ToList();

    }
}