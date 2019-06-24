using System;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class ThresholdCommandTestHelper : SerialCommandTestHelper
    {
        public int Threshold = 30;

        public void TestThresholdCommand ()
        {
            Label = "threshold";
            Key = "T";
            Value = Threshold.ToString ();

            TestCommand ();
        }
    }
}
