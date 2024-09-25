using MediatR;
using MongoDB.Bson;

namespace Discounts.Domain.SetDiscountsUsed;

public class SetDiscountsUsedCommand : IRequest<SetDiscountsUsedResponse>
{
  public  List<ObjectId> DiscountIds { get; set; }
}