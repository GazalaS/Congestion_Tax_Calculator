using Congestion.Calculator.Model;
using Congestion.Calculator.Model.Calendar;
using Congestion.Calculator.Util;

namespace Congestion.Calculator.UnitTest.UtilTest
{
    public class CalculationUtilTest
    {
        private City _city;
        private List<DateTime> _dates;

        [SetUp]
        public void Setup()
        {
            _city = new City
            {
                Name = "Gothenburg",
                HolidayCalendar = new List<HolidayCalendar>
                            {
                                new HolidayCalendar { Date = new DateTime(2013, 01, 01)},
                                new HolidayCalendar { Date = new DateTime(2013, 04, 01)},
                                new HolidayCalendar { Date = new DateTime(2013, 05, 01)},
                                new HolidayCalendar { Date = new DateTime(2013, 06, 22)},
                                new HolidayCalendar { Date = new DateTime(2013, 11, 01)},
                                new HolidayCalendar { Date = new DateTime(2013, 12, 25)},
                            },
                WorkingCalendar = new WorkingCalendar
                            {
                                IsMonday = true,
                                IsTuesday = true,
                                IsWednesday = true,
                                IsThursday = true,
                                IsFriday = true,
                                IsSaturday = false,
                                IsSunday = false
                            },
                HolidayMonths = new HolidayMonths
                            {
                                IsJanuary = false,
                                IsFebruary = false,
                                IsMarch = false,
                                IsApril = false,
                                IsMay = false,
                                IsJune = false,
                                IsJuly = true,
                                IsAugust = false,
                                IsSeptember = false,
                                IsOctober = false,
                                IsDecember = false
                            },
                Tariff = new List<Tariff>
                            {
                                new Tariff{ FromTime = new TimeOnly(6, 0), ToTime = new TimeOnly(6, 29), Charge = 8},
                                new Tariff{ FromTime = new TimeOnly(6, 30), ToTime = new TimeOnly(6, 59), Charge = 13},
                                new Tariff{ FromTime = new TimeOnly(7, 0), ToTime = new TimeOnly(7, 59), Charge = 18},
                                new Tariff{ FromTime = new TimeOnly(8, 0), ToTime = new TimeOnly(8, 29), Charge = 13},
                                new Tariff{ FromTime = new TimeOnly(8, 30), ToTime = new TimeOnly(14, 59), Charge = 8},
                                new Tariff{ FromTime = new TimeOnly(15, 0), ToTime = new TimeOnly(15, 29), Charge = 13},
                                new Tariff{ FromTime = new TimeOnly(15, 30), ToTime = new TimeOnly(16, 59), Charge = 18},
                                new Tariff{ FromTime = new TimeOnly(17, 0), ToTime = new TimeOnly(17, 59), Charge = 13},
                                new Tariff{ FromTime = new TimeOnly(18, 0), ToTime = new TimeOnly(18, 29), Charge = 8},
                                new Tariff{ FromTime = new TimeOnly(18, 30), ToTime = new TimeOnly(05, 59), Charge = 0},
                            },
                TaxExemptVehicles = new List<Vehicle>
                            {
                                new Vehicle{ Name = "Emergency"},
                                new Vehicle{ Name = "Bus"},
                                new Vehicle{ Name = "Diplomat"},
                                new Vehicle{ Name = "Motorcycle"},
                                new Vehicle{ Name = "Military"},
                                new Vehicle{ Name = "Foreign"},
                            },
                CityPreference = new CityPreference
                            {
                                NumberOfTaxFreeDaysAfterHoliday = 0,
                                NumberOfTaxFreeDaysBeforeHoliday = 1,
                                MaxTaxPerDay = 60,
                                SingleChargeIntervalInMin = 60,
                            },
            };
            _dates = new List<DateTime>()
             {
                new DateTime(2013, 02, 08, 06, 27, 00),
                new DateTime(2013, 02, 08, 06, 20, 27),
                new DateTime(2013, 02, 08, 14, 35, 00),
                new DateTime(2013, 02, 08, 15, 29, 00),
                new DateTime(2013, 02, 08, 15, 47, 00),
                new DateTime(2013, 02, 08, 16, 01, 00),
                new DateTime(2013, 02, 08, 16, 48, 00),
                new DateTime(2013, 02, 08, 17, 49, 00),
                new DateTime(2013, 02, 08, 18, 29, 00),
                new DateTime(2013, 02, 08, 18, 35, 00),

             };
        }

        [Test]
        public void when_date_is_weekend_isToolFreeDate()
        {
            //Arrange
            var date = new DateTime(2013, 01, 06);

            //Act
            var actual = CalculationUtil.IsTollFreeDate(date, _city);

            //Assert
            Assert.That(actual, Is.EqualTo(true));
        }

        [Test]
        public void when_month_is_holiday_month_isToolFreeDate()
        {
            //Arrange
            var date = new DateTime(2013, 07, 08);

            //Act
            var actual = CalculationUtil.IsTollFreeDate(date, _city);

            //Assert
            Assert.That(actual, Is.EqualTo(true));
        }

        [Test]
        public void when_dates_is_public_holiday_isToolFreeDate()
        {
            //Arrange
            var date = new DateTime(2013, 01, 01);

            //Act
            var actual = CalculationUtil.IsTollFreeDate(date, _city);

            //Assert
            Assert.That(actual, Is.EqualTo(true));
        }

        [Test]
        public void when_dates_is_pre_public_holiday_isToolFreeDate()
        {
            //Arrange
            var date = new DateTime(2012, 12, 31);

            //Act
            var actual = CalculationUtil.IsTollFreeDate(date, _city);

            //Assert
            Assert.That(actual, Is.EqualTo(true));
        }

        [Test]
        public void when_dates_is_post_public_holiday_not_tool_free()
        {
            //Arrange
            var date = new DateTime(2013, 01, 02);

            //Act
            var actual = CalculationUtil.IsTollFreeDate(date, _city);

            //Assert
            Assert.That(actual, Is.EqualTo(false));
        }

        [Test]
        public void when_getTollFeeByTariffAndDate_calculate_toll()
        {
            //Arrange
            var expected = _city.Tariff[0]!.Charge;
            var date = new DateTime(2013, 02, 08, 06, 27, 00);

            //Act
            var actual = CalculationUtil.GetTollFeeByTariffAndDate(date, _city.Tariff!);

            //Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void when_ConstructChargesByDate_calculate_toll_as_per_single_charge()
        {
            //Arrange
            var key = "2013-02-08";
            var result = new Dictionary<string, List<int>>();
            //Assert
            Assert.That(result.Count, Is.EqualTo(0));
            
            //Act
            CalculationUtil.ConstructChargesByDate(_dates, result, 0, 8);
            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.ContainsKey(key), Is.EqualTo(true));
            Assert.That(result[key].Count, Is.EqualTo(1));
            
            //Act
            CalculationUtil.ConstructChargesByDate(_dates, result, 1, 13);
            //Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result.ContainsKey(key), Is.EqualTo(true));
            Assert.That(result[key].Count, Is.EqualTo(2));
        }

        [Test]
        public void when_getSingleChargeRules_calculate_toll_as_per_single_charge()
        {
            //Arrange
            var key = "2013-02-08";
            //Act
            var actual = CalculationUtil.GetSingleChargeRule(_dates, _city);
            //Assert
            Assert.IsNotNull(actual);
            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual.ContainsKey(key), Is.EqualTo(true));
            Assert.That(actual[key].Count, Is.EqualTo(5));
        }

        [Test]
        public void when_total_tax_is_more_than_Max_amount_per_day_returns_max_amount_per_day()
        {
            //Arrange
            var expected = _city.CityPreference!.MaxTaxPerDay;
            Dictionary<String, List<int>> chargesPerDay = CalculationUtil.GetSingleChargeRule(_dates, _city);

            //Act
            var actual = CalculationUtil.CalculateTotalTaxBySingleChargeRule(_city, chargesPerDay);

            //Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void when_total_tax_is_less_than_Max_amount_per_day_returns_total_amount_per_day()
        {
            //Arrange
            _dates = new List<DateTime>()
             {
                new DateTime(2013, 02, 07, 06, 23,27),
                new DateTime(2013, 02, 07, 15, 27, 00),
              };

            var expected = 21;
            Dictionary<String, List<int>> chargesPerDay = CalculationUtil.GetSingleChargeRule(_dates, _city);

            //Act
            var actual = CalculationUtil.CalculateTotalTaxBySingleChargeRule(_city, chargesPerDay);

            //Assert
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
