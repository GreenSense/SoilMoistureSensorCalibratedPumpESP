using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO.Ports;

namespace SoilMoistureSensorCalibratedPump.Tests.Integration
{
	public class BaseTestFixture
	{
		public int PwmHigh = 255;
		public int AnalogHigh = 1023;
		
		public bool CalibrationIsReversedByDefault = true; // Reversed by default to matches common soil moisture sensors
		
		public BaseTestFixture ()
		{
		}

		[SetUp]
		public void Initialize()
		{
		}

		[TearDown]
		public void Finish()
		{
		}

		public string GetDevicePort()
		{
			var irrigatorPortName = "/dev/ttyUSB0";

			string[] ports = SerialPort.GetPortNames ();
			var multipleDevicePairsDetected = Array.IndexOf (ports, "/dev/ttyUSB2") > -1;
			if (multipleDevicePairsDetected) {
				irrigatorPortName = "/dev/ttyUSB2";
			}

			Console.WriteLine ("Device port: " + irrigatorPortName);

			return irrigatorPortName;
		}

		public string GetSimulatorPort()
		{
			var simulatorPortName = "/dev/ttyUSB1";

			string[] ports = SerialPort.GetPortNames ();
			var multipleDevicePairsDetected = Array.IndexOf (ports, "/dev/ttyUSB2") > -1;
			if (multipleDevicePairsDetected) {
				simulatorPortName = "/dev/ttyUSB3";
			}

			Console.WriteLine ("Simulator port: " + simulatorPortName);
			return simulatorPortName;
		}

		public int GetSerialBaudRate()
		{
			return 9600;
		}

		public bool IsWithinRange(int expectedValue, int actualValue, int allowableMarginOfError)
		{
			Console.WriteLine("Checking value is within range...");
			Console.WriteLine("Expected value: " + expectedValue);
			Console.WriteLine("Actual value: " + actualValue);
			Console.WriteLine("Allowable margin of error: " + allowableMarginOfError);
			
			var minAllowableValue = expectedValue-allowableMarginOfError;
			if (minAllowableValue < 0)
				minAllowableValue = 0;
			var maxAllowableValue = expectedValue+allowableMarginOfError;
			
			Console.WriteLine("Max allowable value: " + maxAllowableValue);
			Console.WriteLine("Min allowable value: " + minAllowableValue);
			
			var isWithinRange = actualValue <= maxAllowableValue &&
				actualValue >= minAllowableValue;
		
			Console.WriteLine("Is within range: " + isWithinRange);
		
			return isWithinRange;
		}

		public Dictionary<string, int> ParseOutputLine(string outputLine)
		{
			var dictionary = new Dictionary<string, int> ();

			if (IsValidOutputLine (outputLine)) {
				foreach (var pair in outputLine.Split(';')) {
					var parts = pair.Split (':');

					if (parts.Length == 2) {
						var key = parts [0];
						var value = 0;
						try {
							value = Convert.ToInt32 (parts [1]);

							dictionary [key] = value;
						} catch {
							Console.WriteLine ("Warning: Invalid key/value pair '" + pair + "'");
						}
					}
				}
			}

			return dictionary;
		}

		public string GetLastDataLine(string output)
		{
			var lines = output.Split ('\n');

			for (int i = lines.Length - 1; i >= 0; i--) {
				var line = lines [i].Trim();
				if (IsValidOutputLine(line))
					return line;
			}

			return String.Empty;
		}

		public bool IsValidOutputLine(string outputLine)
		{
			var dataPrefix = "D;";

			var dataPostFix = ";;";

			return outputLine.StartsWith(dataPrefix)
				&& outputLine.EndsWith(dataPostFix);
		}
	}
}
