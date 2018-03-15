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
	[TestFixture(Category="Integration")]
	public class FullScaleTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_Complete()
		{
			Console.WriteLine ("");
			Console.WriteLine ("==============================");
			Console.WriteLine ("Starting full scale test");
			Console.WriteLine ("");

			SerialClient irrigator = null;
			ArduinoSerialDevice soilMoistureSimulator = null;

			int totalCyclesToRun = 20;

			var irrigatorPortName = GetDevicePort ();
			var simulatorPortName = GetSimulatorPort ();

			try {
				irrigator = new SerialClient (irrigatorPortName, GetSerialBaudRate());
				soilMoistureSimulator = new ArduinoSerialDevice (simulatorPortName, GetSerialBaudRate());

				Console.WriteLine ("");
				Console.WriteLine ("Irrigator port: " + irrigatorPortName);
				Console.WriteLine ("Simulator port: " + simulatorPortName);
				Console.WriteLine ("");
      
				irrigator.Open ();
				soilMoistureSimulator.Connect ();

				Thread.Sleep (5000);

				Console.WriteLine("");
				Console.WriteLine("Reading the output from the monitor device...");
				Console.WriteLine("");

				// Read the output
				var output = irrigator.Read ();

				Console.WriteLine (output);
				Console.WriteLine ("");

				// Reset defaults
				irrigator.WriteLine ("X");

				Thread.Sleep (1000);

				Console.WriteLine("");
				Console.WriteLine("Reading the output from the monitor device...");
				Console.WriteLine("");

				// Read the output
				output = irrigator.Read ();

				Console.WriteLine (output);
				Console.WriteLine ("");

				// Set read interval to 1
				irrigator.WriteLine ("V1");

				Thread.Sleep (1000);

				// Set pump burst off time to 0
				irrigator.WriteLine ("O0");

				Thread.Sleep (1000);

				Console.WriteLine("");
				Console.WriteLine("Reading the output from the monitor device...");
				Console.WriteLine("");

				// Read the output
				output = irrigator.Read ();

				Console.WriteLine (output);
				Console.WriteLine ("");

				// Reverse calibration (if needed)
				if (CalibrationIsReversedByDefault)
					irrigator.WriteLine ("R");

				Thread.Sleep (1000);

				Console.WriteLine("");
				Console.WriteLine("Reading the output from the monitor device...");
				Console.WriteLine("");

				// Read the output
				output = irrigator.Read ();

				Console.WriteLine (output);
				Console.WriteLine ("");

				int soilMoistureValue = 5;

				for (int i = 0; i <= totalCyclesToRun; i++) {
					soilMoistureValue = RunCycle (soilMoistureValue, CalibrationIsReversedByDefault, irrigator, soilMoistureSimulator);
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
		
		public int RunCycle(int soilMoisturePercentage, bool calibrationIsReversed, SerialClient soilMoistureMonitor, ArduinoSerialDevice soilMoistureSimulator)
		{
		
			Console.WriteLine ("");
			Console.WriteLine ("==============================");
			Console.WriteLine ("Starting test cycle");
			Console.WriteLine ("");
      
      
			int percentageValue = soilMoisturePercentage;
      
			Console.WriteLine ("");
			Console.WriteLine ("Sending percentage to simulator: " + percentageValue);
			Console.WriteLine ("");
      
			soilMoistureSimulator.AnalogWritePercentage (9, percentageValue);
      
			Thread.Sleep (8000);
      
			Console.WriteLine ("");
			Console.WriteLine ("Reading data from device...");
			Console.WriteLine ("");
      
			var output = soilMoistureMonitor.Read ();
      
			Console.WriteLine (output);
			Console.WriteLine ("");

			Console.WriteLine ("");
			Console.WriteLine ("Reading value of pump pin...");
			var pumpPinValue = soilMoistureSimulator.DigitalRead (2);
			Console.WriteLine ("Pump pin value: " + pumpPinValue);

			Console.WriteLine ("");
			Console.WriteLine ("Adjusting simulated soil moisture sensor based on whether pump pin is on/off.");
			Console.WriteLine ("");

			if (pumpPinValue) {
				Console.WriteLine ("Pump pin is high. Increasing simulated soil moisture.");
				soilMoisturePercentage += 10;
			} else {
				Console.WriteLine ("Pump pin is low. Decreasing simulated soil moisture.");
				soilMoisturePercentage -= 1;
			}

			Console.WriteLine ("");
			Console.WriteLine ("Checking soil moisture percentage is between 0 and 100");
			Console.WriteLine ("Currently: " + soilMoisturePercentage);
			Console.WriteLine ("");

			if (soilMoisturePercentage > 100)
				Assert.Fail ("Soil moisture hit 100%");
//        soilMoisturePercentage = 100;

			if (soilMoisturePercentage < 0)
				Assert.Fail ("Soil moisture hit 0%");
//        soilMoisturePercentage = 0;
      
			Console.WriteLine ("New soil moisture percentage: " + soilMoisturePercentage);

			Thread.Sleep (2000);

			Console.WriteLine ("");
			Console.WriteLine ("Reading data from device...");
			Console.WriteLine ("");

			output = soilMoistureMonitor.Read ();

			Console.WriteLine (output);
			Console.WriteLine ("");

			var data = ParseOutputLine (GetLastDataLine(output));
      
			Console.WriteLine ("");
			Console.WriteLine ("Checking calibrated value");
			var expectedCalibratedValue = percentageValue;
      
			//if (calibrationIsReversed)
			//	expectedCalibratedValue = ArduinoConvert.ReversePercentage (percentageValue);
      
			Assert.IsTrue (data.ContainsKey ("C"), "'C' key not found");

			var calibratedValue = data ["C"];

			Console.WriteLine ("Calibrated value: " + calibratedValue);

			var calibratedValueIsWithinRange = IsWithinRange (expectedCalibratedValue, calibratedValue, 6);
      
			var percentageToRaw = soilMoisturePercentage * 1023 / 100;

			Assert.IsTrue (calibratedValueIsWithinRange, "Invalid value for 'C' (calibrated value): " + data["C"]);
      
			Console.WriteLine ("");
			Console.WriteLine ("Checking raw value");
      
			var expectedRawValue = ArduinoConvert.PercentageToAnalog (percentageValue);
      
			var rawValueIsWithinRange = IsWithinRange (expectedRawValue, data ["R"], 60);
      
			Assert.IsTrue (rawValueIsWithinRange, "Invalid value for 'R' (raw value): " + data["R"]);
      
			Console.WriteLine ("");
			Console.WriteLine ("Finished test cycle");
			Console.WriteLine ("==============================");
			Console.WriteLine ("");

			return soilMoisturePercentage;
		}
	}
}
