using System;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    // TODO: Remove if not needed. Should be obsolete. It's been merged with the ReadIntervalCommandTestHelper
    public class ReadIntervalEEPROMTestHelper : GreenSenseIrrigatorHardwareTestHelper
    {
        public int ReadInterval = 3;

        public void TestReadIntervalEEPROM ()
        {
            WriteTitleText ("Starting read interval EEPROM test");

            Console.WriteLine ("Read interval: " + ReadInterval + "%");
            Console.WriteLine ("");

            ConnectDevices ();

            ResetDeviceSettings ();

            SendReadIntervalCommand ();

            ResetDeviceViaPin ();

            var dataEntry = WaitForDataEntry ();

            AssertDataValueEquals (dataEntry, "I", ReadInterval);
        }

        public void SendReadIntervalCommand ()
        {
            var command = "I" + ReadInterval;

            WriteParagraphTitleText ("Sending read interval command...");

            SendDeviceCommand (command);

            var dataEntry = WaitForDataEntry ();

            WriteParagraphTitleText ("Checking read interval value...");

            AssertDataValueEquals (dataEntry, "I", ReadInterval);
        }
    }
}
