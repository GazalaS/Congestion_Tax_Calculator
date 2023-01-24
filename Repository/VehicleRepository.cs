using Congestion.Calculator.Model;

namespace Congestion.Calculator.Repository
{
    public interface IVehicleRepository
    {
       Vehicle GetVehicleType(string vehicleName);
       bool isValidVehicle(Vehicle vehicle);
       bool isTollFreeVehicle(List<Vehicle> taxExemptVehicles, Vehicle vehicle);
    }
    public class VehicleRepository: IVehicleRepository 
    {
        private readonly ApiContext _context;
        public VehicleRepository(ApiContext context)
            => _context = context;
        public Vehicle GetVehicleType(string vehicleName) 
            => _context.Vehicles!.FirstOrDefault(a => a.Name == vehicleName);   
        public bool isValidVehicle(Vehicle vehicle) 
            => (GetVehicleType(vehicle.Name!) != null) ? true : false;
        public bool isTollFreeVehicle(List<Vehicle> taxExemptVehicles, Vehicle vehicle)
            => (taxExemptVehicles == null) ? false : (taxExemptVehicles.Where(t => t.Name!.Equals(vehicle.Name)).Count() > 0) ? true : false;
    }
}
