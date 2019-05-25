using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class PumpTestHelper : GreenSenseIrrigatorHardwareTestHelper
    {
        public PumpStatus PumpCommand = PumpStatus.Auto;
        public int SimulatedSoilMoisturePercentage = 50;
        public int BurstOnTime = 3;
        public int BurstOffTime = 3;
        public int Threshold = 30;
        public int DurationToCheckPump = 5;

        public void TestPump ()
        {
            WriteTitleText ("Starting pump test");

            Console.WriteLine ("Pump command: " + PumpCommand);
            Console.WriteLine ("Simulated soil moisture: " + SimulatedSoilMoisturePercentage + "%");
            Console.WriteLine ("");

            ConnectDevices ();

            var cmd = "P" + (int)PumpCommand;

            SendDeviceCommand (cmd);

            SendDeviceCommand ("B" + BurstOnTime);
            SendDeviceCommand ("O" + BurstOffTime);
            SendDeviceCommand ("T" + Threshold);

            SimulateSoilMoisture (SimulatedSoilMoisturePercentage);

            // Skip some data to give the value time to stabilise
            WaitForData (3);

            var dataEntry = WaitForDataEntry ();

            CheckDataValues (dataEntry);
        }

        public void CheckDataValues (Dictionary<string, string> dataEntry)
        {
            AssertDataValueEquals (dataEntry, "P", (int)PumpCommand);
            AssertDataValueEquals (dataEntry, "B", BurstOnTime);
            AssertDataValueEquals (dataEntry, "O", BurstOffTime);
            AssertDataValueEquals (dataEntry, "T", Threshold);

            // TODO: Check PO value matches the pump

            AssertDataValueIsWithinRange (dataEntry, "C", SimulatedSoilMoisturePercentage, CalibratedValueMarginOfError);

            switch (PumpCommand) {
            case PumpStatus.Off:
                CheckPumpIsOff ();
                break;
            case PumpStatus.On:
                CheckPumpIsOn ();
                break;
            case PumpStatus.Auto:
                CheckPumpIsAuto ();
                break;
            }
        }

        public void CheckPumpIsOff ()
        {
            WaitUntilSimulatorPinIs ("pump", SimulatorPumpPin, Off);
            AssertSimulatorPinForDuration ("pump", SimulatorPumpPin, false, DurationToCheckPump);
        }

        public void CheckPumpIsOn ()
        {
            AssertSimulatorPinForDuration ("pump", SimulatorPumpPin, true, DurationToCheckPump);
        }

        public void CheckPumpIsAuto ()
        {
            var waterIsNeeded = SimulatedSoilMoisturePercentage < Threshold;

            if (waterIsNeeded) {
                var pumpStaysOn = BurstOffTime == 0;

                if (pumpStaysOn) {
                    CheckPumpIsOn ();
                } else {
                    // Wait for the pump to turn on, off, then on again for the first time
                    WaitUntilSimulatorPinIs ("pump", SimulatorPumpPin, true);
                    WaitUntilSimulatorPinIs ("pump", SimulatorPumpPin, false);
                    WaitUntilSimulatorPinIs ("pump", SimulatorPumpPin, true);

                    // Check on time     
                    var timeOn = WaitWhileSimulatorPinIs ("pump", SimulatorPumpPin, true);
                    AssertIsWithinRange ("pump on time", BurstOnTime, timeOn, TimeErrorMargin);

                    // Check off time
                    var timeOff = WaitWhileSimulatorPinIs ("pump", SimulatorPumpPin, false);
                    AssertIsWithinRange ("pump off time", BurstOffTime, timeOff, TimeErrorMargin);

                    // Check on time
                    timeOn = WaitWhileSimulatorPinIs ("pump", SimulatorPumpPin, true);
                    AssertIsWithinRange ("pump on time", BurstOnTime, timeOn, TimeErrorMargin);

                    // Check off time
                    timeOff = WaitWhileSimulatorPinIs ("pump", SimulatorPumpPin, false);
                    AssertIsWithinRange ("pump off time", BurstOffTime, timeOff, TimeErrorMargin);
                }
            } else {
                CheckPumpIsOff ();
            }
        }
    }
}
