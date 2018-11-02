echo "Getting library files..."
echo "  Dir: $PWD"


# Nuget is disabled because it's not currently required
#sh get-nuget.sh
#sh nuget-update-self.sh

sh nuget-install.sh NUnit 2.6.4
sh nuget-install.sh NUnit.Runners 2.6.4
sh nuget-install.sh Newtonsoft.Json 11.0.2
sh nuget-install.sh ArduinoSerialControllerClient 1.0.9
sh nuget-install.sh duinocom.core 1.0.6
sh nuget-install.sh M2Mqtt 4.3.0.0

echo "Finished getting library files."
