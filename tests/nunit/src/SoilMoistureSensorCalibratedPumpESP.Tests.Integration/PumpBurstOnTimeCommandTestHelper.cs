using System;
namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
	public class PumpBurstOnTimeCommandTestHelper : GreenSenseIrrigatorHardwareTestHelper
	{
		public int PumpBurstOnTime = 1;

		public void TestPumpBurstOnTimeCommand()
		{
			WriteTitleText("Starting pump burst on time command test");

			Console.WriteLine("Pump burst on time: " + PumpBurstOnTime);
			Console.WriteLine("");

			EnableDevices(false);

			var cmd = "B" + PumpBurstOnTime;

			SendDeviceCommand(cmd);

			var dataEntry = WaitForDataEntry();

			AssertDataValueEquals(dataEntry, "B", PumpBurstOnTime);
		}
	}
}