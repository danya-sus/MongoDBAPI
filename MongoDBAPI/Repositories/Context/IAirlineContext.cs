using MongoDB.Driver;

namespace MongoDBAPI.Repositories.Context
{
    public interface IAirlineContext
    {
        IMongoCollection<Company> GetCollection<Company>(string name);
    }
}
