using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
 
namespace CatalogService.Models
{
    public class CatalogDTO
    {
        [BsonId]
        public int CatalogId { get; set; }
        public DateTime Time { get; set; }
        public List<int> ImageIds  { get; set; }
        public int AppraisalPrice { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string Location { get; set; }
        public int SellerId { get; set; }

    }
}
