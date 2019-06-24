﻿using System;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class CalibrateToCurrentCommandTestHelper : GreenSenseHardwareTestHelper
    {
        public string Label;
        public string Key;
        public int SimulatedSoilMoisturePercentage = -1;
        public int RawSoilMoistureValue = 0;

        public CalibrateToCurrentCommandTestHelper ()
        {
        }

        public void TestCalibrateCommand ()
        {
            WriteTitleText ("Starting calibrate " + Label + " command test");


            Console.WriteLine ("Simulated soil moisture: " + SimulatedSoilMoisturePercentage + "%");

            if (RawSoilMoistureValue == 0)
                RawSoilMoistureValue = SimulatedSoilMoisturePercentage * AnalogPinMaxValue / 100;

            Console.WriteLine ("Raw soil moisture value: " + RawSoilMoistureValue);
            Console.WriteLine ("");

            var simulatorIsNeeded = SimulatedSoilMoisturePercentage > -1;

            ConnectDevices (simulatorIsNeeded);

            if (SimulatorIsEnabled) {
                SimulateSoilMoisture (SimulatedSoilMoisturePercentage);

                // Skip the first X entries to give the value time to stabilise
                WaitForData (1);

                var dataEntry = WaitForDataEntry ();

                AssertDataValueIsWithinRange (dataEntry, "R", RawSoilMoistureValue, RawValueMarginOfError);
            }

            SendCalibrationCommand ();
        }

        public void SendCalibrationCommand ()
        {
            var command = Key;

            // If the simulator isn't enabled then the raw value is passed as part of the command to specify it directly
            if (!SimulatorIsEnabled)
                command = command + RawSoilMoistureValue;

            SendDeviceCommand (command);

            // Skip the first X entries to give the value time to stabilise
            WaitForData (1);

            var dataEntry = WaitForDataEntry ();

            // If using the soil moisture simulator then the value needs to be within a specified range
            if (SimulatorIsEnabled)
                AssertDataValueIsWithinRange (dataEntry, Key, RawSoilMoistureValue, RawValueMarginOfError);
            else // Otherwise it needs to be exact
                AssertDataValueEquals (dataEntry, Key, RawSoilMoistureValue);
        }
    }
}

