using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace web_app_dev_back.Models
{
    public class UserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Firstname { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public bool PremiumMember { get; set; } = false;

    }
}
