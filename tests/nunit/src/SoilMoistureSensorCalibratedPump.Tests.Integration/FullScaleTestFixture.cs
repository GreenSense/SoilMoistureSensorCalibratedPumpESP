using System;
using NUnit.Framework;
using duinocom;
using System.Threading;
using ArduinoSerialControllerClient;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;

namespace SoilMoistureSensorCalibratedSerial.Tests.Integration
{
	[TestFixture(Category="Integration")]
	public class FullScaleTestFixture : BaseTestFixture
	{
		[Test]
		public void Test_Complete()
		{
      Console.WriteLine("");
      Console.WriteLine("==============================");
      Console.WriteLine("Starting full scale test");
      Console.WriteLine("");

      SerialClient irrigator = null;
      ArduinoSerialDevice soilMoistureSimulator = null;

      int totalCyclesToRun = 100;

      var irrigatorPortName = "/dev/ttyUSB0";
      var simulatorPortName = "/dev/ttyUSB1";

      string[] ports = SerialPort.GetPortNames();
      var multipleDevicePairsDetected = Array.IndexOf(ports, "/dev/ttyUSB2") > -1;
      if (multipleDevicePairsDetected)
      {
        Console.WriteLine("Multiple device pairs detected. Automatically configuring port names to become the second device pair.");

        irrigatorPortName = "/dev/ttyUSB2";
        simulatorPortName = "/dev/ttyUSB3";
      }
      try
      {
        irrigator = new SerialClient(irrigatorPortName, 9600);
        soilMoistureSimulator = new ArduinoSerialDevice(simulatorPortName, 9600);
      
        irrigator.Open();
        soilMoistureSimulator.Connect();

        Thread.Sleep(5000);

        // Reset defaults
        irrigator.WriteLine("X");

        int soilMoistureValue = 5;

        for (int i = 0; i <= totalCyclesToRun; i++)
        {
            soilMoistureValue = RunCycle(soilMoistureValue, CalibrationIsReversedByDefault, irrigator, soilMoistureSimulator);
        }

      }
      catch(IOException ex)
      {
        Console.WriteLine(ex.ToString());
      }
      finally
      {
        if (irrigator != null)
          irrigator.Close();
          
        if (soilMoistureSimulator != null)
          soilMoistureSimulator.Disconnect();
      }
		}
		
		public int RunCycle(int soilMoisturePercentage, bool calibrationIsReversed, SerialClient soilMoistureMonitor, ArduinoSerialDevice soilMoistureSimulator)
		{
		
      Console.WriteLine("");
      Console.WriteLine("==============================");
      Console.WriteLine("Starting test cycle");
      Console.WriteLine("");
      
      
      int percentageValue = soilMoisturePercentage;
      
        
      
      Console.WriteLine("");
      Console.WriteLine("Sending percentage to simulator: " + percentageValue);
      
      soilMoistureSimulator.AnalogWritePercentage(9, percentageValue);
      
      Thread.Sleep(1000);
      
      Console.WriteLine("");
      Console.WriteLine("Reading data from soil moisture monitor");
      
      var outputLine = soilMoistureMonitor.Read();
      
      Console.WriteLine(outputLine);
      Console.WriteLine("");

      var pumpPin = soilMoistureSimulator.DigitalRead(2);

      Console.WriteLine("Pump pin: " + pumpPin);

      if (pumpPin)
      {
        Console.WriteLine("Pump pin is high. Increasing simulated soil moisture.");
        soilMoisturePercentage += 10;
      }
      else
      {
        Console.WriteLine("Pump pin is low. Decreasing simulated soil moisture.");
        soilMoisturePercentage -= 1;
      }

      if (soilMoisturePercentage > 100)
        Assert.Fail("Soil moisture hit 100%");
//        soilMoisturePercentage = 100;

      if (soilMoisturePercentage < 0)
        Assert.Fail("Soil moisture hit 0%");
//        soilMoisturePercentage = 0;
      
      Console.WriteLine("New soil moisture percentage: " + soilMoisturePercentage);

      /*var data = ParseOutputLine(outputLine);
      
      
      Console.WriteLine("");
      Console.WriteLine("Checking calibrated value");
      var expectedCalibratedValue = percentageValue;
      
      if (calibrationIsReversed)
        expectedCalibratedValue = ArduinoConvert.ReversePercentage(percentageValue);
      
      var calibratedValueIsWithinRange = IsWithinRange(expectedCalibratedValue, data["C"], 6);
      
      Assert.IsTrue(calibratedValueIsWithinRange, "Invalid value for 'C' (calibrated value).");
      
      Console.WriteLine("");
      Console.WriteLine("Checking raw value");
      
      var expectedRawValue = ArduinoConvert.PercentageToAnalog(percentageValue);
      
      var rawValueIsWithinRange = IsWithinRange(expectedRawValue, data["R"], 60);
      
      Assert.IsTrue(rawValueIsWithinRange, "Invalid value for 'R' (raw value).");*/
      
      Console.WriteLine("");
      Console.WriteLine("Finished test cycle");
      Console.WriteLine("==============================");
      Console.WriteLine("");

      return soilMoisturePercentage;
		}
		
		public Dictionary<string, int> ParseOutputLine(string outputLine)
		{
		  var dictionary = new Dictionary<string, int>();
		  
		  if (IsValidOutputLine(outputLine))
		  {
  		  foreach (var pair in outputLine.Split(';'))
  		  {
  		    var parts = pair.Split(':');
  		    
  		    if (parts.Length == 2)
  		    {
  		      var key = parts[0];
  		      var value = 0;
  		      try
  		      {
  		        value = Convert.ToInt32(parts[1]);
  		      
  		        dictionary[key] = value;
  		      }
  		      catch
  		      {
  		        Console.WriteLine("Warning: Invalid key/value pair '" + pair + "'");
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
