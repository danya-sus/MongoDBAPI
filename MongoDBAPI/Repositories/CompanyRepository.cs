using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBAPI.Models;
using MongoDBAPI.Repositories.Context;
using System.Linq;

namespace MongoDBAPI.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        const string FIND_ALL = "fildAll";
        const string FIND_BY_ID = "findById";
        const string INSERT = "insert";
        const string UPDATE = "update";
        const string DELETE = "delete";
        const string AGGREGATE = "aggregate";

        private readonly IAirlineContext _context;
        private readonly ILogger<CompanyRepository> _logger;
        protected IMongoCollection<Company> _collection;
        protected IMongoCollection<BsonDocument> _bsonCollection;
        protected IMongoCollection<BsonDocument> _profileCollection;

        public CompanyRepository(IAirlineContext context, ILogger<CompanyRepository> logger, IOptions<MongoSettings> options)
        {
            this._context = context;
            this._collection = _context.GetCollection<Company>(options.Value.Companies);
            this._bsonCollection = _context.GetCollection<BsonDocument>(options.Value.Companies);
            this._profileCollection = _context.GetCollection<BsonDocument>(options.Value.Profile);
            this._logger = logger;
        }

        public async Task<long> CountAsync() 
            => await _collection.CountDocumentsAsync(_ => true);

        public async Task<List<Company>> GetAllAsync()
        {
            var comment = GetNewComment();
            var options = new FindOptions();
            options.Comment = comment;

            var res = await _collection.Find(_ => true, options).ToListAsync();

            await PrintAsync(FIND_ALL, comment);

            return res;
        }

        public async Task<Company> GetAsync(int id)
        {
            var comment = GetNewComment();
            var options = new AggregateOptions();
            options.Comment = comment;

            var pipeline = new BsonArray();

            pipeline.Add(new BsonDocument("$lookup",
                                new BsonDocument("from", "flights")
                                    .Add("localField", "_id")
                                    .Add("foreignField", "plane_id")
                                    .Add("as", "flights")));

            var lookup = new BsonDocument("$lookup",
                                new BsonDocument("from", "planes")
                                    .Add("localField", "_id")
                                    .Add("foreignField", "company_id")
                                    .Add("pipeline", pipeline)
                                    .Add("as", "planes"));

            var result = await _bsonCollection
                                .Aggregate(options)
                                .Match(new BsonDocument("_id", id))
                                .AppendStage<BsonDocument>(lookup)
                                .As<Company>()
                                .SingleAsync();

            await PrintAsync(AGGREGATE, comment);

            return result;
        }

        public async Task<IEnumerable<Company>> GetByCountryAsync(string country)
        {
            var comment = GetNewComment();
            var options = new AggregateOptions();
            options.Comment = comment;

            var pipeline = new BsonArray();

            pipeline.Add(new BsonDocument("$lookup",
                                new BsonDocument("from", "flights")
                                    .Add("localField", "_id")
                                    .Add("foreignField", "plane_id")
                                    .Add("as", "flights")));

            var lookup = new BsonDocument("$lookup",
                                new BsonDocument("from", "planes")
                                    .Add("localField", "_id")
                                    .Add("foreignField", "company_id")
                                    .Add("pipeline", pipeline)
                                    .Add("as", "planes"));

            var result = await _bsonCollection
                               .Aggregate(options)
                               .Match(new BsonDocument("country", country))
                               .AppendStage<BsonDocument>(lookup)
                               .As<Company>()
                               .ToListAsync();

            await PrintAsync(AGGREGATE, comment);

            return result.Take(100);
        }

        public async Task<IEnumerable<Company>> GetByNameAsync(string name)
        {
            var comment = GetNewComment();
            var options = new AggregateOptions();
            options.Comment = comment;

            var pipeline = new BsonArray();

            pipeline.Add(new BsonDocument("$lookup",
                                new BsonDocument("from", "flights")
                                    .Add("localField", "_id")
                                    .Add("foreignField", "plane_id")
                                    .Add("as", "flights")));

            var lookup = new BsonDocument("$lookup",
                                new BsonDocument("from", "planes")
                                    .Add("localField", "_id")
                                    .Add("foreignField", "company_id")
                                    .Add("pipeline", pipeline)
                                    .Add("as", "planes"));

            var result = await _bsonCollection
                               .Aggregate(options)
                               .Match(new BsonDocument("name", name))
                               .AppendStage<BsonDocument>(lookup)
                               .As<Company>()
                               .ToListAsync();

            await PrintAsync(AGGREGATE, comment);

            return result.Take(100);
        }


        public async Task InsertAsync(Company company) 
        {
            var comment = GetNewComment();
            var options = new InsertOneOptions();
            options.Comment = comment;

            await _collection.InsertOneAsync(company, options);

            await PrintAsync(INSERT, comment);
        }

        public async Task UpdateAsync(Company company)
        {
            var comment = GetNewComment();
            var options = new ReplaceOptions();
            options.Comment = comment;

            await _collection.ReplaceOneAsync(e => e.Id == company.Id, company, options);

            await PrintAsync(UPDATE, comment);
        }

        public async Task DeleteAsync(int id)
        {
            var comment = GetNewComment();
            var options = new DeleteOptions();
            options.Comment = comment;

            await _collection.DeleteOneAsync(e => e.Id == id, options);

            await PrintAsync(DELETE, comment);
        }

        private string GetNewComment()
            => DateTime.Now.GetHashCode().ToString();

        private async Task PrintAsync(string operation, string comment)
        {
            _logger.LogInformation(comment);

            var filter = new BsonDocument { { "command.comment", comment } };
            var entries = await _profileCollection.Find(filter).ToListAsync();

            _logger.LogInformation($"Operation: {operation}, Time: {entries.Last()["millis"].ToString()}ms");
        }
    }
}
