using MediatR;
using Sol.Caching;

namespace Cart.Domain.DeleteCart;

public class DeleteCartHandler : IRequestHandler<DeleteCartCommand,DeleteCartResponse>
{
    private readonly ICachingService _cachingService;
    
    public DeleteCartHandler(ICachingService cachingService)
    {
        _cachingService = cachingService;
    }
    
    
    public async Task<DeleteCartResponse> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
    {
       await  _cachingService.RemoveData(_cachingService.GenerateKey(Constants.Keys.Cart,request.UserId),cancellationToken);
         return new();
         
    }
}