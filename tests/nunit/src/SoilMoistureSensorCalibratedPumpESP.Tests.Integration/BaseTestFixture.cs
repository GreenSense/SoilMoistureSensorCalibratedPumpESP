using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO.Ports;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
	public class BaseTestFixture
	{
		public BaseTestFixture()
		{
		}

		[SetUp]
		public virtual void Initialize()
		{
			Console.WriteLine("");
			Console.WriteLine("====================");
			Console.WriteLine("Preparing test");
		}

		[TearDown]
		public virtual void Finish()
		{
			Console.WriteLine("Finished test");
			Console.WriteLine("====================");
			Console.WriteLine("");
		}

		public string GetDevicePort()
		{
			var devicePort = Environment.GetEnvironmentVariable("IRRIGATOR_ESP_PORT");

			if (String.IsNullOrEmpty(devicePort))
				devicePort = "/dev/ttyUSB2";

			Console.WriteLine("Device port: " + devicePort);

			return devicePort;
		}

		public string GetSimulatorPort()
		{
			var simulatorPort = Environment.GetEnvironmentVariable("IRRIGATOR_ESP_SIMULATOR_PORT");

			if (String.IsNullOrEmpty(simulatorPort))
				simulatorPort = "/dev/ttyUSB3";

			Console.WriteLine("Simulator port: " + simulatorPort);

			return simulatorPort;
		}

		public int GetDeviceSerialBaudRate()
		{
			var baudRateString = Environment.GetEnvironmentVariable("IRRIGATOR_ESP_BAUD_RATE");

			var baudRate = 0;

			if (String.IsNullOrEmpty(baudRateString))
				baudRate = 115200;
			else
				baudRate = Convert.ToInt32(baudRateString);

			Console.WriteLine("Device baud rate: " + baudRate);

			return baudRate;
		}

		public int GetSimulatorSerialBaudRate()
		{
			var baudRateString = Environment.GetEnvironmentVariable("IRRIGATOR_ESP_SIMULATOR_BAUD_RATE");

			var baudRate = 0;

			if (String.IsNullOrEmpty(baudRateString))
				baudRate = 9600;
			else
				baudRate = Convert.ToInt32(baudRateString);

			Console.WriteLine("Simulator baud rate: " + baudRate);

			return baudRate;
		}
	}
}
