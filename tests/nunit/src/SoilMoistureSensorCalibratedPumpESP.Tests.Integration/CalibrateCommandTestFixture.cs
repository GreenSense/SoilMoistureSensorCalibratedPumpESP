﻿using NUnit.Framework;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    [TestFixture (Category = "Integration")]
    public class CalibrateCommandTestFixture : BaseTestFixture
    {
        [Test]
        public void Test_CalibrateDryCommand_200 ()
        {
            using (var helper = new CalibrateCommandTestHelper ()) {
                helper.Label = "dry";
                helper.Key = "D";
                helper.RawSoilMoistureValue = 200;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestCalibrateCommand ();
            }
        }

        [Test]
        public void Test_CalibrateDryCommand_220 ()
        {
            using (var helper = new CalibrateCommandTestHelper ()) {
                helper.Label = "dry";
                helper.Key = "D";
                helper.RawSoilMoistureValue = 220;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestCalibrateCommand ();
            }
        }

        [Test]
        public void Test_CalibrateWetCommand_880 ()
        {
            using (var helper = new CalibrateCommandTestHelper ()) {
                helper.Label = "wet";
                helper.Key = "W";
                helper.RawSoilMoistureValue = 880;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestCalibrateCommand ();
            }
        }

        [Test]
        public void Test_CalibrateWetCommand_900 ()
        {
            using (var helper = new CalibrateCommandTestHelper ()) {
                helper.Label = "wet";
                helper.Key = "W";
                helper.RawSoilMoistureValue = 900;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestCalibrateCommand ();
            }
        }
    }
}