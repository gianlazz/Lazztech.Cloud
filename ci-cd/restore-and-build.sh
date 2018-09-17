#! /bin/bash

echo "Removing CLI breaking .dcproj from solution."
dotnet sln ../Lazztech.ObsidianPresences.sln remove ../docker-compose.dcproj
echo "Running dotnet restore."
dotnet restore ../Lazztech.ObsidianPresences.sln
echo "Running dotnet build."
dotnet build ../Lazztech.ObsidianPresences.sln
#echo "Re-adding CLI breaking .dcproj to the solution."
#dotnet sln ../Lazztech.ObsidianPresences.sln add ../docker-compose.dcproj

#dotnet publish -c release
#docker build -t dockerhubuser/simplecoreapp:v0.${BUILD_NUMBER} .
#docker login -u dockerhubuser -p dockerhubpassword -e user@domain.com
#docker push dockerhubuser/simplecoreapp:v0.${BUILD_NUMBER}dotnet restore
#dotnet publish -c release
#docker build -t dockerhubuser/simplecoreapp:v0.${BUILD_NUMBER} .
#docker login -u dockerhubuser -p dockerhubpassword -e user@domain.com
#docker push dockerhubuser/simplecoreapp:v0.${BUILD_NUMBER}
