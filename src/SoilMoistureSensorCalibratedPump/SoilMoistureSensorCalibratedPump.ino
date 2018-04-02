#include <Arduino.h>
#include <EEPROM.h>
#include <duinocom.h>

#include "Common.h"
#include "SoilMoistureSensor.h"
#include "Irrigation.h"

#define SERIAL_MODE_CSV 1
#define SERIAL_MODE_QUERYSTRING 2

#define VERSION "1-0-0-0"

int serialMode = SERIAL_MODE_CSV;

long loopNumber = 0;

void setup()
{
  Serial.begin(9600);

  if (isDebugMode)
  {
    Serial.println("Starting irrigator");
  }

  for (int i = 0; i < EEPROM.length();i++)
  {
    EEPROM.write(i, 0);
  }

  setupSoilMoistureSensor();

  setupIrrigation();

  serialOutputInterval = soilMoistureSensorReadingInterval;

}

void loop()
{
// Disabled. Used for debugging
//  Serial.print(".");

  loopNumber++;

  if (isDebugMode)
  {
    Serial.println("==============================");
    Serial.print("===== Start Loop: ");
    Serial.println(loopNumber);
    Serial.println("==============================");
  }

  checkCommand();

  takeSoilMoistureSensorReading();

  serialPrintData();

  irrigateIfNeeded();

  if (isDebugMode)
  {
    Serial.println("==============================");
    Serial.print("===== End Loop: ");
    Serial.println(loopNumber);
    Serial.println("==============================");
    Serial.println("");
    Serial.println("");
  }

  delay(1);
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
  bool isTimeToPrintData = lastSerialOutputTime + secondsToMilliseconds(serialOutputInterval) < millis()
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
      Serial.print(soilMoistureSensorReadingInterval);
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
    /*else
    {
      Serial.print("raw=");
      Serial.print(soilMoistureLevelRaw);
      Serial.print("&");
      Serial.print("calibrated=");
      Serial.print(soilMoistureLevelCalibrated);
      Serial.print("&");
      Serial.print("threshold=");
      Serial.print(threshold);
      Serial.print("&");
      Serial.print("waterNeeded=");
      Serial.print(soilMoistureLevelCalibrated < threshold);
      Serial.print("&");
      Serial.print("pumpStatus=");
      Serial.print(pumpStatus);
      Serial.print("&");
      Serial.print("readingInterval=");
      Serial.print(soilMoistureSensorReadingInterval);
      Serial.print("&");
      Serial.print("pumpBurstOnTime=");
      Serial.print(pumpBurstOnTime);
      Serial.print("&");
      Serial.print("pumpBurstOffTime=");
      Serial.print(pumpBurstOffTime);
      Serial.print("&");
      Serial.print("pumpOn=");
      Serial.print(pumpIsOn);
      Serial.print("&");
      Serial.print("secondsSincePumpOn=");
      Serial.print((millis() - lastPumpFinishTime) / 1000);
      Serial.print("&");
      Serial.print("dry=");
      Serial.print(drySoilMoistureCalibrationValue);
      Serial.print("&");
      Serial.print("wet=");
      Serial.print(wetSoilMoistureCalibrationValue);
      Serial.print(";;");
      Serial.println();
    }*/

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

