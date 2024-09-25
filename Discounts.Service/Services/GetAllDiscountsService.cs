using System.Globalization;
using Discounts.Domain.CreateDiscountCodes;
using Discounts.Domain.GetAllDiscountCodes;
using Grpc.Core;
using MediatR;


namespace Discounts.Service.Services;

public class GetAllDiscountsService : GetAllDiscountCodesService.GetAllDiscountCodesServiceBase
{
    private readonly IMediator _mediator;
    
    public GetAllDiscountsService( IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public override async Task<GetAllDiscountCodesListResponse> GetAllDiscountCodes(GetAllDiscountCodesRequest request, ServerCallContext context)
    {
        var resp= await _mediator.Send(new GetAllDiscountCodesQuery(),context.CancellationToken);
        return new GetAllDiscountCodesListResponse()
        {
            DiscountCodes = {resp.Select(x=> new GetAllDiscountCodesResponse()
            {
                Code = x.Code,
                Used = x.Used,
                Percentage = x.Percentage.ToString(CultureInfo.InvariantCulture)
            })}
        };  
    }
}