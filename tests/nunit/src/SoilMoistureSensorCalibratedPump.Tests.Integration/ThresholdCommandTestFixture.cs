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

		public void TestSetThresholdToSpecifiedValue(int threshold, int simulatedSoilMoisturePercentage)
		{

			Console.WriteLine ("");
			Console.WriteLine ("==============================");
			Console.WriteLine ("Starting set threshold command test");
			Console.WriteLine ("");
			Console.WriteLine ("Threshold: " + threshold);
			Console.WriteLine ("Simulated soil moisture percentage: " + simulatedSoilMoisturePercentage);

			SerialClient irrigator = null;
			ArduinoSerialDevice soilMoistureSimulator = null;

			var irrigatorPortName = GetDevicePort();
			var simulatorPortName = GetSimulatorPort();

			try {
				irrigator = new SerialClient (irrigatorPortName, GetSerialBaudRate());

				if (simulatedSoilMoisturePercentage > -1)
					soilMoistureSimulator = new ArduinoSerialDevice (simulatorPortName, GetSerialBaudRate());

				Console.WriteLine("");
				Console.WriteLine("Connecting to serial devices...");
				Console.WriteLine("");

				irrigator.Open ();
				if (simulatedSoilMoisturePercentage > -1)
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

				Thread.Sleep(1000);
				
				// Set output interval to 1
				irrigator.WriteLine ("V1");

				Thread.Sleep(1000);
				
				if (CalibrationIsReversedByDefault)
				{
					// Reverse calibration to make it more readable
					irrigator.WriteLine ("R");
	
					Thread.Sleep(1000);
				}

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

					Console.WriteLine("");
					Console.WriteLine("Reading output from the device...");
					Console.WriteLine("");

					// Read the output
					output = irrigator.Read ();

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
				irrigator.WriteLine (command);

				Thread.Sleep(6000);

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
				if (irrigator != null)
					irrigator.Close ();

				if (soilMoistureSimulator != null)
					soilMoistureSimulator.Disconnect ();
			}
		}
	}
}
