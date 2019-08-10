using System;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class PumpMqttCommandTestHelper : GreenSenseIrrigatorHardwareTestHelper
    {
        public PumpMode PumpCommand = PumpMode.Auto;

        public void TestPumpCommand ()
        {
            WriteTitleText ("Starting pump command test");

            Console.WriteLine ("Pump command: " + PumpCommand);
            Console.WriteLine ("");

            ConnectDevices ();

            EnableMqtt ();

            Mqtt.SendCommand ("P", (int)PumpCommand);

            WaitForData (2);

            var dataEntry = WaitForDataEntry ();
            AssertDataValueEquals (dataEntry, "P", (int)PumpCommand);
        }
    }
}