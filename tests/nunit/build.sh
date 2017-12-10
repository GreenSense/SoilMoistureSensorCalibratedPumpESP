echo "Starting build for project"
echo "Dir: $PWD"

DIR=$PWD

xbuild src/SoilMoistureSensorCalibratedPump.sln /p:Configuration=Release
