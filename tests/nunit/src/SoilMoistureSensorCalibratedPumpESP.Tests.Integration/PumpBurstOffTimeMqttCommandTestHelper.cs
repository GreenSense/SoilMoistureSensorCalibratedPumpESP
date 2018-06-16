using System;
namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
	public class PumpBurstOffTimeMqttCommandTestHelper : GreenSenseIrrigatorHardwareTestHelper
	{
		public int PumpBurstOffTime = 1;

		public void TestPumpBurstOffTimeCommand()
		{
			WriteTitleText("Starting pump burst off time command test");

			Console.WriteLine("Pump burst off time: " + PumpBurstOffTime);
			Console.WriteLine("");

			EnableDevices(false);

			EnableMqtt();

			Mqtt.SendCommand("O", PumpBurstOffTime);

			var dataEntry = WaitForDataEntry();

			AssertDataValueEquals(dataEntry, "O", PumpBurstOffTime);
		}
	}
}