using Congestion.Calculator.Model.Calendar;
using System.ComponentModel.DataAnnotations;

namespace Congestion.Calculator.Model
{
    public class City
    {
        public int CityId { get; set; }

        [Required(ErrorMessage = "Cityname is required")]
        public string? Name { get; set; }
        public List<HolidayCalendar>? HolidayCalendar { get; set; }
        public WorkingCalendar? WorkingCalendar { get; set; }
        public HolidayMonths? HolidayMonths { get; set; }
        public List<Tariff>? Tariff { get; set; }

        public List<Vehicle>? TaxExemptVehicles;

        public CityPreference? CityPreference;
    }


}
