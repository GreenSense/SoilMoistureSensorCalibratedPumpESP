using System;
using NUnit.Framework;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    [TestFixture (Category = "Integration")]
    public class ThresholdToCurrentCommandTestFixture : BaseTestFixture
    {
        [Test]
        public void Test_SetThresholdToCurrentSoilMoistureValueCommand_15Percent ()
        {
            using (var helper = new ThresholdToCurrentCommandTestHelper ()) {
                var value = 15;

                helper.SimulatedSoilMoisturePercentage = value;
                helper.Threshold = value;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestThresholdCommand ();
            }
        }

        [Test]
        public void Test_SetThresholdToCurrentSoilMoistureValueCommand_25Percent ()
        {
            using (var helper = new ThresholdToCurrentCommandTestHelper ()) {
                var value = 25;

                helper.SimulatedSoilMoisturePercentage = value;
                helper.Threshold = value;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestThresholdCommand ();
            }
        }
    }
}

