#include <Arduino.h>
#include <EEPROM.h>
#include <duinocom.h>

#include "Common.h"
#include "SoilMoistureSensor.h"

#define pumpPin 11

int threshold = 30;

bool pumpIsOn = 0;
long pumpStartTime = 0;
long lastPumpFinishTime = 0;
int pumpBurstDuration = 5 * 1000;
long pumpWaitOffDuration = 5 * 1000;

#define PUMP_STATUS_OFF 0
#define PUMP_STATUS_ON 1
#define PUMP_STATUS_AUTO 2

int pumpStatus = PUMP_STATUS_AUTO;

long lastSerialOutputTime = 0;
long serialOutputInterval = 3 * 1000;

#define SERIAL_MODE_CSV 1
#define SERIAL_MODE_QUERYSTRING 2

int serialMode = SERIAL_MODE_CSV;

int thresholdIsSetEEPROMFlagAddress = 10;
int thresholdEEPROMAddress = thresholdIsSetEEPROMFlagAddress+1;

int loopNumber = 0;

void setup()
{
  Serial.begin(9600);

  Serial.println("Starting irrigator");

  pinMode(pumpPin, OUTPUT);

  setupSoilMoistureSensor();
}

void loop()
{
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

    Serial.print("Received message: ");
    Serial.println(msg);

    switch (letter)
    {
      case 'T':
        setThreshold(msg);
        break;
      case 'D':
        setDrySoilMoistureCalibrationValue(msg);
        break;
      case 'W':
        setWetSoilMoistureCalibrationValue(msg);
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
}

/* Settings */
void restoreDefaultSettings()
{
  Serial.println("Restoring default settings");

  restoreDefaultCalibrationSettings();
}

/* Serial Output */
void serialPrintData()
{
  bool isTimeToPrintData = lastSerialOutputTime + serialOutputInterval < millis()
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
      Serial.print("WN:"); // Water needed
      Serial.print(soilMoistureLevelCalibrated < threshold);
      Serial.print(";");
      Serial.print("PO:"); // Pump on
      Serial.print(pumpIsOn);
      Serial.print(";");
      Serial.print("SSPO:"); // Seconds since pump on
      Serial.print((millis() - lastPumpFinishTime) / 1000);
      Serial.print(";");
      Serial.print("D:"); // Dry calibration value
      Serial.print(drySoilMoistureCalibrationValue);
      Serial.print(";");
      Serial.print("W:"); // Wet calibration value
      Serial.print(wetSoilMoistureCalibrationValue);
      Serial.print(";");
      Serial.println();
    }
    else
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

/* Irrigation */
void irrigateIfNeeded()
{
  if (isDebugMode)
  {
    Serial.println("Irrigating (if needed)");
  }

  if (pumpStatus == PUMP_STATUS_AUTO)
  {
    bool readingHasBeenTaken = lastSoilMoistureSensorReadingTime > 0;
    bool pumpBurstFinished = pumpStartTime + pumpBurstDuration < millis();
    bool waterIsNeeded = soilMoistureLevelCalibrated <= threshold && readingHasBeenTaken;
    bool pumpIsReady = lastPumpFinishTime + pumpWaitOffDuration < millis() || lastPumpFinishTime == 0;

    if (pumpIsOn)
    {
      if (pumpBurstFinished)
      {
        if (isDebugMode)
          Serial.println("  Pump burst finished");
        pumpOff();
      }
    }
    else if (waterIsNeeded && pumpIsReady)
    {
      if (isDebugMode)
        Serial.println("  Pump is turning on");
      pumpOn();
    }
  }
  else
  {
    Serial.println("  Pump has been manually switched off. Switch to auto to enable irrigation again.");
  }
}

void pumpOn()
{
  digitalWrite(pumpPin, HIGH);
  pumpIsOn = true;

  pumpStartTime = millis();
}

void pumpOff()
{
  digitalWrite(pumpPin, LOW);
  pumpIsOn = false;

  lastPumpFinishTime = millis();
}

void setThreshold(char* msg)
{
  int length = strlen(msg);

  if (length == 1)
    setThresholdToCurrent();
  else
  {
    int value = readInt(msg, 1, length-1);

//    Serial.println("Value:");
//    Serial.println(value);

    setThreshold(value);
  }
}

void setThreshold(int newThreshold)
{
  threshold = newThreshold;

  if (isDebugMode)
  {
    Serial.print("Setting threshold to EEPROM: ");
    Serial.println(threshold);
  }

  int compactValue = threshold / 4;

  EEPROM.write(thresholdEEPROMAddress, compactValue); // Must divide by 4 to make it fit in eeprom

  setThresholdIsSetEEPROMFlag();
}

void setThresholdToCurrent()
{
  lastSoilMoistureSensorReadingTime = 0;
  takeSoilMoistureSensorReading();
  setThreshold(soilMoistureLevelCalibrated);
}

int getThreshold()
{
  int value = EEPROM.read(thresholdEEPROMAddress);

  if (value == 0
      || value == 255)
    return threshold;
  else
  {
    int threshold = value * 4; // Must multiply by 4 to get the original value

    if (isDebugMode)
    {
      Serial.print("Threshold found in EEPROM: ");
      Serial.println(threshold);
    }

    return threshold;
  }
}

void setThresholdIsSetEEPROMFlag()
{
  if (EEPROM.read(thresholdIsSetEEPROMFlagAddress) != 99)
    EEPROM.write(thresholdIsSetEEPROMFlagAddress, 99);
}
