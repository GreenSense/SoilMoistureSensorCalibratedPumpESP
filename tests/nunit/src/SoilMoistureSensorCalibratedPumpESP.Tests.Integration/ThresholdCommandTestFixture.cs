using NUnit.Framework;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    [TestFixture (Category = "Integration")]
    public class ThresholdCommandTestFixture : BaseTestFixture
    {
        [Test]
        public void Test_SetThresholdCommand_15Percent ()
        {
            using (var helper = new ThresholdCommandTestHelper ()) {
                helper.Threshold = 15;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestThresholdCommand ();
            }
        }

        [Test]
        public void Test_SetThresholdCommand_25Percent ()
        {
            using (var helper = new ThresholdCommandTestHelper ()) {
                helper.Threshold = 25;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestThresholdCommand ();
            }
        }
    }
}
