using System;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class ReadIntervalCommandTestHelper : SerialCommandTestHelper
    {
        public int ReadingInterval = 3;

        public ReadIntervalCommandTestHelper ()
        {
        }

        public void TestSetReadIntervalCommand ()
        {
            Letter = "I";
            Value = ReadingInterval;
            Label = "reading interval";

            TestCommand ();

            // TODO: Remove if not needed
            /*WriteTitleText ("Starting read interval command test");

            Console.WriteLine ("Read interval: " + ReadInterval);

            WriteSubTitleText ("Preparing test");

            ConnectDevices ();

            WriteSubTitleText ("Testing read interval command");

            SetDeviceReadInterval (ReadInterval);

            var dataEntry = WaitForDataEntry ();

            Console.WriteLine ("Checking read interval was set...");

            AssertDataValueEquals (dataEntry, "I", ReadInterval);

            WriteSubTitleText ("Testing EEPROM preservation of read interval interval setting");

            ResetDeviceViaPin ();

            dataEntry = WaitForDataEntry ();

            Console.WriteLine ("Checking read interval was preserved...");

            AssertDataValueEquals (dataEntry, "I", ReadInterval);*/
        }
    }
}
