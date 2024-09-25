using MediatR;

namespace Discounts.Domain.GetDiscountByCode;

public class GetDiscountByCodeQuery : IRequest<GetDiscountByCodeResponse?>
{
    public string? Code { get; set; }
}