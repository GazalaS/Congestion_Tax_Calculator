using Congestion.Calculator.Model;
using Congestion.Calculator.Repository;
using Congestion.Calculator.Service;
using Moq;

namespace Congestion.Calculator.UnitTest.ServiceTest
{
    public class CongestionTaxCalculatorServiceTest
    {
        private Mock<ICongestionTaxCalculatorService> _congestionTaxCalculatorServiceMoq;
        private Mock<ICityRepository> _cityRepositoryMoq;
        private Mock<IVehicleRepository> _vehicleRepositoryMoq;

        [SetUp]
        public void Setup()
        {
            _cityRepositoryMoq = new Mock<ICityRepository>();
            _vehicleRepositoryMoq = new Mock<IVehicleRepository>();
            _congestionTaxCalculatorServiceMoq = new Mock<ICongestionTaxCalculatorService>();
        }

        [Test]
        public void when_get_tax_called_with_empty_input_then_return_result()
        {
            var vehicle = new Mock<Vehicle>();
            var dates = new Mock<List<DateTime>>();
            _congestionTaxCalculatorServiceMoq.Setup(x => x.GetTax(vehicle.Object, dates.Object, "")).Returns(Task.FromResult(0));
            var actual = _congestionTaxCalculatorServiceMoq.Object.GetTax(vehicle.Object, dates.Object, "");

            Assert.That(0, Is.EqualTo(actual.Result));
            _congestionTaxCalculatorServiceMoq.Verify(x => x.GetTax(vehicle.Object, dates.Object, ""), Times.Once);
            _congestionTaxCalculatorServiceMoq.VerifyNoOtherCalls();
        }


        [Test]
        public void when_get_tax_called_with_valid_input_then_return_result()
        {
            var vehicle = new Mock<Vehicle>();
            var dates = new Mock<List<DateTime>>();
            _congestionTaxCalculatorServiceMoq.Setup(x => x.GetTax(vehicle.Object, dates.Object, "Gothenburg")).Returns(Task.FromResult(10));
            var expected = _congestionTaxCalculatorServiceMoq.Object.GetTax(vehicle.Object, dates.Object, "Gothenburg");

            Assert.That(10, Is.EqualTo(expected.Result));
            _congestionTaxCalculatorServiceMoq.Verify(x => x.GetTax(vehicle.Object, dates.Object, "Gothenburg"), Times.Once);
            _congestionTaxCalculatorServiceMoq.VerifyNoOtherCalls();
        }



    }
}

