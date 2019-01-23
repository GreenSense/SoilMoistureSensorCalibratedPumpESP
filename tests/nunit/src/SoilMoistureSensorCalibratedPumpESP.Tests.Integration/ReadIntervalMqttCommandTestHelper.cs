using System;
namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
	public class ReadIntervalMqttCommandTestHelper : GreenSenseMqttHardwareTestHelper
	{
		public int ReadInterval = 1;

		public void TestSetReadIntervalCommand()
		{
			WriteTitleText("Starting read interval command test");

			Console.WriteLine("Read interval: " + ReadInterval);

			ConnectDevices(false);

			EnableMqtt();

			Mqtt.SendCommand("I", ReadInterval);

			var entries = WaitForData(2);

			var dataEntry = entries[entries.Length-1];

			AssertDataValueEquals(dataEntry, "I", ReadInterval);
		}
	}
}
