using System;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class PumpCommandTestHelper : GreenSenseIrrigatorHardwareTestHelper
    {
        public PumpStatus PumpCommand = PumpStatus.Auto;

        public void TestPumpCommand ()
        {
            WriteTitleText ("Starting pump command test");

            Console.WriteLine ("Pump command: " + PumpCommand);
            Console.WriteLine ("");

            ConnectDevices (false);

            EnableMqtt ();

            Mqtt.SendCommand ("P", (int)PumpCommand);

            // Skip the first few entries in case they're out of date
            WaitForData (1);

            var dataEntry = WaitForDataEntry ();
            AssertDataValueEquals (dataEntry, "P", (int)PumpCommand);
        }
    }
}