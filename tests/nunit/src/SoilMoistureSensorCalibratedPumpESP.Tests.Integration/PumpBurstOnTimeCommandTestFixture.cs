using System;
using NUnit.Framework;
using duinocom;
using System.Threading;
using ArduinoSerialControllerClient;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.IO.Ports;

namespace SoilMoistureSensorCalibratedPump.Tests.Integration
{
	[TestFixture(Category = "Integration")]
	public class PumpBurstOnTimeCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_SetPumpBurstOnTime_1Seconds()
		{
			using (var helper = new PumpBurstOnTimeCommandTestHelper())
			{
				helper.PumpBurstOnTime = 1;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestPumpBurstOnTimeCommand();
			}
		}

		[Test]
		public void Test_SetPumpBurstOnTime_5Seconds()
		{
			using (var helper = new PumpBurstOnTimeCommandTestHelper())
			{
				helper.PumpBurstOnTime = 5;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestPumpBurstOnTimeCommand();
			}
		}
	}
}
