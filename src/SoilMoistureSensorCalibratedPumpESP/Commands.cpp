#include <Arduino.h>
#include <EEPROM.h>

#include "Commands.h"
#include "Irrigation.h"

void checkCommand()
{
  /*if (isDebugMode)
  {
    Serial.println("Checking incoming serial commands");
  }*/

  if (checkMsgReady())
  {
    char* msg = getMsg();
       
    handleCommand(msg);
  }
}

void handleCommand(char* msg)
{
  if (isDebugMode)
  {
    Serial.println("");
    Serial.println("Handling command...");  
  }

  Serial.print("Received message: ");
  Serial.println(msg);

  if (isKeyValue(msg))
  {
    Serial.println("  Is key value");
    
    char* key = getKey(msg);
    
    Serial.print("  Key: \"");
    Serial.print(key);
    Serial.println("\"");
    
    char* value = getValue(msg);
    
    Serial.print("  Value: \"");
    Serial.print(value);
    Serial.println("\"");

    if (strcmp(key, "WN") == 0)
    {
      if (isDebugMode)
        Serial.println("  Set WiFi network command");
      setWiFiNetwork(value);
    }
    else if (strcmp(key, "WPass") == 0)
    {
      if (isDebugMode)
        Serial.println("  Set WiFi password command");
      setWiFiPassword(value);
    }
    else if (strcmp(key, "MHost") == 0)
    {
      if (isDebugMode)
        Serial.println("  Set MQTT host command");
      setMqttHost(value);
    }
    else if (strcmp(key, "MUser") == 0)
    {
      if (isDebugMode)
        Serial.println("  Set MQTT username command");
      setMqttUsername(value);
    }
    else if (strcmp(key, "MPass") == 0)
    {
      if (isDebugMode)
        Serial.println("  Set MQTT password command");
      setMqttPassword(value);
    }
    else if (strcmp(key, "MPort") == 0)
    {
      if (isDebugMode)
        Serial.println("  Set MQTT port command");
      setMqttPort(value);
    }
    else if (strcmp(key, "MDevice") == 0)
    {
      if (isDebugMode)
        Serial.println("  Set MQTT device name");
      setMqttDeviceName(value);
    }
  }
  else
  {
    char letter = msg[0];

    int length = strlen(msg);

    Serial.print("Received message: ");
    Serial.println(msg);

    switch (letter)
    {
      case '#':
        serialPrintDeviceInfo();
        break;
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
      case 'I':
        setSoilMoistureSensorReadingInterval(msg);
        break;
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
  }
  
  forceSerialOutput();
  forceMqttOutput();
  
  if (isDebugMode)
  {
    Serial.println("");
  }
}

void restoreDefaultSettings()
{
  Serial.println("Restoring default settings");

  restoreDefaultSoilMoistureSensorSettings();
  
  EEPROMReset();
}

