using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace web_app_dev_back.Models
{
    public class NewJobModel
    {

        public string Restaurants { get; set; } = null!;

        public decimal Time { get; set; }

        public decimal Limit { get; set; }

        public string Description { get; set; } = null!;


    }
}
