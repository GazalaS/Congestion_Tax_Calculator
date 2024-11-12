using Congestion.Calculator.Model;
using Congestion.Calculator.Model.Calendar;
using Congestion.Calculator.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Congestion.Calculator.Tests
{
    [TestFixture]
    public class CalculationUtilTests
    {
        private City _city;

        [SetUp]
        public void Setup()
        {
            _city = new City
            {
                Tariffs = new List<Tariff>
                {
                    new Tariff { FromTime = TimeSpan.Parse("06:00"), ToTime = TimeSpan.Parse("06:59"), Charge = 8 },
                    new Tariff { FromTime = TimeSpan.Parse("07:00"), ToTime = TimeSpan.Parse("07:59"), Charge = 13 },
                    new Tariff { FromTime = TimeSpan.Parse("08:00"), ToTime = TimeSpan.Parse("08:29"), Charge = 18 }
                },
                WorkingCalendars = new List<WorkingCalendar>
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
                },
                HolidayMonths = new List<HolidayMonths>
                {
                    new HolidayMonths { ActiveHolidayMonths = new List<Month> { Month.July, Month.December } }
                },
                HolidayCalendars = new List<HolidayCalendar>
                {
                    new HolidayCalendar { StartDate = new DateTime(2023, 12, 24), EndDate = new DateTime(2023, 12, 26) }
                },
                CityPreference = new CityPreference
                {
                    NumberOfTaxFreeDaysBeforeHoliday = 1,
                    NumberOfTaxFreeDaysAfterHoliday = 2,
                    MaxTaxPerDay = 60,
                    SingleChargeIntervalInMin = 60
                }
            };
        }

        [Test]
        public void GetTollFeeByTariffAndDate_WhenWithinTariffTime_ShouldReturnCorrectCharge()
        {
            // Arrange
            var date = new DateTime(2023, 7, 5, 7, 15, 0); // 7:15 AM

            // Act
            var tollFee = CalculationUtil.GetTollFeeByTariffAndDate(date, _city.Tariffs ?? new List<Tariff>());

            // Assert
            Assert.That(tollFee, Is.EqualTo(13));
        }

        [Test]
        public void GetTollFeeByTariffAndDate_WhenOutsideTariffTime_ShouldReturnZero()
        {
            // Arrange
            var date = new DateTime(2023, 7, 5, 5, 30, 0); // 5:30 AM

            // Act
            var tollFee = CalculationUtil.GetTollFeeByTariffAndDate(date, _city.Tariffs ?? new List<Tariff>());

            // Assert
            Assert.That(tollFee, Is.EqualTo(0));
        }

        [Test]
        public void IsTollFreeDate_WhenDateIsWeekend_ShouldReturnTrue()
        {
            // Arrange
            var date = new DateTime(2023, 7, 8); // Saturday

            // Act
            var isTollFree = CalculationUtil.IsTollFreeDate(date, _city);

            // Assert
            Assert.That(isTollFree, Is.True);
        }

        [Test]
        public void IsTollFreeDate_WhenDateIsHoliday_ShouldReturnTrue()
        {
            // Arrange
            var date = new DateTime(2023, 12, 25); // Holiday in December

            // Act
            var isTollFree = CalculationUtil.IsTollFreeDate(date, _city);

            // Assert
            Assert.That(isTollFree, Is.True);
        }

        [Test]
        public void IsTollFreeDate_WhenDateIsRegularWorkingDay_ShouldReturnFalse()
        {
            // Arrange
            var date = new DateTime(2023, 7, 5); // Regular weekday
            var city = new City
            {
                WorkingCalendars = new List<WorkingCalendar>
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
                },
                HolidayMonths = new List<HolidayMonths>(),
                HolidayCalendars = new List<HolidayCalendar>(),
                CityPreference = new CityPreference
                {
                    NumberOfTaxFreeDaysBeforeHoliday = 0,
                    NumberOfTaxFreeDaysAfterHoliday = 0
                }
            };

            // Act
            var isTollFree = CalculationUtil.IsTollFreeDate(date, city);

            // Assert
            Assert.That(isTollFree, Is.False);
        }


        [Test]
        public void GetSingleChargeRule_WhenMultipleEntriesWithinInterval_ShouldApplySingleChargeRule()
        {
            // Arrange
            var dates = new List<DateTime>
            {
                new DateTime(2023, 7, 5, 7, 0, 0),
                new DateTime(2023, 7, 5, 7, 30, 0),
                new DateTime(2023, 7, 5, 8, 0, 0)
            };

            // Act
            var singleChargeRule = CalculationUtil.GetSingleChargeRule(dates, _city);

            // Assert
            Assert.That(singleChargeRule["2023-07-05"].Count, Is.EqualTo(1));
            Assert.That(singleChargeRule["2023-07-05"][0], Is.EqualTo(18)); // Maximum charge in interval
        }

        [Test]
        public void CalculateTotalTaxBySingleChargeRule_WhenExceedingDailyMaxTax_ShouldCapToMaxTax()
        {
            // Arrange
            var chargesPerDay = new Dictionary<string, List<int>>
            {
                { "2023-07-05", new List<int> { 18, 13, 8, 21 } } // Total exceeds daily max tax of 60
            };

            // Act
            var totalTax = CalculationUtil.CalculateTotalTaxBySingleChargeRule(_city, chargesPerDay);

            // Assert
            Assert.That(totalTax, Is.EqualTo(60)); // Capped to 60 as per city preference
        }

        [Test]
        public void CalculateTotalTaxBySingleChargeRule_WhenTotalIsWithinMaxTax_ShouldReturnActualTotal()
        {
            // Arrange
            var chargesPerDay = new Dictionary<string, List<int>>
            {
                { "2023-07-05", new List<int> { 8, 13 } } // Total within daily max tax
            };

            // Act
            var totalTax = CalculationUtil.CalculateTotalTaxBySingleChargeRule(_city, chargesPerDay);

            // Assert
            Assert.That(totalTax, Is.EqualTo(21)); // Total without capping
        }
    }
}
