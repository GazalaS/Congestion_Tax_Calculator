using Congestion.Calculator.Model;
using Congestion.Calculator.Repository;
using Moq;

namespace Congestion.Calculator.UnitTest.RepositoryTest
{
    [TestFixture]
    public class VehicleRepositoryTest
    {
        private Mock<IVehicleRepository> _vehicleRepositoryMock;
        private List<Vehicle> _taxExemptVehicles;
        private static readonly Vehicle EmergencyVehicle = new Vehicle { Name = "Emergency" };
        private static readonly Vehicle OtherVehicle = new Vehicle { Name = "Other" };

        [SetUp]
        public void Setup()
        {
            // Arrange
            _vehicleRepositoryMock = new Mock<IVehicleRepository>();

            _vehicleRepositoryMock
                .Setup(r => r.GetVehicleTypeAsync("Emergency"))
                .ReturnsAsync(new Vehicle
                {
                    VehicleId = 1,
                    Name = "Emergency"
                });

            _vehicleRepositoryMock
                .Setup(r => r.isValidVehicleAsync(EmergencyVehicle))
                .ReturnsAsync(true);

            _vehicleRepositoryMock
                .Setup(r => r.isValidVehicleAsync(OtherVehicle))
                .ReturnsAsync(false);

            _taxExemptVehicles = new List<Vehicle>
            {
                new Vehicle { Name = "Emergency" },
                new Vehicle { Name = "Bus" },
                new Vehicle { Name = "Diplomat" },
                new Vehicle { Name = "Motorcycle" },
                new Vehicle { Name = "Military" },
                new Vehicle { Name = "Foreign" },
            };

            _vehicleRepositoryMock
               .Setup(r => r.IsTollFreeVehicleAsync(_taxExemptVehicles, EmergencyVehicle))
               .ReturnsAsync(true);

            _vehicleRepositoryMock
               .Setup(r => r.IsTollFreeVehicleAsync(_taxExemptVehicles, OtherVehicle))
               .ReturnsAsync(false);
        }

        [TestCase("Emergency")]
        public async Task When_VehicleName_Exists_Then_Return_Vehicle(string vehicleName)
        {
            // Act
            var actual = await _vehicleRepositoryMock.Object.GetVehicleTypeAsync(vehicleName);

            // Assert
            Assert.That(actual, Is.Not.Null, "Expected a valid vehicle but got null.");
            Assert.That(actual, Is.InstanceOf<Vehicle>(), "Expected result to be of type Vehicle.");
            Assert.That(actual.Name, Is.EqualTo(vehicleName), $"Expected vehicle name '{vehicleName}' but got '{actual.Name}'.");

            _vehicleRepositoryMock.Verify(x => x.GetVehicleTypeAsync(vehicleName), Times.Once);
        }

        [TestCase("Subway")]
        [TestCase("")]
        [TestCase(" ")]
        public async Task When_VehicleName_DoesNot_Exist_Then_Return_Null(string vehicleName)
        {
            // Act
            var actual = await _vehicleRepositoryMock.Object.GetVehicleTypeAsync(vehicleName);

            // Assert
            Assert.That(actual, Is.Null, $"Expected null for vehicle name '{vehicleName}' but got a non-null value.");
            _vehicleRepositoryMock.Verify(x => x.GetVehicleTypeAsync(vehicleName), Times.Once);
        }

        [Test]
        public async Task When_Vehicle_Is_Valid_Then_Return_True()
        {
            // Act
            var actual = await _vehicleRepositoryMock.Object.isValidVehicleAsync(EmergencyVehicle);

            // Assert
            Assert.That(actual, Is.True, "Expected true for valid vehicle but got false.");
            _vehicleRepositoryMock.Verify(x => x.isValidVehicleAsync(EmergencyVehicle), Times.Once);
        }

        [Test]
        public async Task When_Vehicle_Is_Invalid_Then_Return_False()
        {
            // Act
            var actual = await _vehicleRepositoryMock.Object.isValidVehicleAsync(OtherVehicle);

            // Assert
            Assert.That(actual, Is.False, "Expected false for invalid vehicle but got true.");
            _vehicleRepositoryMock.Verify(x => x.isValidVehicleAsync(OtherVehicle), Times.Once);
        }

        [Test]
        public async Task When_Vehicle_Is_TollFree_Then_Return_True()
        {
            // Act
            var actual = await _vehicleRepositoryMock.Object.IsTollFreeVehicleAsync(_taxExemptVehicles, EmergencyVehicle);

            // Assert
            Assert.That(actual, Is.True, "Expected true for toll-free vehicle but got false.");
            _vehicleRepositoryMock.Verify(x => x.IsTollFreeVehicleAsync(_taxExemptVehicles, EmergencyVehicle), Times.Once);
        }

        [Test]
        public async Task When_Vehicle_Is_Not_TollFree_Then_Return_False()
        {
            // Act
            var actual = await _vehicleRepositoryMock.Object.IsTollFreeVehicleAsync(_taxExemptVehicles, OtherVehicle);

            // Assert
            Assert.That(actual, Is.False, "Expected false for non-toll-free vehicle but got true.");
            _vehicleRepositoryMock.Verify(x => x.IsTollFreeVehicleAsync(_taxExemptVehicles, OtherVehicle), Times.Once);
        }
    }
}
