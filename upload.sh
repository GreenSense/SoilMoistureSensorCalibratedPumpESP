PORT_NAME=$1

if [ ! $PORT_NAME ]; then
  PORT_NAME=$IRRIGATOR_ESP_PORT
fi

if [ ! $PORT_NAME ]; then
  echo "Provide an upload port as an argument."
  exit 1
fi

echo "Port: $PORT_NAME"

pio run -s --target upload --environment=esp12e --upload-port=$PORT_NAME
