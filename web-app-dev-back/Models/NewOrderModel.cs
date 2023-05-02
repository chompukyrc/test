using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace web_app_dev_back.Models
{
    public class NewOrderModel
    {

        public string Job { get; set; } = null!;

        public string Restaurant { get; set; } = null!;

        public string Destination { get; set; } = null!;

        public string Menu { get; set; } = null!;

        public decimal Count { get; set; } = 0!;

        public string Description { get; set; } = null!;
    }
}
