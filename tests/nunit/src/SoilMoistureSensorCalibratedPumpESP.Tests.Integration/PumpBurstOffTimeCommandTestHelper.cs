using System;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class PumpBurstOffTimeCommandTestHelper : SerialCommandTestHelper
    {
        public int PumpBurstOffTime = 1;

        public void TestPumpBurstOffTimeCommand ()
        {
            Key = "O";
            Value = PumpBurstOffTime.ToString ();
            Label = "pump burst off time";

            TestCommand ();
        }
    }
}