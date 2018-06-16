using NUnit.Framework;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
	[TestFixture(Category = "Integration")]
	public class ThresholdCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_SetThresholdToSpecifiedValueCommand_15Percent()
		{
			using (var helper = new ThresholdCommandTestHelper())
			{
				helper.Threshold = 15;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestThresholdCommand();
			}
		}

		[Test]
		public void Test_SetThresholdToSpecifiedValueCommand_25Percent()
		{
			using (var helper = new ThresholdCommandTestHelper())
			{
				helper.Threshold = 25;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestThresholdCommand();
			}
		}

		[Test]
		public void Test_SetThresholdToCurrentSoilMoistureValueCommand_15Percent()
		{
			using (var helper = new ThresholdCommandTestHelper())
			{
				var value = 15;

				helper.SimulatedSoilMoisturePercentage = value;
				helper.Threshold = value;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestThresholdCommand();
			}
		}

		[Test]
		public void Test_SetThresholdToCurrentSoilMoistureValueCommand_25Percent()
		{
			using (var helper = new ThresholdCommandTestHelper())
			{
				var value = 25;

				helper.SimulatedSoilMoisturePercentage = value;
				helper.Threshold = value;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestThresholdCommand();
			}
		}
	}
}
