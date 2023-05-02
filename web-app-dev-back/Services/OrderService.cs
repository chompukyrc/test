using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;
using web_app_dev_back.Models;

namespace web_app_dev_back.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<OrderModel> _OrderCollection;

        public OrderService(
            IOptions<Database> DatabaseSettings)
        {
            var mongoClient = new MongoClient(
                DatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                DatabaseSettings.Value.DatabaseName);

            _OrderCollection = mongoDatabase.GetCollection<OrderModel>(
                DatabaseSettings.Value.OrdersCollectionName);
        }

        public async Task CreateAsync(NewOrderModel newOrder, string owner)
        {
            var _newOrder = new OrderModel
            {
                Count = newOrder.Count,
                Menu = newOrder.Menu,
                Job = newOrder.Job,
                Restaurant = newOrder.Restaurant,
                Description = newOrder.Description,
                Destination = newOrder.Destination,
                Status = "waiting", /*waiting, accept, reject*/
                Owner = owner
            };

            await _OrderCollection.InsertOneAsync(_newOrder);
        }

        public async Task<List<OrderModel>> ListAsync() =>
            await _OrderCollection.Find(_ => true).ToListAsync();

        public async Task<List<OrderModel>> ListOrderByJobIDAsync(string jobId) =>
            await _OrderCollection.Find(e => e.Job == jobId).ToListAsync();

        public async Task<List<OrderModel>> ListOrderByUserIdAsync(string userId) =>
            await _OrderCollection.Find(e => e.Owner == userId).ToListAsync();

        public async Task<List<OrderModel>> ListStatusByJobIDAsync(string jobId,string status) =>
            await _OrderCollection.Find(e => e.Status == status & e.Job == jobId).ToListAsync();
            
        public async Task<OrderModel?> GetAsync(string id) =>
            await _OrderCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task UpdateStatusAsync(string id, string status)
        {
            Console.WriteLine(id + " " + status);
            await _OrderCollection.FindOneAndUpdateAsync(rec => rec.Id == id,
                Builders<OrderModel>.Update.Set(rec => rec.Status, status));
        }
    }
}
