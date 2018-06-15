echo "Starting build for project"
echo "Dir: $PWD"

DIR=$PWD

xbuild src/SoilMoistureSensorCalibratedPumpESP.sln /p:Configuration=Release
