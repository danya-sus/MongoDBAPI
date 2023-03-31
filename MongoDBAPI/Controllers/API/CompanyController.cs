using Microsoft.AspNetCore.Mvc;
using MongoDBAPI.Models;
using MongoDBAPI.Models.DTO;
using MongoDBAPI.Repositories;

namespace MongoDBAPI.Controllers.API
{
    [ApiController]
    [Route("api/{controller}")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _repository;
        private readonly ILogger<CompanyController> _logger;

        public CompanyController(ICompanyRepository repository, ILogger<CompanyController> logger)
        {
            this._repository = repository;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [HttpGet("by_id/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            if (id == default) return BadRequest();

            var item = await _repository.GetAsync(id);

            if (item == default) return NotFound();

            return Ok(item);
        }

        [HttpGet("by_name/{name}")]
        public async Task<IActionResult> GetByNameAsync(string name)
        {
            if (name == "") return BadRequest();

            var item = await _repository.GetByNameAsync(name);

            if (item == default) return NotFound();

            return Ok(item);
        }

        [HttpGet("by_country/{country}")]
        public async Task<IActionResult> GetByCountryAsync(string country)
        {
            if (country == "") return BadRequest();

            var item = await _repository.GetByCountryAsync(country);

            if (item == default) return NotFound();

            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(CompanyDTO dto)
        {
            var company = new Company
            {
                Id = dto.Id,
                Name = dto.Name,
                Code = dto.Code,
                Country = dto.Country,
                YearOfFoundation = dto.YearOfFoundation
            };

            await _repository.InsertAsync(company);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Company company)
        {
            await _repository.UpdateAsync(company);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id == default) return BadRequest();

            await _repository.DeleteAsync(id);

            return NoContent();
        }
    }
}
