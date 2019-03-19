using NUnit.Framework;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    [TestFixture (Category = "Integration")]
    public class ReadIntervalMqttCommandTestFixture : BaseTestFixture
    {
        [Test]
        public void Test_SetReadIntervalCommand_1Second ()
        {
            using (var helper = new ReadIntervalMqttCommandTestHelper ()) {
                helper.ReadInterval = 1;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestSetReadIntervalCommand ();
            }
        }

        [Test]
        public void Test_SetReadIntervalCommand_3Seconds ()
        {
            using (var helper = new ReadIntervalMqttCommandTestHelper ()) {
                helper.ReadInterval = 3;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestSetReadIntervalCommand ();
            }
        }
    }
}
