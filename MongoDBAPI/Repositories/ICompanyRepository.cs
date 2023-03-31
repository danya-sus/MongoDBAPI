using MongoDB.Bson;
using MongoDBAPI.Models;
using MongoDBAPI.Models.DTO;

namespace MongoDBAPI.Repositories
{
    public interface ICompanyRepository
    {
        public Task<long> CountAsync();

        public Task<List<Company>> GetAllAsync();

        public Task<Company> GetAsync(int id);

        public Task<IEnumerable<Company>> GetByCountryAsync(string country);

        public Task<IEnumerable<Company>> GetByNameAsync(string name);

        public Task InsertAsync(Company company);

        public Task UpdateAsync(Company company);

        public Task DeleteAsync(int id);
    }
}
