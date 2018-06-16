using System;
namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
	public class ThresholdMqttCommandTestHelper : GreenSenseIrrigatorHardwareTestHelper
	{
		public int Threshold = 30;

		public void TestThresholdCommand()
		{
			WriteTitleText("Starting threshold command test");

			Console.WriteLine("Threshold: " + Threshold + "%");
			Console.WriteLine("");

			EnableDevices(false);

			EnableMqtt();

			SendThresholdCommand();
		}

		public void SendThresholdCommand()
		{
			WriteParagraphTitleText("Sending threshold command...");

			Mqtt.SendCommand("T", Threshold);

			var dataEntry = WaitForDataEntry();

			WriteParagraphTitleText("Checking threshold value...");

			AssertDataValueEquals(dataEntry, "T", Threshold);
		}
	}
}
