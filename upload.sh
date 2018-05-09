PORT_NAME=$1

if [ ! $PORT_NAME ]; then
  PORT_NAME=$IRRIGATOR_ESP_PORT
fi

if [ ! $PORT_NAME ]; then
  PORT_NAME="/dev/ttyUSB0"
fi

echo "Port: $PORT_NAME"

pio run --target upload --environment=esp12e --upload-port=$PORT_NAME
