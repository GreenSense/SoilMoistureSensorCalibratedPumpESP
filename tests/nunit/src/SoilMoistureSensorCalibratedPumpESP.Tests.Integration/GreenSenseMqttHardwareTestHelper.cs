﻿using System;
namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
	public class GreenSenseMqttHardwareTestHelper : GreenSenseHardwareTestHelper
	{
		public MqttTestHelper Mqtt;

		public GreenSenseMqttHardwareTestHelper()
		{
		}

		public void EnableMqtt()
		{
			Mqtt = new MqttTestHelper();
			Mqtt.Start();
		}

		public void EnableMqtt(string deviceName)
		{
			Mqtt = new MqttTestHelper(deviceName);
			Mqtt.Start();
		}

	}
}
