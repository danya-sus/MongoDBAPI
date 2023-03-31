using MongoDBAPI.Models;

namespace MongoDBAPI.Repositories
{
    public interface IPlaneRepository
    {
        public Task<long> CountAsync();

        public Task<List<Plane>> GetAllAsync();

        public Task InsertAsync(Plane plane);

        public Task UpdateAsync(Plane plane);

        public Task DeleteAsync(int id);
    }
}
