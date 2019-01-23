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
	[TestFixture(Category = "Integration")]
	public class PumpBurstOnTimeEEPROMTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_SetPumpBurstOnTime_1sec()
		{
			using (var helper = new PumpBurstOnTimeEEPROMTestHelper())
			{
				helper.PumpBurstOnTime = 1;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestPumpBurstOnTimeEEPROM();
			}
		}

		[Test]
		public void Test_SetPumpBurstOnTime_5sec()
		{
			using (var helper = new PumpBurstOnTimeEEPROMTestHelper())
			{
				helper.PumpBurstOnTime = 5;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestPumpBurstOnTimeEEPROM();
			}
		}

	}
}
