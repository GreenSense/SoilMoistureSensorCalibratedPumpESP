﻿using NUnit.Framework;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
	[TestFixture(Category = "Integration")]
	public class MqttOutputTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_MqttOutput_1Second_20percent()
		{
			using (var helper = new MqttOutputTestHelper())
			{
				helper.ReadInterval = 1;
				helper.SimulatedSoilMoistureSensorValue = 20;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestMqttOutput();
			}
		}

		[Test]
		public void Test_MqttOutput_4Second_60percent()
		{
			using (var helper = new MqttOutputTestHelper())
			{
				helper.ReadInterval = 1;
				helper.SimulatedSoilMoistureSensorValue = 20;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestMqttOutput();
			}
		}
	}
}
