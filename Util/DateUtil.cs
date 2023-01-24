using Congestion.Calculator.Model;
using Congestion.Calculator.Model.Calendar;

namespace Congestion.Calculator.Util
{
    public static class DateUtil
    {

        public static readonly string dateFormat = "yyyy-MM-dd";
        public static readonly string dateAndTimeFormat = "yyyy-MM-dd HH:mm:ss";

        public static bool IsWeekend(WorkingCalendar workingCalendar, DateTime date)
        {
            if (workingCalendar == null) return false;

            if (workingCalendar.IsMonday == false && date.DayOfWeek == DayOfWeek.Monday) return true;
            if (workingCalendar.IsTuesday == false && date.DayOfWeek == DayOfWeek.Tuesday) return true;
            if (workingCalendar.IsWednesday == false && date.DayOfWeek == DayOfWeek.Wednesday) return true;
            if (workingCalendar.IsThursday == false && date.DayOfWeek == DayOfWeek.Thursday) return true;
            if (workingCalendar.IsFriday == false && date.DayOfWeek == DayOfWeek.Friday) return true;
            if (workingCalendar.IsSaturday == false && date.DayOfWeek == DayOfWeek.Saturday) return true;
            if (workingCalendar.IsSunday == false && date.DayOfWeek == DayOfWeek.Sunday) return true;

            return false;
        }

        public static bool IsHolidayMonth(HolidayMonths holidayMonths, int month)
        {
            if (holidayMonths == null) return false;

            if (holidayMonths.IsJanuary == true && month == 1) return true;
            if (holidayMonths.IsFebruary == true && month == 2) return true;
            if (holidayMonths.IsMarch == true && month == 3) return true;
            if (holidayMonths.IsApril == true && month == 4) return true;
            if (holidayMonths.IsMay == true && month == 5) return true;
            if (holidayMonths.IsJune == true && month == 6) return true;
            if (holidayMonths.IsJuly == true && month == 7) return true;
            if (holidayMonths.IsAugust == true && month == 8) return true;
            if (holidayMonths.IsSeptember == true && month == 9) return true;
            if (holidayMonths.IsOctober == true && month == 10) return true;
            if (holidayMonths.IsNovember == true && month == 11) return true;
            if (holidayMonths.IsDecember == true && month == 12) return true;

            return false;
        }

        public static bool IsPerOrPostOrInPublicHoliday(DateTime date, List<HolidayCalendar> holidayCalendar, CityPreference cityPreference)
        {
            if (holidayCalendar == null || holidayCalendar.Count == 0) return false;

            foreach (var holiday in holidayCalendar)
            {
                if (date.CompareTo(holiday.Date) == 0) return true;
                if (IsDateInBetweenIncludingEndPoints(holiday.Date, holiday.Date.AddDays(cityPreference!.NumberOfTaxFreeDaysAfterHoliday), date.Date)) return true;
                if (IsDateInBetweenIncludingEndPoints(holiday.Date.AddDays(-cityPreference!.NumberOfTaxFreeDaysBeforeHoliday), holiday.Date, date.Date)) return true;
            }

            return false;
        }

        public static bool IsDateInBetweenIncludingEndPoints(DateTime min, DateTime max, DateTime date)
           => (date.CompareTo(min) >= 0 && date.CompareTo(max) <= 0);

        public static string RemoveTime(DateTime date)
            => date.ToString(dateFormat);
    }
}
