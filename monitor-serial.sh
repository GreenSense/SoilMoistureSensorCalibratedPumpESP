#!/bin/bash

. ./common.sh

echo "Uploading to port $IRRIGATOR_PORT"

pio device monitor --port=$IRRIGATOR_PORT
