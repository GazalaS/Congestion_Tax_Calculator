using Congestion.Calculator.Model;

namespace Congestion.Calculator.Repository
{
    public interface ICityRepository
    {
        City GetCity(string cityName);
        bool isValidCity(string cityName);
    }
    public class CityRepository : ICityRepository
    {
        private readonly ApiContext _context;
        public CityRepository(ApiContext context) 
            => _context = context;
        public City GetCity(string cityName) 
            => _context.Cities!.FirstOrDefault(a => a.Name == cityName);
        public bool isValidCity(string cityName) 
            => (GetCity(cityName) != null) ? true : false;
    }
}

