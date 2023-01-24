namespace Congestion.Calculator.Model
{
    public class Tariff
    {
        public int Id { get; set; }
        public TimeOnly FromTime { get; set; }
        public TimeOnly ToTime { get; set; }
        public int Charge { get; set; }
        public int CityId { get; set; }
    }
}