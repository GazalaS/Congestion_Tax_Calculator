namespace Congestion.Calculator.Model
{
    public class CityPreference
    {
        public int Id { get; set; }
        public int NumberOfTaxFreeDaysBeforeHoliday { get; set; }
        public int NumberOfTaxFreeDaysAfterHoliday { get; set; }
        public int MaxTaxPerDay { get; set; }
        public int SingleChargeIntervalInMin { get; set; }
        public int CityId { get; set; }
    }
}