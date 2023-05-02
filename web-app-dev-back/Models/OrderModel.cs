using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace web_app_dev_back.Models
{
    public class OrderModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public string Job { get; set; } = null!;

        public string Owner { get; set; } = null!;

        public string Restaurant { get; set; } = null!;

        public string Destination { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string Menu { get; set; } = null!;

        public decimal Count { get; set; } = 0!;

        public string Description { get; set; } = null!;

    }
}
