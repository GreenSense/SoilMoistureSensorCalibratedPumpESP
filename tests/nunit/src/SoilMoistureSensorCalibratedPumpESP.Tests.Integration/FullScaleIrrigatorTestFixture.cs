using System;
using NUnit.Framework;
using duinocom;
using System.Threading;
using ArduinoSerialControllerClient;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;

namespace SoilMoistureSensorCalibratedPump.Tests.Integration
{
	[TestFixture(Category = "Integration")]
	public class FullScaleIrrigatorTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_FullScaleTest()
		{
			using (var helper = new FullScaleIrrigatorTestHelper())
			{
				helper.DevicePort = GetDevicePort();
				helper.DeviceBaudRate = GetDeviceSerialBaudRate();

				helper.SimulatorPort = GetSimulatorPort();
				helper.SimulatorBaudRate = GetSimulatorSerialBaudRate();

				helper.RunFullScaleTest();
			}
		}
	}
}
