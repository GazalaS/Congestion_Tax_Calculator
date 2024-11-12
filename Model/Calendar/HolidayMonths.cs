namespace Congestion.Calculator.Model.Calendar
{
    /// <summary>
    /// Represents the months in which holidays are observed for a specific city.
    /// </summary>
    public class HolidayMonths
    {
        /// <summary>
        /// Unique identifier for the holiday months record.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A collection of months that have holidays for the city.
        /// </summary>
        public List<Month> ActiveHolidayMonths { get; set; } = new List<Month>();

        /// <summary>
        /// The CityId that this holiday month setting belongs to.
        /// </summary>
        public int CityId { get; set; }

        // Navigation Property to City
        public City City { get; set; }
    }

    /// <summary>
    /// Enum for representing the months of the year.
    /// </summary>
    public enum Month
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }
}
