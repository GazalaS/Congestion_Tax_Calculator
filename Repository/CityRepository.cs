using Congestion.Calculator.Model;
using Microsoft.EntityFrameworkCore;

namespace Congestion.Calculator.Repository
{
    public interface ICityRepository
    {
        Task<City?> GetCityByNameAsync(string cityName); // Updated to async
        Task<bool> IsValidCityAsync(string cityName);  // Updated to async
    }

    public class CityRepository : ICityRepository
    {
        private readonly ApiContext _context;

        public CityRepository(ApiContext context)
            => _context = context;

        // Updated to async
        public async Task<City?> GetCityByNameAsync(string cityName)
            => await _context.Cities!.FirstOrDefaultAsync(a => a.Name == cityName);

        // Updated to async
        public async Task<bool> IsValidCityAsync(string cityName)
            => await _context.Cities!.AnyAsync(a => a.Name == cityName);  // Checks existence asynchronously
    }
}

