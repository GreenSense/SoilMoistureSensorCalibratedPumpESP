using System;
namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
	public class PumpMqttCommandTestHelper : GreenSenseIrrigatorHardwareTestHelper
	{
		public PumpStatus PumpCommand = PumpStatus.Auto;

		public void TestPumpCommand()
		{
			WriteTitleText("Starting pump command test");

			Console.WriteLine("Pump command: " + PumpCommand);
			Console.WriteLine("");

			EnableDevices(false);

			EnableMqtt();

			Mqtt.SendCommand("P", (int)PumpCommand);

			var dataEntry = WaitForDataEntry();
			dataEntry = WaitForDataEntry();
			AssertDataValueEquals(dataEntry, "P", (int)PumpCommand);
		}
	}
}