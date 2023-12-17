using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace CatalogService.Models
{
    public class CatalogDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid CatalogId { get; set; }
        public DateTime Time { get; set; }
        public List<int> ImageIds  { get; set; }
        public int AppraisalPrice { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string Location { get; set; }
        
        [BsonRepresentation(BsonType.String)]
        public Guid SellerId { get; set; }

    }
}
