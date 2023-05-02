using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;
using web_app_dev_back.Models;

namespace web_app_dev_back.Services
{
    public class NotificationsService
    {
        private readonly IMongoCollection<NotificationModel> _notificationCollection;

        public NotificationsService(
            IOptions<Database> DatabaseSettings)
        {
            var mongoClient = new MongoClient(
                DatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                DatabaseSettings.Value.DatabaseName);

            _notificationCollection = mongoDatabase.GetCollection<NotificationModel>(
                DatabaseSettings.Value.NotificationsCollectionName);
        }

        public async Task CreateAsync(OrderModel order,string action){
            var _newNoti = new NotificationModel
            {
                Owner = order.Owner,
                Restaurants = order.Restaurant,
                Time = DateTime.Now.ToString(),
                Action = action,
            };

            await _notificationCollection.InsertOneAsync(_newNoti);
        }

        public async Task<List<NotificationModel>> ListByUserIdAsync(string userId)=>
            await _notificationCollection.Find(x => x.Owner == userId).ToListAsync();

        public async Task<List<NotificationModel>> ListUnreadAsync(string userId)=>
            await _notificationCollection.Find(x => x.Owner == userId & x.isRead == false).ToListAsync();

        public async Task UpdateAsync(string id, NotificationModel updatedUser) =>
            await _notificationCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);
    }
}