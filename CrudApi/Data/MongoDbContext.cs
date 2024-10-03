using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace CrudApi.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _db;

        public MongoDbContext(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDb"));
            _db = client.GetDatabase(config["DatabaseName"]);
        }

        public virtual IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }
}