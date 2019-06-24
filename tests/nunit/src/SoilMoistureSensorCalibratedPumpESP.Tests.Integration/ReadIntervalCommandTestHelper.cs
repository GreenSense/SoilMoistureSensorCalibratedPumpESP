using System;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class ReadIntervalCommandTestHelper : SerialCommandTestHelper
    {
        public int ReadingInterval = 3;

        public void TestSetReadIntervalCommand ()
        {
            Key = "I";
            Value = ReadingInterval.ToString ();
            Label = "reading interval";

            TestCommand ();
        }
    }
}
