using System;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class PumpCommandTestHelper : SerialCommandTestHelper
    {
        public PumpMode PumpCommand = PumpMode.Auto;

        public void TestPumpCommand ()
        {
            Letter = "P";
            Value = (int)PumpCommand;
            Label = "pump mode";
            ValueIsSavedInEEPROM = false;

            TestCommand ();

            /*WriteTitleText ("Starting pump command test");

            Console.WriteLine ("Pump command: " + PumpCommand);
            Console.WriteLine ("");

            ConnectDevices (false);

            EnableMqtt ();

            Mqtt.SendCommand ("P", (int)PumpCommand);

            // Skip the first few entries in case they're out of date
            WaitForData (1);

            var dataEntry = WaitForDataEntry ();
            AssertDataValueEquals (dataEntry, "P", (int)PumpCommand);*/
        }
    }
}