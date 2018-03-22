using System;
using NUnit.Framework;
using duinocom;
using System.Threading;
using ArduinoSerialControllerClient;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SoilMoistureSensorCalibratedPump.Tests.Integration
{
	[TestFixture(Category="Integration")]
	public class CalibrateCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_CalibrateDryToCurrentValueCommand()
		{
			var percentage = 20;

			var raw = 218;

			TestCalibrateToCurrentValueCommand ("dry", "D", percentage, raw);
		}

		[Test]
		public void Test_CalibrateDryToSpecifiedValueCommand()
		{
			var percentage = 20;

			var raw = 220;

			TestCalibrateToCurrentValueCommand ("dry", "D" + raw, -1, raw);
		}

		[Test]
		public void Test_CalibrateWetToCurrentValueCommand()
		{
			var percentage = 80;

			var raw = 880;

			TestCalibrateToCurrentValueCommand ("wet", "W", percentage, raw);
		}

		[Test]
		public void Test_CalibrateWetToSpecifiedValueCommand()
		{
			var raw = 880;

			TestCalibrateToCurrentValueCommand ("wet", "W" + raw, -1, raw);
		}

		public void TestCalibrateToCurrentValueCommand(string label, string command, int simulatedSoilMoisturePercentage, int expectedRaw)
		{

			Console.WriteLine ("");
			Console.WriteLine ("==============================");
			Console.WriteLine ("Starting calibrate " + label + " command test");
			Console.WriteLine ("");
			Console.WriteLine ("Percentage in: " + simulatedSoilMoisturePercentage);
			Console.WriteLine ("Expected raw: " + expectedRaw);

			SerialClient irrigator = null;
			ArduinoSerialDevice soilMoistureSimulator = null;

			try {
				irrigator = new SerialClient (GetDevicePort(), GetSerialBaudRate());
				soilMoistureSimulator = new ArduinoSerialDevice (GetSimulatorPort(), GetSerialBaudRate());

				Console.WriteLine("");
				Console.WriteLine("Connecting to serial devices...");
				Console.WriteLine("");

				irrigator.Open ();
				soilMoistureSimulator.Connect ();

				Thread.Sleep (2000);

				Console.WriteLine("");
				Console.WriteLine("Reading the output from the device...");
				Console.WriteLine("");

				// Read the output
				var output = irrigator.Read ();

				Console.WriteLine (output);
				Console.WriteLine ("");

				Console.WriteLine("");
				Console.WriteLine("Sending 'X' command to device to reset to defaults...");
				Console.WriteLine("");

				// Reset defaults
				irrigator.WriteLine ("X");

				Thread.Sleep(2000);

				// Set read interval to 1
				irrigator.WriteLine ("V1");

				Thread.Sleep(2000);

				Console.WriteLine("");
				Console.WriteLine("Reading the output from the device...");
				Console.WriteLine("");

				// Read the output
				output = irrigator.Read ();

				Console.WriteLine (output);
				Console.WriteLine ("");

				Thread.Sleep(1000);

				// If a percentage is specified for the simulator then set the simulated soil moisture value (otherwise skip)
				if (simulatedSoilMoisturePercentage > -1)
				{
					Console.WriteLine("");
					Console.WriteLine("Sending analog percentage to simulator: " + simulatedSoilMoisturePercentage);
					Console.WriteLine("");

					// Set the simulated soil moisture
					soilMoistureSimulator.AnalogWritePercentage (9, simulatedSoilMoisturePercentage);

					Thread.Sleep(2000);
					// Works but slow
					//Thread.Sleep(8000);
					//Thread.Sleep(12000);

					Console.WriteLine("");
					Console.WriteLine("Reading output from the device...");
					Console.WriteLine("");

					// Read the output
					output = irrigator.Read ();

					Console.WriteLine (output);
					Console.WriteLine ("");

					// Parse the values in the data line
					var values = ParseOutputLine(GetLastDataLine(output));

					// Get the raw soil moisture value
					var rawValue = values["R"];


					Console.WriteLine("");
					Console.WriteLine("Checking the valuesdeviceor device...");
					Console.WriteLine("");

					Console.WriteLine("Expected raw: " + expectedRaw);

					// Ensure the raw value is in the valid range
					Assert.IsTrue(IsWithinRange(expectedRaw, rawValue, 12), "Raw value is outside the valid range: " + rawValue);
				}

				Console.WriteLine("");
				Console.WriteLine("Sending '" + command + "' to the device...");
				Console.WriteLine("");

				// Send the command
				irrigator.WriteLine (command);

				Thread.Sleep(2000);

				Console.WriteLine("");
				Console.WriteLine("Reading the output from the device...");
				Console.WriteLine("");

				// Read the output
				output = irrigator.Read ();

				Console.WriteLine (output);
				Console.WriteLine ("");

				Console.WriteLine("");
				Console.WriteLine("Checking the output...");
				Console.WriteLine("");

				var data = ParseOutputLine(GetLastDataLine(output));

				var value = data[command.Substring(0, 1)];

				Console.Write("Value: " + value);

				Assert.IsTrue(IsWithinRange(expectedRaw, value, 10), "Calibration value is outside the valid range: " + value);


			} catch (IOException ex) {
				Console.WriteLine (ex.ToString ());
				Assert.Fail ();
			} finally {
				if (irrigator != null)
					irrigator.Close ();

				if (soilMoistureSimulator != null)
					soilMoistureSimulator.Disconnect ();
			}
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

		public bool IsValidOutputLine(string outputLine)
		{
			var dataPrefix = "D;";

			return outputLine.StartsWith(dataPrefix);
		}
	}
}