using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace web_app_dev_back.Models
{
    public class NotificationModel
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public string Owner { get; set; } = null!;

        public string Restaurants { get; set; } = null!;

        public string Time { get; set; } = null!;

        public string Action {get; set; } = null!;

        public bool isRead { get; set; } = false;
    }
}
