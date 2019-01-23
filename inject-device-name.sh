DEVICE_NAME="$1"

if [ ! "$DEVICE_NAME" ]; then
  echo "Specify a device name as an argument."
  exit 1
fi

SOURCE_FILE="src/SoilMoistureSensorCalibratedPumpESP/SoilMoistureSensorCalibratedPumpESP.ino"

# Inject the device name into the sketch
sed -i "s/#define MQTT_DEVICE_NAME .*/#define MQTT_DEVICE_NAME \"$DEVICE_NAME\"/" $SOURCE_FILE

# Create the device name security file needed for tests to run
echo $DEVICE_NAME > mqtt-device-name.security
