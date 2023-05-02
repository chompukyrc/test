using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;
using web_app_dev_back.Models;

namespace web_app_dev_back.Services
{
    public class UsersService
    {
        private readonly IMongoCollection<UserModel> _UsersCollection;

        public UsersService(
            IOptions<Database> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _UsersCollection = mongoDatabase.GetCollection<UserModel>(
                bookStoreDatabaseSettings.Value.UsersCollectionName);
        }

        public async Task<UserModel?> GetProfileByIdAsync(string id) 
        {
            return await _UsersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task RegisterAsync(NewUserModel newUser)
        {
            var _user = await _UsersCollection.Find(x => x.Username == newUser.Username).FirstOrDefaultAsync();
            if (_user == null)
            {
                var hashPassword = string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(newUser.Password)).Select(s => s.ToString("x2")));
                newUser.Password= hashPassword;
                var _newUser = new UserModel
                {
                    Phone = newUser.Phone,
                    Username = newUser.Username,
                    Lastname = newUser.Lastname,
                    Firstname= newUser.Firstname,
                    Password = hashPassword
                };
                await _UsersCollection.InsertOneAsync(_newUser);
            }

            else
            {
                throw new NotImplementedException();
            }
            
            
        }

        public async Task<UserModel?> LoginAsync(LoginModel user)
        {
            var hashPassword = string.Join("", MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(user.Password)).Select(s => s.ToString("x2")));
            user.Password = hashPassword;

            var _user = await _UsersCollection.Find(x => x.Username == user.Username && x.Password == user.Password).FirstOrDefaultAsync();

            return _user;

        }

        public async Task<UserModel?> GetAsync(string id) =>
            await _UsersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task UpdateAsync(string id, UserModel updatedUser) =>
            await _UsersCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);

        public async Task RemoveAsync(string id) =>
            await _UsersCollection.DeleteOneAsync(x => x.Id == id);

        public async Task<List<UserModel>> GetAllUserAsync() => 
            await _UsersCollection.Find(x => true).ToListAsync();
    }
}
