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
	public class PumpCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_SetPumpToOn()
		{
			using (var helper = new PumpCommandTestHelper())
			{
				helper.PumpCommand = PumpStatus.On;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestPumpCommand();
			}
		}

		[Test]
		public void Test_SetPumpToOff()
		{
			using (var helper = new PumpCommandTestHelper())
			{
				helper.PumpCommand = PumpStatus.Off;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestPumpCommand();
			}
		}

		[Test]
		public void Test_SetPumpToAuto()
		{
			using (var helper = new PumpCommandTestHelper())
			{
				helper.PumpCommand = PumpStatus.Auto;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestPumpCommand();
			}
		}
	}
}
