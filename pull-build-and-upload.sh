#!/bin/bash
git checkout master && \
git pull origin master && \
sh inject-version.sh && \
sh build-and-upload.sh && \
git checkout src/SoilMoistureSensorCalibratedPump/SoilMoistureSensorCalibratedPump.ino
