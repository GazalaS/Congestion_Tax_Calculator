using Congestion.Calculator.Controllers;
using Congestion.Calculator.Model;
using Congestion.Calculator.Repository;
using Congestion.Calculator.Service;
using Microsoft.Extensions.Logging;
using Moq;

namespace Congestion.Calculator.UnitTest.ControllerTest
{
    public class CongestionControllerTest
    {
        private Mock<ICongestionTaxCalculatorService> _congestionTaxCalculatorServiceMoq;
        private Mock<ICityRepository> _cityRepositoryMoq;
        private Mock<IVehicleRepository> _vehicleRepositoryMoq;
        private readonly ILogger<CongestionTaxCalculatorController> _logger;

        [SetUp]
        public void Setup()
        {
            //Arrange
            _cityRepositoryMoq = new Mock<ICityRepository>();
            _vehicleRepositoryMoq = new Mock<IVehicleRepository>();
            _congestionTaxCalculatorServiceMoq = new Mock<ICongestionTaxCalculatorService>();
        }

        [Test]
        public void When_CalculateTaxWithValidInput_Then_ReturnResult()
        {
            //Arrange
            var vehicle = new Mock<Vehicle>();
            var dates = new Mock<List<DateTime>>();

            var controller = new CongestionTaxCalculatorController(_logger, _congestionTaxCalculatorServiceMoq.Object);
            //Act
            var actual = controller.Get(vehicle.Object, dates.Object, "");

            //Assert
            Assert.IsNotNull(actual);

            _congestionTaxCalculatorServiceMoq.Verify(x => x.GetTax(vehicle.Object, dates.Object, ""), Times.Once);
            _congestionTaxCalculatorServiceMoq.VerifyNoOtherCalls();
        }
    }
}