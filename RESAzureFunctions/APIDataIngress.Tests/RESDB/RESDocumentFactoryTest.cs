using APIDataIngress.RESDB;
using APIDataIngress.RESDB.Implementation;
using APIDataIngress.Test.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading.Tasks;

namespace APIDataIngress.Test.RESDocument
{
    [TestFixture]
    public class RESDocumentFactoryTest
    {
        private Mock<ILogger<RESDocumentFactory>> loggerMock;
        private string dataPath;

        [SetUp]
        public void SetupTests()
        {
            loggerMock = new Mock<ILogger<RESDocumentFactory>>();
            dataPath = TestDataHelper.GetTestDataFolder("Events");
        }

        [Test]
        public void RESDocumentFactory_Constructor_CreatesRESDocumentFactory()
        {
            // Act
            var resDocumentFactory = new RESDocumentFactory(loggerMock.Object);

            // Assert
            Assert.IsInstanceOf<RESDocumentFactory>(resDocumentFactory);
        }

        [Test]
        public void RESDocumentFactory_CreateWithInvalidJson_ThrowsJsonException()
        {
            // Arrange
            string badJson = "123123123";
            var resDocumentFactory = new RESDocumentFactory(loggerMock.Object);
            loggerMock.Setup(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<FormattedLogValues>(),
                It.IsAny<JsonReaderException>(),
                It.IsAny<Func<object, Exception, string>>()
            ));

            // Act 
            var resDocument = resDocumentFactory.Create(badJson);

            // Assert
            Assert.AreEqual(default(RESDocumentBase), resDocument);
            loggerMock.Verify();
        }

        [Test]
        public void RESDocumentFactory_CreateWithValidSensorRESDocument_CreatesAValidRESDocument()
        {
            // Arrange
            string resDocumentJson = File.ReadAllText(Path.Combine(dataPath, "valid_sensor_topic.json"));
            var resDocumentFactory = new RESDocumentFactory(loggerMock.Object);

            // Act
            var resDocument = resDocumentFactory.Create(resDocumentJson);

            // Assert
            Assert.IsInstanceOf<SensorRESDocument>(resDocument);
            Assert.IsTrue(resDocument.IsValid());
        }

        [Test]
        public async Task RESDocumentFactory_CreateWithBadTimestampSensor_LogsErrorAndDoesNotCreateARESDocument()
        {
            // Arrange
            string badJson = await File.ReadAllTextAsync(Path.Combine(dataPath, "ts_invalid_sensor_topic.json"));
            var resDocumentFactory = new RESDocumentFactory(loggerMock.Object);
            loggerMock.Setup(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<FormattedLogValues>(),
                It.IsAny<JsonReaderException>(),
                It.IsAny<Func<object, Exception, string>>()
            ));

            // Act
            var resDocument = resDocumentFactory.Create(badJson);

            // Assert
            Assert.AreEqual(default(RESDocumentBase), resDocument);
            loggerMock.Verify();
        }

        [Test]
        public async Task RESDocumentFactory_CreateWithInvalidSensorRESDocument_CreatesInvalidRESDocument()
        {
            // Arrange
            string resDocumentJson = await File.ReadAllTextAsync(Path.Combine(dataPath, "invalid_sensor_topic.json"));
            var resDocumentFactory = new RESDocumentFactory(loggerMock.Object);

            // Act
            var resDocument = resDocumentFactory.Create(resDocumentJson);

            // Assert
            Assert.IsInstanceOf<SensorRESDocument>(resDocument);
            Assert.IsFalse(resDocument.IsValid());
            loggerMock.VerifyNoOtherCalls();
        }

        [Test]
        public void RESDocumentFactory_CreateWithValidThermostatRESDocument_CreatesAValidRESDocument()
        {
            // Arrange
            string resDocumentJson = File.ReadAllText(Path.Combine(dataPath, "valid_thermostat_topic.json"));
            var resDocumentFactory = new RESDocumentFactory(loggerMock.Object);

            // Act
            var resDocument = resDocumentFactory.Create(resDocumentJson);

            // Assert
            Assert.IsInstanceOf<ThermostatRESDocument>(resDocument);
            Assert.IsTrue(resDocument.IsValid());
        }

        [Test]
        public async Task RESDocumentFactory_CreateWithBadTimestampThermostat_LogsErrorAndDoesNotCreateARESDocument()
        {
            // Arrange
            string badJson = await File.ReadAllTextAsync(Path.Combine(dataPath, "ts_invalid_thermostat_topic.json"));
            var resDocumentFactory = new RESDocumentFactory(loggerMock.Object);
            loggerMock.Setup(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<FormattedLogValues>(),
                It.IsAny<JsonReaderException>(),
                It.IsAny<Func<object, Exception, string>>()
            ));

            // Act
            var resDocument = resDocumentFactory.Create(badJson);

            // Assert
            Assert.AreEqual(default(RESDocumentBase), resDocument);
            loggerMock.Verify();
        }

        [Test]
        public async Task RESDocumentFactory_CreateWithInvalidThermostatRESDocument_LogsMessageAndReturnsNull()
        {
            // Arrange
            string resDocumentJson = await File.ReadAllTextAsync(Path.Combine(dataPath, "invalid_thermostat_topic.json"));
            var resDocumentFactory = new RESDocumentFactory(loggerMock.Object);

            // Act
            loggerMock.Setup(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<FormattedLogValues>(),
                It.IsAny<JsonReaderException>(),
                It.IsAny<Func<object, Exception, string>>()
            ));
            var resDocument = resDocumentFactory.Create(resDocumentJson);

            // Assert
            Assert.IsInstanceOf<ThermostatRESDocument>(resDocument);
            loggerMock.Verify();
        }

        [Test]
        public void RESDocumentFactory_CreateWithValidWaterHeaterRESDocument_CreatesAValidRESDocument()
        {
            // Arrange
            string resDocumentJson = File.ReadAllText(Path.Combine(dataPath, "valid_water_heater_topic.json"));
            var resDocumentFactory = new RESDocumentFactory(loggerMock.Object);

            // Act
            var resDocument = resDocumentFactory.Create(resDocumentJson);

            // Assert
            Assert.IsInstanceOf<WaterHeaterRESDocument>(resDocument);
            Assert.IsTrue(resDocument.IsValid());
        }

        [Test]
        public async Task RESDocumentFactory_CreateWithBadTimestampWaterheater_LogsErrorAndDoesNotCreateARESDocument()
        {
            // Arrange
            string badJson = await File.ReadAllTextAsync(Path.Combine(dataPath, "ts_invalid_water_heater_topic.json"));
            var resDocumentFactory = new RESDocumentFactory(loggerMock.Object);
            loggerMock.Setup(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<FormattedLogValues>(),
                It.IsAny<JsonReaderException>(),
                It.IsAny<Func<object, Exception, string>>()
            ));

            // Act
            var resDocument = resDocumentFactory.Create(badJson);

            // Assert
            Assert.AreEqual(default(RESDocumentBase), resDocument);
            loggerMock.Verify();
        }

        [Test]
        public async Task RESDocumentFactory_CreateWithInvalidWaterHeaterRESDocument_CreatesInvalidRESDocument()
        {
            // Arrange
            string resDocumentJson = await File.ReadAllTextAsync(Path.Combine(dataPath, "invalid_water_heater_topic.json"));
            var resDocumentFactory = new RESDocumentFactory(loggerMock.Object);

            // Act
            var resDocument = resDocumentFactory.Create(resDocumentJson);

            // Assert
            Assert.IsInstanceOf<WaterHeaterRESDocument>(resDocument);
            Assert.IsFalse(resDocument.IsValid());
            loggerMock.VerifyNoOtherCalls();
        }

        [Test]
        public void RESDocumentFactory_CreateWithValidRawRESDocument_CreatesAValidRESDocument()
        {
            // Arrange
            string resDocumentJson = File.ReadAllText(Path.Combine(dataPath, "valid_raw_data.json"));
            var resDocumentFactory = new RESDocumentFactory(loggerMock.Object);

            // Act
            var resDocument = resDocumentFactory.Create(resDocumentJson);

            // Assert
            Assert.IsInstanceOf<RawRESDocument>(resDocument);
            Assert.IsTrue(resDocument.IsValid());
        }

        [Test]
        public async Task RESDocumentFactory_CreateWithBadTimestampRaw_LogsErrorAndDoesNotCreateARESDocument()
        {
            // Arrange
            string badJson = await File.ReadAllTextAsync(Path.Combine(dataPath, "ts_invalid_raw_data.json"));
            var resDocumentFactory = new RESDocumentFactory(loggerMock.Object);
            loggerMock.Setup(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<FormattedLogValues>(),
                It.IsAny<JsonReaderException>(),
                It.IsAny<Func<object, Exception, string>>()
            ));

            // Act
            var resDocument = resDocumentFactory.Create(badJson);

            // Assert
            Assert.AreEqual(default(RESDocumentBase), resDocument);
            loggerMock.Verify();
        }

        [Test]
        public async Task RESDocumentFactory_CreateWithInvalidRawRESDocument_LogsMessageAndReturnsNull()
        {
            // Arrange
            string resDocumentJson = await File.ReadAllTextAsync(Path.Combine(dataPath, "invalid_raw_data.json"));
            var resDocumentFactory = new RESDocumentFactory(loggerMock.Object);

            // Act
            loggerMock.Setup(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<FormattedLogValues>(),
                It.IsAny<JsonReaderException>(),
                It.IsAny<Func<object, Exception, string>>()
            ));
            var resDocument = resDocumentFactory.Create(resDocumentJson);

            // Assert
            Assert.IsInstanceOf<RawRESDocument>(resDocument);
            loggerMock.Verify();
        }
    }
}
