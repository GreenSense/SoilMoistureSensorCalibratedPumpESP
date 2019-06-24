using System;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class ThresholdMqttCommandTestHelper : GreenSenseIrrigatorHardwareTestHelper
    {
        public int Threshold = 30;

        public void TestThresholdCommand ()
        {
            WriteTitleText ("Starting threshold command test");

            Console.WriteLine ("Threshold: " + Threshold + "%");
            Console.WriteLine ("");

            ConnectDevices (false);

            EnableMqtt ();

            SendThresholdCommand ();
        }

        public void SendThresholdCommand ()
        {
            WriteParagraphTitleText ("Sending threshold command...");

            Mqtt.SendCommand ("T", Threshold);

            Console.WriteLine ("Skipping the next data entries in case they're out of date...");

            WaitForData (2);

            var dataEntry = WaitForDataEntry ();

            WriteParagraphTitleText ("Checking threshold value...");

            AssertDataValueEquals (dataEntry, "T", Threshold);
        }
    }
}
