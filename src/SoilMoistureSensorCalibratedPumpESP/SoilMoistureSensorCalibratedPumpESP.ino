#include <Arduino.h>
#include <EEPROM.h>
#include <stdlib.h>

#include <ESP8266WiFi.h>
#include <PubSubClient.h>

#include <NTPClient.h>
#include <WiFiUdp.h>

#include <duinocom.h>

#include "Common.h"
#include "SoilMoistureSensor.h"
#include "Irrigation.h"

#define NTP_OFFSET   60 * 60      // In seconds
#define NTP_INTERVAL 60 * 1000    // In miliseconds
#define NTP_ADDRESS  "europe.pool.ntp.org"

WiFiUDP ntpUDP;
NTPClient timeClient(ntpUDP, NTP_ADDRESS, NTP_OFFSET, NTP_INTERVAL);

#define SERIAL_MODE_CSV 1
#define SERIAL_MODE_QUERYSTRING 2

#define VERSION "1-0-0-0"

int serialMode = SERIAL_MODE_CSV;

#define WIFI_NAME "Telstra995578"
#define WIFI_PASSWORD "3ym9j8hrwhcf"

#define MQTT_HOST "10.0.0.93"
#define MQTT_PORT 1883
#define MQTT_USERNAME "j"
#define MQTT_PASSWORD "ywgtpJ8gdnm!"
#define MQTT_DEVICE_NAME "WiFiIrrigator1"

int totalSubscribeTopics = 7;
String subscribeTopics[] = {"D", "W", "T", "V", "P", "B", "O"};

WiFiClient espClient;
PubSubClient client(espClient);

void setup()
{
  Serial.begin(115200);

  if (isDebugMode)
  {
    Serial.println("Starting WiFi irrigator");
  }

  setupWiFi();

  setupSoilMoistureSensor();

  setupIrrigation();

  serialOutputIntervalInSeconds = soilMoistureSensorReadingIntervalInSeconds;

  timeClient.begin();
}

void setupWiFi()
{
  WiFi.begin(WIFI_NAME, WIFI_PASSWORD);
   
  Serial.print("Connecting to WiFi..");
  while (WiFi.status() != WL_CONNECTED) {
    delay(250);
    Serial.print(".");
  }

  Serial.println();

  Serial.println("Connected to the WiFi network");

  setupMqtt();
}

void setupMqtt()
{
  client.setServer(MQTT_HOST, MQTT_PORT);

  client.setCallback(callback);

  while (!client.connected()) {
    Serial.println("Connecting to MQTT...");
    Serial.print("Device name: ");
    Serial.println(MQTT_DEVICE_NAME);
    Serial.print("MQTT Username: ");
    Serial.println(MQTT_USERNAME);
 
    if (client.connect(MQTT_DEVICE_NAME, MQTT_USERNAME, MQTT_PASSWORD )) {
 
      Serial.println("connected");  
 
    } else {
 
      Serial.print("failed with state ");
      Serial.print(client.state());
      delay(2000);
 
    }
  }

  setupMqttSubscriptions();
}

void setupMqttSubscriptions()
{
  Serial.println("Setting up subscriptions...");

  String baseTopic = "/";
  baseTopic += MQTT_DEVICE_NAME;
  baseTopic += "/";

  for (int i = 0; i < totalSubscribeTopics; i++)
  {
    String topic = baseTopic + subscribeTopics[i] + "/in";

    Serial.println(topic);

    client.subscribe(topic.c_str());
  }

  Serial.println();
}

void callback(char* topic, byte* payload, unsigned int length) {
 
  if (isDebugMode)
  {
    Serial.print("Message arrived in topic: ");
    Serial.println(topic);
  }

  String valueString = "";

  if (isDebugMode) 
    Serial.print("Message:");

  for (int i = 0; i < length; i++) {
    valueString += (char)payload[i]; 
    if (isDebugMode)
      Serial.print((char)payload[i]);
  }

  Serial.println();

  String prefix = "/";
  prefix += MQTT_DEVICE_NAME;
  prefix += "/";

  String postFix = "/in";

  String topicString = (String)topic;

  String subTopic = topicString;
  subTopic.replace(prefix, "");
  subTopic.replace(postFix, "");

  if (isDebugMode)
  {
    Serial.println("Subtopic");
    Serial.println(subTopic);
    Serial.println("Value");
    Serial.println(valueString);
  }

  String msgString = subTopic + valueString;
  Serial.println("Msg");
  Serial.println(msgString);

  char msg[msgString.length()+1];
  msgString.toCharArray(msg, msgString.length()+1);
  handleCommand(msg);

 
  Serial.println();
  Serial.println("---");
 
}


void loop()
{
// Disabled. Used for debugging
//  Serial.print(".");

  loopNumber++;

  serialPrintLoopHeader();
  
  timeClient.update();

  loopWiFi();

  checkCommand();

  takeSoilMoistureSensorReading();

  serialPrintData();

  mqttPublishData();

  irrigateIfNeeded();

  // Reset flag for this loop
  soilMoistureSensorReadingHasBeenTaken = false;

  serialPrintLoopFooter();
}

void loopWiFi()
{
  client.loop();
}

/* MQTT Publish */
void mqttPublishData()
{
  if (soilMoistureSensorReadingHasBeenTaken)
  {
    if (isDebugMode)
      Serial.println("Publishing");
    publishMqttValue("R", soilMoistureLevelRaw);
    publishMqttValue("C", soilMoistureLevelCalibrated);
    publishMqttValue("T", threshold);
    publishMqttValue("P", pumpStatus);
    publishMqttValue("B", pumpBurstOnTime);
    publishMqttValue("O", pumpBurstOffTime);
    publishMqttValue("V", soilMoistureSensorReadingIntervalInSeconds);
    publishMqttValue("WN", soilMoistureLevelCalibrated < threshold);
    publishMqttValue("PO", pumpIsOn);
    publishMqttValue("D", drySoilMoistureCalibrationValue);
    publishMqttValue("W", wetSoilMoistureCalibrationValue);
    publishMqttValue("Z", VERSION);
    publishMqttValue("Time", timeClient.getFormattedTime());
    publishMqttPush(soilMoistureLevelCalibrated);
  }
}

void publishMqttValue(char* subTopic, int value)
{
  char valueString[16];
  itoa(value, valueString, 10);

  publishMqttValue(subTopic, valueString);

}

void publishMqttValue(char* subTopic, char* value)
{
  String topic = "/";
  topic += MQTT_DEVICE_NAME;
  topic += "/";
  topic += subTopic;

  client.publish(topic.c_str(), value);

}

void publishMqttValue(char* subTopic, String value)
{
  String topic = "/";
  topic += MQTT_DEVICE_NAME;
  topic += "/";
  topic += subTopic;
  
  char valueArray[16];
  value.toCharArray(valueArray, 12);

  client.publish(topic.c_str(), valueArray);

}

void publishMqttPush(int soilMoistureValue)
{
  String topic = "/push/";
  topic += MQTT_DEVICE_NAME;

  char valueString[16];
  itoa(soilMoistureValue, valueString, 10);
  
  client.publish(topic.c_str(), valueString);

}

/* Commands */
void checkCommand()
{
  if (isDebugMode)
  {
    Serial.println("Checking incoming serial commands");
  }

  if (checkMsgReady())
  {
    char* msg = getMsg();
        
    char letter = msg[0];

    int length = strlen(msg);

//    Serial.print("Received message: ");
    //Serial.println(msg);

    handleCommand(msg);
  }
  delay(1);
}

void handleCommand(char* msg)
{
  char letter = msg[0];

  int length = strlen(msg);

  Serial.print("Received message: ");
  Serial.println(msg);

  switch (letter)
  {
    case 'P':
      setPumpStatus(msg);
      break;
    case 'T':
      setThreshold(msg);
      break;
    case 'D':
      setDrySoilMoistureCalibrationValue(msg);
      break;
    case 'W':
      setWetSoilMoistureCalibrationValue(msg);
      break;
    case 'V':
      setSoilMoistureSensorReadingInterval(msg);
    case 'B':
      setPumpBurstOnTime(msg);
      break;
    case 'O':
      setPumpBurstOffTime(msg);
      break;
    case 'X':
      restoreDefaultSettings();
      break;
    case 'N':
      Serial.println("Turning pump on");
      pumpStatus = PUMP_STATUS_ON;
      pumpOn();
      break;
    case 'F':
      Serial.println("Turning pump off");
      pumpStatus = PUMP_STATUS_OFF;
      pumpOff();
      break;
    case 'A':
      Serial.println("Turning pump to auto");
      pumpStatus = PUMP_STATUS_AUTO;
      irrigateIfNeeded();
      break;
    case 'Z':
      Serial.println("Toggling IsDebug");
      isDebugMode = !isDebugMode;
      break;
    case 'R':
      reverseSoilMoistureCalibrationValues();
      break;
  }
  
  forceSerialOutput();
}

/* Settings */
void restoreDefaultSettings()
{
  Serial.println("Restoring default settings");

  restoreDefaultSoilMoistureSensorSettings();
  restoreDefaultIrrigationSettings();
}

/* Serial Output */
void serialPrintData()
{
  bool isTimeToPrintData = lastSerialOutputTime + secondsToMilliseconds(serialOutputIntervalInSeconds) < millis()
      || lastSerialOutputTime == 0;

  bool isReadyToPrintData = isTimeToPrintData && soilMoistureSensorReadingHasBeenTaken;

  if (isReadyToPrintData)
  {
    if (isDebugMode)
    {
      Serial.println("Printing serial data");
    }

    if (serialMode == SERIAL_MODE_CSV)
    {
      Serial.print("D;"); // This prefix indicates that the line contains data.
      Serial.print("R:");
      Serial.print(soilMoistureLevelRaw);
      Serial.print(";");
      Serial.print("C:");
      Serial.print(soilMoistureLevelCalibrated);
      Serial.print(";");
      Serial.print("T:");
      Serial.print(threshold);
      Serial.print(";");
      Serial.print("P:");
      Serial.print(pumpStatus);
      Serial.print(";");
      Serial.print("V:");
      Serial.print(soilMoistureSensorReadingIntervalInSeconds);
      Serial.print(";");
      Serial.print("B:");
      Serial.print(pumpBurstOnTime);
      Serial.print(";");
      Serial.print("O:");
      Serial.print(pumpBurstOffTime);
      Serial.print(";");
      Serial.print("WN:"); // Water needed
      Serial.print(soilMoistureLevelCalibrated < threshold);
      Serial.print(";");
      Serial.print("PO:"); // Pump on
      Serial.print(pumpIsOn);
      Serial.print(";");
      //Serial.print("SSPO:"); // Seconds since pump on
      //Serial.print((millis() - lastPumpFinishTime) / 1000);
      //Serial.print(";");
      Serial.print("D:"); // Dry calibration value
      Serial.print(drySoilMoistureCalibrationValue);
      Serial.print(";");
      Serial.print("W:"); // Wet calibration value
      Serial.print(wetSoilMoistureCalibrationValue);
      Serial.print(";");
      Serial.print("Z:");
      Serial.print(VERSION);
      Serial.print(";;");
      Serial.println();
    }

    if (isDebugMode)
    {
      Serial.print("Last pump start time:");
      Serial.println(pumpStartTime);
      Serial.print("Last pump finish time:");
      Serial.println(lastPumpFinishTime);
    }

    lastSerialOutputTime = millis();
  }
}

