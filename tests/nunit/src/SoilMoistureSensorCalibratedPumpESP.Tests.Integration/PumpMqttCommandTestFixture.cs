﻿using NUnit.Framework;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
	[TestFixture(Category = "Integration")]
	public class PumpMqttCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_SetPumpToOn()
		{
			using (var helper = new PumpMqttCommandTestHelper())
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
			using (var helper = new PumpMqttCommandTestHelper())
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
			using (var helper = new PumpMqttCommandTestHelper())
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
