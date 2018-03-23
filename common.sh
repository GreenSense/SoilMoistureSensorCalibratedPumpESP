GARDEN_HOSTNAME=garden
GARDEN_USER=j

IRRIGATOR_PORT="/dev/ttyUSB0"
SIMULATOR_PORT="/dev/ttyUSB1"

# If multiple devices are detected then this becomes the second device pair
if pio device list | grep -q 'ttyUSB1'; then
  IRRIGATOR_PORT="/dev/ttyUSB1"
fi

# If multiple devices pairs are detected then this becomes the second device pair
if pio device list | grep -q 'ttyUSB2'; then
  IRRIGATOR_PORT="/dev/ttyUSB2"
  SIMULATOR_PORT="/dev/ttyUSB3"
fi

