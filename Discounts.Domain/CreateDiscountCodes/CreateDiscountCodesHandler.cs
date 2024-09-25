using Discounts.Domain.Database;
using Discounts.Domain.Entities;
using Discounts.Domain.Services;
using MediatR;

namespace Discounts.Domain.CreateDiscountCodes;

public class CreateDiscountCodesHandler : IRequestHandler<CreateDiscountCodesCommand,CreateDiscountCodesResponse>
{
    private readonly IDiscountsDbContext _context;
    private readonly Random _random;

    public CreateDiscountCodesHandler(IDiscountsDbContext context)
    {
        _context = context;
        _random = new Random();
    }
    public async Task<CreateDiscountCodesResponse> Handle(CreateDiscountCodesCommand request, CancellationToken cancellationToken)
    {
        var codes=  DiscountCodesGeneratorService.GenerateDiscountCodes(request.CodesCount);
        var discounts = codes.Select(code => new Discount { Code = code, CreationDate = DateTime.UtcNow, Used = false, Percentage =_random.Next(1, 100), ExpiryDate = DateTime.UtcNow.AddDays(30)}).ToList();
        await  _context.Discounts.InsertManyAsync(discounts,cancellationToken:cancellationToken);
        return new CreateDiscountCodesResponse();
    }
   
}