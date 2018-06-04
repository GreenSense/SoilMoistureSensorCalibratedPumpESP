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
	public class PumpCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_SetPumpToOn()
		{
			TestSetPump (1, -1, 1);
		}

		[Test]
		public void Test_SetPumpToOff()
		{
			TestSetPump (0, -1, 0);
		}

		[Test]
		public void Test_SetPumpToAuto_Low()
		{
			TestSetPump (2, 1, 1);
		}

		[Test]
		public void Test_SetPumpToAuto_High()
		{
			TestSetPump (2, 99, 0);
		}

		public void TestSetPump(int pumpStatus, int simulatedSoilMoisturePercentage, int expectedPumpOutput)
		{

			Console.WriteLine ("");
			Console.WriteLine ("==============================");
			Console.WriteLine ("Starting set threshold command test");
			Console.WriteLine ("");
			Console.WriteLine ("Pump: " + pumpStatus);
			Console.WriteLine ("Simulated soil moisture percentage: " + simulatedSoilMoisturePercentage);

			SerialClient irrigator = null;
			ArduinoSerialDevice soilMoistureSimulator = null;

			var irrigatorPortName = GetDevicePort();
			var simulatorPortName = GetSimulatorPort ();

			try {
				irrigator = new SerialClient (irrigatorPortName, GetDeviceSerialBaudRate());
				soilMoistureSimulator = new ArduinoSerialDevice (simulatorPortName, GetSimulatorSerialBaudRate());

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

				Thread.Sleep(1000);

				// Set read/output interval to 1 sec
				irrigator.WriteLine ("V1");

				Thread.Sleep(1000);

				// Set burst off time to 0 seconds
				irrigator.WriteLine ("O0");

				Thread.Sleep(1000);

				if (CalibrationIsReversedByDefault)
				{
					// Reverse calibration values
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

					Thread.Sleep(5000);

					Console.WriteLine("");
					Console.WriteLine("Reading output from the device...");
					Console.WriteLine("");

					// Read the output
					output = irrigator.Read ();

					Console.WriteLine (output);
					Console.WriteLine ("");
				}

				var command = "P" + pumpStatus;

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
				Console.WriteLine ("Checking pump is on value");

				Assert.IsTrue(data.ContainsKey("P"));

				var newPumpStatus = Convert.ToInt32(data["P"]);

				Console.WriteLine("Pump status: " + newPumpStatus);

				Assert.AreEqual(pumpStatus, newPumpStatus, "Invalid pump status: " + newPumpStatus);

				if (expectedPumpOutput > -1)
				{
					Assert.IsTrue(data.ContainsKey("PO"));

					var newPumpValue = Convert.ToInt32(data["PO"]);

					Console.WriteLine("Pump: " + newPumpValue);

					Assert.AreEqual(expectedPumpOutput, newPumpValue, "Invalid pump value: " + newPumpValue);
				}

				Console.WriteLine ("");
				Console.WriteLine ("Reading value of pump pin...");
				var pumpPinValue = soilMoistureSimulator.DigitalRead (2);
				Console.WriteLine ("Pump pin value: " + pumpPinValue);

				Assert.AreEqual(Convert.ToBoolean(expectedPumpOutput), pumpPinValue, "Invalid pump pin value");


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
