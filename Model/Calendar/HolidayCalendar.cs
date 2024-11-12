namespace Congestion.Calculator.Model.Calendar
{
    public enum HolidayType
    {
        Public,
        Regional,
        Religious,
        Special
    }

    public class HolidayCalendar
    {
        public int Id { get; set; }

        // Start date of the holiday (if it spans multiple days)
        public DateTime StartDate { get; set; }

        // End date of the holiday (if it spans multiple days)
        public DateTime EndDate { get; set; }

        // Type of holiday (public, regional, etc.)
        public HolidayType Type { get; set; }

        // Optional description of the holiday
        public string? Description { get; set; }

        // City the holiday belongs to
        public int CityId { get; set; }

        // Navigation property for related city
        public City? City { get; set; }
    }
}
