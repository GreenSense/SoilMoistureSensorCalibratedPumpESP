using System;
using System.Threading;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class CalibrateMqttCommandTestHelper : GreenSenseMqttHardwareTestHelper
    {
        public string Label;
        public string Key;
        public int RawSoilMoistureValue = 0;

        public void TestCalibrateCommand ()
        {
            WriteTitleText ("Starting calibrate " + Label + " command test");

            Console.WriteLine ("Raw soil moisture value: " + RawSoilMoistureValue);
            Console.WriteLine ("");

            ConnectDevices (false);

            EnableMqtt ();

            SendMqttCalibrationCommand ();
        }

        public void SendMqttCalibrationCommand ()
        {      
            Mqtt.SendCommand (Key, RawSoilMoistureValue);

            // Skip some data
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