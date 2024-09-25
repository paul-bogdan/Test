
using Discounts.Service;
using Grpc.Net.Client;
using Web.Models;

namespace Web.Services;
public  interface IDiscountService
{
    Task CreateDiscounts(int count,CancellationToken cancellationToken);
    Task<bool> ValidateDiscount(string code, CancellationToken cancellationToken);
    Task<List<DiscountsDto>> GetAllDiscounts(CancellationToken cancellationToken);
    Task<(bool, string)> AddDiscountToCart(string code, string userId, CancellationToken cancellationToken);
}

public class DiscountService : IDiscountService
{
    private readonly string DiscountServiceUrl;

    public DiscountService(IConfiguration configuration)
    {
        DiscountServiceUrl = configuration["DiscountServiceUrl"];
    }
    
    
    public async Task CreateDiscounts(int count,CancellationToken cancellationToken)
    {
        var channel = GrpcChannel.ForAddress(DiscountServiceUrl);
        var client = new CreateDiscounts.CreateDiscountsClient(channel);
        await client.CreateDiscountAsync(new CreateDiscountRequest(){CodesCount = count},cancellationToken:cancellationToken);
    }
    
    
    public async Task<bool> ValidateDiscount(string code,CancellationToken cancellationToken)
    {
        var channel = GrpcChannel.ForAddress(DiscountServiceUrl);
        var client = new ValidateDiscountCode.ValidateDiscountCodeClient(channel);
        var response = await client.ValidateCodeAsync(new ValidateDiscountCodeRequest(){Code = code}, cancellationToken:cancellationToken);
        return response.IsValid;
    }
    
    public async Task<List<DiscountsDto>> GetAllDiscounts(CancellationToken cancellationToken)
    {
        var channel = GrpcChannel.ForAddress(DiscountServiceUrl);
        var client = new GetAllDiscountCodesService.GetAllDiscountCodesServiceClient(channel);
        var response = await client.GetAllDiscountCodesAsync(new(),cancellationToken:cancellationToken);
  
        return   response.DiscountCodes.Select(x=>new DiscountsDto()
        {
            Code = x.Code,
            Used = x.Used,
            Percentage = Convert.ToDecimal( x.Percentage)
        }).ToList();
    }
    
    public async Task<(bool,string)> AddDiscountToCart(string code,string userId,CancellationToken cancellationToken)
    {
        var channel = GrpcChannel.ForAddress(DiscountServiceUrl);
        var client = new AddDiscountToCartService.AddDiscountToCartServiceClient(channel);
       var response= await client.AddDiscountToCartAsync(new (){ DiscountCode = code, UserId = userId},cancellationToken:cancellationToken);
       return new(response.Success, response.Message);
    }
    
    
}