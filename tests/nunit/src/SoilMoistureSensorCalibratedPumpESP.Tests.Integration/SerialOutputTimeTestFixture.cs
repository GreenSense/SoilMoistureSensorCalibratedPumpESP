﻿using NUnit.Framework;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    [TestFixture (Category = "Integration")]
    public class SerialOutputTimeTestFixture : BaseTestFixture
    {
        [Test]
        public void Test_SerialOutputTime_2Seconds ()
        {
            using (var helper = new SerialOutputTimeTestHelper ()) {
                helper.ReadInterval = 2;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestSerialOutputTime ();
            }
        }

        [Test]
        public void Test_SerialOutputTime_3Seconds ()
        {
            using (var helper = new SerialOutputTimeTestHelper ()) {
                helper.ReadInterval = 3;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestSerialOutputTime ();
            }
        }

        [Test]
        public void Test_SerialOutputTime_4Seconds ()
        {
            using (var helper = new SerialOutputTimeTestHelper ()) {
                helper.ReadInterval = 4;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestSerialOutputTime ();
            }
        }
    }
}
