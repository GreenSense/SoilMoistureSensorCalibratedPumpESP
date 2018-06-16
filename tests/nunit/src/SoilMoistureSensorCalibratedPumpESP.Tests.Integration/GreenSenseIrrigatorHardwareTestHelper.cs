using System;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
	public class GreenSenseIrrigatorHardwareTestHelper : GreenSenseHardwareTestHelper
	{
		public int SimulatorPumpPin = 2;

		public GreenSenseIrrigatorHardwareTestHelper()
		{
		}

		public override void PrepareDeviceForTest(bool consoleWriteDeviceOutput)
		{
			base.PrepareDeviceForTest(false);

			SetDevicePumpOffTime(0);

			if (consoleWriteDeviceOutput)
				ReadFromDeviceAndOutputToConsole();
		}

		public void SetDevicePumpOffTime(int pumpOffTime)
		{
			var cmd = "O" + pumpOffTime;

			Console.WriteLine("");
			Console.WriteLine("Setting pump off time to " + pumpOffTime + " seconds...");
			Console.WriteLine("  Sending '" + cmd + "' command to device");
			Console.WriteLine("");

			SendDeviceCommand(cmd);
		}
	}
}
