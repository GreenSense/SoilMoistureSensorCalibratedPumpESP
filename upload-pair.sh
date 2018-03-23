#!/bin/bash

. ./common.sh

echo "Uploading to port $IRRIGATOR_PORT"

# pio run --target upload --environment=nanoatmega328 --upload-port=/dev/ttyUSB0
pio run --target upload --environment=nanoatmega328 --upload-port=$IRRIGATOR_PORT

sh upload-simulator.sh
