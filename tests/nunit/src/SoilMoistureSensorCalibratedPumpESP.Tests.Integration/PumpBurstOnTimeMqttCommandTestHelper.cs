﻿using System;
namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
	public class PumpBurstOnTimeMqttCommandTestHelper : GreenSenseIrrigatorHardwareTestHelper
	{
		public int PumpBurstOnTime = 1;

		public void TestPumpBurstOnTimeCommand()
		{
			WriteTitleText("Starting pump burst on time command test");

			Console.WriteLine("Pump burst on time: " + PumpBurstOnTime);
			Console.WriteLine("");

			EnableDevices(false);

			EnableMqtt();

			Mqtt.SendCommand("B", PumpBurstOnTime);

			var dataEntry = WaitForDataEntry();

			AssertDataValueEquals(dataEntry, "B", PumpBurstOnTime);
		}
	}
}