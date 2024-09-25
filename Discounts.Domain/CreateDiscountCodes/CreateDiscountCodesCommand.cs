using MediatR;

namespace Discounts.Domain.CreateDiscountCodes;

public class CreateDiscountCodesCommand : IRequest<CreateDiscountCodesResponse>
{
    public int CodesCount { get; set; }
}