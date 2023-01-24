using Congestion.Calculator.Model;
using Congestion.Calculator.Repository;
using Congestion.Calculator.Util;

namespace Congestion.Calculator.Service
{
    public interface ICongestionTaxCalculatorService
    {
       Task<int> GetTax(Vehicle vehicle, List<DateTime> dates, string cityName);
    }

    public class CongestionTaxCalculatorService : ICongestionTaxCalculatorService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public CongestionTaxCalculatorService(ICityRepository cityRepository, IVehicleRepository vehicleRepository)
        {
            _cityRepository = cityRepository;
            _vehicleRepository = vehicleRepository;

        }
        public Task<int> GetTax(Vehicle vehicle, List<DateTime> dates, string cityName) 
        {
            var taxAmount = 0;
            var city = new City();
            
            /** check if valid City name */
            if (_cityRepository.isValidCity(cityName)) 
                city = _cityRepository.GetCity(cityName);
            else
                return Task.FromResult(taxAmount);

            /** check for tax exempt vehicle */
            if (_vehicleRepository.isTollFreeVehicle(city.TaxExemptVehicles!, vehicle)) return Task.FromResult(taxAmount);
            
            /** check dates */
            if (dates == null || dates.Count == 0) return Task.FromResult(taxAmount);

            dates.Sort();

            /** remove weekends, public holidays, days before or after a public holiday as per configuration and during the holiday month*/
            foreach (var date in dates)
            {
                if (CalculationUtil.IsTollFreeDate(date, city))
                    dates.Remove(date);
            }

            /** calculate by single charge rule */
            Dictionary<String, List<int>> chargesPerDay = CalculationUtil.GetSingleChargeRule(dates, city);

            /** calculate for total charge */
            taxAmount = CalculationUtil.CalculateTotalTaxBySingleChargeRule(city, chargesPerDay);

            return Task.FromResult(taxAmount);
        }  
    }
}

