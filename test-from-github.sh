#!/bin/bash

echo ""
echo "Testing SoilMoistureSensorCalibratedPump project from github"
echo ""

BRANCH=$1

if [ $# -eq 0 ]
then
  BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')
  echo "Branch not specified as argument. Using current branch: $BRANCH"
fi

TIMESTAMP=$(date +"%Y_%m_%d_%I_%M_%p")
TEMPORARY_DIR="/home/j/tmp/$TIMESTAMP"

echo "Branch: $BRANCH"
echo "Tmp project dir:"
echo "  $TEMPORARY_DIR"

mkdir -p $TEMPORARY_DIR

cd $TEMPORARY_DIR

git clone http://github.com/GreenSense/SoilMoistureSensorCalibratedPump -b $BRANCH && \

cd SoilMoistureSensorCalibratedPump && \
sh init.sh && \
sh build.sh && \
sh upload.sh && \
sh upload-simulator.sh && \
sh test.sh

rm $TEMPORARY_DIR -rf

echo ""
echo "Finished testing SoilMoistureSensorCalibratedSerial project from github"
echo ""
