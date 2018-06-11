﻿using System;
namespace SoilMoistureSensorCalibratedPump.Tests.Integration
{
	public class ReadIntervalCommandTestHelper : GreenSenseHardwareTestHelper
	{
		public int ReadInterval = 1;

		public ReadIntervalCommandTestHelper()
		{
		}

		public void TestSetReadIntervalCommand()
		{
			WriteTitleText("Starting read interval command test");

			Console.WriteLine("Read interval: " + ReadInterval);

			EnableDevices(false);

			SetDeviceReadInterval(ReadInterval);

			var dataEntry = WaitForDataEntry();

			AssertDataValueEquals(dataEntry, "V", ReadInterval);
		}
	}
}
