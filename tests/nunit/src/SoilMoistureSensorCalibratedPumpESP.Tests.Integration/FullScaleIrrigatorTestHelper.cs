﻿using System;
using NUnit.Framework;
using System.Collections.Generic;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class FullScaleIrrigatorTestHelper : GreenSenseIrrigatorHardwareTestHelper
    {
        public int PumpOnIncreaseValue = 8;
        public int PumpOffDecreaseValue = 5;

        public int SoilMoisturePercentageMaximum = 65;
        public int SoilMoisturePercentageMinimum = 10;

        public void RunFullScaleTest ()
        {
            WriteTitleText ("Starting full scale test");

            ConnectDevices ();

            int soilMoisturePercentage = 30;

            int totalCyclesToRun = 15;

            for (int i = 0; i < totalCyclesToRun; i++) {
                soilMoisturePercentage = RunFullScaleTestCycle (soilMoisturePercentage);
            }
        }


        public int RunFullScaleTestCycle (int soilMoisturePercentage)
        {
            WriteSubTitleText ("Starting full scale test cycle");

            WriteParagraphTitleText ("Reading the value of the soil moisture sensor pump pin...");

            var pumpPinValue = SimulatorDigitalRead (SimulatorPumpPin);

            Console.WriteLine ("Pump pin value: " + GetOnOffString (pumpPinValue));

            WriteParagraphTitleText ("Simulating specified soil moisture percentage...");

            SimulateSoilMoisture (soilMoisturePercentage);

            WriteParagraphTitleText ("Getting data to check that values are correct...");

            // Skip some data to wait for the value to stabilise
            WaitForData (1);

            var dataEntry = WaitForDataEntry ();

            AssertSoilMoistureValuesAreCorrect (soilMoisturePercentage, dataEntry);

            var soilMoisturePercentageReading = Convert.ToInt32 (dataEntry ["C"]);

            AssertSoilMoistureValueIsWithinRange (soilMoisturePercentageReading);

            var newSoilMoisturePercentage = AdjustSoilMoisturePercentageBasedOnPumpPin (soilMoisturePercentage, pumpPinValue);

            return newSoilMoisturePercentage;
        }

        public void AssertSoilMoistureValuesAreCorrect (int soilMoisturePercentage, Dictionary<string, string> dataEntry)
        {
            WriteParagraphTitleText ("Checking calibrated value...");

            AssertDataValueIsWithinRange (dataEntry, "C", soilMoisturePercentage, CalibratedValueMarginOfError);

            WriteParagraphTitleText ("Checking raw value...");

            var expectedRawValue = soilMoisturePercentage * AnalogPinMaxValue / 100;

            AssertDataValueIsWithinRange (dataEntry, "R", expectedRawValue, RawValueMarginOfError);

        }

        public void AssertSoilMoistureValueIsWithinRange (int soilMoisturePercentage)
        {
            WriteParagraphTitleText ("Checking soil moisture percentage is between " + SoilMoisturePercentageMinimum + " and " + SoilMoisturePercentageMaximum);

            Console.WriteLine ("  Current soil moisture level: " + soilMoisturePercentage + "%");

            if (soilMoisturePercentage > ApplyOffset (SoilMoisturePercentageMaximum, ExpectedCalibratedValueOffset))
                Assert.Fail ("Soil moisture went above " + ApplyOffset (SoilMoisturePercentageMaximum, ExpectedCalibratedValueOffset) + "%");

            if (soilMoisturePercentage < ApplyOffset (SoilMoisturePercentageMinimum, ExpectedCalibratedValueOffset))
                Assert.Fail ("Soil moisture dropped below " + ApplyOffset (SoilMoisturePercentageMinimum, ExpectedCalibratedValueOffset) + "%");
        }

        public int AdjustSoilMoisturePercentageBasedOnPumpPin (int soilMoisturePercentage, bool pumpPinValue)
        {
            WriteParagraphTitleText ("Adjusting simulated soil moisture sensor based on whether pump pin is on/off.");

            if (pumpPinValue) {
                Console.WriteLine ("  Pump pin is high. Increasing simulated soil moisture.");
                soilMoisturePercentage += PumpOnIncreaseValue;
            } else {
                Console.WriteLine ("  Pump pin is low. Decreasing simulated soil moisture.");
                soilMoisturePercentage -= PumpOffDecreaseValue;
            }

            Console.WriteLine ("  New soil moisture level: " + soilMoisturePercentage + "%");

            return soilMoisturePercentage;
        }
    }
}
