using Congestion.Calculator.Model;
using Congestion.Calculator.Repository;
using Moq;

namespace Congestion.Calculator.UnitTest.RepositoryTest
{
    public class VehicleRepositoryTest
    {
        private Mock<IVehicleRepository> _vehicleRepositoryMoq;
        private List<Vehicle> _taxExemptVehicles;
        private static Vehicle _emergencyVehicle = new Vehicle() { Name = "Emergency" };
        private static Vehicle _otherVehicle = new Vehicle() { Name = "Other" };

        [SetUp]
        public void Setup()
        {
            //Arrange

            _vehicleRepositoryMoq = new Mock<IVehicleRepository>();
            _vehicleRepositoryMoq
                .Setup(r => r.GetVehicleType("Emergency"))
                .Returns(new Vehicle
                {
                    Id = 1,
                    Name = "Emergency"
                }
            );

            _vehicleRepositoryMoq
                .Setup(r => r.isValidVehicle(_emergencyVehicle))
                .Returns(true);

            _vehicleRepositoryMoq
                .Setup(r => r.isValidVehicle(_otherVehicle))
                .Returns(false);

            _taxExemptVehicles = new List<Vehicle>
                            {
                                new Vehicle{ Name = "Emergency"},
                                new Vehicle{ Name = "Bus"},
                                new Vehicle{ Name = "Diplomat"},
                                new Vehicle{ Name = "Motorcycle"},
                                new Vehicle{ Name = "Military"},
                                new Vehicle{ Name = "Foreign"},
                            };

            _vehicleRepositoryMoq
               .Setup(r => r.isTollFreeVehicle(_taxExemptVehicles, _emergencyVehicle))
               .Returns(true);

            _vehicleRepositoryMoq
                .Setup(r => r.isTollFreeVehicle(_taxExemptVehicles, _otherVehicle))
                .Returns(false);
        }

        [TestCase("Emergency")]
        public void when_vehiclename_is_exist_then_return_Vehicle(string vehicleName)
        {
            //Act
            var actual = _vehicleRepositoryMoq.Object.GetVehicleType(vehicleName);

            // Assert
            _vehicleRepositoryMoq.Verify(x => x.GetVehicleType(vehicleName));
            Assert.IsInstanceOf<Vehicle>(actual);
            Assert.IsNotNull(actual);
            Assert.That(vehicleName, Is.EqualTo(actual.Name));
        }
        
        [TestCase("Subway")]
        [TestCase("")]
        [TestCase(" ")]
        public void when_vehiclename_is_doesnot_exist_then_return_Null(string vehicleName)
        {
            //Act
            var actual = _vehicleRepositoryMoq.Object.GetVehicleType(vehicleName);

            // Assert
            _vehicleRepositoryMoq.Verify(x => x.GetVehicleType(vehicleName));
            Assert.IsNull(actual);
        }

        [Test]
        public void when_vehicle_isvalid_then_return_true()
        {
            //Act
            var actual = _vehicleRepositoryMoq.Object.isValidVehicle(_emergencyVehicle);          

            // Assert
            Assert .AreEqual(true, actual);
            _vehicleRepositoryMoq.Verify(x => x.isValidVehicle(_emergencyVehicle));
        }

        [Test]
        public void when_vehicle_isInvalid_then_return_false()
        {
            //Act
            var actual = _vehicleRepositoryMoq.Object.isValidVehicle(_otherVehicle);

            // Assert
            Assert.AreEqual(false, actual);
            _vehicleRepositoryMoq.Verify(x => x.isValidVehicle(_otherVehicle));
        }

        [Test]
        public void when_vehicle_isToolfree_then_return_true()
        {
            //Act           
            var actual= _vehicleRepositoryMoq.Object.isTollFreeVehicle(_taxExemptVehicles, _emergencyVehicle);

            // Assert
            Assert.AreEqual(true, actual);
            _vehicleRepositoryMoq.Verify(x => x.isTollFreeVehicle(_taxExemptVehicles, _emergencyVehicle));
        }

        [Test]
        public void when_vehicle_isNotToolfree_then_return_false()
        {
            //Act
            var actual = _vehicleRepositoryMoq.Object.isTollFreeVehicle(_taxExemptVehicles, _otherVehicle);

            // Assert
            Assert.AreEqual(false, actual);
            _vehicleRepositoryMoq.Verify(x => x.isTollFreeVehicle(_taxExemptVehicles, _otherVehicle));
        }
    }

}

