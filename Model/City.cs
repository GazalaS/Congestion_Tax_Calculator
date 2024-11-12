using Congestion.Calculator.Model.Calendar;
using System.ComponentModel.DataAnnotations;

namespace Congestion.Calculator.Model
{
    public class City
    {
        public int CityId { get; set; }

        [Required(ErrorMessage = "City name is required")]
        [StringLength(100)]
        public string? Name { get; set; }

        // List of holiday months specific to this city (one-to-many)
        public List<HolidayMonths>? HolidayMonths { get; set; }  // Corrected to List<HolidayMonths>

        // List of holidays in this city (one-to-many)
        public List<HolidayCalendar>? HolidayCalendars { get; set; } // Corrected to List<HolidayCalendar>

        // List of working calendars for the city (one-to-many)
        public List<WorkingCalendar>? WorkingCalendars { get; set; } // Corrected to List<WorkingCalendar>

        // List of tariffs for the city (one-to-many)
        public List<Tariff>? Tariffs { get; set; }

        // List of vehicles that are tax exempt (one-to-many)
        public List<Vehicle>? TaxExemptVehicles { get; set; }

        // City preference (one-to-one)
        public CityPreference? CityPreference { get; set; }
    }

}