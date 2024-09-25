using Cart.Contracts;
using MediatR;

namespace Cart.Domain.AddDiscount;

public class AddDiscountCommand : IRequest<AddDiscountResponse>
{
    public Guid UserId { get; set; }
    public string? DiscountId { get; set; }
    public decimal Percentage { get; set; }
    public string? DiscountCode { get; set; }
}