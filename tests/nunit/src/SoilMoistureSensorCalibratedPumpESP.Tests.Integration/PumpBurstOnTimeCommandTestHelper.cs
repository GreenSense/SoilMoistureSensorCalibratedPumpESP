using System;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class PumpBurstOnTimeCommandTestHelper : SerialCommandTestHelper
    {
        public int PumpBurstOnTime = 1;

        public void TestPumpBurstOnTimeCommand ()
        {
            Letter = "O";
            Value = PumpBurstOnTime;
            Label = "pump burst on time";

            TestCommand ();
        }

        /*public void TestPumpBurstOnTimeCommand ()
        {
            WriteTitleText ("Starting pump burst on time command test");

            Console.WriteLine ("Pump burst on time: " + PumpBurstOnTime);
            Console.WriteLine ("");

            ConnectDevices (false);

            var cmd = "B" + PumpBurstOnTime;

            SendDeviceCommand (cmd);

            var dataEntry = WaitForDataEntry ();

            AssertDataValueEquals (dataEntry, "B", PumpBurstOnTime);
        }*/

        /* public void SendPumpBurstOffTimeCommand ()
        {
            WriteParagraphTitleText ("Sending pump burst on time command...");

            var command = "B" + PumpBurstOnTime;

            SendDeviceCommand (command);

            WriteParagraphTitleText ("Checking pump burst on time value...");

            var dataEntry = WaitForDataEntry ();

            AssertDataValueEquals (dataEntry, "B", PumpBurstOnTime);
        }

        public void ResetAndCheckPumpBurstOffTimeIsPreserved ()
        {
            ResetDeviceViaPin ();

            WriteParagraphTitleText ("Checking pump burst on time value...");

            var dataEntry = WaitForDataEntry ();

            AssertDataValueEquals (dataEntry, "O", PumpBurstOffTime);
        }*/
    }
}