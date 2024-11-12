using Congestion.Calculator.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Congestion.Calculator.Util
{
    public static class CalculationUtil
    {
        /// <summary>
        /// Gets the toll fee based on the tariffs and the given date.
        /// </summary>
        public static int GetTollFeeByTariffAndDate(DateTime date, List<Tariff> tariffs)
        {
            if (tariffs == null || !tariffs.Any()) return 0;

            // Get the matching tariff for the given time of day
            foreach (var tariff in tariffs)
            {
                var fromTime = tariff.FromTime;
                var toTime = tariff.ToTime;
                var sourceTime = date.TimeOfDay;

                if (sourceTime >= fromTime && sourceTime <= toTime)
                {
                    return tariff.Charge;
                }
            }

            return 0;
        }

        /// <summary>
        /// Checks if the given date is a toll-free date based on various conditions such as weekends, holidays, etc.
        /// </summary>
        public static bool IsTollFreeDate(DateTime date, City city)
        {
            return (city.WorkingCalendars != null && DateUtil.IsWeekend(city.WorkingCalendars, date)) ||
                   (city.HolidayMonths != null && DateUtil.IsHolidayMonth(city.HolidayMonths, date)) ||
                   (city.HolidayCalendars != null && DateUtil.IsPerOrPostOrInPublicHoliday(date, city.HolidayCalendars, city.CityPreference));
        }


        /// <summary>
        /// Gets the single charge rule based on a list of dates and the city's tariffs.
        /// </summary>
        public static Dictionary<string, List<int>> GetSingleChargeRule(List<DateTime> dates, City city)
        {
            var visitedSlots = new HashSet<DateTime>();
            var result = new Dictionary<string, List<int>>();

            for (int start = 0; start < dates.Count; start++)
            {
                if (visitedSlots.Contains(dates[start])) continue;

                var charge = GetTollFeeByTariffAndDate(dates[start], city.Tariffs ?? new List<Tariff>());
                for (int end = start + 1; end < dates.Count; end++)
                {
                    var timeDifference = (dates[end].TimeOfDay - dates[start].TimeOfDay).TotalMinutes;
                    if (timeDifference <= city.CityPreference.SingleChargeIntervalInMin)
                    {
                        visitedSlots.Add(dates[end]);
                        var tempCharge = GetTollFeeByTariffAndDate(dates[end], city.Tariffs);
                        charge = Math.Max(charge, tempCharge);
                    }
                    else
                    {
                        break;
                    }
                }

                AddChargeToResult(dates[start], charge, result);
            }

            return result;
        }

        /// <summary>
        /// Adds the toll charge to the result dictionary for the given date.
        /// </summary> city.CityPreference
        private static void AddChargeToResult(DateTime date, int charge, Dictionary<string, List<int>> result)
        {
            var dateKey = DateUtil.RemoveTime(date);
            if (!result.ContainsKey(dateKey))
            {
                result[dateKey] = new List<int>();
            }
            result[dateKey].Add(charge);
        }

        /// <summary>
        /// Calculates the total tax based on the single charge rule.
        /// </summary>
        public static int CalculateTotalTaxBySingleChargeRule(City city, Dictionary<string, List<int>> chargesPerDay)
        {
            var totalFee = 0;

            foreach (var dailyCharges in chargesPerDay.Values)
            {
                var totalChargePerDay = dailyCharges.Sum();
                if (city.CityPreference?.MaxTaxPerDay > 0 && totalChargePerDay > city.CityPreference.MaxTaxPerDay)
                {
                    totalChargePerDay = city.CityPreference.MaxTaxPerDay;
                }
                totalFee += totalChargePerDay;
            }

            return totalFee;
        }
    }
}
