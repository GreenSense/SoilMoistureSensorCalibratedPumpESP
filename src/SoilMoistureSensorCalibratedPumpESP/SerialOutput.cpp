#include <Arduino.h>
#include <EEPROM.h>

#include "Common.h"
#include "SoilMoistureSensor.h"
#include "Irrigation.h"

void serialPrintDeviceInfo()
{
  Serial.println("");
  Serial.println("-- Start Device Info");
  Serial.println("Family: GreenSense");
  Serial.println("Group: irrigator");
  Serial.println("Project: SoilMoistureSensorCalibratedPumpESP");
  Serial.print("Board: ");
  Serial.println(BOARD_TYPE);
  Serial.print("Version: ");
  Serial.println(VERSION);
  Serial.println("ScriptCode: irrigator");
  Serial.println("-- End Device Info");
  Serial.println("");
}

void serialPrintData()
{
  bool isTimeToPrintData = millis() - lastSerialOutputTime >= secondsToMilliseconds(serialOutputIntervalInSeconds)
      || lastSerialOutputTime == 0;

  bool isReadyToPrintData = isTimeToPrintData && soilMoistureSensorReadingHasBeenTaken;

  if (isReadyToPrintData)
  {
    lastSerialOutputTime = millis();

    if (isDebugMode)
    {
      Serial.println("Printing serial data");
    }

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
    Serial.print("I:");
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
    Serial.print("V:");
    Serial.print(VERSION);
    Serial.print(";;");
    Serial.println();

    if (isDebugMode)
    {
      Serial.print("Last pump start time:");
      Serial.println(pumpStartTime);
      Serial.print("Last pump finish time:");
      Serial.println(lastPumpFinishTime);
    }

  }
}

void forceSerialOutput()
{
    // Reset the last serial output time 
    lastSerialOutputTime = 0;//millis()-secondsToMilliseconds(serialOutputIntervalInSeconds);
}
