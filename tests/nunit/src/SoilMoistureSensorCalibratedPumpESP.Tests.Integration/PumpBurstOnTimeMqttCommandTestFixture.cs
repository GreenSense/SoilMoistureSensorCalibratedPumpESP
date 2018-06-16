using NUnit.Framework;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
	[TestFixture(Category = "Integration")]
	public class PumpBurstOnTimeMqttCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_SetPumpBurstOnTime_1Seconds()
		{
			using (var helper = new PumpBurstOnTimeMqttCommandTestHelper())
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
			using (var helper = new PumpBurstOnTimeMqttCommandTestHelper())
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
