using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Discounts.Domain.Entities
{
    public class Discount 
    {
        [BsonId]
        public ObjectId Id { get; init ; }
        public string? Code { get; init; }
        
        public decimal Percentage { get; init; }
        
        public DateTime CreationDate { get; init; }
        
        public DateTime ExpiryDate { get; init; }
        
        public DateTime? UsedAt { get; set; }
       
        public bool Used { get; set; }
    }
}