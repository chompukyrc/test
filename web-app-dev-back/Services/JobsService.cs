using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;
using web_app_dev_back.Models;

namespace web_app_dev_back.Services
{
    public class JobsService
    {
        private readonly IMongoCollection<JobModel> _JobCollection;

        public JobsService(
            IOptions<Database> DatabaseSettings)
        {
            var mongoClient = new MongoClient(
                DatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                DatabaseSettings.Value.DatabaseName);

            _JobCollection = mongoDatabase.GetCollection<JobModel>(
                DatabaseSettings.Value.JobsCollectionName);
        }

        public async Task CreateAsync(NewJobModel newJob, string owner)
        {
            var _newJob = new JobModel
            {
                Description = newJob.Description,
                Limit = newJob.Limit,
                Restaurants = newJob.Restaurants,
                Time = newJob.Time,
                Status = "unfinish",
                Owner = owner,
                Count = 0
            };
            await _JobCollection.InsertOneAsync(_newJob);
        }

        public async Task<List<JobModel>> ListAsync(List<string> status) =>
            await _JobCollection.Find(e => status.Contains(e.Status)).ToListAsync();
        public async Task<JobModel?> GetAsync(string id) =>
            await _JobCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task<JobModel?> GetByUserIdAsync(string userId)=>
            await _JobCollection.Find(x => x.Owner == userId).FirstOrDefaultAsync();
        public async Task UpdateAsync(string id, JobModel updatedJob) =>
            await _JobCollection.ReplaceOneAsync(x => x.Id == id, updatedJob);
    }
}
