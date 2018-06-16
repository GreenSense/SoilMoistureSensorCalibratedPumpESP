﻿using NUnit.Framework;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
	[TestFixture(Category = "Integration")]
	public class CalibrateMqttCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_CalibrateDryToSpecifiedValueCommand_200()
		{
			using (var helper = new CalibrateMqttCommandTestHelper())
			{
				helper.Label = "dry";
				helper.Letter = "D";
				helper.RawSoilMoistureValue = 200;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestCalibrateCommand();
			}
		}

		[Test]
		public void Test_CalibrateDryToSpecifiedValueCommand_220()
		{
			using (var helper = new CalibrateMqttCommandTestHelper())
			{
				helper.Label = "dry";
				helper.Letter = "D";
				helper.RawSoilMoistureValue = 220;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestCalibrateCommand();
			}
		}

		[Test]
		public void Test_CalibrateWetToSpecifiedValueCommand_880()
		{
			using (var helper = new CalibrateMqttCommandTestHelper())
			{
				helper.Label = "wet";
				helper.Letter = "W";
				helper.RawSoilMoistureValue = 880;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestCalibrateCommand();
			}
		}

		[Test]
		public void Test_CalibrateWetToSpecifiedValueCommand_900()
		{
			using (var helper = new CalibrateMqttCommandTestHelper())
			{
				helper.Label = "wet";
				helper.Letter = "W";
				helper.RawSoilMoistureValue = 900;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestCalibrateCommand();
			}
		}
	}
}
