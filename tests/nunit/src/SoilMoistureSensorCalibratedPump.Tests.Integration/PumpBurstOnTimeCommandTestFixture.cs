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
	public class PumpBurstOnTimeCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_SetPumpBurstOnTime()
		{

			Console.WriteLine ("");
			Console.WriteLine ("==============================");
			Console.WriteLine ("Starting set pump burst on time command test");
			Console.WriteLine ("");

			SerialClient irrigator = null;
			ArduinoSerialDevice soilMoistureSimulator = null;

			var irrigatorPortName = GetDevicePort();

			try {
				irrigator = new SerialClient (irrigatorPortName, GetSerialBaudRate());

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
				
				// Set output inverval to 1
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

				var pumpBurstOnTime = 10; // Seconds

				var command = "B" + pumpBurstOnTime;

				Console.WriteLine("");
				Console.WriteLine("Sending command to device: " + command);
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

				Console.WriteLine ("");
				Console.WriteLine ("Checking pump burst on time value");

				Assert.IsTrue(data.ContainsKey("B"));

				var newPumpBurstOnTimeValue = data["B"];

				Console.WriteLine("Pump burst on time: " + newPumpBurstOnTimeValue);

				// If the pumpBurstOnTime was specified in the command then the output should be exact
				if (pumpBurstOnTime > 0)
					Assert.AreEqual(pumpBurstOnTime, newPumpBurstOnTimeValue, "Invalid pump burst on time: " + newPumpBurstOnTimeValue);
				else // Otherwise going by the simulated soil moisture sensor theres a small margin of error
				{
					var pumpBurstOnTimeIsWithinRange = IsWithinRange (pumpBurstOnTime, newPumpBurstOnTimeValue, 3);

					Assert.IsTrue (pumpBurstOnTimeIsWithinRange, "Invalid pump burst on time: " + newPumpBurstOnTimeValue);

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
