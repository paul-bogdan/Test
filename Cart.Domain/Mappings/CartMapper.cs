using Cart.Contracts;

namespace Cart.Domain.Mappings;

public static class CartMapper
{
    public static CartDto ToDto(this Entities.Cart cart)
    {
        return new CartDto
        {
            Id = cart.Id,
            Items = cart.Items.Select(i => new CartItemDto
            {
                Id = i.Id,
                Name = i.Name,
                Price = i.Price
            }).ToList(),
            Total = cart.Total,
            SubTotal = cart.SubTotal,
            Discounts = cart.Discounts.Select(d => new CartDiscountDto
            {
                DiscountId = d.DiscountId,
                DiscountCode = d.DiscountCode,
                Value = d.Value
            }).ToList()
        };
    }
}