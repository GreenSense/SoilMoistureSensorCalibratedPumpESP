. ./common.sh

DESTINATION_PROJECT_PATH="~/projects/tmp/greensense/SoilMoistureSensorPump1602LCD"

sh push-to-garden.sh $DESTINATION_PROJECT_PATH && \

ssh $GARDEN_USER@$GARDEN_HOSTNAME "cd $DESTINATION_PROJECT_PATH && sh upload.sh"

echo "Push and upload completed."
