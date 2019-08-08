using System;
using NUnit.Framework;
using duinocom;
using System.Threading;
using ArduinoSerialControllerClient;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.IO.Ports;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    [TestFixture (Category = "Integration")]
    public class PumpTestFixture : BaseTestFixture
    {
        [Test]
        public void Test_PumpOn ()
        {
            using (var helper = new PumpTestHelper ()) {
                helper.PumpCommand = PumpMode.On;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestPump ();
            }
        }

        [Test]
        public void Test_PumpOff ()
        {
            using (var helper = new PumpTestHelper ()) {
                helper.PumpCommand = PumpMode.Off;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestPump ();
            }
        }

        [Test]
        public void Test_PumpAuto_WaterNeeded_Burst1_Off0 ()
        {
            using (var helper = new PumpTestHelper ()) {
                helper.PumpCommand = PumpMode.Auto;
                helper.SimulatedSoilMoisturePercentage = 10;
                helper.BurstOnTime = 1;
                helper.BurstOffTime = 0;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestPump ();
            }
        }

        [Test]
        public void Test_PumpAuto_WaterNeeded_Burst1_Off1 ()
        {
            using (var helper = new PumpTestHelper ()) {
                helper.PumpCommand = PumpMode.Auto;
                helper.SimulatedSoilMoisturePercentage = 10;
                helper.BurstOnTime = 1;
                helper.BurstOffTime = 1;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestPump ();
            }
        }

        [Test]
        public void Test_PumpAuto_WaterNeeded_Burst2_Off2 ()
        {
            using (var helper = new PumpTestHelper ()) {
                helper.PumpCommand = PumpMode.Auto;
                helper.SimulatedSoilMoisturePercentage = 10;
                helper.BurstOnTime = 2;
                helper.BurstOffTime = 2;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestPump ();
            }
        }

        [Test]
        public void Test_PumpAuto_WaterNotNeeded ()
        {
            using (var helper = new PumpTestHelper ()) {
                helper.PumpCommand = PumpMode.Auto;
                helper.SimulatedSoilMoisturePercentage = 80;

                helper.DevicePort = GetDevicePort ();
                helper.DeviceBaudRate = GetDeviceSerialBaudRate ();

                helper.SimulatorPort = GetSimulatorPort ();
                helper.SimulatorBaudRate = GetSimulatorSerialBaudRate ();

                helper.TestPump ();
            }
        }
    }
}
