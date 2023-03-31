using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDBAPI.Models;
using MongoDBAPI.Repositories.Context;

namespace MongoDBAPI.Repositories
{
    public class PlaneRepository : IPlaneRepository
    {
        private readonly IAirlineContext _context;
        private readonly ILogger<CompanyRepository> _logger;
        protected IMongoCollection<Plane> _collection;

        public PlaneRepository(IAirlineContext context, ILogger<CompanyRepository> logger, IOptions<MongoSettings> options)
        {
            this._context = context;
            this._collection = _context.GetCollection<Plane>(options.Value.Planes);
            this._logger = logger;
        }

        public async Task<long> CountAsync()
            => await _collection.CountDocumentsAsync(_ => true);

        public async Task<List<Plane>> GetAllAsync()
            => await _collection.Find(_ => true).ToListAsync();


        public async Task InsertAsync(Plane plane)
            => await _collection.InsertOneAsync(plane);

        public async Task UpdateAsync(Plane plane)
            => await _collection.ReplaceOneAsync(e => e.Id == plane.Id, plane);

        public async Task DeleteAsync(int id)
            => await _collection.DeleteOneAsync(e => e.Id == id);
    }
}
