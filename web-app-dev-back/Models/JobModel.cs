using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace web_app_dev_back.Models
{
    public class JobModel
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public string Owner { get; set; } = null!;

        public string Restaurants { get; set; } = null!;

        public decimal Time { get; set; }

        public decimal Limit { get; set; }

        public string Description { get; set; } = null!;

        public string Status { get; set; } = null!;

        public decimal Count { get; set; } = 0;
    }
}
