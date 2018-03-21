#include <Arduino.h>

#include <EEPROM.h>

#include <duinocom.h>

#include "Common.h"
#include "SoilMoistureSensor.h"
#include "Irrigation.h"

int threshold = 30;

bool pumpIsOn = 0;
long pumpStartTime = 0;
long lastPumpFinishTime = 0;
int pumpBurstOnTime = 3;
int pumpBurstOffTime = 5;

int pumpStatus = PUMP_STATUS_AUTO;

#define thresholdIsSetEEPROMFlagAddress 20
#define thresholdEEPROMAddress 21

#define pumpBurstOnTimeIsSetEEPROMFlagAddress 26
#define pumpBurstOnTimeEEPROMAddress 27

#define pumpBurstOffTimeIsSetEEPROMFlagAddress 33
#define pumpBurstOffTimeEEPROMAddress 34

/* Setup */
void setupIrrigation()
{
  pinMode(PUMP_PIN, OUTPUT);

  setupThreshold();

  setupPumpBurstOnTime();

  setupPumpBurstOffTime();
}

void setupThreshold()
{
  bool eepromIsSet = EEPROM.read(thresholdIsSetEEPROMFlagAddress) == 99;

  if (eepromIsSet)
  {
    if (isDebugMode)
    	Serial.println("EEPROM read interval value has been set. Loading.");

    threshold = getThreshold();
  }
  else
  {
    if (isDebugMode)
      Serial.println("EEPROM read interval value has not been set. Using defaults.");
    
    //setThreshold(threshold);
  }
}

void setupPumpBurstOnTime()
{
  bool eepromIsSet = EEPROM.read(pumpBurstOnTimeIsSetEEPROMFlagAddress) == 99;

  if (eepromIsSet)
  {
    if (isDebugMode)
    	Serial.println("EEPROM pump burst on time has been set. Loading.");

    pumpBurstOnTime = getPumpBurstOnTime();
  }
  else
  {
    if (isDebugMode)
      Serial.println("EEPROM pump burst on time has not been set. Using defaults.");
    
    //setPumpBurstOnTime(pumpBurstOnTime);
  }
}

void setupPumpBurstOffTime()
{
  bool eepromIsSet = EEPROM.read(pumpBurstOffTimeIsSetEEPROMFlagAddress) == 99;

  if (eepromIsSet)
  {
    if (isDebugMode)
    	Serial.println("EEPROM pump burst off time has been set. Loading.");

    pumpBurstOffTime = getPumpBurstOffTime();
  }
  /*else
  {
    if (isDebugMode)
      Serial.println("EEPROM pump burst off time has not been set. Using defaults.");
    
   // setPumpBurstOffTime(pumpBurstOffTime);
  }*/
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
    bool pumpBurstFinished = pumpStartTime + secondsToMilliseconds(pumpBurstOnTime) < millis();
    bool waterIsNeeded = soilMoistureLevelCalibrated <= threshold && readingHasBeenTaken;
    bool pumpIsReady = lastPumpFinishTime + secondsToMilliseconds(pumpBurstOffTime) < millis() || lastPumpFinishTime == 0;

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
  else if(pumpStatus == PUMP_STATUS_ON)
  {
    if (!pumpIsOn)
      pumpOn();
  }
  else
  {
    if (pumpIsOn)
      pumpOff();
  }
}

void pumpOn()
{
  digitalWrite(PUMP_PIN, HIGH);
  pumpIsOn = true;

  pumpStartTime = millis();
}

void pumpOff()
{
  digitalWrite(PUMP_PIN, LOW);
  pumpIsOn = false;

  lastPumpFinishTime = millis();
}

void setPumpStatus(char* msg)
{
  int length = strlen(msg);

  if (length != 2)
  {
    Serial.println("Invalid pump status:");
    printMsg(msg);
  }
  else
  {
    int value = readInt(msg, 1, 1);

//    Serial.println("Value:");
//    Serial.println(value);

    setPumpStatus(value);
  }
}

void setPumpStatus(int newStatus)
{
  pumpStatus = newStatus;
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

  EEPROM.write(thresholdEEPROMAddress, newThreshold); // Must divide by 4 to make it fit in eeprom

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

  if (value >= 0
      || value >= 100)
    return threshold;
  else
  {
    int threshold = value; // Must multiply by 4 to get the original value

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


void setPumpBurstOnTime(char* msg)
{
  int length = strlen(msg);

  if (length >= 2)
  {
    long value = readInt(msg, 1, length-1);

//    Serial.println("Value:");
//    Serial.println(value);

    setPumpBurstOnTime(value);
  }
}

void setPumpBurstOnTime(long newPumpBurstOnTime)
{
  pumpBurstOnTime = newPumpBurstOnTime;

  if (isDebugMode)
  {
    Serial.print("Setting pumpBurstOnTime to EEPROM: ");
    Serial.println(pumpBurstOnTime);
  }

  EEPROMWriteLong(pumpBurstOnTimeEEPROMAddress, pumpBurstOnTime);

  setPumpBurstOnTimeIsSetEEPROMFlag();
}

long getPumpBurstOnTime()
{
  long value = EEPROMReadLong(pumpBurstOnTimeEEPROMAddress);

  if (value == 0
      || value == 255)
    return pumpBurstOnTime;
  else
  {
    int pumpBurstOnTime = value;

    if (isDebugMode)
    {
      Serial.print("PumpBurstOnTime found in EEPROM: ");
      Serial.println(pumpBurstOnTime);
    }

    return pumpBurstOnTime;
  }
}

void setPumpBurstOnTimeIsSetEEPROMFlag()
{
  if (EEPROM.read(pumpBurstOnTimeIsSetEEPROMFlagAddress) != 99)
    EEPROM.write(pumpBurstOnTimeIsSetEEPROMFlagAddress, 99);
}


void setPumpBurstOffTime(char* msg)
{
  int length = strlen(msg);

  if (length >= 2)
  {
    long value = readInt(msg, 1, length-1);

//    Serial.println("Value:");
//    Serial.println(value);

    setPumpBurstOffTime(value);
  }
}

void setPumpBurstOffTime(long newPumpBurstOffTime)
{
  if (isDebugMode)
  {
    Serial.print("Setting pump burst off time to EEPROM: ");
    Serial.println(newPumpBurstOffTime);
  }

  EEPROMWriteLong(pumpBurstOffTimeEEPROMAddress, newPumpBurstOffTime);

  setPumpBurstOffTimeIsSetEEPROMFlag();

  pumpBurstOffTime = newPumpBurstOffTime;
}

long getPumpBurstOffTime()
{

  if (EEPROM.read(pumpBurstOffTimeIsSetEEPROMFlagAddress) == 99)
  {
    long value = EEPROMReadLong(pumpBurstOffTimeEEPROMAddress);
    
    if (value < 0 || value > 9999)
    {
      value = pumpBurstOffTime;
    }

    Serial.println("Value:");
    Serial.println(value);

    return value;
  }
  else
    return pumpBurstOffTime;
}

void setPumpBurstOffTimeIsSetEEPROMFlag()
{
  if (EEPROM.read(pumpBurstOffTimeIsSetEEPROMFlagAddress) != 99)
    EEPROM.write(pumpBurstOffTimeIsSetEEPROMFlagAddress, 99);
}

/* Restore defaults */
void restoreDefaultIrrigationSettings()
{
  Serial.println("Resetting default irrigation settings");

  restoreDefaultThreshold();
  restoreDefaultPumpBurstOnTime();
}

void restoreDefaultThreshold()
{
  Serial.println("Resetting default threshold");

  removeThresholdEEPROMIsSetFlag();

  threshold = 30;

  setThreshold(threshold);
}

void restoreDefaultPumpBurstOnTime()
{
  Serial.println("resetting default pump burst on time");

  removePumpBurstOnTimeEEPROMIsSetFlag();

  pumpBurstOnTime = 3;

  setPumpBurstOnTime(pumpBurstOnTime);
}

void restoreDefaultPumpBurstOffTime()
{
  Serial.println("Resetting default pump burst off time");

  removePumpBurstOffTimeEEPROMIsSetFlag();

  pumpBurstOffTime = 5;

  setPumpBurstOffTime(pumpBurstOffTime);
}

void removeThresholdEEPROMIsSetFlag()
{
    EEPROM.write(thresholdIsSetEEPROMFlagAddress, 0);
}

void removePumpBurstOnTimeEEPROMIsSetFlag()
{
    EEPROM.write(pumpBurstOnTimeIsSetEEPROMFlagAddress, 0);
}
