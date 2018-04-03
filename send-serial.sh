VALUE=$1

PORT_NAME=$2

if [ ! $PORT_NAME ]; then
  PORT_NAME="/dev/ttyUSB0"
fi

echo "Port: $PORT_NAME"

echo "$VALUE" > $PORT_NAME
