using MediatR;

namespace Discounts.Domain.ValidateDiscountCode;

public class ValidateDiscountCodeCommand : IRequest<ValidateDiscountCodeResponse>
{
    public string? Code { get; set; }
}