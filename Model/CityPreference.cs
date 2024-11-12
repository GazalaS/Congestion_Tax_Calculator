namespace Congestion.Calculator.Model
{
    public class CityPreference
    {
        public int CityPreferenceId { get; set; }

        public int NumberOfTaxFreeDaysBeforeHoliday { get; set; }
        public int NumberOfTaxFreeDaysAfterHoliday { get; set; }
        public int MaxTaxPerDay { get; set; }
        public int SingleChargeIntervalInMin { get; set; }

        // City the preference belongs to
        public int CityId { get; set; }

        // Navigation property for related city
        public City? City { get; set; }
    }
}