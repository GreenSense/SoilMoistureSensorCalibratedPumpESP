using NUnit.Framework;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
	[TestFixture(Category = "Integration")]
	public class ThresholdMqttCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_SetThresholdToSpecifiedValueCommand_15Percent()
		{
			using (var helper = new ThresholdMqttCommandTestHelper())
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
			using (var helper = new ThresholdMqttCommandTestHelper())
			{
				helper.Threshold = 25;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestThresholdCommand();
			}
		}
	}
}
