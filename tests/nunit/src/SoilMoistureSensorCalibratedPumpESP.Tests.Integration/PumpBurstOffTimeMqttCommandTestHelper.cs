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

			ConnectDevices(false);

			EnableMqtt();

			Mqtt.SendCommand("O", PumpBurstOffTime);

			var data = WaitForData (2);
			var dataEntry = data[data.Length-1];

			AssertDataValueEquals(dataEntry, "O", PumpBurstOffTime);
		}
	}
}