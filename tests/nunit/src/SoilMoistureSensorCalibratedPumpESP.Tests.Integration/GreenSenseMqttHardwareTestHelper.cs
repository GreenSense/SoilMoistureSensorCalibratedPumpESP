using System;

namespace SoilMoistureSensorCalibratedPumpESP.Tests.Integration
{
    public class GreenSenseMqttHardwareTestHelper : GreenSenseHardwareTestHelper
    {
        public MqttTestHelper Mqtt;

        public string ConnectedToMqttText = "Connected to MQTT";

        public GreenSenseMqttHardwareTestHelper ()
        {
        }

        public void EnableMqtt ()
        {
            Mqtt = new MqttTestHelper ();
            Mqtt.Start ();

            if (!FullDeviceOutput.Contains (ConnectedToMqttText))
                WaitForText (ConnectedToMqttText);
        }

        public void EnableMqtt (string deviceName)
        {
            Mqtt = new MqttTestHelper (deviceName);
            Mqtt.Start ();

            if (!FullDeviceOutput.Contains (ConnectedToMqttText))
                WaitForText (ConnectedToMqttText);
        }

    }
}
