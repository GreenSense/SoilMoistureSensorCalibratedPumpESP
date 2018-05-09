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
	public class ReadIntervalCommandTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_SetReadIntervalCommand()
		{
			Console.WriteLine ("");
			Console.WriteLine ("==============================");
			Console.WriteLine ("Starting read interval command test");
			Console.WriteLine ("");

			SerialClient irrigator = null;

			try {
				irrigator = new SerialClient (GetDevicePort(), GetDeviceSerialBaudRate());

				Console.WriteLine("");
				Console.WriteLine("Connecting to serial devices...");
				Console.WriteLine("");

				irrigator.Open ();

				Thread.Sleep (1000);

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

				Console.WriteLine("");
				Console.WriteLine("Reading the output from the device...");
				Console.WriteLine("");

				// Read the output
				output = irrigator.Read ();

				Console.WriteLine (output);
				Console.WriteLine ("");

				var readInterval = 100; // Seconds

				var command = "V" + readInterval;

				Console.WriteLine("");
				Console.WriteLine("Sending '" + command + "' command todevice...");
				Console.WriteLine("");

				// Send the command
				irrigator.WriteLine (command);

				Thread.Sleep(8000);

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

				// Ensure the calibration value is in the valid range
				Assert.AreEqual(readInterval, data["V"], "Invalid read interval: " + data["V"]);

			} catch (IOException ex) {
				Console.WriteLine (ex.ToString ());
				Assert.Fail ();
			} finally {
				if (irrigator != null)
					irrigator.Close ();
			}
		}

	}
}
