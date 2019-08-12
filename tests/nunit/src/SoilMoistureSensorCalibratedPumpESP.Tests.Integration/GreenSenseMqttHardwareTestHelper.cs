using System;
using System.IO;

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

            SetWiFiSettings ();

            SetMqttSettings ();

            base.PrepareDeviceForTest (consoleWriteDeviceOutput);

        }

        public void SetWiFiSettings ()
        {
            var wiFiName = File.ReadAllText (Path.GetFullPath ("../../../../wifi-name.security")).Trim ();
            SendDeviceCommand ("WN:" + wiFiName);
            var wiFiPassword = File.ReadAllText (Path.GetFullPath ("../../../../wifi-password.security")).Trim ();
            SendDeviceCommand ("WPass:" + wiFiPassword);
        }

        public void SetMqttSettings ()
        {
            var mqttHost = File.ReadAllText (Path.GetFullPath ("../../../../mqtt-host.security")).Trim ();
            SendDeviceCommand ("MHost:" + mqttHost);
            var mqttUsername = File.ReadAllText (Path.GetFullPath ("../../../../mqtt-username.security")).Trim ();
            SendDeviceCommand ("MUser:" + mqttUsername);
            var mqttPassword = File.ReadAllText (Path.GetFullPath ("../../../../mqtt-password.security")).Trim ();
            SendDeviceCommand ("MPass:" + mqttPassword);
            var mqttPort = File.ReadAllText (Path.GetFullPath ("../../../../mqtt-port.security")).Trim ();
            SendDeviceCommand ("MPort:" + mqttPort);
        }

    }
}
