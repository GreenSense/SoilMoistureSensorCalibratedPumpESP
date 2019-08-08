using System;
using NUnit.Framework;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class ThresholdToCurrentCommandTestHelper : GreenSenseIrrigatorHardwareTestHelper
    {
        public int Threshold = 30;
        public int SimulatedSoilMoisturePercentage = -1;

        public void TestThresholdToCurrentValueCommand ()
        {
            WriteTitleText ("Starting threshold command test");

            Console.WriteLine ("Simulated soil moisture: " + SimulatedSoilMoisturePercentage + "%");
            Console.WriteLine ("Threshold: " + Threshold + "%");
            Console.WriteLine ("");

            var simulatorIsNeeded = SimulatedSoilMoisturePercentage > -1;

            ConnectDevices (simulatorIsNeeded);

            if (simulatorIsNeeded) {
                SimulateSoilMoisture (SimulatedSoilMoisturePercentage);

                // Skip some data
                WaitForData (2);

                var dataEntry = WaitForDataEntry ();

                AssertDataValueIsWithinRange (dataEntry, "C", SimulatedSoilMoisturePercentage, CalibratedValueMarginOfError);
            }

            SendThresholdCommand ();
        }

        public void SendThresholdCommand ()
        {
            var command = "T";

            WriteParagraphTitleText ("Sending threshold command...");

            SendDeviceCommand (command);

            var dataEntry = WaitForDataEntry ();

            WriteParagraphTitleText ("Checking threshold value...");

            Assert.IsTrue (dataEntry.ContainsKey ("T"), "Data entry doesn't contain threshold 'T' key/value.");

            var threshold = Convert.ToInt32 (dataEntry ["T"]);

            AssertIsWithinRange ("threshold", ApplyOffset (SimulatedSoilMoisturePercentage, ExpectedCalibratedValueOffset), threshold, CalibratedValueMarginOfError);
        }
    }
}

