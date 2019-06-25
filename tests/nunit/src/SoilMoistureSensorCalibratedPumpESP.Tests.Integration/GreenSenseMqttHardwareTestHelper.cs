using System;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class GreenSenseMqttHardwareTestHelper : GreenSenseHardwareTestHelper
    {
        public MqttTestHelper Mqtt;

        public string ConnectedToMqttText = "Subscribed to MQTT topics";

        public GreenSenseMqttHardwareTestHelper ()
        {
        }

        public void EnableMqtt ()
        {
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

    }
}
