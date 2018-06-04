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
	public class PumpBurstOffTimeCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_SetPumpBurstOffTime()
		{

			Console.WriteLine ("");
			Console.WriteLine ("==============================");
			Console.WriteLine ("Starting set pump burst off time command test");
			Console.WriteLine ("");

			SerialClient irrigator = null;
			ArduinoSerialDevice soilMoistureSimulator = null;

			var irrigatorPortName = GetDevicePort();

			try {
				irrigator = new SerialClient (irrigatorPortName, GetDeviceSerialBaudRate());

				Console.WriteLine("");
				Console.WriteLine("Connecting to serial devices...");
				Console.WriteLine("");

				irrigator.Open ();

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

				Console.WriteLine("");
				Console.WriteLine("Reading the output from the device...");
				Console.WriteLine("");

				// Read the output
				output = irrigator.Read ();

				Console.WriteLine (output);
				Console.WriteLine ("");

				Thread.Sleep(1000);

				var pumpBurstOffTime = 10; // Seconds

				var command = "O" + pumpBurstOffTime;

				Console.WriteLine("");
				Console.WriteLine("Sending command to device: " + command);
				Console.WriteLine("");

				// Send the command
				irrigator.WriteLine (command);

				Thread.Sleep(3000);

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
				Console.WriteLine ("Checking pump burst off time value");

				Assert.IsTrue(data.ContainsKey("O"), "'O' (burst off time) key not found.");

				var newPumpBurstOffTimeValue = Convert.ToInt32(data["O"]);

				Console.WriteLine("Pump burst off time: " + newPumpBurstOffTimeValue);

				// If the pumpBurstOffTime was specified in the command then the output should be exact
				if (pumpBurstOffTime > 0)
					Assert.AreEqual(pumpBurstOffTime, newPumpBurstOffTimeValue, "Invalid pump burst off time: " + newPumpBurstOffTimeValue);
				else // Otherwise going by the simulated soil moisture sensor theres a small margin of error
				{
					var pumpBurstOffTimeIsWithinRange = IsWithinRange (pumpBurstOffTime, newPumpBurstOffTimeValue, 3);

					Assert.IsTrue (pumpBurstOffTimeIsWithinRange, "Invalid pump burst off time: " + newPumpBurstOffTimeValue);

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
