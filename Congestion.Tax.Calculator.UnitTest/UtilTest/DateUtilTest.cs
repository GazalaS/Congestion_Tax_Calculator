using Congestion.Calculator.Model;
using Congestion.Calculator.Model.Calendar;
using Congestion.Calculator.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Congestion.Calculator.Tests
{
    [TestFixture]
    public class DateUtilTest
    {
        private List<WorkingCalendar> _workingCalendars;
        private List<HolidayMonths> _holidayMonths;
        private List<HolidayCalendar> _holidayCalendars;
        private CityPreference _cityPreference;

        [SetUp]
        public void Setup()
        {
            // Arrange: Setup working calendars with configurations for weekends and holidays
            _workingCalendars = new List<WorkingCalendar>
            {
                new WorkingCalendar
                {
                    Monday = WorkingDayType.Normal,
                    Tuesday = WorkingDayType.Normal,
                    Wednesday = WorkingDayType.Normal,
                    Thursday = WorkingDayType.Normal,
                    Friday = WorkingDayType.Normal,
                    Saturday = WorkingDayType.Weekend,
                    Sunday = WorkingDayType.Weekend
                }
            };

            _holidayMonths = new List<HolidayMonths>
            {
                new HolidayMonths { ActiveHolidayMonths = new List<Month> { Month.July, Month.December } }
            };

            _holidayCalendars = new List<HolidayCalendar>
            {
                new HolidayCalendar { StartDate = new DateTime(2023, 12, 24), EndDate = new DateTime(2023, 12, 26) },
                new HolidayCalendar { StartDate = new DateTime(2023, 1, 1), EndDate = new DateTime(2023, 1, 1) }
            };

            _cityPreference = new CityPreference
            {
                NumberOfTaxFreeDaysAfterHoliday = 2,
                NumberOfTaxFreeDaysBeforeHoliday = 1,
                MaxTaxPerDay = 60,
                SingleChargeIntervalInMin = 60
            };
        }

        [Test]
        public void IsWeekend_WhenDateIsWeekend_ShouldReturnTrue()
        {
            // Arrange
            var date = new DateTime(2023, 7, 8); // Saturday

            // Act
            var isWeekend = DateUtil.IsWeekend(_workingCalendars, date);

            // Assert
            Assert.That(isWeekend, Is.True);
        }

        [Test]
        public void IsWeekend_WhenDateIsWeekday_ShouldReturnFalse()
        {
            // Arrange
            var date = new DateTime(2023, 7, 5); // Wednesday

            // Act
            var isWeekend = DateUtil.IsWeekend(_workingCalendars, date);

            // Assert
            Assert.That(isWeekend, Is.False);
        }

        [TestCase(7, ExpectedResult = true)]
        [TestCase(12, ExpectedResult = true)]
        [TestCase(6, ExpectedResult = false)]
        public bool IsHolidayMonth_ShouldReturnExpectedResult(int month)
        {
            // Act & Assert
            return DateUtil.IsHolidayMonth(_holidayMonths, new DateTime(2023, month, 1));
        }

        [Test]
        public void IsPerOrPostOrInPublicHoliday_WhenDateIsDuringHoliday_ShouldReturnTrue()
        {
            // Arrange
            var date = new DateTime(2023, 12, 25); // Christmas Day

            // Act
            var isHoliday = DateUtil.IsPerOrPostOrInPublicHoliday(date, _holidayCalendars, _cityPreference);

            // Assert
            Assert.That(isHoliday, Is.True);
        }

        [Test]
        public void IsPerOrPostOrInPublicHoliday_WhenDateIsPreTaxFreeDay_ShouldReturnTrue()
        {
            // Arrange
            var date = new DateTime(2023, 12, 23); // 1 day before holiday start

            // Act
            var isHoliday = DateUtil.IsPerOrPostOrInPublicHoliday(date, _holidayCalendars, _cityPreference);

            // Assert
            Assert.That(isHoliday, Is.True);
        }

        [Test]
        public void IsPerOrPostOrInPublicHoliday_WhenDateIsPostTaxFreeDay_ShouldReturnTrue()
        {
            // Arrange
            var date = new DateTime(2023, 12, 28); // 2 days after holiday end

            // Act
            var isHoliday = DateUtil.IsPerOrPostOrInPublicHoliday(date, _holidayCalendars, _cityPreference);

            // Assert
            Assert.That(isHoliday, Is.True);
        }

        [Test]
        public void IsPerOrPostOrInPublicHoliday_WhenDateIsOutsideHolidayPeriod_ShouldReturnFalse()
        {
            // Arrange
            var date = new DateTime(2023, 12, 29); // Outside of tax-free and holiday

            // Act
            var isHoliday = DateUtil.IsPerOrPostOrInPublicHoliday(date, _holidayCalendars, _cityPreference);

            // Assert
            Assert.That(isHoliday, Is.False);
        }

        [Test]
        public void IsDateInBetweenIncludingEndpoints_WhenDateIsBetween_ShouldReturnTrue()
        {
            // Arrange
            var start = new DateTime(2023, 12, 24);
            var end = new DateTime(2023, 12, 26);
            var date = new DateTime(2023, 12, 25);

            // Act
            var isBetween = DateUtil.IsDateInBetweenIncludingEndPoints(start, end, date);

            // Assert
            Assert.That(isBetween, Is.True);
        }

        [Test]
        public void IsDateInBetweenIncludingEndpoints_WhenDateIsNotBetween_ShouldReturnFalse()
        {
            // Arrange
            var start = new DateTime(2023, 12, 24);
            var end = new DateTime(2023, 12, 26);
            var date = new DateTime(2023, 12, 27);

            // Act
            var isBetween = DateUtil.IsDateInBetweenIncludingEndPoints(start, end, date);

            // Assert
            Assert.That(isBetween, Is.False);
        }

        [Test]
        public void RemoveTime_ShouldRemoveTimeFromDate()
        {
            // Arrange
            var date = new DateTime(2023, 12, 25, 15, 30, 45);
            var expected = "2023-12-25";

            // Act
            var result = DateUtil.RemoveTime(date);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
