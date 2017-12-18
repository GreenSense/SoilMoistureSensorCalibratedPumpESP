#!/bin/bash

echo ""
echo "Testing SoilMoistureSensorCalibratedPump project from github"
echo ""

TIMESTAMP=$(date +"%Y_%m_%d_%I_%M_%p")
TEMPORARY_DIR="/tmp/$TIMESTAMP"
echo "Tmp project dir:"
echo "  $TEMPORARY_DIR"

mkdir -p $TEMPORARY_DIR

cd $TEMPORARY_DIR

git clone http://github.com/GreenSense/SoilMoistureSensorCalibratedPump && \

cd SoilMoistureSensorCalibratedPump && \
sh init.sh && \
sh build.sh && \
sh upload.sh && \
sh upload-simulator.sh && \
sh test.sh

rm $TEMPORARY_DIR -rf

echo ""
echo "Finised testing SoilMoistureSensorCalibratedSerial project from github"
echo ""
