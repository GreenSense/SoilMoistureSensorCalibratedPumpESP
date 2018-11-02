﻿using System;
using NUnit.Framework;
namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
	public class MqttOutputTestHelper : GreenSenseMqttHardwareTestHelper
	{
		public int ReadInterval = 1;
		public int SimulatedSoilMoistureSensorValue = -1;

		public void TestMqttOutput()
		{
			WriteTitleText("Starting MQTT output test");

			Console.WriteLine("Read interval: " + ReadInterval);
			Console.WriteLine("Soil moisture sensor value: " + SimulatedSoilMoistureSensorValue);

			EnableDevices(SimulatedSoilMoistureSensorValue > -1);

			EnableMqtt();

			SetDeviceReadInterval(ReadInterval);

			if (SimulatedSoilMoistureSensorValue > -1)
				SimulateSoilMoisture(SimulatedSoilMoistureSensorValue);

			Console.WriteLine("Waiting for MQTT data...");

			var numberOfEntriesToWaitFor = 5; // TODO: See if this can be reduced

			Mqtt.WaitForData(numberOfEntriesToWaitFor);

			Assert.AreEqual(numberOfEntriesToWaitFor, Mqtt.Data.Count, "Incorrect number of entries returned.");

			var latestEntry = Mqtt.Data[Mqtt.Data.Count - 1];

			Assert.IsNotNull(latestEntry, "The latest MQTT entry is null.");

			Mqtt.PrintDataEntry(latestEntry);

			Assert.IsTrue(latestEntry.ContainsKey("C"), "The latest MQTT entry doesn't contain a 'C' key/value.");

			var valueString = latestEntry["C"];

			Console.WriteLine("Calibrated value string: \"" + valueString + "\"");

			var containsWhitespace = valueString.Trim().Length != valueString.Length;

			Assert.IsFalse(containsWhitespace, "The calibrated value contains whitespace: \"" + valueString + "\"");

			var isDecimal = valueString.Contains(".");

			Assert.IsFalse(isDecimal, "The calibrated value contains a decimal point when it shouldn't.");

			Console.WriteLine("MQTT calibrated value string: " + valueString);

			var value = Convert.ToInt32(valueString);

			AssertIsWithinRange("MQTT calibrated soil moisture", SimulatedSoilMoistureSensorValue, value, CalibratedValueMarginOfError);
		}
	}
}