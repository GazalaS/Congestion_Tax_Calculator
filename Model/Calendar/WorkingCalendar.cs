namespace Congestion.Calculator.Model.Calendar
{
    public enum WorkingDayType
    {
        Normal,   // A regular working day
        Holiday,  // A day considered a holiday
        Weekend,  // A weekend day
        Special   // A special day (e.g., for certain events or custom settings)
    }

    public class WorkingCalendar
    {
        public int Id { get; set; }

        // Working days represented as enums
        public WorkingDayType Monday { get; set; }
        public WorkingDayType Tuesday { get; set; }
        public WorkingDayType Wednesday { get; set; }
        public WorkingDayType Thursday { get; set; }
        public WorkingDayType Friday { get; set; }
        public WorkingDayType Saturday { get; set; }
        public WorkingDayType Sunday { get; set; }

        // City the working calendar belongs to
        public int CityId { get; set; }

        // Navigation property for related city
        public City? City { get; set; }
    }

}
