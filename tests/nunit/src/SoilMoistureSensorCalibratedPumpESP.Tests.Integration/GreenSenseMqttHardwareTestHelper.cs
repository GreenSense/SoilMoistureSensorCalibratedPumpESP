using System;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class GreenSenseMqttHardwareTestHelper : GreenSenseHardwareTestHelper
    {
        public MqttTestHelper Mqtt;

        public string ConnectedToMqttText = "Subscribed to MQTT topics";

        public bool RequireMqttConnection = false;

        public GreenSenseMqttHardwareTestHelper ()
        {
        }

        public void EnableMqtt ()
        {
            RequireMqttConnection = true;

            Mqtt = new MqttTestHelper (this);
            Mqtt.Start ();

            if (!FullDeviceOutput.Contains (ConnectedToMqttText))
                WaitForText (ConnectedToMqttText);
        }

        public void EnableMqtt (string deviceName)
        {
            Mqtt = new MqttTestHelper (this, deviceName);
            Mqtt.Start ();

            if (!FullDeviceOutput.Contains (ConnectedToMqttText))
                WaitForText (ConnectedToMqttText);
        }

        public override void PrepareDeviceForTest (bool consoleWriteDeviceOutput)
        {
            if (RequireMqttConnection)
                TextToWaitForBeforeTest = ConnectedToMqttText;

            base.PrepareDeviceForTest (consoleWriteDeviceOutput);

        }
    }
}
