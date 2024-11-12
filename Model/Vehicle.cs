using System.ComponentModel.DataAnnotations;

namespace Congestion.Calculator.Model
{
    public enum VehicleType
    {
        Emergency,        // Emergency vehicles (e.g., ambulances, fire trucks)
        Bus,              // Buses
        Diplomat,         // Diplomatic vehicles
        Motorcycle,       // Motorcycles
        Military,         // Military vehicles
        Foreign,          // Foreign vehicles (vehicles not registered in the city/country)
        NonTaxExempted,   // Non-tax exempted vehicles (vehicles that are subject to tax)
    }

    public class Vehicle
    {
        public int VehicleId { get; set; }

        [Required(ErrorMessage = "Vehicle name is required")]
        [StringLength(60)]
        public string? Name { get; set; }

        // Vehicle type, uses the VehicleType enum
        public int VehicleTypeId { get; set; }
        public VehicleType Type { get; set; }

        public string? LicensePlate { get; set; }
    }
}
