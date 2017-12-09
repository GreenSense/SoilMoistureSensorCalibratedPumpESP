echo "Starting build for project"
echo "Dir: $PWD"

DIR=$PWD

msbuild src/SoilMoistureSensorCalibratedPump.sln /p:Configuration=Release
