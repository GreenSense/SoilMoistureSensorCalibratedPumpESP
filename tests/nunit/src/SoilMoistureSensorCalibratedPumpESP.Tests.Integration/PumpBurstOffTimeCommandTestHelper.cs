using System;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class PumpBurstOffTimeCommandTestHelper : SerialCommandTestHelper
    {
        public int PumpBurstOffTime = 1;

        public void TestPumpBurstOffTimeCommand ()
        {
            Letter = "O";
            Value = PumpBurstOffTime;
            Label = "pump burst off time";

            TestCommand ();
        }

        /*public void TestPumpBurstOffTimeCommand ()
        {
            WriteTitleText ("Starting pump burst off time command test");

            Console.WriteLine ("Pump burst off time: " + PumpBurstOffTime);
            Console.WriteLine ("");

            WriteSubTitleText ("Preparing pump burst off time command test");

            ConnectDevices ();

            SendPumpBurstOffTimeCommand ();

            ResetAndCheckPumpBurstOffTimeIsPreserved ();
        }

        public void SendPumpBurstOffTimeCommand ()
        {
            WriteParagraphTitleText ("Sending pump burst off time command...");

            var command = "O" + PumpBurstOffTime;

            SendDeviceCommand (command);

            WriteParagraphTitleText ("Checking pump burst off time value...");

            var dataEntry = WaitForDataEntry ();

            AssertDataValueEquals (dataEntry, "O", PumpBurstOffTime);
        }

        public void ResetAndCheckPumpBurstOffTimeIsPreserved ()
        {
            ResetDeviceViaPin ();

            WriteParagraphTitleText ("Checking pump burst off time value...");

            var dataEntry = WaitForDataEntry ();

            AssertDataValueEquals (dataEntry, "O", PumpBurstOffTime);
        }*/
    }
}