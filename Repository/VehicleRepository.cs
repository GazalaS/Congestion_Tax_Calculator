using Congestion.Calculator.Model;
using Microsoft.EntityFrameworkCore;

namespace Congestion.Calculator.Repository
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetVehicleTypeAsync(string vehicleName); // Async method to get vehicle by name
        Task<bool> isValidVehicleAsync(Vehicle vehicle); // Async validation check for vehicle
        Task<bool> IsTollFreeVehicleAsync(List<Vehicle> taxExemptVehicles, Vehicle vehicle); // Async check for toll-free vehicle
        Task<List<Vehicle>> GetVehiclesByTypeAsync(VehicleType type); // Async method to get vehicles by type
    }

    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApiContext _context;

        public VehicleRepository(ApiContext context)
            => _context = context;

        // Async method to get a vehicle by its name
        // Use FirstOrDefaultAsync to asynchronously find a vehicle by name
        public async Task<Vehicle?> GetVehicleTypeAsync(string vehicleName)
            => await _context.Vehicles!.FirstOrDefaultAsync(a => a.Name == vehicleName);


        // Async method to check if a vehicle is valid based on its existence in the database
        public async Task<bool> isValidVehicleAsync(Vehicle vehicle)
        {
            // Use AnyAsync to check asynchronously if the vehicle exists in the database
            return await _context.Vehicles!.AnyAsync(v => v.Name == vehicle.Name);
        }

        // Async method to check if a vehicle is toll-free (tax-exempt)
        public async Task<bool> IsTollFreeVehicleAsync(List<Vehicle> taxExemptVehicles, Vehicle vehicle)
        {
            // Use Any() to check if the vehicle exists in the tax-exempt list (can also be done asynchronously if needed)
            return await Task.FromResult(taxExemptVehicles?.Any(t => t.Name.Equals(vehicle.Name)) ?? false);
        }

        // Async method to get a list of vehicles by their type
        public async Task<List<Vehicle>> GetVehiclesByTypeAsync(VehicleType type)
        {
            // Asynchronously get a list of vehicles by type
            return await _context.Vehicles.Where(v => v.Type == type).ToListAsync();
        }
    }
}
/* public class VehicleRepository : IVehicleRepository
{
    private readonly ApiContext _context;

    public VehicleRepository(ApiContext context)
        => _context = context;

    public async Task<Vehicle> GetVehicleTypeAsync(string vehicleName)
        => await _context.Vehicles!.FirstOrDefaultAsync(a => a.Name == vehicleName);

    public async Task<bool> IsValidVehicleAsync(Vehicle vehicle)
        => await _context.Vehicles!.AnyAsync(v => v.Name == vehicle.Name);

    public async Task<bool> IsTollFreeVehicleAsync(List<Vehicle> taxExemptVehicles, Vehicle vehicle)
        => await Task.FromResult(taxExemptVehicles?.Any(t => t.Name.Equals(vehicle.Name)) ?? false);

    public async Task<List<Vehicle>> GetVehiclesByTypeAsync(VehicleType type)
        => await _context.Vehicles.Where(v => v.Type == type).ToListAsync();
}
 */