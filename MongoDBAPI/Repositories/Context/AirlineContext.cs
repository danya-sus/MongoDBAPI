using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBAPI.Repositories.Context
{
    public class AirlineContext : IAirlineContext
    {
        private readonly IMongoDatabase _database;
        private readonly IMongoClient _client;

        public AirlineContext(IOptions<MongoSettings> options)
        {
            _client = new MongoClient(options.Value.ConnectionString);
            _database = _client.GetDatabase(options.Value.DatabaseName);
            _database.RunCommand<BsonDocument>(new BsonDocument("profile", 2));
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            return _database.GetCollection<T>(name);
        }
    }
}
