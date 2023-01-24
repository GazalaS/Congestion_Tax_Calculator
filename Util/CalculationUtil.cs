using Congestion.Calculator.Model;

namespace Congestion.Calculator.Util
{
    public static class CalculationUtil
    {
        public static int GetTollFeeByTariffAndDate(DateTime date, List<Tariff> tariffs)
        {
            var totalFee = 0;
            if (tariffs == null || tariffs.Count == 0) return totalFee;

            foreach (Tariff tariff in tariffs)
            {
                TimeSpan fromTime = tariff.FromTime.ToTimeSpan();
                TimeSpan toTime = tariff.ToTime.ToTimeSpan();
                TimeSpan source = date.TimeOfDay;

                if ((source >= fromTime) && (source <= toTime))
                {
                    return totalFee += tariff.Charge;
                }
            }

            return totalFee;
        }

        public static bool IsTollFreeDate(DateTime date, City city)
        {
            if (DateUtil.IsWeekend(city.WorkingCalendar!, date)) return true;
            if (DateUtil.IsHolidayMonth(city.HolidayMonths!, date.Month)) return true;
            if (DateUtil.IsPerOrPostOrInPublicHoliday(date, city.HolidayCalendar, city.CityPreference)) return true;

            return false;
        }

        public static Dictionary<string, List<int>> GetSingleChargeRule(List<DateTime> dates, City city)
        {
            var visitedSlots = new List<DateTime>();
            var result = new Dictionary<string, List<int>>();
            for (int start = 0; start < dates.Count(); start++)
            {
                if (visitedSlots.Contains(dates[start])) continue;
                int charge = GetTollFeeByTariffAndDate(dates[start], city.Tariff!);
                for (int end = start + 1; end < dates.Count(); end++)
                {
                    var diffInMinutes = (dates[end].TimeOfDay - dates[start].TimeOfDay).TotalMinutes;
                    if (diffInMinutes <= city.CityPreference!.SingleChargeIntervalInMin)
                    {
                        visitedSlots.Add(dates[end]);
                        var temp = GetTollFeeByTariffAndDate(dates[end], city.Tariff!);
                        if (temp > charge) charge = temp;
                    }
                    else break;
                }
                ConstructChargesByDate(dates, result, start, charge);
            }
            return result;
        }

        public static void ConstructChargesByDate(List<DateTime> dates, Dictionary<string, List<int>> result, int start, int charge)
        {
            string dateString = DateUtil.RemoveTime(dates[start]);
            List<int> chargeLists;
            if (result.ContainsKey(dateString))
            {
                chargeLists = result[dateString];
            }
            else
            {
                chargeLists = new List<int>();
            }
            chargeLists.Add(charge);
            result.TryAdd(dateString, chargeLists);
        }

        public static int CalculateTotalTaxBySingleChargeRule(City city, Dictionary<string, List<int>> chargesPerDay)
        {
            var totalFee = 0;
            foreach (var entry in chargesPerDay)
            {
                int totalChargePerDay = entry.Value.Sum();
                if (city.CityPreference != null &&
                    city.CityPreference.MaxTaxPerDay != 0 &&
                    totalChargePerDay > city.CityPreference.MaxTaxPerDay)
                    totalChargePerDay = city.CityPreference.MaxTaxPerDay;
                totalFee += totalChargePerDay;
            }
            return totalFee;
        }
    }
}
