using System;
using System.Threading;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class CalibrateCommandTestHelper : SerialCommandTestHelper
    {
        public int RawSoilMoistureValue = 0;

        public void TestCalibrateCommand ()
        {
            Value = RawSoilMoistureValue.ToString ();
            TestCommand ();
        }
    }
}