using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace web_app_dev_back.Models
{
    public class LoginModel
    {
       
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;


    }
}
