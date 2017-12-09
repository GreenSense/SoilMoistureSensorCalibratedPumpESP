#!/bin/bash

. ./common.sh

# pio run --target upload --environment=nanoatmega328 --upload-port=/dev/ttyUSB0
pio run --target upload --environment=nanoatmega328 --upload-port=$IRRIGATOR_PORT
