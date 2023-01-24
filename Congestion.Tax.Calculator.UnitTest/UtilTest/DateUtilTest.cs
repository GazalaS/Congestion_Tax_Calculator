using Congestion.Calculator.Model;
using Congestion.Calculator.Model.Calendar;
using Congestion.Calculator.Util;

namespace Congestion.Calculator.UnitTest.UtilTest
{
    public class DateUtilTest
    {
        private WorkingCalendar _workingCalendar;
        private HolidayMonths _holidayMonths;
        private List<HolidayCalendar> _holidayCalendar;
        private CityPreference _cityPreference;

        [SetUp]
        public void Setup()
        {
            //Arrange
            _workingCalendar = new WorkingCalendar
            {
                IsMonday = true,
                IsTuesday = true,
                IsWednesday = true,
                IsThursday = true,
                IsFriday = true,
                IsSaturday = false,
                IsSunday = false
            };
            _holidayMonths = new HolidayMonths
            {
                IsJuly = true,
            };

            _holidayCalendar = new List<HolidayCalendar>
                            {
                                new HolidayCalendar { Date = new DateTime(2013, 01, 01)},
                                new HolidayCalendar { Date = new DateTime(2013, 12, 25)},
                            };

            _cityPreference = new CityPreference
            {
                NumberOfTaxFreeDaysAfterHoliday = 1,
                NumberOfTaxFreeDaysBeforeHoliday = 1,
                MaxTaxPerDay = 60,
                SingleChargeIntervalInMin = 60,
            };
        }

        [Test]
        public void when_date_isWeekend_then_return_true()
        {
            //Arrange
            var date = new DateTime(2013, 3, 24, 14, 25, 00);
            //Act
            var actual = DateUtil.IsWeekend(_workingCalendar, date);
            // Assert
            Assert.That(actual, Is.EqualTo(true));
        }

        [Test]
        public void when_date_isWeekday_then_return_false()
        {
            //Arrange
            var date = new DateTime(2013, 1, 14, 21, 00, 00);
            //Act
            var actual = DateUtil.IsWeekend(_workingCalendar, date);
            // Assert
            Assert.That(actual, Is.EqualTo(false));
        }

        [TestCase(1, ExpectedResult = false)]
        [TestCase(7, ExpectedResult = true)]
        public bool check_if_month_isHolidayMonth_then_return_expectedresult(int month)
        {
            return DateUtil.IsHolidayMonth(_holidayMonths, month);
        }

        [Test]
        public void when_date_isPerOrPostOrInPublicHoliday_then_return_true()
        {
            //Arrange
            var date = new DateTime(2013, 1, 2, 21, 00, 00);
            //Act
            var actual = DateUtil.IsPerOrPostOrInPublicHoliday(date, _holidayCalendar, _cityPreference);
            //Assert
            Assert.That(actual, Is.EqualTo(true));
        }

        [Test]
        public void when_date_isPerOrPostOrInPublicHoliday_then_return_false()
        {
            //Arrange
            var date = new DateTime(2013, 1, 3, 21, 00, 00);
            //Act
            var actual = DateUtil.IsPerOrPostOrInPublicHoliday(date, _holidayCalendar, _cityPreference);
            //Assert
            Assert.IsFalse(actual);
        }

        [Test]
        public void when_date_is_in_between_including_endPoints_return_true()
        {
            //Arrange
            var min = new DateTime(2013, 1, 1);
            var max = new DateTime(2013, 1, 3);
            var date = new DateTime(2013, 1, 2);
            //Act
            var actual = DateUtil.IsDateInBetweenIncludingEndPoints(min, max, date);

            //Assert
            Assert.IsTrue(actual);
        }

        [Test]
        public void when_date_is_not_in_between_including_endPoints_return_false()
        {
            //Arrange
            var min = new DateTime(2013, 1, 1);
            var max = new DateTime(2013, 1, 3);
            var date = new DateTime(2013, 1, 4);
            //Act
            var actual = DateUtil.IsDateInBetweenIncludingEndPoints(min, max, date);

            //Assert
            Assert.IsFalse(actual);
        }

        [Test]
        public void when_dateformat_datetime_remove_time_component()
        {
            var date = new DateTime(2013, 1, 3, 21, 00, 00);
            var expected = "2013-01-03";
            //Act
            var actual = DateUtil.RemoveTime(date).ToString();
            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
