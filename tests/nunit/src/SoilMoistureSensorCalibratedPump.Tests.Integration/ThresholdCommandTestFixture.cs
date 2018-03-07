using System;
using NUnit.Framework;
using duinocom;
using System.Threading;
using ArduinoSerialControllerClient;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.IO.Ports;

namespace SoilMoistureSensorCalibratedPump.Tests.Integration
{
	[TestFixture(Category="Integration")]
	public class ThresholdCommandTestFixture : BaseTestFixture
	{
		/*[Test]
		public void Test_CalibrateDryToCurrentValueCommand()
		{
			var percentage = 20;

			var raw = 219;//(percentage * 1024 / 100)-1;

			TestCalibrateToCurrentCommand ("dry", "D", percentage, raw);
		}*/

		[Test]
		public void Test_SetThresholdToSpecifiedValueCommand()
		{
			TestSetThresholdToSpecifiedValue (15, -1);
		}

		[Test]
		public void Test_SetThresholdToCurrentValueCommand()
		{
			TestSetThresholdToSpecifiedValue (0, 25);
		}

		/*[Test]
		public void Test_CalibrateWetToCurrentValueCommand()
		{
			var percentage = 80;

			var raw = 880;

			TestCalibrateToCurrentCommand ("wet", "W", percentage, raw);
		}

		[Test]
		public void Test_CalibrateWetToSpecifiedValueCommand()
		{
			var percentage = 80;

			var raw = 880;//(percentage * 1024 / 100)-1;

			TestCalibrateToCurrentCommand ("wet", "W" + raw, -1, raw);
		}*/

		public void TestSetThresholdToSpecifiedValue(int threshold, int simulatedSoilMoisturePercentage)
		{

			Console.WriteLine ("");
			Console.WriteLine ("==============================");
			Console.WriteLine ("Starting set threshold command test");
			Console.WriteLine ("");
			//Console.WriteLine ("Percentage in: " + percentageIn);
			//Console.WriteLine ("Expected raw: " + expectedRaw);

			SerialClient soilMoistureMonitor = null;
			ArduinoSerialDevice soilMoistureSimulator = null;

			var irrigatorPortName = "/dev/ttyUSB0";
			var simulatorPortName = "/dev/ttyUSB1";

			string[] ports = SerialPort.GetPortNames ();
			var multipleDevicePairsDetected = Array.IndexOf (ports, "/dev/ttyUSB2") > -1;
			if (multipleDevicePairsDetected) {
				Console.WriteLine ("Multiple device pairs detected. Automatically configuring port names to become the second device pair.");

				irrigatorPortName = "/dev/ttyUSB2";
				simulatorPortName = "/dev/ttyUSB3";
			}

			try {
				soilMoistureMonitor = new SerialClient (irrigatorPortName, 9600);

				if (simulatedSoilMoisturePercentage > -1)
					soilMoistureSimulator = new ArduinoSerialDevice (simulatorPortName, 9600);

				Console.WriteLine("");
				Console.WriteLine("Connecting to serial devices...");
				Console.WriteLine("");

				soilMoistureMonitor.Open ();
				if (simulatedSoilMoisturePercentage > -1)
					soilMoistureSimulator.Connect ();

				Thread.Sleep (2000);

				Console.WriteLine("");
				Console.WriteLine("Reading the output from the monitor device...");
				Console.WriteLine("");

				// Read the output
				var output = soilMoistureMonitor.Read ();

				Console.WriteLine (output);
				Console.WriteLine ("");

				Console.WriteLine("");
				Console.WriteLine("Sending 'X' command to device to reset to defaults...");
				Console.WriteLine("");

				// Reset defaults
				soilMoistureMonitor.WriteLine ("X");

				Thread.Sleep(1000);

				Console.WriteLine("");
				Console.WriteLine("Reading the output from the monitor device...");
				Console.WriteLine("");

				// Read the output
				output = soilMoistureMonitor.Read ();

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

					Thread.Sleep(5000);
					// Works but slow
					//Thread.Sleep(8000);
					//Thread.Sleep(12000);

					Console.WriteLine("");
					Console.WriteLine("Reading output from the monitor device...");
					Console.WriteLine("");

					// Read the output
					output = soilMoistureMonitor.Read ();

					Console.WriteLine (output);
					Console.WriteLine ("");
				}

				var command = "T";

				if (threshold > 0)
					command = command + threshold;

				Console.WriteLine("");
				Console.WriteLine("Sending command to device: " + command);
				Console.WriteLine("");

				// Send the command
				soilMoistureMonitor.WriteLine (command);

				Thread.Sleep(6000);

				Console.WriteLine("");
				Console.WriteLine("Reading the output from the monitor device...");
				Console.WriteLine("");

				// Read the output
				output = soilMoistureMonitor.Read ();

				Console.WriteLine (output);
				Console.WriteLine ("");

				Console.WriteLine("");
				Console.WriteLine("Checking the output...");
				Console.WriteLine("");

				var data = ParseOutputLine(GetLastDataLine(output));

				Console.WriteLine ("");
				Console.WriteLine ("Checking threshold value");

				var expectedThreshold = 0;
				if (threshold > 0)
					expectedThreshold = threshold;
				else
				{
					expectedThreshold = simulatedSoilMoisturePercentage;
					// Reverse the percentage if it is reversed in the sketch
					if (CalibrationIsReversedByDefault)
						expectedThreshold = ArduinoConvert.ReversePercentage(expectedThreshold);
				}
				
				Assert.IsTrue(data.ContainsKey("T"));

				var newThresholdValue = data["T"];

				Console.WriteLine("Threshold: " + newThresholdValue);

				// If the threshold was specified in the command then the output should be exact
				if (threshold > 0)
					Assert.AreEqual(expectedThreshold, newThresholdValue, "Invalid threshold: " + newThresholdValue);
				else // Otherwise going by the simulated soil moisture sensor theres a small margin of error
				{
					var thresholdIsWithinRange = IsWithinRange (expectedThreshold, newThresholdValue, 3);

					Assert.IsTrue (thresholdIsWithinRange, "Invalid threshold: " + newThresholdValue);

				}

			} catch (IOException ex) {
				Console.WriteLine (ex.ToString ());
				Assert.Fail ();
			} finally {
				if (soilMoistureMonitor != null)
					soilMoistureMonitor.Close ();

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

		public string GetLastDataLine(string output)
		{
			var lines = output.Split ('\n');

			for (int i = lines.Length - 1; i >= 0; i--) {
				var line = lines [i];
				if (line.StartsWith ("D;"))
					return line;
			}

			return String.Empty;
		}
	}
}
