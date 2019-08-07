PORT_NAME=$1

echo "Uploading ESP irrigator..."

if [ ! $PORT_NAME ]; then
  PORT_NAME=$IRRIGATOR_ESP_PORT
fi

if [ ! $PORT_NAME ]; then
  echo "Provide an upload port as an argument."
  exit 1
fi

echo "  Port: $PORT_NAME"

pio run --target upload --environment=esp12e --upload-port=$PORT_NAME || exit 1

echo ""
echo ""
echo "Upload complete"

