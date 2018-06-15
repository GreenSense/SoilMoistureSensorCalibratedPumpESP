#!/bin/bash
PORT_NAME=$1

if [ ! $PORT_NAME ]; then
  echo "Provide a port name as an argument."
  exit 1
fi

echo "Port: $PORT_NAME"

pio device monitor --baud=115200 --port=$PORT_NAME
