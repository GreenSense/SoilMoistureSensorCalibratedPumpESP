using System;
using NUnit.Framework;
using duinocom;
using System.Threading;
using ArduinoSerialControllerClient;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.IO.Ports;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
	[TestFixture(Category = "Integration")]
	public class ThresholdEEPROMTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_SetThreshold_25Percent()
		{
			using (var helper = new ThresholdEEPROMTestHelper())
			{
				helper.Threshold = 25;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestThresholdEEPROM();
			}
		}

		[Test]
		public void Test_SetThreshold_40Percent()
		{
			using (var helper = new ThresholdEEPROMTestHelper())
			{
				helper.Threshold = 40;

				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.TestThresholdEEPROM();
			}
		}

	}
}
