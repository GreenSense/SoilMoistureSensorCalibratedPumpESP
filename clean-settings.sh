
FULL_VERSION="$VERSION-$BUILD_NUMBER"

SOURCE_FILE="src/SoilMoistureSensorCalibratedPumpESP/SoilMoistureSensorCalibratedPumpESP.ino"

sed -i "s/#define WIFI_NAME .*/#define WIFI_NAME \"accesspoint\"/" $SOURCE_FILE
sed -i "s/#define WIFI_PASSWORD .*/#define WIFI_PASSWORD \"password\"/" $SOURCE_FILE
sed -i "s/#define MQTT_HOST .*/#define MQTT_HOST \"garden\"/" $SOURCE_FILE
sed -i "s/#define MQTT_USERNAME .*/#define MQTT_USERNAME \"username\"/" $SOURCE_FILE
sed -i "s/#define MQTT_PASSWORD .*/#define MQTT_PASSWORD \"password\"/" $SOURCE_FILE
sed -i "s/#define MQTT_DEVICE_NAME .*/#define MQTT_DEVICE_NAME \"WiFiIrrigator1\"/" $SOURCE_FILE

sed -i "s/#define VERSION .*/#define VERSION \"1-0-0-0\"/" $SOURCE_FILE
