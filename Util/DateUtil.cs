using Congestion.Calculator.Model;
using Congestion.Calculator.Model.Calendar;


namespace Congestion.Calculator.Util
{
    public static class DateUtil
    {
        /// <summary>
        /// Checks if the given date is a weekend based on the city's working calendars.
        /// </summary>
        public static bool IsWeekend(List<WorkingCalendar> workingCalendars, DateTime date)
            => workingCalendars.Any(wc => GetWorkingDayType(wc, date) == WorkingDayType.Weekend);

        /// <summary>
        /// Gets the working day type (Normal, Holiday, Weekend, Special) for a given date.
        /// </summary>
        private static WorkingDayType GetWorkingDayType(WorkingCalendar workingCalendar, DateTime date)
            => date.DayOfWeek switch
            {
                DayOfWeek.Monday => workingCalendar.Monday,
                DayOfWeek.Tuesday => workingCalendar.Tuesday,
                DayOfWeek.Wednesday => workingCalendar.Wednesday,
                DayOfWeek.Thursday => workingCalendar.Thursday,
                DayOfWeek.Friday => workingCalendar.Friday,
                DayOfWeek.Saturday => workingCalendar.Saturday,
                DayOfWeek.Sunday => workingCalendar.Sunday,
                _ => WorkingDayType.Normal,
            };

        /// <summary>
        /// Checks if the given date is a holiday month based on the city's holiday months.
        /// </summary>
        public static bool IsHolidayMonth(List<HolidayMonths> holidayMonthsList, DateTime date)
            => holidayMonthsList?.Any(hm => hm?.ActiveHolidayMonths?.Contains((Month)date.Month) ?? false) ?? false;


        /// <summary>
        /// Checks if the given date is within a public holiday period (pre, post, or in the holiday range) based on the city's holiday calendar and preferences.
        /// </summary>
        public static bool IsPerOrPostOrInPublicHoliday(DateTime date, List<HolidayCalendar> holidayCalendars, CityPreference? cityPreference)
        {
            if (cityPreference == null) return false;

            return holidayCalendars.Any(holiday =>
                holiday.StartDate <= date && holiday.EndDate >= date ||  // Check if within the holiday period
                holiday.StartDate.AddDays(-cityPreference.NumberOfTaxFreeDaysBeforeHoliday) <= date && holiday.StartDate >= date ||  // Before holiday tax-free
                holiday.EndDate.AddDays(cityPreference.NumberOfTaxFreeDaysAfterHoliday) >= date && holiday.EndDate <= date);  // After holiday tax-free
        }


        /// <summary>
        /// Removes the time portion of a DateTime (used for comparing dates only).
        /// </summary>
        public static string RemoveTime(DateTime date) => date.ToString("yyyy-MM-dd");

        /// <summary>
        /// Checks if a date is within a specified date range, including the start and end dates.
        /// </summary>
        /// <param name="startDate">The start of the date range.</param>
        /// <param name="endDate">The end of the date range.</param>
        /// <param name="date">The date to check.</param>
        /// <returns>True if the date is within the range (inclusive), otherwise false.</returns>
        public static bool IsDateInBetweenIncludingEndPoints(DateTime startDate, DateTime endDate, DateTime date)
        {
            return date >= startDate && date <= endDate;
        }
    }

}
