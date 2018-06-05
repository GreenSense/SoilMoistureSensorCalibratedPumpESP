echo "Creating security files from environment variables"

if [ ! $WIFI_NAME ]; then
  echo "WIFI_NAME environment variable not found"
  exit 1
fi

if [ ! $WIFI_PASSWORD ]; then
  echo "WIFI_PASSWORD environment variable not found"
  exit 1
fi

if [ ! $MQTT_HOST ]; then
  echo "MQTT_HOST environment variable not found"
  exit 1
fi

if [ ! $MQTT_USERNAME ]; then
  echo "MQTT_USERNAME environment variable not found"
  exit 1
fi

if [ ! $MQTT_PASSWORD ]; then
  echo "MQTT_PASSWORD environment variable not found"
  exit 1
fi

if [ ! $MQTT_PORT ]; then
  echo "MQTT_PORT environment variable not found"
  exit 1
fi

echo "WiFi Access Point: $WIFI_NAME"

echo $WIFI_NAME > "wifi-name.security"
echo $WIFI_PASSWORD > "wifi-password.security"

echo "MQTT Host: $MQTT_HOST"
echo "MQTT Username: $MQTT_USERNAME"
echo "MQTT Port: $MQTT_PORT"

echo $MQTT_HOST > "mqtt-host.security"
echo $MQTT_USERNAME > "mqtt-username.security"
echo $MQTT_PASSWORD > "mqtt-password.security"
echo $MQTT_PORT > "mqtt-port.security"
