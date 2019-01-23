using System;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
	public class PumpBurstOnTimeEEPROMTestHelper : GreenSenseIrrigatorHardwareTestHelper
	{
		public int PumpBurstOnTime = 3;

		public void TestPumpBurstOnTimeEEPROM()
		{
			WriteTitleText("Starting pump burst on time EEPROM test");

			Console.WriteLine("Pump burst on time: " + PumpBurstOnTime + "%");
			Console.WriteLine("");

			ConnectDevices();

			ResetDeviceSettings ();

			SendPumpBurstOnTimeCommand();

			ResetDeviceViaPin ();

			var dataEntry = WaitForDataEntry ();

			AssertDataValueEquals(dataEntry, "B", PumpBurstOnTime);
		}

		public void SendPumpBurstOnTimeCommand()
		{
			var command = "B" + PumpBurstOnTime;

			WriteParagraphTitleText("Sending pump burst on time command...");

			SendDeviceCommand(command);

			var dataEntry = WaitForDataEntry();

			WriteParagraphTitleText("Checking pump burst on time value...");

			AssertDataValueEquals(dataEntry, "B", PumpBurstOnTime);
		}
	}
}
