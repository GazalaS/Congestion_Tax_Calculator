using Congestion.Calculator.Model;
using Congestion.Calculator.Model.Calendar;
using Congestion.Calculator.Repository;
using Moq;

namespace Congestion.Calculator.UnitTest.RepositoryTest
{
    public class CityRepositoryTest
    {
        private Mock<ICityRepository> _cityRepositoryMoq;

        [SetUp]
        public void Setup()
        {
            _cityRepositoryMoq = new Mock<ICityRepository>();
            _cityRepositoryMoq
                .Setup(r => r.GetCity("Gothenburg"))
                .Returns(new City
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
                }
            );

            _cityRepositoryMoq
                .Setup(r => r.isValidCity("Gothenburg"))
                .Returns(true);

            _cityRepositoryMoq
                .Setup(r => r.isValidCity("Stockholm"))
                .Returns(false);
        }

        [TestCase("Gothenburg")]
        public void When_CitynameExist_Then_ReturnCity(string cityName)
        {
            //Act
            var actual = _cityRepositoryMoq.Object.GetCity(cityName);

            // Assert
            _cityRepositoryMoq.Verify(x => x.GetCity(cityName));
            Assert.IsInstanceOf<City>(actual);
            Assert.IsNotNull(actual);
            Assert.That(cityName, Is.EqualTo(actual.Name));
        }

        [TestCase("Stockholm")]
        [TestCase("")]
        [TestCase(" ")]
        public void When_CitynameDoesnotExist_Then_ReturnNull(string cityName)
        {
            //Act
            var actual = _cityRepositoryMoq.Object.GetCity(cityName);

            // Assert
            _cityRepositoryMoq.Verify(x => x.GetCity(cityName));
            Assert.IsNull(actual);
        }

        [TestCase("Gothenburg", ExpectedResult = true)]
        [TestCase("Stockholm", ExpectedResult = false)]
        public bool When_CitynameIsvalid_Then_Return_Bool(string cityName)
        {
            //Act
            return _cityRepositoryMoq.Object.isValidCity(cityName);
           
        }
    }
}
