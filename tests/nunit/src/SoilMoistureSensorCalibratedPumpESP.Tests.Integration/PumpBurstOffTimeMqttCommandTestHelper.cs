using System;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class PumpBurstOffTimeMqttCommandTestHelper : GreenSenseIrrigatorHardwareTestHelper
    {
        public int PumpBurstOffTime = 1;

        public void TestPumpBurstOffTimeCommand ()
        {
            WriteTitleText ("Starting pump burst off time command test");

            Console.WriteLine ("Pump burst off time: " + PumpBurstOffTime);
            Console.WriteLine ("");

            RequireMqttConnection = true;

            ConnectDevices ();

            EnableMqtt ();

            Mqtt.SendCommand ("O", PumpBurstOffTime);

            // Skip some data
            WaitForData (1);

            var dataEntry = WaitForDataEntry ();

            AssertDataValueEquals (dataEntry, "O", PumpBurstOffTime);
        }
    }
}