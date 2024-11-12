using Congestion.Calculator.Model;
using Congestion.Calculator.Repository;
using Congestion.Calculator.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        private readonly ILogger<CongestionTaxCalculatorService> _logger;

        public CongestionTaxCalculatorService(
            ICityRepository cityRepository,
            IVehicleRepository vehicleRepository,
            ILogger<CongestionTaxCalculatorService> logger)
        {
            _cityRepository = cityRepository;
            _vehicleRepository = vehicleRepository;
            _logger = logger;
        }

        public async Task<int> GetTax(Vehicle vehicle, List<DateTime> dates, string cityName)
        {
            int taxAmount = 0;

            try
            {
                _logger.LogInformation("Starting tax calculation for vehicle with ID: {VehicleId} in city: {CityName}", vehicle.VehicleId, cityName);

                // Check if the city is valid
                var city = await _cityRepository.GetCityByNameAsync(cityName);
                if (city == null)
                {
                    _logger.LogWarning("City '{CityName}' is not valid or not found.", cityName);
                    return taxAmount; // Return 0 if the city is not valid
                }

                _logger.LogInformation("City '{CityName}' found, checking if vehicle is tax-exempt.", cityName);

                // Check if the vehicle is toll-free (tax-exempt)
                if (city.TaxExemptVehicles != null && await _vehicleRepository.IsTollFreeVehicleAsync(city.TaxExemptVehicles, vehicle)) // Call the async method
                {
                    _logger.LogInformation("Vehicle with ID {VehicleId} is tax-exempt.", vehicle.VehicleId);
                    return taxAmount; // Return 0 if the vehicle is tax-exempt
                }

                // Validate if the dates list is not null or empty
                if (dates == null || !dates.Any())
                {
                    _logger.LogWarning("No valid dates provided for tax calculation.");
                    return taxAmount; // Return 0 if no dates are provided
                }

                // Sort the dates list for easier processing
                dates.Sort();

                // Filter out the toll-free dates (weekends, public holidays, days before or after a public holiday, and holiday months)
                var validDates = dates.Where(date => !CalculationUtil.IsTollFreeDate(date, city)).ToList();

                // If no valid dates remain, return 0
                if (!validDates.Any())
                {
                    _logger.LogInformation("No valid dates remaining after filtering toll-free dates.");
                    return taxAmount;
                }

                // Calculate the single charge rule for the remaining valid dates
                var chargesPerDay = CalculationUtil.GetSingleChargeRule(validDates, city);

                // Calculate the total tax amount based on the charges per day
                taxAmount = CalculationUtil.CalculateTotalTaxBySingleChargeRule(city, chargesPerDay);

                _logger.LogInformation("Tax calculation complete. Total tax amount: {TaxAmount} for vehicle ID: {VehicleId} in city: {CityName}", taxAmount, vehicle.VehicleId, cityName);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating the tax for vehicle with ID {VehicleId} in city: {CityName}", vehicle.VehicleId, cityName);
                // Return 0 if any error occurs
                taxAmount = 0;
            }

            return taxAmount;
        }
    }

}
