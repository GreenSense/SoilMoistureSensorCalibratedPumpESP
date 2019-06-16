using System;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class PumpBurstOnTimeMqttCommandTestHelper : GreenSenseIrrigatorHardwareTestHelper
    {
        public int PumpBurstOnTime = 1;

        public void TestPumpBurstOnTimeCommand ()
        {
            WriteTitleText ("Starting pump burst on time command test");

            Console.WriteLine ("Pump burst on time: " + PumpBurstOnTime);
            Console.WriteLine ("");

            ConnectDevices (false);

            EnableMqtt ();

            Mqtt.SendCommand ("B", PumpBurstOnTime);

            Console.WriteLine ("Skipping the next data entry in case it's out of date...");

            WaitForDataEntry ();

            var dataEntry = WaitForDataEntry ();

            AssertDataValueEquals (dataEntry, "B", PumpBurstOnTime);
        }
    }
}