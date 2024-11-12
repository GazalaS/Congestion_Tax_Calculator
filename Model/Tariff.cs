namespace Congestion.Calculator.Model
{
    public class Tariff
    {
        /// <summary>
        /// Unique identifier for the tariff.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Start time of the tariff.
        /// </summary>
        public TimeSpan FromTime { get; set; }

        /// <summary>
        /// End time of the tariff.
        /// </summary>
        public TimeSpan ToTime { get; set; }

        /// <summary>
        /// Charge amount for the specific tariff.
        /// </summary>
        public int Charge { get; set; }

        /// <summary>
        /// Optional description for the tariff.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Identifier for the city this tariff applies to.
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// Navigation property for the related city.
        /// </summary>
        public City? City { get; set; }
    }
}