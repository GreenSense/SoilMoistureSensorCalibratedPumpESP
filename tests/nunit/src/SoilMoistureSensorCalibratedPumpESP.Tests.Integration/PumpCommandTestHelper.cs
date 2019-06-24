using System;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class PumpCommandTestHelper : SerialCommandTestHelper
    {
        public PumpMode PumpCommand = PumpMode.Auto;

        public void TestPumpCommand ()
        {
            Key = "P";
            Value = ((int)PumpCommand).ToString ();
            Label = "pump mode";
            ValueIsSavedInEEPROM = false;

            TestCommand ();
        }
    }
}