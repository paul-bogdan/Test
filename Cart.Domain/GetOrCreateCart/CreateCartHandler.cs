using Bogus;
using Cart.Domain.Constants;
using Cart.Domain.Entities;
using Cart.Domain.Mappings;
using MediatR;
using Sol.Caching;

namespace Cart.Domain.GetOrCreateCart;

public class GetOrCreateCartHandler : IRequestHandler<GetOrCreateCartCommand, GetOrCreateCartResponse>
{
    private readonly ICachingService _cachingService;
    
    public GetOrCreateCartHandler(ICachingService cachingService)
    {
        _cachingService = cachingService;
    }
    
    public async Task<GetOrCreateCartResponse> Handle(GetOrCreateCartCommand request, CancellationToken cancellationToken)
    {
        var existingCart=  await _cachingService.GetData<Entities.Cart>(_cachingService.GenerateKey(Keys.Cart ,request.UserId),cancellationToken);

        if (existingCart == null)
        {
           existingCart = GenerateRandomCart();
            await _cachingService.SetData(_cachingService.GenerateKey(Keys.Cart ,request.UserId),existingCart,CachingTimes.Daily,cancellationToken);
        }

        return new GetOrCreateCartResponse()
        {
            Cart = existingCart.ToDto()
        };
    }
    
    public static Entities.Cart GenerateRandomCart()
    {
        var cart = new Entities.Cart();

        var cartItemFaker = new Faker<CartItem>()
            .RuleFor(i => i.Id, f => Guid.NewGuid())
            .RuleFor(i => i.Name, f => f.Commerce.ProductName())
            .RuleFor(i => i.Price, f => f.Random.Decimal(1, 100));

        var items = cartItemFaker.Generate(3);
        items.ForEach(i => cart.AddItem(i));
     

        return cart;
    }
    
}