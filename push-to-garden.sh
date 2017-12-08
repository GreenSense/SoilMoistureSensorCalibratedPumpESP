#!/bin/bash

. ./common.sh

DESTINATION_PROJECT_PATH=$1

if [ -z "$DESTINATION_PROJECT_PATH" ]; then
  echo "Destination folder not specified as an argument."
  echo "  Using default..."
  DESTINATION_PROJECT_PATH="~/projects/greensense/SoilMoistureSensorPump1602LCD"
  echo "    $DESTINATION_PROJECT_PATH"
fi

#if [ ! -d "$DESTINATION_PROJECT_PATH" ]; then
#  echo "Destination folder not found."
#  echo "  Creating..."
#  echo "  $DESTINATION_PROJECT_PATH"
#  mkdir -p "$DESTINATION_PROJECT_PATH"
#fi

echo "Pushing to..."
echo "  Host: $GARDEN_HOSTNAME"
echo "  Destination folder: $DESTINATION_PROJECT_PATH"

rsync -avzP . $GARDEN_USER@$GARDEN_HOSTNAME:$DESTINATION_PROJECT_PATH --rsync-path="mkdir -p $DESTINATION_PROJECT_PATH && rsync"

echo "Push completed."
